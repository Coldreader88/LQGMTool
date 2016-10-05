/*
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

namespace ServerManager
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        string LastPath;
        CoreConfig Config;
        string ConfigPath;
        List<ProcessPanel> ProcessPanels;
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
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!SerivceHelper.ExistService(Config.SqlServer))
            {
                this.Error("SQLServer没有安装");
                return;
            }
            if (!SerivceHelper.IsRunningService(Config.SqlServer))
            {
                if (!SerivceHelper.StartService(Config.SqlServer))
                {
                    this.Error("无法启动数据库:"+Config.SqlServer);
                    return;
                }
                this.btnSqlserver.Text = "停止数据库";
            }
            if (ProcessPanels != null)
            {
                foreach (ProcessPanel p in ProcessPanels)
                {
                    if (!p.IsRunning())
                    {
                        p.StartProcess();
                    }
                }
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (SerivceHelper.IsRunningService(Config.SqlServer))
            {
                SerivceHelper.StopService(Config.SqlServer);
                this.btnSqlserver.Text = "启动数据库";
            }
            if (ProcessPanels != null)
            {
                foreach (ProcessPanel p in ProcessPanels)
                {
                    if (p.IsRunning())
                    {
                        p.StopProcess();
                    }
                }
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                Config = XmlHelper.DeserializeByFile<CoreConfig>(ConfigPath);
                this.Text += " " + Config.GameCode;
                InitProcessPanel(Config);
            }
            catch (Exception ex)
            {
                this.Error("读取配置出错\n" + ex);
            }
            if (SerivceHelper.ExistService(Config.SqlServer))
            {
                if (SerivceHelper.IsRunningService(Config.SqlServer))
                {
                    this.btnSqlserver.Text = "停止数据库";
                }
            }
            // 
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
                this.Error("设置失败:code=" + rs);
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
            int width = this.layoutMain.Size.Width- 28;
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
        public void onProcessExit(ProcessPanel p,string path,string args,bool error)
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
                this.btnSqlserver.Text = "启动数据库";
            }
            else
            {
                SerivceHelper.StartService(Config.SqlServer);
                this.btnSqlserver.Text = "停止数据库";
            }
        }
    }
}
