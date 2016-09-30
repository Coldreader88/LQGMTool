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
		public static string StatName(this string name){
			string val ;
			if(DbInfoHelper.Get().ItemStatNames.TryGetValue(name.ToLower(), out val)){
				return val;
			}
			return name;
		}
		public static int Value(this string stat,User user){
			if(user==null)return 0;
			switch(stat){
					case "STR":return user.Stat.STR;
					case "DEX":return user.Stat.DEX;
					case "INT":return user.Stat.INT;
					case "WILL":return user.Stat.WILL;
					case "LUCK":return user.Stat.LUCK;
					case "HP":return user.Stat.HP;
					case "STAMINA":return user.Stat.STAMINA;
					case "AP":return user.AP;
					case "LEVEL":return user.level;
					case "Class":return user.Class.Index();
			}
			return 0;
		}
	}
}
