
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vindictus
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
