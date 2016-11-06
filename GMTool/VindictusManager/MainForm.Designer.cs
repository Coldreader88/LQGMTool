﻿/*
 * 由SharpDevelop创建。
 * 用户： keyoyu
 * 日期: 2016/10/21
 * 时间: 14:40
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace Vindictus
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
			"测试",
			"艾丽莎",
			"12"}, 0);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.serverManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allSalonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.salonPirceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zhTW2zhCNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TabPanelLeft = new System.Windows.Forms.TabControl();
			this.TabUserList = new System.Windows.Forms.TabPage();
			this.UserListView = new Vindictus.Common.DListView();
			this.chUserName = new System.Windows.Forms.ColumnHeader();
			this.chUserClass = new System.Windows.Forms.ColumnHeader();
			this.chUserLevel = new System.Windows.Forms.ColumnHeader();
			this.UserMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addTitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addTitlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.maxSubClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetQuestTimesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.modUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modAPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modAttriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.maxGroupLightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.maxGroupDarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetGroupSkillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.refreshUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TabItemDetail = new System.Windows.Forms.TabPage();
			this.Color3 = new System.Windows.Forms.Label();
			this.Color2 = new System.Windows.Forms.Label();
			this.Color1 = new System.Windows.Forms.Label();
			this.LockColorChk = new System.Windows.Forms.CheckBox();
			this.Color3Text = new System.Windows.Forms.TextBox();
			this.Color2Text = new System.Windows.Forms.TextBox();
			this.Color1Text = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.TabPanelMail = new System.Windows.Forms.TabControl();
			this.TabMailSend = new System.Windows.Forms.TabPage();
			this.MailSendList = new Vindictus.Common.DListView();
			this.chSendingTitle = new System.Windows.Forms.ColumnHeader();
			this.MailMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.refreshMailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.deleteMailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteAllMailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TabMailRecv = new System.Windows.Forms.TabPage();
			this.MailRecvList = new Vindictus.Common.DListView();
			this.chReceiverTitle = new System.Windows.Forms.ColumnHeader();
			this.LogCatText = new System.Windows.Forms.TextBox();
			this.SearchReset = new System.Windows.Forms.Button();
			this.SearchSubCategory = new System.Windows.Forms.ComboBox();
			this.SearchMainCategory = new System.Windows.Forms.ComboBox();
			this.SearchByClass = new System.Windows.Forms.Button();
			this.SearchByName = new System.Windows.Forms.Button();
			this.lbSearchItemClass = new System.Windows.Forms.Label();
			this.lbSearchName = new System.Windows.Forms.Label();
			this.lbSearchCategory = new System.Windows.Forms.Label();
			this.SearchListView = new Vindictus.Common.DListView();
			this.chSearchName = new System.Windows.Forms.ColumnHeader();
			this.SendMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.send1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.send5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.send10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sendNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.copyItemClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SearchItemClass = new System.Windows.Forms.TextBox();
			this.SearchName = new System.Windows.Forms.TextBox();
			this.SearchTitle = new System.Windows.Forms.Label();
			this.tab_items = new System.Windows.Forms.TabControl();
			this.tab_items_normal = new System.Windows.Forms.TabPage();
			this.list_items_normal = new Vindictus.Common.DListView();
			this.ch_items_name = new System.Windows.Forms.ColumnHeader();
			this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
			this.ch_items_type = new System.Windows.Forms.ColumnHeader();
			this.ch_items_color1 = new System.Windows.Forms.ColumnHeader();
			this.ch_items_color2 = new System.Windows.Forms.ColumnHeader();
			this.ch_items_color3 = new System.Windows.Forms.ColumnHeader();
			this.ch_items_time = new System.Windows.Forms.ColumnHeader();
			this.tab_items_cash = new System.Windows.Forms.TabPage();
			this.list_items_cash = new Vindictus.Common.DListView();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.tab_items_quest = new System.Windows.Forms.TabPage();
			this.list_items_quest = new Vindictus.Common.DListView();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader19 = new System.Windows.Forms.ColumnHeader();
			this.tab_items_other = new System.Windows.Forms.TabPage();
			this.list_items_other = new Vindictus.Common.DListView();
			this.columnHeader20 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader21 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader22 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader26 = new System.Windows.Forms.ColumnHeader();
			this.menuStrip1.SuspendLayout();
			this.TabPanelLeft.SuspendLayout();
			this.TabUserList.SuspendLayout();
			this.UserMenuStrip.SuspendLayout();
			this.TabItemDetail.SuspendLayout();
			this.TabPanelMail.SuspendLayout();
			this.TabMailSend.SuspendLayout();
			this.MailMenuStrip.SuspendLayout();
			this.TabMailRecv.SuspendLayout();
			this.SendMenuStrip.SuspendLayout();
			this.tab_items.SuspendLayout();
			this.tab_items_normal.SuspendLayout();
			this.tab_items_cash.SuspendLayout();
			this.tab_items_quest.SuspendLayout();
			this.tab_items_other.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.viewToolStripMenuItem,
			this.toolToolStripMenuItem,
			this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1008, 25);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.serverManagerToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// serverManagerToolStripMenuItem
			// 
			this.serverManagerToolStripMenuItem.Name = "serverManagerToolStripMenuItem";
			this.serverManagerToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.serverManagerToolStripMenuItem.Text = "Server Manager";
			this.serverManagerToolStripMenuItem.Click += new System.EventHandler(this.ServerManagerToolStripMenuItemClick);
			// 
			// toolToolStripMenuItem
			// 
			this.toolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.allSalonToolStripMenuItem,
			this.salonPirceToolStripMenuItem,
			this.zhTW2zhCNToolStripMenuItem});
			this.toolToolStripMenuItem.Name = "toolToolStripMenuItem";
			this.toolToolStripMenuItem.Size = new System.Drawing.Size(46, 21);
			this.toolToolStripMenuItem.Text = "Tool";
			// 
			// allSalonToolStripMenuItem
			// 
			this.allSalonToolStripMenuItem.Name = "allSalonToolStripMenuItem";
			this.allSalonToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.allSalonToolStripMenuItem.Text = "AllSalon";
			this.allSalonToolStripMenuItem.Click += new System.EventHandler(this.AllSalonToolStripMenuItemClick);
			// 
			// salonPirceToolStripMenuItem
			// 
			this.salonPirceToolStripMenuItem.Name = "salonPirceToolStripMenuItem";
			this.salonPirceToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.salonPirceToolStripMenuItem.Text = "SalonPirce";
			this.salonPirceToolStripMenuItem.Click += new System.EventHandler(this.SalonPirceToolStripMenuItemClick);
			// 
			// zhTW2zhCNToolStripMenuItem
			// 
			this.zhTW2zhCNToolStripMenuItem.Name = "zhTW2zhCNToolStripMenuItem";
			this.zhTW2zhCNToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.zhTW2zhCNToolStripMenuItem.Text = "zhTW2zhCN";
			this.zhTW2zhCNToolStripMenuItem.Visible = false;
			this.zhTW2zhCNToolStripMenuItem.Click += new System.EventHandler(this.ZhTW2zhCNToolStripMenuItemClick);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
			// 
			// TabPanelLeft
			// 
			this.TabPanelLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.TabPanelLeft.Controls.Add(this.TabUserList);
			this.TabPanelLeft.Controls.Add(this.TabItemDetail);
			this.TabPanelLeft.Location = new System.Drawing.Point(0, 30);
			this.TabPanelLeft.Name = "TabPanelLeft";
			this.TabPanelLeft.SelectedIndex = 0;
			this.TabPanelLeft.Size = new System.Drawing.Size(250, 280);
			this.TabPanelLeft.TabIndex = 11;
			// 
			// TabUserList
			// 
			this.TabUserList.Controls.Add(this.UserListView);
			this.TabUserList.Location = new System.Drawing.Point(4, 22);
			this.TabUserList.Name = "TabUserList";
			this.TabUserList.Padding = new System.Windows.Forms.Padding(3);
			this.TabUserList.Size = new System.Drawing.Size(242, 254);
			this.TabUserList.TabIndex = 0;
			this.TabUserList.Text = "UserList";
			this.TabUserList.UseVisualStyleBackColor = true;
			// 
			// UserListView
			// 
			this.UserListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.chUserName,
			this.chUserClass,
			this.chUserLevel});
			this.UserListView.ContextMenuStrip = this.UserMenuStrip;
			this.UserListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.UserListView.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.UserListView.FullRowSelect = true;
			this.UserListView.GridLines = true;
			this.UserListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.UserListView.HideSelection = false;
			this.UserListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
			listViewItem1});
			this.UserListView.LabelWrap = false;
			this.UserListView.Location = new System.Drawing.Point(3, 3);
			this.UserListView.MultiSelect = false;
			this.UserListView.Name = "UserListView";
			this.UserListView.ShowItemToolTips = true;
			this.UserListView.Size = new System.Drawing.Size(236, 248);
			this.UserListView.TabIndex = 3;
			this.UserListView.UseCompatibleStateImageBehavior = false;
			this.UserListView.View = System.Windows.Forms.View.Details;
			this.UserListView.SelectedIndexChanged += new System.EventHandler(this.UserListViewSelectedIndexChanged);
			// 
			// chUserName
			// 
			this.chUserName.Text = "Name";
			this.chUserName.Width = 110;
			// 
			// chUserClass
			// 
			this.chUserClass.Text = "Class";
			this.chUserClass.Width = 56;
			// 
			// chUserLevel
			// 
			this.chUserLevel.Text = "Lv.";
			this.chUserLevel.Width = 40;
			// 
			// UserMenuStrip
			// 
			this.UserMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.addTitleToolStripMenuItem,
			this.addTitlesToolStripMenuItem,
			this.maxSubClassToolStripMenuItem,
			this.resetQuestTimesToolStripMenuItem,
			this.toolStripSeparator3,
			this.modUserToolStripMenuItem,
			this.modClassToolStripMenuItem,
			this.modAttriToolStripMenuItem,
			this.toolStripSeparator5,
			this.maxGroupLightToolStripMenuItem,
			this.maxGroupDarkToolStripMenuItem,
			this.resetGroupSkillToolStripMenuItem,
			this.toolStripSeparator2,
			this.refreshUserToolStripMenuItem});
			this.UserMenuStrip.Name = "UserMenuStrip";
			this.UserMenuStrip.Size = new System.Drawing.Size(177, 264);
			// 
			// addTitleToolStripMenuItem
			// 
			this.addTitleToolStripMenuItem.Name = "addTitleToolStripMenuItem";
			this.addTitleToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.addTitleToolStripMenuItem.Text = "AddTitle";
			// 
			// addTitlesToolStripMenuItem
			// 
			this.addTitlesToolStripMenuItem.Name = "addTitlesToolStripMenuItem";
			this.addTitlesToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.addTitlesToolStripMenuItem.Text = "AddTitles";
			this.addTitlesToolStripMenuItem.Click += new System.EventHandler(this.AddTitlesToolStripMenuItemClick);
			// 
			// maxSubClassToolStripMenuItem
			// 
			this.maxSubClassToolStripMenuItem.Name = "maxSubClassToolStripMenuItem";
			this.maxSubClassToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.maxSubClassToolStripMenuItem.Text = "MaxSubClass";
			this.maxSubClassToolStripMenuItem.Click += new System.EventHandler(this.MaxSubClassToolStripMenuItemClick);
			// 
			// resetQuestTimesToolStripMenuItem
			// 
			this.resetQuestTimesToolStripMenuItem.Name = "resetQuestTimesToolStripMenuItem";
			this.resetQuestTimesToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.resetQuestTimesToolStripMenuItem.Text = "ResetQuestTimes";
			this.resetQuestTimesToolStripMenuItem.Click += new System.EventHandler(this.ResetQuestTimesToolStripMenuItemClick);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(173, 6);
			// 
			// modUserToolStripMenuItem
			// 
			this.modUserToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.modAPToolStripMenuItem,
			this.modLevelToolStripMenuItem,
			this.modNameToolStripMenuItem});
			this.modUserToolStripMenuItem.Name = "modUserToolStripMenuItem";
			this.modUserToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.modUserToolStripMenuItem.Text = "ModUser";
			// 
			// modAPToolStripMenuItem
			// 
			this.modAPToolStripMenuItem.Name = "modAPToolStripMenuItem";
			this.modAPToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.modAPToolStripMenuItem.Text = "ModAP";
			this.modAPToolStripMenuItem.Click += new System.EventHandler(this.modAPToolStripMenuItem_Click);
			// 
			// modLevelToolStripMenuItem
			// 
			this.modLevelToolStripMenuItem.Name = "modLevelToolStripMenuItem";
			this.modLevelToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.modLevelToolStripMenuItem.Text = "ModLevel";
			// 
			// modNameToolStripMenuItem
			// 
			this.modNameToolStripMenuItem.Name = "modNameToolStripMenuItem";
			this.modNameToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.modNameToolStripMenuItem.Text = "ModName";
			// 
			// modClassToolStripMenuItem
			// 
			this.modClassToolStripMenuItem.Name = "modClassToolStripMenuItem";
			this.modClassToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.modClassToolStripMenuItem.Text = "ModClass";
			// 
			// modAttriToolStripMenuItem
			// 
			this.modAttriToolStripMenuItem.Name = "modAttriToolStripMenuItem";
			this.modAttriToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.modAttriToolStripMenuItem.Text = "ModAttri";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(173, 6);
			// 
			// maxGroupLightToolStripMenuItem
			// 
			this.maxGroupLightToolStripMenuItem.Name = "maxGroupLightToolStripMenuItem";
			this.maxGroupLightToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.maxGroupLightToolStripMenuItem.Text = "MaxGroupLight";
			this.maxGroupLightToolStripMenuItem.Click += new System.EventHandler(this.MaxGroupLightToolStripMenuItemClick);
			// 
			// maxGroupDarkToolStripMenuItem
			// 
			this.maxGroupDarkToolStripMenuItem.Name = "maxGroupDarkToolStripMenuItem";
			this.maxGroupDarkToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.maxGroupDarkToolStripMenuItem.Text = "MaxGroupDark";
			this.maxGroupDarkToolStripMenuItem.Click += new System.EventHandler(this.MaxGroupDarkToolStripMenuItemClick);
			// 
			// resetGroupSkillToolStripMenuItem
			// 
			this.resetGroupSkillToolStripMenuItem.Name = "resetGroupSkillToolStripMenuItem";
			this.resetGroupSkillToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.resetGroupSkillToolStripMenuItem.Text = "ResetGroupSkill";
			this.resetGroupSkillToolStripMenuItem.Click += new System.EventHandler(this.ResetGroupSkillToolStripMenuItemClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(173, 6);
			// 
			// refreshUserToolStripMenuItem
			// 
			this.refreshUserToolStripMenuItem.Name = "refreshUserToolStripMenuItem";
			this.refreshUserToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.refreshUserToolStripMenuItem.Text = "RefreshUser";
			this.refreshUserToolStripMenuItem.Click += new System.EventHandler(this.RefreshUserToolStripMenuItemClick);
			// 
			// TabItemDetail
			// 
			this.TabItemDetail.BackColor = System.Drawing.SystemColors.Control;
			this.TabItemDetail.Controls.Add(this.Color3);
			this.TabItemDetail.Controls.Add(this.Color2);
			this.TabItemDetail.Controls.Add(this.Color1);
			this.TabItemDetail.Controls.Add(this.LockColorChk);
			this.TabItemDetail.Controls.Add(this.Color3Text);
			this.TabItemDetail.Controls.Add(this.Color2Text);
			this.TabItemDetail.Controls.Add(this.Color1Text);
			this.TabItemDetail.Controls.Add(this.label11);
			this.TabItemDetail.Controls.Add(this.label9);
			this.TabItemDetail.Controls.Add(this.label6);
			this.TabItemDetail.Location = new System.Drawing.Point(4, 22);
			this.TabItemDetail.Name = "TabItemDetail";
			this.TabItemDetail.Size = new System.Drawing.Size(242, 254);
			this.TabItemDetail.TabIndex = 2;
			this.TabItemDetail.Text = "ItemDetail";
			// 
			// Color3
			// 
			this.Color3.BackColor = System.Drawing.SystemColors.Control;
			this.Color3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Color3.Location = new System.Drawing.Point(56, 84);
			this.Color3.Name = "Color3";
			this.Color3.Size = new System.Drawing.Size(70, 23);
			this.Color3.TabIndex = 4;
			// 
			// Color2
			// 
			this.Color2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Color2.Location = new System.Drawing.Point(56, 50);
			this.Color2.Name = "Color2";
			this.Color2.Size = new System.Drawing.Size(70, 23);
			this.Color2.TabIndex = 4;
			// 
			// Color1
			// 
			this.Color1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Color1.Location = new System.Drawing.Point(56, 12);
			this.Color1.Name = "Color1";
			this.Color1.Size = new System.Drawing.Size(70, 23);
			this.Color1.TabIndex = 4;
			// 
			// LockColorChk
			// 
			this.LockColorChk.AutoSize = true;
			this.LockColorChk.Location = new System.Drawing.Point(58, 124);
			this.LockColorChk.Name = "LockColorChk";
			this.LockColorChk.Size = new System.Drawing.Size(84, 16);
			this.LockColorChk.TabIndex = 3;
			this.LockColorChk.Text = "Lock Color";
			this.LockColorChk.UseVisualStyleBackColor = true;
			// 
			// Color3Text
			// 
			this.Color3Text.Location = new System.Drawing.Point(139, 86);
			this.Color3Text.Name = "Color3Text";
			this.Color3Text.Size = new System.Drawing.Size(82, 21);
			this.Color3Text.TabIndex = 2;
			// 
			// Color2Text
			// 
			this.Color2Text.Location = new System.Drawing.Point(139, 52);
			this.Color2Text.Name = "Color2Text";
			this.Color2Text.Size = new System.Drawing.Size(82, 21);
			this.Color2Text.TabIndex = 2;
			// 
			// Color1Text
			// 
			this.Color1Text.Location = new System.Drawing.Point(139, 12);
			this.Color1Text.Name = "Color1Text";
			this.Color1Text.Size = new System.Drawing.Size(82, 21);
			this.Color1Text.TabIndex = 2;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(15, 90);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(41, 12);
			this.label11.TabIndex = 0;
			this.label11.Text = "Color3";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(15, 57);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(41, 12);
			this.label9.TabIndex = 0;
			this.label9.Text = "Color2";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(15, 17);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(41, 12);
			this.label6.TabIndex = 0;
			this.label6.Text = "Color1";
			// 
			// TabPanelMail
			// 
			this.TabPanelMail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.TabPanelMail.Controls.Add(this.TabMailSend);
			this.TabPanelMail.Controls.Add(this.TabMailRecv);
			this.TabPanelMail.Location = new System.Drawing.Point(4, 310);
			this.TabPanelMail.Name = "TabPanelMail";
			this.TabPanelMail.SelectedIndex = 0;
			this.TabPanelMail.Size = new System.Drawing.Size(250, 288);
			this.TabPanelMail.TabIndex = 12;
			// 
			// TabMailSend
			// 
			this.TabMailSend.Controls.Add(this.MailSendList);
			this.TabMailSend.Location = new System.Drawing.Point(4, 22);
			this.TabMailSend.Name = "TabMailSend";
			this.TabMailSend.Size = new System.Drawing.Size(242, 262);
			this.TabMailSend.TabIndex = 0;
			this.TabMailSend.Text = "Sending";
			this.TabMailSend.UseVisualStyleBackColor = true;
			// 
			// MailSendList
			// 
			this.MailSendList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.chSendingTitle});
			this.MailSendList.ContextMenuStrip = this.MailMenuStrip;
			this.MailSendList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MailSendList.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.MailSendList.FullRowSelect = true;
			this.MailSendList.GridLines = true;
			this.MailSendList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.MailSendList.HideSelection = false;
			this.MailSendList.LabelWrap = false;
			this.MailSendList.Location = new System.Drawing.Point(0, 0);
			this.MailSendList.Name = "MailSendList";
			this.MailSendList.ShowItemToolTips = true;
			this.MailSendList.Size = new System.Drawing.Size(242, 262);
			this.MailSendList.TabIndex = 3;
			this.MailSendList.UseCompatibleStateImageBehavior = false;
			this.MailSendList.View = System.Windows.Forms.View.Details;
			// 
			// chSendingTitle
			// 
			this.chSendingTitle.Text = "Title";
			this.chSendingTitle.Width = 205;
			// 
			// MailMenuStrip
			// 
			this.MailMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.refreshMailToolStripMenuItem,
			this.toolStripSeparator1,
			this.deleteMailsToolStripMenuItem,
			this.deleteAllMailsToolStripMenuItem});
			this.MailMenuStrip.Name = "MailMenuStrip";
			this.MailMenuStrip.Size = new System.Drawing.Size(159, 76);
			this.MailMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.MailMenuStrip_Opening);
			// 
			// refreshMailToolStripMenuItem
			// 
			this.refreshMailToolStripMenuItem.Name = "refreshMailToolStripMenuItem";
			this.refreshMailToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.refreshMailToolStripMenuItem.Text = "RefreshMail";
			this.refreshMailToolStripMenuItem.Click += new System.EventHandler(this.RefreshMailToolStripMenuItemClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(155, 6);
			// 
			// deleteMailsToolStripMenuItem
			// 
			this.deleteMailsToolStripMenuItem.Name = "deleteMailsToolStripMenuItem";
			this.deleteMailsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.deleteMailsToolStripMenuItem.Text = "DeleteMails";
			this.deleteMailsToolStripMenuItem.Click += new System.EventHandler(this.DeleteMailsToolStripMenuItemClick);
			// 
			// deleteAllMailsToolStripMenuItem
			// 
			this.deleteAllMailsToolStripMenuItem.Name = "deleteAllMailsToolStripMenuItem";
			this.deleteAllMailsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.deleteAllMailsToolStripMenuItem.Text = "DeleteAllMails";
			this.deleteAllMailsToolStripMenuItem.Click += new System.EventHandler(this.DeleteAllMailsToolStripMenuItemClick);
			// 
			// TabMailRecv
			// 
			this.TabMailRecv.Controls.Add(this.MailRecvList);
			this.TabMailRecv.Location = new System.Drawing.Point(4, 22);
			this.TabMailRecv.Name = "TabMailRecv";
			this.TabMailRecv.Size = new System.Drawing.Size(242, 262);
			this.TabMailRecv.TabIndex = 1;
			this.TabMailRecv.Text = "Receiver";
			this.TabMailRecv.UseVisualStyleBackColor = true;
			// 
			// MailRecvList
			// 
			this.MailRecvList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.chReceiverTitle});
			this.MailRecvList.ContextMenuStrip = this.MailMenuStrip;
			this.MailRecvList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MailRecvList.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.MailRecvList.FullRowSelect = true;
			this.MailRecvList.GridLines = true;
			this.MailRecvList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.MailRecvList.HideSelection = false;
			this.MailRecvList.LabelWrap = false;
			this.MailRecvList.Location = new System.Drawing.Point(0, 0);
			this.MailRecvList.Name = "MailRecvList";
			this.MailRecvList.ShowItemToolTips = true;
			this.MailRecvList.Size = new System.Drawing.Size(242, 262);
			this.MailRecvList.TabIndex = 4;
			this.MailRecvList.UseCompatibleStateImageBehavior = false;
			this.MailRecvList.View = System.Windows.Forms.View.Details;
			// 
			// chReceiverTitle
			// 
			this.chReceiverTitle.Text = "Title";
			this.chReceiverTitle.Width = 208;
			// 
			// LogCatText
			// 
			this.LogCatText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.LogCatText.Location = new System.Drawing.Point(254, 510);
			this.LogCatText.Multiline = true;
			this.LogCatText.Name = "LogCatText";
			this.LogCatText.ReadOnly = true;
			this.LogCatText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.LogCatText.Size = new System.Drawing.Size(506, 84);
			this.LogCatText.TabIndex = 13;
			// 
			// SearchReset
			// 
			this.SearchReset.Location = new System.Drawing.Point(764, 568);
			this.SearchReset.Name = "SearchReset";
			this.SearchReset.Size = new System.Drawing.Size(235, 30);
			this.SearchReset.TabIndex = 25;
			this.SearchReset.Text = "SearchReset";
			this.SearchReset.UseVisualStyleBackColor = true;
			this.SearchReset.Click += new System.EventHandler(this.SearchReset_Click);
			// 
			// SearchSubCategory
			// 
			this.SearchSubCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchSubCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SearchSubCategory.FormattingEnabled = true;
			this.SearchSubCategory.Location = new System.Drawing.Point(912, 505);
			this.SearchSubCategory.Name = "SearchSubCategory";
			this.SearchSubCategory.Size = new System.Drawing.Size(84, 20);
			this.SearchSubCategory.TabIndex = 23;
			// 
			// SearchMainCategory
			// 
			this.SearchMainCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchMainCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SearchMainCategory.FormattingEnabled = true;
			this.SearchMainCategory.Location = new System.Drawing.Point(819, 505);
			this.SearchMainCategory.Name = "SearchMainCategory";
			this.SearchMainCategory.Size = new System.Drawing.Size(83, 20);
			this.SearchMainCategory.TabIndex = 24;
			// 
			// SearchByClass
			// 
			this.SearchByClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchByClass.Location = new System.Drawing.Point(887, 534);
			this.SearchByClass.Name = "SearchByClass";
			this.SearchByClass.Size = new System.Drawing.Size(110, 30);
			this.SearchByClass.TabIndex = 21;
			this.SearchByClass.Text = "SearchByClass";
			this.SearchByClass.UseVisualStyleBackColor = true;
			this.SearchByClass.Click += new System.EventHandler(this.SearchByClass_Click);
			// 
			// SearchByName
			// 
			this.SearchByName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchByName.Location = new System.Drawing.Point(766, 534);
			this.SearchByName.Name = "SearchByName";
			this.SearchByName.Size = new System.Drawing.Size(110, 30);
			this.SearchByName.TabIndex = 22;
			this.SearchByName.Text = "SearchByName";
			this.SearchByName.UseVisualStyleBackColor = true;
			this.SearchByName.Click += new System.EventHandler(this.SearchByName_Click);
			// 
			// lbSearchItemClass
			// 
			this.lbSearchItemClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lbSearchItemClass.AutoSize = true;
			this.lbSearchItemClass.Location = new System.Drawing.Point(761, 480);
			this.lbSearchItemClass.Name = "lbSearchItemClass";
			this.lbSearchItemClass.Size = new System.Drawing.Size(59, 12);
			this.lbSearchItemClass.TabIndex = 18;
			this.lbSearchItemClass.Text = "ItemClass";
			// 
			// lbSearchName
			// 
			this.lbSearchName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lbSearchName.AutoSize = true;
			this.lbSearchName.Location = new System.Drawing.Point(762, 456);
			this.lbSearchName.Name = "lbSearchName";
			this.lbSearchName.Size = new System.Drawing.Size(53, 12);
			this.lbSearchName.TabIndex = 19;
			this.lbSearchName.Text = "ItemName";
			// 
			// lbSearchCategory
			// 
			this.lbSearchCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lbSearchCategory.AutoSize = true;
			this.lbSearchCategory.Location = new System.Drawing.Point(762, 509);
			this.lbSearchCategory.Name = "lbSearchCategory";
			this.lbSearchCategory.Size = new System.Drawing.Size(53, 12);
			this.lbSearchCategory.TabIndex = 20;
			this.lbSearchCategory.Text = "Category";
			// 
			// SearchListView
			// 
			this.SearchListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.SearchListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.chSearchName});
			this.SearchListView.ContextMenuStrip = this.SendMenuStrip;
			this.SearchListView.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.SearchListView.FullRowSelect = true;
			this.SearchListView.GridLines = true;
			this.SearchListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.SearchListView.HideSelection = false;
			this.SearchListView.LabelWrap = false;
			this.SearchListView.Location = new System.Drawing.Point(764, 54);
			this.SearchListView.Name = "SearchListView";
			this.SearchListView.ShowItemToolTips = true;
			this.SearchListView.Size = new System.Drawing.Size(235, 394);
			this.SearchListView.TabIndex = 17;
			this.SearchListView.UseCompatibleStateImageBehavior = false;
			this.SearchListView.View = System.Windows.Forms.View.Details;
			this.SearchListView.SelectedIndexChanged += new System.EventHandler(this.SearchListViewSelectedIndexChanged);
			// 
			// chSearchName
			// 
			this.chSearchName.Text = "Name";
			this.chSearchName.Width = 207;
			// 
			// SendMenuStrip
			// 
			this.SendMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.send1ToolStripMenuItem,
			this.send5ToolStripMenuItem,
			this.send10ToolStripMenuItem,
			this.sendNToolStripMenuItem,
			this.toolStripSeparator4,
			this.copyItemClassToolStripMenuItem});
			this.SendMenuStrip.Name = "SendMenuStrip";
			this.SendMenuStrip.Size = new System.Drawing.Size(167, 120);
			// 
			// send1ToolStripMenuItem
			// 
			this.send1ToolStripMenuItem.Name = "send1ToolStripMenuItem";
			this.send1ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.send1ToolStripMenuItem.Text = "Send 1";
			this.send1ToolStripMenuItem.Click += new System.EventHandler(this.send1ToolStripMenuItem_Click);
			// 
			// send5ToolStripMenuItem
			// 
			this.send5ToolStripMenuItem.Name = "send5ToolStripMenuItem";
			this.send5ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.send5ToolStripMenuItem.Text = "Send 5";
			this.send5ToolStripMenuItem.Click += new System.EventHandler(this.send5ToolStripMenuItem_Click);
			// 
			// send10ToolStripMenuItem
			// 
			this.send10ToolStripMenuItem.Name = "send10ToolStripMenuItem";
			this.send10ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.send10ToolStripMenuItem.Text = "Send 10";
			this.send10ToolStripMenuItem.Click += new System.EventHandler(this.send10ToolStripMenuItem_Click);
			// 
			// sendNToolStripMenuItem
			// 
			this.sendNToolStripMenuItem.Name = "sendNToolStripMenuItem";
			this.sendNToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.sendNToolStripMenuItem.Text = "Send N";
			this.sendNToolStripMenuItem.Click += new System.EventHandler(this.sendNToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(163, 6);
			// 
			// copyItemClassToolStripMenuItem
			// 
			this.copyItemClassToolStripMenuItem.Name = "copyItemClassToolStripMenuItem";
			this.copyItemClassToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.copyItemClassToolStripMenuItem.Text = "Copy ItemClass";
			this.copyItemClassToolStripMenuItem.Click += new System.EventHandler(this.copyItemClassToolStripMenuItem_Click);
			// 
			// SearchItemClass
			// 
			this.SearchItemClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchItemClass.Location = new System.Drawing.Point(820, 476);
			this.SearchItemClass.MaxLength = 128;
			this.SearchItemClass.Name = "SearchItemClass";
			this.SearchItemClass.Size = new System.Drawing.Size(176, 21);
			this.SearchItemClass.TabIndex = 15;
			this.SearchItemClass.WordWrap = false;
			// 
			// SearchName
			// 
			this.SearchName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchName.Location = new System.Drawing.Point(820, 453);
			this.SearchName.MaxLength = 128;
			this.SearchName.Name = "SearchName";
			this.SearchName.Size = new System.Drawing.Size(176, 21);
			this.SearchName.TabIndex = 16;
			this.SearchName.WordWrap = false;
			// 
			// SearchTitle
			// 
			this.SearchTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SearchTitle.BackColor = System.Drawing.SystemColors.Control;
			this.SearchTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.SearchTitle.Location = new System.Drawing.Point(764, 30);
			this.SearchTitle.Name = "SearchTitle";
			this.SearchTitle.Size = new System.Drawing.Size(235, 23);
			this.SearchTitle.TabIndex = 14;
			this.SearchTitle.Text = "SearchList";
			this.SearchTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tab_items
			// 
			this.tab_items.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tab_items.Controls.Add(this.tab_items_normal);
			this.tab_items.Controls.Add(this.tab_items_cash);
			this.tab_items.Controls.Add(this.tab_items_quest);
			this.tab_items.Controls.Add(this.tab_items_other);
			this.tab_items.Location = new System.Drawing.Point(253, 31);
			this.tab_items.Name = "tab_items";
			this.tab_items.SelectedIndex = 0;
			this.tab_items.Size = new System.Drawing.Size(507, 473);
			this.tab_items.TabIndex = 26;
			// 
			// tab_items_normal
			// 
			this.tab_items_normal.Controls.Add(this.list_items_normal);
			this.tab_items_normal.Location = new System.Drawing.Point(4, 22);
			this.tab_items_normal.Name = "tab_items_normal";
			this.tab_items_normal.Padding = new System.Windows.Forms.Padding(3);
			this.tab_items_normal.Size = new System.Drawing.Size(499, 447);
			this.tab_items_normal.TabIndex = 0;
			this.tab_items_normal.Text = "背包";
			this.tab_items_normal.UseVisualStyleBackColor = true;
			// 
			// list_items_normal
			// 
			this.list_items_normal.BackColor = System.Drawing.SystemColors.Window;
			this.list_items_normal.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.ch_items_name,
			this.columnHeader12,
			this.columnHeader15,
			this.ch_items_type,
			this.ch_items_color1,
			this.ch_items_color2,
			this.ch_items_color3,
			this.ch_items_time});
			this.list_items_normal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list_items_normal.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.list_items_normal.FullRowSelect = true;
			this.list_items_normal.GridLines = true;
			this.list_items_normal.HideSelection = false;
			this.list_items_normal.LabelWrap = false;
			this.list_items_normal.Location = new System.Drawing.Point(3, 3);
			this.list_items_normal.Name = "list_items_normal";
			this.list_items_normal.ShowItemToolTips = true;
			this.list_items_normal.Size = new System.Drawing.Size(493, 441);
			this.list_items_normal.TabIndex = 3;
			this.list_items_normal.UseCompatibleStateImageBehavior = false;
			this.list_items_normal.View = System.Windows.Forms.View.Details;
			// 
			// ch_items_name
			// 
			this.ch_items_name.Text = "名字";
			this.ch_items_name.Width = 61;
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "数量";
			this.columnHeader12.Width = 40;
			// 
			// columnHeader15
			// 
			this.columnHeader15.Text = "分类";
			// 
			// ch_items_type
			// 
			this.ch_items_type.Text = "类型";
			this.ch_items_type.Width = 80;
			// 
			// ch_items_color1
			// 
			this.ch_items_color1.Text = "颜色1";
			this.ch_items_color1.Width = 52;
			// 
			// ch_items_color2
			// 
			this.ch_items_color2.Text = "颜色2";
			this.ch_items_color2.Width = 52;
			// 
			// ch_items_color3
			// 
			this.ch_items_color3.Text = "颜色3";
			this.ch_items_color3.Width = 49;
			// 
			// ch_items_time
			// 
			this.ch_items_time.Text = "到期时间";
			this.ch_items_time.Width = 76;
			// 
			// tab_items_cash
			// 
			this.tab_items_cash.Controls.Add(this.list_items_cash);
			this.tab_items_cash.Location = new System.Drawing.Point(4, 22);
			this.tab_items_cash.Name = "tab_items_cash";
			this.tab_items_cash.Padding = new System.Windows.Forms.Padding(3);
			this.tab_items_cash.Size = new System.Drawing.Size(499, 447);
			this.tab_items_cash.TabIndex = 1;
			this.tab_items_cash.Text = "现金";
			this.tab_items_cash.UseVisualStyleBackColor = true;
			// 
			// list_items_cash
			// 
			this.list_items_cash.BackColor = System.Drawing.SystemColors.Window;
			this.list_items_cash.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeader5,
			this.columnHeader14,
			this.columnHeader6,
			this.columnHeader7,
			this.columnHeader8,
			this.columnHeader9,
			this.columnHeader10});
			this.list_items_cash.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list_items_cash.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.list_items_cash.FullRowSelect = true;
			this.list_items_cash.GridLines = true;
			this.list_items_cash.HideSelection = false;
			this.list_items_cash.LabelWrap = false;
			this.list_items_cash.Location = new System.Drawing.Point(3, 3);
			this.list_items_cash.Name = "list_items_cash";
			this.list_items_cash.ShowItemToolTips = true;
			this.list_items_cash.Size = new System.Drawing.Size(493, 441);
			this.list_items_cash.TabIndex = 4;
			this.list_items_cash.UseCompatibleStateImageBehavior = false;
			this.list_items_cash.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "名字";
			this.columnHeader5.Width = 115;
			// 
			// columnHeader14
			// 
			this.columnHeader14.Text = "数量";
			this.columnHeader14.Width = 50;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "类型";
			this.columnHeader6.Width = 80;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "颜色1";
			this.columnHeader7.Width = 52;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "颜色2";
			this.columnHeader8.Width = 52;
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "颜色3";
			this.columnHeader9.Width = 49;
			// 
			// columnHeader10
			// 
			this.columnHeader10.Text = "到期时间";
			this.columnHeader10.Width = 75;
			// 
			// tab_items_quest
			// 
			this.tab_items_quest.Controls.Add(this.list_items_quest);
			this.tab_items_quest.Location = new System.Drawing.Point(4, 22);
			this.tab_items_quest.Name = "tab_items_quest";
			this.tab_items_quest.Size = new System.Drawing.Size(499, 447);
			this.tab_items_quest.TabIndex = 2;
			this.tab_items_quest.Text = "任务";
			this.tab_items_quest.UseVisualStyleBackColor = true;
			// 
			// list_items_quest
			// 
			this.list_items_quest.BackColor = System.Drawing.SystemColors.Window;
			this.list_items_quest.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeader2,
			this.columnHeader11,
			this.columnHeader13,
			this.columnHeader19});
			this.list_items_quest.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list_items_quest.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.list_items_quest.FullRowSelect = true;
			this.list_items_quest.GridLines = true;
			this.list_items_quest.HideSelection = false;
			this.list_items_quest.LabelWrap = false;
			this.list_items_quest.Location = new System.Drawing.Point(0, 0);
			this.list_items_quest.Name = "list_items_quest";
			this.list_items_quest.ShowItemToolTips = true;
			this.list_items_quest.Size = new System.Drawing.Size(499, 447);
			this.list_items_quest.TabIndex = 5;
			this.list_items_quest.UseCompatibleStateImageBehavior = false;
			this.list_items_quest.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "名字";
			this.columnHeader2.Width = 255;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "数量";
			this.columnHeader11.Width = 65;
			// 
			// columnHeader13
			// 
			this.columnHeader13.Text = "类型";
			this.columnHeader13.Width = 50;
			// 
			// columnHeader19
			// 
			this.columnHeader19.Text = "到期时间";
			this.columnHeader19.Width = 100;
			// 
			// tab_items_other
			// 
			this.tab_items_other.Controls.Add(this.list_items_other);
			this.tab_items_other.Location = new System.Drawing.Point(4, 22);
			this.tab_items_other.Name = "tab_items_other";
			this.tab_items_other.Size = new System.Drawing.Size(499, 447);
			this.tab_items_other.TabIndex = 3;
			this.tab_items_other.Text = "隐藏";
			this.tab_items_other.UseVisualStyleBackColor = true;
			// 
			// list_items_other
			// 
			this.list_items_other.BackColor = System.Drawing.SystemColors.Window;
			this.list_items_other.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeader20,
			this.columnHeader21,
			this.columnHeader22,
			this.columnHeader26});
			this.list_items_other.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list_items_other.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.list_items_other.FullRowSelect = true;
			this.list_items_other.GridLines = true;
			this.list_items_other.HideSelection = false;
			this.list_items_other.LabelWrap = false;
			this.list_items_other.Location = new System.Drawing.Point(0, 0);
			this.list_items_other.Name = "list_items_other";
			this.list_items_other.ShowItemToolTips = true;
			this.list_items_other.Size = new System.Drawing.Size(499, 447);
			this.list_items_other.TabIndex = 5;
			this.list_items_other.UseCompatibleStateImageBehavior = false;
			this.list_items_other.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader20
			// 
			this.columnHeader20.Text = "名字";
			this.columnHeader20.Width = 255;
			// 
			// columnHeader21
			// 
			this.columnHeader21.Text = "数量";
			this.columnHeader21.Width = 65;
			// 
			// columnHeader22
			// 
			this.columnHeader22.Text = "类型";
			this.columnHeader22.Width = 50;
			// 
			// columnHeader26
			// 
			this.columnHeader26.Text = "到期时间";
			this.columnHeader26.Width = 100;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1008, 601);
			this.Controls.Add(this.tab_items);
			this.Controls.Add(this.SearchReset);
			this.Controls.Add(this.SearchSubCategory);
			this.Controls.Add(this.SearchMainCategory);
			this.Controls.Add(this.SearchByClass);
			this.Controls.Add(this.SearchByName);
			this.Controls.Add(this.lbSearchItemClass);
			this.Controls.Add(this.lbSearchName);
			this.Controls.Add(this.lbSearchCategory);
			this.Controls.Add(this.SearchListView);
			this.Controls.Add(this.SearchItemClass);
			this.Controls.Add(this.SearchName);
			this.Controls.Add(this.SearchTitle);
			this.Controls.Add(this.LogCatText);
			this.Controls.Add(this.TabPanelMail);
			this.Controls.Add(this.TabPanelLeft);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "VindictusManager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.TabPanelLeft.ResumeLayout(false);
			this.TabUserList.ResumeLayout(false);
			this.UserMenuStrip.ResumeLayout(false);
			this.TabItemDetail.ResumeLayout(false);
			this.TabItemDetail.PerformLayout();
			this.TabPanelMail.ResumeLayout(false);
			this.TabMailSend.ResumeLayout(false);
			this.MailMenuStrip.ResumeLayout(false);
			this.TabMailRecv.ResumeLayout(false);
			this.SendMenuStrip.ResumeLayout(false);
			this.tab_items.ResumeLayout(false);
			this.tab_items_normal.ResumeLayout(false);
			this.tab_items_cash.ResumeLayout(false);
			this.tab_items_quest.ResumeLayout(false);
			this.tab_items_other.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem resetGroupSkillToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem maxGroupDarkToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem maxGroupLightToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modUserToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resetQuestTimesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem maxSubClassToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem refreshUserToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addTitlesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addTitleToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip UserMenuStrip;
		private System.Windows.Forms.TextBox LogCatText;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem deleteAllMailsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteMailsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem refreshMailToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip MailMenuStrip;
		private System.Windows.Forms.ColumnHeader chReceiverTitle;
		private Vindictus.Common.DListView MailRecvList;
		private System.Windows.Forms.TabPage TabMailRecv;
		private System.Windows.Forms.ColumnHeader chSendingTitle;
		private Vindictus.Common.DListView MailSendList;
		private System.Windows.Forms.TabPage TabMailSend;
		private System.Windows.Forms.TabControl TabPanelMail;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox Color1Text;
		private System.Windows.Forms.TextBox Color2Text;
		private System.Windows.Forms.TextBox Color3Text;
		private System.Windows.Forms.CheckBox LockColorChk;
		private System.Windows.Forms.Label Color1;
		private System.Windows.Forms.Label Color2;
		private System.Windows.Forms.Label Color3;
		private System.Windows.Forms.TabPage TabItemDetail;
		private System.Windows.Forms.ColumnHeader chUserLevel;
		private System.Windows.Forms.ColumnHeader chUserClass;
		private System.Windows.Forms.ColumnHeader chUserName;
		private Vindictus.Common.DListView UserListView;
		private System.Windows.Forms.TabPage TabUserList;
		private System.Windows.Forms.TabControl TabPanelLeft;
		private System.Windows.Forms.ToolStripMenuItem zhTW2zhCNToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem salonPirceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allSalonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem serverManagerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem modLevelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modClassToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modNameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modAPToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modAttriToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.Button SearchReset;
		private System.Windows.Forms.ComboBox SearchSubCategory;
		private System.Windows.Forms.ComboBox SearchMainCategory;
		private System.Windows.Forms.Button SearchByClass;
		private System.Windows.Forms.Button SearchByName;
		private System.Windows.Forms.Label lbSearchItemClass;
		private System.Windows.Forms.Label lbSearchName;
		private System.Windows.Forms.Label lbSearchCategory;
		private Vindictus.Common.DListView SearchListView;
		private System.Windows.Forms.TextBox SearchItemClass;
		private System.Windows.Forms.TextBox SearchName;
		private System.Windows.Forms.TabControl tab_items;
		private System.Windows.Forms.TabPage tab_items_normal;
		private Vindictus.Common.DListView list_items_normal;
		private System.Windows.Forms.ColumnHeader ch_items_name;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader15;
		private System.Windows.Forms.ColumnHeader ch_items_type;
		private System.Windows.Forms.ColumnHeader ch_items_color1;
		private System.Windows.Forms.ColumnHeader ch_items_color2;
		private System.Windows.Forms.ColumnHeader ch_items_color3;
		private System.Windows.Forms.ColumnHeader ch_items_time;
		private System.Windows.Forms.TabPage tab_items_cash;
		private Vindictus.Common.DListView list_items_cash;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader14;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.TabPage tab_items_quest;
		private Vindictus.Common.DListView list_items_quest;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.ColumnHeader columnHeader19;
		private System.Windows.Forms.TabPage tab_items_other;
		private Vindictus.Common.DListView list_items_other;
		private System.Windows.Forms.ColumnHeader columnHeader20;
		private System.Windows.Forms.ColumnHeader columnHeader21;
		private System.Windows.Forms.ColumnHeader columnHeader22;
		private System.Windows.Forms.ColumnHeader columnHeader26;
		private System.Windows.Forms.ColumnHeader chSearchName;
		private System.Windows.Forms.Label SearchTitle;
		private System.Windows.Forms.ContextMenuStrip SendMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem send1ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem send5ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem send10ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sendNToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem copyItemClassToolStripMenuItem;
	}
}
