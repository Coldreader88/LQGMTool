﻿/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/30
 * 时间: 14:49
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Text;
using Vindictus.Extensions;
using System.Data.Common;
using Vindictus.Helper;

namespace Vindictus.Bean
{
	/// <summary>
	/// Description of ItemStatInfo.
	/// </summary>
	public class ItemStatInfo : StatInfo
	{
		public int ATK;
		public int ATK_Speed;
		public int Critical;
		public int Balance;
		public int MATK;
		public int DEF;
		public int Res_Critical;
		public int PVP_ATK;
		public int PVP_MATK;
		public int PVP_DEF;
		public int TOWN_SPEED;
		
		public ItemStatInfo()
		{
		}
		public ItemStatInfo(ItemStatInfo info)
		{
			this.ATK=info.ATK;
			this.ATK_Speed=info.ATK_Speed;
			this.Critical=info.Critical;
			this.Balance=info.Balance;
			this.MATK=info.MATK;
			this.DEF=info.DEF;
			this.Res_Critical=info.Res_Critical;
			this.PVP_ATK=info.PVP_ATK;
			this.PVP_MATK=info.PVP_MATK;
			this.PVP_DEF=info.PVP_DEF;
			this.TOWN_SPEED=info.TOWN_SPEED;
		}
		public ItemStatInfo(DbDataReader reader):base(reader){
			this.ATK = reader.ReadInt32("ATK");
			this.ATK_Speed = reader.ReadInt32("ATK_Speed");
			this.Critical = reader.ReadInt32("Critical");
			this.Balance = reader.ReadInt32("Balance");
			this.MATK = reader.ReadInt32("MATK");
			this.DEF = reader.ReadInt32("DEF");
			this.Res_Critical = reader.ReadInt32("Res_Critical");
			this.PVP_ATK = reader.ReadInt32("PVP_ATK");
			this.PVP_MATK = reader.ReadInt32("PVP_MATK");
			this.PVP_DEF = reader.ReadInt32("PVP_DEF");
			this.TOWN_SPEED = reader.ReadInt32("TOWN_SPEED");
		}
		public new bool IsEmpty(){
			bool b = (ATK != 0 || ATK_Speed !=0 || Critical!=0||Balance!=0||MATK!=0
			          ||DEF!=0||Res_Critical!=0||PVP_ATK!=0||PVP_MATK!=0||PVP_DEF!=0||TOWN_SPEED!=0);
			if(b)return false;
			if(!base.IsEmpty())return false;
			return  true;
		}
		public override string ToString()
		{
			StringBuilder sb=new StringBuilder();
			if(ATK>0)
				sb.Append("\n"+"ATK".StatName()+":"+ATK);
			if(ATK_Speed>0)
				sb.Append("\n"+"ATK_Speed".StatName()+":"+ATK_Speed);
			if(Critical>0)
				sb.Append("\n"+"Critical".StatName()+":"+Critical);
			if(Balance>0)
				sb.Append("\n"+"Balance".StatName()+":"+Balance);
			if(MATK>0)
				sb.Append("\n"+"MATK".StatName()+":"+MATK);
			if(DEF>0)
				sb.Append("\n"+"DEF".StatName()+":"+DEF);
			if(Res_Critical>0)
				sb.Append("\n"+"Res_Critical".StatName()+":"+Res_Critical);
			if(!base.IsEmpty())
				sb.Append(base.ToString());
			if(PVP_ATK>0)
				sb.Append("\n"+"PVP_ATK".StatName()+":"+PVP_ATK);
			if(PVP_MATK>0)
				sb.Append("\n"+"PVP_MATK".StatName()+":"+PVP_MATK);
			if(PVP_DEF>0)
				sb.Append("\n"+"PVP_DEF".StatName()+":"+PVP_DEF);
			if(TOWN_SPEED>0)
				sb.Append("\n"+"TOWN_SPEED".StatName()+":+"+TOWN_SPEED+"%");
			return sb.ToString();
		}

	}
}
