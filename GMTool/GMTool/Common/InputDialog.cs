/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/26
 * 时间: 15:05
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
	public partial class InputDialog : Form
	{
		public InputDialog()
		{
			InitializeComponent();
		}
		protected Func<string, bool> OnCheckText;
		/// <summary>
		/// 标题
		/// </summary>
		public string Title{
			get{return this.Text;}
			set{this.Text= value;}
		}
		/// <summary>
		/// 描述
		/// </summary>
		public string ContentText{
			get{return this.content.Text;}
			set{this.content.Text= value;}
		}
		/// <summary>
		/// 输入内容
		/// </summary>
		public string InputText{
			get{return this.text.Text;}
			set{this.text.Text= value;}
		}
		private void BtnOKClick(object sender, EventArgs e)
		{
			if(OnCheckText!=null){
				if(OnCheckText(InputText)){
					this.DialogResult = DialogResult.OK;
				}
			}
		}
		
	}
}
