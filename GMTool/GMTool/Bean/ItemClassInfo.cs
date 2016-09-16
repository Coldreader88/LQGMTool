using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMTool.Bean
{
    public class ItemClassInfo
    {
        public string ItemClass;
        public ItemCategory Category;
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
    }
}
