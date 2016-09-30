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
		static DbInfoHelper sItemClassInfoHelper = null;
		public Dictionary<string, string> MailTitles { get; private set; }
		public Dictionary<string, string> ItemStatNames{get; private set;}
		public static DbInfoHelper Get()
		{
			return sItemClassInfoHelper;
		}
		
		public bool IsOpen
		{
			get { return mInit; }
		}
		private string textFile, dbFile;
		private bool mInit = false;
		public SearchHelper Searcher{get;private set;}
		private Dictionary<string, EnchantInfo> Enchants = new Dictionary<string, EnchantInfo>();
		private Dictionary<long, TitleInfo> Titles=new Dictionary<long, TitleInfo>();
		public DbInfoHelper()
		{
			sItemClassInfoHelper = this;
			Searcher = new SearchHelper();
			IniHelper helper = new IniHelper(Program.INT_FILE);
			this.textFile = helper.ReadValue("data", "text");
			if (!File.Exists(textFile))
			{
				textFile = "./heroes_text_taiwan.txt";
			}
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
			HeroesText.Read(textFile);
			MailTitles = HeroesText.MailTitles;
			ItemStatNames = HeroesText.ItemStatNames;
			SQLiteHelper db = new SQLiteHelper(dbFile);
			db.Open();
			ReadItems(db,HeroesText );
			ReadEnchants(db,HeroesText);
			ReadTitles(db, HeroesText);
			db.Close();
			return true;
		}
		#endregion
		
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
					TitleInfo info = new TitleInfo();
					info.TitleID = reader.ReadInt64("titleid");// Convert.ToInt64(ToString(reader["titleid"]));
					info.Name = reader.ReadString("name","");
					HeroesText.TitleNames.TryGetValue(info.Name.ToLower(), out info.Name);
					info.Description = reader.ReadString("description","");
					HeroesText.TitleDescs.TryGetValue(info.Description.ToLower(), out info.Description);
					info.Category = reader.ReadString("Category");
					info.Feature = reader.ReadString("feature","");
					info.OnlyClass = info.Feature.ToClassInfo();
					info.AutoGiveLevel = reader.ReadInt32("AutoGiveLevel");
					info.RequiredLevel = reader.ReadInt32("RequiredLevel");
					info.ClassRestriction = reader.ReadInt32("ClassRestriction",-1);
					if (info.ClassRestriction > 0)
					{
						titles.Add(info);
					}
				}
			}
			foreach(TitleInfo info in titles){
				using(DbDataReader reader = db.GetReader("select * from titlestatinfo where titleID = "+info.TitleID)){
					info.Effect = "";
					while (reader != null && reader.Read())
					{
						string stat = reader.ReadString("Stat","");
						int val = reader.ReadInt32("Amount");
						int tmp = 0;
						if(!info.Stats.TryGetValue(stat, out tmp)){
							info.Stats.Add(stat, val);
							string name = stat.StatName();
							info.Effect +=name+"+"+val+",";
						}
					}
					//if(string.IsNullOrEmpty(info.Effect))
					//	continue;
				}
				if(info.Effect.EndsWith(",")){
					info.Effect = info.Effect.Substring(0, info.Effect.Length-1);
				}
				if(!Titles.ContainsKey(info.TitleID)){
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
					ItemClassInfo info = new ItemClassInfo();
					info.ItemClass = reader.ReadString("ItemClass", "").ToLower();
					info.SubCategory = reader.ReadEnum<SubCategory>("Category", SubCategory.NONE);
					info.MainCategory= reader.ReadEnum<MainCategory>("TradeCategory", MainCategory.NONE);
					info.MaxStack =reader.ReadInt64("MaxStack");
					info.RequiredLevel = reader.ReadInt32("RequiredLevel");
					info.ClassRestriction = reader.ReadInt32("ClassRestriction");
					HeroesText.ItemNames.TryGetValue(info.ItemClass, out info.Name);
					HeroesText.ItemDescs.TryGetValue(info.ItemClass, out info.Desc);
					
					ItemStatInfo stat=new ItemStatInfo(reader);
					if(!stat.IsEmpty()){
						info.Stat = stat;
					}
					Searcher.Add(info);
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
					EnchantInfo info = new EnchantInfo();
					info.Class =reader2.ReadString("EnchantClass","").ToLower();
					if (info.Class.EndsWith("_100"))
					{
						continue;
					}
					info.Constraint = reader2.ReadString("ItemConstraint");
					info.Desc = reader2.ReadString("ItemConstraintDesc");
					info.IsPrefix = reader2.ReadBoolean("IsPrefix");
					info.MinArg = reader2.ReadInt32("MinArgValue");
					info.MaxArg = reader2.ReadInt32("MaxArgValue");
					info.EnchantLevel = reader2.ReadInt32("EnchantLevel");
					string var;
					if (info.IsPrefix)
					{
						if(!HeroesText.PrefixNames.TryGetValue(info.Class, out info.Name)){
							info.Name = info.Class;
						}
					}
					else
					{
						if(!HeroesText.SuffixNames.TryGetValue(info.Class, out info.Name)){
							info.Name = info.Class;
						}
					}
					HeroesText.EnchantDescs.TryGetValue(info.Class, out info.Desc);
					info.Effect = "";
					bool has= false;
					for (int i = 1; i <= 5; i++)
					{
						if (HeroesText.EnchantEffects.TryGetValue(info.Class + "_" + i, out var))
						{
							has = true;
							info.Effect += i + "." + var;
						}
						if (has)
						{
							if (HeroesText.EnchantEffectIfs.TryGetValue(info.Class + "_" + i, out var))
							{
								info.Effect += "[" + var + "]";
							}
							info.Effect += "\n";
						}
						else
						{
							break;
						}
					}
					if (info.Effect.Contains("{0}"))
					{
						info.Effect = info.Effect.Replace("{0}", info.GetValue());
					}
					EnchantInfo tmp = new EnchantInfo();
					if (Enchants.TryGetValue(info.Class, out tmp))
					{
						//  Enchants[info.ItemClass] = info;
					}
					else
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
