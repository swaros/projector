using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class CsvContent
    {
        private string OriginString;
        public string Text;
        private int rowIndex;
        private int columnIndex;

        public CsvContent(string content, int row, int column)
        {
            this.OriginString = content;
            this.Text = this.getValue(content);
            this.rowIndex = row;
            this.columnIndex = column;
        }

        private String getValue(string str)
        {
            string result = str;
            str = str.Trim();
            
            if (str.Length > 0 && str[0] == '"' && str[str.Length - 1] == '"')
            {
                result = str.Substring(1, str.Length - 2);
            }
            return result;
        }


    }
}
