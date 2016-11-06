using System;
using System.Data.Common;
using Vindictus.Enums;
using Vindictus.Helper;

namespace Vindictus.Bean
{
	public class ItemAttribute
	{
		public ItemAttributeType Type{get; private set;}
		public string Value{get; private set;}
		public int Arg{get; private set;}
		public int Arg2{get; private set;}
		public string Desc{get;private set;}
		public ItemAttribute(DbDataReader reader, DataHelper datahelper){
			this.Type = reader.ReadEnum<ItemAttributeType>("Attribute",ItemAttributeType.NONE);
			this.Value = reader.ReadString("Value");
			this.Arg = reader.ReadInt32("Arg");
			this.Arg2 = reader.ReadInt32("Arg2");
			this.Desc = getDesc(datahelper);
		}
		public ItemAttribute(){
			
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
		public ItemAttribute SetArg2(int arg){
			this.Arg2=arg;
			return this;
		}
		public ItemAttribute SetArg(int arg){
			this.Arg=arg;
			return this;
		}
		private string getDesc(DataHelper datahelper){
			string _Desc;
			if (Type == ItemAttributeType.ENHANCE)
			{
				_Desc= R.Enhance+":" + Value;
			}
			else if (Type == ItemAttributeType.PREFIX)
			{
				EnchantInfo einfo = datahelper.GetEnchant(Value);
				_Desc= R.PrefixEnchant+":" + (einfo == null ? Value : einfo.ToString());
			}
			else if (Type == ItemAttributeType.SUFFIX)
			{
				EnchantInfo einfo = datahelper.GetEnchant(Value);
				_Desc= R.SuffixEnchant+":" + (einfo == null ? Value : einfo.ToString());
			}
			else if (Type == ItemAttributeType.QUALITY)
			{
				_Desc= R.Star+":" + Arg;
			}else if (Type == ItemAttributeType.SYNTHESISGRADE)
			{
				_Desc= R.Score+":";
				if (!string.IsNullOrEmpty(Value))
				{
					string[] strs = Value.Split('/');
					if (strs.Length > 1)
					{
						int i = Convert.ToInt32(strs[1]);
						SkillBonusInfo info = datahelper.getSkillBonusInfo(i);
						if (info != null)
						{
							_Desc += info.GetKey()+" "+info.DESC;
						}
					}
				}else{
					_Desc += Value;
				}
			}
			else{
				_Desc = Type+":"+Value+" "+Arg+","+Arg2;
			}
			return _Desc;
		}
		public override string ToString()
		{
			return Desc;
		}
	}
}
