
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
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
		private const string SQL_QUERY_USERS = "select * from CharacterInfo WHERE DeleteTime is NULL and CreateTime >= '2016-01-01' ORDER BY Name, CreateTime;";
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
	}
}
