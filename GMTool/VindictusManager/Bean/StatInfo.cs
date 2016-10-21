/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/30
 * 时间: 14:49
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Text;
using Vindictus.Helper;
using Vindictus.Extensions;
using System.Data.Common;

namespace Vindictus.Bean
{
	public class StatInfo
	{
		public int STR{get;private set;}
		public int DEX{get;private set;}
		public int INT{get;private set;}
		public int WILL{get;private set;}
		public int LUCK{get;private set;}
		public int HP{get;private set;}
		public int STAMINA{get;private set;}
		public StatInfo(){
			
		}
		public StatInfo(DbDataReader reader){
			this.STR = reader.ReadInt32("STR");
			this.DEX = reader.ReadInt32("DEX");
			this.INT = reader.ReadInt32("INT");
			this.WILL = reader.ReadInt32("WILL");
			this.LUCK = reader.ReadInt32("LUCK");
			this.HP = reader.ReadInt32("HP");
			this.STAMINA = reader.ReadInt32("STAMINA");
		}
		public bool IsEmpty(){
			return !(STR != 0 || DEX !=0 || INT!=0||WILL!=0||LUCK!=0
			         ||HP!=0||STAMINA!=0);
		}
		public override string ToString()
		{
			StringBuilder sb=new StringBuilder();
			if(STR>0)
				sb.Append("\n"+"STR".StatName()+":\t"+STR);
			if(DEX>0)
				sb.Append("\n"+"DEX".StatName()+":\t"+DEX);
			if(INT>0)
				sb.Append("\n"+"INT".StatName()+":\t"+INT);
			if(WILL>0)
				sb.Append("\n"+"WILL".StatName()+":\t"+WILL);
			if(LUCK>0)
				sb.Append("\n"+"LUCK".StatName()+":\t"+LUCK);
			if(HP>0)
				sb.Append("\n"+"HP".StatName()+":\t"+HP);
			if(STAMINA>0)
				sb.Append("\n"+"STAMINA".StatName()+":\t"+STAMINA);
			return sb.ToString();
		}

	}
}
