/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/28
 * 时间: 15:47
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Text;
using System.Runtime.InteropServices;

namespace System.Win32
{
	public static class User32
	{
		public static readonly UInt32 GW_HWNDNEXT = 2;
		
		[DllImport("user32", EntryPoint = "FindWindow", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32", EntryPoint = "FindWindowEx", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32", EntryPoint = "ShowWindow", SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

		[DllImport("user32.dll")]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		
		[DllImport("user32.dll")]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
		
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindow(IntPtr hWnd, UInt32 uCmd);
		[DllImport("user32.dll")]
		public static extern IntPtr GetTopWindow(IntPtr hWnd);
		
		public static IntPtr FindConsoleWindow(string title)
		{
			return FindWindow("ConsoleWindowClass", title);
		}

		public static IntPtr GetWindowByPid(Int32 pID)
		{
			IntPtr h = GetTopWindow(IntPtr.Zero);
			while (h != IntPtr.Zero)
			{
				UInt32 newID;
				GetWindowThreadProcessId(h, out newID);
				if (newID == pID)
				{
					return h;
				}
				
				h = GetWindow(h, GW_HWNDNEXT);
			}
			return h;
		}
		public static bool ShowWindow(IntPtr hWnd)
		{
			return ShowWindow(hWnd, 1);
		}
		public static bool HideWindow(IntPtr hWnd)
		{
			return ShowWindow(hWnd, 0);
		}
	}
}
