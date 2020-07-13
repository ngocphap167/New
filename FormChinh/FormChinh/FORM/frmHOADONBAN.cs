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
    public partial class frmHOADONBAN : Form
    {
        DataTable tblCTHDB;

        public static string usename;

        public frmHOADONBAN()
        {
            InitializeComponent();
        }

        private void frmHOADONBAN_Load(object sender, EventArgs e)
        {
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            btnInhoadon.Enabled = false;
            txtMaHDBan.ReadOnly = true;
            txtTennhanvien.ReadOnly = true;
            txtTenkhach.ReadOnly = true;
            txtDiachi.ReadOnly = true;
            txtDienthoai.ReadOnly = true;
            txtTenhang.ReadOnly = true;
            txtDongiaban.ReadOnly = true;
            txtThanhtien.ReadOnly = true;
            txtTongtien.ReadOnly = true;
            txtGiamgia.Text = "0";
            txtTongtien.Text = "0";
            functions.FillCombo("SELECT Makhach, Tenkhach FROM tblKhach", cboMakhach, "Makhach", "Makhach");
            cboMakhach.SelectedIndex = -1;
            functions.FillCombo("SELECT Manhanvien, Tennhanvien FROM tblNhanvien", cboManhanvien, "Manhanvien", "Tenkhach");
            cboManhanvien.SelectedIndex = -1;
            functions.FillCombo("SELECT Mahang, Tenhang FROM tblHang", cboMahang, "Mahang", "Mahang");
            cboMahang.SelectedIndex = -1;
            //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
            if (txtMaHDBan.Text != "")
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
            sql = "SELECT a.Mahang, b.Tenhang, a.Soluong, b.Dongiaban, a.Giamgia,a.Thanhtien FROM tblChitietHDBan AS a, tblHang AS b WHERE a.MaHDBan = N'" + txtMaHDBan.Text + "' AND a.Mahang=b.Mahang";
            tblCTHDB = functions.GetDataToTable(sql);
            DataGridView.DataSource = tblCTHDB;
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
            str = "SELECT Ngayban FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            txtNgayban.Text = functions.ConvertDateTime(functions.GetFieldValues(str));
            str = "SELECT Manhanvien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            cboManhanvien.Text = functions.GetFieldValues(str);
            str = "SELECT Makhach FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            cboMakhach.Text = functions.GetFieldValues(str);
            str = "SELECT Tongtien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            txtTongtien.Text = functions.GetFieldValues(str);
        }

        private void btnThemmoi_Click(object sender, EventArgs e)
        {
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnInhoadon.Enabled = false;
            btnThemmoi.Enabled = false;
            ResetValues();
            txtMaHDBan.Text = functions.CreateKey("HDB");
            LoadDataGridView();
            string sql;
            sql = "select b.Manhanvien from tblAccount as c, tblNhanVien as b where c.Manhanvien = b.Manhanvien and c.Username = '"+usename+"'";
            cboManhanvien.Text = functions.GetFieldValues(sql);
            LoadDataGridView();
        }
        private void ResetValues()
        {
            txtMaHDBan.Text = "";
            txtNgayban.Text = DateTime.Now.ToShortDateString();
            cboManhanvien.Text = "";
            cboMakhach.Text = "";
            txtTongtien.Text = "0";
            //lblBangchu.Text = "Bằng chữ: ";
            cboMahang.Text = "";
            txtSoluong.Text = "";
            txtGiamgia.Text = "0";
            txtThanhtien.Text = "0";
            
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon, tong, Tongmoi;
            sql = "SELECT MaHDBan FROM tblHDBan WHERE MaHDBan=N'" + txtMaHDBan.Text + "'";
            if (!functions.CheckKey(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HDBan được sinh tự động do đó không có trường hợp trùng khóa
                if (txtNgayban.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập ngày bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNgayban.Focus();
                    return;
                }
                if (cboManhanvien.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboManhanvien.Focus();
                    return;
                }
                if (cboMakhach.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMakhach.Focus();
                    return;
                }
                sql = "INSERT INTO tblHDBan(MaHDBan, Ngayban, Manhanvien, Makhach, Tongtien) VALUES (N'" + txtMaHDBan.Text.Trim() + "','" +
                        functions.ConvertDateTime(txtNgayban.Text.Trim()) + "',N'" + cboManhanvien.SelectedValue + "',N'" +
                        cboMakhach.SelectedValue + "'," + txtTongtien.Text + ")";
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
            sql = "SELECT Mahang FROM tblChitietHDBan WHERE MaHang=N'" + cboMahang.SelectedValue + "' AND MaHDBan = N'" + txtMaHDBan.Text.Trim() + "'";
            if (functions.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cboMahang.Focus();
                return;
            }
            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(functions.GetFieldValues("SELECT Soluong FROM tblHang WHERE Mahang = N'" + cboMahang.SelectedValue + "'"));
            if (Convert.ToDouble(txtSoluong.Text) > sl)
            {
                MessageBox.Show("Số lượng mặt hàng này chỉ còn " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoluong.Text = "";
                txtSoluong.Focus();
                return;
            }
            sql = "INSERT INTO tblChitietHDBan(MaHDBan,Mahang,Soluong,Dongia, Giamgia,Thanhtien) VALUES(N'" + txtMaHDBan.Text.Trim() + "',N'" + cboMahang.SelectedValue + "'," + txtSoluong.Text + "," + txtDongiaban.Text + "," + txtGiamgia.Text + "," + txtThanhtien.Text + ")";
            functions.RunSQL(sql);
            LoadDataGridView();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl - Convert.ToDouble(txtSoluong.Text);
            sql = "UPDATE tblHang SET Soluong =" + SLcon + " WHERE Mahang= N'" + cboMahang.SelectedValue + "'";
            functions.RunSQL(sql);
            // Cập nhật lại tổng tiền cho hóa đơn bán
            tong = Convert.ToDouble(functions.GetFieldValues("SELECT Tongtien FROM tblHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'"));
            Tongmoi = tong + Convert.ToDouble(txtThanhtien.Text);
            sql = "UPDATE tblHDBan SET Tongtien =" + Tongmoi + " WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
            functions.RunSQL(sql);
            txtTongtien.Text = Tongmoi.ToString();
            //lblBangchu.Text = "Bằng chữ: " + Functions.ChuyenSoSangChu(Tongmoi.ToString());
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

        private void cboMakhach_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMakhach.Text == "")
            {
                txtTenkhach.Text = "";
                txtDiachi.Text = "";
                txtDienthoai.Text = "";
            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select Tenkhach from tblKhach where Makhach = N'" + cboMakhach.SelectedValue + "'";
            txtTenkhach.Text = functions.GetFieldValues(str);
            str = "Select Diachi from tblKhach where Makhach = N'" + cboMakhach.SelectedValue + "'";
            txtDiachi.Text = functions.GetFieldValues(str);
            str = "Select Dienthoai from tblKhach where Makhach= N'" + cboMakhach.SelectedValue + "'";
            txtDienthoai.Text = functions.GetFieldValues(str);
        }

        private void cboMahang_TextChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMahang.Text == "")
            {
                txtTenhang.Text = "";
                txtDongiaban.Text = "";
            }
            // Khi chọn mã hàng thì các thông tin về hàng hiện ra
            str = "SELECT Tenhang FROM tblHang WHERE Mahang =N'" + cboMahang.SelectedValue + "'";
            txtTenhang.Text = functions.GetFieldValues(str);
            str = "SELECT Dongiaban FROM tblHang WHERE Mahang =N'" + cboMahang.SelectedValue + "'";
            txtDongiaban.Text = functions.GetFieldValues(str);
            str = "SELECT Soluong FROM tblHang WHERE Mahang =N'" + cboMahang.SelectedValue + "'";
            txtTon.Text = functions.GetFieldValues(str);
            str = "SELECT Ghichu FROM tblHang WHERE Mahang =N'" + cboMahang.SelectedValue + "'";
            txtBH.Text = functions.GetFieldValues(str);
        }

        private void txtSoluong_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoluong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoluong.Text);
            if (txtGiamgia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamgia.Text);
            if (txtDongiaban.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDongiaban.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhtien.Text = tt.ToString();
        }

        private void txtGiamgia_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi giảm giá thì tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoluong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoluong.Text);
            if (txtGiamgia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamgia.Text);
            if (txtDongiaban.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDongiaban.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhtien.Text = tt.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT Mahang,Soluong FROM tblChitietHDBan WHERE MaHDBan = N'" + txtMaHDBan.Text + "'";
                DataTable tblHang = functions.GetDataToTable(sql);
                for (int hang = 0; hang <= tblHang.Rows.Count - 1; hang++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(functions.GetFieldValues("SELECT Soluong FROM tblHang WHERE Mahang = N'" + tblHang.Rows[hang][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(tblHang.Rows[hang][1].ToString());
                    slcon = sl + slxoa;
                    sql = "UPDATE tblHang SET Soluong =" + slcon + " WHERE Mahang= N'" + tblHang.Rows[hang][0].ToString() + "'";
                    functions.RunSQL(sql);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE tblChitietHDBan WHERE MaHDBan=N'" + txtMaHDBan.Text + "'";
                functions.RunSqlDel(sql);

                //Xóa hóa đơn
                sql = "DELETE tblHDBan WHERE MaHDBan=N'" + txtMaHDBan.Text + "'";
                functions.RunSqlDel(sql);
                ResetValues();
                LoadDataGridView();
                btnXoa.Enabled = false;
                btnInhoadon.Enabled = false;
            }
        }

        private void btnNgay_Click(object sender, EventArgs e)
        {
            txtNgayban.Text = DateTime.Now.ToString();
        }

        private void frmHOADONBAN_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetValues();
        }

        private void txtSoluong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;
        }

        private void txtGiamgia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;
        }

        private void btnInhoadon_Click(object sender, EventArgs e)
        {
            // Khởi động chương trình Excel
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
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
            exRange.Range["C2:E2"].Value = "HÓA ĐƠN BÁN";
            // Biểu diễn thông tin chung của hóa đơn bán
            sql = "SELECT a.MaHDBan, a.Ngayban, a.Tongtien, b.Tenkhach, b.Diachi, b.Dienthoai, c.Tennhanvien FROM tblHDBan AS a, tblKhach AS b, tblNhanvien AS c WHERE a.MaHDBan = N'" + txtMaHDBan.Text + "' AND a.Makhach = b.Makhach AND a.Manhanvien = c.Manhanvien";
            tblThongtinHD = functions.GetDataToTable(sql);
            exRange.Range["B6:C9"].Font.Size = 12;
            exRange.Range["B6:B6"].Value = "Mã hóa đơn:";
            exRange.Range["C6:E6"].MergeCells = true;
            exRange.Range["C6:E6"].Value = tblThongtinHD.Rows[0][0].ToString();
            exRange.Range["B7:B7"].Value = "Khách hàng:";
            exRange.Range["C7:E7"].MergeCells = true;
            exRange.Range["C7:E7"].Value = tblThongtinHD.Rows[0][3].ToString();
            exRange.Range["B8:B8"].Value = "Địa chỉ:";
            exRange.Range["C8:E8"].MergeCells = true;
            exRange.Range["C8:E8"].Value = tblThongtinHD.Rows[0][4].ToString();
            exRange.Range["B9:B9"].Value = "Điện thoại:";
            exRange.Range["C9:E9"].MergeCells = true;
            exRange.Range["C9:E9"].Value = tblThongtinHD.Rows[0][5].ToString();
            //Lấy thông tin các mặt hàng
            sql = "SELECT b.Tenhang, a.Soluong, b.Dongiaban, a.Giamgia, a.Thanhtien " +
                  "FROM tblChitietHDBan AS a , tblHang AS b WHERE a.MaHDBan = N'" +
                  txtMaHDBan.Text + "' AND a.Mahang = b.Mahang";
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
            exRange.Range["A6:C6"].Value = tblThongtinHD.Rows[0][6];
            exSheet.Name = "Hóa đơn nhập";
            exApp.Visible = true;
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (cboMaHDBan.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHDBan.Focus();
                return;
            }
            txtMaHDBan.Text = cboMaHDBan.Text;
            LoadInfoHoadon();
            LoadDataGridView();
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            btnInhoadon.Enabled = true;
            cboMaHDBan.SelectedIndex = -1;
        }

        private void cboMaHDBan_DropDown(object sender, EventArgs e)
        {
            functions.FillCombo("SELECT MaHDBan FROM tblHDBan", cboMaHDBan, "MaHDBan", "MaHDBan");
            cboMaHDBan.SelectedIndex = -1;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnThemmoi.Enabled = true;
            ResetValues();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FORM.frmKHACHHANG frm = new frmKHACHHANG();
            this.Hide();
            frm.ShowDialog();
            this.Show();
            
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
