/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/30
 * 时间: 17:43
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;

namespace GMTool.Common
{
	public class DListView : ListView
	{
		public DListView()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer |
			         ControlStyles.AllPaintingInWmPaint,
			         true);
			UpdateStyles();
		}
	}
}
