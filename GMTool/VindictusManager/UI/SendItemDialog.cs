using System;
using System.Windows.Forms;
using Vindictus.Bean;

namespace Vindictus.UI
{
	public class SendItemDialog: InputDialog
	{
		private readonly MainForm mainForm;
		private User user;
		private ItemClassInfo item;
		private int count;
		public SendItemDialog(MainForm main):base()
		{
			this.mainForm=main;
			this.Title = "输入发送数量";
			this.OnCheckText  = OnCheckCount;
			this.InputText = "1";
			SetUserAndItem(mainForm.CurUser, null);
		}
		public void SetUserAndItem(User user,ItemClassInfo item){
			if(user==null){
				return;
			}
			this.user = user;
			if(item!=null){
				this.item=item;
				this.ContentText = "";
				this.InputText = "1";
			}
		}
		public int Count{
			get{return count;}
		}
		protected bool OnCheckCount(string text){
			if (mainForm == null|| mainForm.CurUser==null)
			{
				mainForm.Error(R.NoUser);
				DialogResult = DialogResult.Cancel;
			}if((user.level+"") == text){
				DialogResult = DialogResult.Cancel;
			}else {
				try{
					count = Convert.ToInt32(text);
					if(count > 0){
						return true;
					}
				}catch{
					
				}
				mainForm.Error(R.InputNumber);
					return false;
			}
			return false;
		}
	}
}
