﻿using GMTool.Bean;
using GMTool.Helper;
using GMTool.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GMTool.Enums;
using System.Data.Common;
using System.Windows.Forms;
using GMTool.Extensions;

namespace GMTool
{
	public static class MainFormExtensions
	{
		#region 数据库
		private static MSSqlHelper db = new MSSqlHelper();
		private const string SQL_QUERY_USERS = "select * from CharacterInfo where CreateTime >= '2016-01-01' ORDER BY CreateTime;";

		public static bool IsOpen(this MainForm main)
		{
			return db.IsOpen;
		}

		public static void ReadSettings(this MainForm main)
		{
			IniHelper helper = new IniHelper(Program.INT_FILE);
			main.tb_mssql_server.Text = helper.ReadValue("mssql", "server");
			main.tb_mssql_db.Text = helper.ReadValue("mssql", "database");
			main.tb_mssql_user.Text = helper.ReadValue("mssql", "user");
			main.tb_mssql_pwd.Text = helper.ReadValue("mssql", "password");
		}

		public static bool connectDataBase(this MainForm main, string server, string user, string pwd, string dbname)
		{
			string connect = "Data Source=" + server + ";User Id=" + user + ";Password=" + pwd + ";Initial Catalog=" + dbname;
			bool rs = db.Open(connect);
			if (rs)
			{
				IniHelper helper = new IniHelper(Environment.CurrentDirectory + "/DBIni.ini");
				helper.WriteValue("mssql", "server", server.Trim());
				helper.WriteValue("mssql", "database", dbname.Trim());
				helper.WriteValue("mssql", "user", user.Trim());
				helper.WriteValue("mssql", "password", pwd.Trim());
			}
			return rs;
		}

		public static void CloseDataBase(this MainForm main)
		{
			db.Close();
		}
		#endregion

		#region 读取数据
		public static List<User> ReadAllUsers(this MainForm main)
		{
			List<User> userList = new List<User>();
			using (DbDataReader reader = db.GetReader(SQL_QUERY_USERS))
			{
				while (reader != null && reader.Read())
				{
					User item = new User(reader.ReadInt64("ID"),
					                     reader.ReadInt32("UID"),
					                     reader.ReadInt32("CharacterSN"),
					                     reader.ReadString("Name"),
					                     reader.ReadInt32("Class"),
					                     reader.ReadInt32("Level")
					                    );
					userList.Add(item);
				}
			}
			return userList;
		}

		public static List<Mail> ReadSendMailList(this MainForm main, User user)
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
                    string title = reader.ReadString("MailTitle");
                    title = DbInfoHelper.Get().GetMailTitle(title);
                    Mail item = new Mail(
						reader.ReadInt64("RowID"),
                        title,
						reader.ReadString("MailContent")
					);
					item.Count = reader.ReadInt32("Count");
					mails.Add(item);
				}
			}
			return mails;
		}
		public static List<Mail> ReadUserMailList(this MainForm main, User user)
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
					Mail item = new Mail(
						reader.ReadInt64("mailID"),
						reader.ReadString("title"),
						reader.ReadString("content")
					);
					mails.Add(item);
				}
			}
			return mails;
		}
		public static List<Item> ReadUserItems(this MainForm main, User user, PackageType type)
		{
			List<Item> items = new List<Item>();
			try
			{
				string sql;
				if (type == PackageType.Normal)
				{
					sql = "SELECT i.ID,i.ExpireDateTime,i.ItemClass,i.Collection,i.Count,i.Slot,e.Color1,e.Color2,e.Color3 "
						+ "FROM  Item as i left join Equippable e on e.ID = i.ID "
						+ "WHERE i.Collection<100 and i.OwnerID =" + user.CID + " ORDER BY i.Collection,i.Slot";
				}
				else if (type == PackageType.Cash)
				{
					sql = "SELECT i.ID,i.ExpireDateTime,i.ItemClass,i.Collection,i.Count,i.Slot,e.Color1,e.Color2,e.Color3 "
						+ "FROM  Item as i left join Equippable e on e.ID = i.ID "
						+ "WHERE i.Collection=100 and i.OwnerID =" + user.CID + " ORDER BY i.Collection,i.Slot";
				}
				else if (type == PackageType.Other)
				{
					sql = "SELECT i.ID,i.ExpireDateTime,i.ItemClass,i.Collection,i.Count,i.Slot,e.Color1,e.Color2,e.Color3 "
						+ "FROM  Item as i left join Equippable e on e.ID = i.ID "
						+ "WHERE (i.Collection=101 or i.Collection > 103 )and i.OwnerID =" + user.CID + " ORDER BY i.Collection,i.Slot";
				}
				else if (type == PackageType.Quest)
				{
					sql = "SELECT i.ID,i.ExpireDateTime,i.ItemClass,i.Collection,i.Count,i.Slot,e.Color1,e.Color2,e.Color3 "
						+ "FROM  Item as i left join Equippable e on e.ID = i.ID "
						+ "WHERE i.Collection=103 and i.OwnerID =" + user.CID + " ORDER BY i.Collection,i.Slot";
				}
				else
				{
					sql = "SELECT i.ID,i.ExpireDateTime,i.ItemClass,i.Collection,i.Count,i.Slot,e.Color1,e.Color2,e.Color3 "
						+ "FROM  Item as i left join Equippable e on e.ID = i.ID "
						+ "WHERE i.OwnerID =" + user.CID + " ORDER BY i.Collection,i.Slot";
				}
				using (DbDataReader reader = db.GetReader(sql))
				{
					while (reader != null && reader.Read())
					{
						Item item = new Item(reader.ReadInt64("ID"),
						                     reader.ReadString("itemClass"),
						                     ""// Convert.ToString(reader["itemType"])
						                    );
						string time = reader.ReadString("ExpireDateTime", null);
						if (time != null)
						{
							item.Time = time.Split(' ')[0];
						}
						else
						{
							item.Time = "无限期";
						}
						item.Collection = reader.ReadInt32("Collection");
						item.Slot = reader.ReadInt32("Slot");
						//  item.attrName = reader["Attribute"] == DBNull.Value ? null : Convert.ToString(reader["Attribute"]);
						//  item.attrValue = reader["Value"] == DBNull.Value ? null : Convert.ToString(reader["Value"]);
						item.Count = reader.ReadInt32("Count", 1);
						item.Color1  = reader.ReadInt32("Color1",0);
						item.Color2  = reader.ReadInt32("Color2",0);
						item.Color3  = reader.ReadInt32("Color3",0);
						items.Add(item);
					}
				}
				foreach (Item item in items)
				{
					using (DbDataReader reader2 = db.GetReader("SELECT * FROM ItemAttribute WHERE ItemID = " + item.ItemID))
					{
						List<ItemAttribute> attrs = new List<ItemAttribute>();
						while (reader2 != null && reader2.Read())
						{
							ItemAttribute attr = new ItemAttribute();
							try
							{
								attr.Type = reader2.ReadEnum<ItemAttributeType>("Attribute",ItemAttributeType.NONE);
								attr.Value = reader2.ReadString("Value");
								attr.Arg = reader2.ReadString("Arg");
								attr.Arg2 = reader2.ReadString("Arg2");
								attrs.Add(attr);
							}
							catch (Exception)
							{
							}

						}
						item.Attributes = attrs.ToArray<ItemAttribute>();
					}
				}
			}
			catch (Exception exception)
			{
				main.Error("读取物品错误\n" + exception);
			}
			return items;
		}
		#endregion

		#region 邮件
		public static void DeleteUserMail(this MainForm main, Mail mail)
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
		public static void DeleteSendMail(this MainForm main, Mail mail)
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

		#region 角色修改
		
		public static List<long> GetTitles(this MainForm main,User user){
			List<long> titles=new List<long>();
			using(DbDataReader reader=db.GetReader("select TitleID from Title where Acquired =1 and CID="+user.CID)){
				while(reader!=null&&reader.Read()){
					titles.Add(reader.ReadInt64("TitleID"));
				}
			}
			return titles;
		}
		
		public static bool AddTitle(this MainForm main,User user,TitleInfo title){
			return GetCurTitles(main, user, title.TitleID)>0;
		}
		public static void MaxSecondClass(this MainForm main, User user, string className)
		{
			try
			{
				if (db.ExcuteScalarSQL(string.Concat(new object[] {
				                                     	"select count(*) from Manufacture where ManufacturelID = N'", className, "' AND CID=", user.CID })) == 0)
				{
					db.ExcuteSQL(string.Concat(new object[] {
					                           	"insert into Manufacture(CID, ManufacturelID, Grade, [ExperiencePoint]) values(", user.CID, ",N'", className, "',4,3990000)" }));
				}
				else
				{
					db.ExcuteSQL(string.Concat(new object[] {
					                           	"update Manufacture set Grade = 4,ExperiencePoint = 3990000 where ManufacturelID = N'", className, "' and CID = ", user.CID }));
				}
			}
			catch (Exception ex)
			{
				main.Error("副职业满级错误\n"+ex);
			}
		}
		public static void MaxAllSecondClass(this MainForm main, User user)
		{
			string[] strArray = new string[] { "cooking", "metal_weapon", "heavy_armor", "light_armor", "sewing", "workmanship", "armor", "armor_dc", "spirit_stone", "gathering", "metal_weapon_dc", "sewing_dc", "workmanship_dc" };
			for (int i = 0; i < strArray.Length; i++)
			{
				main.MaxSecondClass(user, strArray[i]);
			}
		}

		public static bool ModUserLevel(this MainForm main, User user, int level)
		{
			if (user.level == level)
			{
				return true;
			}
			try
			{
				db.ExcuteSQL(string.Concat(new object[] { "update characterInfo set level = ", level, " where id = ", user.CID }));
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
		public static bool ModUserClass(this MainForm main, User user, ClassInfo cls)
		{
			if (user.Class.Index() == cls.Index())
			{
				return true;
			}
			try
			{
				db.ExcuteSQL(string.Concat(new object[] { "update characterInfo set Class = ", cls.Index(), " where id = ", user.CID }));
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
		public static bool ModUserName(this MainForm main, User user, string name)
		{
			if (db.ExcuteScalarSQL("select count(*) from characterInfo where Name = N'" + name + "'") == 0)
			{
				db.ExcuteSQL(string.Concat(new object[] { "update characterInfo set name = N'", name, "' where id = ", user.CID }));
				// this.output("将角色 [" + this.userList[this.userIndex].name + "] 角色名修改为 [" + this.txtUserName.Text + "] 成功!");
				return true;
			}
			else
			{
				return false;
				// this.output("角色名 [" + this.txtUserName.Text + "] 已存在!");
			}
		}
		private static string ToString(object obj)
		{
			if (obj == DBNull.Value)
				return "";
			return Convert.ToString(obj);
		}
		public static bool CheckName(this MainForm main, string name)
		{
			return db.ExcuteScalarSQL("select count(*) from characterInfo where Name = N'" + name + "'") == 0;
		}

		/// <summary>
		/// 
		/// 0 light
		/// 1 dark
		/// </summary>
		public static void SetGroupLevel(this MainForm main, User user, GroupInfo group,int level)
		{
			try
			{
				if (db.ExcuteScalarSQL("select count(*) from vocation where cid=" + user.CID) == 0)
				{
					db.ExcuteSQL(string.Concat(new object[] { "insert into vocation(CID,vocationClass,VocationLevel,VocationEXP,LastTransform) values(", user.CID, ",", (int)group, ","+level+",0,'", DateTime.Now.ToString(), "')" }));
				}
				else
				{
					db.ExcuteSQL("update vocation set vocationClass = "+ (int)group + ",VocationLevel = "+level+" where cid =" + user.CID);
				}
				if(level <=1){
					ResetGroupSkill(main, user);
				}
				//this.output("角色 [" + this.userList[this.userIndex].name + "] 光明骑士等级修改成功!");
			}
			catch (Exception exception)
			{
				main.Error("最大阵营技能错误\n"+exception);
			}
		}

		public static void ResetGroupSkill(this MainForm main, User user)
		{
			string strSQL = "delete from VocationSkill WHERE CID =" + user.CID;
			try
			{
				db.ExcuteSQL(strSQL);
			}
			catch (Exception exception)
			{
				main.Error("重置阵营技能错误\n" + exception);
			}
		}
		
		public static int GetCurTitles(this MainForm main,User user,long id){
			//[Title]
			string strSQL = "update Title set Acquired = 1 , AcquiredTime='"+DateTime.Now.ToString()+"'"
				+" WHERE CID =" + user.CID;
			if(id > 0){
				strSQL += " and TitleID="+id;
			}else{
				strSQL += " and Acquired = 0";
			}
			try
			{
				int count = db.ExcuteSQL(strSQL);
				if(count==0 && id>0){
					//插入
					count = db.ExcuteSQL("INSERT INTO Title"+
					                     "(CID,TitleID,Acquired,"+
					                     "AcquiredTime,AcquiredQuest,ExpireDateTime)"+
					                     " VALUES(" +
					                     user.CID+","+
					                     id+","+
					                     "1,'"+
					                     DateTime.Now.ToString()+"',NULL,NULL"+
					                     ")");
				}
				//清空记录
				strSQL = "delete from TitleGoalProgress WHERE CID =" + user.CID;
				if(id > 0){
					strSQL += " and TitleGoalID="+id;
				}
				db.ExcuteSQL(strSQL);
				return count;
			}
			catch (Exception exception)
			{
				main.Error("获取全部头衔\n" + exception);
			}
			return 0;
		}
		#endregion

		#region 发送
		/// <summary>
		/// 发送物品
		/// </summary>
		public static int SendItems(this MainForm main, User user, int count, params ItemClassInfo[] items)
		{
			if (user == null || items == null)
			{
				return 0;
			}
			int rs = 0;
			foreach (ItemClassInfo item in items)
			{
				if (item != null)
				{
					rs += SendItem(main, user, count, item.ItemClass, item.Name);
				}
			}
			return rs;
		}

		/// <summary>
		/// 发送物品
		/// </summary>
		public static int SendItem(this MainForm main, User user, int count, string item, string name)
		{
			if (user == null || string.IsNullOrEmpty(item))
			{
				return 0;
			}
			if (string.IsNullOrEmpty(name))
			{
				name = item;
			}
			return SendMail(main, user, name + "(" + count + ")", name + "(" + item + ":" + count + ")", count, item);
		}

		public static int SendMail(this MainForm main, User user, string title, string content, int count, string itemClass)
		{
			try
			{
				string strSQL = "heroes.dbo.HDV_SendItemMail";
				SqlParameter[] paras = new SqlParameter[8];
				paras[0] = new SqlParameter("@CID", SqlDbType.BigInt);
				paras[0].Direction = ParameterDirection.Input;
				paras[0].Value = user.CID;
				paras[1] = new SqlParameter("@ItemClassEx", SqlDbType.NVarChar);
				paras[1].Direction = ParameterDirection.Input;
				paras[1].Value = itemClass;
				paras[2] = new SqlParameter("@Count", SqlDbType.Int);
				paras[2].Direction = ParameterDirection.Input;
				paras[2].Value = count;
				paras[3] = new SqlParameter("@IsCharacterBinded", SqlDbType.Bit);
				paras[3].Direction = ParameterDirection.Input;
				paras[3].Value = 0;
				paras[4] = new SqlParameter("@MailTitle", SqlDbType.NVarChar);
				paras[4].Direction = ParameterDirection.Input;
				paras[4].Value = title;
				paras[5] = new SqlParameter("@MailContent", SqlDbType.NVarChar);
				paras[5].Direction = ParameterDirection.Input;
				paras[5].Value = content;
				paras[6] = new SqlParameter("@Result", SqlDbType.Int);
				paras[6].Direction = ParameterDirection.Output;
				paras[6].Value = 0;
				paras[7] = new SqlParameter("@CharacterName", SqlDbType.NVarChar);
				paras[7].Direction = ParameterDirection.Output;
				paras[7].Value = "";
				db.ExcuteSQL(strSQL, CommandType.StoredProcedure, paras);
				return 1;
			}
			catch (Exception)
			{
			}
			return 0;
		}
		#endregion

		#region 物品操作
		/// <summary>
		/// 删除物品
		/// </summary>
		public static bool DeleteItem(this MainForm main, User user,params Item[] items)
		{
			if (items != null)
			{
				try
				{
					foreach (Item item in items)
					{
						db.ExcuteSQL("DELETE FROM Item Where ID=" + item.ItemID);
					}
					return true;
				}
				catch (Exception)
				{
				}
			}
			return false;
		}
		/// <summary>
		/// 强化
		/// </summary>
		public static bool ModItemPower(this MainForm main, User user, Item item, int power)
		{
			try
			{
				if (power == 0)
				{
					db.ExcuteSQL(string.Concat(new object[] { "DELETE FROM ItemAttribute where itemID=", item.ItemID, "and Attribute = 'ENHANCE'" }));
					return true;
				}
				switch (db.ExcuteScalarSQL("select count(*) from ItemAttribute where itemID = " + item.ItemID + "and Attribute = 'ENHANCE'"))
				{
					case 0:
						db.ExcuteSQL(string.Concat(new object[] { "insert into ItemAttribute([ItemID], [Attribute], [Value], [Arg], [Arg2])VALUES(", item.ItemID, ",'ENHANCE',", power, ",0,0)" }));
						break;

					case 1:
						db.ExcuteSQL(string.Concat(new object[] { "update ItemAttribute set Value = ", power, " where itemID=", item.ItemID, "and Attribute = 'ENHANCE'" }));
						break;
				}
			}
			catch (Exception e)
			{
				main.Error("强化失败：" + e);
			}

			return true;
		}
		/// <summary>
		/// 无限时间
		/// </summary>
		public static bool UnLimitTime(this MainForm main, User user, params Item[] items)
		{
			if (items == null || items.Length == 0)
			{
				db.ExcuteSQL("UPDATE Item SET EXpireDateTime = NULL WHERE OwnerID =" + user.CID);
			}
			else
			{
				foreach (Item item in items)
				{
					db.ExcuteSQL("UPDATE Item SET EXpireDateTime = NULL WHERE OwnerID =" + user.CID + " and ID = " + item.ItemID);
				}
			}
			return true;
		}
		/// <summary>
		/// 品质最大
		/// </summary>
		public static bool MaxStar(this MainForm main, User user, params Item[] items)
		{
			if (items == null || items.Length == 0)
			{
				db.ExcuteSQL("UPDATE Item SET EXpireDateTime = NULL WHERE OwnerID =" + user.CID);
			}
			else
			{
				foreach (Item item in items)
				{
					MaxStart(main, item);
				}
			}
			return true;
		}
		private static void MaxStart(this MainForm main, Item item)
		{
			long itemID = item.ItemID;
			if (db.ExcuteScalarSQL("select count(*) from ItemAttribute where attribute = 'QUALITY' and ItemID = " + itemID) > 0)
			{
				db.ExcuteSQL("update ItemAttribute set Arg = 5 where ItemID = " + itemID);
			}
			else
			{
				db.ExcuteSQL("insert into ItemAttribute(ItemID,Attribute,Value,Arg,Arg2) values(" + itemID + ",'QUALITY','',5,0)");
			}
		}
		/// <summary>
		/// 附魔
		/// </summary>
		public static bool Enchant(this MainForm main, Item item, EnchantInfo attribute)
		{
			if (attribute == null || item == null)
			{
				return false;
			}
			string name = attribute.IsPrefix ? "PREFIX" : "SUFFIX";
			if (db.ExcuteScalarSQL("SELECT COUNT(*) FROM ItemAttribute ia LEFT JOIN Item i ON i.ID = ia.ItemID"
			                       + " WHERE (ia.Attribute = '" + name + "') AND i.ID =" + item.ItemID) == 0)
			{
				return db.ExcuteSQL(string.Concat(new object[] {
				                                  	"INSERT INTO ItemAttribute ([ItemID], [Attribute], [Value], [Arg], [Arg2]) VALUES (",
				                                  	item.ItemID, ", '"+name+"','", attribute.Class, "', '"+attribute.MaxArg+"', '0')" })) > 0;
			}
			else if (db.ExcuteSQL("UPDATE ItemAttribute SET [Value] ='"+ attribute.Class+ "',[Arg]='"+ attribute.MaxArg +
			                      "' WHERE ItemID =" + item.ItemID+ " AND Attribute = '"+name+"'") > 0)
			{
				return true;
			}
			return false;
		}
		public static bool CleanItemColor(this MainForm main, User user, params Item[] items)
		{
			if (items == null)
			{
				return false;
			}
			try
			{
				foreach (Item item in items)
				{
					string str = "UPDATE Equippable SET Color1 = 0,Color2 = 0,Color3 = 0  WHERE ID IN (SELECT e.ID FROM Item as i,Equippable as e WHERE i.OwnerID ="
						+ user.CID + " AND e.ID = " + item.ItemID + " AND e.ID = i.ID)";
					db.ExcuteSQL(str);
				}
				return true;
			}
			catch (Exception exception)
			{
				main.Error("修改物品颜色失败!\n" + exception);
			}
			return false;
		}
		/// <summary>
		/// 修改颜色
		/// </summary>
		public static bool ModItemColor(this MainForm main, User user, int color1, int color2, int color3,params Item[] items)
		{
			if (items == null)
			{
				return false;
			}
			try
			{
				foreach (Item item in items)
				{
					string str = "UPDATE Equippable SET ";
					if (color1 != 0)
					{
						str += " Color1 = " + color1 + ",";
					}
					if (color2 != 0)
					{
						str += " Color2 = " + color2 + ",";
					}
					if (color3 != 0)
					{
						str += " Color3 = " + color3 + ",";
					}
					if (str.EndsWith(","))
					{
						str = str.Substring(0, str.Length - 1);
					}
					string strSQL = str + " WHERE ID IN (SELECT e.ID FROM Item as i,Equippable as e WHERE i.OwnerID ="
						+ user.CID + " AND e.ID = " + item.ItemID + " AND e.ID = i.ID)";
					db.ExcuteSQL(strSQL);
				}
				return true;
			}
			catch (Exception exception)
			{
				main.Error("修改物品颜色失败!\n" + exception);
			}
			return false;
		}
		#endregion

	}
}