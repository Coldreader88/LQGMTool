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
        public string ItemType { get; private set; }
        public long ItemID { get; private set; }
        public string Time { get; set; }

        public int Count { get; set; }
        public int Collection { get; set; }
        public int Slot { get; set; }
        /// <summary>
        /// 强化
        /// </summary>
        public string attrValue { get; set; }

        public string attrName { get; set; }

        /// <summary>
        /// 附魔id
        /// </summary>
        public string enchId { get; set; }
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
                this.ItemType = info.Category.GetName();
            }
        }
        public override string ToString()
        {
            return "物品ID：" + ItemClass + "\n物品名：" + ItemName
                + "\n颜色1：" + (Color1 == 0 ? "无" : "#" + Color1.ToString("X"))
                 + "，颜色2：" + (Color2 == 0 ? "无" : "#" + Color2.ToString("X"))
                  + "，颜色3：" + (Color3 == 0 ? "无" : "#" + Color3.ToString("X"))
                + "\n物品描述：\n    " + ItemDesc;
        }
    }
}
