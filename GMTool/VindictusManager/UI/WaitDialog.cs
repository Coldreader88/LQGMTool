
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vindictus.UI
{
	public partial class WaitDialog : Form
	{
		private delegate void CloseForm(Form form);
		private delegate void SetInfoText(string text);
		public WaitDialog()
		{
			InitializeComponent();
		}
		public string Info{
			get{return textLabel.Text;}
			set{textLabel.Text = value;}
		}
		public bool CanClose;
		
		private void setInfoText(string text){
			textLabel.Text= text;
		}
		public void SetInfo(string info){
			if (textLabel.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
			{
				while (!textLabel.IsHandleCreated)
				{
					//解决窗体关闭时出现“访问已释放句柄“的异常
					if (textLabel.Disposing || textLabel.IsDisposed)
						return;
				}
				textLabel.Invoke(new SetInfoText(setInfoText), new object[]{info});
			}
			else
			{
				setInfoText(info);
			}
		}
		private void closeForm(Form form){
			form.Close();
		}
		public void CloseDialog(){
			CanClose = true;
			if (this.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
			{
				while (!this.IsHandleCreated)
				{
					//解决窗体关闭时出现“访问已释放句柄“的异常
					if (this.Disposing || this.IsDisposed)
						return;
				}
				Invoke(new CloseForm(closeForm), new object[]{this});
			}
			else
			{
				closeForm(this);
			}
		}
	}
}
