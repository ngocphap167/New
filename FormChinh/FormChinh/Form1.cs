using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace FormChinh
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel2.Hide();
            MessageBox.Show("Chúc mừng, bạn đã đăng nhập thành công");
            if (DOT.ADMIN.Type == 1)
            {
                this.mnuNhaCC.Enabled = false;
                this.mnuHanghoa.Enabled = false;
                this.mnuNhanvien.Enabled = false;
                this.mnuTimkiem.Enabled = false;
                this.lậpBáoCáoToolStripMenuItem.Enabled = false;

            }
            //type quyền sử dụng chức năng
            else if (DOT.ADMIN.Type == 2)
            {
                this.danhMụcToolStripMenuItem.Enabled = false;
                this.lậpBáoCáoToolStripMenuItem.Enabled = false;
                this.mnuHoadon.Enabled = false;
            }
        }

        private void mnuNhaCC_Click(object sender, EventArgs e)
        {
            FORM.frmNHACUNGCAP frm = new FORM.frmNHACUNGCAP();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void mnuKhachhang_Click(object sender, EventArgs e)
        {
            FORM.frmKHACHHANG frm = new FORM.frmKHACHHANG();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            DOT.functions.Disconnect();
            Application.Exit();
        }

        private void mnuNhanvien_Click(object sender, EventArgs e)
        {
            FORM.frmNHANVIEN frm = new FORM.frmNHANVIEN();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void loạiSảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FORM.frmLOAISANPHAM frm = new FORM.frmLOAISANPHAM();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void mnuHanghoa_Click(object sender, EventArgs e)
        {
            FORM.frmSANPHAM frm = new FORM.frmSANPHAM();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void mnuHDBan_Click(object sender, EventArgs e)
        {
            FORM.frmHOADONBAN frm = new FORM.frmHOADONBAN();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void báoCáoHàngTồnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaocaosanpham frm = new frmBaocaosanpham();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void báoCáoKinhDoanhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBAOCAODOANHTHU frm = new frmBAOCAODOANHTHU();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void hóaĐơnNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FORM.frmHOADONNHAP frm = new FORM.frmHOADONNHAP();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void nhapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FORM.nhap frm = new FORM.nhap();
            this.Hide();
            //frm.ShowDialog();
            this.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            //DOT.functions.Disconnect();
           // Application.Exit();
            
        }

        private void mnuLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            MessageBox.Show("Tạm biệt, hẹn gặp lại");
            DOT.ADMIN.tennguoidung = "";
            DOT.ADMIN.Type = -1;
            FORM.frmLOGIN frm = new FORM.frmLOGIN();
            frm.Show();
        }

        private void tìmKiếmSảnSẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FORM.frmTIMKIEMSANPHAM frm = new FORM.frmTIMKIEMSANPHAM();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void mnuFindHoadon_Click(object sender, EventArgs e)
        {
            FORM.frmTIMKIEMKHACHHANG frm = new FORM.frmTIMKIEMKHACHHANG();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void cậpNhậtTàiKhảoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FORM.frmADDTAIKHOAN frm = new FORM.frmADDTAIKHOAN();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        int x = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            x++;
            if (x == 10)
            {
                timer1.Stop();
                pictureBox1.Hide();
                label7.Hide();
                label1.Hide();
                label2.Hide();
                label3.Hide();
                label4.Hide();
                label5.Hide();
                label6.Hide();
                panel2.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DOT.functions.Disconnect();
            Application.Exit();
        }

        private void tìmKiếmNhânViênToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FORM.frmTIMKIEMNHANVIEN frm = new FORM.frmTIMKIEMNHANVIEN();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FORM.frmBIEUDO frm = new FORM.frmBIEUDO();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void tìmKiếmNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FORM.frmNHANVIEN frm = new FORM.frmNHANVIEN();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FORM.frmSANPHAM frm = new FORM.frmSANPHAM();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FORM.frmKHACHHANG frm = new FORM.frmKHACHHANG();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FORM.frmNHACUNGCAP frm = new FORM.frmNHACUNGCAP();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FORM.frmHOADONBAN frm = new FORM.frmHOADONBAN();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }
    }
}
