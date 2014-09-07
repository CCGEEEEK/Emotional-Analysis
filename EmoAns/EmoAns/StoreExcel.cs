using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using Microsoft.Win32;
using Microsoft.Office.Interop.Excel;



namespace EmoAns
{
    class StoreExcel
    {
        //获取表格中的数据
        public System.Data.DataTable LoadExcel(string pPath)
        {

            string connString = "Driver={Driver do Microsoft Excel(*.xls)};DriverId=790;SafeTransactions=0;ReadOnly=1;MaxScanRows=16;Threads=3;MaxBufferSize=2024;UserCommitSync=Yes;FIL=excel 8.0;PageTimeout=5;";  //连接字符串  

            //简单解释下这个连续字符串，Driver={Driver do Microsoft Excel(*.xls)} 这种连接写法不需要创建一个数据源DSN，DRIVERID表示驱动ID，Excel2003后都使用790，

            //FIL表示Excel文件类型，Excel2007用excel 8.0，MaxBufferSize表示缓存大小， 如果你的文件是2010版本的，也许会报错，所以要找到合适版本的参数设置。

            connString += "DBQ=" + pPath; //DBQ表示读取Excel的文件名（全路径）
            OdbcConnection conn = new OdbcConnection(connString);
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = conn;
            //获取Excel中第一个Sheet名称，作为查询时的表名
            string sheetName = this.GetExcelSheetName(pPath);
            string sql = "select * from [" + sheetName.Replace('.', '#') + "$]";
            cmd.CommandText = sql;
            OdbcDataAdapter da = new OdbcDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                return ds.Tables[0];    //返回Excel数据中的内容，保存在DataTable中
            }
            catch (Exception x)
            {
                ds = null;
                throw new Exception("从Excel文件中获取数据时发生错误！可能是Excel版本问题，可以考虑降低版本或者修改连接字符串值");
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                da.Dispose();
                da = null;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn = null;
            }
        }

        // 获取工作表名称
        private string GetExcelSheetName(string pPath)
        {
            //打开一个Excel应用
            Microsoft.Office.Interop.Excel.Application excelApp;
            Workbook excelWB;//创建工作簿（WorkBook：即Excel文件主体本身）
            Workbooks excelWBs;
            Worksheet excelWS;//创建工作表（即Excel里的子表sheet）

            Sheets excelSts;

            excelApp = new Microsoft.Office.Interop.Excel.Application();
            if (excelApp == null)
            {
                throw new Exception("打开Excel应用时发生错误！");
            }
            excelWBs = excelApp.Workbooks;
            //打开一个现有的工作薄
            excelWB = excelWBs.Add(pPath);
            excelSts = excelWB.Sheets;
            //选择第一个Sheet页
            excelWS = excelSts.get_Item(1);
            string sheetName = excelWS.Name;

            ReleaseCOM(excelWS);
            ReleaseCOM(excelSts);
            ReleaseCOM(excelWB);
            ReleaseCOM(excelWBs);
            excelApp.Quit();
            ReleaseCOM(excelApp);
            return sheetName;
        }

        // 释放资源
        private void ReleaseCOM(object pObj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pObj);
            }
            catch
            {
                throw new Exception("释放资源时发生错误！");
            }
            finally
            {
                pObj = null;
            }
        }




      
    }
}
