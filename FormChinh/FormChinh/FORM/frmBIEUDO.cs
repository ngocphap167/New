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
    public partial class frmBIEUDO : Form
    {
        DataTable bd;
        public frmBIEUDO()
        {
            InitializeComponent();
        }

        private void frmBIEUDO_Load(object sender, EventArgs e)
        {
            fillChart();
            napvaolistview();
            anh();
            
        }
        private void fillChart()
        {
            string sql;
            sql = "select MONTH(Ngayban),sum(Soluong) from tblHDBan,tblChiTietHDBan where tblHDBan.MaHDBan = tblChiTietHDBan.MaHDBan group by MONTH(Ngayban)";
            bd = functions.GetDataToTable(sql);
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "tháng";
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "Soluong";
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            for (int i = 0; i < bd.Rows.Count; i++)
            {
                chart1.Series["abc"].Points.AddXY(bd.Rows[i][0], bd.Rows[i][1]);
            }
        }
        private void napvaolistview() 
        {
            listView1.Items.Clear();
            string sql;
            sql = "select MONTH(Ngayban),COUNT(tblHDBan.MaHDBan),COUNT(Mahang),sum(Soluong),SUM(Tongtien) from tblHDBan,tblChiTietHDBan where tblHDBan.MaHDBan = tblChiTietHDBan.MaHDBan group by MONTH(Ngayban)";
            DataTable nodecha = new DataTable();
            nodecha = functions.GetDataToTable(sql);

            foreach (DataRow dwr in nodecha.Rows)
            {
                ListViewItem lvw = new ListViewItem();
                lvw.Text = dwr[0].ToString();
                lvw.SubItems.Add(dwr[1].ToString());
                lvw.SubItems.Add(dwr[2].ToString());
                lvw.SubItems.Add(dwr[3].ToString());
                lvw.SubItems.Add(dwr[4].ToString());
                listView1.Items.Add(lvw);
            }
            functions.GetFieldValues(sql);
        }
        private void anh()
        {
            string sql = "SELECT Anh,SUM(dh.Soluong) FROM (tblHang as nv INNER JOIN tblChiTietHDBan as dh ON nv.Mahang = dh.Mahang) INNER JOIN tblHDBan as ct ON ct.MaHDBan=dh.MaHDBan GROUP BY Anh having SUM(dh.Soluong) >= all(SELECT SUM(h.Soluong) FROM (tblHang as v INNER JOIN tblChiTietHDBan as h ON v.Mahang = h.Mahang) INNER JOIN tblHDBan as t ON t.MaHDBan=h.MaHDBan GROUP BY Anh)";
            txtAnh.Text = functions.GetFieldValues(sql);
            picAnh.Image = Image.FromFile(txtAnh.Text);
            string sql2 = "SELECT SUM(dh.Soluong) FROM (tblHang as nv INNER JOIN tblChiTietHDBan as dh ON nv.Mahang = dh.Mahang) INNER JOIN tblHDBan as ct ON ct.MaHDBan=dh.MaHDBan GROUP BY Anh having SUM(dh.Soluong) >= all(SELECT SUM(h.Soluong) FROM (tblHang as v INNER JOIN tblChiTietHDBan as h ON v.Mahang = h.Mahang) INNER JOIN tblHDBan as t ON t.MaHDBan=h.MaHDBan GROUP BY Anh)";
            label4.Text = functions.GetFieldValues(sql2);
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
