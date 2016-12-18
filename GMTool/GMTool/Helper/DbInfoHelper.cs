using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GMTool.Helper;
using System.Data.SQLite;
using GMTool.Bean;
using GMTool.Helpers;
using System.Data.Common;
using GMTool.Common;
using GMTool.Enums;
using GMTool.Extensions;
using System.Windows.Forms;

namespace GMTool.Helper
{
	public class DbInfoHelper
	{
		#region
		public readonly static DbInfoHelper sItemClassInfoHelper = new DbInfoHelper();
		public Dictionary<string, string> MailTitles { get; private set; }
		public Dictionary<string, string> ItemStatNames{get; private set;}
		public Dictionary<int, SkillBonusInfo> SynthesisSkillBonues{get; private set;}

		private string textFile,patchTextFile, dbFile;
		private bool mInit = false;
		public SearchHelper Searcher{get;private set;}
		private Dictionary<string, EnchantInfo> Enchants = new Dictionary<string, EnchantInfo>();
		private Dictionary<long, TitleInfo> Titles=new Dictionary<long, TitleInfo>();
		private static string DEF_TEXT = "heroes_text_chinese.txt";
		private static string DEF_TW = "heroes_text_taiwan.txt";
		private static string DEF_DB = "heroes.db3";
		
		public static DbInfoHelper Get()
		{
			return sItemClassInfoHelper;
		}
		
		public bool IsOpen
		{
			get { return mInit; }
		}
		private DbInfoHelper()
		{
			Searcher = new SearchHelper();
			SynthesisSkillBonues = new Dictionary<int, SkillBonusInfo>();
			IniHelper helper = new IniHelper(Program.INT_FILE);
			this.textFile = helper.ReadValue("data", "text");
			if (!File.Exists(textFile))
			{
				textFile = "./"+DEF_TEXT;
			}
			if (!File.Exists(textFile))
			{
				textFile = "./"+DEF_TW;
			}
			patchTextFile = helper.ReadValue("data", "patch_text");
			this.dbFile = helper.ReadValue("data", "heroes");
			if (!File.Exists(dbFile))
			{
				this.dbFile = "./"+DEF_DB;
			}
		}

		public bool Read()
		{
			if(mInit){
				return true;
			}
			mInit = true;
			if (!File.Exists(this.dbFile))
			{
				return false;
			}
			HeroesTextHelper HeroesText  = new HeroesTextHelper();
			HeroesText.Read(textFile, patchTextFile);
			MailTitles = HeroesText.MailTitles;
			ItemStatNames = HeroesText.ItemStatNames;
			SQLiteHelper db = new SQLiteHelper(dbFile);
			db.Open();
			ReadItems(db,HeroesText );
			ReadEnchants(db,HeroesText);
			ReadTitles(db, HeroesText);
			ReadSkillBonuds(db, HeroesText);
			db.Close();
			return true;
		}
		#endregion
		
		#region checkfile
		public bool CheckFiles(Form form){
			IniHelper helper = new IniHelper(Program.INT_FILE);
			string SEP = ""+Path.PathSeparator;
			string name = null;
			bool CN = false;
			string dir = "";
			if(!File.Exists(textFile)){
				CN = SelectGameVer(out name);
				if(string.IsNullOrEmpty(name)){
					return false;
				}
				form.Info("请选择游戏目录");
				dir = SelectGamePath(CN);
				if(dir == null){
					return false;
				}
				//查找本地已经存在的
				string deftext= PathHelper.Combine(dir, "resource","localized_text",(CN?"chinese":"taiwan"),(CN?DEF_TEXT:DEF_TW));
				if(File.Exists(deftext)){
					form.Info("找到翻译文件\n"+deftext);
					helper.WriteValue("data", "text",deftext);
					this.textFile = deftext;
				}else{
					//解压
					if(VZip.ExtractAllHfsFindFile(PathHelper.Combine(dir, "hfs"), name, Application.StartupPath)){
						form.Info("找到翻译文件\n"+name);
						helper.WriteValue("data", "text", name);
						this.textFile = PathHelper.Combine(Application.StartupPath, name);
					}else{
						form.Info("没找到翻译文件\n"+name);
						return false;
					}
				}
			}
			if(!File.Exists(dbFile)){
				if(string.IsNullOrEmpty(name)){
					CN = SelectGameVer(out name);
					if(string.IsNullOrEmpty(name)){
						return false;
					}
					form.Info("请选择游戏目录");
					dir = SelectGamePath(CN);
					if(dir == null){
						return false;
					}
				}
				string defdb= PathHelper.Combine(dir, "sql", DEF_DB);
				if(File.Exists(defdb)){
					form.Info("找到数据库\n"+defdb);
					helper.WriteValue("data", "heroes",defdb);
					this.dbFile = defdb;
				}else{
					//解压
					if(VZip.ExtractAllHfsFindFile(PathHelper.Combine(dir, "hfs"), DEF_DB, Application.StartupPath)){
						form.Info("找到数据库\n"+DEF_DB);
						helper.WriteValue("data", "heroes",DEF_DB);
						this.dbFile = DEF_DB;
					}else{
						form.Error("没找到数据库\n"+DEF_DB);
						return false;
					}
				}
			}
			return true;
		}
		private bool SelectGameVer(out string text){
			bool CN = false;
			DialogResult rs = MessageBoxEx.Show(
				"没有找到翻译文件，接下来请选择游戏目录，自动导出客户端的翻译文件",
				"请选择游戏目录",
				MessageBoxButtons.YesNoCancel,
				MessageBoxIcon.Question,
				new string[]{"国服","台服","取消"}
			);
			if(rs==DialogResult.Yes){
				text = DEF_TEXT;
				CN = true;
			}else if(rs==DialogResult.No){
				text = DEF_TW;
				CN = false;
			}else{
				text = null;
				return false;
			}
			return CN;
		}
		private string SelectGamePath(bool CN){
			string dir="";
			do{
				using(FolderBrowserDialog dlg=new FolderBrowserDialog()){
					dlg.SelectedPath = Application.StartupPath;
					dlg.Description = "请选择"+(CN?"":"")+"游戏文件夹";
					if(dlg.ShowDialog()==DialogResult.OK){
						dir = dlg.SelectedPath;
					}else{
						return null;
					}
				}
			}while(!Directory.Exists(PathHelper.Combine(dir, "hfs")));
			return dir;
		}
		#endregion

		#region synskillbonuds
		private void ReadSkillBonuds(SQLiteHelper db,HeroesTextHelper HeroesText){
			using (DbDataReader reader = db.GetReader("select * from synthesisskillbonus order by classRestriction;"))
			{
				// MessageBox.Show("count=" + HeroesText.SynSkillBonuds.Count);
				while (reader != null && reader.Read())
				{
					SkillBonusInfo info = new SkillBonusInfo(reader, HeroesText);
					if (!SynthesisSkillBonues.ContainsKey(info.ID))
					{
						SynthesisSkillBonues.Add(info.ID, info);
					}
				}
			}
		}
		#endregion#
		
		#region title
		//select titleid,ts.description,targetcount,ispositive,isparty,category,autogivelevel,requiredlevel,classrestriction from ( titlegoalinfo as ts left join  titleinfo as ti on  ti.id=ts.titleid) order by requiredlevel;
		
		private void ReadTitles(SQLiteHelper db,HeroesTextHelper HeroesText){
			List<TitleInfo> titles=new List<TitleInfo>();
			using (DbDataReader reader = db.GetReader(
				"select ts.titleid,ti.description as name,feature,tg.description,category,autogivelevel,requiredlevel,classrestriction,tg.targetcount as tcount" +
				" from (titlestatinfo as ts left join titleinfo as ti on  ts.titleid = ti.id )"+
				" left join titlegoalinfo as tg on tg.titleid=ts.titleid"+
				" group by ts.titleid order by requiredlevel"))
			{
				while (reader != null && reader.Read())
				{
					TitleInfo info = new TitleInfo(reader, HeroesText);
					if (info.ClassRestriction > 0)
					{
						titles.Add(info);
					}
				}
			}
			foreach(TitleInfo info in titles){
				if(!Titles.ContainsKey(info.TitleID)){
					using(DbDataReader reader = db.GetReader("select * from titlestatinfo where titleID = "+info.TitleID)){
						while (reader != null && reader.Read())
						{
							info.UpdateEffect(reader);
						}
						//if(string.IsNullOrEmpty(info.Effect))
						//	continue;
					}
					info.Trim();
					Titles.Add(info.TitleID, info);
				}
			}
		}
		#endregion
		
		#region items
		private void ReadItems(SQLiteHelper db,HeroesTextHelper HeroesText){
			using (DbDataReader reader = db.GetReader("SELECT * FROM ItemClassInfo left join equipiteminfo on ItemClassInfo.itemclass = equipiteminfo.itemclass order by RequiredLevel;"))
			{
				while (reader != null && reader.Read())
				{
					Searcher.Add(new ItemClassInfo(reader, HeroesText));
				}
			}
		}
		#endregion
		
		#region enchant
		private void ReadEnchants(SQLiteHelper db,HeroesTextHelper HeroesText){
			
			using (DbDataReader reader2 = db.GetReader("SELECT * FROM EnchantInfo ORDER BY EnchantLevel;"))
			{
				while (reader2 != null && reader2.Read())
				{
					EnchantInfo info = new EnchantInfo(reader2,HeroesText);
					if (info.Class.EndsWith("_100"))
					{
						continue;
					}
					if (!Enchants.ContainsKey(info.Class))
					{
						Enchants.Add(info.Class, info);
					}

				}
			}
		}
		#endregion

		#region Cache/Get
		public string GetMailTitle(string title)
		{
			if (title!=null && title.StartsWith("#")) {
				string t;
				if (MailTitles.TryGetValue(title.Substring(1).ToLower(), out t))
				{
					return t;
				}
			}
			return title;
		}
		public TitleInfo GetTitle(int id){
			TitleInfo info = new TitleInfo();
			Titles.TryGetValue(id, out info);
			return info;
		}
		
		public TitleInfo[] GetTitles(){
			return Titles.Values.ToArray<TitleInfo>();
		}
		public EnchantInfo[] GetEnchantInfos()
		{
			return Enchants.Values.ToArray<EnchantInfo>();
		}
		public ItemClassInfo GetItemInfo(string itemclass)
		{
			if (itemclass == null) return null;
			itemclass = itemclass.ToLower();
			ItemClassInfo info = new ItemClassInfo();
			Searcher.Items.TryGetValue(itemclass, out info);
			return info;
		}
		public EnchantInfo GetEnchant(string name)
		{
			if (name == null) return null;
			name = name.ToLower();
			if (name.EndsWith("_100"))
			{
				name = name.Substring(0, name.Length - 4);
			}
			EnchantInfo info = new EnchantInfo();
			Enchants.TryGetValue(name, out info);
			return info;
		}
		#endregion
	}
}
