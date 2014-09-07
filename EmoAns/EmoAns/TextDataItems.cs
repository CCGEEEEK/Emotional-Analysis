using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmoAns


{

        public class TextDataItem
    {
        public string Word
        {
            get;
            set;
        }
        public int Count
        {
            get;
            set;
        }
    }

    public class TextDataItems
    {
        public List<TextDataItem> Items
        {
            get;
            set;
        }
        public TextDataItems(){
            Items = new List<TextDataItem>();
        }
      
        public void DescSort()
        {
            descCompara desc = new descCompara();
            Items.Sort(desc);
        }
        public void AscSort()
        {
            ascCompara asc = new ascCompara();
            Items.Sort(asc);
        }
        private class descCompara : IComparer<TextDataItem>
        {
            public int Compare(TextDataItem x, TextDataItem y)
            {
                return y.Count.CompareTo(x.Count);
            }
        }
        private class ascCompara : IComparer<TextDataItem>
        {
            public int Compare(TextDataItem x, TextDataItem y)
            {
                return x.Count.CompareTo(y.Count);
            }
        }
    }

}
