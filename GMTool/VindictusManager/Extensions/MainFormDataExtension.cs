
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vindictus.Enums;
using Vindictus.Helper;
using System.ComponentModel;
using System.Xml;
using ServerManager;
using Vindictus.UI;
using System.Data.Common;
using System.IO;
using System.Threading;
using Vindictus.Bean;

namespace Vindictus.Extensions
{
	public static class MainFormDataExtension
	{
		#region user
		private const string SQL_QUERY_USERS = "select * from CharacterInfo WHERE DeleteTime is NULL ORDER BY Name, CreateTime;";
		public static List<User> ReadAllUsers(this MSSqlHelper db)
		{
			List<User> userList = new List<User>();
			using (DbDataReader reader = db.GetReader(SQL_QUERY_USERS))
			{
				while (reader != null && reader.Read())
				{
					User item = new User(reader);
					userList.Add(item);
				}
			}
			foreach (User user in userList)
			{
				using (DbDataReader reader = db.GetReader("Select vocationClass,VocationLevel from vocation where CID=" + user.CID))
				{
					while (reader != null && reader.Read())
					{
						user.UpdateGroup(reader);
						break;
					}
				}
			}
			return userList;
		}
		public static List<long> GetTitles(this MSSqlHelper db,User user){
			List<long> titles=new List<long>();
			using(DbDataReader reader=db.GetReader("select TitleID from Title where Acquired =1 and CID="+user.CID)){
				while(reader!=null&&reader.Read()){
					titles.Add(reader.ReadInt64("TitleID"));
				}
			}
			return titles;
		}
		#endregion
		
		#region mail
		public static List<Mail> ReadSendMailList(this MSSqlHelper db, User user)
		{
			List<Mail> mails = new List<Mail>();
			if (user == null)
			{
				return mails;
			}
			using (DbDataReader reader = db.GetReader("SELECT * FROM QueuedItem WHERE CID =" + user.CID))
			{
				while (reader != null && reader.Read())
				{
					Mail item = new Mail(reader);
					mails.Add(item);
				}
			}
			return mails;
		}
		public static List<Mail> ReadRecvMailList(this MSSqlHelper db, User user)
		{
			List<Mail> mails = new List<Mail>();
			if (user == null)
			{
				return mails;
			}
			using (DbDataReader reader = db.GetReader("select mi.itemCount,m.* from Mail as m LEFT JOIN MailItem as mi ON mi.MailID = m.MailID"
			                                          + " where m.ToCID =" + user.CID))
			{
				while (reader != null && reader.Read())
				{
					mails.Add(new Mail().AttachBox(reader));
				}
			}
			return mails;
		}
		public static void DeleteUserMail(this MSSqlHelper db, Mail mail)
		{
			try
			{
				db.ExcuteSQL("delete from mailItem where mailID = " + mail.MailID);
				db.ExcuteSQL("delete from mail where mailID = " + mail.MailID);
			}
			catch (Exception)
			{

			}
		}
		public static void DeleteSendMail(this MSSqlHelper db, Mail mail)
		{
			try
			{
				db.ExcuteSQL("DELETE FROM QueuedItem Where RowID=" + mail.MailID);
			}
			catch (Exception)
			{

			}
		}
		#endregion
		/// <summary>
		/// 删除物品
		/// </summary>
		public static bool DeleteItem(this MainForm main, User user,params Item[] items)
		{
			if (items != null)
			{
				if (items.Length == 1)
				{
					if (!main.Question(string.Format(R.DeleteItem, items[0].Name)))
					{
						return false;
					}
				}
				else if(items.Length > 1)
				{
					if (!main.Question(string.Format(R.DeleteItems,items.Length)))
					{
						return false;
					}
				}
				try
				{
					foreach (Item item in items)
					{
						main.Db.ExcuteSQL("DELETE FROM Item Where ID=" + item.ItemID);
						main.log("删除["+item.Name+"]"+item.ItemClass);
					}
					return true;
				}
				catch (Exception)
				{
				}
			}
			return false;
		}
		public static bool ModUserInfo(this MainForm main, User user,string name,object value)
		{
			try
			{
				main.Db.ExcuteSQL("update characterInfo set "+name+" = N'"+value+"'"+
				                  " where id = "+ user.CID );
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
		public static bool ModUserLevel(this MainForm db, User user, int level)
		{
			return ModUserInfo(db, user, "Level", level);
		}
		public static bool ModUserClass(this MainForm db, User user, ClassInfo cls)
		{
			return ModUserInfo(db, user, "class", cls.Index());
		}
		public static bool ModUserName(this MainForm db, User user, string name)
		{
			return ModUserInfo(db, user, "name", name);
		}
		public static bool ModUserAP(this MainForm db, User user, int ap)
		{
			return ModUserInfo(db, user, "ap", ap);
		}
		/// <summary>
		/// 评分最大
		/// </summary>
		public static int ModItemScoreMax(this MainForm main, User user,params Item[] items)
		{
			return ModItemScore(main, user,"S", items);
		}
		/// <summary>
		/// 评分最大
		/// </summary>
		public static int ModItemScore(this MainForm main, User user,string score, params Item[] items)
		{
			if (items == null || items.Length == 0)
			{
				return 0;
			}
			else
			{
				int count = 0;
				foreach (Item item in items)
				{
					if(ModItemAttr(main, new ItemAttribute(ItemAttributeType.SYNTHESISGRADE, score), item)){
						count++;
					}
				}
				return count;
			}
		}
		/// <summary>
		/// 强化
		/// </summary>
		public static bool ModItemPower(this MainForm main, Item item, int power)
		{
			if (item == null) return false;
			try
			{
				if (power == 0)
				{
					main.Db.ExcuteSQL(string.Concat(new object[] { "DELETE FROM ItemAttribute where itemID=", item.ItemID, "and Attribute = 'ENHANCE'" }));
					return true;
				}
				return ModItemAttr(main, new ItemAttribute(ItemAttributeType.ENHANCE, ""+power), item);
			}
			catch (Exception e)
			{
				main.Error("强化失败：" + e);
			}
			return false;
		}
		/// <summary>
		/// 无限时间
		/// </summary>
		public static bool ModItemTime(this MainForm main, User user, params Item[] items)
		{
			if (items == null || items.Length == 0)
			{
				main.Db.ExcuteSQL("UPDATE Item SET EXpireDateTime = NULL WHERE OwnerID =" + user.CID);
			}
			else
			{
				foreach (Item item in items)
				{
					main.Db.ExcuteSQL("UPDATE Item SET EXpireDateTime = NULL WHERE OwnerID =" + user.CID + " and ID = " + item.ItemID);
				}
			}
			return true;
		}
		/// <summary>
		/// 修改物品数量
		/// </summary>
		public static bool ModItemCount(this MainForm main, User user, Item item,int count)
		{
			if (item == null)
			{
				return false;
			}
			if(count > item.MaxStack){
				return false;
			}
			main.Db.ExcuteSQL("UPDATE Item SET Count = "+count+
			                  " WHERE OwnerID =" + user.CID + " and ID = " + item.ItemID);
			return true;
		}
		/// <summary>
		/// 品质最大
		/// </summary>
		public static int ModItemStar(this MainForm main, User user, int q,params Item[] items)
		{
			if (items == null || items.Length == 0)
			{
				return 0;
			}
			else
			{
				int count = 0;
				foreach (Item item in items)
				{
					if(ModItemAttr(main, new ItemAttribute(ItemAttributeType.QUALITY, q), item)){
						count++;
					}
				}
				return count;
			}
		}
		private static bool ModItemAttr(this MainForm main,ItemAttribute attr, Item item)
		{
			long itemID = item.ItemID;
			string val = (attr.Value==null?"":attr.Value);
			if(main.Db.ExcuteSQL("update ItemAttribute set Value='"+val+
			                     "',Arg="+attr.Arg+
			                     ",Arg2="+attr.Arg2+
			                     " where ItemID = " + itemID +" and Attribute = '"+
			                     attr.Type.ToString()
			                     +"'")==0){
				//修改
				return main.Db.ExcuteSQL("insert into ItemAttribute(ItemID,Attribute,Value,Arg,Arg2)"+
				                         " values(" + itemID + ",'"+attr.Type.ToString()
				                         +"','"+val+"',"+attr.Arg+","+attr.Arg2+")") > 0;
			}else{
				//插入
				return true;
			}
		}
		/// <summary>
		/// 附魔
		/// </summary>
		public static bool ItemEnchant(this MainForm main, Item item, EnchantInfo attribute)
		{
			if (attribute == null || item == null)
			{
				return false;
			}
			ItemClassInfo info = main.DataHelper.getItemClassInfo(item.ItemClass);
			if (info != null)
			{
				if (!(info.MainCategory == MainCategory.WEAPON
				      || info.SubCategory == SubCategory.INNERARMOR
				      || info.MainCategory == MainCategory.CLOTH
				      || info.MainCategory == MainCategory.LIGHTARMOR
				      || info.MainCategory == MainCategory.HEAVYARMOR
				      || info.MainCategory == MainCategory.PLATEARMOR
				      || info.MainCategory == MainCategory.ACCESSORY))
				{
					if (!main.Question("该类型[" + info.MainCategory.Name() + "]不适合附魔，确定强制附魔？"))
					{
						return false;
					}
				}
			}
			
			ItemAttributeType type = attribute.IsPrefix ? ItemAttributeType.PREFIX : ItemAttributeType.SUFFIX;
			
			return ModItemAttr(main, new ItemAttribute(type, attribute.Class).SetArg(attribute.MaxArg), item);
		}
	}
}
