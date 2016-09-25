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
                return ColorTranslator.FromHtml(i);
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
            string str = color.ToString("X");
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
            return "#"+str;
        }
        public static string ColorString(this Color i)
        {
            return ColorString(i.ToArgb());
        }
    }
}
