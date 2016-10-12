﻿using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace ServerManager.Common
{

    public class HttpProcessor
    {
        public TcpClient socket;
        public HttpServer srv;

        private StreamReader inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            this.socket = s;
            this.srv = srv;
        }

        public void process(object obj = null)
        {
            // bs = new BufferedStream(s.GetStream());
            try
            {
                inputStream = new StreamReader(socket.GetStream());
                outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
                parseRequest();
                readHeaders();
                if (http_method == null)
                {
                    return;
                }
                if (http_method.ToLower() == "get")
                {
                    handleGETRequest();
                }
                else if (http_method.ToLower() == "post")
                {
                    handlePOSTRequest();
                }
                else
                {
                    handleGETRequest();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
            finally
            {
                try
                {
                    outputStream.Flush();
                    socket.Close();
                }
                catch { }
                // bs.Flush(); // flush any remaining output
                inputStream = null; outputStream = null; // bs = null;
            }
        }

        public void parseRequest()
        {
            String request = inputStream.ReadLine();
            if (request == null)
            {
                return;
            }
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            //.WriteLine("starting: " + request);
        }

        public void readHeaders()
        {
            //	Console.WriteLine("readHeaders()");
            String line;
            while ((line = inputStream.ReadLine()) != null)
            {
                if (line.Equals(""))
                {
                    //Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                //Console.WriteLine("header: {0}:{1}",name,value);
                httpHeaders[name] = value;
            }
        }

        public void handleGETRequest()
        {
            srv.handleGETRequest(this);
        }

        public void handlePOSTRequest()
        {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream
            // we hand him needs to let him see the "end of the stream" at this content
            // length, because otherwise he won't know when he's seen it all!

            //Console.WriteLine("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                                      content_len));
                }
                byte[] buf = new byte[4096];
                int to_read = content_len;
                while (to_read > 0)
                {
                    int numread = this.inputStream.BaseStream.Read(buf, 0, Math.Min(4096, to_read));
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            //Console.WriteLine("get post data end");
            srv.handlePOSTRequest(this, new StreamReader(ms));
        }

        public void writeSuccess()
        {
            try
            {
                outputStream.Write("HTTP/1.0 200 OK\r\n");
                outputStream.Write("Content-Type: text/html;charset=utf-8\r\n");
                outputStream.Write("Access-Control-Allow-Origin: * \r\n");
                outputStream.Write("Access-Control-Allow-Methods: * \r\n");
                outputStream.Write("Access-Control-Allow-Headers: x-requested-with,content-type \r\n");
                outputStream.Write("Connection: close\r\n");
                outputStream.Write("\r\n");
            }
            catch { }
        }

        public void writeFailure()
        {
            if (outputStream == null)
            {
                return;
            }
            try
            {
                outputStream.Write("HTTP/1.0 404 File not found\r\n");
                outputStream.Write("Connection: close\r\n");
                outputStream.Write("\r\n");
            }
            catch { }
        }
    }

    public abstract class HttpServer
    {
        protected int port;
        TcpListener listener;
        protected bool is_active = true;

        public HttpServer(int port)
        {
            this.port = port;
        }

        public void listen()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(s, this);
                ThreadPool.QueueUserWorkItem(new WaitCallback(processor.process));
                //				Thread thread = new Thread(new ThreadStart(processor.process));
                //				thread.IsBackground=true;
                //				thread.Start();
                Thread.Sleep(1);
            }
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }
}
