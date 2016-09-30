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
using GMTool.Extensions;
using System.Data.Common;

namespace GMTool.Bean
{
	/// <summary>
	/// Description of ItemAttribute.
	/// </summary>
	public class ItemAttribute
	{
		public ItemAttributeType Type{get; private set;}
		public string Value{get; private set;}
		public int Arg{get; private set;}
		public int Arg2{get; private set;}
		
		private string _Desc;
		public ItemAttribute(DbDataReader reader){
			this.Type = reader.ReadEnum<ItemAttributeType>("Attribute",ItemAttributeType.NONE);
			this.Value = reader.ReadString("Value");
			this.Arg = reader.ReadInt32("Arg");
			this.Arg2 = reader.ReadInt32("Arg2");
		}
		public ItemAttribute(ItemAttributeType type,string val)
		{
			this.Type = type;
			this.Value =val;
		}
		public ItemAttribute(ItemAttributeType type,int arg)
		{
			this.Type = type;
			this.Arg =arg;
		}
		public ItemAttribute()
		{
		}
		public ItemAttribute SetArg2(int arg){
			this.Arg2=arg;
			return this;
		}
		public ItemAttribute SetArg(int arg){
			this.Arg=arg;
			return this;
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
				_Desc= "星数：" + Arg;
			}else if (Type == ItemAttributeType.SYNTHESISGRADE)
			{
				_Desc= "评分：" + Value;
                if (!string.IsNullOrEmpty(Value))
                {
                    string[] strs = Value.Split('/');
                    if (strs.Length > 1)
                    {
                        int i = Convert.ToInt32(strs[1]);
                        SkillBonusInfo info = new SkillBonusInfo();
                        if (DbInfoHelper.Get().SynthesisSkillBonues.TryGetValue(i, out info))
                        {
                            _Desc = "评分：" + info.GetKey()+" "+info.DESC;
                        }
                    }
                }
            }
            else if (Type == ItemAttributeType.VALUE){
                _Desc = "";
                
			}
			return _Desc;
		}
	}
}
