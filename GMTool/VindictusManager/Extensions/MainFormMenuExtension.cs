
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using ServerManager;
using Vindictus;
using System.Data.Common;
using Vindictus.Bean;
using Vindictus.Enums;
using Vindictus.UI;

namespace Vindictus.Extensions
{
	public static class MainFormMenuExtension
	{
		#region main menu
		private static ServerForm serverForm;
		public static void ShowServerManager(this MainForm form)
		{
			if(serverForm==null){
				serverForm =new ServerForm();
				serverForm.Closing+= delegate(object sender, CancelEventArgs e) {
					serverForm.Hide();
					e.Cancel = true;
				};
			}
			serverForm.Show();
			serverForm.Activate();
		}
		public static void ShowAbout(this MainForm form){
			string str = R.AboutText;
			str=str.Replace("$author","QQ247321453");
			str=str.Replace("$name",R.Title);
			str=str.Replace("$vesion",Application.ProductVersion.ToString());
			MessageBox.Show(str,R.About,MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		public static bool CloseServer(this MainForm form){
			if(serverForm!=null && serverForm.isStart){
				if(form.Question(R.TipCloseServer)){
					serverForm.Close();
					return true;
				}else{
					return false;
				}
			}
			return true;
		}
		#endregion
		
		#region 时装合成
		public static void AddSkillBouns(this MainForm main,User user,ToolStripDropDownItem menuitem){
			menuitem.DropDownItems.Clear();
			var infos =main.DataHelper.SynthesisSkillBonues.Values;
			foreach (SkillBonusInfo info in infos)
			{
				if(user.IsEnable(info.ClassRestriction)){
					var tsmi = new ToolStripMenuItem(info.Grade+" "+info.DESC);
					tsmi.Tag = info;
					tsmi.ToolTipText = info.ToString();
					tsmi.Click += (sender, e) => {
						if (!main.CheckUser()) return;
						ListView listview= main.GetListView();
						if (listview != null && main.ModItemScore(main.CurUser, info.GetKey(), listview.GetSelectItems<Item>())>0)
						{
							main.ReadPackage(PackageType.Cash);
						}
					};
					menuitem.DropDownItems.Add(tsmi);
				}
			}
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
		static ClassInfo lastClass;
		public static void HideAddTitles(this  MainForm main,User user, ToolStripDropDownItem menuitem){
			if(user==null||lastClass == user.Class){
				return;
			}
			lastClass = user.Class;
			ToolStripItemCollection _items= menuitem.DropDownItems;
			foreach(ToolStripDropDownItem _item in _items){
				int count = _item.DropDownItems.Count;
				ToolStripItemCollection items= _item.DropDownItems;
				foreach(ToolStripDropDownItem item in items){
					item.Visible = true;
					var info = item.Tag as TitleInfo;
					if(info != null){
						if (!lastClass.IsEnable(info.ClassRestriction))
						{
							item.Visible = false;
							count--;
						}
						if (info.OnlyClass != ClassInfo.UnKnown)
						{
							if (lastClass != info.OnlyClass)
							{
								item.Visible = false;
								count--;
							}
						}
					}
				}
				if(count == 0){
					_item.Visible = false;
				}else{
					_item.Visible = true;
				}
			}
		}
		public static void AddTitles(this MainForm main, ToolStripDropDownItem menuitem)
		{
			List<long> titleIds= main.GetTitles(main.CurUser);
			TitleInfo[] titles = main.DataHelper.GetTitles();
			int k = 0;
			var items=new List<ToolStripDropDownItem>();
			var level = new ToolStripMenuItem("lv." + (k * 10 + 1) + "-" + ((k + 1) * 10));
			items.Add(level);
			int max = 20;
			int i = 0;
			foreach (TitleInfo cls in titles)
			{
				if (cls.RequiredLevel <= (k + 1) * 10)
				{
					if (i % max == 0 && i >= max)
					{
						level = new ToolStripMenuItem("lv." + (k * 10 + 1) + "-" + ((k + 1) * 10) + "(" + (i / max + 1) + ")");
						items.Add(level);
					}
					i++;
					var tsmi = new ToolStripMenuItem(cls.ToShortString());
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
							var menu = sender as ToolStripMenuItem;
							if (menu != null && menu.Tag != null)
							{
								var info = menu.Tag as TitleInfo;
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
			menuitem.DropDownItems.Clear();
			menuitem.DropDownItems.AddRange(items.ToArray());
		}
		
		#endregion
		
		#region 分类
		public static void AddTypes(this MainForm main, ComboBox mainmenuitem, ComboBox submenuitem)
		{
			mainmenuitem.Items.Clear();
			submenuitem.Items.Clear();
			mainmenuitem.Items.AddRange(MainCategoryEx.Values);
			submenuitem.Items.AddRange(SubCategoryEx.Values);
			mainmenuitem.SelectedIndex = 0;
			submenuitem.SelectedIndex = 0;
		}
		#endregion
		
		#region 职业
		public static void AddClassesMenus(this MainForm main,ToolStripDropDownItem menuitem)
		{
			menuitem.DropDownItems.Clear();
			Array classes = Enum.GetValues(typeof(ClassInfo));
			foreach (ClassInfo cls in classes)
			{
				if (cls != ClassInfo.UnKnown)
				{
					var tsmi = new ToolStripMenuItem(cls.Name());
					tsmi.Tag = cls;
					tsmi.ToolTipText = cls.ToString() + " " + cls.Index();
					tsmi.Click += (object sender, EventArgs e) => {
						if (!main.CheckUser()) return;
						var menu = sender as ToolStripMenuItem;
						if (menu != null && menu.Tag != null)
						{
							var info = (ClassInfo)menu.Tag;
							if(main.CurUser.Class == info){
								return;
							}
							if(main.Question(string.Format(R.ClassMod,  main.CurUser.Class.Name(), info.Name()))){
								//
								if (main.ModUserClass(main.CurUser, info))
								{
									main.ReadUsers(false);
								}
							}
						}
					};
					menuitem.DropDownItems.Add(tsmi);
				}
			}
		}
		#endregion
		
		#region 属性
		public static void InitModAttrMenu(this MainForm main,ToolStripDropDownItem menuitem){
			string[] stats=new string[]{
				"STR",
				"DEX",
				"INT",
				"WILL",
				"LUCK",
				"HP",
				"STAMINA"
			};
			menuitem.DropDownItems.Clear();
			foreach(string stat in stats){
				string name = stat.StatName();
				var tsmi = new ToolStripMenuItem(name);
				tsmi.Tag = stat;
				tsmi.Click += (object sender, EventArgs e) => {
					if (!main.CheckUser()) return;
					var menu = sender as ToolStripMenuItem;
					if (menu != null && menu.Tag != null)
					{
						string info = (string)menu.Tag;
						//
						using (var form = new UserAttributeDialog(main))
						{
							form.SetUser(main.CurUser, stat, name);
							if (form.ShowDialog() == DialogResult.OK)
							{
								int ap = form.Value;
								if(ap>0 && main.ModUserInfo(main.CurUser, info, ap)){
									main.log("Mod Ap[" + main.CurUser.Name + "]"+name+"为"+ap);
									main.ReadUsers(false);
								}
							}
						}
					}
				};
				menuitem.DropDownItems.Add(tsmi);
			}
		}
		#endregion
		
		#region 附魔
		private static void EnchantItem(MainForm main, EnchantInfo info,ListView listview){
			if(info ==null||listview==null)
				return;
			if(main.isMailListView()){
				Mail mail = listview.GetSelectItem<Mail>();
				string itemclass= mail.ItemClassEx;
				//xxx[prefix:xx][suffix:xxx]
				string type= (info.IsPrefix?ItemAttributeType.PREFIX:ItemAttributeType.SUFFIX).ToString();
				string head = "["+type+":";
				int i = itemclass.IndexOf(head);
				if(i>0){
					int e = itemclass.IndexOf("]", i+head.Length);
					string val = itemclass.Substring(0, i);
					val+="["+type+":"+info.Class+"]";
					val+=itemclass.Substring(e+1);
					itemclass = val;
				}else{
					itemclass +="["+type+":"+info.Class+"]";
				}
				if(main.Db.ModMailItem(mail, itemclass)){
					main.log(mail.Title + " 附魔【" + info.Name + "】成功。");
					main.ReadMails();
				}else{
					main.log(mail.Title + " 附魔【" + info.Name + "】失败。");
				}
			}else{
				Item item = listview.GetSelectItem<Item>();
				//附魔
				if (main.ItemEnchant(item, info))
				{
					main.log(item.Name + " 附魔【" + info.Name + "】成功。");
					main.ReadPackage(item.Package);
				}
				else
				{
					main.log(item.Name + " 附魔【" + info.Name + "】失败。");
				}
			}
		}
		
		public static void InitCashEnchantMenu(this MainForm main, ToolStripDropDownItem inner)
		{
			//contentMenuCashInnerEnchant
			inner.DropDownItems.Clear();
			EnchantInfo[] enchantinfos = main.DataHelper.GetEnchantInfos();
			foreach (EnchantInfo info in enchantinfos)
			{
				var tsmi = new ToolStripMenuItem(info.Name);
				tsmi.Tag = info;
				tsmi.ToolTipText = info.ToString();//提示文字为真实路径
				tsmi.Click += (sender, e) => {
					ListView listview = main.GetListView();
					if(listview == null){
						return;
					}
					
					var menu = sender as ToolStripMenuItem;
					if (menu == null || menu.Tag == null)
					{
						return;
					}
					var _info = menu.Tag as EnchantInfo;
					EnchantItem(main,_info,listview);
				};
				if (info.Constraint!=null && info.Constraint.Contains(SubCategory.INNERARMOR.ToString()))
				{
					if (!info.Class.EndsWith("day7"))
					{
						inner.DropDownItems.Add(tsmi);
					}
				}
			}
		}
		public static void InitEnchantMenu(this MainForm main, ToolStripDropDownItem prefixmenuitem, ToolStripDropDownItem suffixmenuitem)
		{
			prefixmenuitem.DropDownItems.Clear();
			suffixmenuitem.DropDownItems.Clear();
			EnchantInfo[] enchantinfos = main.DataHelper.GetEnchantInfos();
			ToolStripMenuItem prelist = null;
			ToolStripMenuItem suflist = null;
			int li=0, lj = 0;
			int maxi = 0, maxj = 0;
			int max = 15;
			foreach (EnchantInfo info in enchantinfos)
			{
				var tsmi = new ToolStripMenuItem(info.Name);
				tsmi.Tag = info;
				tsmi.ToolTipText = info.ToString();//提示文字为真实路径
				tsmi.Click += (sender, e)=> {
					ListView listview = main.GetListView();
					if(listview == null){
						return;
					}
					
					var menu = sender as ToolStripMenuItem;
					if (menu == null || menu.Tag == null)
					{
						return;
					}
					var _info = menu.Tag as EnchantInfo;
					EnchantItem(main,_info,listview);
				};
				
				if (info.IsPrefix)
				{
					if (prelist == null || li != info.EnchantLevel)
					{
						maxi = 0;
						li = info.EnchantLevel;
						prelist = new ToolStripMenuItem("等级："+li);
						prefixmenuitem.DropDownItems.Add(prelist);
					}
					if (maxi % max == 0 && maxi>=max)
					{
						prelist = new ToolStripMenuItem("等级：" + li + "-" + (maxi / max+1));
						prefixmenuitem.DropDownItems.Add(prelist);
					}
					if (info.Constraint != SubCategory.INNERARMOR.ToString())
					{
						maxi++;
						prelist.DropDownItems.Add(tsmi);
					}
				}
				else
				{
					if (suflist == null || lj != info.EnchantLevel)
					{
						maxj = 0;
						lj = info.EnchantLevel;
						suflist = new ToolStripMenuItem("等级：" + lj);
						suffixmenuitem.DropDownItems.Add(suflist);
					}
					if (maxj % max == 0 && maxj>=max)
					{
						prelist = new ToolStripMenuItem("等级：" + lj + "-" + (maxj / max+1));
						suffixmenuitem.DropDownItems.Add(suflist);
					}
					maxj++;
					suflist.DropDownItems.Add(tsmi);
				}
			}
			
		}
		#endregion
	}
}
