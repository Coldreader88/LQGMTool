/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/28
 * 时间: 15:44
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Diagnostics;
using System.Win32;
using System.Windows.Forms;

namespace GMTool.Common
{
	public class ProcessPlus
	{
		private Process process;
		public bool isRunning{get;private set;}
		public string Path{get;private set;}
		public string Args{get;set;}
		public string Title{get;private set;}
		public IntPtr Window{get;private set;}
		public bool IsShow{get;private set;}
		public Action<string,string,bool> OnExitEvent;
		
		public ProcessPlus(string title,string path,string args)
		{
			this.Title = title;
			this.Path = path;
			this.Args = args;
		}
		public ProcessPlus(string path,string args)
		{
			this.Title = path;
			this.Path = path;
			this.Args = args;
		}
//		private void GetWindow()
//		{
//			if (Window != IntPtr.Zero) return;
//			Window = User32.FindConsoleWindow(Title);
//		}
//		
		public bool Show()
		{
//			GetWindow();
			if (Window != IntPtr.Zero)
			{
				IsShow = true;
				return User32.ShowWindow(Window);
			}
			return false;
		}
		public bool Hide()
		{
//			GetWindow();
			if (Window != IntPtr.Zero)
			{
				IsShow = false;
				return User32.HideWindow(Window);
			}
			return false;
		}
		public bool Start(){
			if(isRunning) return true;
			isRunning = true;
			if(process==null||process.HasExited){
				process=new Process();
			}
			process.StartInfo.FileName = Path;
			//设定程式执行参数
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.Arguments = Args;
			process.EnableRaisingEvents = true;
			IsShow = false;
//			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.Exited+=(object sender, EventArgs e)=>{
				if(isRunning){
					Stop();
					//异常结束
					if(OnExitEvent!=null){
						OnExitEvent(Path, Args, true);
					}
				}else{
					Stop();
					OnExitEvent(Path, Args, false);
				}
			};
			try{
				process.Start();
				Window = User32.GetWindowByPid(process.Id);
				return true;
			}catch(Exception e){
				MessageBox.Show(""+e, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return false;
		}
		public void Stop(){
			if(!isRunning)return;
			isRunning = false;
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
