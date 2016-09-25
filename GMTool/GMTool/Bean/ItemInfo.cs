using GMTool.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMTool.Enums;
using GMTool.Extensions;
namespace GMTool.Bean
{
    public class Item
    {
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public string ItemClass { get; private set; }
        public string SubCategory { get; private set; }
        public string MainCategory { get; private set; }
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
            this.MainCategory = ItemType;
        }
        /// <summary>
        /// 背包类型
        /// </summary>
        public PackageType Package
        {
            get
            {
                if (Collection < 100)
                {
                    return PackageType.Normal;
                } else if (Collection == (int)PackageType.Cash) {
                    return PackageType.Cash;
                }
                else if (Collection == (int)PackageType.Quest)
                {
                    return PackageType.Quest;
                }
                else if (Collection == (int)PackageType.Other)
                {
                    return PackageType.Other;
                }
                return PackageType.All;
            }
        }
        public void Attach(ItemClassInfo info)
        {
            if (info != null)
            {
                this.ItemDesc = info.Desc;
                this.ItemName = info.Name;
                this.MainCategory = info.MainCategory.Name();
                this.SubCategory = info.SubCategory.Name();
                this.RequiredClass = info.ClassRestriction;
            }
        }
        public override string ToString()
        {
            string text = "物品ID：" + ItemClass + "\n物品名：" + ItemName
                +"\n背包："+ Collection + "，格子："+Slot
                + "\n类型：" +MainCategory + " [" +  SubCategory + "]";
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
            text += "\n职业限制：" + ClassInfoEx.GetClassText(this.RequiredClass);
            text += "\n物品描述：\n    " + ItemDesc;

            if (Attributes != null)
            {
                foreach (ItemAttribute attr in Attributes)
                {
                	text += "\n-----------------------------\n"+attr.ToString();
                }
            }
            return text;
        }
    }
}
