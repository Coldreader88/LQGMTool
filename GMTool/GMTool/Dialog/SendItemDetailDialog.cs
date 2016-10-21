/*
 * 由SharpDevelop创建。
 * 用户： keyoyu
 * 日期: 2016/10/21
 * 时间: 13:03
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using GMTool.Bean;

namespace GMTool.Dialog
{
	public partial class SendItemDetailDialog : Form
	{
		private MainForm main;
		private Item item;
		public SendItemDetailDialog(MainForm main,Item item)
		{
			this.item = item;
			this.main = main;
			InitializeComponent();
			this.Text = this.Text += " "+item.ItemName;
		}
		
		private void OKButtonClick(object sender, EventArgs e)
		{
			
		}
		
		private void PrefixComboBoxLevelSelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		private void SuffixComboBoxLevelSelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		private void SpiritLevelComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
	}
}
