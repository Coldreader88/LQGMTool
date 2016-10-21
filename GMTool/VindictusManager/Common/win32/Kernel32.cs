using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Win32
{
    public static class Kernel32
    {
		[DllImport("kernel32")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        public static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

        public const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        public const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        public const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);
    }
}
