using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Drawing
{
   public static class ColorEx
    {
        public static Color GetColor(this int i)
        {
            if (i == 0)
            {
                return Color.Transparent;
            }
            return GetColor(i.ColorString());
        }
        public static Color GetColor(this String i)
        {
            try
            {
                return ColorTranslator.FromHtml(CheckColorString(i));
            }
            catch (Exception)
            {
                return Color.Transparent;
            }
        }
        public static string ColorString(this int color)
        {
            if (color == 0)
            {
                return "#FFFFFF";
            }
            return CheckColorString(color.ToString("X"));
        }

        private static string CheckColorString(string str)
        {
            if (str == null) return "#FFFFFF";
            if (str.StartsWith("#"))
            {
                str = str.Substring(1);
            }
            if (str.Length != 6 && str.Length != 8)
            {
                if (str.Length < 6)
                {
                    for (int i = 0; i < 6 - str.Length; i++)
                    {
                        str = "0" + str;
                    }
                    return "#" + str;
                }
                else if (str.Length < 8)
                {
                    return "#F" + str;
                }
            }
            if (str.Length == 8)
            {
                if (str.StartsWith("FF"))
                {
                    str = str.Substring(2);
                }
            }
            return "#" + str;
        }
        public static string ColorString(this Color i)
        {
            return ColorString(i.ToArgb());
        }
    }
}
