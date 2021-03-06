﻿/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/27
 * 时间: 16:26
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GMTool.Common;
using System.IO;
using System.Threading;
using WebServer;
using System.Diagnostics;

namespace ServerManager
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private delegate void UpdateUI(bool ignore);
		private bool isStart;
		private HttpWebServer HttpServer;
		string LastPath;
		CoreConfig Config;
		string ConfigPath;
		List<ProcessPanel> ProcessPanels;
		private int Pid;
		public MainForm(string path = null)
		{
			LastPath = Application.StartupPath;
			if (string.IsNullOrEmpty(path))
			{
				ConfigPath = PathHelper.Combine(Application.StartupPath, "config.xml");
			}
			ProcessPanels = new List<ProcessPanel>();
			InitializeComponent();
		}

		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(isStart){
				if(!this.Question("服务端正在运行，是否强制退出？")){
					e.Cancel = true;
					//				this.WindowState = FormWindowState.Minimized;
					return;
				}
			}
			if (ProcessPanels != null)
			{
				foreach (ProcessPanel p in ProcessPanels)
				{
					if (p.IsRunning())
					{
						p.ShowProcess();
						p.StopProcess();
					}
				}
			}
			isStart = false;
			if(HttpServer!=null){
				HttpServer.Stop();
			}
			updateStatu();
		}
		private void btnStart_Click(object sender, EventArgs e)
		{
			if(isStart){
				isStart = false;
				btnStart.Enabled=false;
				StopAll();
				btnStart.Enabled=true;
			}else{
				btnStart.Enabled=false;
				if(StartAll()){
					isStart = true;
				}
				btnStart.Enabled=true;
			}
			updateStatu();
		}
		private bool StartAll(){
			if (!SerivceHelper.ExistService(Config.SqlServer))
			{
				this.Error("SQLServer没有安装");
				return false;
			}
			if (!SerivceHelper.IsRunningService(Config.SqlServer))
			{
				if (!SerivceHelper.StartService(Config.SqlServer))
				{
					this.Error("无法启动数据库:" + Config.SqlServer);
					return false;
				}
			}
			if (ProcessPanels != null)
			{
				new Thread(() =>
				           {
				           	foreach (ProcessPanel p in ProcessPanels)
				           	{
				           		if (!p.IsRunning())
				           		{
				           			p.StartProcess();
				           		}
				           	}
				           	//	Invoke(new UpdateUI(updateStatu), new object[]{false});
				           }).Start();
			}
			if(HttpServer!=null){
				HttpServer.Start();
			}
			return true;
		}
		private void StopAll(){
			if (ProcessPanels != null)
			{
				new Thread(() =>
				           {
				           	foreach (ProcessPanel p in ProcessPanels)
				           	{
				           		if (p.IsRunning())
				           		{
				           			p.StopProcess();
				           		}
				           	}
				           	//	Invoke(new UpdateUI(updateStatu), new object[]{false});
				           }).Start();
			}
			if (SerivceHelper.IsRunningService(Config.SqlServer))
			{
				if (this.Question("是否停止数据库?"))
				{
					SerivceHelper.StopService(Config.SqlServer);
				}
			}
			if(HttpServer!=null){
				HttpServer.Stop();
			}
		}
		private void btnStop_Click(object sender, EventArgs e)
		{
			if(HttpServer!=null){
				if(!HttpServer.isListening){
					HttpServer.Start();
				}else{
					HttpServer.Stop();
				}
				updateStatu();
			}
		}
		private void MainForm_Load(object sender, EventArgs e)
		{
			try
			{
				Config = XmlHelper.DeserializeByFile<CoreConfig>(ConfigPath);
				this.Text += " " + Config.GameCode;
				InitProcessPanel(Config);
				HttpServer = new HttpWebServer(Config.WebRoot, Config.WebPort);
			}
			catch (Exception ex)
			{
				this.Error("读取配置出错\n" + ex);
			}
			updateStatu();
		}

		private void btnUpdateConfig_Click(object sender, EventArgs e)
		{
			int rs = this.UpdateConfig(Config);
			if (rs == 0)
			{
				this.Info("设置成功");
			}
			else
			{
				switch(rs){
					case -5:
						this.Error("设置失败:没找到配置文件");
						break;
					case -4:
						this.Error("设置失败:无法识别的游戏代码:"+Config.GameCode);
						break;
					case -3:
						this.Error("设置失败:db3没有找到");
						break;
					case -2:
						this.Error("设置失败:设置游戏代码失败");
						break;
					case -1:
						this.Error("设置失败:数据库连接失败");
						break;
					default:
						this.Error("设置失败:code=" + rs);
						break;
				}
				
			}
		}

		private void btnCreateDbFromBackup_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog dlg = new FolderBrowserDialog())
			{
				dlg.SelectedPath = LastPath;
				dlg.Description = "选择数据库备份bak文件的文件夹";
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					//bak
					string path = dlg.SelectedPath;
					this.LastPath = path;
					int rs = this.CreateDataBase(Config, path);
					if (rs > 0)
					{
						this.Info("创建" + rs + "个数据库成功");
					}
					else
					{
						this.Error("创建数据库失败\n请确保数据库备份文件存在");
					}
				}
			}
		}

		private void btnSplitDb_Click(object sender, EventArgs e)
		{
			int i = this.SplitDb(Config);
			if (i > 0)
			{
				this.Info("分离" + i + "个数据库成功");
			}
			else
			{
				this.Error("分离数据库失败\n请确保数据库存在");
			}
		}


		private void btnAttachDb_Click(object sender, EventArgs e)
		{
			if (this.Question("是否清空日志？"))
			{
				int j = this.CleanDataBaseLog(Config);
				if (j > 0)
				{
					this.Info("删除" + j + "个数据库日志");
				}
				else if (j < 0)
				{
					return;
				}
			}
			//遍历文件，附加
			// string[] files=Directory.GetFiles(Config.DatabasePath, "*.mdf");
			int i = this.AttachDataBase(Config);
			if (i > 0)
			{
				this.Info("附加" + i + "个数据库成功");
			}
			else
			{
				this.Error("附加数据库失败\n请确保数据库mdf存在");
			}
		}

		private void InitProcessPanel(CoreConfig config)
		{
			int width = this.layoutMain.Size.Width - 28;
			this.layoutMain.SuspendLayout();
			this.layoutMain.Controls.Clear();
			//  List<StubApp> apps =new List<StubApp>();
			if (config.Apps != null)
			{
				foreach (App app in config.Apps)
				{
					if (app.Disable) continue;
					if (app.HasStubApp)
					{
						foreach (StubApp a in app.Apps)
						{
							if (a.Disable) continue;
							// apps.Add(a);
							a.Path = app.Path;
							ProcessPanel p = new ProcessPanel(a, width,Config.ServerPriority);
							p.OnProcessExit = onProcessExit;
							ProcessPanels.Add(p);
							this.layoutMain.Controls.Add(p);
						}
					}
					else
					{
						//apps.Add(app);
						ProcessPanel p = new ProcessPanel(app, width,Config.ServerPriority);
						p.OnProcessExit = onProcessExit;
						ProcessPanels.Add(p);
						this.layoutMain.Controls.Add(p);
					}
				}
			}
			this.layoutMain.ResumeLayout();
		}
		public void onProcessExit(ProcessPanel p, string path, string args, bool error)
		{
			if (error)
			{
				if (chkNoPeople.Checked)
				{
					p.StartProcess();
				}
				else
				{
					MessageBox.Show("异常结束!\n" + path + " " + args);
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			int rs = this.ShrinkDataBase(Config);
			if (rs > 0)
			{
				this.Info("压缩" + rs + "个数据库成功");
			}
			else
			{
				this.Error("压缩数据库失败\n请确保数据库存在");
			}
		}

		private void btnSqlserver_Click(object sender, EventArgs e)
		{
			if (!SerivceHelper.ExistService(Config.SqlServer))
			{
				this.Error("SQLServer没有安装");
				return;
			}
			if (SerivceHelper.IsRunningService(Config.SqlServer))
			{
				SerivceHelper.StopService(Config.SqlServer);
			}
			else
			{
				SerivceHelper.StartService(Config.SqlServer);
			}
			updateStatu();
		}
		
		private void updateStatu(){
			if(HttpServer!=null){
				if(HttpServer.isListening){
					this.btnStop.Text = "停止Web";
					this.btnStop.BackColor = Color.DarkRed;
				}else{
					this.btnStop.Text = "启动Web";
					this.btnStop.BackColor = Color.ForestGreen;
				}
			}
			if(!isStart){
				this.btnStart.Text = "全部启动";
				this.btnStart.BackColor = Color.ForestGreen;
			}else{
				this.btnStart.Text = "全部停止";
				this.btnStart.BackColor = Color.DarkRed;
			}
			if (!SerivceHelper.IsRunningService(Config.SqlServer))
			{
				this.btnSqlserver.Text = "启动数据库";
				this.btnSqlserver.BackColor = Color.ForestGreen;
			}
			else
			{
				this.btnSqlserver.Text = "停止数据库";
				this.btnSqlserver.BackColor = Color.DarkRed;
			}
//			if(ignore){
//				if(isStart){
//					this.btnSqlserver.Text = "停止数据库";
//					this.btnSqlserver.BackColor = Color.DarkRed;
//				}else{
//					this.btnSqlserver.Text = "启动数据库";
//					this.btnSqlserver.BackColor = Color.ForestGreen;
//				}
//			}
		}
		void MainFormSizeChanged(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.Hide();
				this.ShowInTaskbar = false;
				this.notifyIcon1.Visible = true;
			}
		}
		void NotifyIcon1Click(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.Show();
				this.WindowState = FormWindowState.Normal;
				this.notifyIcon1.Visible = false;
				this.ShowInTaskbar = true;
			}
		}
		void BtnGameClick(object sender, EventArgs ea)
		{
			string path = PathHelper.Combine(Config.GamePath, "Vindictus.exe");
			string arg =  " -stage -dev -lang "+Config.GameCode+" -noupdate  -localized";
			var process = new Process();
			process.StartInfo.FileName = path;
			process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
			//设定程式执行参数
//			process.StartInfo.UseShellExecute = false;
//			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.Arguments = arg;
			try{
				process.Start();
				Keys.F10
				Pid = process.Id;
				this.WindowState = FormWindowState.Minimized;
			}catch(Exception e){
				this.Error("启动游戏失败。\n"+(path+" "+arg)+"\n"+e);
			}
			if((int)Config.ClientPriority > 0){
				try{
					process.PriorityClass = Config.ClientPriority;
				}catch{
					
				}
			}
		}
	}
}
