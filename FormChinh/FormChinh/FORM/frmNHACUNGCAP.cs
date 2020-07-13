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
    public partial class frmNHACUNGCAP : Form
    {
        DataTable tbNCC;
        public frmNHACUNGCAP()
        {
            InitializeComponent();
        }

        private void frmNHACUNGCAP_Load(object sender, EventArgs e)
        {
            txtMaNCC.Enabled = false;
            btnLuu.Enabled = false;
            LoadDataGridView();
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * FROM dbo.NHACUNGCAP";
            tbNCC = functions.GetDataToTable(sql);
            DataGridView1.DataSource = tbNCC;
            DataGridView1.Columns[0].HeaderText = "Mã Nhà cung cấp";
            DataGridView1.Columns[1].HeaderText = "Tên Nhà cung cấp";
            DataGridView1.Columns[0].Width = 100;
            DataGridView1.Columns[1].Width = 350;
            DataGridView1.AllowUserToAddRows = false;
            DataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void DataGridView1_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNCC.Focus();
                return;
            }
            if (tbNCC.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtMaNCC.Text = DataGridView1.CurrentRow.Cells["MaNCC"].Value.ToString();
            txtTenNCC.Text = DataGridView1.CurrentRow.Cells["TenNCC"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }
        private void ResetValue()
        {
            txtMaNCC.Text = "";
            txtMaNCC.Text = "";
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = true;
            ResetValue();
            txtMaNCC.Enabled = true;
            txtMaNCC.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaNCC.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNCC.Focus();
                return;
            }
            if (txtTenNCC.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNCC.Focus();
                return;
            }
            sql = "SELECT * FROM dbo.NHACUNGCAP WHERE maNCC = N'" + txtMaNCC.Text.Trim() + "'";
            if (functions.CheckKey(sql))
            {
                MessageBox.Show("Mã Nhà Cung Cấp này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNCC.Text = "";
                txtMaNCC.Focus();
                return;
            }
            sql = "insert into dbo.NHACUNGCAP(maNCC,tenNCC) values('" + txtMaNCC.Text + "',N'" + txtTenNCC.Text + "')";
            functions.RunSQL(sql);
            LoadDataGridView();
            ResetValue();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaNCC.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tbNCC.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNCC.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "DELETE dbo.NHACUNGCAP WHERE MaNCC='" + txtMaNCC.Text + "'";
                functions.RunSqlDel(sql);
                LoadDataGridView();
                ResetValue();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (tbNCC.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNCC.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenNCC.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNCC.Focus();
                return;
            }
            sql = "update dbo.NHACUNGCAP set TenNCC = N'" + txtTenNCC.Text.Trim().ToString() + "' where maNCC ='"+txtMaNCC.Text.Trim()+"'";
            functions.RunSQL(sql);
            LoadDataGridView();
            ResetValue();

        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            ResetValue();
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaNCC.Enabled = false;
            txtTenNCC.Clear();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMaNCC_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

        private void txtTenNCC_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
        }

    }
}
