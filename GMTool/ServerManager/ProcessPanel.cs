using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GMTool.Common;

namespace ServerManager
{
    public partial class ProcessPanel : UserControl
    {
        private ProcessPlus process;
        public Action<ProcessPanel,string, string, bool> OnProcessExit;
        private StubApp App;
        public ProcessPanel()
        {
            InitializeComponent();
        }
        public ProcessPanel(StubApp app,int width)
        {
            InitializeComponent();
            int h = this.Size.Height;
            this.Size = new Size(width, h);
            SetApp(app);
        }
        public void SetApp(StubApp app)
        {
            this.App = app;
            //TODO
            StopProcess();
            if (app != null)
            {
                if (!string.IsNullOrEmpty(app.Name))
                {
                    btnProcessTitle.Text = app.Name;
                }
                else
                {
                    btnProcessTitle.Text = Path.GetFileNameWithoutExtension(app.Path);
                }
                toolTip1.SetToolTip(btnProcessTitle, btnProcessTitle.Text+"\n"+app.Path+"\n"+app.Args);
                process = new ProcessPlus(app.Path, app.Args);
                process.OnExitEvent += delegate (string path, string arg, bool error) {
                    UpdateStatus();
                    if (OnProcessExit != null)
                    {
                        OnProcessExit(this,path, arg, error);
                    }
                };
            }
        }

        public void UpdateStatus()
        {
            if (IsShowing())
            {
                btnShow.Text = "隐藏";
            }
            else
            {
                btnShow.Text = "显示";
            }
            if (IsRunning())
            {
                btnStart.Text = "停止";
                btnStart.BackColor = Color.DarkRed;
            }
            else
            {
                btnStart.Text = "启动";
                btnStart.BackColor = Color.ForestGreen;
            }
        }

        public void StartProcess()
        {
            if (process != null)
            {
                process.Start();
                UpdateStatus();
            }
        }
        public void StopProcess()
        {
            if (process != null)
            {
                process.Stop();
                UpdateStatus();
            }
        }
        public void ShowProcess()
        {
            if (process != null)
            {
                process.Show();
                UpdateStatus();
            }
        }

        public void HideProcess()
        {
            if (process != null)
            {
                process.Hide();
                UpdateStatus();
            }
        }

        public bool IsShowing()
        {
            return process!=null&& process.IsShow;
        }
        public bool IsRunning()
        {
            return process != null && process.isRunning;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (IsRunning())
            {
                StopProcess();
            }
            else
            {
                StartProcess();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsShowing())
            {
                HideProcess();
            }
            else
            {
                ShowProcess();
            }
        }
    }
}
