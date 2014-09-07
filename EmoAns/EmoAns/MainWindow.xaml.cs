using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.Collections;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Data;
 using System.ComponentModel;
 using System.Drawing;
 using EmoAns;


namespace EmoAns
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
      //选择数据库连接
        //设置界面默认值
        private void DBComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            DBComboBox.Items.Add("EmoDB");
            DBComboBox.SelectedIndex = 0;
            FileNameTextBox.Text = "";
            searchtextbox.Text = "";

        }

        //连接到数据库，选择相应数据库中所有表名显示
        private void ConnButton_Click_1(object sender, RoutedEventArgs e)
        {
            try { 
            
              
            SqlConnection conn =  DBinfo.ConnectionHelper.GetConnection();
            conn.Open();
            MessageBox.Show("成功连接数据库");
            SqlDataAdapter adp = new SqlDataAdapter("select * from sysobjects where xtype='U'", conn);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            TableComboBox.ItemsSource = ds.Tables[0].DefaultView;
            TableComboBox.DisplayMemberPath = "name";
            TableComboBox.SelectedIndex = 0;
            TableComboBoxcopy.ItemsSource = ds.Tables[0].DefaultView;
            TableComboBoxcopy.DisplayMemberPath = "name";
            TableComboBoxcopy.SelectedIndex = 0;
                }
            catch (Exception msg)
            {
                throw new Exception(msg.ToString());  //异常处理  
            }  
            
        }

        
        //选定表名之后把表内容显示在datagrid中
        private void ViewTable_Click(object sender, RoutedEventArgs e)
        {
            String TableName=TableComboBox.Text;
            SqlConnection conn =  DBinfo.ConnectionHelper.GetConnection();
            string CmdString = string.Empty;
            CmdString = "Select * from "+TableName;//如果只显示某几列数据，可以用其他sql语句控制，如select 部门经理 from 部门表
            SqlCommand cmd = new SqlCommand(CmdString, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet(); 
            adp.Fill(dt);
            GTablecontent.ItemsSource= dt.Tables[0].DefaultView;
            GTablecontent.LoadingRow += new EventHandler<DataGridRowEventArgs>(GTablecontent_LoadingRow); //载入计算得出的行数
        }

        public void GTablecontent_LoadingRow(object sender, DataGridRowEventArgs e) //计算总行数
        {
             e.Row.Header = e.Row.GetIndex() + 1;
        }

        

          //0表示编辑状态，1为添加状态。因为后面的增加和编辑都在同一个事件中，所以建一个变量来区分操作    
          //  private void btnAdd_Click(object sender, RoutedEventArgs e)
            //{
              //  int judge = 0; 
                //judge = 1;
                //GTablecontent.CanUserAddRows = true;

    //        }


        //打开文件读取文件名
        private void openSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension  
            dlg.DefaultExt = ".xls"; dlg.Filter = "Text documents (.xls)|*.xls";
            // Display OpenFileDialog by calling ShowDialog method  
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox  
            if (result == true)
            {
                // Open document  
                string filename = dlg.FileName;
                FileNameTextBox.Text = filename;
            }

        }

        private void FileAddBtn_Click(object sender, RoutedEventArgs e)
        {
            string TableName = TableComboBoxcopy.Text.Trim();
            string FileName = FileNameTextBox.Text;
             
            StoreExcel StrExcel = new StoreExcel();
            System.Data.DataTable dt = StrExcel.LoadExcel(FileName); //通过路径获取到的数据  
            //此时我们就可以用这数据进行处理了，比如绑定到显示数据的控件当中去  
              

               switch (TableName) 
               {
                   case "EmoPhrase":
                       foreach (DataRow datarow in dt.Rows)
                       {
                           string sql = "INSERT INTO EmoPhrase ([Phrase],[PhraseType],[ThemeNo],[Theme])" + "VALUES('" + datarow["Phrase"].ToString() + "'" + ",'" + datarow["PhraseType"].ToString() + "'" + ",'" + datarow["ThemeNo"].ToString() + "'" + ",'" + datarow["Theme"].ToString() + "')";

                           SqlConnection conn =  DBinfo.ConnectionHelper.GetConnection();
                           conn.Open();
                               SqlCommand sqlcommand = new SqlCommand(sql, conn);
                               sqlcommand.ExecuteNonQuery();
                               conn.Close();
                        }
                       break;
                   case "StopQ":
                       foreach (DataRow datarow in dt.Rows)
                       {
                           string sql = "INSERT INTO StopQ ([StopPhrase])" + "VALUES('" + datarow["StopPhrase"].ToString() + "')";

                           SqlConnection conn =  DBinfo.ConnectionHelper.GetConnection();
                           conn.Open();
                               SqlCommand sqlcommand = new SqlCommand(sql, conn);
                               sqlcommand.ExecuteNonQuery();
                               conn.Close();
                        }
                       break;


                   case "ReverseList":                         
                       foreach (DataRow datarow in dt.Rows)
                       {
                           string sql = "INSERT INTO ReverseList ([ReversePhrase])" + "VALUES('" + datarow["ReversePhrase"].ToString() + "')";
                           SqlConnection conn =  DBinfo.ConnectionHelper.GetConnection();
                           conn.Open();
                               SqlCommand sqlcommand = new SqlCommand(sql, conn);
                               sqlcommand.ExecuteNonQuery();
                               conn.Close();
                        }
                       break;
                }
               MessageBox.Show("导入成功");
               SqlConnection con = DBinfo.ConnectionHelper.GetConnection();
               con.Open();
              
               String CmdString = "Select * from " + TableName;//如果只显示某几列数据，可以用其他sql语句控制，如select 部门经理 from 部门表
               SqlCommand cmd = new SqlCommand(CmdString, con);
               SqlDataAdapter adp = new SqlDataAdapter(cmd);
               DataSet dt2 = new DataSet();
               adp.Fill(dt2);
               GTablecontent.ItemsSource = dt2.Tables[0].DefaultView;
               GTablecontent.LoadingRow += new EventHandler<DataGridRowEventArgs>(GTablecontent_LoadingRow); //载入计算得出的行数
               }
        

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            String TableName = TableComboBox.Text;
            string SerTex=searchtextbox.Text.Trim();
            string CHeader = (string)GTablecontent.Columns[0].Header;

            try
            {
                SqlConnection conn =  DBinfo.ConnectionHelper.GetConnection();
                string CmdString = string.Empty;
                CmdString = "Select * from " + TableName + " where charindex ( '" + SerTex + "' , " + CHeader + " ) > 0 ";

                SqlCommand cmd = new SqlCommand(CmdString, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                adp.Fill(dt);
                GTablecontent.ItemsSource = dt.Tables[0].DefaultView;
                GTablecontent.LoadingRow += new EventHandler<DataGridRowEventArgs>(GTablecontent_LoadingRow); //载入计算得出
            }
            catch
            {
                throw new Exception("查询发生错误！");
            }
     

            }


        
        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
            string TableName = TableComboBox.Text.Trim();
            

            for (int i = GTablecontent.SelectedItems.Count - 1; i >= 0; i--)
            {

               DataRowView mySelectedElement = (DataRowView) GTablecontent.SelectedItem;
                 string resultcol = mySelectedElement.Row[0].ToString();
                string selectedColumnHeader = (string)GTablecontent.SelectedCells[0].Column.Header;
                     
                        try
                        {
                            string sql = "DELETE FROM " + TableName + " where " +selectedColumnHeader+" = '" + resultcol + "'";
                            SqlConnection conn =  DBinfo.ConnectionHelper.GetConnection();
                            conn.Open();
                            SqlCommand sqlcommand = new SqlCommand(sql, conn);
                            sqlcommand.ExecuteNonQuery();
                            conn.Close();
                            MessageBox.Show("删除成功！");
                            string CmdString = string.Empty;
                            CmdString = "Select * from " + TableName;//如果只显示某几列数据，可以用其他sql语句控制，如select 部门经理 from 部门表
                            SqlCommand cmd = new SqlCommand(CmdString, conn);
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            DataSet dt = new DataSet();
                            adp.Fill(dt);
                            GTablecontent.ItemsSource = dt.Tables[0].DefaultView;
                            GTablecontent.LoadingRow += new EventHandler<DataGridRowEventArgs>(GTablecontent_LoadingRow); //载入计算得出的行数

                        }
                        catch
                        {
                            throw new Exception("删除发生错误！");
                        }
     
            }

           
             
           
        }

        private void DoBtn_Click(object sender, RoutedEventArgs e)
        {
            Alg1 Alg = new Alg1();
            Alg.AnalysisEmo();
        }


       

      





        
        }





    }
