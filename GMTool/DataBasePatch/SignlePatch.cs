using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMTool.Common;
using GMTool.Helper;
using GMTool.Helpers;
using GMTool.Enums;
using System.Data.Common;

namespace DataBasePatch
{
    public class SignlePatch : BasePatch
    {
        protected ClassInfo Class { get; private set; }
        protected Category Type { get; private set; }
        protected string CashshopType;
        protected ArrayList<string> ItemClasses = new ArrayList<string>();
        public SignlePatch(SQLiteHelper db, Category type,ClassInfo Class,string cashshopType="zh-CN") : base(db)
        {
            this.Class = Class;
            this.Type = type;
            this.CashshopType = cashshopType;
        }
        public SignlePatch(string db, Category type, ClassInfo Class, string cashshopType = "zh-CN") : base(db)
        {
            this.Class = Class;
            this.Type = type;
            this.CashshopType = cashshopType;
        }

        protected override bool ReadData()
        {
            Console.WriteLine("读取存在的数据");
            //已经添加的列表
            string sql = "SELECT \"Order\",ItemClass from CustomizeItemInfo " +
                "where (CashShopType='"+ CashshopType + "' or CashShopType ISNULL) " +
                "and Category ='" + Type.Name() + "' and ItemClass like '" + Class + "%"+Type.Key()+"%';";
#if DEBUG
            Console.WriteLine(sql);
#endif
            using (DbDataReader reader = db.GetReader(sql))
            {
                while (reader != null && reader.Read())
                {
                    int order = reader.ReadInt32("Order");
                    string itemclass = reader.ReadString("ItemClass");
                    if (Items.ContainsKey(order))
                    {
                        return false;
                    }
                    Items.Add(order, itemclass);
                }
            }
            Console.WriteLine("读取全数据:"+ Class + "_"+Type);
            //sql = "SELECT ItemClass from ItemClassInfo " +
            //    "where ItemClass like '" + Class + "%" + Type.Key() + "%' and Category='" + Type.Name() + "';";
            sql ="SELECT ItemClass from ItemClassInfo "+
            	"where (classRestriction & "+(int)Class+") = "+(int)Class+
            	" and Category='"+Type.Name()+"' and itemclass like '%_"+Type.Name()+"_%' and itemclass not like 'cash%' order by itemclass;";
#if DEBUG
            Console.WriteLine(sql);
#endif
            //可用列表
            using (DbDataReader reader = db.GetReader(sql))
            {
                while (reader != null && reader.Read())
                {
                    string id = reader.ReadString("ItemClass");
                    if (!ItemClasses.Contains(id))
                    {
                        ItemClasses.Add(id);
                    }
                }
            }
            return true;
        }
        public override int Patch()
        {
            int max= Items.Count>0?Items.Keys.Max()+1:1;
            Console.WriteLine("开始位置:" + max);
            ArrayList<string> sqls = new ArrayList<string>();
            foreach (string item in ItemClasses)
            {
                if (Items.Values.Contains<string>(item))
                {
                    continue;
                }
                string sql = GetSQL(item, max++);
#if DEBUG
                Console.WriteLine(sql);
#endif
                sqls.Add(sql);
            }
            Console.WriteLine("准备添加:"+ sqls.Count+"个物品");
#if DEBUG
            return sqls.Count;//
#else
            return db.ExcuteSQLs(sqls.ToArray<string>());
#endif
        }

        private string GetSQL(string item,int order)
        {
            return "INSERT INTO \"CustomizeItemInfo\" (\"Category\", \"Order\",\"ItemClass\", \"Weight\", \"IsCash\", \"IsNew\", \"Description\",\"CashShopType\") " +
                "\nVALUES ('"+Type.Name()+"', "+ order + ", '"+ item + "', 1, 'True', 'False', 'HEROES_ITEM_NAME_"+ item + "', '"+ CashshopType + "');";
        }
    }
}
