using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using FormChinh.DOT;

namespace FormChinh.FORM
{
    public partial class frmADDTAIKHOAN : Form
    {
        
        public frmADDTAIKHOAN()
        {
            InitializeComponent();
        }
        private void ResetValue()
        {
            txtAccount.Clear();
            txtPassword.Clear();
            txtPassword2.Clear();
            cboManv.Text = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtAccount.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên tài khoản", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAccount.Focus();
                return;
            }
            if (txtPassword.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }
            if (txtPassword2.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập lại mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword2.Focus();
                return;
            }
            if (cboManv.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải thêm nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboManv.Focus();
                return;
            }
            if (txtPassword.Text != txtPassword2.Text)
            {
                MessageBox.Show("mật khẩu không trùng khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword2.Focus();
                return;
            }
            sql = "select Username from tblAccount where Username = '"+txtAccount.Text+"'";
            if (functions.CheckKey(sql))
            {
                MessageBox.Show("Tào khoản này đã có, bạn phải nhập tên tài khoản khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAccount.Text = "";
                txtAccount.Focus();
                return;
            }
            sql = "insert into tblAccount values ('" + txtAccount.Text.Trim() + "','" + txtPassword.Text.Trim() + "','" + cboManv.Text + "')";
            functions.RunSQL(sql);
            ResetValue();
        }

        private void frmADDTAIKHOAN_Load(object sender, EventArgs e)
        {
            functions.FillCombo("SELECT Manhanvien, Tennhanvien FROM tblNhanvien", cboManv, "Manhanvien", "Manhanvien");
            cboManv.SelectedIndex = -1;
        }
    }
}
