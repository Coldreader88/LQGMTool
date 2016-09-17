﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMTool.Bean
{
    public class ItemClassInfo
    {
        public string ItemClass;
        public ItemCategory Category;
        public ItemTradeCategory TradeCategory;
        /// <summary>
        /// 等级需求
        /// </summary>
        public int RequiredLevel;
        /// <summary>
        /// 职业限制
        /// </summary>
        public int ClassRestriction;

        public string Name;

        public string Desc;

        public override string ToString()
        {
            return "物品ID："+ ItemClass+"\n物品名字："+Name+" \n需求等级： "+ RequiredLevel +"\n分类："+ Category.GetName()+ "  "+TradeCategory.GetName()
                + "\n职业限制："+ClassInfoEx.GetText(ClassRestriction) + "\n物品描述" + Desc;
        }
    }
}