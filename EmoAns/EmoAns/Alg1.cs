using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoAns
{
    class Alg1
    {

        public void AnalysisEmo(){

       
           if (!EmoAns.ICTCLAS.ICTCLAS_Init(null))
            {
                System.Console.WriteLine("Init ICTCLAS failed!");
                System.Console.Read();
                return;

            }

            System.Console.WriteLine("Init ICTCLAS Success!");
            
            
            //导入用户所需添加的词库，再次分词更加准确
            EmoAns.ICTCLAS.ICTCLAS_ImportUserDict("userdic.txt",eCodeType.CODE_TYPE_UNKNOWN);

           // String s = "ICTCLAS在国内973专家组组织的评测中活动获得了第一名，在第一届国际中文处理研究机构SigHan组织的评测中都获得了多项第一名。";
            String s = "abc他说的确实在理123";
            int count =s.Length;//先得到结果的词数
            result_t[] result = new result_t[count];//在客户端申请资源
            int i = 0;
         
            //
           int nWrdCnt=EmoAns.ICTCLAS.ICTCLAS_ParagraphProcessAW(s,eCodeType.CODE_TYPE_GB,1,result);
            result_t r;
            //取字符串真实长度:
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(s);
            byte[] byteWord;
            for (i=0;i<nWrdCnt;i++)
            {
                r=result[i];
                String sWhichDic = "";

                switch (r.word_type)
                {
                    case 0:
                        sWhichDic = "核心词典";
                        break;
                    case 1:
                        sWhichDic = "用户词典";
                        break;
                    case 2:
                        sWhichDic = "专业词典";
                        break;
                    default:
                        break;
                }

                byteWord = new byte[r.length];
                //取字符串一部分
                Array.Copy(mybyte, r.start, byteWord, 0, r.length);
                string wrd = System.Text.Encoding.Default.GetString(byteWord);

                Console.WriteLine("No.{0}:start:{1}, length:{2},POS_ID:{3},Word_ID:{4}, UserDefine:{5}, Word:{6}\n", i, r.start, r.length, r.POS_id, r.word_ID, sWhichDic, wrd);
          
               // Console.WriteLine("{0}:{1},{2}\n", r.start, r.length, wrd);
            }

           //文件处理
            EmoAns.ICTCLAS.ICTCLAS_FileProcess("test.txt",eCodeType.CODE_TYPE_GB, "Input_result.txt", 1);
            EmoAns.ICTCLAS.ICTCLAS_Exit();

            System.Console.Read();
        }
    }
}









    
