using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Win32;

namespace GMTool
{
	static class Program
	{
		public static String INT_FILE =　Application.StartupPath + "/DBIni.ini";
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
