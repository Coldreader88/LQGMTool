﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Vindictus.Common;
using System.Threading;

namespace Vindictus.UI
{
    public partial class ProcessPanel : UserControl
    {
        private ProcessPlus process;
        public Action<ProcessPanel,string, string, bool> OnProcessExit;
        private StubApp App;
        SynchronizationContext m_SyncContext = null;
        public ProcessPanel()
        {
            InitializeComponent();
            m_SyncContext = SynchronizationContext.Current;
            UpdateStatus();
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
        delegate void SetTextBoxText(ButtonBase tb, string txt);

        private void SetText(ButtonBase tb, string text)
        {
            if (tb.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!tb.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (tb.Disposing || tb.IsDisposed)
                        return;
                }
                tb.Invoke(new SetTextBoxText(SetText), new object[] { tb, text });
            }
            else
            {
                tb.Text = text;
                if (tb == btnStart)
                {
                    tb.Text = text;
                    if(IsRunning()){
                    	tb.BackColor = Color.DarkRed;
                    }else{
                    	tb.BackColor = Color.ForestGreen;
                    }
                }
            }
        }
        
        public void UpdateStatus()
        {
        	SetText(btnShow, IsShowing() ? R.Hide : R.Show);
            SetText(btnStart, IsRunning() ? R.Stop : R.Start);
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
