using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMTool.Enums;

namespace GMTool.Extensions
{
    public static class GroupExtension
    {
        public static string Name(this GroupInfo info)
        {
            switch (info) {
                case GroupInfo.Dark:
                    return "黑暗";
                case GroupInfo.Light:
                    return "光明";
                default:
                    return "未知";
            }
        }

        public static GroupInfo ToGroupInfo(this int i)
        {
            GroupInfo info = GroupInfo.Unknown;
            try
            {
                info = (GroupInfo)i;
            }
            catch (Exception)
            {
            }
            return info;
        }
    }
}
