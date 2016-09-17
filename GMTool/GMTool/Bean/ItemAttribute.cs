using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMTool.Bean
{
    public class ItemAttribute
    {
        public ItemAttributeType Type;
        public string Value;
        public string Arg;
        public string Arg2;
    }
    public enum ItemAttributeType
    {
        /// <summary>
        /// 附魔,Arg2
        /// </summary>
        PREFIX,
        VALUE,
        ANTIBIND,
        /// <summary>
        /// 强化
        /// </summary>
        ENHANCE,
        COMBINATION,
        VARIABLESTAT,
        SPIRIT_INJECTION,
        /// <summary>
        /// 0;0;1;0;2;0;3;0
        /// </summary>
        GEMSTONEINFO,
        LOOK,
        /// <summary>
        /// 附魔2
        /// </summary>
        SUFFIX,
        SYNTHESISGRADE,
        /// <summary>
        /// Arg
        /// </summary>
        QUALITY,
        PS_1,PS_0,PS_2,
        NONE,
    }
}
