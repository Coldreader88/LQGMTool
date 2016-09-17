using GMTool.Bean;
using GMTool.Helper;
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
    public partial class ChangeNameForm : Form
    {
        private MainForm mainForm;
        private bool check = false;
        private string NewName;
        public ChangeNameForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            if (mainForm != null && mainForm.CurUser != null)
            {
                tb_name.Text = mainForm.CurUser.Name;
            }
        }

        private void btn_check_Click(object sender, EventArgs e)
        {
            if (mainForm == null|| mainForm.CurUser==null)
            {
                this.Error("没有选择角色");
                return;
            }
            tb_name.Text = TextHelper.ToTraditional(tb_name.Text);
            NewName = tb_name.Text;
            if (mainForm.CheckName(NewName))
            {
                check = true;
                this.Info("名字可用:"+ NewName);
            }
            else
            {
                this.Warnning("名字["+ NewName + "]已经存在");
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (!check)
            {
                return;
            }
            try {
                mainForm.ModUserName(mainForm.CurUser, NewName);
            }
            catch (Exception)
            {
            }
            this.DialogResult = DialogResult.OK;
        }

        private void tb_name_TextChanged(object sender, EventArgs e)
        {
            check = false;
        }
    }
}
