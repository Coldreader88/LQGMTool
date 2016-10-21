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

namespace Vindictus
{
	public partial class MainForm : Form
	{
		private CoreConfig Config;
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
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			string code;
			if(!SettingHelper.CheckGameCode(Config,out code)){
				this.Error(R.ErrorGameCode);
				Application.Exit();
			}
			this.Text = R.Title+"-"+Application.ProductVersion.ToString();
		}
		
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
			using(FolderBrowserDialog dlg=new FolderBrowserDialog()){
				dlg.SelectedPath = Application.StartupPath;
				dlg.Description=R.SelectZhTWPath;
				if(dlg.ShowDialog()==DialogResult.OK){
//					dlg.SelectedPath;
				}
			}
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(!this.CloseServer()){
				e.Cancel = true;
			}
		}
	}
}
