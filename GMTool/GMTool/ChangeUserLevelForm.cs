using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GMTool
{
    public partial class ChangeUserLevelForm : Form
    {
        private MainForm mainForm;
        public ChangeUserLevelForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            if (mainForm != null && mainForm.CurUser != null)
            {
                tb_level.Text = ""+mainForm.CurUser.level;
            }
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if(mainForm == null||mainForm.CurUser == null)
            {
                this.Error("没有选择角色");
                return;
            }
            try
            {
                int level = Convert.ToInt32(tb_level.Text);
                if(level>=1 && level <= 200)
                {
                    mainForm.ModUserLevel(mainForm.CurUser, level);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.Error("等级超出1-200的范围");
                }
            }
            catch (Exception)
            {
            }
           
        }
    }
}
