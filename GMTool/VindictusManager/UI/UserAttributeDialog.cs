using System;
using System.Windows.Forms;
using Vindictus.Bean;
using Vindictus.Extensions;

namespace Vindictus.UI
{
	public class UserAttributeDialog: InputDialog
	{
		private MainForm mainForm;
		private User user;
		private int _value;
		public UserAttributeDialog(MainForm main):base()
		{
			this.mainForm=main;
			this.ContentText="";
			this.OnCheckText  = OnCheckValue;
			SetUser(mainForm.CurUser, "AP","AP");
		}
		public void SetUser(User user, string stat,string name){
			if(user==null){
				return;
			}
			this.user = user;
			this.Title =string.Format(R.ModAttri, name);
			int val = stat.Value(user);
			this.ContentText= string.Format(R.CurValue, val);
			this.InputText = ""+val;
		}
		public int Value{
			get{return _value;}
		}
		protected bool OnCheckValue(string text){
			if (mainForm == null|| mainForm.CurUser==null)
			{
				mainForm.Error(R.NoUser);
				DialogResult = DialogResult.Cancel;
			}else if((user.level+"") == text){
				DialogResult = DialogResult.Cancel;
			}else {
				try{
					_value = Convert.ToInt32(text);
					if(_value>=1)
					{
						return true;
					}
				}catch(Exception){
				}
				mainForm.Error(R.InputNumber);
			}
			return false;
		}
	}
}
