
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
using System.Data;
using System.IO;
using System.Threading;
using Vindictus.Bean;
using Vindictus.Enums;
using System.Data.SqlClient;

namespace Vindictus.Extensions
{
	public static class MainFormExtension
	{
		public delegate void _AddMenus(ToolStripDropDownItem menuitem, ToolStripDropDownItem[] items);
		public static void AddMenus(ToolStripDropDownItem menuitem, ToolStripDropDownItem[] items){
			menuitem.DropDownItems.Clear();
			menuitem.DropDownItems.AddRange(items);
		}
		#region posttask
		public static void PostTask(this Form form, Action<IWaitDialog> action){
			if(action==null)return;
			using(WaitDialog dlg=new WaitDialog()){
				dlg.Closing+= delegate(object sender, CancelEventArgs e) {
					if(!dlg.CanClose){
						form.Info(R.TipStopTask);
						e.Cancel=true;
					}
				};
				dlg.Shown += delegate {
					Thread thread=new Thread(()=>
					                         {
					                         	action(dlg);
					                         	dlg.CanClose = true;
					                         	dlg.CloseDialog();
					                         });
					thread.IsBackground=true;
					thread.Start();
				};
				dlg.ShowDialog();
			}
		}
		#endregion
		
		#region add user list
		public static ListViewItem[] GetUsersItems(this List<User> users,User user,out int index){
			int count = users.Count;
//			listView.BeginUpdate();
//			listView.Items.Clear();
//			TabUser.Text = TabUser.Text.Split(' ')[0] + " (" + count + ")";
			index = 0;
			ListViewItem[] items = new ListViewItem[count];
			if (count >= 0)
			{
				for (int i = 0; i < count; i++)
				{
					User u = users[i];
					items[i] = new ListViewItem();
					items[i].Tag = u;
					items[i].Text = u.Name;
					if (user != null && user.CID == u.CID)
					{
						index = i;
						items[i].Checked = true;
						items[i].Selected = true;
					}
					if (i % 2 == 0)
						items[i].BackColor = Color.GhostWhite;
					else
						items[i].BackColor = Color.White;
					items[i].SubItems.Add("" + u.Class.Name());
					items[i].SubItems.Add("" + u.level);
					items[i].ToolTipText = u.ToLongString();
				}
//				listView.Items.AddRange(items);
			}
//			listView.EndUpdate();
//			listView.GoToRow(index);
			return items;
		}
		#endregion
		public static ListViewItem[] ToMailListViewItems(this List<Mail> mails)
		{
			int count = mails.Count;
			ListViewItem[] items = new ListViewItem[count];
			if (count >= 0)
			{
				for (int i = 0; i < count; i++)
				{
					Mail u = mails[i];
					items[i] = new ListViewItem();
					items[i].Tag = u;
					items[i].Text = u.Title;
					if (i % 2 == 0)
						items[i].BackColor = Color.GhostWhite;
					else
						items[i].BackColor = Color.White;
					items[i].ToolTipText = u.ToString();
				}
			}
			return items;
		}
		public static void RefeshPackage(this MainForm form,PackageType type){
			
		}
		public static void AddSearchItemList(this MainForm form,ListView listview,Control label, List<ItemClassInfo> items)
		{
			int count = items.Count;
//			form.Info("count="+count);
			listview.BeginUpdate();
			listview.Items.Clear();
			if(label!=null){
				label.Text = label.Text.Split(' ')[0] + " (" + count + ")";
			}
			if (count >= 0)
			{
				var vitems = new ListViewItem[count];
				for (int i = 0; i < count; i++)
				{
					ItemClassInfo t = items[i];
					vitems[i] = new ListViewItem();
					vitems[i].Text = t.Name ?? t.ItemClass;
					vitems[i].ToolTipText = t.ToString();
					vitems[i].Tag = t;
					if (i % 2 == 0)
						vitems[i].BackColor = Color.GhostWhite;
					else
						vitems[i].BackColor = Color.White;
				}
				listview.Items.AddRange(vitems);
			}
			listview.EndUpdate();
//			listview.GoToRow(0);
		}
		#region user
		public static void ResetQuest(this MainForm main,User user=null){
			string SQL = "Update Quest set TodayPlayCount = 0";
			if(user!=null){
				SQL += " WHERE OwnerID="+user.CID;
			}
			try{
				main.Db.ExcuteSQL(SQL);
			}catch(Exception e){
				main.Error("ResetQuest\n"+e);
			}
		}

		public static void MaxSecondClass(this MainForm main, User user, string className)
		{
			try
			{
				if (main.Db.ExcuteScalarSQL(string.Concat(new object[] {
				                                          	"select count(*) from Manufacture where ManufacturelID = N'", className, "' AND CID=", user.CID })) == 0)
				{
					main.Db.ExcuteSQL(string.Concat(new object[] {
					                                	"insert into Manufacture(CID, ManufacturelID, Grade, [ExperiencePoint]) values(", user.CID, ",N'", className, "',4,1000000)" }));
				}
				else
				{
					main.Db.ExcuteSQL(string.Concat(new object[] {
					                                	"update Manufacture set Grade = 4,ExperiencePoint = 1000 where ManufacturelID = N'", className, "' and CID = ", user.CID }));
				}
			}
			catch (Exception ex)
			{
				main.Error("MaxSecondClass\n"+ex);
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
		public static bool CheckName(this MainForm main, string name)
		{
			return main.Db.ExcuteScalarSQL("select count(*) from characterInfo where Name = N'" + name + "'") == 0;
		}
		/// <summary>
		/// 
		/// 0 light
		/// 1 dark
		/// </summary>
		public static void SetGroupLevel(this MainForm main, User user, GroupInfo group,int level=1,bool reset=false)
		{
			if(user.level<40)return;
			try
			{
				if (main.Db.ExcuteScalarSQL("select count(*) from vocation where cid=" + user.CID) == 0)
				{
					main.Db.ExcuteSQL(string.Concat(new object[] { "insert into vocation(CID,vocationClass,VocationLevel,VocationEXP,LastTransform) values(", user.CID, ",", (int)group, ","+level+",0,'", DateTime.Now.ToString(), "')" }));
				}
				else
				{
					main.Db.ExcuteSQL("update vocation set vocationClass = "+ (int)group + ",VocationLevel = "+level+" where cid =" + user.CID);
				}
				if (reset)
				{
					ResetGroupSkill(main, user);
				}
				//this.output("角色 [" + this.userList[this.userIndex].name + "] 光明骑士等级修改成功!");
			}
			catch (Exception exception)
			{
				main.Error("SetGroupLevel\n"+exception);
			}
		}

		public static void ResetGroupSkill(this MainForm main, User user)
		{
			if(user.level<40)return;
			string strSQL = "delete from VocationSkill WHERE CID =" + user.CID;
			try
			{
				main.Db.ExcuteSQL(strSQL);
			}
			catch (Exception exception)
			{
				main.Error("ResetGroupSkill\n" + exception);
			}
		}
		public static int GetAllTitles(this MainForm main,User user){
			int count = 0;
			TitleInfo[] titles = main.DataHelper.GetTitles();
			foreach (TitleInfo cls in titles)
			{
				if (!user.IsEnable(cls.ClassRestriction))
				{
					continue;
				}
				if (cls.OnlyClass != ClassInfo.UnKnown)
				{
					if (user.Class != cls.OnlyClass)
					{
						continue;
					}
				}
				count += UserAddTitle(main, user, cls.TitleID);
			}
			return count;
		}
		
		public static bool AddTitle(this MainForm main,User user,TitleInfo title){
			return UserAddTitle(main, user, title.TitleID)>0;
		}
		public static int UserAddTitle(this MainForm main,User user,long titleId){
			//[Title]

			string strSQL = "update Title set Acquired = 1 , AcquiredTime='"+DateTime.Now.ToString()
				+"' WHERE CID =" + user.CID+" and TitleID="+titleId;
			try
			{
				int count = main.Db.ExcuteSQL(strSQL);
				if(count==0){
					//插入
					count = main.Db.ExcuteSQL("INSERT INTO Title"+
					                          "(CID,TitleID,Acquired,"+
					                          "AcquiredTime,AcquiredQuest,ExpireDateTime)"+
					                          " VALUES(" +
					                          user.CID+","+
					                          titleId+","+
					                          "1,'"+
					                          DateTime.Now.ToString()+"',NULL,NULL"+
					                          ")");
				}
				//清空记录
				strSQL = "delete from TitleGoalProgress "+
					"WHERE CID =" + user.CID+" and TitleGoalID="+titleId;
				main.Db.ExcuteSQL(strSQL);
				return count;
			}
			catch (Exception exception)
			{
				main.Error("UserAddTitle\n" + exception);
			}
			return 0;
		}
		#endregion
		
		#region 头衔
		public static List<long> GetTitles(this MainForm main,User user){
			List<long> titles=new List<long>();
			using(DbDataReader reader=main.Db.GetReader("select TitleID from Title where Acquired =1 and CID="+user.CID)){
				while(reader!=null&&reader.Read()){
					titles.Add(reader.ReadInt64("TitleID"));
				}
			}
			return titles;
		}

		static Thread TitleThread;
		public static void AddTitles(this MainForm main, ToolStripDropDownItem menuitem)
		{
			List<long> titleIds= main.GetTitles(main.CurUser);
			if(TitleThread!=null){
				TitleThread.Interrupt();
			}
			TitleThread=new Thread(
				()=>{
					TitleInfo[] titles = main.DataHelper.GetTitles();
					int k = 0;
					User user = main.CurUser;
					if (user == null) return;
					List<ToolStripDropDownItem> items=new List<ToolStripDropDownItem>();
					ToolStripMenuItem level = new ToolStripMenuItem("lv." + (k * 10 + 1) + "-" + ((k + 1) * 10));
					items.Add(level);
					int max = 20;
					int i = 0;
					foreach (TitleInfo cls in titles)
					{
						if (!user.IsEnable(cls.ClassRestriction))
						{
							continue;
						}
						if (cls.OnlyClass != ClassInfo.UnKnown)
						{
							if (user.Class != cls.OnlyClass)
							{
								continue;
							}
						}
						
						if (cls.RequiredLevel <= (k + 1) * 10)
						{
							if (i % max == 0 && i >= max)
							{
								level = new ToolStripMenuItem("lv." + (k * 10 + 1) + "-" + ((k + 1) * 10) + "(" + (i / max + 1) + ")");
								items.Add(level);
							}
							i++;
							ToolStripMenuItem tsmi = new ToolStripMenuItem(cls.ToShortString());
							tsmi.Tag = cls;
							tsmi.ToolTipText = cls.ToString();

							if (titleIds.Contains(cls.TitleID))
							{
								tsmi.Checked = true;
							}
							else
							{
								tsmi.Click += (object sender, EventArgs e) => {
									if (!main.CheckUser()) return;
									ToolStripMenuItem menu = sender as ToolStripMenuItem;
									if (menu != null && menu.Tag != null)
									{
										TitleInfo info = (TitleInfo)menu.Tag;
//								if (!main.CurUser.IsEnable(info.ClassRestriction))
//								{
//									main.Info("该头衔不适合当前职业");
//									return;
//								}
										//
										main.AddTitle(main.CurUser, info);
										main.ReadUsers(false);
									}
								};
							}
							level.DropDownItems.Add(tsmi);
						}
						else
						{
							i = 0;
							k = cls.RequiredLevel / 10;
							level = new ToolStripMenuItem("lv." + (k * 10 + 1) + "-" + ((k + 1) * 10));
							items.Add(level);
						}
					}
					main.Invoke(new _AddMenus(AddMenus), new object[]{menuitem, items.ToArray()});
				});
			TitleThread.IsBackground =true;
			TitleThread.Start();
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
					rs += SendItem(main, user, count, item.ItemClass, item.Name, null);
				}
			}
			return rs;
		}

		/// <summary>
		/// 发送物品
		/// </summary>
		public static int SendItem(this MainForm main, User user, int count, string item, string name,string value)
		{
			if (user == null || string.IsNullOrEmpty(item))
			{
				return 0;
			}
            if (!string.IsNullOrEmpty(value))
            {
                item += "[VALUE:" + value + "]";
            }
            if (string.IsNullOrEmpty(name))
			{
				name = item;
			}
			return SendMail(main, user, name + "(" + count + ")", name + "\\n(" + item + ":" + count + ")", count, item);
		}

		public static int SendMail(this MainForm main, User user, string title, string content, int count, string itemClass)
		{
			try
			{
				string strSQL = "heroes.dbo.HDV_SendItemMail";
				var paras = new SqlParameter[8];
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
				main.Db.ExcuteSQL(strSQL, CommandType.StoredProcedure, paras);
				return 1;
			}
			catch (Exception)
			{
			}
			return 0;
		}
		#endregion
	}
}
