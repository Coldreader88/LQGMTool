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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btn_mssql_open = new System.Windows.Forms.Button();
			this.tb_mssql_db = new System.Windows.Forms.TextBox();
			this.tb_mssql_server = new System.Windows.Forms.TextBox();
			this.tb_mssql_user = new System.Windows.Forms.TextBox();
			this.tb_mssql_pwd = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btn_db_restore = new System.Windows.Forms.Button();
			this.btn_db_attach = new System.Windows.Forms.Button();
			this.btn_db_split = new System.Windows.Forms.Button();
			this.tb_exec_path = new System.Windows.Forms.TextBox();
			this.btn_exec_start = new System.Windows.Forms.Button();
			this.btn_exec_show = new System.Windows.Forms.Button();
			this.btn_update_settings = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.btn_mssql_open);
			this.groupBox1.Controls.Add(this.tb_mssql_db);
			this.groupBox1.Controls.Add(this.tb_mssql_server);
			this.groupBox1.Controls.Add(this.tb_mssql_user);
			this.groupBox1.Controls.Add(this.tb_mssql_pwd);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(385, 79);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "数据库";
			// 
			// btn_mssql_open
			// 
			this.btn_mssql_open.Location = new System.Drawing.Point(313, 20);
			this.btn_mssql_open.Name = "btn_mssql_open";
			this.btn_mssql_open.Size = new System.Drawing.Size(64, 48);
			this.btn_mssql_open.TabIndex = 0;
			this.btn_mssql_open.Text = "连接";
			this.btn_mssql_open.UseVisualStyleBackColor = true;
			// 
			// tb_mssql_db
			// 
			this.tb_mssql_db.Location = new System.Drawing.Point(207, 20);
			this.tb_mssql_db.MaxLength = 16;
			this.tb_mssql_db.Name = "tb_mssql_db";
			this.tb_mssql_db.Size = new System.Drawing.Size(100, 21);
			this.tb_mssql_db.TabIndex = 2;
			this.tb_mssql_db.Text = "heroes";
			// 
			// tb_mssql_server
			// 
			this.tb_mssql_server.Location = new System.Drawing.Point(53, 20);
			this.tb_mssql_server.MaxLength = 64;
			this.tb_mssql_server.Name = "tb_mssql_server";
			this.tb_mssql_server.Size = new System.Drawing.Size(100, 21);
			this.tb_mssql_server.TabIndex = 2;
			this.tb_mssql_server.Text = ".\\SQLEXPRESS";
			// 
			// tb_mssql_user
			// 
			this.tb_mssql_user.Location = new System.Drawing.Point(53, 47);
			this.tb_mssql_user.MaxLength = 16;
			this.tb_mssql_user.Name = "tb_mssql_user";
			this.tb_mssql_user.Size = new System.Drawing.Size(100, 21);
			this.tb_mssql_user.TabIndex = 2;
			this.tb_mssql_user.Text = "sa";
			// 
			// tb_mssql_pwd
			// 
			this.tb_mssql_pwd.Location = new System.Drawing.Point(207, 47);
			this.tb_mssql_pwd.MaxLength = 16;
			this.tb_mssql_pwd.Name = "tb_mssql_pwd";
			this.tb_mssql_pwd.PasswordChar = '*';
			this.tb_mssql_pwd.Size = new System.Drawing.Size(100, 21);
			this.tb_mssql_pwd.TabIndex = 1;
			this.tb_mssql_pwd.Text = "123456";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(160, 50);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(41, 12);
			this.label4.TabIndex = 0;
			this.label4.Text = "密  码";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 12);
			this.label3.TabIndex = 0;
			this.label3.Text = "用户名";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(160, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "数据库";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "服务器";
			// 
			// btn_db_restore
			// 
			this.btn_db_restore.Location = new System.Drawing.Point(403, 12);
			this.btn_db_restore.Name = "btn_db_restore";
			this.btn_db_restore.Size = new System.Drawing.Size(93, 23);
			this.btn_db_restore.TabIndex = 3;
			this.btn_db_restore.Text = "初始化数据库";
			this.toolTip1.SetToolTip(this.btn_db_restore, "从bak文件创建数据库");
			this.btn_db_restore.UseVisualStyleBackColor = true;
			this.btn_db_restore.Click += new System.EventHandler(this.Btn_db_restoreClick);
			// 
			// btn_db_attach
			// 
			this.btn_db_attach.Location = new System.Drawing.Point(403, 41);
			this.btn_db_attach.Name = "btn_db_attach";
			this.btn_db_attach.Size = new System.Drawing.Size(93, 23);
			this.btn_db_attach.TabIndex = 3;
			this.btn_db_attach.Text = "附加数据库";
			this.toolTip1.SetToolTip(this.btn_db_attach, "把存在的数据库文件添加到sql server");
			this.btn_db_attach.UseVisualStyleBackColor = true;
			this.btn_db_attach.Click += new System.EventHandler(this.Btn_db_attachClick);
			// 
			// btn_db_split
			// 
			this.btn_db_split.Location = new System.Drawing.Point(403, 74);
			this.btn_db_split.Name = "btn_db_split";
			this.btn_db_split.Size = new System.Drawing.Size(93, 23);
			this.btn_db_split.TabIndex = 3;
			this.btn_db_split.Text = "分离数据库";
			this.toolTip1.SetToolTip(this.btn_db_split, "把数据库从sql server分离出来");
			this.btn_db_split.UseVisualStyleBackColor = true;
			this.btn_db_split.Click += new System.EventHandler(this.Btn_db_splitClick);
			// 
			// tb_exec_path
			// 
			this.tb_exec_path.Location = new System.Drawing.Point(12, 97);
			this.tb_exec_path.Name = "tb_exec_path";
			this.tb_exec_path.Size = new System.Drawing.Size(385, 21);
			this.tb_exec_path.TabIndex = 4;
			this.tb_exec_path.Text = "D:\\heroes\\server\\NMServer.exe";
			// 
			// btn_exec_start
			// 
			this.btn_exec_start.Location = new System.Drawing.Point(18, 138);
			this.btn_exec_start.Name = "btn_exec_start";
			this.btn_exec_start.Size = new System.Drawing.Size(75, 23);
			this.btn_exec_start.TabIndex = 5;
			this.btn_exec_start.Text = "启动";
			this.btn_exec_start.UseVisualStyleBackColor = true;
			this.btn_exec_start.Click += new System.EventHandler(this.Tb_exec_startClick);
			// 
			// btn_exec_show
			// 
			this.btn_exec_show.Location = new System.Drawing.Point(99, 138);
			this.btn_exec_show.Name = "btn_exec_show";
			this.btn_exec_show.Size = new System.Drawing.Size(75, 23);
			this.btn_exec_show.TabIndex = 5;
			this.btn_exec_show.Text = "显示";
			this.btn_exec_show.UseVisualStyleBackColor = true;
			this.btn_exec_show.Click += new System.EventHandler(this.Tb_exec_showClick);
			// 
			// btn_update_settings
			// 
			this.btn_update_settings.Location = new System.Drawing.Point(403, 103);
			this.btn_update_settings.Name = "btn_update_settings";
			this.btn_update_settings.Size = new System.Drawing.Size(93, 23);
			this.btn_update_settings.TabIndex = 5;
			this.btn_update_settings.Text = "更新设置";
			this.toolTip1.SetToolTip(this.btn_update_settings, "把当前数据库配置保存");
			this.btn_update_settings.UseVisualStyleBackColor = true;
			this.btn_update_settings.Click += new System.EventHandler(this.Btn_update_settingsClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(498, 468);
			this.Controls.Add(this.btn_update_settings);
			this.Controls.Add(this.btn_exec_show);
			this.Controls.Add(this.btn_exec_start);
			this.Controls.Add(this.tb_exec_path);
			this.Controls.Add(this.btn_db_split);
			this.Controls.Add(this.btn_db_attach);
			this.Controls.Add(this.btn_db_restore);
			this.Controls.Add(this.groupBox1);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "服务端管理";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button btn_update_settings;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button btn_exec_show;
		private System.Windows.Forms.Button btn_exec_start;
		private System.Windows.Forms.TextBox tb_exec_path;
		private System.Windows.Forms.Button btn_db_split;
		private System.Windows.Forms.Button btn_db_attach;
		private System.Windows.Forms.Button btn_db_restore;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		internal System.Windows.Forms.TextBox tb_mssql_pwd;
		internal System.Windows.Forms.TextBox tb_mssql_user;
		internal System.Windows.Forms.TextBox tb_mssql_server;
		internal System.Windows.Forms.TextBox tb_mssql_db;
		private System.Windows.Forms.Button btn_mssql_open;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}
