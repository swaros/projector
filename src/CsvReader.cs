using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{   
    /// <summary>
    /// Csv Reader.
    /// Reads and CSV File and handles the content
    /// in an Data matrix
    /// </summary>
    class CsvReader
    {

        /// <summary>
        /// used ContenReader
        /// </summary>
        private ContentFile fileReader = new ContentFile();

        /// <summary>
        /// Contains header infomations
        /// </summary>
        private List<CsvContent> Header = new List<CsvContent>();

        /// <summary>
        ///  contains all cell infomations
        /// </summary>
        private List<CsvContent> Content = new List<CsvContent>();

        /// <summary>
        /// the Data Storage
        /// </summary>
        private Hashtable tableMatrix = new Hashtable();

        /// <summary>
        /// the count of all rows
        /// </summary>
        private int rowCount;

        /// <summary>
        /// the count of all Columns
        /// </summary>
        private int columnCount;

        /// <summary>
        /// loads csv file if possible
        /// </summary>
        /// <param name="filename">full path to csv file</param>
        /// <returns>false if file not exists or readable</returns>
        public Boolean loadFile(string filename)
        {
            this.fileReader.loadFile(filename);
            if (this.fileReader.fileIsLoaded())
            {
                this.parseCsv();
                return true;
            }
            return false;
        }

        /// <summary>
        /// sets the char thats seperates the columns
        /// most ; or ,
        /// can be set to ";," to make sure it works in any of this cases
        /// </summary>
        /// <param name="delimiter"></param>
        public void setDelimiter(string delimiter)
        {
            this.fileReader.setDelimiter(delimiter);
        }

        /// <summary>
        /// returns all errors
        /// </summary>
        /// <returns>Stringlist that contains all occured errors while reading
        /// and parsing</returns>
        public List<string> getErrors(){
            return this.fileReader.getErrors();
        }


        /// <summary>
        /// returns the row count
        /// </summary>
        /// <returns>count of rows</returns>
        public int getRowCount()
        {
            return this.rowCount;
        }

        /// <summary>
        /// returns maximal count of columns.
        /// </summary>
        /// <returns>Max Colums</returns>
        public int getColumnCount()
        {
            return this.columnCount;
        }

        /// <summary>
        /// gets the header from CSV
        /// </summary>
        /// <returns></returns>
        public List<string> getHeader()
        {
            List<string> result = new List<string>();
            foreach (CsvContent headText in this.Header){
                result.Add(headText.Text);
            }
            return result;
        }

        /// <summary>
        /// get the Conten from the given row and column index.
        /// returns null if there not data exists
        /// </summary>
        /// <param name="rowNr"></param>
        /// <param name="ColumnNr"></param>
        /// <returns></returns>
        public CsvContent getContent(int rowNr, int ColumnNr)
        {
            string key = this.getMatrixKey(rowNr, ColumnNr);
            if (this.tableMatrix.ContainsKey(key))
            {
                return (CsvContent)this.tableMatrix[key];
            }
            return null;
        }

        /// <summary>
        /// creates the index key
        /// </summary>
        /// <param name="row">current Row Index</param>
        /// <param name="column">current Column Index</param>
        /// <returns>the key that used for this combination of row and cloumn index</returns>
        private String getMatrixKey(int row, int column){
            return "c" + row + ":" + column;
        }

        /// <summary>
        /// parse the csv file
        /// </summary>
        /// <returns></returns>
        private Boolean parseCsv()
        {
            Hashtable fContent = this.fileReader.getContent();
            if (this.fileReader.lineCount > 0 && this.fileReader.maxCellCount > 0)
            {
                this.columnCount = this.fileReader.maxCellCount;
                List<string>header = (List<string>) fContent[0];

                foreach (string name in header)
                {
                    CsvContent head = new CsvContent(name, 0, 0);
                    this.Header.Add(head);
                }

                int rowNr = 0;
                for (int i = 1; i < fContent.Count; i++)
                {
                    int columnNr=0;
                    foreach (string cellText in this.fileReader.getContentAtIndex(i))
                    {
                        CsvContent row = new CsvContent(cellText, rowNr, columnNr );
                        this.tableMatrix.Add(this.getMatrixKey(rowNr, columnNr), row);
                        columnNr++;
                        
                    }
                    rowNr++;
                    this.rowCount = i;
                }
                return true;
            }
            return false;
        }
    }
}
