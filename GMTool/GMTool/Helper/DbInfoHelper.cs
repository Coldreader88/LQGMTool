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
using GMTool.Enums;
using GMTool.Extensions;
using System.Windows.Forms;

namespace GMTool.Helper
{
	public class DbInfoHelper
	{
		#region
		static DbInfoHelper sItemClassInfoHelper = new DbInfoHelper();
		public Dictionary<string, string> MailTitles { get; private set; }
		public Dictionary<string, string> ItemStatNames{get; private set;}
		public Dictionary<int, SkillBonusInfo> SynthesisSkillBonues{get; private set;}

		private string textFile,patchTextFile, dbFile;
		private bool mInit = false;
		public SearchHelper Searcher{get;private set;}
		private Dictionary<string, EnchantInfo> Enchants = new Dictionary<string, EnchantInfo>();
		private Dictionary<long, TitleInfo> Titles=new Dictionary<long, TitleInfo>();
				
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
			sItemClassInfoHelper = this;
			Searcher = new SearchHelper();
			SynthesisSkillBonues = new Dictionary<int, SkillBonusInfo>();
			IniHelper helper = new IniHelper(Program.INT_FILE);
			this.textFile = helper.ReadValue("data", "text");
			if (!File.Exists(textFile))
			{
				textFile = "./heroes_text_taiwan.txt";
			}
            patchTextFile = helper.ReadValue("data", "patch_text");
            this.dbFile = helper.ReadValue("data", "heroes");
			if (!File.Exists(dbFile))
			{
				dbFile = "./heroes.db3";
			}
		}

		public bool Read()
		{
			if(mInit){
				return true;
			}
			mInit = true;
			if (!File.Exists(dbFile))
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
				"select ts.titleid,ti.description as name,feature,tg.description,category,autogivelevel,requiredlevel,classrestriction" +
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
			if (title.StartsWith("#")) {
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

		private string ToString(object obj)
		{
			if (obj == DBNull.Value)
				return "";
			return Convert.ToString(obj);
		}
		#endregion
	}
}
