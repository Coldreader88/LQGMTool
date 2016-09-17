using GMTool.Bean;
using GMTool.Helper;
using LY;
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

namespace GMTool
{
    public partial class MainForm : Form
    {
        #region Member
        private string DefTitle;
        private ItemClassInfoHelper itemsHelper;
        private ColorDialog colorDialog;
        public User CurUser { get; private set; }

        private int NormalCurItem = -1;
        private int CashCurItem = -1;

        private Item CurItem;
        #endregion

        #region 窗体
        public MainForm()
        {
            CurUser = new User(0, 0, 0, "", 0, 1);
            itemsHelper = new ItemClassInfoHelper("./heroes_text_taiwan.txt", "./heroes.db3");
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
            AddTypes();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.CloseDataBase();
        }
        #endregion

        #region 数据库
        private void btn_mssql_open_Click(object sender, EventArgs e)
        {
            this.tab_items.Enabled = false;
            this.tab_left.Enabled = false;
            this.tab_mail.Enabled = false;
            this.list_search.Enabled = false;
            this.btn_senditem_send.Enabled = false;
            this.btn_search_name.Enabled = false;
            this.btn_search_id.Enabled = false;
            this.tb_senditem_class.Enabled = false;
            this.tb_senditem_name.Enabled = false;
            if (this.btn_mssql_open.Text == "断开")
            {
                CurItem = null;
                CurUser = null;
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
                if (!itemsHelper.IsOpen)
                {
                    if (!itemsHelper.Read())
                    {
                        this.Error("物品文本读取错误。\n请确保heroes_text_taiwan.txt和heroes.db3在程序目录下面");
                        return;
                    }
                    //初始化菜单
                    InitEnchantMenu();
                }
                if (this.connectDataBase(this.tb_mssql_server.Text, this.tb_mssql_user.Text, this.tb_mssql_pwd.Text, this.tb_mssql_db.Text))
                {
                    this.tab_items.Enabled = true;
                    this.tab_left.Enabled = true;
                    this.tab_mail.Enabled = true;
                    this.list_search.Enabled = true;
                    this.btn_senditem_send.Enabled = true;
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

        private void InitEnchantMenu()
        {
            this.contentMenuEnchantPrefix.DropDownItems.Clear();
            this.contentMenuEnchantSuffix.DropDownItems.Clear();
            EnchantInfo[] enchantinfos = itemsHelper.GetEnchantInfos();
            int i = 0, j = 0;
            int max = 20;
            ToolStripMenuItem prelist = null;
            ToolStripMenuItem suflist = null;
            foreach (EnchantInfo info in enchantinfos)
            {
                if (info.IsPrefix)
                {
                    if (prelist == null || i % max == 0)
                    {
                        prelist = new ToolStripMenuItem(i + "-" + (i + max));
                        this.contentMenuEnchantPrefix.DropDownItems.Add(prelist);
                    }
                    i++;
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(info.Name);
                    tsmi.Tag = info;
                    tsmi.ToolTipText = info.ToString();//提示文字为真实路径
                    tsmi.Click += Tsmi_Click;
                    prelist.DropDownItems.Add(tsmi);
                }
                else
                {
                    if (suflist == null || j % max == 0)
                    {
                        suflist = new ToolStripMenuItem(j + "-" + (j + max));
                        this.contentMenuEnchantSuffix.DropDownItems.Add(suflist);
                    }
                    j++;
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(info.Name);
                    tsmi.Tag = info;
                    tsmi.ToolTipText = info.ToString();//提示文字为真实路径
                    tsmi.Click += Tsmi_Click;
                    suflist.DropDownItems.Add(tsmi);
                }
            }

        }

        private void Tsmi_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            if (menu != null && menu.Tag != null)
            {
                EnchantInfo info = menu.Tag as EnchantInfo;
                if (info != null)
                {
                    //附魔
                    if (this.Enchant(CurItem, info))
                    {
                        log(CurItem.ItemName + " 附魔【" + info.Name + "】成功。");
                        ReadItems();
                    }
                    else
                    {
                        this.Warnning(CurItem.ItemName + " 附魔【" + info.Name + "】失败。");
                        log(CurItem.ItemName + " 附魔【" + info.Name + "】失败。");
                    }
                }
            }
        }
        #endregion

        #region 数据读取和添加
        private void ReadItems()
        {
            if (!CheckUser()) return;
            AddItemList(this.list_items_normal, this.ReadUserItems(CurUser, PackType.Normal));
            AddItemList(this.list_items_cash, this.ReadUserItems(CurUser, PackType.Cash));
            AddItemList(this.list_items_quest, this.ReadUserItems(CurUser, PackType.Quest));
            AddItemList(this.list_items_other, this.ReadUserItems(CurUser, PackType.Other));
        }
        private void SetCurItems(Item item)
        {
            if (item != null)
            {
                if (CurItem != null)
                {
                    if (item == CurItem)
                    {
                        return;
                    }
                }
                CurItem = item;
                if (item.Color1 != 0)
                {
                    // this.lb_color1.BackColor = ColorTranslator.FromHtml((item.Color1 == 0 ? "#00ffffff" : "#" + item.Color1.ToString("X")));
                    // this.lb_color1.BackColor = ColorTranslator.FromHtml((item.Color1 == 0 ? "#00ffffff" : "#" + item.Color1.ToString("X")));
                    // this.lb_color1.BackColor = ColorTranslator.FromHtml((item.Color1 == 0 ? "#00ffffff" : "#" + item.Color1.ToString("X")));
                    if (!chk_lock_color.Checked)
                    {
                        this.tb_color1.Text = (item.Color1 == 0 ? "" : "#" + item.Color1.ToString("X"));
                        this.tb_color2.Text = (item.Color2 == 0 ? "" : "#" + item.Color2.ToString("X"));
                        this.tb_color3.Text = (item.Color3 == 0 ? "" : "#" + item.Color3.ToString("X"));
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
        private void AddTypes()
        {
            this.cb_category.Items.Clear();
            this.cb_item_type.Items.Clear();
            this.cb_category.Items.AddRange(ItemTradeCategoryEx.Values);
            this.cb_item_type.Items.AddRange(CategoryEx.Values);
            this.cb_category.SelectedIndex = 0;
            this.cb_item_type.SelectedIndex = 0;
        }

        #region 添加物品
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
                    Item t = items[i];
                    ItemClassInfo info = itemsHelper.Get(t.ItemClass);
                    t.Attach(info);
                    vitems[i] = new ListViewItem();
                    vitems[i].Tag = t;
                    vitems[i].Text = (t.ItemName == null ? t.ItemClass : t.ItemName);
                    if ((listview == list_items_normal || listview == list_items_cash )&& t.Attributes != null)
                    {
                        string head = "";
                        foreach (ItemAttribute attr in t.Attributes)
                        {
                            if (attr.Type == ItemAttributeType.ENHANCE)
                            {
                                head = "+" + attr.Value+" ";
                                break;
                            }
                        }

                        foreach (ItemAttribute attr in t.Attributes)
                        {
                            if (attr.Type == ItemAttributeType.PREFIX)
                            {
                                EnchantInfo einfo = itemsHelper.GetEnchant(attr.Value);
                                head += "【" + (einfo == null ? attr.Value : einfo.Name) + "】";
                                break;
                            }
                        }
                        foreach (ItemAttribute attr in t.Attributes)
                        {
                            if (attr.Type == ItemAttributeType.SUFFIX)
                            {
                                EnchantInfo einfo = itemsHelper.GetEnchant(attr.Value);
                                head += "【" + (einfo == null ? attr.Value : einfo.Name) + "】";
                                break;
                            }
                        }
                        foreach (ItemAttribute attr in t.Attributes)
                        {
                            if (attr.Type == ItemAttributeType.QUALITY)
                            {
                                head += "★" + attr.Arg+" ";
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(head))
                        {
                            vitems[i].Text = head  + vitems[i].Text;
                        }
                    }
                    if (i % 2 == 0)
                        vitems[i].BackColor = Color.GhostWhite;
                    else
                        vitems[i].BackColor = Color.White;
                    if (NormalCurItem == i)
                    {
                        index = i;
                        vitems[i].Checked = true;
                        vitems[i].Selected = true;
                    }
                    vitems[i].SubItems.Add("" + t.Count);
                    if (listview == list_items_normal)
                    {
                        vitems[i].SubItems.Add("" + t.ItemType);
                    }
                    vitems[i].SubItems.Add("" + t.Category);
                    if (listview == list_items_normal || listview == list_items_cash)
                    {
                        vitems[i].UseItemStyleForSubItems = false;
                        int colorIndex = vitems[i].SubItems.Count;
                        vitems[i].SubItems.Add("");// + (t.Color1 == 0 ? "" : t.Color1.ToString("x")));
                        vitems[i].SubItems.Add("");// + (t.Color1 == 0 || t.Color2 == 0 ? "" : t.Color2.ToString("x")));
                        vitems[i].SubItems.Add("");// + (t.Color1 == 0 || t.Color3 == 0 ? "" : t.Color3.ToString("x")));
                        vitems[i].SubItems[colorIndex].BackColor = ColorTranslator.FromHtml((t.Color1 == 0 ? "#00ffffff" : "#" + t.Color1.ToString("X")));
                        vitems[i].SubItems[colorIndex + 1].BackColor = ColorTranslator.FromHtml((t.Color2 == 0 ? "#00ffffff" : "#" + t.Color2.ToString("X")));
                        vitems[i].SubItems[colorIndex + 2].BackColor = ColorTranslator.FromHtml((t.Color3 == 0 ? "#00ffffff" : "#" + t.Color3.ToString("X")));
                        vitems[i].SubItems[colorIndex].ForeColor = vitems[i].SubItems[colorIndex].BackColor;
                        vitems[i].SubItems[colorIndex + 1].ForeColor = vitems[i].SubItems[colorIndex + 1].BackColor;
                        vitems[i].SubItems[colorIndex + 2].ForeColor = vitems[i].SubItems[colorIndex + 2].BackColor;
                    }
                    vitems[i].SubItems.Add("" + t.Time);
                    vitems[i].ToolTipText = t.ToString();
                }
                listview.Items.AddRange(vitems);
            }
            listview.EndUpdate();
            listview.GoToRow(index);
        }
        #endregion

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
            if (!CheckUser()) return;
            ReadMails();
        }

        private void contentMenuDeleteMail_Click(object sender, EventArgs e)
        {
            if (!CheckUser()) return;
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
            if (!CheckUser()) return;
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
            if (!CheckUser()) return;
            ItemClassInfo[] items = list_search.GetSelectItems<ItemClassInfo>();
            if (items == null || items.Length == 0) return;
            int count = this.SendItems(CurUser, 1, items);
            if (count > 0)
            {
                ReadMails();
            }
            log("发送" + count + "个物品成功");
        }

        private void contentMenuSendItem10_Click(object sender, EventArgs e)
        {
            if (!CheckUser()) return;
            ItemClassInfo[] items = list_search.GetSelectItems<ItemClassInfo>();
            if (items == null || items.Length == 0) return;
            int count = this.SendItems(CurUser, 10, items);
            if (count > 0)
            {
                ReadMails();
            }
            log("发送" + count + "个物品成功");
        }

        private void contentMenuSendItem100_Click(object sender, EventArgs e)
        {
            if (!CheckUser()) return;
            ItemClassInfo[] items = list_search.GetSelectItems<ItemClassInfo>();
            if (items == null || items.Length == 0) return;
            int count = this.SendItems(CurUser, 100, items);
            if (count > 0)
            {
                ReadMails();
            }
            log("发送" + count + "个物品成功");
        }
        #endregion

        #region 用户列表菜单
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
            this.MaxDarkLevel(CurUser);
        }

        private void contentMenuUserMaxLight_Click(object sender, EventArgs e)
        {
            if (!CheckUser()) return;
            log("[" + CurUser.Name + "]光明阵营满级");
            this.MaxLightLevel(CurUser);
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
            if (!CheckUser()) return;
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
        private Item GetSelectItem(object sender)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            Control parent = null;
            if (menu != null)
            {
                parent = menu.GetMenuConrtol();
            }
            ListView listview = null;
            if (parent == this.list_items_cash)
            {
                listview = this.list_items_cash;
            }
            else if (parent == this.list_items_normal)
            {
                listview = this.list_items_normal;
            }
            else
            {
                return null;
            }
            return listview.GetSelectItem<Item>();
        }
        private Item[] GetSelectItems(object sender)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            Control parent = null;
            if (menu != null)
            {
                parent = menu.GetMenuConrtol();
            }
            ListView listview = null;
            if (parent == this.list_items_cash)
            {
                listview = this.list_items_cash;
            }
            else if (parent == this.list_items_normal)
            {
                listview = this.list_items_normal;
            }
            else
            {
                return null;
            }
            return listview.GetSelectItems<Item>();
        }
        private Item[] GetItems(object sender)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            Control parent = null;
            if (menu != null)
            {
                parent = menu.GetMenuConrtol();
            }
            ListView listview = null;
            if (parent == this.list_items_cash)
            {
                listview = this.list_items_cash;
            }
            else if (parent == this.list_items_normal)
            {
                listview = this.list_items_normal;
            }
            else
            {
                return null;
            }
            return listview.GetItems<Item>();
        }
        private void contentMenuDeleteItems_Click(object sender, EventArgs e)
        {
            if (!CheckUser()) return;
            // if (!CheckItem()) return;
            if (this.DeleteItem(CurUser, GetSelectItems(sender)))
            {
                ReadItems();
            }
        }

        private void contentMenuItemPower_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            if (this.ModItemPower(CurUser, GetSelectItem(sender), 0))
            {
                ReadItems();
            }
        }

        private void contentMenuItemPowers5_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            if (this.ModItemPower(CurUser, GetSelectItem(sender), 5))
            {
                ReadItems();
            }
        }

        private void contentMenuItemPowers10_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            if (this.ModItemPower(CurUser, GetSelectItem(sender), 10))
            {
                ReadItems();
            }
        }

        private void contentMenuItemPowers12_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            if (this.ModItemPower(CurUser, GetSelectItem(sender), 12))
            {
                ReadItems();
            }
        }

        private void contentMenuItemPowers15_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            if (this.ModItemPower(CurUser, GetSelectItem(sender), 15))
            {
                ReadItems();
            }
        }



        private void contentMenuItemMaxStar_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            if (this.MaxStar(CurUser, GetSelectItem(sender)))
            {
                ReadItems();
            }
        }

        private void contentMenuItemUnLimitTime_Click(object sender, EventArgs e)
        {
            if (!CheckItem()) return;
            if (this.UnLimitTime(CurUser, GetSelectItem(sender)))
            {
                ReadItems();
            }
        }

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
        }
        #endregion

        #region 搜索相关

        private void btn_search_id_Click(object sender, EventArgs e)
        {
            AddSearchItemList(itemsHelper.SearchItems(null, tb_senditem_class.Text, cb_category.Text, cb_item_type.Text));
        }

        private void btn_search_name_Click(object sender, EventArgs e)
        {
            AddSearchItemList(itemsHelper.SearchItems(tb_senditem_name.Text, null, cb_category.Text, cb_item_type.Text));
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
                if (this.SendItem(CurUser, count, tb_senditem_class.Text, tb_senditem_name.Text) > 0)
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
                    AddSearchItemList(itemsHelper.SearchItems(null, tb_senditem_class.Text, cb_category.Text, cb_item_type.Text));
                }
            }
        }

        private void tb_senditem_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (tb_senditem_name.Enabled)
                {
                    AddSearchItemList(itemsHelper.SearchItems(tb_senditem_name.Text, null, cb_category.Text, cb_item_type.Text));
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
                this.CurUser = user;
                this.Text = this.DefTitle + "  - " + user.ToString();
                itemsHelper.ClearCache();
                ReadMails();
                ReadItems();
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
            try
            {
                this.lb_color1.BackColor = ColorTranslator.FromHtml(tb_color1.Text);
            }
            catch (Exception)
            {

            }
        }

        private void tb_color2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.lb_color2.BackColor = ColorTranslator.FromHtml(tb_color2.Text);
            }
            catch (Exception)
            {

            }
        }

        private void tb_color3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.lb_color3.BackColor = ColorTranslator.FromHtml(tb_color3.Text);
            }
            catch (Exception)
            {

            }
        }

        private Color SelectColor(Color def1, Color def2, Color def3, Color def)
        {
            if (!CheckItem()) return def;
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
                this.lb_color1.Enabled = false;
                this.lb_color2.Enabled = false;
                this.lb_color3.Enabled = false;
                this.tb_color1.Enabled = false;
                this.tb_color2.Enabled = false;
                this.tb_color3.Enabled = false;
            }
            else
            {
                this.lb_color1.Enabled = true;
                this.lb_color2.Enabled = true;
                this.lb_color3.Enabled = true;
                this.tb_color1.Enabled = true;
                this.tb_color2.Enabled = true;
                this.tb_color3.Enabled = true;
            }
        }


        private bool CheckItem()
        {
            bool b = CurItem != null;
            if (!b)
            {
                this.Warnning("没有选中物品");
            }
            return b;
        }
        private bool CheckUser()
        {
            bool b = CurUser != null;
            if (!b)
            {
                this.Warnning("没有选择用户");
            }
            return b;
        }
        private void contentMenuColor1_Click(object sender, EventArgs e)
        {
            this.ModItemColor(CurUser, lb_color1.BackColor.ToArgb(), 0, 0, GetSelectItems(sender));
            ReadItems();
        }

        private void contentMenuColor2_Click(object sender, EventArgs e)
        {
            this.ModItemColor(CurUser, 0, lb_color2.BackColor.ToArgb(), 0, GetSelectItems(sender));
            ReadItems();
        }

        private void contentMenuColor3_Click(object sender, EventArgs e)
        {
            this.ModItemColor(CurUser, 0, 0, lb_color3.BackColor.ToArgb(), GetSelectItems(sender));
            ReadItems();
        }

        private void contentMenuColorAll_Click(object sender, EventArgs e)
        {
            this.ModItemColor(CurUser, lb_color1.BackColor.ToArgb(), lb_color2.BackColor.ToArgb(), lb_color3.BackColor.ToArgb(), GetSelectItems(sender));
            ReadItems();
        }


        private void contentMenuItemColorClean_Click(object sender, EventArgs e)
        {
            this.CleanItemColor(CurUser, GetSelectItems(sender));
            ReadItems();
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
    }
}
