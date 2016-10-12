using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillPatch
{
    class Program
    {
        static void Main(string[] args)
        {
            FilePatch patch = new FilePatch(
                @"D:\Program Files (x86)\zh-CN\data\", 
                @"D:\Program Files (x86)\zh-CN\data\models\player\_anim",
                ".txt", 
                ".efx",
                @"H:\洛奇\洛英文件提取工具台服\.efx");
            patch.Patch();
            Console.WriteLine("任务完成");
            Console.ReadKey();
        }
    }
}
