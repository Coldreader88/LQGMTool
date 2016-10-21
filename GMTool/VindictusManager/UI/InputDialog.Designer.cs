/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/26
 * 时间: 15:05
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace Vindictus
{
	partial class InputDialog
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.content = new System.Windows.Forms.Label();
			this.text = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// content
			// 
			this.content.Location = new System.Drawing.Point(3, 5);
			this.content.Name = "content";
			this.content.Size = new System.Drawing.Size(271, 41);
			this.content.TabIndex = 1;
			this.content.Text = "Info";
			this.content.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// text
			// 
			this.text.Location = new System.Drawing.Point(27, 53);
			this.text.MaxLength = 64;
			this.text.Name = "text";
			this.text.Size = new System.Drawing.Size(222, 21);
			this.text.TabIndex = 1;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(100, 86);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 35);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.BtnOKClick);
			// 
			// InputDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(276, 147);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.text);
			this.Controls.Add(this.content);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Title";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox text;
		private System.Windows.Forms.Label content;
	}
}
