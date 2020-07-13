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
    public partial class frmLOAISANPHAM : Form
    {
        DataTable tbl;
        public frmLOAISANPHAM()
        {
            InitializeComponent();
        }

        private void frmLOAISANPHAM_Load(object sender, EventArgs e)
        {
            txtMasp.Enabled = false;
            btnLuu.Enabled = false;
            LoadDataGridView();
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * FROM dbo.LOAISANPHAM";
            tbl = functions.GetDataToTable(sql);
            dataGridView1.DataSource = tbl;
            dataGridView1.Columns[0].HeaderText = "Mã Loại sản phẩm";
            dataGridView1.Columns[1].HeaderText = "Tên Loại sản phẩm";
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 250;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMasp.Focus();
                return;
            }
            if (tbl.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtMasp.Text = dataGridView1.CurrentRow.Cells["maloai"].Value.ToString();
            txtTensp.Text = dataGridView1.CurrentRow.Cells["tenloai"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }
        private void ResetValue()
        {
            txtMasp.Text = "";
            txtTensp.Text = "";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = true;
            ResetValue();
            txtMasp.Enabled = true;
            txtMasp.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (tbl.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMasp.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTensp.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập loại Sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTensp.Focus();
                return;
            }
            sql = "update dbo.LOAISANPHAM set Tenloai = N'" + txtTensp.Text.Trim().ToString() + "' where maLoai ='" + txtMasp.Text.Trim() + "'";
            functions.RunSQL(sql);
            LoadDataGridView();
            txtMasp.Clear();
            txtTensp.Clear();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tbl.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMasp.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE dbo.LOAISANPHAM WHERE maLoai='" + txtMasp.Text + "'";
                functions.RunSqlDel(sql);
                LoadDataGridView();
                ResetValue();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMasp.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã loại sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMasp.Focus();
                return;
            }
            if (txtTensp.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải tên loại sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTensp.Focus();
                return;
            }
            sql = "SELECT * FROM dbo.loaisanpham WHERE maloai = N'" + txtMasp.Text.Trim() + "'";
            if (functions.CheckKey(sql))
            {
                MessageBox.Show("Mã Nhà Cung Cấp này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMasp.Text = "";
                txtMasp.Focus();
                return;
            }
            sql = "insert into dbo.LOAISANPHAM(maLoai,Tenloai) values('" + txtMasp.Text + "',N'" + txtTensp.Text + "')";
            functions.RunSQL(sql);
            LoadDataGridView();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMasp.Enabled = false;
        }
    }
}
