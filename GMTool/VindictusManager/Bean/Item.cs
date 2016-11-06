using System;
using System.Collections.Generic;
using System.Linq;
using Vindictus.Enums;
using Vindictus.Extensions;
using System.Data.Common;
using Vindictus.Helper;

namespace Vindictus.Bean
{
	public class Item : ItemClassInfo
	{
		public long ItemID { get; private set; }

		public int RequiredClass  { get; private set; }
		public string Time  { get; private set; }

		public int Count  { get; private set; }
		public int Collection  { get; private set; }
		public int Slot  { get; private set; }
		public int Color1  { get; private set; }
		public int Color2  { get; private set; }
		public int Color3 { get; private set; }
		public List<ItemAttribute> Attributes { get; private set; }
		public Item(ItemClassInfo info):base(info)
		{
		}
		
		public Item(){
			
		}
		public Item(DbDataReader reader, DataHelper datahelper)
		{
			this.ItemID = reader.ReadInt64("ID");
			this.ItemClass =   reader.ReadString("itemClass");
			string time = reader.ReadString("ExpireDateTime", null);
			if (time != null)
			{
				this.Time = time.Split(' ')[0];
			}
			else
			{
				this.Time = "-";
			}
			this.Collection = reader.ReadInt32("Collection");
			this.Slot = reader.ReadInt32("Slot");
			this.Count = reader.ReadInt32("Count", 1);
			this.Color1  = reader.ReadInt32("Color1",0);
			this.Color2  = reader.ReadInt32("Color2",0);
			this.Color3  = reader.ReadInt32("Color3",0);
			ItemClassInfo item = datahelper.getItemClassInfo(this.ItemClass);
			cloneFrom(item);
		}
		
		public void UpdateAttributes(DbDataReader reader, DataHelper datahelper){
			List<ItemAttribute> attrs = new List<ItemAttribute>();
			while (reader != null && reader.Read())
			{
				attrs.Add(new ItemAttribute(reader, datahelper));
			}
			this.Attributes = attrs;
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
		public override string ToString()
		{
			string text = ItemClass + "\n" + Name
				+"\n"+R.Package+":"+ Collection + "("+Slot+")"
				+ "\n"+R.Category+":" +MainCategory + "," +  SubCategory + "";
			text+="\n"+R.MaxStackCount+":"+(MaxStack<0? R.UnLimit :""+MaxStack);
			if (Color1 != 0)
			{
				text += "\n"+R.Color+":" + "#" + Color1.ToString("X")
					+ "，" + (Color2 == 0 ? "-" : "#" + Color2.ToString("X"))
					+ "，" + (Color3 == 0 ? "-" : "#" + Color3.ToString("X"));
			}
			text += "\n"+R.ClassRestriction+":" + ClassInfoEx.GetClassText(this.RequiredClass);
			if(Stat!=null){
				text+=Stat.ToString();
			}
			if (Attributes != null)
			{
				foreach (ItemAttribute attr in Attributes)
				{
					text += "\n"+attr.ToString();
				}
			}
			text += "\n" + Desc;
			if (Time != "-")
			{
				text += "\n"+R.TimeDate+":" + Time;
			}
			return text;
		}
	}
}
