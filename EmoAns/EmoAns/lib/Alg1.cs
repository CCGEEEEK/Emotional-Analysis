using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace TextProcessing
{
    class Alg1
    {
        public void Analy()
        {

            EmoAns.WordParsing Word = new EmoAns.WordParsing();
            int count = Word.Counting("【环球网综合报道】据俄罗斯军工综合体新闻网12月27日报道，俄罗斯世界武器贸易分析中心12月26日根据已经签订的合同和采购意向，对2012年");
            string result = Word.Parsing("【环球网综合报道】据俄罗斯军工综合体新闻网12月27日报道，俄罗斯世界武器贸易分析中心12月26日根据已经签订的合同和采购意向，对2012年", 0);
            System.Console.WriteLine(count);
            System.Console.WriteLine(result);


           
             
        }
    }

}
