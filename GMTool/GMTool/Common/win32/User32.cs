﻿/*
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
		
		//WM_KEYDOWN 按下一个键
		public const Int32 WM_KEYDOWN = 0x0100;
		//释放一个键
		public const Int32 WM_KEYUP = 0x0101;
		
		[DllImport("user32", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32",SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32" ,SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hWnd, UInt32 nCmdShow);
		//发送按键
		[DllImport("user32", EntryPoint = "SendMessage")]
		static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);
		
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
		
		public static void SendKey(IntPtr window, int key){
			if(window != IntPtr.Zero){
				SendMessage(window, WM_KEYDOWN, key, 0);
				SendMessage(window, WM_KEYUP, key, 0);
			}
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
	}
	#region keys
	public enum WinKeys : int{
		VK_LBUTTON=0x01,//	1	鼠标的左键
		VK_RBUTTON=	0x02,//	2	鼠标的右键
		VK_CANCEL=	0x03,//	3	Ctrl+Break(通常不需要处理)
		VK_MBUTTON=	0x04,//	4	鼠标的中键（三按键鼠标)
		VK_BACK=	0x08,//	8	Backspace键
		VK_TAB	=0x09,//	9	Tab键
		VK_CLEAR	=0x0C,//	12	Clear键（Num Lock关闭时的数字键盘5）
		VK_RETURN	=0x0D,//	13	Enter键
		VK_SHIFT	=0x10,//	16	Shift键
		VK_CONTROL=	0x11,//	17	Ctrl键
		VK_MENU	=0x12,//	18	Alt键
		VK_PAUSE	=0x13,//	19	Pause键
		VK_CAPITAL=	0x14,//	20	Caps Lock键
		VK_ESCAPE	=0x1B,//	27	Ese键
		VK_SPACE	=0x20,//	32	Spacebar键
		VK_PRIOR=	0x21,//	33	Page Up键
		VK_NEXT=	0x22,//	34,//	Page Domw键
		VK_END	=0x23,//,//	35	End键
		VK_HOME=	0x24,//	36	Home键
		VK_LEFT=	0x25,//	37	LEFT ARROW 键(←)
		VK_UP	=0x26,//	38	UP ARROW键(↑)
		VK_RIGHT	=0x27,//	39	RIGHT ARROW键(→)
		VK_DOWN =	0x28,//	40	DOWN ARROW键(↓)
		VK_Select	=0x29,//	41	Select键
		VK_PRINT	=0x2A,//	42
		VK_EXECUTE	=0x2B,//	43	EXECUTE键
		VK_SNAPSHOT=	0x2C,//	44	Print Screen键（抓屏）
		VK_Insert	=0x2D,//	45	Ins键(Num Lock关闭时的数字键盘0)
		VK_Delete	=0x2E,//	46	Del键(Num Lock关闭时的数字键盘.)
		VK_HELP	=0x2F,//	47	Help键
		VK_0	=0x30,//	48	0键
		VK_1	=0x31,//	49	1键
		VK_2	=0x32,//	50	2键
		VK_3	=0x33,//	51	3键
		VK_4	=0x34,//	52	4键
		VK_5	=0x35,//	53	5键
		VK_6	=0x36,//	54	6键
		VK_7	=0x37,//	55	7键
		VK_8	=0x38,//	56	8键
		VK_9	=0x39,//	57	9键
		VK_A	=0x41,//	65	A键
		VK_B	=0x42,//	66	B键
		VK_C	=0x43,//	67	C键
		VK_D	=0x44	,//68	D键
		VK_E	=0x45	,//69	E键
		VK_F	=0x46,//	70	F键
		VK_G	=0x47	,//71	G键
		VK_H	=0x48	,//72	H键
		VK_I	=0x49	,//73	I键
		VK_J	=0x4A	,//74	J键
		VK_K	=0x4B	,//75	K键
		VK_L	=0x4C	,//76	L键
		VK_M	=0x4D	,//77	M键
		VK_N	=0x4E	,//78	N键
		VK_O	=0x4F	,//79	O键
		VK_P	=0x50	,//80	P键
		VK_Q	=0x51	,//81	Q键
		VK_R	=0x52	,//82	R键
		VK_S	=0x53	,//83	S键
		VK_T	=0x54	,//84	T键
		VK_U	=0x55	,//,//85	U键
		VK_V	=0x56	,//86	V键
		VK_W	=0x57	,//87	W键
		VK_X	=0x58	,//88	X键
		VK_Y	=0x59	,//89	Y键
		VK_Z	=0x5A	,//90	Z键
		VK_NUMPAD0	=0x60,//	96	数字键0键
		VK_NUMPAD1	=0x61	,//97	数字键1键
		VK_NUMPAD2	=0x62	,//98	数字键2键
		VK_NUMPAD3	=0x62	,//99	数字键3键
		VK_NUMPAD4	=0x64	,//100	数字键4键
		VK_NUMPAD5	=0x65	,//101	数字键5键
		VK_NUMPAD6	=0x66	,//102	数字键6键
		VK_NUMPAD7	=0x67	,//103	数字键7键
		VK_NUMPAD8	=0x68	,//104	数字键8键
		VK_NUMPAD9	=0x69	,//105	数字键9键
		VK_MULTIPLY	=0x6A	,//106	数字键盘上的*键
		VK_ADD	=0x6B	,//107	数字键盘上的+键
		VK_SEPARATOR	=0x6C,//	108	Separator键
		VK_SUBTRACT	=0x6D,//,//	109	数字键盘上的-键
		VK_DECIMAL	=0x6E,//	110	数字键盘上的.键
		VK_DIVIDE	=0x6F,//	111	数字键盘上的/键
		VK_F1	=0x70,//	112	F1键
		VK_F2	=0x71,//	113	F2键
		VK_F3	=0x72,//	114	F3键
		VK_F4	=0x73,//	115	F4键
		VK_F5	=0x74,//	116	F5键
		VK_F6	=0x75,//	117	F6键
		VK_F7	=0x76,//	118	F7键
		VK_F8	=0x77,//	119	F8键
		VK_F9	=0x78,//	120	F9键
		VK_F10	=0x79,//	121	F10键
		VK_F11	=0x7A,//	122	F11键
		VK_F12	=0x7B,//	123	F12键
		VK_NUMLOCK	=0x90,//	144	Num Lock 键
		VK_SCROLL	=0x91,//	145	Scroll Lock键
		VK_RWIN		=0x92,//	右win键
		VK_APPS		=0x93,//	快捷菜单
	}
	#endregion
}