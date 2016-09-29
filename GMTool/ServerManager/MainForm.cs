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
using ServerManager.Comon;

namespace ServerManager
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		ProcessPlus process;
		public MainForm()
		{
			InitializeComponent();
		}
		
		private void Tb_exec_showClick(object sender, EventArgs e)
		{
			if(process!=null&&process.isRunning){
				if(process.IsShow){
					if(process.Hide()){
						btn_exec_show.Text="显示";
					}
				}else{
					if(process.Show()){
						btn_exec_show.Text="隐藏";
					}
				}
			}
		}
		
		private void Tb_exec_startClick(object sender, EventArgs e)
		{
			if(process==null){
				process=new ProcessPlus(tb_exec_path.Text,"");
				process.OnExitEvent += delegate(string path, string arg, bool error) {
					if(error){
						this.Error("异常结束!\n"+path+" "+arg);
					}
				};
			}
			if(process.isRunning){
				process.Stop();
				btn_exec_start.Text="启动";
			}else{
				if(process.Start()){
					btn_exec_start.Text="停止";
					this.Info("启动成功:\n"+process.Title);
				}
			}
		}
		
		
		
		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(process!=null&&process.isRunning){
				process.Stop();
			}
		}
		
		void Btn_update_settingsClick(object sender, EventArgs e)
		{
			XmlHelper helper=new XmlHelper("\\ServiceCore.dll.config");
			//applicationSettings/UnifiedNetwork.Properties.Settings/setting[GameCode]
			//connectionStrings/add[]/connectionString
		}
		
		void Btn_db_splitClick(object sender, EventArgs e)
		{
			
		}
		
		void Btn_db_restoreClick(object sender, EventArgs e)
		{
			
		}
		
		void Btn_db_attachClick(object sender, EventArgs e)
		{
			
		}
	}
}
