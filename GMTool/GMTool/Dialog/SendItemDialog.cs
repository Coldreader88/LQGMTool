/*
 * 由SharpDevelop创建。
 * 用户： keyoyu
 * 日期: 2016/10/21
 * 时间: 12:51
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using GMTool.Bean;

namespace GMTool.Dialog
{
	/// <summary>
	/// Description of SendItemDialog.
	/// </summary>
	public class SendItemDialog: InputDialog
	{
		private MainForm mainForm;
		private User user;
		private Item item;
		private int count;
		public SendItemDialog(MainForm main):base()
		{
			this.mainForm=main;
			this.Title = "输入发送数量";
			this.OnCheckText  = OnCheckCount;
			this.InputText = "1";
			SetUserAndItem(mainForm.CurUser, null);
		}
		public void SetUserAndItem(User user,Item item){
			if(user==null){
				return;
			}
			this.user = user;
			if(item!=null){
				this.item=item;
				this.ContentText = "";
				this.InputText = ""+item.Count;
			}
		}
		public int Count{
			get{return count;}
		}
		protected bool OnCheckCount(string text){
			if (mainForm == null|| mainForm.CurUser==null)
			{
				mainForm.Error("没有选择角色");
				DialogResult = DialogResult.Cancel;
			}if((user.level+"") == text){
				DialogResult = DialogResult.Cancel;
			}else {
				try{
					count = Convert.ToInt32(text);
					if(count>0){
						return true;
					}
				}catch(Exception){
					
				}
				mainForm.Error("输入的数字不对");
					return false;
			}
			return false;
		}
	}
}
