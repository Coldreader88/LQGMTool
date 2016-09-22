/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/22
 * 时间: 9:49
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;

namespace GMTool.Bean
{
	public class TitleInfo
	{
		public long TitleID;
		/// <summary>
		/// 名字
		/// </summary>
		public string  Description;
		public int TargetCount;
		public bool IsParty;
		public string Category;
		public int AutoGiveLevel;
		public int RequiredLevel;
		public int ClassRestriction;
		/// <summary>
		/// 属性
		/// </summary>
		public Dictionary<string, int> Stats;
		
		public TitleInfo()
		{
			Stats=new Dictionary<string, int>();
		}
		public override string ToString()
		{
			return Description;
		}

	}
}
