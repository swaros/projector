using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
//using Microsoft.Office.Interop.Excel;

namespace Projector
{
    class ListViewExport
    {
        /*
        #region Private Members
        private ListView listview;
        private string title;
        private double onePoint = 0.13;
        private string[] columnLetters;

        private List<ListView> addedListViews = new List<ListView>();
        private List<string> addedlabesl = new List<string>();

        // Excel object references.
        private Microsoft.Office.Interop.Excel.Application objExcel = null;
        private Microsoft.Office.Interop.Excel.Workbooks objBooks = null;
        private Microsoft.Office.Interop.Excel._Workbook objBook = null;
        private Microsoft.Office.Interop.Excel.Sheets objSheets = null;
        private Microsoft.Office.Interop.Excel._Worksheet objSheet = null;
        private Microsoft.Office.Interop.Excel.Range objRange = null;

        // Frequenty-used variable for optional arguments.
        private object objOpt = System.Reflection.Missing.Value;
        #endregion

        public ListViewExport(string Title)
        {
            //this.listview = Listview;
            this.title = Title;      
            
        }

        public void assignListView(ListView listv)
        {
            this.listview = listv;
        }


        #region Public Methods

        public bool addListView(ListView lview,string label)
        {
            if (lview.Columns.Count >= this.listview.Columns.Count)
            {
                this.addedListViews.Add(lview);
                this.addedlabesl.Add(label);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GenerateDynamicExcelSheet()
        {
            if (this.listview != null)
            {
                try
                {
                    // Show SaveFileDialog
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "In Excel exportieren";
                    sfd.DefaultExt = "*.xls";
                    sfd.RestoreDirectory = true;
                    sfd.AddExtension = true;
                    sfd.Filter = "Excel Sheets|*.xls";
                    DialogResult dr = sfd.ShowDialog();
                    if (dr == DialogResult.Cancel)
                    {
                        MessageBox.Show("return");
                        return;
                    }
                    string path = sfd.FileName;

                    // Start a new workbook in Excel.
                    objExcel = new Microsoft.Office.Interop.Excel.Application();
                    objBooks = (Microsoft.Office.Interop.Excel.Workbooks)objExcel.Workbooks;
                    objBook = (Microsoft.Office.Interop.Excel._Workbook)(objBooks.Add(objOpt));
                    objSheets = (Microsoft.Office.Interop.Excel.Sheets)objBook.Worksheets;
                    objSheet = (Microsoft.Office.Interop.Excel._Worksheet)(objSheets.get_Item(1));

                    // Determine column count
                    int columncount = 0;
                    object[] objHeaders = new object[this.listview.Columns.Count];

                    columnLetters = new string[this.listview.Columns.Count];

                    int zaehler = -1;
                    int SpaltenCounter = 0;
                    for (int c = 0; c < this.listview.Columns.Count; c++)
                    {
                        string Spalte = "";
                        Spalte = Convert.ToChar(65 + SpaltenCounter).ToString();
                        SpaltenCounter++;
                        if (zaehler >= 0)
                        {
                            Spalte = Convert.ToChar(65 + zaehler).ToString() + Spalte;
                        }
                        columnLetters[c] = Spalte;
                        if (SpaltenCounter == 26)
                        {
                            //wenn bei Z angekommen Zähler erhöhen
                            zaehler++;
                            SpaltenCounter = 0;
                        }
                    }


                    foreach (ColumnHeader ch in this.listview.Columns)
                    {
                        objHeaders[columncount] = ch.Text;
                        columncount++;
                    }
                    columncount--;

                    // Merge title cells
                    objRange = objSheet.get_Range("A1", this.columnLetters[columncount] + "1");
                    objRange.MergeCells = true;
                    objRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Set table title and format it
                    objRange = objSheet.get_Range("A1", "A1");
                    objRange.set_Value(objOpt, this.title);
                    objRange.Font.Size = "14";
                    objRange.Font.Name = "Arial";
                    objRange.Font.Bold = true;

                    // Format header cells
                    objRange = objSheet.get_Range("A3", this.columnLetters[columncount] + "3");
                    objRange.set_Value(objOpt, objHeaders);
                    objRange.Font.Bold = true;
                    objRange.Font.Italic = true;
                    objRange.Font.Name = "Arial";

                    // Fill Cells with the listview data                
                    int i = 4;
                    foreach (ListViewItem item in this.listview.Items)
                    {
                        for (int t = 0; t <= columncount; t++)
                        {
                            objRange = objSheet.get_Range(this.columnLetters[t] + i.ToString(), this.columnLetters[t] + i.ToString());
                            objRange.set_Value(objOpt, item.SubItems[t].Text);
                            objRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlLeft;
                            objRange.Font.Size = "8";
                            objRange.Font.Name = "Arial";

                            objRange.Interior.Color = item.BackColor.ToArgb();

                        }
                        i++;
                    }

                    // if existsing other listviews ..add this

                    if (addedListViews.Count > 0)
                    {
                        for (int mt = 0; mt < addedListViews.Count; mt++)
                        {
                            //i ++;
                            //objRange = objSheet.get_Range(this.columnLetters[0] + i.ToString(), this.columnLetters[0] + i.ToString());
                            //objRange.set_Value(objOpt, addedlabesl[mt]);
                            //objRange.Font.Size = "12";
                            //objRange.Font.Name = "Arial";
                            //i++;
                            foreach (ListViewItem item in addedListViews[mt].Items)
                            {

                                for (int t = 0; t <= columncount; t++)
                                {
                                    objRange = objSheet.get_Range(this.columnLetters[t] + i.ToString(), this.columnLetters[t] + i.ToString());
                                    if (item.SubItems[t] != null) objRange.set_Value(objOpt, item.SubItems[t].Text);
                                    else objRange.set_Value(objOpt, "VALUE NOT FOUND");
                                    objRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlLeft;
                                    objRange.Font.Size = "8";
                                    objRange.Font.Name = "Arial";

                                    objRange.Interior.Color = item.BackColor.ToArgb();

                                }
                                i++;
                                Application.DoEvents();
                            }
                        }
                    }


                    // Set Column widths
                    int x = 0;
                    foreach (ColumnHeader ch in this.listview.Columns)
                    {
                        objRange = objSheet.get_Range(this.columnLetters[x] + "1", this.columnLetters[x] + "1");
                        objRange.Columns.ColumnWidth = convertPixelToPoint(ch.Width);
                        x++;
                    }

                    // Set table borders
                    int j = i - 1;
                    objRange = objSheet.get_Range("A3", this.columnLetters[columncount] + j.ToString());
                    objRange.Borders.Value = Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal;
                    objRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    objRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    // Set title border
                    objRange = objSheet.get_Range("A3", this.columnLetters[columncount] + "3");
                    objRange.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 001001001);

                    // Save the workbook and quit Excel.
                    objBook.SaveAs(path, objOpt, objOpt,
                        objOpt, objOpt, objOpt, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                        objOpt, objOpt, objOpt, objOpt, objOpt);
                    objBook.Close(false, objOpt, objOpt);
                    objExcel.Quit();

                    MessageBox.Show("Export Done");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Es trat ein Fehler beim Speichern auf!\n\n" + ex.ToString(), "In Excel exportieren", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Private Methods
        private double convertPixelToPoint(int Pixel)
        {
            return this.onePoint * Convert.ToDouble(Pixel + 35);
        }
        #endregion
         */
    }
}
