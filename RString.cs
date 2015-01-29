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

        public string between(string startStr, string endStr)
        {
            string res = "";
            startStr = startStr.Replace('*', '"');
            endStr = endStr.Replace('*', '"');
            int first = val.IndexOf(startStr);
            if (first > -1)
            {
                int offset = first + startStr.Length;
                int last = val.IndexOf(endStr, offset, StringComparison.Ordinal);
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
