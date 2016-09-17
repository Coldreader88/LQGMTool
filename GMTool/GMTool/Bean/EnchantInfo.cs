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
        public int MinArg;
        public int MaxArg;
        public string GetValue()
        {
            /*
            if (Class == null) return "?";
            Regex reg = new Regex("[0-9]+");
            Match m = reg.Match(Class);
            if (m.Groups.Count > 0)
            {
                return m.Groups[0].Value;
            }*/
            return MinArg+"-"+MaxArg;
        }
        public override string ToString()
        {
            return Name+ "\n描述：" + Desc + "\n效果：" + Effect;
        }
    }
}
