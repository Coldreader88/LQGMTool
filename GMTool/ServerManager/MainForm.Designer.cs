/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/9/27
 * 时间: 16:26
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace ServerManager
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnUpdateConfig = new System.Windows.Forms.Button();
            this.btnSplitDb = new System.Windows.Forms.Button();
            this.btnAttachDb = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.chkNoPeople = new System.Windows.Forms.CheckBox();
            this.btnCreateDbFromBackup = new System.Windows.Forms.Button();
            this.layoutMain = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnShrink = new System.Windows.Forms.Button();
            this.btnSqlserver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUpdateConfig
            // 
            this.btnUpdateConfig.Location = new System.Drawing.Point(4, 5);
            this.btnUpdateConfig.Name = "btnUpdateConfig";
            this.btnUpdateConfig.Size = new System.Drawing.Size(75, 28);
            this.btnUpdateConfig.TabIndex = 1;
            this.btnUpdateConfig.Text = "更新配置";
            this.toolTip1.SetToolTip(this.btnUpdateConfig, "更新数据库连接配置\r\n游戏代码\r\nDS路径\r\n版本信息路径");
            this.btnUpdateConfig.UseVisualStyleBackColor = true;
            this.btnUpdateConfig.Click += new System.EventHandler(this.btnUpdateConfig_Click);
            // 
            // btnSplitDb
            // 
            this.btnSplitDb.Location = new System.Drawing.Point(255, 5);
            this.btnSplitDb.Name = "btnSplitDb";
            this.btnSplitDb.Size = new System.Drawing.Size(63, 28);
            this.btnSplitDb.TabIndex = 1;
            this.btnSplitDb.Text = "分离";
            this.toolTip1.SetToolTip(this.btnSplitDb, "分离后，可以删除日志，再附加");
            this.btnSplitDb.UseVisualStyleBackColor = true;
            this.btnSplitDb.Click += new System.EventHandler(this.btnSplitDb_Click);
            // 
            // btnAttachDb
            // 
            this.btnAttachDb.Location = new System.Drawing.Point(320, 5);
            this.btnAttachDb.Name = "btnAttachDb";
            this.btnAttachDb.Size = new System.Drawing.Size(63, 28);
            this.btnAttachDb.TabIndex = 1;
            this.btnAttachDb.Text = "附加";
            this.toolTip1.SetToolTip(this.btnAttachDb, "可以先清空日志再附加\r\n路径必须和分离前的一致");
            this.btnAttachDb.UseVisualStyleBackColor = true;
            this.btnAttachDb.Click += new System.EventHandler(this.btnAttachDb_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(83, 38);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(82, 28);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "全部启动";
            this.toolTip1.SetToolTip(this.btnStart, "启动全部服务");
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chkNoPeople
            // 
            this.chkNoPeople.AutoSize = true;
            this.chkNoPeople.Location = new System.Drawing.Point(7, 45);
            this.chkNoPeople.Name = "chkNoPeople";
            this.chkNoPeople.Size = new System.Drawing.Size(72, 16);
            this.chkNoPeople.TabIndex = 3;
            this.chkNoPeople.Text = "静默模式";
            this.toolTip1.SetToolTip(this.chkNoPeople, "服务异常结束不提示，自动重新启动服务");
            this.chkNoPeople.UseVisualStyleBackColor = true;
            // 
            // btnCreateDbFromBackup
            // 
            this.btnCreateDbFromBackup.Location = new System.Drawing.Point(83, 5);
            this.btnCreateDbFromBackup.Name = "btnCreateDbFromBackup";
            this.btnCreateDbFromBackup.Size = new System.Drawing.Size(82, 28);
            this.btnCreateDbFromBackup.TabIndex = 1;
            this.btnCreateDbFromBackup.Text = "创建数据库";
            this.btnCreateDbFromBackup.UseVisualStyleBackColor = true;
            this.btnCreateDbFromBackup.Click += new System.EventHandler(this.btnCreateDbFromBackup_Click);
            // 
            // layoutMain
            // 
            this.layoutMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutMain.AutoScroll = true;
            this.layoutMain.Location = new System.Drawing.Point(2, 70);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.Size = new System.Drawing.Size(382, 390);
            this.layoutMain.TabIndex = 2;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(170, 38);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(81, 28);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "全部停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnShrink
            // 
            this.btnShrink.Location = new System.Drawing.Point(255, 37);
            this.btnShrink.Name = "btnShrink";
            this.btnShrink.Size = new System.Drawing.Size(126, 28);
            this.btnShrink.TabIndex = 1;
            this.btnShrink.Text = "压缩数据库";
            this.btnShrink.UseVisualStyleBackColor = true;
            this.btnShrink.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSqlserver
            // 
            this.btnSqlserver.Location = new System.Drawing.Point(170, 5);
            this.btnSqlserver.Name = "btnSqlserver";
            this.btnSqlserver.Size = new System.Drawing.Size(82, 28);
            this.btnSqlserver.TabIndex = 1;
            this.btnSqlserver.Text = "启动数据库";
            this.btnSqlserver.UseVisualStyleBackColor = true;
            this.btnSqlserver.Click += new System.EventHandler(this.btnSqlserver_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.chkNoPeople);
            this.Controls.Add(this.layoutMain);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnShrink);
            this.Controls.Add(this.btnAttachDb);
            this.Controls.Add(this.btnSplitDb);
            this.Controls.Add(this.btnSqlserver);
            this.Controls.Add(this.btnCreateDbFromBackup);
            this.Controls.Add(this.btnUpdateConfig);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 800);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "服务端管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnUpdateConfig;
        private System.Windows.Forms.Button btnCreateDbFromBackup;
        private System.Windows.Forms.Button btnSplitDb;
        private System.Windows.Forms.Button btnAttachDb;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.FlowLayoutPanel layoutMain;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox chkNoPeople;
        private System.Windows.Forms.Button btnShrink;
        private System.Windows.Forms.Button btnSqlserver;
    }
}
