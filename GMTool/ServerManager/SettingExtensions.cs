using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GMTool.Common;
using GMTool.Helper;

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
            string connStr = config.ConnectionString;
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
                SetValue(node, "" + config.GameCode);
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
