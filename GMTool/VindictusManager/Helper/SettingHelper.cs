using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Xml;
using System.Windows.Forms;
using Vindictus.Helper;

namespace Vindictus.Helper
{
	public static class SettingHelper
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
		public static bool CheckGameCode(CoreConfig config, out string code){
			string gamecode  =config.GameCode;
			string cdb3= PathHelper.Combine(config.GamePath, "sql/heroes.db3");
			string srcdb3=PathHelper.Combine(config.BinPath, "heroesContents.db3");
			int i;
			string db3 = cdb3;
			code = gamecode;
			
			//查sql
			if(!File.Exists(db3)){
				db3 = srcdb3;
				if(!File.Exists(db3)){
					return false;
				}
			}
			using(SQLiteHelper db = new SQLiteHelper(db3)){
				if(!db.Open()){
					return false;
				}
				if(!int.TryParse(gamecode, out i)){
					using(DbDataReader reader= db.GetReader("select \""+gamecode+"\" from featurematrix where feature='GameCode';")){
						if(reader!=null&&reader.HasRows && reader.Read()){
							string _code = reader.GetString(0);
							if(gamecode == _code){
								return false;
							}
							config.GameCode = _code;
							code = _code;
//						System.Windows.Forms.MessageBox.Show(gamecode);
//						return -3;
						}else{
							return false;
						}
					}
				}else{
					using(DbDataReader reader= db.GetReader("select * from featurematrix where feature='GameCode';")){
						if(reader!=null&&reader.HasRows && reader.Read()){
							int c = reader.FieldCount;
							for(i=0;i<c;i++){
								string _code = reader.GetString(i);
								if(_code == gamecode){
									string val = reader.GetName(i);
									string[] vs = val.Split('-');
									if(vs.Length>1){
										config.GameCode=vs[0].ToLower()+"-"+vs[1].ToUpper();
									}else{
										config.GameCode=vs[0].ToLower();
									}
									code = _code;
									return true;
								}
							}
							return false;
						}else{
							return false;
						}
					}
				}
			}
			
			return true;
		}
		public static int UpdateConfig(this Form main, CoreConfig config, string configname = "ServiceCore.dll.config")
		{
			string xmlfile = PathHelper.Combine(config.BinPath, configname);
			if(!File.Exists(xmlfile)){
				//	return -5;
			}
			string connStr = config.ConnectionString;
			string gamecode;
			if(!CheckGameCode(config,out gamecode)){
				return -4;
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
