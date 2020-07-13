using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FormChinh.DOT;
using COMExcel = Microsoft.Office.Interop.Excel;
using app = Microsoft.Office.Interop.Excel.Application;

namespace FormChinh.FORM
{
    public partial class frmTIMKIEMKHACHHANG : Form
    {
        public frmTIMKIEMKHACHHANG()
        {
            InitializeComponent();
        }

        private void frmTIMKIEMKHACHHANG_Load(object sender, EventArgs e)
        {
            
        }

        private void txtmakh_TextChanged(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from tblKhach where Makhach like '" + txtmakh.Text.Trim() + "%'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
        }

        private void txttenkh_TextChanged(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from tblKhach where Tenkhach like '" + txttenkh.Text.Trim() + "%'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
        }

        private void txtsdt_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtmakh_MouseClick(object sender, MouseEventArgs e)
        {
            txtmakh.Clear();
        }

        private void txttenkh_MouseClick(object sender, MouseEventArgs e)
        {
            txttenkh.Clear();
        }

        private void txtsdt_MouseClick(object sender, MouseEventArgs e)
        {
            //txtsdt.Clear();
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
            obj.Range["B4:D4"].Value = "DANH SÁCH KHÁCH HÀNG";
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
            obj.Range["D20:F20"].Value = "Quảng Bình, ngày  " + "  " + " tháng  " +"    "+ " năm " +"   ";
            //obj.ActiveWorkbook.SaveCopyAs(sql + tentap + ".xlsx");
            //obj.ActiveWorkbook.Saved = true;
            obj.Visible = true;
        }

        private void txtsdt_TextChanged_1(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from tblKhach where Dienthoai like '(" + txtsdt.Text.Trim() + "%'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
            
        }

        private void txtsdt_MouseClick_1(object sender, MouseEventArgs e)
        {
            txtsdt.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from tblKhach where Makhach ='" + txtmakh.Text.Trim() + "'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
            sql = "SELECT * from tblKhach where Tenkhach ='" + txttenkh.Text.Trim() + "'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
            sql = "SELECT * from tblKhach where Dienthoai ='" + txtsdt.Text.Trim() + "'";
            DataGridView.DataSource = functions.GetDataToTable(sql);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
