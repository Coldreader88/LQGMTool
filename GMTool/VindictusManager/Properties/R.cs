
using System;
using System.Collections.Generic;
using System.IO;

namespace Vindictus
{
	public static class R
	{
		private static Dictionary<string,string> Texts=new Dictionary<string,string>();
		static R()
		{
			string[] lines=File.ReadAllLines(Program.Config.Language);
			foreach(string line in lines){
				string[] ws = line.Split('=');
				Texts.Add(ws[0].ToLower(), ws[1].Trim().Replace("\\t", "\t").Replace("\\n","\n"));
			}
		}
		private static string Get(string name){
			return Texts[name.ToLower()];
		}
		public static string Title{ get{ return Get("Title"); } }
		public static string Start{ get{ return Get("start"); } }
		public static string Hide{ 	get{ return Get("Hide"); 	} 	}
		public static string Stop{ 	get{ return Get("Stop"); 	} }
		public static string Show{ 	get{ 	return Get("Show"); } }
		public static string OK{ get{ return Get("OK"); 	} 	}
		public static string Info{ 	get{ return Get("Info"); } }
		public static string Warnning{ get{ return Get("warnning"); } }
		public static string Error{ get{ return Get("error"); } }
		public static string Question{ 	get{ return Get("Question"); } }
		public static string View{ 	get{ return Get("View"); } }
		public static string ServerManager{ 	get{ return Get("ServerManager"); } }
		public static string Help{ 	get{ return Get("Help"); } }
		public static string About{ 	get{ return Get("About"); } }
		public static string AboutText{get{ return Get("AboutText");}}
		public static string AllSalon{ 	get{ return Get("allSalon"); } }
		public static string SalonPirce{ 	get{ return Get("SalonPirce"); } }
		public static string Tool{ 	get{ return Get("Tool"); } }
		//tipStopTask
		public static string TipStopTask{ 	get{ return Get("TipStopTask"); } }
		//tipFileNotExist
		public static string TipFileNotExist{ 	get{ return Get("tipFileNotExist"); } }
		public static string TipDealClientDb{ 	get{ return Get("TipDealClientDb"); } }
		public static string TipDealServerDb{ 	get{ return Get("tipDealServerDb"); } }
		//tipDealDb
		public static string TipDealDb{ 	get{ return Get("tipDealDb"); } }
		//tipTaskCompleted
		public static string TipTaskCompleted{ 	get{ return Get("tipTaskCompleted"); } }
		//errorGameCode
		public static string ErrorGameCode{ 	get{ return Get("errorGameCode"); } }
		//zhtw2zhcn
		public static string zhTw2zhCn{ 	get{ return Get("zhtw2zhcn"); } }
		public static string SelectZhTWPath{ 	get{ return Get("selectZhTWPath"); } }
		public static string EnEu2zhcn{ 	get{ return Get("enEu2zhcn"); } }
		public static string SelectEnEuPath{ 	get{ return Get("selectEnEuPath"); } }
		//tipcloseserver
		public static string TipCloseServer{ 	get{ return Get("tipcloseserver"); } }
	}
}
