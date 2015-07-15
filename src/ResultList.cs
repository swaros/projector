using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector.Data
{
    public class ResultList
    {
        /// <summary>
        /// Max Number of allowed Columns
        /// </summary>
        private int ColumnCount = 0;
        /// <summary>
        /// Max allowed count of Rows
        /// </summary>
        private int RowCount = 0;

        /// <summary>
        /// the names of the columns
        /// </summary>
        private List<string> ColumnNames = new List<string>();

        /// <summary>
        /// for faster access to Colums.
        /// stores the index for any key
        /// </summary>
        private Hashtable ColumnIndex = new Hashtable();

        /// <summary>
        /// The whole Datamatrix
        /// </summary>
        private List<List<Object>> DataMatrix = new List<List<Object>>();

        private List<List<string>> StringMatrix = new List<List<string>>();

        /// <summary>
        /// sets the maximal count of Columns
        /// </summary>
        /// <param name="count"></param>
        public void setColumnCount(int count)
        {
            int old = this.ColumnCount;
            this.ColumnCount = count;
            this.addEmptyColumns(old);
        }

        /// <summary>
        /// gets the count of columns
        /// </summary>
        public int getColumnCount()
        {
            return this.ColumnCount;
        }

        /// <summary>
        /// gets the count of rows
        /// </summary>
        /// <returns>max number of rows</returns>
        public int getRowCount()
        {            
            return this.RowCount;
        }

        /// <summary>
        /// Resets all Content
        /// </summary>
        public void Clear()
        {
            this.DataMatrix.Clear();
            this.ColumnIndex.Clear();
            this.ColumnNames.Clear();
            this.ColumnCount = 0;
            this.RowCount = 0;
        }

        /// <summary>
        /// returns the headers as StringList
        /// </summary>
        /// <returns></returns>
        public List<string> getColumns()
        {
            List<string> copy = new List<string>();
            copy.AddRange(this.ColumnNames);
            return copy;
        }


        /// <summary>
        /// add an named Column if not Exists
        /// </summary>
        /// <param name="name">the Column Name</param>
        /// <returns>false if Column with this name already exists</returns>
        public Boolean AddColumn(string name)
        {
            if (!this.ColumnNames.Contains(name))
            {                
                this.ColumnNames.Add(name);
                this.ColumnIndex.Add(name, this.ColumnCount);
                this.setColumnCount(this.getColumnCount() + 1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// add an named Column if not Exists
        /// </summary>
        /// <param name="name">the Column Name</param>
        /// <returns>false if Column with this name already exists</returns>
        public Boolean AddColumnWithNumber(string name)
        {
            name = name + (this.ColumnCount + 1);
            if (!this.ColumnNames.Contains(name))
            {
                this.ColumnNames.Add(name);
                this.ColumnIndex.Add(name, this.ColumnCount);
                this.setColumnCount(this.getColumnCount() + 1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// add a new Row and returns the
        /// index for the added row
        /// </summary>
        /// <returns></returns>
        public int AddRow()
        {
            this.setRowCount(this.getRowCount() + 1);
            return this.RowCount - 1;
        }

        /// <summary>
        /// updates an Object in the Matrix
        /// </summary>
        /// <param name="x">Column Index</param>
        /// <param name="y">Row Index</param>
        /// <param name="Value">The Object</param>
        /// <returns>false if one of booth indicies out of Range</returns>
        private Boolean setCellValue(int x, int y, Object Value)
        {
            if (x < this.ColumnCount && y < this.RowCount)
            {
                this.DataMatrix[y][x] = Value;
                this.StringMatrix[y][x] = Value.ToString();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the Oject from given Indicies
        /// </summary>
        /// <param name="x">Index of Column</param>
        /// <param name="y">Index of Row</param>
        /// <returns></returns>
        private Object getCellValue(int x, int y)
        {
            if (x < this.ColumnCount && y < this.RowCount)
            {
                return this.DataMatrix[y][x];
                
            }
            return null;
        }


        /// <summary>
        /// gets the index of an Row by name
        /// </summary>
        /// <param name="name">the name of the Column</param>
        /// <returns>the index or -1 if not exists</returns>
        private int getColumnIndex(string name)
        {
            if (this.ColumnIndex.ContainsKey(name))
            {
                return (int) this.ColumnIndex[name];
            }
            return -1;
        }

        /// <summary>
        /// Updates an Object in the Matrix by name and Row Number
        /// </summary>
        /// <param name="name">name of the Field</param>
        /// <param name="rowNumber">Number of the Row</param>
        /// <param name="Value">The Object that have to be stored</param>
        /// <returns>False if Fieldname not exists, or rownumber ot of range</returns>
        public Boolean setValue(string name, int rowNumber, Object Value)
        {
            int columnIndex = this.getColumnIndex(name);
            if (columnIndex >= 0)
            {
                return this.setCellValue(columnIndex, rowNumber, Value);
            }
            return false;
        }

        /// <summary>
        /// Returns an Object by submitted Indicies
        /// </summary>
        /// <param name="name">Index of Column</param>
        /// <param name="rowNumber">Index or Row</param>
        /// <returns>the stored Object or null if noot found</returns>
        public Object getValue(string name, int rowNumber)
        {
            int columnIndex = this.getColumnIndex(name);
            if (columnIndex >= 0)
            {
                return this.getCellValue(columnIndex, rowNumber);
            }
            return null;
        }

        /// <summary>
        /// returns an Objectin the matrix as String
        /// </summary>
        /// <param name="name">name of Row</param>
        /// <param name="rowNumber">Index of Column</param>
        /// <returns></returns>
        public String getValueToString(string name, int rowNumber)
        {
            return this.getValue(name, rowNumber).ToString();
        }


        /// <summary>
        /// set the maximum amount of rows
        /// </summary>
        /// <param name="count"></param>
        public void setRowCount(int count)
        {
            int old = this.RowCount;
            this.RowCount = count;
            this.addEmptyRows(old);
        }

        /// <summary>
        /// adds empty columns
        /// </summary>
        /// <param name="offset">old max value as Offset</param>
        private void addEmptyColumns(int offset)
        {
            this.buildEmptyMatrix(offset, this.RowCount);
        }

        /// <summary>
        /// returns the complete row as StringList
        /// </summary>
        /// <param name="rowNr"></param>
        /// <returns></returns>
        public string[] getRowStringList(int rowNr)
        {
            if (rowNr < this.RowCount)
            {
                string[] res = new string[this.StringMatrix[rowNr].Count];
                this.StringMatrix[rowNr].CopyTo(res, 0);
                return res;
            }
            return null;
        }


        /// <summary>
        /// adds empty Rows
        /// </summary>
        /// <param name="offset">old max value as Offset</param>
        private void addEmptyRows(int offset)
        {
            this.buildEmptyMatrix(0, offset);
        }

        /// <summary>
        /// build the matrix with empty values
        /// </summary>
        private void buildEmptyMatrix()
        {
            this.buildEmptyMatrix(0, 0);
        }

        /// <summary>
        /// Build part of the matrix from offset to max Count
        /// </summary>
        /// <param name="offsetX">start offset for Columns</param>
        /// <param name="offsetY">start offset for Rows</param>
        private void buildEmptyMatrix(int offsetX, int offsetY)
        {            
            for (int y = offsetY; y <= this.RowCount; y++)
            {
                List<Object> addX = new List<object>();
                List<String> addStr = new List<string>();
                for (int x = offsetX; x <= this.ColumnCount; x++)
                {
                    addX.Add(null);
                    addStr.Add(null);
                }
                this.DataMatrix.Add(addX);
                this.StringMatrix.Add(addStr);
            }
        }

    }
}
