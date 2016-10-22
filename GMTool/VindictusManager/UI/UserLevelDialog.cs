/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/26
 * 时间: 15:34
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using Vindictus.Bean;

namespace Vindictus.Dialog
{
	public class UserLevelDialog: InputDialog
	{
		private readonly MainForm mainForm;
		private User user;
		private int level;
		public UserLevelDialog(MainForm main):base()
		{
			this.mainForm=main;
			this.Title = R.ModLevel;
			this.ContentText = R.TipModLevel;
			this.OnCheckText  = OnCheckLevel;
			SetUser(mainForm.CurUser);
		}
		public void SetUser(User user){
			if(user==null){
				return;
			}
			this.user = user;
			this.InputText = ""+user.level;
		}
		public int Level{
			get{return level;}
		}
		protected bool OnCheckLevel(string text){
			if (mainForm == null|| mainForm.CurUser==null)
			{
				mainForm.Error(R.NoUser);
				DialogResult = DialogResult.Cancel;
			}if((user.level+"") == text){
				DialogResult = DialogResult.Cancel;
			}else {
				try{
					level = Convert.ToInt32(text);
				}catch(Exception){
					mainForm.Error(R.TipModLevel);
					return false;
				}
				if(level>=1 && level <= 200)
				{
					return true;
				}
				else
				{
					this.Error(R.TipModLevel);
				}
			}
			return false;
		}
	}
}
