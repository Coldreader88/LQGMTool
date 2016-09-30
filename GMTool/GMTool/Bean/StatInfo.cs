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
using GMTool.Helper;
using GMTool.Extensions;
using System.Data.Common;

namespace GMTool.Bean
{
	public class StatInfo
	{
		public int STR;
		public int DEX;
		public int INT;
		public int WILL;
		public int LUCK;
		public int HP;
		public int STAMINA;
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
				sb.Append("\n"+"STR".StatName()+":"+STR);
			if(DEX>0)
				sb.Append("\n"+"DEX".StatName()+":"+DEX);
			if(INT>0)
				sb.Append("\n"+"INT".StatName()+":"+INT);
			if(WILL>0)
				sb.Append("\n"+"WILL".StatName()+":"+WILL);
			if(LUCK>0)
				sb.Append("\n"+"LUCK".StatName()+":"+LUCK);
			if(HP>0)
				sb.Append("\n"+"HP".StatName()+":"+HP);
			if(STAMINA>0)
				sb.Append("\n"+"STAMINA".StatName()+":"+STAMINA);
			return sb.ToString();
		}

	}
}
