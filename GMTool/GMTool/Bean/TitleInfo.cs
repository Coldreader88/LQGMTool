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
using System.Data.Common;
using GMTool.Helper;

namespace GMTool.Bean
{
	public class TitleInfo
	{
		public long TitleID;
		/// <summary>
		/// 名字
		/// </summary>
		public string Name{get;private set;}
		public string Description{get;private set;}
		public string Category{get;private set;}
		public int AutoGiveLevel{get;private set;}
		public int RequiredLevel{get;private set;}
		public int ClassRestriction{get;private set;}
		public string Feature{get;private set;}
		public int TotalCount{get;private set;}
		public ClassInfo OnlyClass{get;private set;}
		/// <summary>
		/// 属性
		/// </summary>
		public Dictionary<string, int> Stats{get;private set;}
		
		public string Effect{get;private set;}
		public TitleInfo(){
			
		}
		public TitleInfo(DbDataReader reader,HeroesTextHelper HeroesText)
		{
			Stats=new Dictionary<string, int>();
			this.TitleID = reader.ReadInt64("titleid");// Convert.ToInt64(ToString(reader["titleid"]));
			string tmp;
			this.Name = reader.ReadString("name","");
			if(HeroesText.TitleNames.TryGetValue(this.Name.ToLower(), out tmp)){
				this.Name =tmp;
			}
			this.Description = reader.ReadString("description","");
			if(HeroesText.TitleDescs.TryGetValue(this.Description.ToLower(), out tmp)){
				this.Description =tmp;
			}
			this.Category = reader.ReadString("Category");
			this.Feature = reader.ReadString("feature","");
			this.OnlyClass = this.Feature.ToClassInfo();
			this.AutoGiveLevel = reader.ReadInt32("AutoGiveLevel");
			this.RequiredLevel = reader.ReadInt32("RequiredLevel");
			this.ClassRestriction = reader.ReadInt32("ClassRestriction",-1);
			this.TotalCount = reader.ReadInt32("tcount", -1);
			this.Effect="";
		}
		
		public void UpdateEffect(DbDataReader reader){
			string stat = reader.ReadString("Stat","");
			int val = reader.ReadInt32("Amount");
			int tmp = 0;
			if(!this.Stats.TryGetValue(stat, out tmp)){
				this.Stats.Add(stat, val);
				string name = stat.StatName();
				this.Effect +=name+"+"+val+",";
			}
		}
		public void Trim(){
			if(this.Effect.EndsWith(",")){
					this.Effect = this.Effect.Substring(0, this.Effect.Length-1);
				}
		}
		public string ToShortString(){
			return "lv." + RequiredLevel + " " + Name;
		}
		public string ToLineString(){
			return "lv." + RequiredLevel + " " + Name+" ("+Effect+")";
		}
		public override string ToString()
		{
			return "("+TitleID+")"+Name+(OnlyClass!=ClassInfo.UnKnown? "[" + OnlyClass.Name()+"专属]":"")+
				"\n"+Description
//				+"\n等级限制："+RequiredLevel+"\n职业限制："+ClassInfoEx.GetClassText(ClassRestriction)
				+"\n"+Effect;
		}

	}
}
