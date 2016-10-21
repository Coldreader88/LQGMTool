namespace Vindictus.UI
{
    partial class ProcessPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        	this.components = new System.ComponentModel.Container();
        	this.btnProcessTitle = new System.Windows.Forms.Button();
        	this.btnStart = new System.Windows.Forms.Button();
        	this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
        	this.btnShow = new System.Windows.Forms.Button();
        	this.SuspendLayout();
        	// 
        	// btnProcessTitle
        	// 
        	this.btnProcessTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.btnProcessTitle.Location = new System.Drawing.Point(4, 3);
        	this.btnProcessTitle.Name = "btnProcessTitle";
        	this.btnProcessTitle.Size = new System.Drawing.Size(238, 35);
        	this.btnProcessTitle.TabIndex = 1;
        	this.btnProcessTitle.Text = "Process Name";
        	this.btnProcessTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	this.btnProcessTitle.UseVisualStyleBackColor = true;
        	// 
        	// btnStart
        	// 
        	this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnStart.BackColor = System.Drawing.Color.ForestGreen;
        	this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        	this.btnStart.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
        	this.btnStart.Location = new System.Drawing.Point(248, 3);
        	this.btnStart.Name = "btnStart";
        	this.btnStart.Size = new System.Drawing.Size(60, 35);
        	this.btnStart.TabIndex = 2;
        	this.btnStart.Text = "启动";
        	this.btnStart.UseVisualStyleBackColor = false;
        	this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
        	// 
        	// btnShow
        	// 
        	this.btnShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.btnShow.Location = new System.Drawing.Point(315, 3);
        	this.btnShow.Name = "btnShow";
        	this.btnShow.Size = new System.Drawing.Size(60, 35);
        	this.btnShow.TabIndex = 2;
        	this.btnShow.Text = "显示";
        	this.btnShow.UseVisualStyleBackColor = true;
        	this.btnShow.Click += new System.EventHandler(this.button1_Click);
        	// 
        	// ProcessPanel
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.Controls.Add(this.btnShow);
        	this.Controls.Add(this.btnStart);
        	this.Controls.Add(this.btnProcessTitle);
        	this.Name = "ProcessPanel";
        	this.Size = new System.Drawing.Size(380, 40);
        	this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnProcessTitle;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnShow;
    }
}
