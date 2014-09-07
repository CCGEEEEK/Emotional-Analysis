using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

namespace EmoAns
{
    class Statistic
    {
        public DataTable tempdt
        {
            get;
            set;
        }


        public Statistic(DataTable dt) { this.tempdt = dt; }
        public ChartDataItems StasticCount()
        {
             

            int positivenum = 0, negativenum = 0, neturalnum = 0;

            foreach (DataRow dr in tempdt.Rows)
            {

                int unknown = Convert.ToInt32(dr[1]);

                if (unknown > 0) { positivenum += 1; }
                else if (unknown < 0) { negativenum += 1; }
                else neturalnum += 1;



            }

            ChartData positive = new ChartData("正情感", positivenum);

            ChartData negative = new ChartData("负情感", negativenum);

            ChartData netural = new ChartData("中性", neturalnum);

            ChartDataItems emotionStatic = new ChartDataItems();

            emotionStatic.Items.Add(positive);

            emotionStatic.Items.Add(negative);
            emotionStatic.Items.Add(netural);
            return emotionStatic;
        }
 
    }
}
