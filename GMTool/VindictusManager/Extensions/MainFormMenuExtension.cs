
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vindictus.Bean;
using Vindictus.Enums;
using Vindictus.Helper;
using System.ComponentModel;
using System.Xml;
using ServerManager;
using Vindictus.UI;
using System.Data.Common;
using System.IO;
using System.Threading;

namespace Vindictus.Extensions
{
	public static class MainFormMenuExtension
	{
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
	}
}
