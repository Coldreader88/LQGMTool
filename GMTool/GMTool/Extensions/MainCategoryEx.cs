/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/21
 * 时间: 15:40
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using GMTool.Enums;

namespace GMTool.Extensions
{
	public static class MainCategoryEx
	{
		public static string[] Values = new string[] { "-","首饰","时装","衣服","碎片","沙龙","技能书",
			"活动","商品","重甲","轻甲","材料","板甲","任务","背部","尾巴","Tir硬币","武器"
		};
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
}
