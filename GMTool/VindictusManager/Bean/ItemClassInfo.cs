using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vindictus.Enums;
using Vindictus.Extensions;
using System.Data.Common;
using Vindictus.Helper;

namespace Vindictus.Bean
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
        public string Feature { get; private set; }
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
            this.Feature = reader.ReadString("Feature");
        }
		public override string ToString()
		{
			return "ID："+ ItemClass+"\n"+R.ItemName+"："+Name
				+" \n"+R.ItemRequireLevel+"： "+ RequiredLevel
                +"\n"+R.ItemMaxStack+"："+(MaxStack<0?R.UnLimit:""+MaxStack)
                +"\n"+R.Category+"："+ SubCategory.Name()+ "  "+MainCategory.Name()
				+ "\n"+R.Feature+":"+Feature
				+"\n"+R.ClassRestriction+"：" +ClassInfoEx.GetClassText(ClassRestriction)  +(Stat==null?"":"\n"+Stat.ToString()) 
                + "\n" + Desc;
		}
	}
}
