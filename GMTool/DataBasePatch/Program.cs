using DataBasePatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMTool.Helper;
using System.Threading.Tasks;
using GMTool.Enums;

namespace DataBasePatch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("使用前请做好备份，CustomizePriceInfo和CustomizeItemInfo");
            Console.ReadKey();
            //select * from customizeiteminfo where (Feature ISNULL or Feature = 'zh-CN' or Feature='') order by category ,"order"
            //
            string path = args.Length > 0 ? args[0] : "heroesContents.db3";
            SQLiteHelper db = new SQLiteHelper(path);
            if (!db.Open())
            {
                Console.WriteLine("无法打开数据库");
                Console.ReadKey();
                return;
            }
            List<BasePatch> patchs = new ArrayList<BasePatch>();

            //时装
           // patchs.Add(new AvatarPatch(db));
            //发型
            //patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Fiona, "zh-CN"));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Fiona));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Evy));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Lynn));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Vella));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Arisha));
            //眉毛
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Fiona));
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Evy));
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Lynn));
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Vella));
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Arisha));
            //妆容
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Fiona));
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Evy));
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Lynn));
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Vella));
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Arisha));
            //内衣
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Fiona));
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Evy));
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Lynn));
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Vella));
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Arisha));
            //价格必须放在最后
            patchs.Add(new PircePatch(db));
            foreach (BasePatch patch in patchs)
            {
                if (patch.Read())
                {
                    patch.Patch();
                }
                else
                {
                    Console.Error.WriteLine("读取出错");
                }
            }  
            Console.WriteLine("任务完成");
            Console.ReadKey();
        }
    }
}
