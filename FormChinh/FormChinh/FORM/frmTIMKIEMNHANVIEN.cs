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
using app = Microsoft.Office.Interop.Excel.Application;

namespace FormChinh.FORM
{
    public partial class frmTIMKIEMNHANVIEN : Form
    {
        public frmTIMKIEMNHANVIEN()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from tblNhanVien where Manhanvien ='" + txtmanv.Text.Trim() + "'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
            sql = "SELECT * from dbo.tblNhanVien where Tennhanvien ='" + txttennv.Text.Trim() + "'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
        }

        private void txtmanv_TextChanged(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from tblNhanVien where Manhanvien like '" + txtmanv.Text.Trim() + "%'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
        }

        private void txttennv_TextChanged(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from tblNhanVien where Tennhanvien like'" + txttennv.Text.Trim() + "%'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
        }

        private void txtmanv_MouseClick(object sender, MouseEventArgs e)
        {
            txtmanv.Clear();
        }

        private void txttennv_MouseClick(object sender, MouseEventArgs e)
        {
            txttennv.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            app obj = new app();
            obj.Application.Workbooks.Add(Type.Missing);
            obj.Columns.ColumnWidth = 25;
            obj.Range["A1:Z300"].Font.Name = "Times new roman"; //Font chữ
            obj.Range["A1:B3"].Font.Size = 10;
            obj.Range["A1:B3"].Font.Bold = true;
            obj.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
            obj.Range["A1:A1"].ColumnWidth = 20;
            obj.Range["B1:B1"].ColumnWidth = 15;
            obj.Range["A1:B1"].MergeCells = true;
            obj.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            obj.Range["A1:A1"].Value = "CÔNG TY MÁY TÍNH CHUNG VÀ QUÝ";
            obj.Range["D1:E1"].MergeCells = true;
            obj.Range["D1:E1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            obj.Range["D1:E1"].Value = "Quảng Bình AND Huế";
            obj.Range["B4:D4"].Font.Size = 16;
            obj.Range["B4:D4"].Font.Bold = true;
            obj.Range["B4:D4"].Font.ColorIndex = 3; //Màu đỏ
            obj.Range["B4:D4"].MergeCells = true;
            obj.Range["B4:D4"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            obj.Range["B4:D4"].Value = "DANH SÁCH NHÂN VIÊN";
            for (int i = 1; i < DataGridView.Columns.Count + 1; i++)
            {
                obj.Cells[6, i] = DataGridView.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < DataGridView.Columns.Count; j++)
                {
                    if (DataGridView.Rows[i].Cells[j].Value != null)
                    {
                        obj.Cells[i + 7, j + 1] = DataGridView.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }
            obj.Range["D20:F20"].MergeCells = true;
            obj.Range["D20:F20"].Font.Italic = true;
            obj.Range["D20:F20"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            //DateTime d = new DateTime();
            obj.Range["D20:F20"].Value = "Quảng Bình, ngày  " + "  " + " tháng  " + "    " + " năm " + "   ";
            //obj.ActiveWorkbook.SaveCopyAs(sql + tentap + ".xlsx");
            //obj.ActiveWorkbook.Saved = true;
            obj.Visible = true;
        }
    }
}
