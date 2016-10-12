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
		public static IntPtr ConsoleWindow;
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if(args!=null && args.Length>0){
				new Command(args).run();
				Console.WriteLine("按任意键退出");
				Console.ReadKey();
				return;
			}
			#if !DEBUG
			ConsoleWindow = User32.FindWindow(null, Console.Title);
			if(ConsoleWindow != IntPtr.Zero){
				User32.ShowWindowAsync(ConsoleWindow, User32.SW_HIDE);
			}
			#endif
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
