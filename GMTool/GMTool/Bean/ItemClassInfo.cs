using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMTool.Enums;
using GMTool.Extensions;

namespace GMTool.Bean
{
    public class ItemClassInfo
    {
        public string ItemClass;
        /// <summary>
        /// 子目录
        /// </summary>
        public SubCategory SubCategory;
        /// <summary>
        /// 主分类
        /// </summary>
        public MainCategory MainCategory;
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
        
        public long MaxStack;

        public override string ToString()
        {
        	return "物品ID："+ ItemClass+"\n物品名字："+Name+" \n需求等级： "+ RequiredLevel +"\n最大叠放数量："+(MaxStack<0?"不限":""+MaxStack)+"\n分类："+ SubCategory.Name()+ "  "+MainCategory.Name()
                + "\n职业限制："+ClassInfoEx.GetClassText(ClassRestriction) + "\n物品描述" + Desc;
        }
    }
}
