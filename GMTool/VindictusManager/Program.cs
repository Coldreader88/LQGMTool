/*
 * 由SharpDevelop创建。
 * 用户： keyoyu
 * 日期: 2016/10/21
 * 时间: 14:40
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using System.Xml;
using Vindictus.Helper;

namespace Vindictus
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		public static CoreConfig Config;
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Config = XmlHelper.DeserializeByFile<CoreConfig>(args.Length>0?args[1]:"config.xml");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
	}
}
