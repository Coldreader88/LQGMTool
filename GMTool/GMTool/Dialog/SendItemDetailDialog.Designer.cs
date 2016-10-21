/*
 * 由SharpDevelop创建。
 * 用户： keyoyu
 * 日期: 2016/10/21
 * 时间: 13:03
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace GMTool.Dialog
{
	partial class SendItemDetailDialog
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
			this.QualityComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SpiritLevelComboBox = new System.Windows.Forms.ComboBox();
			this.SpiritSkillComboBox = new System.Windows.Forms.ComboBox();
			this.EnhanceComboBox = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.CountTextBox = new System.Windows.Forms.TextBox();
			this.SuffixComboBoxLevel = new System.Windows.Forms.ComboBox();
			this.SuffixComboBox = new System.Windows.Forms.ComboBox();
			this.PrefixComboBoxLevel = new System.Windows.Forms.ComboBox();
			this.PrefixComboBox = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.ItemClassTextBox = new System.Windows.Forms.TextBox();
			this.OKButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// QualityComboBox
			// 
			this.QualityComboBox.FormattingEnabled = true;
			this.QualityComboBox.Items.AddRange(new object[] {
									"1",
									"2",
									"3",
									"4",
									"5",
									"6",
									"7"});
			this.QualityComboBox.Location = new System.Drawing.Point(315, 42);
			this.QualityComboBox.Name = "QualityComboBox";
			this.QualityComboBox.Size = new System.Drawing.Size(44, 20);
			this.QualityComboBox.TabIndex = 0;
			this.QualityComboBox.Text = "1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(280, 46);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "星数";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 136);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "时装品质";
			// 
			// SpiritLevelComboBox
			// 
			this.SpiritLevelComboBox.FormattingEnabled = true;
			this.SpiritLevelComboBox.Items.AddRange(new object[] {
									"S",
									"A",
									"B",
									"C"});
			this.SpiritLevelComboBox.Location = new System.Drawing.Point(62, 136);
			this.SpiritLevelComboBox.Name = "SpiritLevelComboBox";
			this.SpiritLevelComboBox.Size = new System.Drawing.Size(125, 20);
			this.SpiritLevelComboBox.TabIndex = 0;
			this.SpiritLevelComboBox.Text = "S";
			this.SpiritLevelComboBox.SelectedIndexChanged += new System.EventHandler(this.SpiritLevelComboBoxSelectedIndexChanged);
			// 
			// SpiritSkillComboBox
			// 
			this.SpiritSkillComboBox.FormattingEnabled = true;
			this.SpiritSkillComboBox.Items.AddRange(new object[] {
									"1",
									"2",
									"3",
									"4",
									"5",
									"6",
									"7"});
			this.SpiritSkillComboBox.Location = new System.Drawing.Point(198, 136);
			this.SpiritSkillComboBox.Name = "SpiritSkillComboBox";
			this.SpiritSkillComboBox.Size = new System.Drawing.Size(161, 20);
			this.SpiritSkillComboBox.TabIndex = 0;
			// 
			// EnhanceComboBox
			// 
			this.EnhanceComboBox.FormattingEnabled = true;
			this.EnhanceComboBox.Items.AddRange(new object[] {
									"0",
									"3",
									"5",
									"10",
									"11",
									"12",
									"13",
									"14",
									"15"});
			this.EnhanceComboBox.Location = new System.Drawing.Point(231, 42);
			this.EnhanceComboBox.Name = "EnhanceComboBox";
			this.EnhanceComboBox.Size = new System.Drawing.Size(44, 20);
			this.EnhanceComboBox.TabIndex = 0;
			this.EnhanceComboBox.Text = "0";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(198, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 12);
			this.label3.TabIndex = 1;
			this.label3.Text = "强化";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 105);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 12);
			this.label4.TabIndex = 1;
			this.label4.Text = "字尾附魔";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 76);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(53, 12);
			this.label5.TabIndex = 1;
			this.label5.Text = "字首附魔";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(14, 46);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(29, 12);
			this.label6.TabIndex = 1;
			this.label6.Text = "数量";
			// 
			// CountTextBox
			// 
			this.CountTextBox.Location = new System.Drawing.Point(62, 40);
			this.CountTextBox.MaxLength = 64;
			this.CountTextBox.Name = "CountTextBox";
			this.CountTextBox.Size = new System.Drawing.Size(125, 21);
			this.CountTextBox.TabIndex = 3;
			this.CountTextBox.Text = "1";
			// 
			// SuffixComboBoxLevel
			// 
			this.SuffixComboBoxLevel.FormattingEnabled = true;
			this.SuffixComboBoxLevel.Items.AddRange(new object[] {
									"0",
									"3",
									"5",
									"10",
									"11",
									"12",
									"13",
									"14",
									"15"});
			this.SuffixComboBoxLevel.Location = new System.Drawing.Point(62, 102);
			this.SuffixComboBoxLevel.Name = "SuffixComboBoxLevel";
			this.SuffixComboBoxLevel.Size = new System.Drawing.Size(125, 20);
			this.SuffixComboBoxLevel.TabIndex = 0;
			this.SuffixComboBoxLevel.Text = "0";
			this.SuffixComboBoxLevel.SelectedIndexChanged += new System.EventHandler(this.SuffixComboBoxLevelSelectedIndexChanged);
			// 
			// SuffixComboBox
			// 
			this.SuffixComboBox.FormattingEnabled = true;
			this.SuffixComboBox.Items.AddRange(new object[] {
									"0",
									"3",
									"5",
									"10",
									"11",
									"12",
									"13",
									"14",
									"15"});
			this.SuffixComboBox.Location = new System.Drawing.Point(198, 102);
			this.SuffixComboBox.Name = "SuffixComboBox";
			this.SuffixComboBox.Size = new System.Drawing.Size(161, 20);
			this.SuffixComboBox.TabIndex = 0;
			this.SuffixComboBox.Text = "0";
			// 
			// PrefixComboBoxLevel
			// 
			this.PrefixComboBoxLevel.FormattingEnabled = true;
			this.PrefixComboBoxLevel.Items.AddRange(new object[] {
									"0",
									"3",
									"5",
									"10",
									"11",
									"12",
									"13",
									"14",
									"15"});
			this.PrefixComboBoxLevel.Location = new System.Drawing.Point(62, 73);
			this.PrefixComboBoxLevel.Name = "PrefixComboBoxLevel";
			this.PrefixComboBoxLevel.Size = new System.Drawing.Size(125, 20);
			this.PrefixComboBoxLevel.TabIndex = 0;
			this.PrefixComboBoxLevel.Text = "0";
			this.PrefixComboBoxLevel.SelectedIndexChanged += new System.EventHandler(this.PrefixComboBoxLevelSelectedIndexChanged);
			// 
			// PrefixComboBox
			// 
			this.PrefixComboBox.FormattingEnabled = true;
			this.PrefixComboBox.Items.AddRange(new object[] {
									"0",
									"3",
									"5",
									"10",
									"11",
									"12",
									"13",
									"14",
									"15"});
			this.PrefixComboBox.Location = new System.Drawing.Point(198, 73);
			this.PrefixComboBox.Name = "PrefixComboBox";
			this.PrefixComboBox.Size = new System.Drawing.Size(161, 20);
			this.PrefixComboBox.TabIndex = 0;
			this.PrefixComboBox.Text = "0";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(9, 13);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(41, 12);
			this.label7.TabIndex = 1;
			this.label7.Text = "物品ID";
			// 
			// ItemClassTextBox
			// 
			this.ItemClassTextBox.Location = new System.Drawing.Point(62, 10);
			this.ItemClassTextBox.MaxLength = 64;
			this.ItemClassTextBox.Name = "ItemClassTextBox";
			this.ItemClassTextBox.ReadOnly = true;
			this.ItemClassTextBox.Size = new System.Drawing.Size(301, 21);
			this.ItemClassTextBox.TabIndex = 3;
			this.ItemClassTextBox.Text = "1";
			// 
			// OKButton
			// 
			this.OKButton.Location = new System.Drawing.Point(152, 164);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(75, 31);
			this.OKButton.TabIndex = 4;
			this.OKButton.Text = "确定";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButtonClick);
			// 
			// SendItemDetailDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(375, 203);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.ItemClassTextBox);
			this.Controls.Add(this.CountTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.SpiritLevelComboBox);
			this.Controls.Add(this.SpiritSkillComboBox);
			this.Controls.Add(this.PrefixComboBox);
			this.Controls.Add(this.PrefixComboBoxLevel);
			this.Controls.Add(this.SuffixComboBox);
			this.Controls.Add(this.SuffixComboBoxLevel);
			this.Controls.Add(this.EnhanceComboBox);
			this.Controls.Add(this.QualityComboBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "SendItemDetailDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "物品属性";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.TextBox ItemClassTextBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox PrefixComboBox;
		private System.Windows.Forms.ComboBox PrefixComboBoxLevel;
		private System.Windows.Forms.ComboBox SuffixComboBox;
		private System.Windows.Forms.ComboBox SuffixComboBoxLevel;
		private System.Windows.Forms.TextBox CountTextBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox EnhanceComboBox;
		private System.Windows.Forms.ComboBox SpiritSkillComboBox;
		private System.Windows.Forms.ComboBox SpiritLevelComboBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox QualityComboBox;
	}
}
