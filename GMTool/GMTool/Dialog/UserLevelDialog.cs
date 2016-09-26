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
using GMTool.Bean;

namespace GMTool.Dialog
{
	/// <summary>
	/// Description of UserLevelDialog.
	/// </summary>
	public class UserLevelDialog: InputDialog
	{
		private MainForm mainForm;
		private User user;
		public UserLevelDialog(MainForm main):base()
		{
			this.mainForm=main;
			this.Title = "修改角色等级";
			this.ContentText = "等级范围1-200";
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
		protected bool OnCheckLevel(string text){
			if (mainForm == null|| mainForm.CurUser==null)
			{
				mainForm.Error("没有选择角色");
			}if((user.level+"") == text){
				DialogResult = DialogResult.Cancel;
				return false;
			}else {
				int level = Convert.ToInt32(text);
				if(level>=1 && level <= 200)
				{
					mainForm.ModUserLevel(mainForm.CurUser, level);
					return true;
				}
				else
				{
					this.Error("等级超出1-200的范围");
				}
			}
			return false;
		}
	}
}
