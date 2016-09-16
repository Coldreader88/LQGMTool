using GMTool.win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMTool.Helper
{
    public class TextHelper
    {
        /// <summary>
        /// 将字符转换成简体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToSimplified(string source)
        {
            String target = new String(' ', source.Length);
            int ret = Kernel32.LCMapString(Kernel32.LOCALE_SYSTEM_DEFAULT, Kernel32.LCMAP_SIMPLIFIED_CHINESE, source, source.Length, target, source.Length);
            return target;
        }

        /// <summary>
        /// 讲字符转换为繁体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToTraditional(string source)
        {
            String target = new String(' ', source.Length);
            int ret = Kernel32.LCMapString(Kernel32.LOCALE_SYSTEM_DEFAULT, Kernel32.LCMAP_TRADITIONAL_CHINESE, source, source.Length, target, source.Length);
            return target;
        }
    }
}
