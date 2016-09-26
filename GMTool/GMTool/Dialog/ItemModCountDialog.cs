/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/26
 * 时间: 16:08
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;
using GMTool.Bean;

namespace GMTool.Dialog
{
	/// <summary>
	/// Description of ItemModCount.
	/// </summary>
	public class ItemModCountDialog: InputDialog
	{
		private MainForm mainForm;
		private User user;
		private Item item;
		private int count;
		public ItemModCountDialog(MainForm main):base()
		{
			this.mainForm=main;
			this.Title = "修改物品数量";
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
				this.ContentText = "最大堆叠数量："+item.MaxStack;
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
				}catch(Exception){
					mainForm.Error("请输入一个数字");
					return false;
				}
				if(count>=1 && count <= item.MaxStack)
				{
					return true;
				}
				else
				{
					this.Error("超出最大堆叠数量:"+item.MaxStack);
				}
			}
			return false;
		}
	}
}
