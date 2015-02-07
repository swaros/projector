using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{   
    /// <summary>
    /// Csv Reader.
    /// </summary>
    class CsvReader
    {
        private ContentFile fileReader = new ContentFile();

        private List<CsvContent> Header = new List<CsvContent>();

        private List<CsvContent> Content = new List<CsvContent>();

        private Hashtable tableMatrix = new Hashtable();

        private int rowCount;

        private int columnCount;


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

        public int getRowCount()
        {
            return this.rowCount;
        }

        public int getColumnCount()
        {
            return this.columnCount;
        }

        public List<string> getHeader()
        {
            List<string> result = new List<string>();
            foreach (CsvContent headText in this.Header){
                result.Add(headText.Text);
            }
            return result;
        }

        public CsvContent getContent(int rowNr, int ColumnNr)
        {
            string key = this.getMatrixKey(rowNr, ColumnNr);
            if (this.tableMatrix.ContainsKey(key))
            {
                return (CsvContent)this.tableMatrix[key];
            }
            return null;
        }


        private String getMatrixKey(int row, int column){
            return "c" + row + ":" + column;
        }

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
