using System;
using System.Text;
using Vindictus.Bean;
using Vindictus.Enums;

namespace Vindictus.Extensions
{
	public static class ClassInfoEx
	{
		static ClassInfoEx(){
			Names = R.Classes.Split(',');
		}
		/// <summary>
		/// 名字
		/// </summary>
		public static string Name(this ClassInfo cls){
			int index = cls.Index();
			if(index >= 0 && index < Names.Length)
				return Names[index];
			return "?";
		}

		/// <summary>
		/// 职业ID
		/// </summary>
		public static int Index(this ClassInfo cls){
			int len = Indexs.Length;
			int c = (int)cls;
			for(int i =0;i<len;i++){
				if(Indexs[i] == c){
					return i;
				}
			}
			return 0;
		}
		/// <summary>
		/// 索引
		/// </summary>
		public static ClassInfo ToClassInfo(this int i){
			if(i >= 0 && i < Indexs.Length)
				return (ClassInfo)Indexs[i];
			return ClassInfo.UnKnown;
		}
        public static ClassInfo ToClassInfo(this string name)
        {
            try
            {
                return (ClassInfo)Enum.Parse(typeof(ClassInfo), name);
            }
            catch (Exception)
            {
                return ClassInfo.UnKnown;
            }
        }
        /// <summary>
        /// 索引集合
        /// </summary>
        public static int[] Indexs=new int[]{
			0x1,
			0x2,
			0x4,
			0x8,
			0x10,
			0x20,
			0x40,
			0x80,
			0x100,
			0x200,
			0x400,
		};
		/// <summary>
		/// 名字集合
		/// </summary>
		public static string[] Names;

		public const int ALL =
			(int)ClassInfo.Lethita
			| (int)ClassInfo.Fiona
			| (int)ClassInfo.Evy
			| (int)ClassInfo.Kalok
			| (int)ClassInfo.Kay
			| (int)ClassInfo.Vella
			| (int)ClassInfo.Lynn
			| (int)ClassInfo.Hurk
			| (int)ClassInfo.Arisha;
		public const int ALL_20150922 = ALL | (int)ClassInfo.Hagie;
		public const int ALL_20160203 = ALL_20150922 | (int)ClassInfo.Delia;
		
		public static bool IsEnable(this User user,int ClassRestriction){
            if (ClassRestriction == 0) return true;
            int cls = (int)user.Class;
			return (cls & ClassRestriction) == cls;
		}
		/// <summary>
		/// 职业限制
		/// </summary>
		public static string GetClassText(int cls)
		{
			if (cls == ALL || cls== ALL_20150922||cls == ALL_20160203)
			{
				return "-";
			}
			Array arr= Enum.GetValues(typeof(ClassInfo));
			StringBuilder sb = new StringBuilder();
			foreach(ClassInfo c in arr)
			{
				if(c!=ClassInfo.UnKnown){
					if((cls & (int)c) != 0)
					{
						sb.Append("" + c.Name() + ",");
					}
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
