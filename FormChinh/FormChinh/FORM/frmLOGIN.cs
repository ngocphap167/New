using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FormChinh.FORM
{
    public partial class frmLOGIN : Form
    {
        public frmLOGIN()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from tblAccount where Username=@Username and Password=@Password", DOT.ADMIN.con);
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DOT.ADMIN.tennguoidung = dt.Rows[0]["Username"].ToString().Trim();
                DOT.ADMIN.Type = Convert.ToInt32(dt.Rows[0]["Type"]);

                frmHOADONBAN.usename = txtUsername.Text;
                frmHOADONNHAP.usename = txtUsername.Text;

                Form1 f = new Form1();
                f.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại, Sai tên người dùng hoặc mật khẩu", "Thông báo đăng nhập thất bại", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void frmLOGIN_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát khỏi chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void frmLOGIN_Load(object sender, EventArgs e)
        {
            DOT.functions.Connect();
            DOT.ADMIN.con = new SqlConnection(DOT.ADMIN.ConnectString);
            DOT.ADMIN.con.Open();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            txtUsername.Focus();
        }
    }
}
