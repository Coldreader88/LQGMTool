using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vindictus.Helper;
using ServerManager;
using Vindictus.UI;
using System.Data.Common;
using System.ComponentModel;
using System.Threading;
using Vindictus.Extensions;
using Vindictus.Bean;
using Vindictus.Enums;
using Vindictus.Common;
using System.Text;

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
		private ListView curListView;
		private ServerForm serverForm;
		private bool IsClosing = false;
		private delegate void InitFail(bool force);
		private delegate void InitOk();
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
			addTitle0ToolStripMenuItem.Text=R.AddTitle;
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
			//
			send1ToolStripMenuItem.Text=string.Format(R.SendItemCount, 1);
			send5ToolStripMenuItem.Text=string.Format(R.SendItemCount, 5);
			send10ToolStripMenuItem.Text=string.Format(R.SendItemCount, 10);
			sendNToolStripMenuItem.Text=string.Format(R.SendItemCount, "N");
			copyItemClassToolStripMenuItem.Text=R.CopyItemClass;//;
			//
			itemCountToolStripMenuItem.Text = R.ModItemCount;
			clearEnhanceToolStripMenuItem.Text = R.Clear;
			clearGemToolStripMenuItem.Text = R.Clear;
			clearInnerEnchantToolStripMenuItem.Text = R.Clear;
			clearItemStarToolStripMenuItem.Text = R.Clear;
			clearPrefixToolStripMenuItem.Text = R.Clear;
			clearSocreToolStripMenuItem.Text = R.Clear;
			clearSuffixToolStripMenuItem.Text = R.Clear;
			clearLookToolStripMenuItem.Text = R.Clear;
			deleteItemToolStripMenuItem.Text = R.DeleteSelectItem;
			refreshItemToolStripMenuItem.Text = R.RefreshPackage;
			unlimitTimeToolStripMenuItem.Text = R.UnLimitTime;
			color1ToolStripMenuItem.Text=R.ModColor1;
			color2ToolStripMenuItem.Text=R.ModColor2;
			color3ToolStripMenuItem.Text=R.ModColor3;
			allColorToolStripMenuItem.Text=R.AllModColor;
			lbColor1.Text=R.Color1;
			lbColor2.Text=R.Color2;
			lbColor3.Text=R.Color3;
			LockColorChk.Text=R.LockColor;
			enhanceToolStripMenuItem.Text=R.Enhance;
			itemStarToolStripMenuItem.Text=R.Star;
			prefixEnchantToolStripMenuItem.Text=R.PrefixEnchant;
			suffixEnchantToolStripMenuItem.Text=R.SuffixEnchant;
			innerEnchantToolStripMenuItem.Text=R.InnerEnchant;
			clothesSocreToolStripMenuItem.Text=R.ClothesScore;
			gemToolStripMenuItem.Text=R.Gem;
			lookToolStripMenuItem.Text=R.Look;
			setItemClassToolStripMenuItem.Text=R.SetItemClass;
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
				Hide();
				ShowServerManager(true);
			}else{
				Init();
			}
		}
		void Init(){
			if(Db != null){
				Db.Close();
			}
			Db = new MSSqlHelper(Config.ConnectionString);
			if(DataHelper==null){
				DataHelper=new DataHelper(Config);
			}
			this.PostTask(InitTask);
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(!CloseServer()){
				e.Cancel = true;
			}
		}
		void ShowAbout(){
			string str = R.AboutText;
			str=str.Replace("$author","QQ247321453");
			str=str.Replace("$name",R.Title);
			str=str.Replace("$vesion",Application.ProductVersion.ToString());
			MessageBox.Show(str,R.About,MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		bool CloseServer(){
			if(serverForm!=null && serverForm.isStart){
				if(this.Question(R.TipCloseServer)){
					IsClosing = true;
					serverForm.Close();
					return true;
				}else{
					return false;
				}
			}
			return true;
		}
		void ShowServerManager(bool force=false)
		{
			if(serverForm == null){
				serverForm =new ServerForm();
				serverForm.Closing += (sender, e)=> {
					serverForm.Hide();
					e.Cancel = true;
					if(!IsClosing){
						if(Db == null){
							Init();
						}
					}
				};
			}
			if(force){
				serverForm.ShowDialog();
			}else{
				serverForm.Show();
				serverForm.Activate();
			}
		}
		void InitTask(IWaitDialog dlg){
			dlg.SetTitle(R.TipInit);
			dlg.SetInfo(R.ConnectSqlServer);
			if(!Db.Open()){
				Db.Close();
				Db = null;
				this.Error(R.ErrorSqlServerNotConnect);
				Invoke(new InitFail(ShowServerManager), new object[]{true});
				return;
			}else{
				try{
					Db.ExcuteSQL("use heroes;");
				}catch(Exception e){
					this.Error(""+e);
					Db.Close();
					Db=null;
					Invoke(new InitFail(ShowServerManager), new object[]{true});
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
			if(Searcher==null)
				Searcher = new SearchHelper(DataHelper.Items);
			else
				Searcher.Attch(DataHelper.Items);
//			MessageBox.Show("count="+DataHelper.Items.Count);
			dlg.SetInfo(R.TipReadUsers);
			Invoke(new InitOk(InitAllMenus));
			//读取用户
			ReadUsers(true);
		}
		void InitAllMenus(){
			//第一次菜单初始化
			InitUserMenu();
			this.AddTypes(SearchMainCategory, SearchSubCategory);
			this.AddTitles(addTitle0ToolStripMenuItem);
			this.InitCashEnchantMenu(innerEnchantToolStripMenuItem);
			this.InitEnchantMenu(prefixEnchantToolStripMenuItem, suffixEnchantToolStripMenuItem);
			this.InitStart(itemStarToolStripMenuItem);
			this.InitEnhance(enhanceToolStripMenuItem);
			this.InitSkillBouns(clothesSocreToolStripMenuItem);
			Show();
		}
		#endregion
		
		#region menu
		void ServerManagerToolStripMenuItemClick(object sender, EventArgs ex)
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
		
		public void ReadPackage(PackageType type){
			
		}
		
		#region 列表选择
		void ClothesSocreToolStripMenuItemMouseEnter(object sender, EventArgs e)
		{
			this.HideSkillBouns(clothesSocreToolStripMenuItem, CurUser);
		}
		void AddTitle0ToolStripMenuItemMouseEnter(object sender, EventArgs e)
		{
			this.HideAddTitles(CurUser, addTitle0ToolStripMenuItem);
		}
		void UserListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			User user = this.UserListView.GetSelectItem<User>();
			if (user != null)
			{
				this.CurUser = user;
				this.ReadPackage(PackageType.All);
				this.ReadMails();
//				this.NormalCurItem = 0;
//				this.CashCurItem = 0;
				this.Text = this.DefTitle + "  -  " + user.ToString();
				//form.AddSkillBouns(user, this.contentMenuItemMaxScore, this.list_items_cash);
			}
		}
		
		void SearchListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			ItemClassInfo item = this.SearchListView.GetSelectItem<ItemClassInfo>();
			//SetCurItems(item);
		}
		void ItemListPackageSelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		void ItemListCashSelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		#endregion
		
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
		
		#region 菜单显示
		void MailMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			var menu = sender as ContextMenuStrip;
			if(menu !=null){
				menu.Tag = menu.SourceControl;
				curListView = menu.SourceControl as ListView;
				if(curListView == MailSendList){
					//发送箱
					showMenu(MENU_MAIL_BASE|MENU_ATTRIBUTES|MENU_INNET_ANCHANT);
				}else if(curListView == MailRecvList){
					//收件箱
					showMenu(MENU_MAIL_BASE);
				}else if(curListView ==SearchListView){
					//搜过结果
				}else if(curListView == ItemListCash){
					//现金
					showMenu(MENU_ITEM_BASE|MENU_INNET_ANCHANT);
				}else if(curListView==ItemListOther){
					//隐藏
					showMenu(MENU_ITEM_BASE);
				}else if(curListView==ItemListPackage){
					//背包
					showMenu(MENU_ITEM_BASE|MENU_ATTRIBUTES|MENU_COLOR);
				}else if(curListView==ItemListTask){
					//任务
					showMenu(MENU_ITEM_BASE);
				}
			}
		}
		
		public ListView GetListView(){
			return curListView;
		}
		public bool isMailListView(){
			return curListView == MailSendList
				||curListView == MailRecvList;
		}
		
		private void showMenu(int flags){
			this.refreshMailToolStripMenuItem.Visible = (flags & MENU_MAIL_BASE)==MENU_MAIL_BASE;
			this.refreshItemToolStripMenuItem.Visible= (flags & MENU_ITEM_BASE)==MENU_ITEM_BASE;
			this.refreshSep.Visible = (flags & MENU_ATTRIBUTES)==MENU_ATTRIBUTES || (flags & MENU_INNET_ANCHANT)==MENU_INNET_ANCHANT
				||(flags & MENU_ITEM_BASE)==MENU_ITEM_BASE;
			this.innerEnchantToolStripMenuItem.Visible= (flags & MENU_INNET_ANCHANT)==MENU_INNET_ANCHANT;
			this.prefixEnchantToolStripMenuItem.Visible= (flags & MENU_ATTRIBUTES)==MENU_ATTRIBUTES;
			this.suffixEnchantToolStripMenuItem.Visible= (flags & MENU_ATTRIBUTES)==MENU_ATTRIBUTES;
			this.enhanceToolStripMenuItem.Visible= (flags & MENU_ATTRIBUTES)==MENU_ATTRIBUTES;
			this.clothesSocreToolStripMenuItem.Visible= (flags & MENU_INNET_ANCHANT)==MENU_INNET_ANCHANT;
			this.itemStarToolStripMenuItem.Visible= (flags & MENU_ATTRIBUTES)==MENU_ATTRIBUTES;
			this.itemCountToolStripMenuItem.Visible=(flags & MENU_ITEM_BASE)==MENU_ITEM_BASE;
			this.unlimitTimeToolStripMenuItem.Visible= (flags & MENU_ITEM_BASE)==MENU_ITEM_BASE;
			this.colorSep.Visible= (flags & MENU_COLOR)==MENU_COLOR;
			this.color1ToolStripMenuItem.Visible= (flags & MENU_COLOR)==MENU_COLOR;
			this.color2ToolStripMenuItem.Visible= (flags & MENU_COLOR)==MENU_COLOR;
			this.color3ToolStripMenuItem.Visible= (flags & MENU_COLOR)==MENU_COLOR;
			this.allColorToolStripMenuItem.Visible= (flags & MENU_COLOR)==MENU_COLOR;
			this.deleteSep.Visible = (flags & MENU_MAIL_BASE)==MENU_MAIL_BASE || (flags & MENU_ITEM_BASE)==MENU_ITEM_BASE;
			this.deleteItemToolStripMenuItem.Visible= (flags & MENU_ITEM_BASE)==MENU_ITEM_BASE;
			this.gemToolStripMenuItem.Visible= (flags & MENU_ATTRIBUTES)==MENU_ATTRIBUTES;
			this.lookToolStripMenuItem.Visible= (flags & MENU_ATTRIBUTES)==MENU_ATTRIBUTES;
			this.deleteMailsToolStripMenuItem.Visible = (flags & MENU_MAIL_BASE)==MENU_MAIL_BASE;
			this.deleteAllMailsToolStripMenuItem.Visible= (flags & MENU_MAIL_BASE)==MENU_MAIL_BASE;
		}
		const int MENU_MAIL_BASE =1;
		const int MENU_ITEM_BASE =2;
		const int MENU_ATTRIBUTES=4;
		const int MENU_INNET_ANCHANT=8;
		const int MENU_COLOR = 0x10;
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

		void RefreshMailToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!CheckUser()) return;
			ReadMails();
		}
		
		void DeleteMailsToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!CheckUser() || curListView==null) return;
			ListView listview = curListView;
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
			if (!CheckUser() || curListView==null) return;
			ListView listview = curListView;
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
			var items= this.SearchListView.GetSelectItems<ItemClassInfo>();
			if (items != null)
			{
				var sb = new StringBuilder();
				foreach (ItemClassInfo item in items)
				{
					sb.Append(item.ItemClass);
					sb.AppendLine();
				}
				Clipboard.SetDataObject(sb.ToString());
				this.Info("复制了"+items.Length+"个物品ID");
			}
		}
		void send1ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SendSelectItems(1);
		}
		void send5ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SendSelectItems(5);
		}
		void send10ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SendSelectItems(10);
		}
		void sendNToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using(var dlg=new SendItemDialog(this, R.InputSendCount)){
				if(dlg.ShowDialog() == DialogResult.OK){
					int count = dlg.Count;
					SendSelectItems(count);
				}
			}
		}
		private void SendSelectItems(int count)
		{
			if (!CheckUser()) return;
			ItemClassInfo[] items = SearchListView.GetSelectItems<ItemClassInfo>();
			if (items == null || items.Length == 0) return;
			foreach(var item in items){
				int val = 0;
				if (item.Name != null && item.Name.Contains("{0}"))
				{
					using(var dlg=new SendItemDialog(this, R.InputNumber)){
						if(dlg.ShowDialog() == DialogResult.OK){
							val = dlg.Count;
						}
					}
					//弹框输入
					if(val < 0){
						continue;
					}
				}
				string extra = val > 0? ""+val:null;
				if(this.SendItem(CurUser, count, item.ItemClass, item.Name, extra)>0){
					log("发送" + item.Name + "成功");
				}else{
					log("发送" + item.Name + "失败");
				}
			}
			ReadMails();
		}
		#endregion
		
		#region item
		void UnlimitTimeToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(CheckUser()){
				this.ModItemTime(CurUser,null);
			}
		}
		void ItemCountToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (!CheckUser()||curListView==null) return;
			Item item = curListView.GetSelectItem<Item>();
			if (item == null)
			{
				return;
			}
			using (var dlg = new ItemModCountDialog(this))
			{
				dlg.SetUserAndItem(CurUser, item);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					if (this.ModItemCount(CurUser, item, dlg.Count))
					{
						ReadPackage(PackageType.All);
					}
				}
			}
		}
		void RefreshItemToolStripMenuItemClick(object sender, EventArgs e)
		{
			ReadPackage(PackageType.All);
		}
		
		void DeleteItemToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(!CheckUser())return;
			ListView list=GetListView();
			if(isMailListView()){
				return;
			}
			Item[] items=list.GetSelectItems<Item>();
			if(items!=null){
				this.DeleteItem(CurUser, items);
				ReadPackage(PackageType.All);
			}
		}
		void SetItemClassToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(!CheckUser())return;
			ListView listview=GetListView();
			if(listview == null||isMailListView()){
				return;
			}
			Item item = listview.GetSelectItem<Item>();
			if(item == null){
				return;
			}
			using(var dlg = new ItemClassDialog(this)){
				dlg.SetItem(CurUser, item);
				if(dlg.ShowDialog() == DialogResult.OK){
					ItemClassInfo info = dlg.ItemInfo;
					if(this.ModItemAttr(new ItemAttribute(ItemAttributeType.LOOK, info.ItemClass), item)){
						log("修改["+item.Name+"]外观为["+info.Name+"]成功");
					}else{
						log("修改["+item.Name+"]外观为["+info.Name+"]失败");
					}
				}
			}
		}
		#endregion
	}
}
