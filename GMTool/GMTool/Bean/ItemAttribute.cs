/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/21
 * 时间: 15:46
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using GMTool.Enums;

namespace GMTool.Bean
{
	/// <summary>
	/// Description of ItemAttribute.
	/// </summary>
	public class ItemAttribute
	{
		public ItemAttributeType Type{get;set;}
		public string Value{get;set;}
		public string Arg{get;set;}
		public string Arg2{get;set;}
		public ItemAttribute()
		{
		}
		public override string ToString()
		{
			return base.ToString();
		}
	}
}
