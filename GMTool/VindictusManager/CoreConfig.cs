using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Vindictus
{
    [XmlType("CoreConfig")]
    public class CoreConfig
    {
        public string ConnectionString;
        public string DatabasePath;
        public string GamePath;
        public string GameCode;
        public string ProviderName;
        public string ServerPath;
        public string SqlServer;
		public int WebPort;
		public string WebRoot;
		public string Language;
        [XmlArray("DataBases")]
        [XmlArrayItem("item")]
        public List<string> DataBases;

        [XmlArray("Apps")]
        [XmlArrayItem("App")]
        public List<App> Apps;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ConnectionString="); sb.Append(ConnectionString);
            sb.Append("\nDatabasePath="); sb.Append(DatabasePath);
            sb.Append("\nGamePath="); sb.Append(GamePath);
            sb.Append("\nGameCode="); sb.Append(GameCode);
            if (DataBases != null)
            {
                sb.Append("\nDataBase:");
                foreach (string db in DataBases)
                {
                    sb.Append(db);
                    sb.Append(",");
                }
            }
             sb.Append("\n");
            if (Apps != null)
            {
                foreach (App app in Apps) {
                    sb.Append(app.ToString());
                }
            }
            return sb.ToString();
        }
    }
    [XmlType("StubApp")]
    public class StubApp
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("disable")]
        public bool Disable;
        [XmlAttribute("path")]
        public string Path;
        [XmlAttribute("args")]
        public string Args;
        public override string ToString()
        {
            return Name+":" + Path + ",Args = " + Args;
        }
    }
    [XmlType("App")]
    public class App : StubApp
    {
        [XmlArray("StubApps")]
        [XmlArrayItem("StubApp")]
        public List<StubApp> Apps;
        public bool HasStubApp
        {
            get
            { return Apps != null && Apps.Count > 0; }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            sb.Append(":");
            sb.Append(Path);
            sb.Append("\n");
            if (Apps != null)
            {
                foreach (StubApp arg in Apps)
                {
                    sb.Append(arg);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
