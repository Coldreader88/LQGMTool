using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMTool.Bean
{
    public enum ItemCategory
    {
        NONE,
        ALCHEMIST_BOX,
        ALCHEMIST_KIT,
        APPEARANCE,
        ARTIFACT,
        AVATAR_BOOTS,
        AVATAR_GLOVES,
        AVATAR_HELM,
        AVATAR_PACK,
        AVATAR_PACKAGE,
        AVATAR_PANTS,
        AVATAR_SET,
        AVATAR_TUNIC,
        BADGE,
        BEARD,
        BELT,
        BODYPAINTING,
        BOOTS,
        BRACELET,
        BULLET,
        CAPSULE,
        CASHITEM,
        CASTLET,
        CHARM,
        CLEANER,
        COMBINE_PART,
        EARRING,
        EPAULET,
        ERG,
        ETC,
        EYEBROW,
        FACEPAINTING,
        FEATHER,
        GACHA_AVATAR,
        GACHA_WEAPON,
        GARBAGE,
        GEMSTONE,
        GLOVES,
        HAIR,
        HELM,
        INNERARMOR,
        KIT,
        LARGESHIELD,
        MAKEUP,
        MANA_GEM,
        NECKLACE,
        PACKAGE,
        PANTS,
        PATTERN,
        POTION,
        QUESTSCROLL,
        RAREITEM,
        REAR,
        RING,
        SCAR,
        SCROLL,
        SHIELD,
        SKILLBOOK,
        SKILL_ENHANCE_STONE,
        SPELLBOOK,
        STORYSCROLL,
        SUBWEAPON,
        TAIL,
        TITLESCROLL,
        TOWN_EFFECT,
        TUNIC,
        WEAPON,
    }

    public static class CategoryEx
    {

        public static string GetName(this ItemCategory category)
        {
            switch (category)
            {
                case ItemCategory.ALCHEMIST_BOX: return "炼金术士盒子";
                case ItemCategory.ALCHEMIST_KIT: return "炼金试剂盒";
                case ItemCategory.APPEARANCE: return "外观";
                case ItemCategory.ARTIFACT: return "神器";
                case ItemCategory.AVATAR_BOOTS: return "时装鞋子";
                case ItemCategory.AVATAR_GLOVES: return "时装手套";
                case ItemCategory.AVATAR_HELM: return "时装帽子";
                case ItemCategory.AVATAR_PACK: return "礼包";
                case ItemCategory.AVATAR_PACKAGE: return "包裹";
                case ItemCategory.AVATAR_PANTS: return "时装裤子";
                case ItemCategory.AVATAR_SET: return "时装套装";
                case ItemCategory.AVATAR_TUNIC: return "外衣";
                case ItemCategory.BADGE: return "徽章";
                case ItemCategory.BEARD: return "胡子";
                case ItemCategory.BELT: return "带";
                case ItemCategory.BODYPAINTING: return "人体彩绘";
                case ItemCategory.BOOTS: return "鞋子";
                case ItemCategory.BRACELET: return "手镯";
                case ItemCategory.BULLET: return "子弹";
                case ItemCategory.CAPSULE: return "胶囊";
                case ItemCategory.CASHITEM: return "现金物品";
                case ItemCategory.CASTLET: return "灭魔手套";
                case ItemCategory.CHARM: return "魅力";
                case ItemCategory.CLEANER: return "沐浴剂";
                case ItemCategory.COMBINE_PART: return "结合部分";
                case ItemCategory.EARRING: return "耳环";
                case ItemCategory.EPAULET: return "肩章";
                case ItemCategory.ERG: return "ERG";
                case ItemCategory.ETC: return "ETC";
                case ItemCategory.EYEBROW: return "眉毛";
                case ItemCategory.FACEPAINTING: return "脸彩";
                case ItemCategory.FEATHER: return "羽毛";
                case ItemCategory.GACHA_AVATAR: return "Gacha时装";
                case ItemCategory.GACHA_WEAPON: return "Gacha武器";
                case ItemCategory.GARBAGE: return "Garbage";
                case ItemCategory.GEMSTONE: return "宝石";
                case ItemCategory.GLOVES: return "手套";
                case ItemCategory.HAIR: return "头发";
                case ItemCategory.HELM: return "头盔";
                case ItemCategory.INNERARMOR: return "内衣";
                case ItemCategory.KIT: return "材料";
                case ItemCategory.LARGESHIELD: return "巨盾";
                case ItemCategory.MAKEUP: return "妆容";
                case ItemCategory.MANA_GEM: return "法力宝石";
                case ItemCategory.NECKLACE: return "项链";
                case ItemCategory.PACKAGE: return "包裹";
                case ItemCategory.PANTS: return "裤子";
                case ItemCategory.PATTERN: return "模式";
                case ItemCategory.POTION: return "药剂";
                case ItemCategory.QUESTSCROLL: return "任务卷轴";
                case ItemCategory.RAREITEM: return "稀有物品";
                case ItemCategory.REAR: return "Rear";
                case ItemCategory.RING: return "戒指";
                case ItemCategory.SCAR: return "伤痕";
                case ItemCategory.SCROLL: return "纸卷";
                case ItemCategory.SHIELD: return "盾";
                case ItemCategory.SKILLBOOK: return "技能书";
                case ItemCategory.SKILL_ENHANCE_STONE: return "技能增强石";
                case ItemCategory.SPELLBOOK: return "魔法书";
                case ItemCategory.STORYSCROLL: return "故事卷轴";
                case ItemCategory.SUBWEAPON: return "辅助武器";
                case ItemCategory.TAIL: return "尾";
                case ItemCategory.TITLESCROLL: return "标题卷轴";
                case ItemCategory.TOWN_EFFECT: return "城镇效果";
                case ItemCategory.TUNIC: return "外衣";
                case ItemCategory.WEAPON: return "武器";
                default:
                    return "未知";
            }
        }
    }
}
