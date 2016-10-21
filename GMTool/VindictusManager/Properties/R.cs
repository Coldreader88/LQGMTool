
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
				if(line.StartsWith("#")){
					continue;
				}
				string[] ws = line.Split('=');
				if(ws.Length>1){
					Texts.Add(ws[0].ToLower(), ws[1].Trim().Replace("\\t", "\t").Replace("\\n","\n"));
				}
			}
		}
		private static string Get(string name){
			string val;
			if(Texts.TryGetValue(name.ToLower(), out val)){
				return val;
			}
			return name;
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
		//tipServerDbSplit
		public static string ServerDbSplit{ 	get{ return Get("ServerDbSplit"); } }
		public static string TipServerDbSplitFail{ 	get{ return Get("TipServerDbSplitFail"); } }
		public static string ServerDbAttach{ 	get{ return Get("ServerDbAttach"); } }
		public static string TipServerDbAttachFail{ 	get{ return Get("TipServerDbAttachFail"); } }
		public static string ServerDbCreate{ 	get{ return Get("ServerDbCreate"); } }
		public static string TipServerDbCreateFail{ 	get{ return Get("TipServerDbCreateFail"); } }
		public static string StartAll{ 	get{ return Get("startall"); } }
		public static string StopAll{ 	get{ return Get("stopall"); } }
		public static string StartWeb{ 	get{ return Get("startWeb"); } }
		public static string StopWeb{ 	get{ return Get("stopWeb"); } }
		public static string StartSql{ 	get{ return Get("startSql"); } }
		public static string StopSql{ 	get{ return Get("stopSql"); } }
		//Daemon
		public static string Daemon{ 	get{ return Get("Daemon"); } }
		public static string UpdateServerSettings{ 	get{ return Get("UpdateServerSettings"); } }
		public static string ServerDbCompress{ 	get{ return Get("ServerDbCompress"); } }
		public static string ErrorSqlServerNotInstall{ 	get{ return Get("ErrorSqlServerNotInstall"); } }
		public static string ErrorSqlServerNotStart{ 	get{ return Get("ErrorSqlServerNotStart"); } }
		public static string ErrorSqlServerNotConnect{ 	get{ return Get("ErrorSqlServerNotConnect"); } }
		public static string TipServerStopSqlServer{ 	get{ return Get("TipServerStopSqlServer"); } }
		public static string TipServerSettingOk{ 	get{ return Get("TipServerSettingOk"); } }
		public static string TipServerSettingFail{ 	get{ return Get("TipServerSettingFail"); } }
		public static string TipBadGameCode{ 	get{ return Get("TipBadGameCode"); } }
		public static string ErrorNoFindSettings{ 	get{ return Get("ErrorNoFindSettings"); } }
		public static string ErrorSetGameCodeFail{ 	get{ return Get("ErrorSetGameCodeFail"); } }
		public static string TipSelectSqlBakPath{ 	get{ return Get("TipSelectSqlBakPath"); } }
		//TipServerCreateDbOK
		public static string TipServerCreateDbOK{ 	get{ return Get("TipServerCreateDbOK"); } }
		public static string ErrorServerCreateDbFail{ 	get{ return Get("ErrorServerCreateDbFail"); } }
		public static string TipServerCompressDbOK{ 	get{ return Get("TipServerCompressDbOK"); } }
		public static string ErrorServerCompressDbFail{ 	get{ return Get("ErrorServerCompressDbFail"); } }
		public static string ErrorProcessStop{ 	get{ return Get("ErrorProcessStop"); } }
		public static string TipServerAttachDbOK{ 	get{ return Get("TipServerAttachDbOK"); } }
		public static string ErrorServerAttachDbFail{ 	get{ return Get("ErrorServerAttachDbFail"); } }
		public static string TipServerClearSqlLog{ 	get{ return Get("TipServerClearSqlLog"); } }
		
		public static string TipServerSplitDbOK{ 	get{ return Get("TipServerSplitDbOK"); } }
		public static string ErrorServerSplitDbFail{ 	get{ return Get("ErrorServerSplitDbFail"); } }
	}
}
