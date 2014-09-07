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
            try
            {


                SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
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
            String TableName = TableComboBox.Text;
            SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
            string CmdString = string.Empty;
            CmdString = "Select * from " + TableName;//如果只显示某几列数据，可以用其他sql语句控制，如select 部门经理 from 部门表
            SqlCommand cmd = new SqlCommand(CmdString, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            adp.Fill(dt);
            GTablecontent.ItemsSource = dt.Tables[0].DefaultView;
            GTablecontent.LoadingRow += new EventHandler<DataGridRowEventArgs>(GTablecontent_LoadingRow); //载入计算得出的行数
        }

        public void GTablecontent_LoadingRow(object sender, DataGridRowEventArgs e) //计算总行数
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }



        

        //打开文件读取文件名
        private void openSourceBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension  
            dlg.DefaultExt = ".csv"; dlg.Filter = "Text documents (.csv)|*.csv";
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
            string FileName = FileNameTextBox.Text.ToString().Trim();

            StoreExcel StrExcel = new StoreExcel();
            System.Data.DataTable dt = StrExcel.LoadExcel(FileName); //通过路径获取到的数据  
            //此时我们就可以用这数据进行处理了，比如绑定到显示数据的控件当中去  


            switch (TableName)
            {
                case "EmoPhrase":
                    foreach (DataRow datarow in dt.Rows)
                    {
                        string sql = "INSERT INTO EmoPhrase ([Phrase],[PhraseType],[ThemeNo],[Theme])" + "VALUES('" + datarow["Phrase"].ToString() + "'" + ",'" + datarow["PhraseType"].ToString() + "'" + ",'" + datarow["ThemeNo"].ToString() + "'" + ",'" + datarow["Theme"].ToString() + "')";

                        SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
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

                        SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
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
                        SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
                        conn.Open();
                        SqlCommand sqlcommand = new SqlCommand(sql, conn);
                        sqlcommand.ExecuteNonQuery();
                        conn.Close();
                    }
                    break;
                
                case "NegativeList":
                    foreach (DataRow datarow in dt.Rows)
                    {
                        string sql = "INSERT INTO NegativeList ([NegativePhrase])" + "VALUES('" + datarow["NegativePhrase"].ToString() + "')";
                        SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
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
            string SerTex = searchtextbox.Text.Trim();
            string CHeader = (string)GTablecontent.Columns[0].Header;

            try
            {
                SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
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

                DataRowView mySelectedElement = (DataRowView)GTablecontent.SelectedItem;
                string resultcol = mySelectedElement.Row[0].ToString();
                string selectedColumnHeader = (string)GTablecontent.SelectedCells[0].Column.Header;

                try
                {
                    string sql = "DELETE FROM " + TableName + " where " + selectedColumnHeader + " = '" + resultcol + "'";
                    SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
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

        

        private void ImportSource_Click(object sender, RoutedEventArgs e)
        {

                DataImporting dataimport = new DataImporting();
                DataTable source = dataimport.SourceReady(sourcetext.Text.ToString());
                DGSource.ItemsSource = source.DefaultView;

                
            

        }

        private void DoBtn_Click(object sender, RoutedEventArgs e)
        {
            DataImporting dataimport = new DataImporting();
            
            DataTable source = dataimport.SourceReady(sourcetext.Text.ToString());

             
            string AsName = AspectCombo.Text.ToString();

            if (AsName != null)
            {

                for (int i = source.Rows.Count - 1; i >= 0; i--)
                {
                    string sourcep = source.Rows[i][0].ToString();

                    if (!sourcep.Contains(AsName))
                    {
                        source.Rows.RemoveAt(i);
                    }
                }

            }



            DataConfig dcc = new DataConfig(source);

            ChartDataItems resultsready = dcc.ResultReady();
                         
            DataTable Results = resultsready.ValueToGrid(resultsready);

            DGSource.ItemsSource = Results.DefaultView;

            DGSource.LoadingRow += DGSource_LoadingRow;

            Statistic ST = new Statistic(Results);

            ChartDataItems emotionResult = ST.StasticCount();

            PieChart pie = new PieChart(150, emotionResult);

            emot4ionPie.Child = pie.PieDrawing();

           
             
        }


        public void DGSource_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;

            try
            {
                int date = Convert.ToInt32((e.Row.Item as System.Data.DataRowView).Row[1]);

                if (date > 0)
                {
                    e.Row.Background = Brushes.Green;
                }

                else if (date < 0)
                {
                    e.Row.Background = Brushes.Red;
                }

                else e.Row.Background = Brushes.White;
            }
            catch { }
        }
        



        private void opensourcebtn_Click_1(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension  
            dlg.DefaultExt = ".csv"; dlg.Filter = "Text documents (.csv)|*.csv";
            // Display OpenFileDialog by calling ShowDialog method  
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox  
            if (result == true)
            {
                // Open document  
                string filename = dlg.FileName;
                sourcetext.Text = filename;
            }

        }

        private void OpenAlg_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension  
            dlg.DefaultExt = ".cs"; dlg.Filter = "Text documents (.cs)|*.cs";
            // Display OpenFileDialog by calling ShowDialog method  
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox  
            if (result == true)
            {
                // Open document  
                string filename = dlg.FileName;
                Algurl.Text = filename;
            }

        }

        private void AddAlgBtn_Click(object sender, RoutedEventArgs e)
        {
            int rowcount=AlgSource.Items.Count;
            string sql = "INSERT INTO Algorithm VALUES('"+rowcount+"','" + AlgName.Text.ToString() + "', '" + AlgDes.Text.ToString() + "', '" + Algurl.Text.ToString() + "')";
            SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
            conn.Open();
            SqlCommand sqlcommand = new SqlCommand(sql, conn);
            sqlcommand.ExecuteNonQuery();

            String TableName = "Algorithm";
             
            string CmdString = string.Empty;
            CmdString = "Select * from " + TableName;//如果只显示某几列数据，可以用其他sql语句控制，如select 部门经理 from 部门表
            SqlCommand cmd = new SqlCommand(CmdString, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            adp.Fill(dt);
            AlgSource.ItemsSource = dt.Tables[0].DefaultView;
            AlgSource.LoadingRow += new EventHandler<DataGridRowEventArgs>(GTablecontent_LoadingRow); //载入计算得出的行数



            conn.Close();

        }


       
        private void AlgSource_Loaded(object sender, RoutedEventArgs e)
        {
            string TableName = "Algorithm";
            string CmdString = string.Empty;
            CmdString = "Select * from " + TableName;//如果只显示某几列数据，可以用其他sql语句控制，如select 部门经理 from 部门表
            SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand(CmdString, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            adp.Fill(dt);
            AlgSource.ItemsSource = dt.Tables[0].DefaultView;
            AlgSource.LoadingRow += new EventHandler<DataGridRowEventArgs>(GTablecontent_LoadingRow); //载入计算得出的行数
 
        }
        
        
        
        
        private void DeleteAlg_Click(object sender, RoutedEventArgs e)
        {
            string TableName = "Algorithm";

            for (int i = AlgSource.SelectedItems.Count - 1; i >= 0; i--)
            {

                DataRowView mySelectedElement = (DataRowView)AlgSource.SelectedItem;
                string resultcol = mySelectedElement.Row[0].ToString();
                string selectedColumnHeader = (string)AlgSource.SelectedCells[0].Column.Header;
                System.Console.WriteLine(selectedColumnHeader);
                try
                {
                    string sql = "DELETE FROM " + TableName + " where " + selectedColumnHeader + " = '" + resultcol + "'";
                    SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
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
                    AlgSource.ItemsSource = dt.Tables[0].DefaultView;
                    AlgSource.LoadingRow += new EventHandler<DataGridRowEventArgs>(GTablecontent_LoadingRow); //载入计算得出的行数

                }
                catch
                {
                    throw new Exception("删除发生错误！");
                }

            }

        }

        private void AlgCombobox_Loaded(object sender, RoutedEventArgs e)
        {
            string TableName = "Algorithm";
            string CmdString = "Select [AlgName] from " + TableName;//如果只显示某几列数据，可以用其他sql语句控制，如select 部门经理 from 部门表
         
            SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();         
            conn.Open();
            SqlCommand cmd = new SqlCommand(CmdString, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            adp.Fill(dt);
             
            AlgCombobox.ItemsSource = dt.Tables[0].DefaultView;
            AlgCombobox.DisplayMemberPath = "AlgName";
            AlgCombobox.SelectedIndex = 0;
           
        }

        private void Outputbtn_Click(object sender, RoutedEventArgs e)
        {
               //DataTable dt;

               //dt = DGSource.ItemsSource as DataTable;

            DataImporting dataimport = new DataImporting();

            DataTable source = dataimport.SourceReady(sourcetext.Text.ToString());

            DataConfig dc = new DataConfig(source);

            ChartDataItems resultsready = dc.ResultReady();

            DataTable Results = resultsready.ValueToGrid(resultsready);



               StoreExcel se=new StoreExcel();
            
               se.CreateCSVFile(Results, "E:\\qingganfenxi\\EmoAns\\result.csv");

        }



        private void ThemeCombo_Loaded(object sender, RoutedEventArgs e)
        {
            string TableName = "Aspect";
            string CmdString = "Select distinct Theme from " + TableName;//如果只显示某几列数据，可以用其他sql语句控制，如select 部门经理 from 部门表

            SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand(CmdString, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet dtt = new DataSet();
            adp.Fill(dtt);

            ThemeCombo.ItemsSource = dtt.Tables[0].DefaultView;
            ThemeCombo.DisplayMemberPath = "Theme";
            ThemeCombo.SelectedIndex = 0;

            conn.Close();
            
        }


        private void AspectCombo_Loaded(object sender, RoutedEventArgs e)
        {
            string TableName = "Aspect";
            string ThemeName = ThemeCombo.Text.ToString();

            string CmdString = "Select distinct Aspect from " + TableName + " where Theme = '" + ThemeName + "'";//如果只显示某几列数据，可以用其他sql语句控制，如select 部门经理 from 部门表

            SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand(CmdString, conn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet dts = new DataSet();
            adp.Fill(dts);

            AspectCombo.ItemsSource = dts.Tables[0].DefaultView;
            AspectCombo.DisplayMemberPath = "Aspect";
        

            conn.Close();

        }
       

       


    }
}
