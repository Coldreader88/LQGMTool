using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vindictus.Common;
using Vindictus.Helper;
using System.Data.Common;
using Vindictus.Enums;

namespace DataBasePatch
{
    public class PircePatch : BasePatch
    {
        private string CashshopType;
        private List<string> ItemClasses = new List<string>();
        public PircePatch(SQLiteHelper db, string cashshopType = "zh-CN") : base(db)
        {
            this.CashshopType = cashshopType;
        }

        public override int Patch()
        {
            Console.WriteLine("清空旧价格表，重新添加价格表");
            db.ExcuteSQL("delete from CustomizePriceInfo");
            Console.WriteLine("开始处理价格");
            List<string> sqls = new List<string>();
            foreach (string item in ItemClasses)
            {
                string s = "INSERT INTO CustomizePriceInfo (ItemClass, Duration, Price, CashShopType, Feature)" +
          " VALUES ('"+ item + "', -1, 0, NULL, NULL);";
                sqls.Add(s);
            }
            Console.WriteLine("准备添加:" + sqls.Count + "物品价格");
#if DEBUG
            return sqls.Count;//
#else
            return db.ExcuteSQLs(sqls.ToArray<string>());
#endif
        }

        protected override bool ReadData()
        {
            Console.WriteLine("读取存在的数据");
            //已经添加的列表
            string sql = "SELECT * from CustomizeItemInfo GROUP BY ItemClass";
#if DEBUG
            Console.WriteLine(sql);
#endif
            using (DbDataReader reader = db.GetReader(sql))
            {
                while (reader != null && reader.Read())
                {
                    string itemclass = reader.ReadString("ItemClass");
                    if (ItemClasses.Contains(itemclass))
                    {
                        return false;
                    }
                    ItemClasses.Add(itemclass);
                }
            }
            return true;
        }
    }
}
