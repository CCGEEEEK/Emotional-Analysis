﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using System.Threading;
using System.Configuration;


namespace EmoAns
{
    class ReadExcel
    {

        /// <summary> 
        /// 把Excel里的数据转换为DataTable,应用引用的com组件：Microsoft.Office.Interop.Excel.dll 读取EXCEL文件
        /// </summary> 
        /// <param name="filenameurl">物理路径</param> 
        /// <param name="sheetIndex">sheet名称的索引</param> 
        /// <param name="splitstr">如果是已存在列，则自定义添加的字符串</param> 
        /// <returns></returns> 
        public static DataTable ExecleToDataSet(string filenameurl, int sheetIndex, string splitstr)
        {
            // 
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            Microsoft.Office.Interop.Excel.Worksheet ws = null;
            bool isEqual = false;//不相等 
            ArrayList columnArr = new ArrayList();//列字段表 
            DataSet myDs = new DataSet();
            DataTable xlsTable = myDs.Tables.Add("show");
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();//lauch excel application
            if (excel != null)
            {
                excel.Visible = false;
                excel.UserControl = true;
                // 以只读的形式打开EXCEL文件 
                wb = excel.Workbooks.Open(filenameurl, missing, true, missing, missing, missing,
                 missing, missing, missing, true, missing, missing, missing, missing, missing);
                //取得第一个工作薄 
                ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets.get_Item(sheetIndex);
                //取得总记录行数(包括标题列) 
                int rowsint = ws.UsedRange.Cells.Rows.Count; //得到行数 
                int columnsint = ws.UsedRange.Cells.Columns.Count;//得到列数 
                DataRow dr;
                for (int i = 1; i <= columnsint; i++)
                {
                    //判断是否有列相同 
                    if (i >= 2)
                    {
                        int r = 0;
                        for (int k = 1; k <= i - 1; k++)//列从第一列到第i-1列遍历进行比较 
                        {
                            if (((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, i]).Text.ToString() == ((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, k]).Text.ToString())
                            {
                                //如果该列的值等于前面列中某一列的值 
                                xlsTable.Columns.Add(((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, i]).Text.ToString() + splitstr + (r + 1).ToString(), typeof(string));
                                columnArr.Add(((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, i]).Text.ToString() + splitstr + (r + 1).ToString());
                                isEqual = true;
                                r++;
                                break;
                            }
                            else
                            {
                                isEqual = false;
                                continue;
                            }
                        }
                        if (!isEqual)
                        {
                            xlsTable.Columns.Add(((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, i]).Text.ToString(), typeof(string));
                            columnArr.Add(((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, i]).Text.ToString());
                        }
                    }
                    else
                    {
                        xlsTable.Columns.Add(((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, i]).Text.ToString(), typeof(string));
                        columnArr.Add(((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, i]).Text.ToString());
                    }
                }
                for (int i = 2; i <= rowsint; i++)
                {
                    dr = xlsTable.NewRow();
                    for (int j = 1; j <= columnsint; j++)
                    {
                        dr[columnArr[j - 1].ToString()] = ((Microsoft.Office.Interop.Excel.Range)ws.Cells[i, j]).Text.ToString();
                    }
                    xlsTable.Rows.Add(dr);
                }
            }
            excel.Quit();
            excel = null;
             
            return xlsTable;
        }

    }
}
