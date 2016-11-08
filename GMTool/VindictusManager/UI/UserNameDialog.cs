using System;
using System.Windows.Forms;
using Vindictus.Bean;
using Vindictus.Extensions;

namespace Vindictus.UI
{
	/// <summary>
	/// 修改角色名字
	/// </summary>
	public class UserNameDialog : InputDialog
	{
		private readonly MainForm mainForm;
		private User user;
		public UserNameDialog(MainForm main):base()
		{
			this.mainForm = main;
			this.Title = R.ModName;
			this.ContentText = R.TipModName;
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
				mainForm.Error(R.NoUser);
				DialogResult = DialogResult.Cancel;
			}else if(user.Name == text){
				DialogResult = DialogResult.Cancel;
			}else if (!mainForm.CheckName(text))
			{
				mainForm.Warnning(string.Format(R.NameExist, text));
			}
			else
			{
				return true;
			}
			return false;
		}
	}
}
