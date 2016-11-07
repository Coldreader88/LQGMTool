
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
		public static string MailTitle{ 	get{ return Get("MailTitle"); } }
		public static string UserName{ 	get{ return Get("UserName"); } }
		public static string UserClass{ 	get{ return Get("UserClass"); } }
		public static string UserLevel{ 	get{ return Get("UserLevel"); } }
		public static string UserList{ 	get{ return Get("UserList"); } }
		public static string ItemDetail{ 	get{ return Get("ItemDetail"); } }
		public static string MailSending{ 	get{ return Get("MailSending"); } }
		public static string MailReceiver{ 	get{ return Get("MailReceiver"); } }
		public static string TipInit{ 	get{ return Get("TipInit"); } }
		public static string ConnectSqlServer{ 	get{ return Get("ConnectSqlServer"); } }
		public static string TipReadText{ 	get{ return Get("TipReadText"); } }
		public static string TipReadItem{ 	get{ return Get("TipReadItem"); } }
		public static string Classes{get{ return Get("Classes"); }}
		public static string GroupLight{get{ return Get("GroupLight"); }}
		public static string GroupDark{get{ return Get("GroupDark"); }}
		public static string UnKnown{get{ return Get("UnKnown"); }}
		public static string UserGroup{get{ return Get("UserGroup"); }}
		public static string MainCategory{get{ return Get("MainCategory"); }}
		public static string SubCategory{get{ return Get("SubCategory"); }}
		
		public static string ItemName{get{ return Get("ItemName"); }}
		public static string ItemRequireLevel{get{ return Get("ItemRequireLevel"); }}
		public static string ItemMaxStack{get{ return Get("ItemMaxStack"); }}
		public static string UnLimit{get{ return Get("UnLimit"); }}
		public static string Category{get{ return Get("Category"); }}
		public static string Feature{get{ return Get("Feature"); }}
		public static string ClassRestriction{get{ return Get("ClassRestriction"); }}
		
		public static string TipReadUsers{get{ return Get("TipReadUsers"); }}
		
		public static string RefeshMail{get{ return Get("RefeshMail"); }}
		public static string DeleteMails{get{ return Get("DeleteMails"); }}
		public static string DeleteAllMails{get{ return Get("DeleteAllMails"); }}
		
		public static string NoUser{get{ return Get("NoUser"); }}
		
		public static string RefreshUser{get{ return Get("RefreshUser"); }}
		public static string AddTitle{get{ return Get("AddTitle"); }}
		public static string AddTitles{get{ return Get("AddTitles"); }}
		public static string MaxSubClass{get{ return Get("MaxSubClass"); }}
		public static string MaxGroupLight{get{ return Get("MaxGroupLight"); }}
		public static string MaxGroupDark{get{ return Get("MaxGroupDark"); }}
		public static string ResetGroupSkill{get{ return Get("ResetGroupSkill"); }}
		public static string ModUser{get{ return Get("ModUser"); }}
		public static string ResetQuestTimes{get{ return Get("ResetQuestTimes"); }}
		
		public static string ModLevel{get{ return Get("ModLevel"); }}
		public static string ModClass{get{ return Get("ModClass"); }}
		public static string ModName{get{ return Get("ModName"); }}
		public static string ModAP{get{ return Get("ModAP"); }}
		public static string ModAttri{get{ return Get("ModAttri"); }}
		
		public static string ClassMod{get{ return Get("ClassMod"); }}
		public static string NameExist{get{ return Get("NameExist"); }}
		public static string TipModLevel{get{ return Get("TipModLevel"); }}
		public static string TipModName{get{ return Get("TipModName"); }}
		public static string ModCurAttri{get{ return Get("ModCurAttri"); }}
		public static string CurValue{get{ return Get("CurValue"); }}
		public static string InputNumber{get{ return Get("InputNumber"); }}
		
		public static string SearchReset{get{ return Get("SearchReset"); }}
		public static string SearchByName{get{ return Get("SearchByName"); }}
		public static string SearchByItemClass{get{ return Get("SearchByItemClass"); }}
		public static string ItemClass{get{ return Get("ItemClass"); }}
		public static string SearchList{get{ return Get("SearchList"); }}
		public static string InputSendCount{get{ return Get("InputSendCount"); }}
		public static string Package{get{ return Get("Package"); }}
		public static string TimeDate{get{ return Get("TimeDate"); }}
		public static string Color{get{ return Get("Color"); }}
		public static string MaxStackCount{get{ return Get("MaxStackCount"); }}
		public static string Enhance{get{ return Get("Enhance"); }}
		
		public static string  Score{get{ return Get("Score"); }}
		public static string  Star{get{ return Get("Star"); }}
		public static string  PrefixEnchant{get{ return Get("PrefixEnchant"); }}
		public static string  SuffixEnchant{get{ return Get("SuffixEnchant"); }}
		public static string  ModItemCount{get{ return Get("ModItemCount"); }}
		public static string  SendItemCount{get{ return Get("SendItemCount"); }}
		public static string  CopyItemClass{get{ return Get("CopyItemClass"); }}
		
	}
}
