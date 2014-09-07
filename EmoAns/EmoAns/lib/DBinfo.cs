using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Controls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace EmoAns
{
    class DBinfo
    {
        
        public void GetDBInfo()
        {
            SqlConnection conn = ConnectionHelper.GetConnection();
            conn.Open();
            MessageBox.Show("成功连接数据库");
            SqlDataAdapter adp = new SqlDataAdapter("select * from sysobjects where xtype='U'", conn);
            DataSet ds = new DataSet();
            adp.Fill(ds);


        }



        


        public class ConnectionHelper
        {
            public static SqlConnection GetConnection()
            {
                string connectionStr = "Data Source=.;Initial Catalog=EmoDB;Integrated Security=SSPI";
                SqlConnection conn = new SqlConnection(connectionStr);
                return conn;
            }
        }





    }
}
