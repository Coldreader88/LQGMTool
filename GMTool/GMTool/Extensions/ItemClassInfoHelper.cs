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

namespace GMTool.Helper
{
    public class ItemClassInfoHelper
    {
        private string textFile, dbFile;
        private bool TW2CN = true;
        private List<ItemClassInfo> Infos = new List<ItemClassInfo>();
        private Dictionary<string, EnchantInfo> CacheEnchants = new Dictionary<string, EnchantInfo>();
        private Dictionary<string, ItemClassInfo> CacheItems = new Dictionary<string, ItemClassInfo>();


        private Dictionary<string, ItemClassInfo> Items = new Dictionary<string, ItemClassInfo>();
        private Dictionary<string, EnchantInfo> Enchants = new Dictionary<string, EnchantInfo>();
        private Dictionary<string, string> Enchs = new Dictionary<string, string>();

        static ItemClassInfoHelper sItemClassInfoHelper = null;
        public static ItemClassInfoHelper Get()
        {
            return sItemClassInfoHelper;
        }
        public ItemClassInfoHelper()
        {
            IniHelper helper = new IniHelper(Program.INT_FILE);
            string val  =helper.ReadValue("data", "forceZhCN");
            if(val!=null){
            	val  = val.ToLower();
            }
            TW2CN = "开" == val ||"true" == val;

            sItemClassInfoHelper = this;
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

        public bool IsOpen
        {
            get { return Items.Count > 0; }
        }
        public bool Read()
        {
            if (!File.Exists(textFile) || !File.Exists(dbFile))
            {
                return false;
            }
            SQLiteHelper db = new SQLiteHelper(dbFile);
            db.Open();
            using (DbDataReader reader = db.GetReader("SELECT * FROM ItemClassInfo"))
            {
                Dictionary<string, string> names = GetNameDic(textFile);
                Dictionary<string, string> descs = GetDescDic(textFile);
                while (reader != null && reader.Read())
                {
                    ItemClassInfo info = new ItemClassInfo();
                    info.ItemClass = ToString(reader["ItemClass"]).ToLower();
                    try
                    {
                        info.Category = (SubCategory)Enum.Parse(typeof(SubCategory), ToString(reader["Category"]));
                    }
                    catch (Exception)
                    {
                        info.Category = SubCategory.NONE;
                    }
                    try
                    {
                        info.TradeCategory = (MainCategory)Enum.Parse(typeof(MainCategory), ToString(reader["TradeCategory"]));
                    }
                    catch (Exception)
                    {
                        info.TradeCategory = MainCategory.NONE;
                    }
                    info.RequiredLevel = Convert.ToInt32(reader["RequiredLevel"]);
                    info.ClassRestriction = Convert.ToInt32(reader["ClassRestriction"]);
                    string name, desc;
                    names.TryGetValue("name_" + info.ItemClass, out name);
                    descs.TryGetValue("desc_" + info.ItemClass, out desc);
                    info.Name = ToCN(name);
                    info.Desc = ToCN(desc);
                    ItemClassInfo tmp = new ItemClassInfo();
                    if (Items.TryGetValue(info.ItemClass, out tmp))
                    {
                        //  Items[info.ItemClass] = info;
                    }
                    else
                    {
                        Infos.Add(info);
                        Items.Add(info.ItemClass, info);
                    }

                }
            }
            using (DbDataReader reader2 = db.GetReader("SELECT * FROM EnchantInfo ORDER BY EnchantLevel;"))
            {
                Dictionary<string, string> names1 = GetPrefixNameDic(textFile);
                Dictionary<string, string> names2 = GetSuffixNameDic(textFile);
                //说明
                Dictionary<string, string> descs = GetEnchDescDic(textFile);
                Dictionary<string, string> effects = GetEnchEffectDic(textFile);
                Dictionary<string, string> effectIfs = GetEnchEffectIfDic(textFile);
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
                        if (names1.TryGetValue(info.Class, out var))
                        {
                            info.Name = ToCN(var);
                        }
                    }
                    else
                    {
                        if (names2.TryGetValue(info.Class, out var))
                        {
                            info.Name = ToCN(var);
                        }
                    }

                    if (descs.TryGetValue(info.Class, out var))
                    {
                        info.Desc = ToCN(var);
                    }
                    info.Effect = "";
                    for (int i = 1; i <= 5; i++)
                    {
                        if (effects.TryGetValue(info.Class + "_" + i, out var))
                        {
                            info.Effect += i + "." + ToCN(var);
                        }
                        if (effectIfs.TryGetValue(info.Class + "_" + i, out var))
                        {
                            info.Effect += ",条件：" + ToCN(var);
                        }
                        info.Effect += "\n";
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
            db.Close();
            return true;
        }

        public void ClearCache()
        {
            CacheEnchants.Clear();
            CacheItems.Clear();
        }
        private string ToCN(string tw)
        {
            if (tw == null)
            {
                return null;
            }
            if (TW2CN)
            {
                tw = tw.Replace("\\n", "\n").Trim();
                return ChineseTextHelper.ToSimplified(tw);
            }
            return tw;
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
            Items.TryGetValue(itemclass, out info);
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


        //
        public List<ItemClassInfo> SearchItems(string name, string id, string itemtype, string category, User user)
        {
        	int _class = user==null?0:(int)user.Class;
            List<ItemClassInfo> rs = new List<ItemClassInfo>();
            foreach (ItemClassInfo info in Infos)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (info.Name == null || !info.Name.Contains(name))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(id))
                {
                    if (info.ItemClass == null || !info.ItemClass.Contains(id))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(category) && category != SubCategory.NONE.Name())
                {
                    if (info.Category.Name() != category)
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(itemtype) && itemtype != MainCategory.NONE.Name())
                {
                    if (info.TradeCategory.Name() != itemtype)
                    {
                        continue;
                    }
                }
                if(_class!=0 && info.ClassRestriction !=0){
                	if((_class & info.ClassRestriction)==0){
                		continue;
                	}
                	
                }
                rs.Add(info);
            }
            return rs;
        }
        //HEROES_ATTRIBUTE_PREFIX_
        private Dictionary<string, string> GetPrefixNameDic(string file)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
                {
                    string line = null;
                    //HEROES_ITEMCONSTRAINT_名字_  附魔
                    //Heroes_EnchantCondition_名字_1-4 4个效果
                    Regex regex = new Regex("HEROES_ATTRIBUTE_PREFIX_(\\S+?)\"\\s+\"([\\s\\S]+?)\"");
                    Match m = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if ((m = regex.Match(line)) != null)
                        {
                            if (m.Groups.Count > 2)
                            {
                                string v;
                                string k = m.Groups[1].Value.ToLower();
                                if (!dic.TryGetValue(k, out v))
                                {
                                    dic.Add(k, m.Groups[2].Value);
                                }
                            }
                        }
                    }
                }
                return dic;
            }
        }
        //HEROES_ATTRIBUTE_SUFFIX_
        private Dictionary<string, string> GetSuffixNameDic(string file)
        {
            return GetDic(file, "HEROES_ATTRIBUTE_SUFFIX_(\\S+?)\"\\s+\"([\\s\\S]+?)\"");
        }

        private Dictionary<string, string> GetEnchDescDic(string file)
        {
            return GetDic(file, "HEROES_ITEMCONSTRAINT_(\\S+?)\"\\s+\"([\\s\\S]+?)\"");
        }
        private Dictionary<string, string> GetEnchEffectDic(string file)
        {
            return GetDic(file, "HEROES_ENCHANTSTAT_(\\S+?)\"\\s+\"([\\s\\S]+?)\"");
        }
        private Dictionary<string, string> GetEnchEffectIfDic(string file)
        {
            //HEROES_ENCHANTCONDITION_
            return GetDic(file, "HEROES_ENCHANTCONDITION_(\\S+?)\"\\s+\"([\\s\\S]+?)\"");
        }

        private Dictionary<string, string> GetNameDic(string file)
        {
            return GetDic(file, "HEROES_ITEM_(N\\S+?)\"\\s+\"([\\s\\S]+?)\"");
        }
        private Dictionary<string, string> GetDescDic(string file)
        {
            return GetDic(file, "HEROES_ITEM_(D\\S+?)\"\\s+\"([\\s\\S]+?)\"");
        }
        private Dictionary<string, string> GetDic(string file, string regx)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
                {
                    string line = null;
                    Regex regex = new Regex(regx);
                    Match m = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if ((m = regex.Match(line)) != null)
                        {
                            if (m.Groups.Count > 2)
                            {
                                string v;
                                string k = m.Groups[1].Value.ToLower();
                                if (!dic.TryGetValue(k, out v))
                                {
                                    dic.Add(k, m.Groups[2].Value);
                                }
                            }
                        }
                    }
                }
                return dic;
            }
        }
    }
}
