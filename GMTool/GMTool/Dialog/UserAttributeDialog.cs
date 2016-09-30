/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/26
 * 时间: 16:46
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using GMTool.Bean;
using GMTool.Enums;
using GMTool.Extensions;

namespace GMTool.Dialog
{
	/// <summary>
	/// Description of UserAttributeDialog.
	/// </summary>
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
			this.Title = "修改角色属性："+name;
			int val = stat.Value(user);
			this.ContentText="当前值："+val;
			this.InputText = ""+val;
		}
		public int Value{
			get{return _value;}
		}
		protected bool OnCheckValue(string text){
			if (mainForm == null|| mainForm.CurUser==null)
			{
				mainForm.Error("没有选择角色");
				DialogResult = DialogResult.Cancel;
			}if((user.level+"") == text){
				DialogResult = DialogResult.Cancel;
			}else {
				try{
					_value = Convert.ToInt32(text);
				}catch(Exception){
					mainForm.Error("请输入一个数字");
					return false;
				}
				if(_value>=1)
				{
					return true;
				}
				else
				{
					this.Error("不能为0");
				}
			}
			return false;
		}
	}
}
