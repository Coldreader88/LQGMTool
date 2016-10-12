using GMTool.Bean;
using GMTool.Helper;
using GMTool.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMTool.Enums;
using GMTool.Extensions;
using GMTool.Dialog;


namespace GMTool
{
	public partial class MainForm : Form
	{
		#region Member
		private string DefTitle;
		public DbInfoHelper DataHelper { get; private set; }
		private ColorDialog colorDialog;
		public User CurUser { get; private set; }

		public int NormalCurItem { get; private set; }
		public int CashCurItem { get; private set; }

		private Item ColorItem;
		#endregion

		#region 窗体
		public MainForm()
		{
			CashCurItem = -1;
			NormalCurItem = -1;
			CurUser = new User();
			DataHelper = DbInfoHelper.Get();
			InitializeComponent();
			this.Text += " " + Application.ProductVersion.ToString();
			this.DefTitle = this.Text;
		}


		private void MainForm_Load(object sender, EventArgs e)
		{
			this.ReadSettings();
			this.list_users.Items.Clear();
			this.list_items_normal.Items.Clear();
			this.list_search.Items.Clear();
			this.tb_logcat.Text = "";
			this.AddTypes(this.cb_maincategory, this.cb_subcategory);
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.CloseDataBase();
		}
		#endregion

		#region common
		public bool CheckUser()
		{
			bool b = CurUser != null;
			if (!b)
			{
				this.Warnning("没有选择用户");
			}
			return b;
		}
		public void log(string text)
		{
			if (this.tb_logcat.Text.Length >= this.tb_logcat.MaxLength - 1024)
			{
				this.tb_logcat.Text = "";
			}
			this.tb_logcat.AppendText("\n" + DateTime.UtcNow.ToLongTimeString() + "  " + text + "\n");
		}
		#endregion

		#region 数据库
		private void btn_mssql_open_Click(object sender, EventArgs e)
		{
			SetAllEnable(false);
			if (this.btn_mssql_open.Text == "断开")
			{
				CurUser = null;
				this.CloseDataBase();

				SetMssqlEnable(true);
				AddUserList(new List<User>());
				AddSearchItemList(new List<ItemClassInfo>());
				AddItemList(this.list_items_normal, new List<Item>());
				AddItemList(this.list_items_cash, new List<Item>());
				AddItemList(this.list_items_other, new List<Item>());
				AddItemList(this.list_items_quest, new List<Item>());
				AddUserMails(new List<Mail>());
				AddSendMails(new List<Mail>());
			}
			else
			{
				if (!DataHelper.IsOpen)
				{
					if(!DataHelper.CheckFiles(this)){
						Application.Exit();
					}
					if (!DataHelper.Read())
					{
						this.Error("物品文本读取错误。\n请确保heroes.db3在程序目录下面");
						return;
					}
					//初始化菜单
					this.InitEnchantMenu(this.contentMenuEnchantPrefix, this.contentMenuEnchantSuffix, this.list_items_normal);
					this.InitCashEnchantMenu(this.contentMenuCashInnerEnchant, this.list_items_cash);
					this.InitModAttrMenu(this.contentMenuUserModAttr);
					//this.InitEnchantMenu(this.contentMenuCashInnerEnchant, null);
				}
				if (this.connectDataBase(this.tb_mssql_server.Text, this.tb_mssql_user.Text, this.tb_mssql_pwd.Text, this.tb_mssql_db.Text))
				{
					SetAllEnable(true);
					SetMssqlEnable(false);
					this.tb_logcat.Text = "";
					ReadUsers();
				}
				else
				{
					this.Error("连接数据库失败");
				}
			}
		}
		private void SetAllEnable(bool enable)
		{
			this.tab_items.Enabled = enable;
			this.tab_left.Enabled = enable;
			this.tab_mail.Enabled = enable;
			this.list_search.Enabled = enable;
			this.btn_senditem_send.Enabled = enable;
			this.btn_search_name.Enabled = enable;
			this.btn_search_id.Enabled = enable;
			this.tb_senditem_class.Enabled = enable;
			this.tb_senditem_name.Enabled = enable;
		}
		private void SetMssqlEnable(bool enable)
		{

			this.tb_mssql_server.Enabled = enable;
			this.tb_mssql_user.Enabled = enable;
			this.tb_mssql_db.Enabled = enable;
			this.tb_mssql_pwd.Enabled = enable;
			if (!enable)
			{
				this.btn_mssql_open.Text = "断开";
				this.tb_mssql_server.PasswordChar = '*';
				this.tb_mssql_db.PasswordChar = '*';
				this.tb_mssql_user.PasswordChar = '*';
			}
			else
			{
				this.btn_mssql_open.Text = "连接";
				this.tb_mssql_server.PasswordChar = '\0';
				this.tb_mssql_db.PasswordChar = '\0';
				this.tb_mssql_user.PasswordChar = '\0';
			}

		}
		#endregion

		#region 数据读取和添加
		public void ReadPackage(PackageType type)
		{
			if (!CheckUser()) return;
			switch (type)
			{
				case PackageType.Normal:
					AddItemList(this.list_items_normal, this.ReadUserItems(CurUser, PackageType.Normal));
					break;
				case PackageType.Cash:
					AddItemList(this.list_items_cash, this.ReadUserItems(CurUser, PackageType.Cash));
					break;
				case PackageType.Quest:
					AddItemList(this.list_items_quest, this.ReadUserItems(CurUser, PackageType.Quest));
					break;
				case PackageType.Other:
					AddItemList(this.list_items_other, this.ReadUserItems(CurUser, PackageType.Other));
					break;
				case PackageType.All:
					AddItemList(this.list_items_normal, this.ReadUserItems(CurUser, PackageType.Normal));
					AddItemList(this.list_items_cash, this.ReadUserItems(CurUser, PackageType.Cash));
					AddItemList(this.list_items_quest, this.ReadUserItems(CurUser, PackageType.Quest));
					AddItemList(this.list_items_other, this.ReadUserItems(CurUser, PackageType.Other));
					break;
			}
		}
		private void SetCurItems(Item item)
		{
			if (item != null)
			{
				if (ColorItem == null || ColorItem.ItemID != item.ItemID)
				{
					ColorItem = item;
					if (item.Color1 != 0)
					{
						if (!chk_lock_color.Checked)
						{
							this.tb_color1.Text = item.Color1.ColorString();
							this.tb_color2.Text = item.Color2.ColorString();
							this.tb_color3.Text = item.Color3.ColorString();
						}
					}
				}
				//this.tab_color.Text = this.tab_color.Text.Split(' ')[0] + " ("+item.ItemName+")";

				this.tb_senditem_name.Text = item.ItemName;
				this.tb_senditem_class.Text = item.ItemClass;
			}
		}
		private void SetCurItems(ItemClassInfo item)
		{
			if (item != null)
			{
				this.tb_senditem_name.Text = item.Name;
				this.tb_senditem_class.Text = item.ItemClass;
			}
		}
		private void AddSearchItemList(List<ItemClassInfo> items)
		{
			int count = items.Count;
			//TODO
			this.list_search.BeginUpdate();
			this.list_search.Items.Clear();
			this.lb_search.Text = this.lb_search.Text.Split(' ')[0] + " (" + count + ")";
			int index = -1;
			if (count >= 0)
			{
				ListViewItem[] vitems = new ListViewItem[count];
				for (int i = 0; i < count; i++)
				{
					ItemClassInfo t = items[i];
					vitems[i] = new ListViewItem();
					vitems[i].Text = t.Name == null ? t.ItemClass : t.Name;
					vitems[i].ToolTipText = t.ToString();
					vitems[i].Tag = t;
					if (i % 2 == 0)
						vitems[i].BackColor = Color.GhostWhite;
					else
						vitems[i].BackColor = Color.White;
				}
				this.list_search.Items.AddRange(vitems);
			}
			this.list_search.EndUpdate();
			this.list_search.GoToRow(index);
		}

		private void AddItemList(ListView listview, List<Item> items)
		{
			int count = items.Count;
			//TODO
			listview.BeginUpdate();
			listview.Items.Clear();
			if (listview == list_items_normal)
			{
				this.tab_items_normal.Text = this.tab_items_normal.Text.Split(' ')[0] + " (" + count + ")";
			}
			else if (listview == list_items_cash)
			{
				this.tab_items_cash.Text = this.tab_items_cash.Text.Split(' ')[0] + " (" + count + ")";
			}
			else if (listview == list_items_quest)
			{
				this.tab_items_quest.Text = this.tab_items_quest.Text.Split(' ')[0] + " (" + count + ")";
			}
			else if (listview == list_items_other)
			{
				this.tab_items_other.Text = this.tab_items_other.Text.Split(' ')[0] + " (" + count + ")";
			}
			int index = -1;
			if (count >= 0)
			{
				ListViewItem[] vitems = new ListViewItem[count];
				for (int i = 0; i < count; i++)
				{
					bool hascolor = listview == list_items_normal || listview == list_items_cash;
					bool has2category = listview == list_items_normal;
					bool fullname = listview == list_items_normal || listview == list_items_cash;
					vitems[i] = this.GetItemView(items[i], i, listview, fullname, hascolor, has2category);

					if ((listview == this.list_items_normal && i == NormalCurItem) || (listview == this.list_items_cash && i == CashCurItem))
					{
						index = i;
						vitems[i].Checked = true;
						vitems[i].Selected = true;
					}
				}
				listview.Items.AddRange(vitems);
			}
			listview.EndUpdate();
			listview.GoToRow(index);
		}

		public void ReadUsers()
		{
			AddUserList(this.ReadAllUsers());
		}
		private void AddUserList(List<User> users)
		{
			int count = users.Count;
			//TODO
			this.list_users.BeginUpdate();
			this.list_users.Items.Clear();
			this.tab_user.Text = this.tab_user.Text.Split(' ')[0] + " (" + count + ")";
			int index = -1;
			if (count >= 0)
			{
				ListViewItem[] items = new ListViewItem[count];
				for (int i = 0; i < count; i++)
				{
					User u = users[i];
					items[i] = new ListViewItem();
					items[i].Tag = u;
					items[i].Text = u.Name;
					if (CurUser != null && CurUser.CID == u.CID)
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
				this.list_users.Items.AddRange(items);
			}
			this.list_users.EndUpdate();
			this.list_users.GoToRow(index);
		}

		private void ReadMails()
		{
			AddUserMails(this.ReadUserMailList(CurUser));
			AddSendMails(this.ReadSendMailList(CurUser));
		}
		private void AddUserMails(List<Mail> mails)
		{
			int count = this.AddMails("[邮箱]", this.list_mail_user, mails);
			this.tab_mail_user.Text = this.tab_mail_user.Text.Split(' ')[0] + " (" + count + ")";
		}
		private void AddSendMails(List<Mail> mails)
		{
			int count = this.AddMails("[发送中]", this.list_mail_send, mails);
			this.tab_mail_send.Text = this.tab_mail_send.Text.Split(' ')[0] + " (" + count + ")";
		}

		#endregion

		#region 邮件列表菜单
		private ListView GetMailMenu(object sender)
		{
			ToolStripMenuItem menu = sender as ToolStripMenuItem;
			Control parent = null;
			ListView listview = null;
			if (menu != null)
			{
				parent = menu.GetMenuConrtol();
			}
			if (parent == this.list_mail_send)
			{
				listview = this.list_mail_send;
			}
			else if (parent == this.list_mail_user)
			{
				listview = this.list_mail_user;
			}
			return listview;
		}
		private void contentMenuRefreshMail_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ReadMails();
		}

		private void contentMenuDeleteMail_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ListView listview = GetMailMenu(sender);
			bool isSend = listview == this.list_mail_send;

			if (listview != null)
			{
				Mail[] mails = listview.GetSelectItems<Mail>();
				if (mails != null)
				{
					foreach (Mail mail in mails)
					{
						if (isSend)
						{
							this.DeleteSendMail(mail);
						}
						else
						{
							this.DeleteUserMail(mail);
						}
					}
				}
				ReadMails();
			}
		}

		private void contentMenuDeleteAllMails_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ListView listview = GetMailMenu(sender);
			bool isSend = listview == this.list_mail_send;
			if (listview != null)
			{
				Mail[] mails = listview.GetItems<Mail>();
				if (mails != null)
				{
					foreach (Mail mail in mails)
					{
						if (isSend)
						{
							this.DeleteSendMail(mail);
						}
						else
						{
							this.DeleteUserMail(mail);
						}
					}
				}
				ReadMails();
			}
		}
		#endregion

		#region 搜索列表菜单
		private void contentMenuSendItem1_Click(object sender, EventArgs e)
		{
			SendSelectItems(1);
		}

		private void contentMenuSendItem10_Click(object sender, EventArgs e)
		{
			SendSelectItems(10);
		}

		private void ContentMenuSendItem5Click(object sender, EventArgs e)
		{
			SendSelectItems(5);
		}

		private void ContentMenuSendItem20Click(object sender, EventArgs e)
		{
			SendSelectItems(20);
		}
		private void contentMenuSendItem100_Click(object sender, EventArgs e)
		{
			SendSelectItems(100);
		}
		private void SendSelectItems(int count)
		{
			if (!CheckUser()) return;
			ItemClassInfo[] items = list_search.GetSelectItems<ItemClassInfo>();
			if (items == null || items.Length == 0) return;
			int _count = this.SendItems(CurUser, count, items);
			if (_count > 0)
			{
				ReadMails();
				log("发送" + _count + "个物品成功");
			}
			else
			{
				this.Warnning("含有特殊物品，不能批量发送。\n名字带有{0}都是特殊物品");
			}
			
		}
		#endregion

		#region 用户列表菜单
		private void ContentMenuUserModApClick(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			using (UserAttributeDialog form = new UserAttributeDialog(this))
			{
				form.SetUser(CurUser, "AP", "AP");
				if (form.ShowDialog() == DialogResult.OK)
				{
					int ap = form.Value;
					if (ap > 0 && this.ModUserAP(CurUser, ap))
					{
						log("成功修改用户[" + CurUser.Name + "]AP为" + ap);
						ReadUsers();
					}
				}
			}
		}
		private void ContentMenuUserResetQuestClick(object sender, EventArgs e)
		{
			this.ResetQuest(CurUser);
		}
		private void ContentMenuUserTitlesClick(object sender, EventArgs e)
		{
			if (this.Question("是否获取全部头衔？"))
			{
				int count = this.GetAllTitles(CurUser);
				this.Info("完成" + count + "个头衔");
			}
		}
		private void contentMenuUserResetGroup_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			log("[" + CurUser.Name + "]阵营技能重置");
			this.ResetGroupSkill(CurUser);
		}
		private void contentMenuUserMaxDark_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			log("[" + CurUser.Name + "]黑暗阵营满级");
			this.SetGroupLevel(CurUser, GroupInfo.Dark, 40, true);
			ReadUsers();
		}

		private void contentMenuUserMaxLight_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			log("[" + CurUser.Name + "]光明阵营满级");
			this.SetGroupLevel(CurUser, GroupInfo.Light, 40, true);
			ReadUsers();
		}

		private void ContentMenuUserResetDarkGroupClick(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			log("[" + CurUser.Name + "]重置阵营技能");
			this.ResetGroupSkill(CurUser);
		}
		private void contentMenuUserMaxSecondClass_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			log("[" + CurUser.Name + "]全部副职业满级");
			this.MaxAllSecondClass(CurUser);
		}

		private void contentMenuUserModLevel_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			using (UserLevelDialog form = new UserLevelDialog(this))
			{
				form.SetUser(CurUser);
				if (form.ShowDialog() == DialogResult.OK)
				{
					int level = form.Level;
					if (level > 0 && this.ModUserLevel(CurUser, level))
					{
						log("成功修改用户[" + CurUser.Name + "]等级");
						ReadUsers();
					}
				}
			}
		}

		private void contentMenuUserModName_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			using (UserNameDialog dlg = new UserNameDialog(this))
			{
				dlg.SetUser(CurUser);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					try
					{
						string name = dlg.InputText;
						this.ModUserName(CurUser, name);
						log("成功修改[" + CurUser.Name + "]名字为[" + name + "]");
						ReadUsers();
					}
					catch (Exception ex)
					{
						this.Error("修改角色名字失败\n" + ex.Message);
					}
				}
			}
		}

		private void menuRefreshUsers_Click(object sender, EventArgs e)
		{
			AddUserList(this.ReadAllUsers());
		}
		#endregion

		#region 搜索相关

		private void contentMenuSendItemCopyIDs_Click(object sender, EventArgs e)
		{
			ItemClassInfo[] items= this.list_search.GetSelectItems<ItemClassInfo>();
			if (items != null)
			{
				StringBuilder sb = new StringBuilder();
				foreach (ItemClassInfo item in items)
				{
					sb.Append(item.ItemClass);
					sb.AppendLine();
				}
				Clipboard.SetDataObject(sb.ToString());
				this.Info("复制了"+items.Length+"个物品ID");
			}
		}
		private void btn_search_id_Click(object sender, EventArgs e)
		{
			AddSearchItemList(DataHelper.Searcher.SearchItems(null, tb_senditem_class.Text, cb_maincategory.Text, cb_subcategory.Text, CurUser));
		}

		private void btn_search_name_Click(object sender, EventArgs e)
		{
			AddSearchItemList(DataHelper.Searcher.SearchItems(tb_senditem_name.Text, null, cb_maincategory.Text, cb_subcategory.Text, CurUser));
		}

		private void btn_senditem_send_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			if (string.IsNullOrEmpty(tb_senditem_class.Text) || string.IsNullOrEmpty(tb_senditem_count.Text))
			{
				return;
			}
			try
			{
				int count = Convert.ToInt32(tb_senditem_count.Text);
				if (this.SendItem(CurUser, count, tb_senditem_class.Text, tb_senditem_name.Text, this.tb_senditem_value.Text) > 0)
				{
					ReadMails();
					log("发送成功:" + tb_senditem_name.Text);
				}
				else
				{
					log("发送失败:" + tb_senditem_name.Text);
				}
			}
			catch (Exception)
			{
				this.Error("数量不是一个数字");
			}
		}
		private void tb_senditem_class_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				if (tb_senditem_class.Enabled)
				{
					AddSearchItemList(DataHelper.Searcher.SearchItems(null, tb_senditem_class.Text, cb_maincategory.Text, cb_subcategory.Text, CurUser));
				}
			}
		}

		private void tb_senditem_name_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				if (tb_senditem_name.Enabled)
				{
					AddSearchItemList(DataHelper.Searcher.SearchItems(tb_senditem_name.Text, null, cb_maincategory.Text, cb_subcategory.Text, CurUser));
				}
			}
		}
		#endregion

		#region 选择
		private void list_users_SelectedIndexChanged(object sender, EventArgs e)
		{
			User user = this.list_users.GetSelectItem<User>();
			if (user != null)
			{
				this.NormalCurItem = 0;
				this.CashCurItem = 0;
				this.CurUser = user;
				this.Text = this.DefTitle + "  -  角色：" + user.Name;
				ReadMails();
				ReadPackage(PackageType.All);
				this.AddTitles(this.contentMenuUserAddTitle, this.GetTitles(CurUser));
				this.AddClasses(CurUser, this.contentMenuUserClasses);
				this.AddSkillBouns(CurUser, this.contentMenuItemMaxScore, this.list_items_cash);
			}
		}
		private void list_items_normal_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = this.list_items_normal.GetSelectIndex();
			if (index >= 0)
			{
				NormalCurItem = index;
			}
			Item item = this.list_items_normal.GetSelectItem<Item>();
			SetCurItems(item);
		}

		private void list_items_cash_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = this.list_items_cash.GetSelectIndex();
			if (index >= 0)
			{
				CashCurItem = index;
			}
			Item item = this.list_items_cash.GetSelectItem<Item>();
			SetCurItems(item);
		}

		private void list_search_SelectedIndexChanged(object sender, EventArgs e)
		{
			ItemClassInfo item = this.list_search.GetSelectItem<ItemClassInfo>();
			SetCurItems(item);
		}
		#endregion

		#region 颜色
		private void tb_color1_TextChanged(object sender, EventArgs e)
		{
			this.lb_color1.BackColor = tb_color1.Text.GetColor();
		}

		private void tb_color2_TextChanged(object sender, EventArgs e)
		{
			this.lb_color2.BackColor = tb_color2.Text.GetColor();
		}

		private void tb_color3_TextChanged(object sender, EventArgs e)
		{
			this.lb_color3.BackColor = tb_color3.Text.GetColor();
		}

		private Color SelectColor(Color def1, Color def2, Color def3, Color def)
		{
			if (!CheckUser()) return def;
			if (colorDialog == null)
			{
				colorDialog = new ColorDialog();
				colorDialog.FullOpen = true;
				colorDialog.AnyColor = true;
			}
			//是否显示ColorDialog有半部分，运行一下就很了然了
			colorDialog.Color = def;// new int[] { def1.ToArgb() , def2 .ToArgb(), def3.ToArgb()};//设置自定义颜色
			if (colorDialog.ShowDialog() == DialogResult.OK)//确定事件响应
			{
				return colorDialog.Color;
			}
			return def;
		}

		private void chk_lock_color_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chk_lock_color.Checked)
			{
				SetColorEnable(false);
			}
			else
			{
				SetColorEnable(true);
			}
		}

		private void SetColorEnable(bool enable)
		{
			this.lb_color1.Enabled = enable;
			this.lb_color2.Enabled = enable;
			this.lb_color3.Enabled = enable;
			this.tb_color1.Enabled = enable;
			this.tb_color2.Enabled = enable;
			this.tb_color3.Enabled = enable;
		}

		private void lb_color1_Click(object sender, EventArgs e)
		{
			Color color = SelectColor(lb_color1.BackColor, lb_color2.BackColor, lb_color3.BackColor, lb_color1.BackColor);
			if (color != lb_color1.BackColor)
			{
				lb_color1.BackColor = color;
			}
		}

		private void lb_color2_Click(object sender, EventArgs e)
		{
			Color color = SelectColor(lb_color1.BackColor, lb_color2.BackColor, lb_color3.BackColor, lb_color2.BackColor);
			if (color != lb_color2.BackColor)
			{
				lb_color2.BackColor = color;
			}
		}

		private void lb_color3_Click(object sender, EventArgs e)
		{
			Color color = SelectColor(lb_color1.BackColor, lb_color2.BackColor, lb_color3.BackColor, lb_color3.BackColor);
			if (color != lb_color3.BackColor)
			{
				lb_color3.BackColor = color;
			}
		}

		#endregion

		#region 普通物品菜单
		private void contentMenuItemRefesh_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ReadPackage(PackageType.Normal);
		}

		private void ContentMenuItemCountClick(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			Item item = list_items_normal.GetSelectItem<Item>();
			if (item == null)
			{
				return;
			}
			using (ItemModCountDialog dlg = new ItemModCountDialog(this))
			{
				dlg.SetUserAndItem(CurUser, item);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					if (this.ModItemCount(CurUser, item, dlg.Count))
					{
						ReadPackage(PackageType.Normal);
					}
				}
			}
		}
		private void contentMenuDeleteItems_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			if (this.DeleteItem(CurUser, this.list_items_normal.GetSelectItems<Item>()))
			{
				ReadPackage(PackageType.Normal);
			}
		}

		private void contentMenuItemPower5_Click(object sender, EventArgs e)
		{
			PowerItem(5);
		}

		private void contentMenuItemPower10_Click(object sender, EventArgs e)
		{
			PowerItem(10);
		}

		private void contentMenuItemPower12_Click(object sender, EventArgs e)
		{
			PowerItem(12);
		}

		private void contentMenuItemPower13_Click(object sender, EventArgs e)
		{
			PowerItem(13);
		}

		private void contentMenuItemPower15_Click(object sender, EventArgs e)
		{
			PowerItem(15);
		}

		private void contentMenuItemPower0_Click(object sender, EventArgs e)
		{
			PowerItem(0);
		}
		private void PowerItem(int level)
		{
			if (!CheckUser()) return;
			if (this.ModItemPower(CurUser, this.list_items_normal.GetSelectItem<Item>(), level))
			{
				ReadPackage(PackageType.Normal);
			}
		}
		private void contentMenuItemMaxStar_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			if (this.ModItemStarMax(CurUser, this.list_items_normal.GetSelectItems<Item>()) > 0)
			{
				ReadPackage(PackageType.Normal);
			}
		}

		private void contentMenuItemUnLimitTime_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			if (this.ModItemTime(CurUser, this.list_items_normal.GetSelectItems<Item>()))
			{
				ReadPackage(PackageType.Normal);
			}
		}
		private void contentMenuColor1_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			this.ModItemColor(CurUser, lb_color1.BackColor.ToArgb(), 0, 0, this.list_items_normal.GetSelectItems<Item>());
			ReadPackage(PackageType.Normal);
		}

		private void contentMenuColor2_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			this.ModItemColor(CurUser, 0, lb_color2.BackColor.ToArgb(), 0, this.list_items_normal.GetSelectItems<Item>());
			ReadPackage(PackageType.Normal);
		}

		private void contentMenuColor3_Click(object sender, EventArgs e)
		{
			if (this.ModItemColor(CurUser, 0, 0, lb_color3.BackColor.ToArgb(), this.list_items_normal.GetSelectItems<Item>()))
			{
				ReadPackage(PackageType.Normal);
			}
		}

		private void contentMenuColorAll_Click(object sender, EventArgs e)
		{
			if (this.ModItemColor(CurUser,
			                      lb_color1.BackColor.ToArgb(), lb_color2.BackColor.ToArgb(), lb_color3.BackColor.ToArgb(),
			                      this.list_items_normal.GetSelectItems<Item>()))
			{
				ReadPackage(PackageType.Normal);
			}
		}


		/*
        private void contentMennuAllMaxStar_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            if (this.MaxStar(CurUser, GetItems(sender)))
            {
                ReadItems();
            }
        }
        private void contentMenuItemAllUnLimitTime_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            if (this.UnLimitTime(CurUser, GetItems(sender)))
            {
                ReadItems();
            }
        }*/
		#endregion

		#region 现金背包菜单
		private void contentMenuCashDelete_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			if (this.DeleteItem(CurUser, this.list_items_cash.GetSelectItems<Item>()))
			{
				ReadPackage(PackageType.Cash);
			}
		}
		private void ContentMenuCashCountClick(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			Item item = list_items_cash.GetSelectItem<Item>();
			if (item == null)
			{
				return;
			}
			using (ItemModCountDialog dlg = new ItemModCountDialog(this))
			{
				dlg.SetUserAndItem(CurUser, item);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					if (this.ModItemCount(CurUser, item, dlg.Count))
					{
						ReadPackage(PackageType.Cash);
					}
				}
			}
		}

		private void contentMenuCashRefresh_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ReadPackage(PackageType.Cash);
		}
		private void contentMenuCashColorAll_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			this.ModItemColor(CurUser, lb_color1.BackColor.ToArgb(), lb_color2.BackColor.ToArgb(), lb_color3.BackColor.ToArgb(), this.list_items_cash.GetSelectItems<Item>());
			ReadPackage(PackageType.Cash);
		}

		private void contentMenuCashColor3_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			this.ModItemColor(CurUser, 0, 0, lb_color3.BackColor.ToArgb(), this.list_items_cash.GetSelectItems<Item>());
			ReadPackage(PackageType.Cash);
		}

		private void contentMenuCashColor2_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			this.ModItemColor(CurUser, 0, lb_color2.BackColor.ToArgb(), 0, this.list_items_cash.GetSelectItems<Item>());
			ReadPackage(PackageType.Cash);
		}

		private void contentMenuCashColor1_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			this.ModItemColor(CurUser, lb_color1.BackColor.ToArgb(), 0, 0, this.list_items_cash.GetSelectItems<Item>());
			ReadPackage(PackageType.Cash);
		}

		private void contentMenuCashUnlimitTime_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			if (this.ModItemTime(CurUser, this.list_items_cash.GetSelectItems<Item>()))
			{
				ReadPackage(PackageType.Cash);
			}
		}
		private void contentMenuCashUnlimit_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			if (this.ModItemTime(CurUser, this.list_items_cash.GetSelectItems<Item>()))
			{
				ReadPackage(PackageType.Cash);
			}
		}
		#endregion

		#region 任务/隐藏物品
		private ListView GetItemMenu(object sender)
		{
			ToolStripMenuItem menu = sender as ToolStripMenuItem;
			Control parent = null;
			ListView listview = null;
			if (menu != null)
			{
				parent = menu.GetMenuConrtol();
			}
			if (parent == this.list_items_other)
			{
				listview = this.list_items_other;
			}
			else if (parent == this.list_items_quest)
			{
				listview = this.list_items_quest;
			}
			return listview;
		}
		private void contentMenuQuestUnlimit_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ListView listview = GetItemMenu(sender);
			if (listview == null) return;
			if (this.ModItemTime(CurUser, listview.GetSelectItems<Item>()))
			{
				contentMenuOtherRefresh_Click(sender, e);
			}
		}

		private void contentMenuQuestDelete_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ListView listview = GetItemMenu(sender);
			if (listview == null) return;
			if (this.DeleteItem(CurUser, listview.GetSelectItems<Item>()))
			{
				contentMenuOtherRefresh_Click(sender, e);
			}
		}
		private void contentMenuOtherRefresh_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ListView listview = GetItemMenu(sender);
			if (listview == null) return;
			if (listview == this.list_items_quest)
			{
				ReadPackage(PackageType.Quest);
			}
			else
			{
				ReadPackage(PackageType.Other);
			}
		}

		private void ContentMenuOtherCountClick(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ListView listview = GetItemMenu(sender);
			if (listview == null) return;
			Item item = listview.GetSelectItem<Item>();
			if (item == null)
			{
				return;
			}
			using (ItemModCountDialog dlg = new ItemModCountDialog(this))
			{
				dlg.SetUserAndItem(CurUser, item);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					if (this.ModItemCount(CurUser, item, dlg.Count))
					{
						contentMenuOtherRefresh_Click(sender, e);
					}
				}
			}

		}

		#endregion

	}
}
