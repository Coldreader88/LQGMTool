using GMTool.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMTool.Enums;
using GMTool.Extensions;
using System.Data.Common;

namespace GMTool.Bean
{
	public class Item
	{
		public string ItemName  { get; private set; }
		public string ItemDesc  { get; private set; }
		public string ItemClass { get; private set; }
		public string SubCategory { get; private set; }
		public string MainCategory { get; private set; }
		public long ItemID { get; private set; }

		public int RequiredClass  { get; private set; }
		public string Time  { get; private set; }

		public int Count  { get; private set; }
		public int Collection  { get; private set; }
		public int Slot  { get; private set; }
		/// <summary>
		/// 强化
		/// </summary>
		public ItemAttribute[] Attributes { get; private set; }
		public ItemStatInfo Stat { get; private set; }
		public long MaxStack { get; private set; }
		public int Color1  { get; private set; }
		public int Color2  { get; private set; }
		public int Color3 { get; private set; }
		public Item(){
			
		}
		public Item(DbDataReader reader)
		{
			this.ItemID = reader.ReadInt64("ID");
			this.ItemClass =   reader.ReadString("itemClass");
			this.MainCategory = "";
			this.MaxStack=1;
			string time = reader.ReadString("ExpireDateTime", null);
			if (time != null)
			{
				this.Time = time.Split(' ')[0];
			}
			else
			{
				this.Time = "无限期";
			}
			this.Collection = reader.ReadInt32("Collection");
			this.Slot = reader.ReadInt32("Slot");
			//  this.attrName = reader["Attribute"] == DBNull.Value ? null : Convert.ToString(reader["Attribute"]);
			//  this.attrValue = reader["Value"] == DBNull.Value ? null : Convert.ToString(reader["Value"]);
			this.Count = reader.ReadInt32("Count", 1);
			this.Color1  = reader.ReadInt32("Color1",0);
			this.Color2  = reader.ReadInt32("Color2",0);
			this.Color3  = reader.ReadInt32("Color3",0);
		}
		
		public void UpdateAttributes(DbDataReader reader){
			List<ItemAttribute> attrs = new List<ItemAttribute>();
			while (reader != null && reader.Read())
			{
				attrs.Add(new ItemAttribute(reader));
			}
			this.Attributes = attrs.ToArray<ItemAttribute>();
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
				this.MaxStack = info.MaxStack;
				this.Stat = info.Stat;
			}
		}
		public override string ToString()
		{
			string text = "物品ID：" + ItemClass + "\n物品名：" + ItemName
				+"\n背包："+ Collection + "，格子："+Slot
				+ "\n类型：" +MainCategory + " [" +  SubCategory + "]";
			text+="\n最大堆叠数量："+(MaxStack<0?"不限":""+MaxStack);
			if (Time != "无限期")
			{
				text += "\n到期时间：" + Time;
			}
			
			if (Color1 != 0)
			{
				text += "\n颜色：" + (Color1 == 0 ? "无" : "#" + Color1.ToString("X"))
					+ "，" + (Color2 == 0 ? "无" : "#" + Color2.ToString("X"))
					+ "，" + (Color3 == 0 ? "无" : "#" + Color3.ToString("X"));
			}
			text += "\n职业限制：" + ClassInfoEx.GetClassText(this.RequiredClass);
			if(Stat!=null){
				text+=Stat.ToString();
			}
			text += "\n物品描述：\n    " + ItemDesc;

			if (Attributes != null)
			{
				foreach (ItemAttribute attr in Attributes)
				{
					text += "\n"+attr.ToString();
				}
			}
			return text;
		}
	}
}
