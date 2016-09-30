using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMTool.Enums;
using GMTool.Extensions;
using System.Data.Common;
using GMTool.Helper;

namespace GMTool.Bean
{
	public class ItemClassInfo
	{
		public string ItemClass{get;private set;}
		/// <summary>
		/// 子目录
		/// </summary>
		public SubCategory SubCategory{get;private set;}
		/// <summary>
		/// 主分类
		/// </summary>
		public MainCategory MainCategory{get;private set;}
		/// <summary>
		/// 等级需求
		/// </summary>
		public int RequiredLevel{get;private set;}
		/// <summary>
		/// 职业限制
		/// </summary>
		public int ClassRestriction{get;private set;}

		public string Name{get;private set;}

		public string Desc{get;private set;}
		public ItemStatInfo Stat{get;private set;}
		public long MaxStack{get;private set;}
		public ItemClassInfo(){
			
		}
		public ItemClassInfo(DbDataReader reader,HeroesTextHelper HeroesText){
			this.ItemClass = reader.ReadString("ItemClass", "").ToLower();
			this.SubCategory = reader.ReadEnum<SubCategory>("Category", SubCategory.NONE);
			this.MainCategory= reader.ReadEnum<MainCategory>("TradeCategory", MainCategory.NONE);
			this.MaxStack =reader.ReadInt64("MaxStack");
			this.RequiredLevel = reader.ReadInt32("RequiredLevel");
			this.ClassRestriction = reader.ReadInt32("ClassRestriction");
			string tmp;
			if(HeroesText.ItemNames.TryGetValue(this.ItemClass, out tmp)){
				this.Name = tmp;
			}
			if(HeroesText.ItemDescs.TryGetValue(this.ItemClass, out tmp)){
				this.Desc = tmp;
			}
			
			ItemStatInfo stat=new ItemStatInfo(reader);
			if(!stat.IsEmpty()){
				this.Stat = stat;
			}
		}
		public override string ToString()
		{
			return "物品ID："+ ItemClass+"\n物品名字："+Name+" \n需求等级： "+ RequiredLevel +"\n最大叠放数量："+(MaxStack<0?"不限":""+MaxStack)+"\n分类："+ SubCategory.Name()+ "  "+MainCategory.Name()
				+ "\n职业限制："+ClassInfoEx.GetClassText(ClassRestriction)+(Stat==null?"":"\n"+Stat.ToString()) + "\n" + Desc;
		}
	}
}
