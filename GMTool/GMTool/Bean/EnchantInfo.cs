using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GMTool.Bean
{
    public class EnchantInfo
    {
        public string Class;
        public string Name;
        public string Constraint;
        public string Desc;
        public string Effect;
        public bool IsPrefix = true;
        public int EnchantLevel;
        public int MinArg;
        public int MaxArg;
        public string GetValue()
        {
            return MinArg+"-"+MaxArg;
        }
        public override string ToString()
        {
            string txt= Name+ "\n" + Class+"\n"+Desc + "\n" + Effect;
            return txt;
        }
    }
}
