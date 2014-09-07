using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.IO;


namespace EmoAns
{
    class StoreExcel
    {
        //获取表格中的数据
        public System.Data.DataTable LoadExcel(string pPath)
        {

            DataTable csvDataTable = new DataTable();
            try
            {
                //no try/catch - add these in yourselfs or let exception happen
                String[] csvData = File.ReadAllLines(pPath);

                //if no data
                if (csvData.Length == 0)
                {
                    return csvDataTable;
                }

                String[] headings = csvData[0].Split(';');

                //for each heading
                for (int i = 0; i < headings.Length; i++)
                {
                    ////replace spaces with underscores for column names
                    //headings[i] = headings[i].Replace(" ", "_");

                    //add a column for each heading
                    csvDataTable.Columns.Add(headings[i]);

                }

                //populate the DataTable
                for (int i = 1; i < csvData.Length; i++)
                {
                    //create new rows
                    DataRow row = csvDataTable.NewRow();

                    for (int j = 0; j < headings.Length; j++)
                    {
                        //fill them
                        row[j] = csvData[i].Split(';')[j];

                    }

                    //add rows to over DataTable

                    csvDataTable.Rows.Add(row);


                }
            }

            catch (Exception ex)
            {

            }
            //return the CSV DataTable

            return csvDataTable;



        }

        // 获取工作表名称
        public void CreateCSVFile(DataTable temp, string strFilePath)
        {
            DataTable dt = new DataTable();
            dt = temp;

            try
            {
                StreamWriter sw = new StreamWriter(strFilePath, false);
                
                int columnCount = dt.Columns.Count;
                
                
                for (int i = 0; i < columnCount; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < columnCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < columnCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString());
                        }
                        if (i < columnCount - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  







   

}
}



