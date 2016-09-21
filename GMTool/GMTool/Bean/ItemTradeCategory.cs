using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMTool.Bean
{
    public enum ItemTradeCategory
    {
        NONE,
        ACCESSORY,
        AVATAR,
        CLOTH,
        COMBINE_PART,
        EQUIPMENT,
        ETC,
        EVENT,
        GOODS,
        HEAVYARMOR,
        LIGHTARMOR,
        MATERIAL,
        PLATEARMOR,
        QUEST,
        REAR,
        TAIL,
        TIRCOIN,
        WEAPON
    }
    public static class ItemTradeCategoryEx
    {
        public static string[] Values = new string[] { "-","首饰","时装","衣服","碎片","沙龙","技能书",
            "活动","商品","重甲","轻甲","材料","板甲","任务","背部","尾巴","Tir硬币","武器"
            };
        public static string Name(this ItemTradeCategory category)
        {
            int index = (int)category;
            if (index >= 0 && index < Values.Length)
            {
                return Values[index];
            }
            return Values[0];
        }
    }
}