using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GMTool.Extensions;
using System.Data.Common;
using GMTool.Helper;

namespace GMTool.Bean
{
	public class EnchantInfo
	{
		public string Class{get;private set;}
		public string Name{get;private set;}
		public string Constraint{get;private set;}
		public string Desc{get;private set;}
		public string Effect{get;private set;}
		public bool IsPrefix{get;private set;}
		public int EnchantLevel{get;private set;}
		public int MinArg{get;private set;}
		public int MaxArg{get;private set;}
		
		public EnchantInfo(DbDataReader reader2,HeroesTextHelper HeroesText){
			this.Class =reader2.ReadString("EnchantClass","").ToLower();
			this.Constraint = reader2.ReadString("ItemConstraint");
			this.Desc = reader2.ReadString("ItemConstraintDesc");
			this.IsPrefix = reader2.ReadBoolean("IsPrefix");
			this.MinArg = reader2.ReadInt32("MinArgValue");
			this.MaxArg = reader2.ReadInt32("MaxArgValue");
			this.EnchantLevel = reader2.ReadInt32("EnchantLevel");
			string tmp;
			if (this.IsPrefix)
			{
				if(!HeroesText.PrefixNames.TryGetValue(this.Class, out tmp)){
					this.Name = this.Class;
				}else{
					this.Name = tmp;
				}
			}
			else
			{
				if(!HeroesText.SuffixNames.TryGetValue(this.Class, out tmp)){
					this.Name = this.Class;
				}else{
					this.Name = tmp;
				}
			}
			if(HeroesText.EnchantDescs.TryGetValue(this.Class, out tmp)){
				this.Desc = tmp;
			}
			this.Effect = "";
			bool has= false;
			for (int i = 1; i <= 5; i++)
			{
                has = false;
                if (HeroesText.EnchantEffects.TryGetValue(this.Class + "_" + i, out tmp))
				{
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        has = true;
                        this.Effect += i + "." + tmp;
                    }
				}
				if (has)
				{
					if (HeroesText.EnchantEffectIfs.TryGetValue(this.Class + "_" + i, out tmp))
					{
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            this.Effect += "[" + tmp + "]";
                        }
					}
					this.Effect += "\n";
				}
				else
				{
					break;
				}
			}
			if (this.Effect.Contains("{0}"))
			{
				this.Effect = this.Effect.Replace("{0}", this.GetValue());
			}
		}
		
		public EnchantInfo(){
			
		}
		
		public string GetValue()
		{
			return MinArg+"-"+MaxArg;
		}
		public override string ToString()
		{
			return Name+ "("+Class+")\n" +Desc + "\n" + Effect;
		}
	}
}
