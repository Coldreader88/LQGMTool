using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vindictus.Common;
using Vindictus.Helper;
using Vindictus.Enums;
using System.Data.Common;

namespace DataBasePatch
{
    public class AvatarInfo
    {
        public string Key;
        public string Head;
        public string Lower;
        public string Upper;
        public string Hand;
        public string Foot;

        public int Count;

        public bool IsVaild()
        {
            int i = 0;
            if (!string.IsNullOrEmpty(Head))
            {
                i++;
            }
            if (!string.IsNullOrEmpty(Lower))
            {
                i++;
            }
            if (!string.IsNullOrEmpty(Upper))
            {
                i++;
            }
            if (!string.IsNullOrEmpty(Hand))
            {
                i++;
            }
            if (!string.IsNullOrEmpty(Foot))
            {
                i++;
            }
            Count = i;
            return i >= 5;
        }
        public override string ToString()
        {
            return "Key=" + Key + "\tHead=" + Head + "\tLower=" + Lower + "\tUpper=" + Upper + "\tHand=" + Hand + "\tFoot=" + Foot;
        }
    }
    public class AvatarPatch : BasePatch
    {
        private string CashshopType;
        private Dictionary<string, AvatarInfo> AvatarInfos = new Dictionary<string, AvatarInfo>();
        public AvatarPatch(SQLiteHelper db, string cashshopType = "zh-CN") : base(db)
        {
            this.CashshopType = cashshopType;
        }
        public AvatarPatch(string db, string cashshopType = "zh-CN") : base(db)
        {
            this.CashshopType = cashshopType;
        }

        public override int Patch()
        {
            Console.WriteLine("清空全部时装，重新添加");
#if !DEBUG
            db.ExcuteSQL("delete from CustomizeItemInfo WHERE "+
                        "(Feature ISNULL or Feature = '"+ CashshopType + "' or Feature='') and " +
                        " Category in ('AVATAR_PACKAGE'," +
                        "'AVATAR_HELM'," +
                        "'AVATAR_TUNIC'," +
                        "'AVATAR_PANTS'," +
                        "'AVATAR_BOOTS'," +
                        "'AVATAR_GLOVES') ");
#endif
            Console.WriteLine("开始处理时装");
            List<string> sqls = new List<string>();
            int order = 0;
            foreach (AvatarInfo info in AvatarInfos.Values)
            {
                int start = order * 6;
                if (info.IsVaild())
                {
                    if (!string.IsNullOrEmpty(info.Head))
                    {
                        sqls.Add(GetSQL(info.Head, Category.AVATAR_HELM.ToString(), start + 1));
                    }
                    if (!string.IsNullOrEmpty(info.Upper))
                    {
                        sqls.Add(GetSQL(info.Upper, Category.AVATAR_TUNIC.ToString(), start + 2));
                    }
                    if (!string.IsNullOrEmpty(info.Lower))
                    {
                        sqls.Add(GetSQL(info.Lower, Category.AVATAR_PANTS.ToString(), start + 3));
                    }
                   
                    if (!string.IsNullOrEmpty(info.Hand))
                    {
                        sqls.Add(GetSQL(info.Hand, Category.AVATAR_GLOVES.ToString(), start + 4));
                    }
                    if (!string.IsNullOrEmpty(info.Foot))
                    {
                        sqls.Add(GetSQL(info.Foot, Category.AVATAR_BOOTS.ToString(), start+5));
                    }
                    order++;
                }
            }
            Console.WriteLine("准备添加时装:"+ sqls.Count);
#if DEBUG
            return sqls.Count;//
#else
            return db.ExcuteSQLs(sqls.ToArray<string>());
#endif
        }

        protected override bool ReadData()
        {
            Console.WriteLine("读取全部时装");
            //已经添加的列表
            string sql = "SELECT * FROM ItemClassInfo where " +
                        "(Feature ISNULL or Feature = '"+ CashshopType + "' or Feature='') and " +
                        " Category in ('AVATAR_PACKAGE'," +
                        "'AVATAR_HELM'," +
                        "'AVATAR_TUNIC'," +
                        "'AVATAR_PANTS'," +
                        "'AVATAR_BOOTS'," +
                        "'AVATAR_GLOVES') " +
                        " and ItemClass not like '%day%' " +
                        " and ItemClass not like 'cash%' " +
                        "ORDER BY ItemClass; ";
#if DEBUG
            Console.WriteLine(sql);
#endif
            List<string> packages = new List<string>();
            int foot = 0, head = 0, hand = 0, lower = 0, upper = 0;
            using (DbDataReader reader = db.GetReader(sql))
            {
                while (reader != null && reader.Read())
                {
                    string category = reader.ReadString("Category");
                    string itemclass = reader.ReadString("ItemClass");
                    string key = null;
                    if (category.Equals(Category.AVATAR_PACKAGE.ToString()))
                    {
                        //key = itemclass.Replace("_foot", "{0}");
                        if (!packages.Contains(itemclass))
                        {
                            packages.Add(itemclass);
                        }
                        continue;
                    }
                    else if (category.Equals(Category.AVATAR_BOOTS.ToString()))
                    {
                        key = itemclass.Replace("_foot", "{0}");
                        foot++;
                    }
                    else if (category.Equals(Category.AVATAR_GLOVES.ToString()))
                    {
                        key = itemclass.Replace("_hand", "{0}");
                        hand++;
                    }
                    else if (category.Equals(Category.AVATAR_HELM.ToString()))
                    {
                        key = itemclass.Replace("_head", "{0}");
                        head++;
                    }
                    else if (category.Equals(Category.AVATAR_PANTS.ToString()))
                    {
                        key = itemclass.Replace("_lower", "{0}");
                        lower++;
                    }
                    else if (category.Equals(Category.AVATAR_TUNIC.ToString()))
                    {
                        key = itemclass.Replace("_upper", "{0}");
                        upper++;
                    }
                    if (key == null)
                    {
                        continue;
                    }
                    AvatarInfo info;
                    if (!AvatarInfos.TryGetValue(key, out info))
                    {
                        info = new AvatarInfo();
                        info.Key = key;
                        AvatarInfos.Add(key, info);
                    }
                    if (category.Equals(Category.AVATAR_BOOTS.ToString()))
                    {
                        info.Foot = itemclass;
                    }
                    else if (category.Equals(Category.AVATAR_GLOVES.ToString()))
                    {
                        info.Hand = itemclass;
                    }
                    else if (category.Equals(Category.AVATAR_HELM.ToString()))
                    {
                        info.Head = itemclass;
                    }
                    else if (category.Equals(Category.AVATAR_PANTS.ToString()))
                    {
                        info.Lower = itemclass;
                    }
                    else if (category.Equals(Category.AVATAR_TUNIC.ToString()))
                    {
                        info.Upper = itemclass;
                    }
                }
            }
#if DEBUG
            Console.WriteLine("packages=" + packages.Count);
            Console.WriteLine("foot=" + foot);
            Console.WriteLine("hand=" + hand);
            Console.WriteLine("head=" + head);
            Console.WriteLine("lower=" + lower);
            Console.WriteLine("upper=" + upper);
            int i = 0;
            foreach (AvatarInfo info in AvatarInfos.Values)
            {
                if (info.IsVaild())
                {
                    i++;
                }
                else
                {
                    if (i > 1)
                        Console.WriteLine(info);
                }
            }
            Console.WriteLine("set=" + i);
#endif
            return true;
        }
        private string GetSQL(string item,string type, int order)
        {
            return "INSERT INTO \"CustomizeItemInfo\" (\"Category\", \"Order\",\"ItemClass\", \"Weight\", \"IsCash\", \"IsNew\", \"Description\",\"CashShopType\") " +
                "\nVALUES ('" + type + "', " + order + ", '" + item + "', 1, 'True', 'False', 'HEROES_ITEM_NAME_" + item + "', '" + CashshopType + "');";
        }
    }
}
