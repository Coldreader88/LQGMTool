/*
 * 由SharpDevelop创建。
 * 用户： keyoyu
 * 日期: 2016/10/21
 * 时间: 14:40
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace Vindictus
{
	partial class MainForm
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.serverManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allSalonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.salonPirceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.viewToolStripMenuItem,
									this.toolToolStripMenuItem,
									this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(919, 25);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.serverManagerToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// serverManagerToolStripMenuItem
			// 
			this.serverManagerToolStripMenuItem.Name = "serverManagerToolStripMenuItem";
			this.serverManagerToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.serverManagerToolStripMenuItem.Text = "Server Manager";
			this.serverManagerToolStripMenuItem.Click += new System.EventHandler(this.ServerManagerToolStripMenuItemClick);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItemClick);
			// 
			// toolToolStripMenuItem
			// 
			this.toolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.allSalonToolStripMenuItem,
									this.salonPirceToolStripMenuItem});
			this.toolToolStripMenuItem.Name = "toolToolStripMenuItem";
			this.toolToolStripMenuItem.Size = new System.Drawing.Size(46, 21);
			this.toolToolStripMenuItem.Text = "Tool";
			// 
			// allSalonToolStripMenuItem
			// 
			this.allSalonToolStripMenuItem.Name = "allSalonToolStripMenuItem";
			this.allSalonToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.allSalonToolStripMenuItem.Text = "AllSalon";
			this.allSalonToolStripMenuItem.Click += new System.EventHandler(this.AllSalonToolStripMenuItemClick);
			// 
			// salonPirceToolStripMenuItem
			// 
			this.salonPirceToolStripMenuItem.Name = "salonPirceToolStripMenuItem";
			this.salonPirceToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.salonPirceToolStripMenuItem.Text = "SalonPirce";
			this.salonPirceToolStripMenuItem.Click += new System.EventHandler(this.SalonPirceToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(919, 546);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "VindictusManager";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripMenuItem salonPirceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allSalonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem serverManagerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
	}
}
