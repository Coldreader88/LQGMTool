using System;
using System.Linq;
using Vindictus.Enums;

namespace Vindictus.Extensions
{
    public static class GroupExtension
    {
        public static string Name(this GroupInfo info)
        {
            switch (info) {
                case GroupInfo.Dark:
                    return R.GroupDark;
                case GroupInfo.Light:
                    return R.GroupLight;
                default:
                    return R.UnKnown;
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
