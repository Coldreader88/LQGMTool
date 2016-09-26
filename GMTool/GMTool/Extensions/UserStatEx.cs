/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/26
 * 时间: 16:54
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using GMTool.Enums;
using GMTool.Helper;
using GMTool.Bean;

namespace GMTool.Extensions
{
	/// <summary>
	/// Description of UserStatEx.
	/// </summary>
	public static class UserStatEx
	{
		public static string Name(this UserStat stat)
		{
			if(stat== UserStat.LUCK){
				return "幸运";
			}
			string val;
			if(DbInfoHelper.Get().StatNames.TryGetValue(stat.ToString().ToLower(), out val)){
				return val;
			}
			return stat.ToString();
		}
		public static string UserStatName(this string stat)
		{
			if(stat==null)return "";
			string val;
			if(DbInfoHelper.Get().StatNames.TryGetValue(stat.ToLower(), out val)){
				return val;
			}
			return stat.ToString();
		}
		public static int Value(this UserStat stat,User user){
			if(user==null)return 0;
			switch(stat){
					case UserStat.STR:return user.STR;
					case UserStat.DEX:return user.DEX;
					case UserStat.INT:return user.INT;
					case UserStat.WILL:return user.WILL;
					case UserStat.LUCK:return user.LUCK;
					case UserStat.HP:return user.HP;
					case UserStat.STAMINA:return user.STAMINA;
					case UserStat.AP:return user.AP;
					case UserStat.LEVEL:return user.level;
					case UserStat.Class:return user.Class.Index();
			}
			return 0;
		}
	}
}
