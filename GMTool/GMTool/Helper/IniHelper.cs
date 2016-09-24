using System.Win32;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GMTool.Helpers
{
	internal class IniHelper
	{
		private string sPath;

		public IniHelper(string path)
		{
			this.sPath = path;
		}

		public string ReadValue(string section, string key,string def="")
		{
			StringBuilder retVal = new StringBuilder(0xff);
			Kernel32.GetPrivateProfileString(section, key, def, retVal, 0xff, this.sPath);
			return retVal.ToString();
		}
		public int ReadInt(string section, string key)
		{
			StringBuilder retVal = new StringBuilder(0xff);
			Kernel32.GetPrivateProfileString(section, key, "", retVal, 0xff, this.sPath);
			return Convert.ToInt32(retVal.ToString());
		}
		public void WriteValue(string section, string key, string value,string def="")
		{
			Kernel32.WritePrivateProfileString(section, key, value, this.sPath);
		}
	}
}

