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
        	if(args.Length ==0){
        		Console.WriteLine("请拖一个db3文件到本程序的exe上面，打开本程序。");
        		Console.ReadKey();
        		return;
        	}
        	string path = args[0];
            Console.WriteLine("使用前请做好备份，CustomizePriceInfo和CustomizeItemInfo");
            Console.WriteLine("国服请输入g，台服请输入t");
            var key = Console.ReadKey();
            string country = "zh-CN";
            Console.WriteLine();
            if(key.Key == ConsoleKey.G){
            	 Console.WriteLine("你输入的是国服，如果不对，请关闭再试");
            }else{
            	 Console.WriteLine("你输入的是台服，如果不对，请关闭再试");
            	 country = "zh-TW";
            }
             Console.ReadKey();
            //select * from customizeiteminfo where (Feature ISNULL or Feature = 'zh-CN' or Feature='') order by category ,"order"
            //
          
            SQLiteHelper db = new SQLiteHelper(path);
            if (!db.Open())
            {
                Console.WriteLine("无法打开数据库");
                Console.ReadKey();
                return;
            }
            
            List<BasePatch> patchs = new ArrayList<BasePatch>();

            //时装
           // patchs.Add(new AvatarPatch(db, country));
            //发型
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Fiona, country));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Evy, country));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Lynn, country));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Vella, country));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Arisha, country));
            patchs.Add(new SignlePatch(db, Category.HAIR, ClassInfo.Delia, country));
            //眉毛
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Fiona, country));
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Evy, country));
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Lynn, country));
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Vella, country));
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Arisha, country));
            patchs.Add(new SignlePatch(db, Category.EYEBROW, ClassInfo.Delia, country));
            //妆容
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Fiona, country));
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Evy, country));
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Lynn, country));
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Vella, country));
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Arisha, country));
            patchs.Add(new SignlePatch(db, Category.MAKEUP, ClassInfo.Delia, country));
            //内衣
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Fiona, country));
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Evy, country));
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Lynn, country));
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Vella, country));
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Arisha, country));
            patchs.Add(new SignlePatch(db, Category.INNERARMOR, ClassInfo.Delia, country));
            //价格必须放在最后
            patchs.Add(new PircePatch(db, country));
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
