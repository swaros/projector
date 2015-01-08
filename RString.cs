using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class RString
    {

        private string val = "";

        public void setString(string str)
        {
            this.val = str;
        }

        public String subString(int start, int lengt){
            return val.Substring(start, lengt);   
        }

        public string between(string right, string left)
        {
            string res = "";
            right = right.Replace('*', '"');
            left = left.Replace('*', '"');
            int first = val.IndexOf(right);
            if (first > -1)
            {
                int offset = first + right.Length;
                int last = val.IndexOf(left, offset, StringComparison.Ordinal);
                if (last > -1)
                {
                    int len = last - offset;
                    if (len > 0)
                    {
                        return val.Substring(offset, len);
                    }
                }
            }
            return res;
        }

        public String replace(string what, string inthis)
        {
            return val.Replace(what, inthis);
        }

        public string getString()
        {
            return this.val;
        }
    }
}
