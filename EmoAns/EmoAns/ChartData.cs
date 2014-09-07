using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;


namespace EmoAns
{
    public class ChartData
    {
        public string Name
        {
            get;
            set;
        }
        public double Value
        {
            get;
            set;
        }
        public ChartData(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
    public class ChartDataItems
    {
        public List<ChartData> Items
        {
            get;
            set;
        }
        public ChartDataItems()
        {
            Items = new List<ChartData>();
        }


         public DataTable ValueToGrid(ChartDataItems resultlist)
        {
            DataTable dt = new DataTable();
            ChartData cd = null;
            dt.Columns.Add("source");
            dt.Columns.Add("result");
             
            int k = 0;

            for (k = 0; k < resultlist.Items.Count(); k++)
            {
                cd = resultlist.Items[k];
                DataRow dr = dt.NewRow();
                dr["source"] = cd.Name;
                dr["result"] = cd.Value;
                dt.Rows.Add(dr);
            }
             
            return dt;
        }

    }
}
