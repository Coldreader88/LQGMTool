using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMTool.Bean
{
	public enum GameClass :byte
	{
		Lethita=0,//0x1
		Fiona,//0x2
		Evy,//0x4
		Kalok,//0x8
		Kay,//0x10
		Vella,//0x20
		Hurk,//0x40
		Lynn,//0x80
		Arisha,//0x100
		Hagie,//0x200
		Delia//0x400
	}
	public enum ClassInfo
	{
		Lethita = 0x1,
		Fiona=0x2,
		Evy=0x4,
		Kalok=0x8,
		Kay=0x10,
		Vella=0x20,
		Hurk=0x40,
		Lynn=0x80,
		Arisha=0x100,
		Hagie=0x200,
		Delia=0x400,
	}

	public static class ClassInfoEx
	{
		public static ClassInfo Info(this GameClass cls){
			string name = cls.ToString();
			return (ClassInfo)Enum.Parse(typeof(ClassInfo), name);
		}
		public static GameClass Info(this ClassInfo cls){
			string name = cls.ToString();
			return (GameClass)Enum.Parse(typeof(GameClass), name);
		}
		public static string Name(this ClassInfo cls){
			return cls.Info().Name();
		}
		public static string Name(this GameClass cls){
			return Values[cls.Value()];
		}
		public static string[] Values=new string[]{
			"利斯塔",
			"菲欧娜",
			"伊菲",
			"卡鲁",
			"凯",
			"维拉",
			"赫克",
			"琳",
			"艾瑞莎",
			"赫基",
			"蒂莉亚",
		};
		public static int Value(this ClassInfo cls){
			return (int)cls;
		}
		public static int Value(this GameClass cls){
			return (int)cls;
		}
		public const int ALL1 =
			(int)ClassInfo.Lethita
			| (int)ClassInfo.Fiona
			| (int)ClassInfo.Evy
			| (int)ClassInfo.Kalok
			| (int)ClassInfo.Kay
			| (int)ClassInfo.Vella
			| (int)ClassInfo.Lynn
			| (int)ClassInfo.Hurk
			| (int)ClassInfo.Arisha;
		public const int ALL2 = ALL1 | (int)ClassInfo.Hagie;
		public const int ALL3 = ALL2 | (int)ClassInfo.Delia;
		public static string GetText(int cls)
		{
			if (cls == ALL1 || cls== ALL2||cls == ALL3)
			{
				return "无";
			}
			Array arr= Enum.GetValues(typeof(ClassInfo));
			StringBuilder sb = new StringBuilder();
			foreach(ClassInfo c in arr)
			{
				if((cls & (int)c) != 0)
				{
					sb.Append("" + c + ",");
				}
			}
			string txt= sb.ToString();
			if (txt.EndsWith(","))
			{
				return txt.Substring(0, txt.Length - 1);
			}
			return txt;
		}
	}
}
