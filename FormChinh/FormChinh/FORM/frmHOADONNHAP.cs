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
using COMExcel = Microsoft.Office.Interop.Excel;

namespace FormChinh.FORM
{
    public partial class frmHOADONNHAP : Form
    {
        public static string usename;
        DataTable tblCTHDN;
        public frmHOADONNHAP()
        {
            InitializeComponent();
        }

        private void frmHOADONNHAP_Load(object sender, EventArgs e)
        {
            btnInhoadon.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            btnInhoadon.Enabled = false;
            txtMaHDNhap.ReadOnly = true;
            txtTennhanvien.ReadOnly = true;
            txtTenncc.ReadOnly = true;
            txtThanhtien.ReadOnly = true;
            txtTongtien.ReadOnly = true;
            txtGiamgia.Text = "0";
            txtTongtien.Text = "0";
            functions.FillCombo("SELECT maNCC, TenNCC FROM NHACUNGCAP", cboMancc, "maNCC", "maNCC");
            cboMancc.SelectedIndex = -1;
            functions.FillCombo("SELECT Manhanvien, Tennhanvien FROM tblNhanvien", cboManhanvien, "Manhanvien", "Tenkhach");
            cboManhanvien.SelectedIndex = -1;
            functions.FillCombo("SELECT Mahang, Tenhang FROM tblHang", cboMahang, "Mahang", "Mahang");
            cboMahang.SelectedIndex = -1;
            if (txtMaHDNhap.Text != "")
            {
                LoadInfoHoadon();
                btnXoa.Enabled = true;
                btnInhoadon.Enabled = true;
            }
            LoadDataGridView();
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.Mahang, b.Tenhang, a.Soluong, b.Dongiaban, a.Giamgia,a.Thanhtien FROM tblChitietHDNhap AS a, tblHang AS b WHERE a.MaHDNhap = N'" + txtMaHDNhap.Text + "' AND a.Mahang=b.Mahang";
            tblCTHDN = functions.GetDataToTable(sql);
            DataGridView.DataSource = tblCTHDN;
            DataGridView.Columns[0].HeaderText = "Mã hàng";
            DataGridView.Columns[1].HeaderText = "Tên hàng";
            DataGridView.Columns[2].HeaderText = "Số lượng";
            DataGridView.Columns[3].HeaderText = "Đơn giá";
            DataGridView.Columns[4].HeaderText = "Giảm giá %";
            DataGridView.Columns[5].HeaderText = "Thành tiền";
            DataGridView.Columns[0].Width = 80;
            DataGridView.Columns[1].Width = 130;
            DataGridView.Columns[2].Width = 80;
            DataGridView.Columns[3].Width = 90;
            DataGridView.Columns[4].Width = 90;
            DataGridView.Columns[5].Width = 90;
            DataGridView.AllowUserToAddRows = false;
            DataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        private void LoadInfoHoadon()
        {
            string str;
            str = "SELECT Ngaynhap FROM tblHDNhap WHERE MaHDNhap = N'" + txtMaHDNhap.Text + "'";
            txtNgaynhap.Text = functions.ConvertDateTime(functions.GetFieldValues(str));
            str = "SELECT Manhanvien FROM tblHDNhap WHERE MaHDNhap = N'" + txtMaHDNhap.Text + "'";
            cboManhanvien.Text = functions.GetFieldValues(str);
            str = "SELECT maNCC FROM tblHDNhap WHERE MaHDNhap = N'" + txtMaHDNhap.Text + "'";
            cboMancc.Text = functions.GetFieldValues(str);
            str = "SELECT Tongtien FROM tblHDNhap WHERE MaHDNhap = N'" + txtMaHDNhap.Text + "'";
            txtTongtien.Text = functions.GetFieldValues(str);
            str = "SELECT MaHang FROM tblHDNhap,tblChitietHDNhap WHERE tblHDNhap.MaHDNhap = tblChitietHDNhap.MaHDNhap AND tblHDNhap.MaHDNhap = N'" + txtMaHDNhap.Text + "'";
            cboMahang.Text = functions.GetFieldValues(str);
            str = "SELECT Soluong FROM tblHDNhap,tblChitietHDNhap WHERE tblHDNhap.MaHDNhap = tblChitietHDNhap.MaHDNhap AND tblHDNhap.MaHDNhap = N'" + txtMaHDNhap.Text + "'";
            txtSoluong.Text = functions.GetFieldValues(str);
        }

        private void btnThemmoi_Click(object sender, EventArgs e)
        {
            string sql;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnInhoadon.Enabled = false;
            btnThemmoi.Enabled = false;
            ResetValues();
            txtMaHDNhap.Text = functions.CreateKey("HDN");
            LoadDataGridView();
            sql = "select b.Manhanvien from tblAccount as c, tblNhanVien as b where c.Manhanvien = b.Manhanvien and c.Username = '" + usename + "'";
            cboManhanvien.Text = functions.GetFieldValues(sql);
            btnInhoadon.Enabled = false;
        }
        private void ResetValues()
        {
            txtMaHDNhap.Text = "";
            txtNgaynhap.Text = DateTime.Now.ToShortDateString();
            cboManhanvien.Text = "";
            cboMancc.Text = "";
            txtTongtien.Text = "0";
            txtTennhanvien.Clear();
            txtTenncc.Text = "";
            //lblBangchu.Text = "Bằng chữ: ";
            cboMahang.Text = "";
            txtTenhang.Clear();
            txtSoluong.Text = "";
            txtBH.Clear();
            txtDongianhap.Clear();
            txtGiamgia.Text = "0";
            txtThanhtien.Text = "0";
            txtTongtien.Clear();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon, tong, Tongmoi;
            sql = "SELECT MaHDNhap FROM tblChiTietHDNhap WHERE MaHDNhap=N'" + txtMaHDNhap.Text + "'";
            if(!functions.CheckKey(sql))
            {
                if (txtNgaynhap.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập ngày nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNgaynhap.Focus();
                    return;
                }
                if (cboManhanvien.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboManhanvien.Focus();
                    return;
                }
                if (cboMancc.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMancc.Focus();
                    return;
                }
                sql = "INSERT INTO tblHDNhap VALUES ('"+txtMaHDNhap.Text.Trim()+"','"+cboManhanvien.SelectedValue +"','"+functions.ConvertDateTime(txtNgaynhap.Text.Trim())+"','"+cboMancc.SelectedValue+"','"+txtTongtien.Text.Trim()+"')";
                functions.RunSQL(sql);
            }
            // Lưu thông tin của các mặt hàng
            if (cboMahang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMahang.Focus();
                return;
            }
            if ((txtSoluong.Text.Trim().Length == 0) || (txtSoluong.Text == "0"))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoluong.Text = "";
                txtSoluong.Focus();
                return;
            }
            if (txtGiamgia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập giảm giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGiamgia.Focus();
                return;
            }
            sql = "SELECT Mahang FROM tblChiTietHDNhap WHERE MaHang=N'" + cboMahang.SelectedValue + "' AND MaHDNhap = N'" + txtMaHDNhap.Text.Trim() + "'";
            if (functions.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cboMahang.Focus();
                return;
            }
            sl = Convert.ToDouble(functions.GetFieldValues("SELECT Soluong FROM tblHang WHERE Mahang = N'" + cboMahang.SelectedValue + "'"));
            sql = "INSERT INTO tblChiTietHDNhap VALUES(N'" + txtMaHDNhap.Text.Trim() + "',N'" + cboMahang.SelectedValue + "'," + txtSoluong.Text + "," + txtDongianhap.Text + "," + txtThanhtien.Text + "," + txtGiamgia.Text + ")";
            functions.RunSQL(sql);
            LoadDataGridView();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl + Convert.ToDouble(txtSoluong.Text);
            sql = "UPDATE tblHang SET Soluong =" + SLcon + " WHERE Mahang= N'" + cboMahang.SelectedValue + "'";
            functions.RunSQL(sql);
            // Cập nhật lại tổng tiền cho hóa đơn nhập
            tong = Convert.ToDouble(functions.GetFieldValues("SELECT Tongtien FROM tblHDNhap WHERE MaHDNhap = N'" + txtMaHDNhap.Text + "'"));
            Tongmoi = tong + Convert.ToDouble(txtThanhtien.Text);
            sql = "UPDATE tblHDNhap SET Tongtien =" + Tongmoi + " WHERE MaHDNhap = N'" + txtMaHDNhap.Text + "'";
            functions.RunSQL(sql);
            txtTongtien.Text = Tongmoi.ToString();
            ResetValuesHang();
            btnXoa.Enabled = true;
            btnThemmoi.Enabled = true;
            btnInhoadon.Enabled = true;
        }
        private void ResetValuesHang()
        {
            cboMahang.Text = "";
            txtSoluong.Text = "";
            txtGiamgia.Text = "0";
            txtThanhtien.Text = "0";
        }

        private void cboManhanvien_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (cboManhanvien.Text == "")
                txtTennhanvien.Text = "";
            // Khi chọn Mã nhân viên thì tên nhân viên tự động hiện ra
            str = "Select Tennhanvien from tblNhanvien where Manhanvien =N'" + cboManhanvien.SelectedValue + "'";
            txtTennhanvien.Text = functions.GetFieldValues(str);
        }

        private void cboMancc_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMancc.Text == "")
                txtTenncc.Text = "";
            // Khi chọn Mã nahf cung cấp thì tên nhân viên tự động hiện ra
            str = "Select TenNCC from NHACUNGCAP where maNCC =N'" + cboMancc.SelectedValue + "'";
            txtTenncc.Text = functions.GetFieldValues(str);
        }

        private void txtSoluong_TextChanged(object sender, EventArgs e)
        {
            double tt, sl, dg, gg;
            if (txtSoluong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoluong.Text);
            if (txtGiamgia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamgia.Text);
            if (txtDongianhap.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDongianhap.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhtien.Text = tt.ToString();
        }

        private void btnNgay_Click(object sender, EventArgs e)
        {
            txtNgaynhap.Text = DateTime.Now.ToString();
        }

        private void cboMahang_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMahang.Text == "")
            {
                txtTenhang.Text = "";
                txtDongianhap.Text = "";
            }
            // Khi chọn mã hàng thì các thông tin về hàng hiện ra
            str = "SELECT Tenhang FROM tblHang WHERE Mahang =N'" + cboMahang.SelectedValue + "'";
            txtTenhang.Text = functions.GetFieldValues(str);
            str = "SELECT Dongianhap FROM tblHang WHERE Mahang =N'" + cboMahang.SelectedValue + "'";
            txtDongianhap.Text = functions.GetFieldValues(str);
            //str = "SELECT Soluong FROM tblHang WHERE Mahang =N'" + cboMahang.SelectedValue + "'";
            //txtSoluong.Text = functions.GetFieldValues(str);
            str = "SELECT Ghichu FROM tblHang WHERE Mahang =N'" + cboMahang.SelectedValue + "'";
            txtBH.Text = functions.GetFieldValues(str);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT Mahang,Soluong FROM tblChitietHDNhap WHERE MaHDNhap = N'" + txtMaHDNhap.Text + "'";
                DataTable tblHang = functions.GetDataToTable(sql);
                for (int hang = 0; hang <= tblHang.Rows.Count - 1; hang++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(functions.GetFieldValues("SELECT Soluong FROM tblHang WHERE Mahang = N'" + tblHang.Rows[hang][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(tblHang.Rows[hang][1].ToString());
                    slcon = sl - slxoa;
                    sql = "UPDATE tblHang SET Soluong =" + slcon + " WHERE Mahang= N'" + tblHang.Rows[hang][0].ToString() + "'";
                    functions.RunSQL(sql);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE tblChitietHDNhap WHERE MaHDNhap=N'" + txtMaHDNhap.Text + "'";
                functions.RunSqlDel(sql);

                //Xóa hóa đơn
                sql = "DELETE tblHDNhap WHERE MaHDNhap=N'" + txtMaHDNhap.Text + "'";
                functions.RunSqlDel(sql);
                ResetValues();
                LoadDataGridView();
                btnXoa.Enabled = false;
                btnInhoadon.Enabled = false;
            }
        }

        private void btnInhoadon_Click(object sender, EventArgs e)
        {
            // Khởi động chương trình Excel
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet0
            COMExcel.Range exRange;
            string sql;
            int hang = 0, cot = 0;
            DataTable tblThongtinHD, tblThongtinHang;
            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            // Định dạng chung
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:Z300"].Font.Name = "Times new roman"; //Font chữ
            exRange.Range["A1:B3"].Font.Size = 10;
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
            exRange.Range["A1:A1"].ColumnWidth = 7;
            exRange.Range["B1:B1"].ColumnWidth = 15;
            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "CÔNG TY Điện Thoại";
            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "TPHCM";
            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại: 1800.100 có";
            exRange.Range["C2:E2"].Font.Size = 16;
            exRange.Range["C2:E2"].Font.Bold = true;
            exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C2:E2"].MergeCells = true;
            exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:E2"].Value = "HÓA ĐƠN NHẬP";
            // Biểu diễn thông tin chung của hóa đơn bán
            sql = "SELECT a.MaHDNhap, a.Ngaynhap, a.Tongtien, b.TenNCC, c.Tennhanvien FROM tblHDNhap AS a, NHACUNGCAP AS b, tblNhanvien AS c WHERE a.MaHDNhap = N'" + txtMaHDNhap.Text + "' AND a.maNCC = b.maNCC AND a.Manhanvien = c.Manhanvien";
            tblThongtinHD = functions.GetDataToTable(sql);
            exRange.Range["B6:C7"].Font.Size = 12;
            exRange.Range["B6:B6"].Value = "Mã hóa đơn:";
            exRange.Range["C6:E6"].MergeCells = true;
            exRange.Range["C6:E6"].Value = tblThongtinHD.Rows[0][0].ToString();
            exRange.Range["B7:B7"].Value = "Nhà cung cấp:";
            exRange.Range["C7:E7"].MergeCells = true;
            exRange.Range["C7:E7"].Value = tblThongtinHD.Rows[0][3].ToString();
            //Lấy thông tin các mặt hàng
            sql = "SELECT b.Tenhang, a.Soluong, b.DongiaNhap, a.Giamgia, a.Thanhtien " +
                  "FROM tblChitietHDNhap AS a , tblHang AS b WHERE a.MaHDNhap = N'" +
                  txtMaHDNhap.Text + "' AND a.Mahang = b.Mahang";
            tblThongtinHang = functions.GetDataToTable(sql);
            //Tạo dòng tiêu đề bảng
            exRange.Range["A11:F11"].Font.Bold = true;
            exRange.Range["A11:F11"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C11:F11"].ColumnWidth = 12;
            exRange.Range["A11:A11"].Value = "STT";
            exRange.Range["B11:B11"].Value = "Tên hàng";
            exRange.Range["C11:C11"].Value = "Số lượng";
            exRange.Range["D11:D11"].Value = "Đơn giá";
            exRange.Range["E11:E11"].Value = "Giảm giá";
            exRange.Range["F11:F11"].Value = "Thành tiền";
            for (hang = 0; hang < tblThongtinHang.Rows.Count; hang++)
            {
                //Điền số thứ tự vào cột 1 từ dòng 12
                exSheet.Cells[1][hang + 12] = hang + 1;
                for (cot = 0; cot < tblThongtinHang.Columns.Count; cot++)
                //Điền thông tin hàng từ cột thứ 2, dòng 12
                {
                    exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString();
                    if (cot == 3) exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString() + "%";
                }
            }
            exRange = exSheet.Cells[cot][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = "Tổng tiền:";
            exRange = exSheet.Cells[cot + 1][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = tblThongtinHD.Rows[0][2].ToString();
            exRange = exSheet.Cells[1][hang + 15]; //Ô A1 
            exRange.Range["A1:F1"].MergeCells = true;
            exRange.Range["A1:F1"].Font.Bold = true;
            exRange.Range["A1:F1"].Font.Italic = true;
            exRange.Range["A1:F1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignRight;
            // exRange.Range["A1:F1"].Value = "Bằng chữ: " + Functions.ChuyenSoSangChu(tblThongtinHD.Rows[0][2].ToString());
            exRange = exSheet.Cells[4][hang + 17]; //Ô A1 
            exRange.Range["A1:C1"].MergeCells = true;
            exRange.Range["A1:C1"].Font.Italic = true;
            exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            DateTime d = Convert.ToDateTime(tblThongtinHD.Rows[0][1]);
            exRange.Range["A1:C1"].Value = "TPHCM, ngày " + d.Day + " tháng " + d.Month + " năm " + d.Year;
            exRange.Range["A2:C2"].MergeCells = true;
            exRange.Range["A2:C2"].Font.Italic = true;
            exRange.Range["A2:C2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:C2"].Value = "Nhân viên bán hàng";
            exRange.Range["A6:C6"].MergeCells = true;
            exRange.Range["A6:C6"].Font.Italic = true;
            exRange.Range["A6:C6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A6:C6"].Value = tblThongtinHD.Rows[0][4];
            exSheet.Name = "Hóa đơn nhập";
            exApp.Visible = true;
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (cboMaHDNhap.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHDNhap.Focus();
                return;
            }
            txtMaHDNhap.Text = cboMaHDNhap.Text;
            LoadInfoHoadon();
            LoadDataGridView();
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            btnInhoadon.Enabled = true;
            cboMaHDNhap.SelectedIndex = -1;
        }

        private void cboMaHDNhap_DropDown(object sender, EventArgs e)
        {
            functions.FillCombo("SELECT MaHDNhap FROM tblHDNhap", cboMaHDNhap, "MaHDNhap", "MaHDNhap");
            cboMaHDNhap.SelectedIndex = -1;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnThemmoi.Enabled = true;
            ResetValues();
            btnLuu.Enabled = false;
            btnThoat.Enabled = true;
            btnXoa.Enabled = true;
            btnInhoadon.Enabled = true;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
