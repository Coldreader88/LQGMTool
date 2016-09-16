using System;
using System.Collections.Generic;
using System.Linq;
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
        public ItemClassInfoHelper(string textFile, string dbFile)
        {
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
            Dictionary<string, string> texts = GetTextDic(textFile);
            db.Open();
            using (SQLiteDataReader reader = db.GetReader("SELECT * FROM ItemClassInfo;"))
            {
                while (reader != null && reader.Read())
                {
                    ItemClassInfo info = new ItemClassInfo();
                    info.ItemClass = ToString(reader["ItemClass"]).ToLower();
                    try {
                        info.Category = (ItemCategory)Enum.Parse(typeof(ItemCategory), ToString(reader["Category"]));
                    }
                    catch (Exception)
                    {
                        info.Category = ItemCategory.NONE;
                    }
                    info.RequiredLevel = Convert.ToInt32(reader["RequiredLevel"]);
                    info.ClassRestriction = Convert.ToInt32(reader["ClassRestriction"]);
                    string name,desc;
                    texts.TryGetValue("name_" + info.ItemClass, out name);
                    texts.TryGetValue("desc_" + info.ItemClass, out desc);
                    info.Name = ToCN(name);
                    info.Desc = ToCN(desc);
                    ItemClassInfo tmp = new ItemClassInfo();
                    if (Items.TryGetValue(info.ItemClass,out tmp))
                    {
                        Items[info.ItemClass] = info;
                    }
                    else
                    {
                        Infos.Add(info);
                        Items.Add(info.ItemClass, info);
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
        public Dictionary<string, string> GetTextDic(string file)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.Unicode))
                {
                    string line = null;
                    Regex regex = new Regex("HEROES_ITEM_([N|D]\\S+?)\"\\s+\"([\\s\\S]+?)\"");
                    Match m = null;
                    while ((line = sr.ReadLine()) != null)
                    { /*
                        string lline = line.ToLower();

                        if (lline.Contains("heroes_item_"))
                        {
                            if (lline.Contains("heroes_item_name_") || lline.Contains("heroes_item_desc_"))
                            {
                                int i = line.IndexOf("\"");
                                int j = line.IndexOf("\"", i + 1);
                                int k = line.IndexOf("\"", j + 1);
                                int l = line.LastIndexOf("\"");
                                if (j >= 0 && l>k)
                                {
                                    string key = line.Substring(i + 1, j - i - 2);
                                    string value = line.Substring(k + 1, l - j - 2);
                                    dic.Add(key.ToLower(), value);
                                }
                            }
                        }
                       */
                        if ((m = regex.Match(line)) != null)
                        {
                            if (m.Groups.Count > 2)
                            {
                                string v;
                                string k = m.Groups[1].Value.ToLower();
                                if (dic.TryGetValue(k, out v))
                                {
                                    // Console.WriteLine("old=" + v + ",new=" + m.Groups[2].Value);
                                    // dic[k] = m.Groups[2].Value;
                                }
                                else
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
