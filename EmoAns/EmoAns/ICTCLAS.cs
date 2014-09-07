using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Runtime.InteropServices;
namespace EmoAns
{
    //////////////////////////////////////////////////////////////////////////
    // character coding types 
    //////////////////////////////////////////////////////////////////////////
    enum eCodeType
    {
        CODE_TYPE_UNKNOWN,//type unknown
        CODE_TYPE_ASCII,//ASCII
        CODE_TYPE_GB,//GB2312,GBK,GB10380
        CODE_TYPE_UTF8,//UTF-8
        CODE_TYPE_BIG5//BIG5
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct result_t
    {
        [FieldOffset(0)]
        public int start;
        [FieldOffset(4)]
        public int length;
        [FieldOffset(8)]
        public int sPos;
        [FieldOffset(12)]
        public int sPosLow;
        [FieldOffset(16)]
        public int POS_id;
        [FieldOffset(20)]
        public int word_ID;
        [FieldOffset(24)]
        public int word_type;
        [FieldOffset(28)]
        public int weight;

    }


    namespace EmoAns
    {
        class ICTCLAS
        {

            const string path = @"ICTCLAS50.dll";

            [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "ICTCLAS_Init")]
            public static extern bool ICTCLAS_Init(String sInitDirPath);

            [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "ICTCLAS_Exit")]
            public static extern bool ICTCLAS_Exit();

            [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "ICTCLAS_ParagraphProcessAW")]
            public static extern int ICTCLAS_ParagraphProcessAW(String sParagraph, eCodeType eCT, int bPOSTagged, [Out, MarshalAs(UnmanagedType.LPArray)]result_t[] result);

            //[DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "ICTCLAS_ParagraphProcess")]
            //public static extern int ICTCLAS_ParagraphProcess(String sParagraph, int nPaLen, eCodeType eCt, int bPOStagged, String sResult);

            [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "ICTCLAS_ImportUserDict")]
            public static extern int ICTCLAS_ImportUserDict(String sFilename, eCodeType eCT);

            [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "ICTCLAS_FileProcess")]
            public static extern double ICTCLAS_FileProcess(String sSrcFilename, eCodeType eCt, String sDsnFilename, int bPOStagged);

        }
    }
}
