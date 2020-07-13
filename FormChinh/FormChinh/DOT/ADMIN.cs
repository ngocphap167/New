using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace FormChinh.DOT
{
    public class ADMIN
    {

        public static int Type;
        public static string tennguoidung;
        public static string ConnectString = @"Data Source=DESKTOP-AAEPA3F\SQLEXPRESS;Initial Catalog=DOANKETTHUCMON;Integrated Security=True";
        public static SqlConnection con;
        
    }
}
