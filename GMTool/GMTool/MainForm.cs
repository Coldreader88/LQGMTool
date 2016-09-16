using GMTool.Bean;
using GMTool.Helper;
using LY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMTool
{
    public partial class MainForm : Form
    {
        private string DefTitle;
        private ItemClassInfoHelper txtHelper;
        public User CurUser { get; private set; }

        private Item NormalCurItem=null;
        private Item CashCurItem = null;
        #region 窗体
        public MainForm()
        {
            CurUser = new User(0, 0, 0, "", 0, 1);
            txtHelper = new ItemClassInfoHelper("./heroes_text_taiwan.txt", "./heroes.db3");
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
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.CloseDataBase();
        }
        #endregion

        #region 数据库
        private void btn_mssql_open_Click(object sender, EventArgs e)
        {
            this.list_items_normal.Enabled = false;
            this.list_users.Enabled = false;
            this.tab_mail.Enabled = false;
            this.list_search.Enabled = false;
            this.btn_senditem_send.Enabled = false;
            this.btn_senditem_send_other.Enabled = false;
            this.btn_search_name.Enabled = false;
            this.btn_search_id.Enabled = false;
            this.tb_senditem_class.Enabled = false;
            this.tb_senditem_name.Enabled = false;
            this.list_items_cash.Enabled = false;
            if (this.btn_mssql_open.Text == "断开")
            {
                this.CloseDataBase();
                this.btn_mssql_open.Text = "连接";
                this.tb_mssql_server.Enabled = true;
                this.tb_mssql_user.Enabled = true;
                this.tb_mssql_db.Enabled = true;

                this.tb_mssql_pwd.Enabled = true;
                this.tb_mssql_server.PasswordChar = '\0';
                this.tb_mssql_db.PasswordChar = '\0';
                this.tb_mssql_user.PasswordChar = '\0';
                AddUserList(new List<User>());
                AddSearchItemList(new List<Item>());
                AddItemList(this.list_items_normal, new List<Item>());
                AddItemList(this.list_items_cash, new List<Item>());
                AddUserMails(new List<Mail>());
                AddSendMails(new List<Mail>());
            }
            else
            {
                if (!txtHelper.IsOpen)
                {
                    if (!txtHelper.Read())
                    {
                        this.Error("物品文本读取错误。\n请确保heroes_text_taiwan.txt和heroes.db3在程序目录下面");
                        return;
                    }
                }
                if (this.connectDataBase(this.tb_mssql_server.Text, this.tb_mssql_user.Text, this.tb_mssql_pwd.Text, this.tb_mssql_db.Text))
                {
                    this.list_items_normal.Enabled = true;
                    this.list_items_cash.Enabled = true;
                    this.list_users.Enabled = true;
                    this.tab_mail.Enabled = true;
                    this.list_search.Enabled = true;
                    this.btn_senditem_send.Enabled = true;
                    this.btn_senditem_send_other.Enabled = true;
                    this.btn_search_name.Enabled = true;
                    this.btn_search_id.Enabled = true;
                    this.tb_senditem_class.Enabled = true;
                    this.tb_senditem_name.Enabled = true;

                    this.tb_mssql_server.Enabled = false;
                    this.tb_mssql_user.Enabled = false;
                    this.tb_mssql_db.Enabled = false;
                    this.tb_mssql_pwd.Enabled = false;
                    this.tb_mssql_server.PasswordChar = '*';
                    this.tb_mssql_db.PasswordChar = '*';
                    this.tb_mssql_user.PasswordChar = '*';

                    this.btn_mssql_open.Text = "断开";
                    this.tb_logcat.Text = "";
                    AddUserList(this.ReadUserList());
                }
                else
                {
                    this.Error("连接数据库失败");
                }
            }
        }
        #endregion

        #region 数据读取和添加
        private void ReadItems()
        {

            AddItemList(this.list_items_normal,this.ReadUserItems(CurUser, PackType.Normal));
            AddItemList(this.list_items_cash,this.ReadUserItems(CurUser, PackType.Cash));
        }
        private void AddSearchItemList(List<Item> items)
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
                for(int i=0;i< count; i++)
                {
                    vitems[i] = new ListViewItem();
                }
                this.list_search.Items.AddRange(vitems);
            }
            this.list_search.EndUpdate();
            this.list_search.GoToRow(index);
        }
        private void AddItemList(ListView listview,List<Item> items)
        {
            int count = items.Count;
            //TODO 
            listview.BeginUpdate();
            listview.Items.Clear();
            bool cash = false;
            if (listview == list_items_normal)
            {
                this.tab_items_normal.Text = this.tab_items_normal.Text.Split(' ')[0] + " (" + count + ")";
            }
            else
            {
                cash = true;
                this.tab_items_cash.Text = this.tab_items_cash.Text.Split(' ')[0] + " (" + count + ")";
            }
            int index = -1;
            if (count >= 0)
            {
                ListViewItem[] vitems = new ListViewItem[count];
                for (int i = 0; i < count; i++)
                {
                    Item t = items[i];
                    ItemClassInfo info = txtHelper.Get(t.ItemClass);
                    t.Attach(info);
                    vitems[i] = new ListViewItem();
                    vitems[i].UseItemStyleForSubItems = false;
                    vitems[i].Tag = t;
                    vitems[i].Text = (t.ItemName == null ? t.ItemClass : t.ItemName);
                    if (NormalCurItem != null && NormalCurItem.ItemID == t.ItemID)
                    {
                        index = i;
                        vitems[i].Checked = true;
                        vitems[i].Selected = true;
                    }
                    vitems[i].SubItems.Add("" + t.Count);
                    vitems[i].SubItems.Add("" + t.ItemType);
                    vitems[i].SubItems.Add("");// +(t.Color1==0?"":"#"+t.Color1.ToString("x")));
                    vitems[i].SubItems.Add("");// + (t.Color2 == 0 ? "" : "#" + t.Color2.ToString("x")));
                    vitems[i].SubItems.Add("");// + (t.Color3 == 0 ? "" : "#" + t.Color3.ToString("x")));
                    if (!cash)
                    {
                        if (t.attrName == "ENHANCE")
                        {
                            vitems[i].SubItems.Add("" + t.attrValue);
                        }
                        else
                        {
                            vitems[i].SubItems.Add("");
                        }
                    }
                    else
                    {
                        if (t.attrName == "PREFIX")
                        {
                            vitems[i].SubItems.Add("" + t.attrValue);
                        }
                        else
                        {
                            vitems[i].SubItems.Add("");
                        }
                    }

                    vitems[i].SubItems.Add("" + t.Time);
                    //      vitems[i].SubItems[2].ForeColor = Color.White;
                    //     vitems[i].SubItems[3].ForeColor = Color.White;
                    //     vitems[i].SubItems[4].ForeColor = Color.White;
                    vitems[i].SubItems[3].BackColor = System.Drawing.ColorTranslator.FromHtml((t.Color1 == 0 ? "#00ffffff" : "#" + t.Color1.ToString("X")));
                    vitems[i].SubItems[4].BackColor = System.Drawing.ColorTranslator.FromHtml((t.Color2 == 0 ? "#00ffffff" : "#" + t.Color2.ToString("X")));
                    vitems[i].SubItems[5].BackColor = System.Drawing.ColorTranslator.FromHtml((t.Color3 == 0 ? "#00ffffff" : "#" + t.Color3.ToString("X")));
                    vitems[i].ToolTipText = t.ToString();
                }
                listview.Items.AddRange(vitems);
            }
            listview.EndUpdate();
            listview.GoToRow(index);
        }

        private void AddUserList(List<User> users)
        {
            int count = users.Count;
            //TODO 
            this.list_users.BeginUpdate();
            this.list_users.Items.Clear();
            this.lb_users.Text = this.lb_users.Text.Split(' ')[0] + " (" + count + ")";
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
                    items[i].SubItems.Add("" + u.Class);
                    items[i].SubItems.Add("" + u.level);
                    items[i].ToolTipText = "CID:" + u.CID + "\n" + u.ToString();
                }
                this.list_users.Items.AddRange(items);
            }
            this.list_users.EndUpdate();
            this.list_users.GoToRow(index);
        }
        private void log(string text)
        {
            if (this.tb_logcat.Text.Length >= this.tb_logcat.MaxLength - 1024)
            {
                this.tb_logcat.Text = "";
            }
            this.tb_logcat.AppendText("\n" + DateTime.UtcNow.ToLongTimeString() + "  " + text + "\n");
        }
        private void ReadMails()
        {
            AddUserMails(this.ReadUserMailList(CurUser));
            AddSendMails(this.ReadSendMailList(CurUser));
        }
        private void AddUserMails(List<Mail> mails)
        {
            AddMails("[邮箱]", this.list_mail_user, mails);
        }
        private void AddSendMails(List<Mail> mails)
        {
            AddMails("[发送中]", this.list_mail_send, mails);
        }
        private void AddMails(string tag, ListView listview, List<Mail> mails)
        {
            //TODO
            int count = mails.Count;
            //TODO 
            listview.BeginUpdate();
            listview.Items.Clear();
            if (listview == list_mail_send)
            {
                this.tab_mail_send.Text = this.tab_mail_send.Text.Split(' ')[0] + " (" + count + ")";
            }
            else
            {
                this.tab_mail_user.Text = this.tab_mail_user.Text.Split(' ')[0] + " (" + count + ")";
            }
            if (count >= 0)
            {
                ListViewItem[] items = new ListViewItem[count];
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
                listview.Items.AddRange(items);
            }
            listview.EndUpdate();
        }
        #endregion

        #region 邮件列表菜单
        private void contentMenuRefreshMail_Click(object sender, EventArgs e)
        {
            ReadMails();
        }

        private void contentMenuDeleteMail_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            Control parent = null;
            if (menu != null)
            {
                parent = menu.GetMenuConrtol();
            }
            ListView listview = null;
            Boolean isSend = false;
            if (parent == this.list_mail_send)
            {
                listview = this.list_mail_send;
                isSend = true;
            }
            else if (parent == this.list_mail_user)
            {
                listview = this.list_mail_user;
            }
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
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            Control parent = null;
            if (menu != null)
            {
                parent = menu.GetMenuConrtol();
            }
            ListView listview = null;
            Boolean isSend = false;
            if (parent == this.list_mail_send)
            {
                listview = this.list_mail_send;
                isSend = true;
            }
            else if (parent == this.list_mail_user)
            {
                listview = this.list_mail_user;
            }
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
            this.SendItems(CurUser, 1, list_search.GetSelectItems<Item>());
        }

        private void contentMenuSendItem10_Click(object sender, EventArgs e)
        {
            this.SendItems(CurUser, 10, list_search.GetSelectItems<Item>());
        }

        private void contentMenuSendItem100_Click(object sender, EventArgs e)
        {
            this.SendItems(CurUser, 100, list_search.GetSelectItems<Item>());
        }
        #endregion

        #region 用户列表菜单
        private void contentMenuUserResetGroup_Click(object sender, EventArgs e)
        {
            log("[" + CurUser.Name + "]阵营技能重置");
            this.ResetGroupSkill(CurUser);
        }

        private void contentMenuUserMaxDark_Click(object sender, EventArgs e)
        {
            log("[" + CurUser.Name + "]黑暗阵营满级");
            this.MaxDarkLevel(CurUser);
        }

        private void contentMenuUserMaxLight_Click(object sender, EventArgs e)
        {
            log("[" + CurUser.Name + "]光明阵营满级");
            this.MaxLightLevel(CurUser);
        }

        private void contentMenuUserMaxSecondClass_Click(object sender, EventArgs e)
        {
            log("[" + CurUser.Name + "]全部副职业满级");
            this.MaxAllSecondClass(CurUser);
        }

        private void contentMenuUserModLevel_Click(object sender, EventArgs e)
        {
            using (ChangeUserLevelForm form = new ChangeUserLevelForm(this))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    log("成功修改用户[" + CurUser.Name + "]等级");
                    AddUserList(this.ReadUserList());
                }
            }
        }

        private void contentMenuUserModName_Click(object sender, EventArgs e)
        {
            using (ChangeNameForm form = new ChangeNameForm(this))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    log("成功修改[" + CurUser.Name + "]名字");
                    AddUserList(this.ReadUserList());
                }
            }
        }

        private void menuRefreshUsers_Click(object sender, EventArgs e)
        {
            AddUserList(this.ReadUserList());
        }
        #endregion

        #region 物品菜单
        private void contentMenuItemAllUnLimitTime_Click(object sender, EventArgs e)
        {
            ReadItems();
        }

        private void contentMenuDeleteItems_Click(object sender, EventArgs e)
        {
            ReadItems();
        }

        private void contentMenuItemPower5_Click(object sender, EventArgs e)
        {
            ReadItems();
        }

        private void contentMenuItemPower10_Click(object sender, EventArgs e)
        {
            ReadItems();
        }

        private void contentMenuItemPower12_Click(object sender, EventArgs e)
        {
            ReadItems();
        }

        private void contentMenuItemPower15_Click(object sender, EventArgs e)
        {
            ReadItems();
        }

        private void contentMenuItemMaxStar_Click(object sender, EventArgs e)
        {
            ReadItems();
        }

        private void contentMenuItemUnLimitTime_Click(object sender, EventArgs e)
        {
            ReadItems();
        }

        private void contentMennuAllMaxStar_Click(object sender, EventArgs e)
        {
            ReadItems();
        }
        #endregion

        private void list_users_SelectedIndexChanged(object sender, EventArgs e)
        {
            User user = this.list_users.GetSelectItem<User>();
            if (user != null)
            {
                this.CurUser = user;
                this.Text = this.DefTitle + "  - " + user.ToString();
                ReadMails();
                ReadItems();
            }
        }

        #region 搜索相关

        private void btn_search_id_Click(object sender, EventArgs e)
        {

        }

        private void btn_search_name_Click(object sender, EventArgs e)
        {

        }

        private void btn_senditem_send_Click(object sender, EventArgs e)
        {

        }

        private void btn_senditem_send_other_Click(object sender, EventArgs e)
        {

        }

        private void tb_senditem_class_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void tb_senditem_name_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        #endregion

        private void list_items_normal_SelectedIndexChanged(object sender, EventArgs e)
        {
            NormalCurItem = this.list_items_normal.GetSelectItem<Item>();
        }

        private void list_items_cash_SelectedIndexChanged(object sender, EventArgs e)
        {
            CashCurItem = this.list_items_cash.GetSelectItem<Item>();
        }
    }
}
