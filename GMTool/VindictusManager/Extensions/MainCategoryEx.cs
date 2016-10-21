using System;
using Vindictus.Enums;

namespace Vindictus.Extensions
{
	public static class MainCategoryEx
	{
		static MainCategoryEx(){
			Values=R.MainCategory.Split(',');
		}
		public static string[] Values;
		public static string Name(this MainCategory category)
		{
			int index = (int)category;
			if (index >= 0 && index < Values.Length)
			{
				return Values[index];
			}
			return Values[0];
		}
	}
	public static class SubCategoryEx
	{
		static SubCategoryEx(){
			Values = R.SubCategory.Split(',');
		}

        public static string Name(this SubCategory category)
        {
            int index = (int)category;
            if (index >= 0 && index < Values.Length)
            {
                return Values[index];
            }
            return Values[0];
        }

        public static string[] Values;
	}
}
