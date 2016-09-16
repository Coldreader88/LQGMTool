using GMTool.Bean;
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
        private User CurUser;
        public MainForm()
        {
            InitializeComponent();
            this.Text += " " + Application.ProductVersion.ToString();
            this.DefTitle = this.Text;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ReadSettings();
            this.list_users.Items.Clear();
            this.list_items.Items.Clear();
            this.list_search.Items.Clear();
            this.tb_logcat.Text = "";
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.CloseDataBase();
        }

        #region 数据库
        private void btn_mssql_open_Click(object sender, EventArgs e)
        {
            this.list_items.Enabled = false;
            this.list_users.Enabled = false;
            this.tab_mail.Enabled = false;
            this.list_search.Enabled = false;
            this.btn_senditem_send.Enabled = false;
            this.btn_senditem_send_other.Enabled = false;
            this.btn_search_name.Enabled = false;
            this.btn_search_id.Enabled = false;
            this.tb_senditem_class.Enabled = false;
            this.tb_senditem_name.Enabled = false;
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
                AddItemList(new List<Item>());
                AddUserMails(new List<Mail>());
                AddSendMails(new List<Mail>());
            }
            else
            {
                if (this.connectDataBase(this.tb_mssql_server.Text, this.tb_mssql_user.Text, this.tb_mssql_pwd.Text, this.tb_mssql_db.Text))
                {
                    this.list_items.Enabled = true;
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

        private void ReadItems()
        {
            AddItemList(this.ReadUserItems(CurUser));
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
                this.list_search.Items.AddRange(vitems);
            }
            this.list_search.EndUpdate();
            this.list_search.GoToRow(index);
        }
        private void AddItemList(List<Item> items)
        {
            int count = items.Count;
            //TODO 
            this.list_items.BeginUpdate();
            this.list_items.Items.Clear();
            this.lb_items.Text = this.lb_items.Text.Split(' ')[0] + " (" + count + ")";
            int index = -1;
            if (count >= 0)
            {
                ListViewItem[] vitems = new ListViewItem[count];
                this.list_items.Items.AddRange(vitems);
            }
            this.list_items.EndUpdate();
            this.list_items.GoToRow(index);
        }

        private void AddUserList(List<User> users)
        {
            int count = users.Count;
            //TODO 
            this.list_users.BeginUpdate();
            this.list_users.Items.Clear();
            this.lb_users.Text = this.lb_users.Text.Split(' ')[0] + " ("+count+")";
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
        #region 邮件列表菜单
        private void contentMenuRefreshMail_Click(object sender, EventArgs e)
        {
            ReadMails();
        }

        private void contentMenuDeleteMail_Click(object sender, EventArgs e)
        {
            ReadMails();
        }

        private void contentMenuDeleteAllMails_Click(object sender, EventArgs e)
        {
            ReadMails();
        }
        #endregion

        #region 邮件列表菜单
        private void contentMenuSendItem1_Click(object sender, EventArgs e)
        {

        }

        private void contentMenuSendItem10_Click(object sender, EventArgs e)
        {

        }

        private void contentMenuSendItem100_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 用户列表菜单
        private void contentMenuUserResetGroup_Click(object sender, EventArgs e)
        {

        }

        private void contentMenuUserMaxDark_Click(object sender, EventArgs e)
        {

        }

        private void contentMenuUserMaxLight_Click(object sender, EventArgs e)
        {

        }

        private void contentMenuUserMaxSecondClass_Click(object sender, EventArgs e)
        {

        }

        private void contentMenuUserModLevel_Click(object sender, EventArgs e)
        {
            ReadMails();
        }

        private void contentMenuUserModName_Click(object sender, EventArgs e)
        {
            ReadMails();
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
    }
}
