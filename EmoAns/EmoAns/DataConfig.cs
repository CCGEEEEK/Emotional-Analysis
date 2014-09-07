using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.Threading;
using System.Data.SqlClient;


namespace EmoAns
{
    /// <summary>
    /// DataConfig.xaml 的交互逻辑
    /// </summary>
    class DataConfig
    {
        DataTable  dt = null;
         
        string stopQ;//停词表每行一词，结果为(词\r\n)。但直接匹配词语就可以
        string positiveList;
        string negativeList;
        TextDataItems keywords = new TextDataItems();
        TextDataItems positiveWords = new TextDataItems();
        TextDataItems negativeWords = new TextDataItems();

        public DataConfig(DataTable DT)
        {
            dt = DT;
        }





        public ChartDataItems ResultReady()
        {
            string positiveRegexPattern = "[";
            string negativeRegexPattern = "[";
            ChartDataItems emotionResult = new ChartDataItems();
            stopQ = fileRead("StopQ");
            positiveList = fileRead("PositiveList");
            negativeList = fileRead("NegativeList");
            positiveRegexPattern += positiveList.Replace("\r\n", "|");
            negativeRegexPattern += negativeList.Replace("\r\n", "|");
            positiveRegexPattern = positiveRegexPattern.Substring(0, positiveRegexPattern.Length - 1).Replace("\t","").Replace(" ","");
            negativeRegexPattern = negativeRegexPattern.Substring(0, negativeRegexPattern.Length - 1).Replace("\t", "").Replace(" ", "");
            positiveRegexPattern += "]";
            negativeRegexPattern += "]";
            
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    string sourcepiece = dr[dc].ToString();

                    Alg1 ag = new Alg1(sourcepiece, "");
                    ChartData resultpiece = ag.emotionresult(sourcepiece, positiveRegexPattern, negativeRegexPattern);
                    //System.Console.WriteLine(resultpiece.Value);
                    emotionResult.Items.Add(resultpiece);
                }
            }

            return emotionResult;
        }

       


      




        private string fileRead(string tablename)
        {

            try
            {
                
                SqlConnection conn = DBinfo.ConnectionHelper.GetConnection();
                conn.Open();
                string commandString = "Select * from " + tablename;

                SqlDataAdapter da = new SqlDataAdapter(commandString, conn);

                DataSet tempdataSet = new DataSet();
                //把数据表添加到数据集中  
               
                //将数据填充到数据集中  
                da.Fill(tempdataSet,"res");
                string res = DataTableToString(tempdataSet.Tables["res"]);
                byte[] bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(res));
                res = Encoding.UTF8.GetString(bytes);//转成utf-8编码
                return res;

               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string DataTableToString(DataTable dt)
        {
            string dtstring = "";

            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dr.Table.Columns)
                {

                    dtstring = dtstring + dr[dc].ToString() + "\t";

                    dtstring = dtstring + "\r\n";
                }
            }

            return dtstring;
        }




    }
}
