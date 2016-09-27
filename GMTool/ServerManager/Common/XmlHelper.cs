/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/27
 * 时间: 16:31
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using GMTool.Helper;

namespace ServerManager.Comon
{
	/// <summary>
	/// Description of XmlHelper.
	/// </summary>
	public class XmlHelper
	{
		private string xmlfile;
		public XmlHelper(string xmlfile)
		{
			this.xmlfile=xmlfile;
		}
		public void ModConnectStrings(string server="127.0.0.1,1433",string user=null,string pwd=null)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(xmlfile);
			XmlNodeList list=  doc.SelectNodes("//configuration//connectionStrings//add");
			if(list!=null){
				foreach(XmlNode node in list){
					Console.WriteLine(node.Attributes["name"].Value);
					//Console.WriteLine(node.Attributes["connectionString"].Value);
					string dbname = GetDbName(node.Attributes["connectionString"].Value);
					string connectionString = MSSqlHelper.MakeConnectString(server, user, pwd, dbname);
					node.Attributes["connectionString"].Value = connectionString;
					Console.WriteLine(connectionString);
				}
			}
			doc.Save(xmlfile);
		}
		
		private string GetDbName(string connectionStr){
			//Initial Catalog=
			string head = "Initial Catalog=";
			int i = connectionStr.IndexOf(head);
			if(i>=0){
				int j = connectionStr.IndexOf(";", i+head.Length);
				if(j>=0){
					return connectionStr.Substring(i+head.Length, j-(i+head.Length));
				}else{
					return connectionStr.Substring(i+head.Length);
				}
			}
			return "";
		}
	}
}
