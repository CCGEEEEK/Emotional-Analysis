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
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;


namespace EmoAns
{
    class DataImporting
    {

        public DataTable SourceReady(string Sourcepath)
        {
            StoreExcel LoadExcel = new StoreExcel();
            DataTable Sourcetable = LoadExcel.LoadExcel(Sourcepath);
            DataTable AfterParsedSource=new DataTable();
             
            AfterParsedSource.Columns.Add("Content");
            Nlpir nlpir = new Nlpir();
            foreach (DataRow dr in Sourcetable.Rows)
            {
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    string afterPased = null;
                    string beforePased = dr[dc].ToString();
                    
                    afterPased = nlpir.ParagraphProcess(beforePased, 0);
                    AfterParsedSource.Rows.Add(afterPased);
                     
                }
            }

            
            return AfterParsedSource;
        }
    }
}
