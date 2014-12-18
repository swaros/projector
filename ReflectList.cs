using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    public partial class ReflectList : UserControl
    {


        private ReflectionScript onSelectScript;

        public ReflectList()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            exportCsv();
        }

        public void setListView(ListView list)
        {
            if (list != null)
            {
                ListViewWorker Worker = new ListViewWorker();
                Worker.copyListView(list, this.listView);
            }
        }

        public void setTop(int val){
            this.Top = val;
        }

        public void setLeft(int val)
        {
            this.Left = val;
        }

        public void setWidth(int val)
        {
            this.Width = val;
        }

        public void setHeight(int val)
        {
            this.Height = val;
        }

        public string getEntry(string entry, int rownumber)
        {
            ListViewWorker Worker = new ListViewWorker();
            return Worker.getEntry(listView, entry, rownumber);
        }

        public string getSelectedRowEntry(string entry)
        {
            if (listView.SelectedItems.Count == 1)
            {
                ListViewWorker Worker = new ListViewWorker();
                return Worker.getEntry(listView, entry, listView.SelectedIndices[0]);
            }
            return "";
        }

        public void Iterate()
        {
            foreach (ListViewItem lItem in this.listView.Items){
                lItem.Selected = true;
            }
        }

        public void OnSelectItem(ReflectionScript refScript)
        {
            this.onSelectScript = refScript;
        }

        public void saveAsCsv(string filename)
        {
            this.exportCsvToFile(filename);
        }

        private void exportCsv()
        {            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.exportCsvToFile(saveFileDialog.FileName);
            }
        }

        private void exportCsvToFile(string filename)
        {
            ListViewWorker Worker = new ListViewWorker();
            string csvContent = Worker.exportCsv(this.listView);

            System.IO.File.WriteAllText(filename, csvContent);
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.onSelectScript != null)
            {
                RefScriptExecute executer = new RefScriptExecute(this.onSelectScript, this);
                executer.run();
            }
        }
    }
}
