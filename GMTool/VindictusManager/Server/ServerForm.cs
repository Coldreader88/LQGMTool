using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using WebServer;
using Vindictus;
using Vindictus.UI;
using Vindictus.Helper;
using Vindictus.Common;
using System.Data.Common;

namespace ServerManager
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class ServerForm : Form
	{
		private delegate void UpdateUI(bool ignore);
		public bool isStart{get;private set;}
		private HttpWebServer HttpServer;
		string LastPath;
		CoreConfig Config;
		List<ProcessPanel> ProcessPanels;
		public ServerForm()
		{
			LastPath = Application.StartupPath;
			ProcessPanels = new List<ProcessPanel>();
			Config = Program.Config;
			InitializeComponent();
			this.Text = R.ServerManager;
			chkNoPeople.Text= R.Daemon;
			btnAttachDb.Text=R.ServerDbAttach;
			btnCreateDbFromBackup.Text=R.ServerDbCreate;
			btnSplitDb.Text = R.ServerDbSplit;
			btnUpdateConfig.Text=R.UpdateServerSettings;
			btnShrink.Text=R.ServerDbCompress;
		}

		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
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
				this.Error(R.ErrorSqlServerNotInstall);
				return false;
			}
			if (!SerivceHelper.IsRunningService(Config.SqlServer))
			{
				if (!SerivceHelper.StartService(Config.SqlServer))
				{
					this.Error(string.Format(R.ErrorSqlServerNotStart, Config.SqlServer));
					return false;
				}
			}
			using(MSSqlHelper db=new MSSqlHelper(Config.ConnectionString)){
				if(!db.Open()){
					this.Error(R.ErrorSqlServerNotConnect);
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
			if (SerivceHelper.IsRunningService(Config.SqlServer))
			{
				if (this.Question(R.TipServerStopSqlServer))
				{
					SerivceHelper.StopService(Config.SqlServer);
				}
			}
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
			this.Text += " " + Config.GameCode;
			InitProcessPanel(Config);
			HttpServer = new HttpWebServer(Config.WebRoot, Config.WebPort);
			updateStatu();
		}

		private void btnUpdateConfig_Click(object sender, EventArgs e)
		{
			int rs = this.UpdateConfig(Config);
			if (rs == 0)
			{
				this.Info(R.TipServerSettingOk);
			}
			else
			{
				string msg = string.Format(R.TipServerSettingFail, rs);
				switch(rs){
					case -5:
						msg +="\n"+R.ErrorNoFindSettings;//设置失败:没找到配置文件");
						break;
					case -4:
						msg +="\n"+string.Format(R.TipBadGameCode,Config.GameCode);
//						this.Error("设置失败:无法识别的游戏代码:"+Config.GameCode);
						break;
					case -2:
						msg +="\n"+R.ErrorSetGameCodeFail;
//						this.Error("设置失败:设置游戏代码失败");
						break;
					case -1:
						msg +="\n"+R.ErrorSqlServerNotConnect;
//						this.Error("设置失败:数据库连接失败");
						break;
					default:
						break;
				}
				this.Error(msg);
				
			}
		}

		private void btnCreateDbFromBackup_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog dlg = new FolderBrowserDialog())
			{
				dlg.SelectedPath = LastPath;
				dlg.Description = R.TipSelectSqlBakPath;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					//bak
					string path = dlg.SelectedPath;
					this.LastPath = path;
					int rs = this.CreateDataBase(Config, path);
					if (rs > 0)
					{
						this.Info(string.Format(R.TipServerCreateDbOK, rs));
					}
					else
					{
						this.Error(R.ErrorServerCreateDbFail);
					}
				}
			}
		}

		private void btnSplitDb_Click(object sender, EventArgs e)
		{
			int i = this.SplitDb(Config);
			if (i > 0)
			{
				this.Info(string.Format(R.TipServerSplitDbOK, i));
			}
			else
			{
				this.Error(R.ErrorServerSplitDbFail);
			}
		}


		private void btnAttachDb_Click(object sender, EventArgs e)
		{
			if (this.Question(R.TipServerClearSqlLog))
			{
				int j = this.CleanDataBaseLog(Config);
				#if DEBUG
				if (j > 0)
				{
					this.Info("delete " + j + " sql logs");
				}
				else if (j < 0)
				{
					return;
				}
				#endif
			}
			//遍历文件，附加
			// string[] files=Directory.GetFiles(Config.DatabasePath, "*.mdf");
			int i = this.AttachDataBase(Config);
			if (i > 0)
			{
				this.Info(string.Format(R.TipServerAttachDbOK , i));
			}
			else
			{
				this.Error(R.TipServerDbAttachFail);
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
					app.Path = app.Path.Replace("${ServerPath}", config.ServerPath);
					if (app.HasStubApp)
					{
						foreach (StubApp a in app.Apps)
						{
							if (a.Disable) continue;
							// apps.Add(a);
							a.Path = app.Path;
							ProcessPanel p = new ProcessPanel(a, width);
							p.OnProcessExit = onProcessExit;
							ProcessPanels.Add(p);
							this.layoutMain.Controls.Add(p);
						}
					}
					else
					{
						//apps.Add(app);
						ProcessPanel p = new ProcessPanel(app, width);
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
					MessageBox.Show(R.ErrorProcessStop+"\n" + path + " " + args);
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			int rs = this.ShrinkDataBase(Config);
			if (rs > 0)
			{
				this.Info(string.Format(R.TipServerCompressDbOK,  rs));
			}
			else
			{
				this.Error(R.ErrorServerCompressDbFail);
			}
		}

		private void btnSqlserver_Click(object sender, EventArgs e)
		{
			if (!SerivceHelper.ExistService(Config.SqlServer))
			{
				this.Error(R.ErrorSqlServerNotInstall);
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
					this.btnStop.Text = R.StopWeb;
					this.btnStop.BackColor = Color.DarkRed;
				}else{
					this.btnStop.Text = R.StartWeb;
					this.btnStop.BackColor = Color.ForestGreen;
				}
			}
			if(!isStart){
				this.btnStart.Text = R.StartAll;
				this.btnStart.BackColor = Color.ForestGreen;
			}else{
				this.btnStart.Text = R.StopAll;
				this.btnStart.BackColor = Color.DarkRed;
			}
			if (!SerivceHelper.IsRunningService(Config.SqlServer))
			{
				this.btnSqlserver.Text = R.StartSql;
				this.btnSqlserver.BackColor = Color.ForestGreen;
			}
			else
			{
				this.btnSqlserver.Text = R.StopSql;
				this.btnSqlserver.BackColor = Color.DarkRed;
			}
		}
	}
}
