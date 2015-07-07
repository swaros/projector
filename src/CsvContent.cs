using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    /// <summary>
    /// Containerobject for an Entry
    /// in a CSV File.
    /// </summary>
    class CsvContent
    {
        /// <summary>
        /// This is the String in original Format, so
        /// all removed chars can be found here
        /// </summary>
        private string OriginString;
        /// <summary>
        /// The string without any mask chars. 
        /// </summary>
        public string Text;
        /// <summary>
        /// this is the index of the row
        /// </summary>
        private int rowIndex;
        /// <summary>
        /// this is the index of the column
        /// </summary>
        private int columnIndex;

        /// <summary>
        /// Creates an new CSV Entry
        /// </summary>
        /// <param name="content">The Origin Conten off the cell</param>
        /// <param name="row">Index of row</param>
        /// <param name="column">Index of column</param>
        public CsvContent(string content, int row, int column)
        {
            this.OriginString = content;
            this.Text = this.getValue(content);
            this.rowIndex = row;
            this.columnIndex = column;
        }
        /// <summary>
        /// translates the Origin to Text
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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
