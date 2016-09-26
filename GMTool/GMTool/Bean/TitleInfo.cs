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
using GMTool.Extensions;
using GMTool.Enums;

namespace GMTool.Bean
{
	public class TitleInfo
	{
		public long TitleID;
		/// <summary>
		/// 名字
		/// </summary>
		public string Name;
		public string Description;
		public string Category;
		public int AutoGiveLevel;
		public int RequiredLevel;
		public int ClassRestriction;
        public string Feature;
        public ClassInfo OnlyClass;
		/// <summary>
		/// 属性
		/// </summary>
		public Dictionary<string, int> Stats;
		
		public string Effect;
		
		public TitleInfo()
		{
			Stats=new Dictionary<string, int>();
		}
		
		public string ToLineString(){
			return "lv." + RequiredLevel + " " + Name+" ("+Effect+")";
		}
		public override string ToString()
		{
			return Name+(OnlyClass!=ClassInfo.UnKnown? "[" + OnlyClass.Name()+"专属]":"")+
                "\n"+Description+"\n等级限制："+RequiredLevel+"\n职业限制："+ClassInfoEx.GetClassText(ClassRestriction)+"\n"+Effect;
		}

	}
}
