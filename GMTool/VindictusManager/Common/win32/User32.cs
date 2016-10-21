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
		public const UInt32 SW_HIDE = 0;
		public const UInt32 SW_SHOWNORMAL = 1;
		public const UInt32 SW_NORMAL = 1;
		public const UInt32 SW_SHOWMINIMIZED = 2;
		public const UInt32 SW_SHOWMAXIMIZED = 3;
		public const UInt32 SW_MAXIMIZE = 3;
		public const UInt32 SW_SHOWNOACTIVATE = 4;
		public const UInt32 SW_SHOW = 5;
		public const UInt32 SW_MINIMIZE = 6;
		public const UInt32 SW_SHOWMINNOACTIVE = 7;
		public const UInt32 SW_SHOWNA = 8;
		public const UInt32 SW_RESTORE = 9;
		
		public const UInt32 GW_HWNDNEXT = 2;
		
		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32",SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32" ,SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hWnd, UInt32 nCmdShow);
		
		[DllImport("User32")]
		public static extern bool ShowWindowAsync(IntPtr hWnd, UInt32 cmdShow);
		
		[DllImport("user32")]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		
		[DllImport("user32")]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32")]
		public static extern Int32 GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
		
		[DllImport("user32")]
		public static extern IntPtr GetWindow(IntPtr hWnd, UInt32 uCmd);
		[DllImport("user32")]
		public static extern IntPtr GetTopWindow(IntPtr hWnd);
		[DllImport("User32")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		
		
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
	}
}
