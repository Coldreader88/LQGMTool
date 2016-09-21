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
	public class ItemClassInfoHelper
	{
		static ItemClassInfoHelper sItemClassInfoHelper = null;
		public static ItemClassInfoHelper Get()
		{
			return sItemClassInfoHelper;
		}
		
		public bool IsOpen
		{
			get { return mInit; }
		}
		private string textFile, dbFile;
		private bool mInit = false;
		public HeroesTextHelper HeroesText{get;private set;}
		public SearchHelper Searcher{get;private set;}
		private Dictionary<string, EnchantInfo> Enchants = new Dictionary<string, EnchantInfo>();

		private Dictionary<string, EnchantInfo> CacheEnchants = new Dictionary<string, EnchantInfo>();
		private Dictionary<string, ItemClassInfo> CacheItems = new Dictionary<string, ItemClassInfo>();

		public ItemClassInfoHelper()
		{
			sItemClassInfoHelper = this;
			HeroesText = new HeroesTextHelper();
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
				textFile = "./heroes.db3";
			}
		}

		public bool Read()
		{
			if(mInit){
				return true;
			}
			mInit = true;
			if (!File.Exists(textFile) || !File.Exists(dbFile))
			{
				return false;
			}
			HeroesText.Read(textFile);
			SQLiteHelper db = new SQLiteHelper(dbFile);
			db.Open();
			ReadItems(db);
			ReadEnchants(db);
			db.Close();
			return true;
		}
		
		private void ReadItems(SQLiteHelper db){
			using (DbDataReader reader = db.GetReader("SELECT * FROM ItemClassInfo"))
			{
				while (reader != null && reader.Read())
				{
					ItemClassInfo info = new ItemClassInfo();
					info.ItemClass = ToString(reader["ItemClass"]).ToLower();
					try
					{
						info.SubCategory = (SubCategory)Enum.Parse(typeof(SubCategory), ToString(reader["Category"]));
					}
					catch (Exception)
					{
						info.SubCategory = SubCategory.NONE;
					}
					try
					{
						info.MainCategory = (MainCategory)Enum.Parse(typeof(MainCategory), ToString(reader["TradeCategory"]));
					}
					catch (Exception)
					{
						info.MainCategory = MainCategory.NONE;
					}
					info.RequiredLevel = Convert.ToInt32(reader["RequiredLevel"]);
					info.ClassRestriction = Convert.ToInt32(reader["ClassRestriction"]);
					HeroesText.ItemNames.TryGetValue(info.ItemClass, out info.Name);
					HeroesText.ItemDescs.TryGetValue(info.ItemClass, out info.Desc);
					Searcher.Add(info);
				}
			}
		}
		
		private void ReadEnchants(SQLiteHelper db){
			
			using (DbDataReader reader2 = db.GetReader("SELECT * FROM EnchantInfo ORDER BY EnchantLevel;"))
			{
				while (reader2 != null && reader2.Read())
				{
					EnchantInfo info = new EnchantInfo();
					info.Class = ToString(reader2["EnchantClass"]).ToLower();
					if (info.Class != null && info.Class.EndsWith("_100"))
					{
						continue;
					}
					info.Constraint = ToString(reader2["ItemConstraint"]);
					info.Desc = ToString(reader2["ItemConstraintDesc"]);
					info.IsPrefix = Convert.ToBoolean(reader2["IsPrefix"]);
					info.MinArg = Convert.ToInt32(reader2["MinArgValue"]);
					info.MaxArg = Convert.ToInt32(reader2["MaxArgValue"]);
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
						if(has){
							if (HeroesText.EnchantEffectIfs.TryGetValue(info.Class + "_" + i, out var))
							{
								info.Effect += "[" + var+"]";
							}
							info.Effect += "\n";
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
		
		#region Cache/Get
		public void ClearCache()
		{
			CacheEnchants.Clear();
			CacheItems.Clear();
		}

		public EnchantInfo[] GetEnchantInfos()
		{
			return Enchants.Values.ToArray<EnchantInfo>();
		}
		public ItemClassInfo Get(string itemclass)
		{
			if (itemclass == null) return null;
			itemclass = itemclass.ToLower();
			ItemClassInfo info = new ItemClassInfo();
			if (CacheItems.TryGetValue(itemclass, out info))
			{
				return info;
			}
			Searcher.Items.TryGetValue(itemclass, out info);
			CacheItems.Add(itemclass, info);
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
			if (CacheEnchants.TryGetValue(name, out info))
			{
				return info;
			}
			Enchants.TryGetValue(name, out info);
			CacheEnchants.Add(name, info);
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
