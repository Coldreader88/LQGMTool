/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/27
 * 时间: 16:26
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using ServerManager.Comon;
using GMTool.Helper;
using System.IO;

namespace ServerManager
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
			//new XmlHelper(@"D:\heroes\server\Bin\ServiceCore.dll.config").ModConnectStrings("127.0.0.1,1433","sa","123456");
//			MSSqlHelper helper=new MSSqlHelper("127.0.0.1,1433",null);
//			//Console.WriteLine(Path.GetFileNameWithoutExtension(@"D:\heroes\EN\database\heroes.bak"));
//			if(helper.Open()){
//				try{
//				    //helper.RestoreOrCreate(@"D:\heroes\EN\database\heroes.bak");
//				  //  helper.SplitDataBase("heroes");
//				   helper.AttachDataBase("heroes", @"D:\heroes\EN\database\heroes.mdf");
//				}catch(Exception e){
//					Console.WriteLine(e.ToString());
//				}
//			}
//			Console.ReadKey();
		}
		
	}
}
