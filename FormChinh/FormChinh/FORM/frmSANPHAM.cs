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
    public partial class frmSANPHAM : Form
    {
        DataTable tblH;
        public frmSANPHAM()
        {
            InitializeComponent();
        }

        private void frmSANPHAM_Load(object sender, EventArgs e)
        {
            string sql;
            sql = "select * from dbo.LOAISANPHAM";
            txtMahang.Enabled = false;
            btnLuu.Enabled = false;
            btnBoqua.Enabled = false;
            LoadDataGridView();
            functions.FillCombo(sql,cboLoaisp, "maLoai","maLoai");
            cboLoaisp.SelectedIndex = -1;
            ResetValues();
        }
        private void ResetValues()
        {
            txtMahang.Text = "";
            txtTenhang.Text = "";
            txtSoluong.Text = "0";
            txtDongianhap.Text = "0";
            txtDongiaban.Text = "0";
            txtSoluong.Enabled = true;
            txtDongianhap.Enabled = false;
            txtDongiaban.Enabled = false;
            txtAnh.Text = "";
            picAnh.Image = null;
            txtGhichu.Text = "";
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * from tblHang";
            tblH = functions.GetDataToTable(sql);
            DataGridView.DataSource = tblH;
            DataGridView.Columns[0].HeaderText = "Mã hàng";
            DataGridView.Columns[1].HeaderText = "Tên hàng";
            DataGridView.Columns[2].HeaderText = "Loại";
            DataGridView.Columns[3].HeaderText = "Số lượng";
            DataGridView.Columns[4].HeaderText = "Đơn giá nhập";
            DataGridView.Columns[5].HeaderText = "Đơn giá bán";
            DataGridView.Columns[6].HeaderText = "Ảnh";
            DataGridView.Columns[7].HeaderText = "Bảo hành + giảm giá";
            DataGridView.Columns[0].Width = 85;
            DataGridView.Columns[1].Width = 160;
            DataGridView.Columns[2].Width = 50;
            DataGridView.Columns[3].Width = 80;
            DataGridView.Columns[4].Width = 170;
            DataGridView.Columns[5].Width = 170;
            DataGridView.Columns[6].Width = 200;
            DataGridView.Columns[7].Width = 200;
            DataGridView.AllowUserToAddRows = false;
            DataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void DataGridView_Click(object sender, EventArgs e)
        {
            string loai;
            string sql;
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMahang.Focus();
                return;
            }
            if (tblH.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtMahang.Text = DataGridView.CurrentRow.Cells["Mahang"].Value.ToString();
            txtTenhang.Text = DataGridView.CurrentRow.Cells["Tenhang"].Value.ToString();
            loai = DataGridView.CurrentRow.Cells["maLoai"].Value.ToString();
            sql = "select Tenloai from dbo.LOAISANPHAM WHERE maLoai = N'" + loai + "'";
            cboLoaisp.Text = functions.GetFieldValues(sql);
            txtSoluong.Text = DataGridView.CurrentRow.Cells["Soluong"].Value.ToString();
            txtDongianhap.Text = DataGridView.CurrentRow.Cells["Dongianhap"].Value.ToString();
            txtDongiaban.Text = DataGridView.CurrentRow.Cells["Dongiaban"].Value.ToString();
            sql = "SELECT Anh FROM tblHang WHERE Mahang=N'" + txtMahang.Text + "'";
            txtAnh.Text = functions.GetFieldValues(sql);
            picAnh.Image = Image.FromFile(txtAnh.Text);
            sql = "SELECT Ghichu FROM tblHang WHERE Mahang = N'" + txtMahang.Text + "'";
            txtGhichu.Text =functions.GetFieldValues(sql);
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoqua.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoqua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            ResetValues();
            txtMahang.Enabled = true;
            txtMahang.Focus();
            txtSoluong.Enabled = true;
            txtDongianhap.Enabled = true;
            txtDongiaban.Enabled = true;
     
        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnThem.Enabled = true;
            btnBoqua.Enabled = false;
            btnLuu.Enabled = false;
            txtMahang.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMahang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMahang.Focus();
                return;
            }
            if (txtTenhang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenhang.Focus();
                return;
            }
            if (txtAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn ảnh minh hoạ cho hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnOpen.Focus();
                return;
            }
            sql = "SELECT Mahang FROM tblHang WHERE Mahang=N'" + txtMahang.Text.Trim() + "'";
            if (functions.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã tồn tại, bạn phải chọn mã hàng khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMahang.Focus();
                return;
            }
            sql = "INSERT INTO tblHang(Mahang,Tenhang,maLoai,Soluong,Dongianhap, Dongiaban,Anh,Ghichu) VALUES(N'"
                + txtMahang.Text.Trim() + "',N'" + txtTenhang.Text.Trim() +
                "',N'"+cboLoaisp.SelectedValue.ToString()+"'," + txtSoluong.Text.Trim() + "," + txtDongianhap.Text +
                "," + txtDongiaban.Text + ",N'" + txtAnh.Text + "',N'" + txtGhichu.Text.Trim() + "')";

            functions.RunSQL(sql);
            LoadDataGridView();
            //ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnBoqua.Enabled = false;
            btnLuu.Enabled = false;
            txtMahang.Enabled = false;
        }

        private void txtDongiaban_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtDongianhap_TextChanged(object sender, EventArgs e)
        {
            double gn, gb;
            gn = Convert.ToDouble(txtDongianhap.Text);
            gb = gn + (gn / 10);
            txtDongiaban.Text = gb.ToString();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*";
            dlgOpen.FilterIndex = 2;
            dlgOpen.Title = "Chọn ảnh minh hoạ cho sản phẩm";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                picAnh.Image = Image.FromFile(dlgOpen.FileName);
                txtAnh.Text = dlgOpen.FileName;
            }
        }

        private void btnHienthi_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT Mahang, Tenhang,maLoai,Soluong,Dongianhap,Dongiaban,Anh,Ghichu FROM tblHang";
            tblH = functions.GetDataToTable(sql);
            DataGridView.DataSource = tblH;
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            string sql;
            if ((txtMahang.Text == "") && (txtTenhang.Text == "") && (cboLoaisp.Text == ""))
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from tblHang WHERE 1=1";
            if (txtMahang.Text != "")
                sql += " AND Mahang LIKE N'%" + txtMahang.Text + "%'";
            if (cboLoaisp.Text != "")
                sql += "AND maLoai LIKE N'%" + cboLoaisp.SelectedValue + "%'";
            if (txtTenhang.Text != "")
                sql += " AND Tenhang LIKE N'%" + txtTenhang.Text + "%'";
            tblH = functions.GetDataToTable(sql);
            if (tblH.Rows.Count == 0)
                MessageBox.Show("Không có hàng hóa thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tblH.Rows.Count + "  hàng hóa thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DataGridView.DataSource = tblH;
            ResetValues();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMahang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMahang.Focus();
                return;
            }
            if (txtTenhang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenhang.Focus();
                return;
            }
            if (txtAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải ảnh minh hoạ cho hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAnh.Focus();
                return;
            }
            sql = "UPDATE tblHang SET Tenhang=N'" + txtTenhang.Text.Trim().ToString() +
                "',maLoai = N'"+cboLoaisp.SelectedValue.ToString()+"',Soluong=" + txtSoluong.Text +
                ",Anh='" + txtAnh.Text + "',Ghichu=N'" + txtGhichu.Text + "' WHERE Mahang=N'" + txtMahang.Text + "'";
            functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btnBoqua.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblH.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMahang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE tblHang WHERE Mahang=N'" + txtMahang.Text + "'";
                functions.RunSqlDel(sql);
                LoadDataGridView();
                ResetValues();
            }
        }
    }
}
