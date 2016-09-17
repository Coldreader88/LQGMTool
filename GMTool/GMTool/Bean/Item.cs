﻿using GMTool.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMTool.Bean
{
    public enum PackType
    {
        All = -1,
        Normal = 0,
        Cash = 100
    }
    public class Item
    {
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string ItemClass { get; private set; }
        public string Category { get; private set; }
        public string ItemType { get; private set; }
        public long ItemID { get; private set; }

        public int RequiredClass { get; set; }
        public string Time { get; set; }

        public int Count { get; set; }
        public int Collection { get; set; }
        public int Slot { get; set; }
        /// <summary>
        /// 强化
        /// </summary>
        public ItemAttribute[] Attributes;

        public int Color1 { get; set; }
        public int Color2 { get; set; }
        public int Color3 { get; set; }

        public Item(long ItemID, string ItemClass, string ItemType)
        {
            this.ItemID = ItemID;
            this.ItemClass = ItemClass;
            this.ItemType = ItemType;
        }

        public void Attach(ItemClassInfo info)
        {
            if (info != null)
            {
                this.ItemDesc = info.Desc;
                this.ItemName = info.Name;
                this.ItemType = info.TradeCategory.GetName();
                this.Category = info.Category.GetName();
                this.RequiredClass = info.ClassRestriction;
            }
        }
        public override string ToString()
        {
            string text = "物品ID：" + ItemClass + "\n物品名：" + ItemName
                + "\n类型：" + Category + " [" + ItemType + "]";
            if (Time != "无限期")
            {
                text += "\n到期时间：" + Time;
            }


            if (Color1 != 0)
            {
                text += "\n-----------------------------";
                text += "\n颜色：" + (Color1 == 0 ? "无" : "#" + Color1.ToString("X"))
                     + "，" + (Color2 == 0 ? "无" : "#" + Color2.ToString("X"))
                      + "，" + (Color3 == 0 ? "无" : "#" + Color3.ToString("X"));
                text += "\n-----------------------------";
            }
            text += "\n职业限制：" + ClassInfoEx.GetText(this.RequiredClass);
            text += "\n物品描述：\n    " + ItemDesc;

            if (Attributes != null)
            {
                foreach (ItemAttribute attr in Attributes)
                {
                    if (attr.Type == ItemAttributeType.ENHANCE)
                    {
                        text += "\n-----------------------------\n强化：" + attr.Value;
                    }
                    else if (attr.Type == ItemAttributeType.PREFIX)
                    {
                        EnchantInfo einfo = ItemClassInfoHelper.Get().GetEnchant(attr.Value);
                        text += "\n-----------------------------\n附魔属性：" + (einfo == null ? attr.Value : einfo.ToString());
                    }
                    else if (attr.Type == ItemAttributeType.QUALITY)
                    {
                        text += "\n品质：" + attr.Arg;
                    }
                }
            }
            return text;
        }
    }
}
