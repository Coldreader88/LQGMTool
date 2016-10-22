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
using Vindictus.Extensions;
using Vindictus.Bean;
using Vindictus.Enums;
using Vindictus.Common;

namespace Vindictus
{
	public partial class MainForm : Form
	{
		#region form
		private CoreConfig Config;
		public MSSqlHelper Db{get;private set;}
		public User CurUser{get;private set;}
		public DataHelper DataHelper{get;private set;}
		private string DefTitle;
		private SearchHelper Searcher;
		public MainForm()
		{
			Config = Program.Config;
			InitializeComponent();
			viewToolStripMenuItem.Text = R.View;
			serverManagerToolStripMenuItem.Text = R.ServerManager;
			helpToolStripMenuItem.Text = R.Help;
			aboutToolStripMenuItem.Text= R.About;
			allSalonToolStripMenuItem.Text = R.AllSalon;
			salonPirceToolStripMenuItem.Text= R.SalonPirce;
			toolToolStripMenuItem.Text= R.Tool;
			zhTW2zhCNToolStripMenuItem.Text=R.zhTw2zhCn;
			//
			chSendingTitle.Text = R.MailTitle;
			chReceiverTitle.Text = R.MailTitle;
			chUserClass.Text = R.UserClass;
			chUserName.Text= R.UserName;
			chUserLevel.Text = R.UserLevel;
			TabUserList.Text = R.UserList;
			TabItemDetail.Text =R.ItemDetail;
			TabMailRecv.Text=R.MailReceiver;
			TabMailSend.Text =R.MailSending;
			//
			refreshMailToolStripMenuItem.Text = R.RefeshMail;
			deleteAllMailsToolStripMenuItem.Text=R.DeleteAllMails;
			deleteMailsToolStripMenuItem.Text =R.DeleteMails;
			//
			resetGroupSkillToolStripMenuItem.Text=R.ResetGroupSkill;
			maxGroupDarkToolStripMenuItem.Text=R.MaxGroupDark;
			maxGroupLightToolStripMenuItem.Text=R.MaxGroupLight;
			modUserToolStripMenuItem.Text=R.ModUser;
			resetQuestTimesToolStripMenuItem.Text=R.ResetQuestTimes;
			maxSubClassToolStripMenuItem.Text=R.MaxSubClass;
			refreshUserToolStripMenuItem.Text=R.RefreshUser;
			addTitlesToolStripMenuItem.Text=R.AddTitles;
			addTitleToolStripMenuItem.Text=R.AddTitle;
			//
			modAPToolStripMenuItem.Text = R.ModAP;
			modAttriToolStripMenuItem.Text = R.ModAttri;
			modClassToolStripMenuItem.Text=R.ModClass;
			modLevelToolStripMenuItem.Text=R.ModLevel;
			modNameToolStripMenuItem.Text=R.ModName;
			//
			SearchTitle.Text=R.SearchList;
			chSearchName.Text=R.ItemName;
			lbSearchName.Text =R.ItemName;
			lbSearchItemClass.Text=R.ItemClass;
			lbSearchCategory.Text=R.Category;
			SearchByName.Text = R.SearchByName;
			SearchByClass.Text=R.SearchByItemClass;
			SearchReset.Text =R.SearchReset;
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			string code;
			if(!SettingHelper.CheckGameCode(Config,out code)){
				this.Error(R.ErrorGameCode);
				Application.Exit();
			}
			DefTitle = R.Title+"-"+Application.ProductVersion.ToString()+" ("+Config.GameCode+")";
			this.Text = DefTitle;
			if(!SerivceHelper.IsRunningService(Config.SqlServer)){
				SerivceHelper.StartService(Config.SqlServer);
			}
			Db=new MSSqlHelper(Config.ConnectionString);
			DataHelper=new DataHelper(Config);
			this.PostTask(InitTask);
			//第一次菜单初始化
			InitUserMenu();
			this.AddTypes(SearchMainCategory, SearchSubCategory);
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(!this.CloseServer()){
				e.Cancel = true;
			}
		}
		private void InitTask(IWaitDialog dlg){
			dlg.SetTitle(R.TipInit);
			dlg.SetInfo(R.ConnectSqlServer);
			if(!Db.Open()){
				this.Error(R.ErrorSqlServerNotConnect);
			}else{
				try{
					Db.ExcuteSQL("use heroes;");
				}catch(Exception e){
					this.Error(""+e);
					return;
				}
			}
			dlg.SetInfo(R.TipReadText);
			var HeroesText=new HeroesTextHelper();
			HeroesText.Read(Config.GameText, Config.PatchText);
			//
			dlg.SetInfo(R.TipReadItem);
			//读取物品
			try{
				DataHelper.ReadData(HeroesText);
			}catch(Exception e){
				this.Error("ReadData\n"+e);
			}
			Searcher = new SearchHelper(DataHelper.Items);
//			MessageBox.Show("count="+DataHelper.Items.Count);
			dlg.SetInfo(R.TipReadUsers);
			//读取用户
			ReadUsers(true);
		}
		#endregion
		
		#region menu
		void ServerManagerToolStripMenuItemClick(object sender2, EventArgs ex)
		{
			this.ShowServerManager();
		}
		
		void AboutToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.ShowAbout();
		}
		
		void AllSalonToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.AllSaLonPatch(Config);
		}
		
		void SalonPirceToolStripMenuItemClick(object sender2, EventArgs e2)
		{
			this.PricePatch(Config);
		}
		
		void ZhTW2zhCNToolStripMenuItemClick(object sender, EventArgs e)
		{
			using(var dlg=new FolderBrowserDialog()){
				dlg.SelectedPath = Application.StartupPath;
				dlg.Description=R.SelectZhTWPath;
				if(dlg.ShowDialog()==DialogResult.OK){
//					dlg.SelectedPath;
				}
			}
		}
		#endregion

		void UserListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			User user = this.UserListView.GetSelectItem<User>();
			if (user != null)
			{
				this.CurUser = user;
				this.RefeshPackage(PackageType.All);
				this.ReadMails();
//				this.NormalCurItem = 0;
//				this.CashCurItem = 0;
				this.Text = this.DefTitle + "  -  " + user.ToString();
				this.AddTitles(this.addTitleToolStripMenuItem);
				//form.AddSkillBouns(user, this.contentMenuItemMaxScore, this.list_items_cash);
			}
		}
		
		#region common
		public delegate void AddListView(ListView listview,Control label, ListViewItem[] items,int index);
		
		public void AddListViewItems(ListView listview,Control label, ListViewItem[] items,int index){
			label.Text = label.Text.Split('(')[0]+"("+items.Length+")";
			listview.BeginUpdate();
			listview.Items.Clear();
			listview.Items.AddRange(items);
			listview.EndUpdate();
			listview.GoToRow(index);
		}
		public bool CheckUser()
		{
			bool b = CurUser != null;
			if (!b)
			{
				this.Warnning(R.NoUser);
			}
			return b;
		}
		public void log(string text)
		{
			if (this.LogCatText.Text.Length >= this.LogCatText.MaxLength - 1024)
			{
				this.LogCatText.Text = "";
			}
			this.LogCatText.AppendText("\n" + DateTime.UtcNow.ToLongTimeString() + "  " + text + "\n");
		}
		#endregion
		
		#region mail menu
		Thread MailThread;
		public void ReadMails(){
			List<Mail> _sending =Db.ReadSendMailList(CurUser);
			List<Mail> _receiver =Db.ReadRecvMailList(CurUser);
			if(MailThread != null){
				MailThread.Interrupt();
			}
			MailThread=new Thread(
				()=>{
					ListViewItem[] sending = _sending.ToMailListViewItems();
					ListViewItem[] receiver = _receiver.ToMailListViewItems();
					Invoke(new AddListView(AddListViewItems), new object[]{MailSendList, TabMailSend, sending,0});
					Invoke(new AddListView(AddListViewItems), new object[]{MailRecvList, TabMailRecv, receiver,0});
				});
			MailThread.IsBackground =true;
			MailThread.Start();
//			this.AddMails(, Db.ReadRecvMailList(CurUser));
		}
		private ListView GetMailMenu(object sender)
		{
			var menu = sender as ToolStripMenuItem;
			Control parent = null;
			ListView listview = null;
			if (menu != null)
			{
				parent = menu.GetMenuConrtol();
			}
			if (parent == this.MailSendList)
			{
				listview = this.MailSendList;
			}
			else if (parent == this.MailRecvList)
			{
				listview = this.MailRecvList;
			}
			return listview;
		}
		
		void RefreshMailToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ReadMails();
		}
		
		void DeleteMailsToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ListView listview = GetMailMenu(sender);
			bool isSend = listview == this.MailSendList;
			if (listview != null)
			{
				Mail[] mails = listview.GetSelectItems<Mail>();
				if (mails != null)
				{
					foreach (Mail mail in mails)
					{
						if (isSend)
						{
							Db.DeleteSendMail(mail);
						}
						else
						{
							Db.DeleteUserMail(mail);
						}
					}
				}
				ReadMails();
			}
		}
		
		void DeleteAllMailsToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ListView listview = GetMailMenu(sender);
			bool isSend = listview == this.MailSendList;
			if (listview != null)
			{
				Mail[] mails = listview.GetItems<Mail>();
				if (mails != null)
				{
					foreach (Mail mail in mails)
					{
						if (isSend)
						{
							Db.DeleteSendMail(mail);
						}
						else
						{
							Db.DeleteUserMail(mail);
						}
					}
				}
				ReadMails();
			}
		}
		#endregion
		
		#region user menu
		void InitUserMenu(){
			//classs
			this.AddClassesMenus(modClassToolStripMenuItem);
			//attri
			this.InitModAttrMenu(modAttriToolStripMenuItem);
		}
		Thread UserThread;
		void RefreshUserToolStripMenuItemClick(object sender, EventArgs e)
		{
			ReadUsers(false);
		}
		public void ReadUsers(bool isThread=false){
			int index;
			List<User> users = Db.ReadAllUsers();
			if(isThread){
				ListViewItem[] items = users.GetUsersItems(CurUser,out index);
				Invoke(new AddListView(AddListViewItems), new object[]{UserListView, TabUserList, items,index});
			}else{
				if(UserThread!=null){
					UserThread.Interrupt();
				}
				UserThread=new Thread(
					()=>{
						ListViewItem[] items = users.GetUsersItems(CurUser,out index);
						Invoke(new AddListView(AddListViewItems), new object[]{UserListView, TabUserList, items,index});
					}
				);
				UserThread.IsBackground = true;
				UserThread.Start();
			}
		}
		
		void ResetGroupSkillToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(!CheckUser())return;
			this.ResetGroupSkill(CurUser);
		}
		
		void MaxGroupDarkToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(!CheckUser())return;
			this.SetGroupLevel(CurUser, GroupInfo.Dark, 40, true);
		}
		
		void MaxGroupLightToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(!CheckUser())return;
			this.SetGroupLevel(CurUser, GroupInfo.Light, 40, true);
		}
		
		void ResetQuestTimesToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(!CheckUser())return;
			this.ResetQuest(CurUser);
		}
		
		void MaxSubClassToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(!CheckUser())return;
			this.MaxAllSecondClass(CurUser);
		}
		
		void AddTitlesToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(!CheckUser())return;
			this.GetAllTitles(CurUser);
		}
		void modNameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			using (var dlg = new UserNameDialog(this))
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
		void modLevelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			using (var form = new UserLevelDialog(this))
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
		void modAPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			using (var form = new UserAttributeDialog(this))
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
		#endregion
		
		#region Search
		void SearchReset_Click(object sender, EventArgs e)
		{
			SearchItemClass.Text = "";
			SearchName.Text = "";
			if(SearchMainCategory.Items.Count >0){
				SearchMainCategory.SelectedIndex = 0;
			}
			if(SearchSubCategory.Items.Count >0){
				SearchSubCategory.SelectedIndex = 0;
			}
		}
		void SearchByName_Click(object sender, EventArgs e)
		{
			this.AddSearchItemList(SearchListView,SearchTitle,Searcher.SearchItems(SearchName.Text, null, SearchMainCategory.Text, SearchSubCategory.Text, CurUser));
		}
		void SearchByClass_Click(object sender, EventArgs e)
		{
			this.AddSearchItemList(SearchListView,SearchTitle, Searcher.SearchItems(null, SearchItemClass.Text, SearchMainCategory.Text, SearchSubCategory.Text, CurUser));
		}
		void copyItemClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
	
		}
		void send1ToolStripMenuItem_Click(object sender, EventArgs e)
		{
	
		}
		void send5ToolStripMenuItem_Click(object sender, EventArgs e)
		{
	
		}
		void send10ToolStripMenuItem_Click(object sender, EventArgs e)
		{
	
		}
		void sendNToolStripMenuItem_Click(object sender, EventArgs e)
		{
	
		}
		void sendItemToolStripMenuItem_Click(object sender, EventArgs e)
		{
	
		}

		#endregion
	}
}
