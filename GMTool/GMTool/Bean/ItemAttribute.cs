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
using GMTool.Helper;

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
		
		private string _Desc;
		
		public ItemAttribute()
		{
		}
		public override string ToString()
		{
			if(!string.IsNullOrEmpty(_Desc)){
				return _Desc;
			}
			if (Type == ItemAttributeType.ENHANCE)
			{
				_Desc= "强化：" + Value;
			}
			else if (Type == ItemAttributeType.PREFIX)
			{
				EnchantInfo einfo = DbInfoHelper.Get().GetEnchant(Value);
				_Desc= "【字首】附魔：" + (einfo == null ? Value : einfo.ToString());
			}
			else if (Type == ItemAttributeType.SUFFIX)
			{
				EnchantInfo einfo = DbInfoHelper.Get().GetEnchant(Value);
				_Desc= "【字尾】附魔：" + (einfo == null ? Value : einfo.ToString());
			}
			else if (Type == ItemAttributeType.QUALITY)
			{
				_Desc= "品质：" + Arg;
			}else{
				_Desc= Type+":"+Value;
			}
			return _Desc;
		}
	}
}
