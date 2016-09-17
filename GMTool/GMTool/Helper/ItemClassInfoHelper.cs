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

namespace GMTool.Helper
{
    public class ItemClassInfoHelper
    {
        private string textFile, dbFile;
        private List<ItemClassInfo> Infos = new List<ItemClassInfo>();
        private Dictionary<string, ItemClassInfo> Items = new Dictionary<string, ItemClassInfo>();
        private Dictionary<string, EnchantInfo> Enchants = new Dictionary<string, EnchantInfo>();
        private Dictionary<string, string> Enchs = new Dictionary<string, string>();

        static ItemClassInfoHelper sItemClassInfoHelper = null;
        public static ItemClassInfoHelper Get()
        {
            return sItemClassInfoHelper;
        }
        public ItemClassInfoHelper(string textFile, string dbFile)
        {
            sItemClassInfoHelper = this;
            this.textFile = textFile;
            this.dbFile = dbFile;
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
            using (SQLiteDataReader reader = db.GetReader("SELECT * FROM ItemClassInfo;"))
            {
                Dictionary<string, string> names = GetNameDic(textFile);
                Dictionary<string, string> descs = GetDescDic(textFile);
                while (reader != null && reader.Read())
                {
                    ItemClassInfo info = new ItemClassInfo();
                    info.ItemClass = ToString(reader["ItemClass"]).ToLower();
                    try
                    {
                        info.Category = (ItemCategory)Enum.Parse(typeof(ItemCategory), ToString(reader["Category"]));
                    }
                    catch (Exception)
                    {
                        info.Category = ItemCategory.NONE;
                    }
                    try
                    {
                        info.TradeCategory = (ItemTradeCategory)Enum.Parse(typeof(ItemTradeCategory), ToString(reader["TradeCategory"]));
                    }
                    catch (Exception)
                    {
                        info.TradeCategory = ItemTradeCategory.NONE;
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
            using (SQLiteDataReader reader2 = db.GetReader("SELECT * FROM EnchantInfo;"))
            {
                Dictionary<string, string> names = GetEnchNameDic(textFile);
                //说明
                Dictionary<string, string> descs = GetEnchDescDic(textFile);
                Dictionary<string, string> effects = GetEnchEffectDic(textFile);
                Dictionary<string, string> effectIfs = GetEnchEffectIfDic(textFile);
                while (reader2 != null && reader2.Read())
                {
                    EnchantInfo info = new EnchantInfo();
                    info.Class = ToString(reader2["EnchantClass"]).ToLower();
                    info.Constraint = ToString(reader2["ItemConstraint"]);
                    info.Desc = ToString(reader2["ItemConstraintDesc"]);
                    string var;
                    if (names.TryGetValue(info.Class, out var))
                    {
                        info.Name = ToCN(var);
                    }
                    if (descs.TryGetValue(info.Class, out var))
                    {
                        info.Desc = ToCN(var);
                    }
                    info.Effect = "";
                    for (int i = 0; i < 4; i++)
                    {
                        if (effects.TryGetValue(info.Class+"_"+i, out var))
                        {
                            info.Effect += i+"."+ ToCN(var);
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

        private string ToCN(string tw)
        {
            if (tw == null)
            {
                return null;
            }
            tw = tw.Replace("\\n", "\n");
            return TextHelper.ToSimplified(tw);
        }

        public ItemClassInfo Get(string itemclass)
        {
            if (itemclass == null) return null;
            itemclass = itemclass.ToLower();
            ItemClassInfo info = new ItemClassInfo();
            Items.TryGetValue(itemclass, out info);
            return info;
        }
        private string ToString(object obj)
        {
            if (obj == DBNull.Value)
                return "";
            return Convert.ToString(obj);
        }

        public EnchantInfo GetEnchant(string name)
        {
            if (name == null) return null;
            name = name.ToLower();
            EnchantInfo info = new EnchantInfo();
            Enchants.TryGetValue(name, out info);
            return info;
        }
        //
        public List<ItemClassInfo> SearchItems(string name, string id, string itemtype, string category)
        {
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
                if (!string.IsNullOrEmpty(category) && category != "无")
                {
                    if (info.Category.GetName() != category)
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(itemtype) && itemtype != "无")
                {
                    if (info.TradeCategory.GetName() != itemtype)
                    {
                        continue;
                    }
                }
                rs.Add(info);
            }
            return rs;
        }
        //HEROES_ATTRIBUTE_PREFIX_
        private Dictionary<string, string> GetEnchNameDic(string file)
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
