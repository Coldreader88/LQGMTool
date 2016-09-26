/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/26
 * 时间: 15:16
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using GMTool.Bean;

namespace GMTool.Dialog
{
	/// <summary>
	/// 修改角色名字
	/// </summary>
	public class UserNameDialog : InputDialog
	{
		private MainForm mainForm;
		private User user;
		public UserNameDialog(MainForm main):base()
		{
			this.mainForm = main;
			this.Title = "修改角色名字";
			this.ContentText = "请输入长度小于32的名字，台服请用繁体";
			this.OnCheckText  = OnCheckName;
			SetUser(mainForm.CurUser);
		}
		public void SetUser(User user){
			if(user==null){
				return;
			}
			this.user = user;
			this.InputText = user.Name;
		}
		protected bool OnCheckName(string text){
			if (mainForm == null|| mainForm.CurUser==null)
			{
				mainForm.Error("没有选择角色");
			}if(user.Name == text){
				DialogResult = DialogResult.Cancel;
				return false;
			}else if (!mainForm.CheckName(text))
			{
				mainForm.Warnning("名字["+ text + "]已经存在");
			}
			else
			{
				return true;
			}
			return false;
		}
	}
}
