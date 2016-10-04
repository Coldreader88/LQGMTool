/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/28
 * 时间: 15:44
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.IO;
using System.Diagnostics;
using System.Win32;
using System.Windows.Forms;
using System.Text;

namespace GMTool.Common
{
	public class ProcessPlus
	{
		private Process process;
		public bool isRunning{get;private set;}
		public string ExePath{get;private set;}
		public string Args{get;set;}
		public string Title{get;private set;}
		public IntPtr Window{get;private set;}
		public bool IsShow{get;private set;}
		public Action<string,string,bool> OnExitEvent;
		
		public ProcessPlus(string title,string path,string args)
		{
			this.Window = IntPtr.Zero;
			this.Title = title;
			this.ExePath = path;
			this.Args = args;
		}
		public ProcessPlus(string path,string args)
		{
			this.Title = path;
			this.ExePath = path;
			this.Args = args;
		}
		public IntPtr GetWindow()
		{
            if (process == null)
                return IntPtr.Zero;
            if (Window == IntPtr.Zero){
				Window = User32.GetWindowByPid(process.Id);
			}
			return Window;
		}
		public bool Show()
		{
            if (IsShow) return true;
            GetWindow();
			if (Window != IntPtr.Zero)
			{
				return (IsShow=User32.ShowWindowAsync(Window, User32.SW_SHOW));
			}
			return false;
		}
		public bool Hide()
		{
            if (!IsShow) return true;
			GetWindow();
			if (Window != IntPtr.Zero)
			{
				return (IsShow = !User32.ShowWindowAsync(Window, User32.SW_HIDE));
			}
			return false;
		}
		public bool Start(){
			if(isRunning) return true;
			isRunning = true;
			if(process==null||process.HasExited){
				process=new Process();
			}
			process.StartInfo.FileName = ExePath;
			process.StartInfo.WorkingDirectory = Path.GetDirectoryName(ExePath);
			//设定程式执行参数
//			process.StartInfo.UseShellExecute = false;
//			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.Arguments = Args;
			process.EnableRaisingEvents = true;
			IsShow = false;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.Exited += (object sender, EventArgs e)=>{
				if(isRunning){
					Stop();
					//异常结束
					if(OnExitEvent!=null){
						OnExitEvent(ExePath, Args, true);
					}
				}else{
					Stop();
					OnExitEvent(ExePath, Args, false);
				}
			};
			try{
				process.Start();
//				StringBuilder sbText = new StringBuilder(200);
//				User32.GetWindowText(Window,sbText ,200);
//				Title = sbText.ToString();
				return true;
			}catch(Exception e){
                process.Close();
                process = null;
                isRunning = false;
                MessageBox.Show(ExePath + "\n"+e, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return false;
		}
		public void Stop(){
			if(!isRunning)return;
			isRunning = false;
			this.Window = IntPtr.Zero;
			if(process!=null){
				try{
					process.Kill();
				}catch(Exception){
					
				}finally{
					try{
						process.Close();
					}catch{}
					process = null;
				}
			}
		}
	}
}
