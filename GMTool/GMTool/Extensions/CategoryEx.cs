/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/21
 * 时间: 15:39
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using GMTool.Enums;

namespace GMTool.Extensions
{
	/// <summary>
	/// Description of CategoryEx.
	/// </summary>
	public static class SubCategoryEx
	{

        public static string Name(this SubCategory category)
        {
            int index = (int)category;
            if (index >= 0 && index < Values.Length)
            {
                return Values[index];
            }
            return Values[0];
        }

        public static string[] Values=new string[] {
            "-", "炼金术士盒子","炼金工具","外观","神器","时装鞋子",
            "时装手套","时装帽子","活动礼包","时装礼包",
            "时装裤子","时装套装", "外衣","徽章",
            "胡子","腰带","人体彩绘","鞋子", "手镯",
            "子弹", "胶囊", "现金物品","灭魔手套",
            "饰品", "清洁剂","碎片","耳环", "肩章",
            "结晶","技能书", "眉毛","脸彩", "羽毛", "Gacha时装",
            "Gacha武器","礼包", "宝石","手套", "头发",
            "头盔","内衣","工具","巨盾","妆容", "法力宝石",
            "项链","时装礼包","裤子","图案","药剂","任务","稀有",
            "背部","戒指", "伤疤","卷轴","盾", "技能书",
                "觉醒石", "魔法书", "任务卷轴","辅助武器","尾","头衔卷轴",
            "城镇效果","外衣","武器"
        };
	}
}
