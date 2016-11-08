
using System;
using System.Windows.Forms;
using Vindictus.Bean;

namespace Vindictus.UI
{
	public class ItemModCountDialog: InputDialog
	{
		private MainForm mainForm;
		private User user;
		private Item item;
		private int count;
		public ItemModCountDialog(MainForm main):base()
		{
			this.mainForm=main;
			this.Title = R.ModItemCount;
			this.OnCheckText  = OnCheckCount;
			this.InputText = "0";
			SetUserAndItem(mainForm.CurUser, null);
		}
		public void SetUserAndItem(User user,Item item){
			if(user==null){
				return;
			}
			this.user = user;
			if(item!=null){
				this.item=item;
				this.ContentText = R.MaxStackCount+":"+item.MaxStack;
				this.InputText = ""+item.Count;
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
			}else if((user.level+"") == text){
				DialogResult = DialogResult.Cancel;
			}else {
				try{
					count = Convert.ToInt32(text);
				}catch(Exception){
					mainForm.Error(R.InputNumber);
					return false;
				}
				if(count>=1 && count <= item.MaxStack)
				{
					return true;
				}
				else
				{
					this.Error(R.MaxStackCount+":"+item.MaxStack+"!");
				}
			}
			return false;
		}
	}
}
