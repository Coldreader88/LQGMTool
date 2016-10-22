using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace WebServer
{
	public class HttpWebServer
	{
		/// <summary>
		/// Simple web server dll, to be self hosted in a parent application
		/// Full source at https://github.com/happyt/webServer
		/// 
		/// Some pages for reference chat
		/// http://stackoverflow.com/questions/427326/httplistener-server-header-c
		/// http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx
		/// http://www.dreamincode.net/forums/topic/215467-c%23-httplistener-and-threading/
		/// Full source at https://github.com/happyt/webServer
		/// </summary>

		private HttpListener listener;
		public int portNumber;

		// file type of the processed pages
		public string appFileType = ".lua";
		
		public HttpWebServer(string rootPath="./www",int portNumber=8081){
			this.rootPath=rootPath;
			this.portNumber=portNumber;
		}
		
		// if no file found, search for this string
		// raise an event with the command string
		// just reply with the OK reply below
		public string appCommand = "cmd=";
		public string appCmdResponse = "<html>OK</html>";
		public string appErrorResponse = "<html>No application handler!</html>";
		public bool isListening{get;private set;}
		public string rootPath{get;private set;}
		private const string defaultPage = @"index.html";

		//===================================================================

		// declare a Delegate
		public delegate string ProcessMessage(string type, string file);
		// use a instance of the declared Delegate
		public ProcessMessage FileHandler;

		/// <summary>
		/// Creates the http server and opens listener
		/// </summary>
		public void Start()
		{
			if(isListening)return;
			isListening=true;
			listener = new HttpListener();
			listener.Prefixes.Add("http://*:" + portNumber + "/");
			listener.Start();
			listener.BeginGetContext(ProcessRequest, listener);
		}

		/// <summary>
		/// Stops listening
		/// </summary>
		public void Stop()
		{
			if(!isListening || listener==null)return;
			isListening=false;
			listener.Stop();
		}

		string GetContentType(string filetype){
			string responseType;
			switch (filetype)
			{
				case (".png"):
				case (".jpg"):
				case (".jpeg"):
				case (".gif"):
				case (".tiff"):
					responseType = "image/" + filetype.Substring(1);    // leave off the decimal point
					break;
				case (".js"):
					responseType = "application/javascript";
					break;

				case (".ico"):
					responseType = "image/x-icon";
					break;
				case (".txt"):
					responseType = "text/" + filetype.Substring(1);    // leave off the decimal point
					break;
				case (".xml"):
				case (".css"):
					responseType = "text/" + filetype.Substring(1);    // leave off the decimal point
					break;

				case (".json"):
					responseType = "application/json";
					break;

				case (".cachemanifest"):
					responseType = " text/cache-manifest";
					break;

				case (".htm"):
				case (".html"):
				case (".htmls"):
				default:
					responseType = "text/html";
					break;
			}
			return responseType;
		}
		/// <summary>
		/// Process the web request
		/// </summary>
		/// <param name="result"></param>
		void ProcessRequest(IAsyncResult result)
		{
			if(!isListening)return;
			var listener = (HttpListener)result.AsyncState;
			try{
				byte[] responseContent;
				HttpListenerContext context = listener.EndGetContext(result);
				HttpListenerRequest request = context.Request;
				string path = rootPath + request.RawUrl;
				string webMethod = request.HttpMethod;
				if (path == rootPath + "/"){
					path += defaultPage;
				}
				//
				string filetype = Path.GetExtension(path);
				filetype = StripParameters(filetype);
				string contentType = GetContentType(filetype);
				HttpListenerResponse response = context.Response;
				response.StatusCode = 200;
				if(FileHandler!=null){
					string str = FileHandler(filetype, path);
					if(str == null){
						str = "";
					}
					responseContent = Encoding.UTF8.GetBytes(str);
				}else{
					if(File.Exists(path)){
						var fInfo = new FileInfo(path);
						long numBytes = fInfo.Length;
						using(var fStream = new FileStream(path, FileMode.Open, FileAccess.Read)){
							using(var binaryReader = new BinaryReader(fStream)){
								responseContent = binaryReader.ReadBytes((int)numBytes);
								binaryReader.Close();
							}
						}
					}else{
						response.StatusCode = 404;
						responseContent = new byte[0];
					}
				}
				response.ContentType = contentType;
				response.ContentLength64 = responseContent.Length;
				using(Stream output = response.OutputStream){
					output.Write(responseContent, 0, responseContent.Length);
				}
			}catch(Exception e){
			//	MessageBox.Show("web\n"+e);
			}
			listener.BeginGetContext(ProcessRequest, listener);
		}

		/// <summary>
		/// Need to take off any string after a question mark character
		/// </summary>
		/// <param name="filetype"></param>
		/// <returns></returns>
		private string StripParameters(string filetype)
		{
			string stripped = filetype;
			if (stripped.IndexOf('?') != -1)
			{
				int n = stripped.IndexOf('?');
				stripped = stripped.Substring(0, n);
			}
			return stripped;
		}
	}
}
