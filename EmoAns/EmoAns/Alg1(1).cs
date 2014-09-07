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
    

        class Alg1
        {
            public string sourcetext
            {
                get;
                set;
            }
            public string stoplist
            {
                get;
                set;
            }

            public Alg1(string sourcetext, string stoplist = "")
            {

                byte[] bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(sourcetext));
                this.sourcetext = Encoding.UTF8.GetString(bytes);
                this.stoplist = stoplist;
            }

            public ChartData emotionresult(string sourcetextpattern, string positiveregexpattern, string negativeregexpattern)
            {
                byte[] bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(sourcetextpattern));
                sourcetextpattern = Encoding.UTF8.GetString(bytes); 
                bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(negativeregexpattern));
                negativeregexpattern = Encoding.UTF8.GetString(bytes);
                bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(positiveregexpattern));
                positiveregexpattern = Encoding.UTF8.GetString(bytes);
                Regex positiveregex = new Regex(positiveregexpattern);
                Regex negativeregex = new Regex(negativeregexpattern);
                
                //System.Console.WriteLine(this.sourcetext);
                //System.Console.WriteLine(negativeregex.Matches(this.sourcetext).Count);
                //System.Console.WriteLine(positiveregex.Matches(this.sourcetext).Count);

                double result= positiveregex.Matches(this.sourcetext).Count-negativeregex.Matches(this.sourcetext).Count;
                
                ChartData emotionresult = new ChartData(sourcetext,result);
                return emotionresult;   
            }

          

        }

    }


