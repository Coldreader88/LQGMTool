using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GMTool.Common;
using System.Data.Common;
using GMTool.Helper;
using System.IO;

namespace ServerManager
{
	public static class SettingExtensions
	{
		private static string GetDbName(string connectionStr)
		{
			//Initial Catalog=
			string head = "Initial Catalog=";
			int i = connectionStr.IndexOf(head);
			if (i >= 0)
			{
				int j = connectionStr.IndexOf(";", i + head.Length);
				if (j >= 0)
				{
					return connectionStr.Substring(i + head.Length, j - (i + head.Length));
				}
				else
				{
					return connectionStr.Substring(i + head.Length);
				}
			}
			return "";
		}
		private static string GetConnectionString(string connstr, string db)
		{
			if (!string.IsNullOrEmpty(connstr))
			{
				if (!connstr.EndsWith(";"))
				{
					connstr += ";";
				}
			}
			return connstr + "Initial Catalog=" + db;
		}

		public static int UpdateConfig(this MainForm main, CoreConfig config, string configname = "ServiceCore.dll.config")
		{
			string xmlfile = PathHelper.Combine(config.BinPath, configname);
			if(!File.Exists(xmlfile)){
			//	return -5;
			}
			string connStr = config.ConnectionString;
			string gamecode = config.GameCode;
			int i;
			if(!int.TryParse(gamecode, out i)){
				//查sql
				string db3 = PathHelper.Combine(config.GamePath, "sql/heroes.db3");
				if(!File.Exists(db3)){
					db3 = PathHelper.Combine(config.BinPath, "heroesContents.db3");
					if(!File.Exists(db3)){
						return -3;
					}
				}
				SQLiteHelper db = new SQLiteHelper(db3);
				if(!db.Open()){
					return -3;
				}
				using(DbDataReader reader= db.GetReader("select \""+gamecode+"\" from featurematrix where feature='GameCode';")){
					if(reader!=null&&reader.HasRows && reader.Read()){
						string _code = reader.GetString(0);
						if(gamecode == _code){
							return -4;
						}
//						System.Windows.Forms.MessageBox.Show(gamecode);
//						return -3;
					}else{
						return -4;
					}
				}
			}
			XmlDocument doc = new XmlDocument();
			doc.Load(xmlfile);
			XmlNodeList list = doc.SelectNodes("//configuration//connectionStrings//add");
			if (list != null)
			{
				foreach (XmlNode _node in list)
				{
					Console.WriteLine(_node.Attributes["name"].Value);
					//Console.WriteLine(node.Attributes["connectionString"].Value);
					string dbname = GetDbName(_node.Attributes["connectionString"].Value);
					//Initial Catalog=;
					string connectionString = GetConnectionString(connStr, dbname);
					_node.Attributes["connectionString"].Value = connectionString;
					Console.WriteLine(connectionString);
				}
			}
			else
			{
				return -1;
			}
			XmlNode node = doc.SelectSingleNode("//configuration//applicationSettings//UnifiedNetwork.Properties.Settings"+
			                                    "//setting[@name='GameCode']");
			if (node != null)
			{
				SetValue(node, "" + gamecode);
			}
			else
			{
				return -2;
			}
			XmlNodeList Properties = doc.SelectNodes("//configuration//applicationSettings//ServiceCore.Properties.Settings//setting");
			if (Properties != null)
			{
				foreach (XmlNode x in Properties)
				{
					XmlAttribute attr = x.Attributes["name"];
					if (attr == null) continue;
					if ("DSExecPath".Equals(attr.Value, StringComparison.OrdinalIgnoreCase))
					{
						SetValue(x, PathHelper.Combine(config.GamePath, "srcds.exe"));
					}
					else if ("VersionMatchUpToDateFile".Equals(attr.Value, StringComparison.OrdinalIgnoreCase)){
						SetValue(x, PathHelper.Combine(config.GamePath, "CurrentInfo.txt"));
					}
				}
			}
			else
			{
				return -3;
			}
			doc.Save(xmlfile);
			return 0;
		}
		private static void SetValue(XmlNode node, string value)
		{
			XmlNode val = node.SelectSingleNode("value");
			if (val != null)
			{
				val.InnerText = value;
			}
		}
	}
}
