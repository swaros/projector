using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using Projector.Data;
using Projector.Script;

namespace Projector
{
    /// <summary>
    /// List Controll usable for reflectionscripts.
    /// 
    /// 
    /// </summary>
    public partial class ReflectList : UserControl
    {


        private ReflectionScript onSelectScript;
        private ReflectionScript startIterateScript;
        private ReflectionScript endIterateScript;
        private ReflectionScript OnErrorScript;
        private String errorScriptVar;
        private Color defaultDisplayColor;
        private string displayText = "No Name";

        int columnAutoSizeMode = 0;

        private Boolean abortCmd = false;

        public ReflectList()
        {
            InitializeComponent();
            this.defaultDisplayColor = this.stateLabel.ForeColor;
            this.Progress.Visible = false;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            exportCsv();
        }

        public void Abort()
        {
            this.abortCmd = true;
        }


        private Boolean ifAbort() 
        {
            Boolean ab = this.abortCmd;
            this.abortCmd = false;
            return ab;
        }
        public void setText(string label)
        {
            this.displayText = label;
            this.stateLabel.Text = label;
            
        }

        private void resetlabel()
        {
            this.stateLabel.ForeColor = this.defaultDisplayColor;
            this.stateLabel.Text = this.displayText;
            this.Progress.Value = 0;
            this.Progress.Visible = false;
        }

        public void setVisible(Boolean onoff)
        {
            this.Visible = onoff;
        }

        private void setStatusMsg(string msg)
        {
            this.setStatusMsg(msg, Color.Blue);
        }

        private void setStatusMsg(string msg, Color displayColor)
        {
            this.stateLabel.ForeColor = displayColor;
            this.stateLabel.Text = msg;
        }

        public void setListView(ListView list)
        {
            if (list != null)
            {
                ListViewWorker Worker = new ListViewWorker();
                Worker.copyListView(list, this.listView);
            }
        }

        public void setResultList(ResultList rList)
        {
            this.listView.Clear();
            // add header first
            foreach (string header in rList.getColumns())
            {
                this.listView.Columns.Add(header);
            }

            // adding content
            for (int rowNr = 0; rowNr < rList.getRowCount(); rowNr++)
            {
                ListViewItem lwAdd = new ListViewItem(rList.getRowStringList(rowNr));
                this.listView.Items.Add(lwAdd);                 
            }
        }



        private void autosortColumns()
        {
            for (int i = 0; i < listView.Columns.Count; i++)
            {
                switch (columnAutoSizeMode)
                {
                    case 0:
                        listView.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                        break;

                    case 1:
                        listView.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                        break;

                    default:
                        listView.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.None);
                        break;

                }
            }
        }

        public ListView getListView()
        {
            ListView list = new ListView();
            ListViewWorker Worker = new ListViewWorker();
            Worker.copyListView(this.listView, list);
            return list;
        }

        public void joinFields(string left, string right, string newName, string between)
        {
            Stopwatch watch = new Stopwatch();
            this.setStatusMsg("Joining...");
            

            this.listView.Columns.Add(newName);
            int newCnt = listView.Columns.Count - 2;
            int rPos = -1;
            int lPos = -1;
            for (int a = 0; a < listView.Columns.Count; a++)
            {
                if (listView.Columns[a].Text == right)
                {
                    rPos = a;
                }
                if (listView.Columns[a].Text == left)
                {
                    lPos = a;
                }
            }

            if (lPos > -1 && rPos > -1)
            {
                watch.Start();
                for (int i = 0; i < this.listView.Items.Count; i++)
                {
                    string rText = this.listView.Items[i].SubItems[rPos].Text;
                    string lText = this.listView.Items[i].SubItems[lPos].Text;
                    string newText = lText + between + rText;
                    this.listView.Items[i].SubItems.Add(newText);
                    if (watch.ElapsedMilliseconds > 1000)
                    {
                        this.Progress.Visible = true;
                        this.Progress.Maximum = this.listView.Items.Count;
                        this.Progress.Value = i;
                        this.listView.TopItem = this.listView.Items[i];
                        this.listView.Update();
                        this.setStatusMsg("Joining..." + i + "/" + this.listView.Items.Count); 
                        Application.DoEvents();
                        if (ifAbort())
                        {
                            resetlabel();
                            return;
                        }
                        watch.Restart();
                    }
                }
            }
            this.resetlabel();
        }

        public void addFieldFrom(string left, string newName, string leftAdd, string rightAdd)
        {
            Stopwatch watch = new Stopwatch();
            this.setStatusMsg("Joining...");


            this.listView.Columns.Add(newName);
            int newCnt = listView.Columns.Count - 2;
            int lPos = -1;
            for (int a = 0; a < listView.Columns.Count; a++)
            {

                if (listView.Columns[a].Text == left)
                {
                    lPos = a;
                }
            }

            if (lPos > -1)
            {
                watch.Start();
                for (int i = 0; i < this.listView.Items.Count; i++)
                {                    
                    string lText = this.listView.Items[i].SubItems[lPos].Text;
                    string newText = leftAdd + lText + rightAdd;
                    this.listView.Items[i].SubItems.Add(newText);
                    if (watch.ElapsedMilliseconds > 1000)
                    {
                        this.Progress.Visible = true;
                        this.Progress.Maximum = this.listView.Items.Count;
                        this.Progress.Value = i;
                        this.listView.TopItem = this.listView.Items[i];
                        this.listView.Update();
                        this.setStatusMsg("adding..." + i + "/" + this.listView.Items.Count);
                        Application.DoEvents();
                        if (ifAbort())
                        {
                            resetlabel();
                            return;
                        }
                        watch.Restart();
                    }
                }
            }
            this.resetlabel();
        }

        public void removeIfEqual(string left, string checkStr, Boolean alsoNotExist)
        {
            Stopwatch watch = new Stopwatch();
          
            this.setStatusMsg("Removing...");

            int startCount = this.listView.Items.Count;
            int removed = 0;
           // this.listView.Visible = false;
            int lPos = -1;
            int redrawTick = 0;
            for (int a = 0; a < listView.Columns.Count; a++)
            {

                if (listView.Columns[a].Text == left)
                {
                    lPos = a;
                }
            }

            if (lPos > -1)
            {
                watch.Start();
   
                for (int i = this.listView.Items.Count-1; i >= 0; i--)
                {
                    if (watch.ElapsedMilliseconds > 2000)
                    {
                        this.Progress.Visible = true;
                        this.Progress.Maximum = startCount;
                        this.Progress.Value = startCount - i;
                        this.setStatusMsg("Deleted:" + removed + " rows left:" + this.listView.Items.Count + " check Row:" + (startCount - i));
                        Application.DoEvents();
                        redrawTick++;
                        if (redrawTick > 10)
                        {
                            this.listView.TopItem = this.listView.Items[i];
                            this.listView.Update();
                            redrawTick = 0;
                            if (ifAbort())
                            {
                                resetlabel();
                                return;
                            }
       
                        }
                        watch.Restart();
                    }

                    if (listView.Items[i].SubItems.Count > lPos)
                    {
                        string lText = this.listView.Items[i].SubItems[lPos].Text;

                        
                        if (lText == checkStr)
                        {
                            this.listView.Items[i].Remove();
                            removed++;
                        }
                    }
                    else
                    {
                        if (alsoNotExist)
                        {
                            this.listView.Items[i].Remove();
                            removed++;
                        }
                    }
                }
            }
            this.listView.Visible = true;
            this.resetlabel();
        }

        public void mark(string fieldname, string value)
        {
            Stopwatch watch = new Stopwatch();
            this.setStatusMsg("Mark:");
            int lPos = -1;
            for (int a = 0; a < listView.Columns.Count; a++)
            {

                if (listView.Columns[a].Text == fieldname)
                {
                    lPos = a;
                }
            }

            if (lPos > -1)
            {
                watch.Start();
                for (int i = 0; i < this.listView.Items.Count; i++)
                {

                    string lText = this.listView.Items[i].SubItems[lPos].Text;
                    if (lText == value)
                    {
                        this.listView.TopItem = this.listView.Items[i];
                        this.listView.Items[i].Selected = true;
                        this.listView.Update();
                        Application.DoEvents();
                        this.resetlabel();
                        return;
                    }
                    if (watch.ElapsedMilliseconds > 1000)
                    {
                        this.Progress.Visible = true;
                        this.Progress.Maximum = this.listView.Items.Count;
                        this.Progress.Value = i;
                        this.listView.TopItem = this.listView.Items[i];
                        this.setStatusMsg("Search..." + i + "/" + this.listView.Items.Count);
                        this.listView.Update();
                        Application.DoEvents();
                        if (ifAbort())
                        {
                            resetlabel();
                            return;
                        }
                        watch.Restart();
                    }
                }
            }
            this.resetlabel();
        }


        private string getSplitStr(String source, string splitter, int pos)
        {
            string[] newTexts = source.Split(splitter.ToCharArray());

            if (pos < 0)
            {
                pos = 0;
            }

            if (pos >= newTexts.Count())
            {
                pos = newTexts.Count() - 1;
            }

            return newTexts[pos];
        }

        public void split(string fieldname, string splitChars)
        {
            this.split(fieldname, splitChars, 0);
        }

        public void splitByPos(string fieldname, string splitChars, int pos)
        {
            this.split(fieldname, splitChars, pos);
        }

        private void split(string fieldname, string splitChars, int pos)
        {
            Stopwatch watch = new Stopwatch();
            this.setStatusMsg("Splitting....");
            int lPos = -1;
            for (int a = 0; a < listView.Columns.Count; a++)
            {

                if (listView.Columns[a].Text == fieldname)
                {
                    lPos = a;
                }
            }

            if (lPos > -1)
            {
                watch.Start();
                for (int i = 0; i < this.listView.Items.Count; i++)
                {

                    string lText = this.listView.Items[i].SubItems[lPos].Text;
                    string spltStr = this.getSplitStr(lText, splitChars, pos);
                    
                    this.listView.Items[i].SubItems[lPos].Text = spltStr;
                    if (watch.ElapsedMilliseconds > 1000)
                    {
                        this.Progress.Visible = true;
                        this.Progress.Maximum = this.listView.Items.Count;
                        this.Progress.Value = i;
                        this.setStatusMsg("Splitting..." + i + "/" + this.listView.Items.Count);

                        this.listView.TopItem = this.listView.Items[i];
                        this.listView.Update();
                        Application.DoEvents();
                        if (ifAbort())
                        {
                            resetlabel();
                            return;
                        }
                        watch.Restart();
                    }
                }
            }
            resetlabel();
        }


        public void getBetween(string fieldname, string leftStart, string  rightEnd)
        {
            Stopwatch watch = new Stopwatch();
            RString RStr = new RString();
            this.setStatusMsg("Splitting....");
            int lPos = -1;
            for (int a = 0; a < listView.Columns.Count; a++)
            {

                if (listView.Columns[a].Text == fieldname)
                {
                    lPos = a;
                }
            }

            if (lPos > -1)
            {
                watch.Start();
                for (int i = 0; i < this.listView.Items.Count; i++)
                {

                    string lText = this.listView.Items[i].SubItems[lPos].Text;
                    RStr.setString(lText);
                    string spltStr = RStr.between(leftStart, rightEnd);

                    this.listView.Items[i].SubItems[lPos].Text = spltStr;
                    if (watch.ElapsedMilliseconds > 1000)
                    {
                        this.Progress.Visible = true;
                        this.Progress.Maximum = this.listView.Items.Count;
                        this.Progress.Value = i;
                        this.setStatusMsg("Between..." + i + "/" + this.listView.Items.Count);

                        this.listView.TopItem = this.listView.Items[i];
                        this.listView.Update();
                        Application.DoEvents();
                        if (ifAbort())
                        {
                            resetlabel();
                            return;
                        }
                        watch.Restart();
                    }
                }
            }
            resetlabel();
        }


        public void firstWord(string fieldname)
        {
            split(fieldname, " ");

        }

        public void unionListView(ListView adList)
        {
            if (this.listView.Items.Count == 0)
            {
                this.setListView(adList);
                return;
            }

            if (this.listView.Columns.Count == adList.Columns.Count)
            {
                foreach (ListViewItem adItem in adList.Items)
                {
                    ListViewItem cloneList = (ListViewItem) adItem.Clone();
                    this.listView.Items.Add(cloneList);

                    if (ifAbort())
                    {
                        resetlabel();
                        return;
                    }
                }
            }

        }

        public void joinListView(string onJoin,ListView list)
        {
            if (list != null)
            {
                ListViewWorker Worker = new ListViewWorker();
                Worker.mergeListView(onJoin, list, this.listView);
            }
        }

        public void fillUpListView(string onJoin, ListView list)
        {
            if (list != null)
            {
                ListViewWorker Worker = new ListViewWorker();
                Worker.fillListView (onJoin, list, this.listView);
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

        public void OnStartIterate(ReflectionScript refl)
        {
            this.startIterateScript = refl;
        }

        public void OnDoneIterate(ReflectionScript refl)
        {
            this.endIterateScript = refl;
        }

        public void OnError(string errorVar, ReflectionScript refl)
        {
            this.errorScriptVar = errorVar;
            this.OnErrorScript = refl;
        }

        private void execOnError(string message)
        {
            if (this.OnErrorScript != null)
            {
                this.OnErrorScript.createOrUpdateStringVar("&" + this.errorScriptVar, message);
                RefScriptExecute exec = new RefScriptExecute(this.OnErrorScript, this);
                exec.run();
            }
        }

        private void execOnDone()
        {
            if (this.endIterateScript != null)
            {
                RefScriptExecute exec = new RefScriptExecute(this.endIterateScript, this);
                exec.run();
            }
        }

        private void execOnStart()
        {
            if (this.startIterateScript != null)
            {
                RefScriptExecute exec = new RefScriptExecute(this.startIterateScript, this);
                exec.run();
            }
        }

        public void Iterate()
        {
            this.execOnStart();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            setStatusMsg("Iterating ...", Color.DarkGreen);
            int i = 0;
            this.Progress.Visible = true;
            this.Progress.Maximum = this.listView.Items.Count;
            
            foreach (ListViewItem lItem in this.listView.Items){
                i++;
                if (watch.ElapsedMilliseconds > 500)
                {
                    this.Progress.Value = i;
                    this.setStatusMsg("Iterating..." + i + "/" + this.listView.Items.Count, Color.DarkGreen);
                    this.listView.TopItem = lItem;
                    this.listView.Update();
                    Application.DoEvents();
                    if (ifAbort())
                    {
                        this.execOnDone();
                        resetlabel();
                        return;
                    }
                    watch.Restart();
                }
                intIterate(lItem);
            }
            resetlabel();
            this.execOnDone();
        }

        private void intIterate(ListViewItem lItem)
        {
            if (this.onSelectScript != null)
            {
               
                int number = 0;
                foreach (ListViewItem.ListViewSubItem lwItem in lItem.SubItems)
                {
                    string headName = this.listView.Columns[number].Text;
                    string[] solits = headName.Split(' ');
                    string varName = solits[0];
                    this.onSelectScript.createOrUpdateStringVar("&" + varName, lwItem.Text);
                    number++;
                    this.onSelectScript.createOrUpdateStringVar("&ROW." + number, lwItem.Text);
                }
                RefScriptExecute executer = new RefScriptExecute(this.onSelectScript, this);
                executer.run();
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

        public void saveAsCsvDlg(string filename)
        {
            saveFileDialog.FileName = filename;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.exportCsvToFile(saveFileDialog.FileName);
            }
        }

        private void exportCsv()
        {            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.exportCsvToFile(saveFileDialog.FileName);
            }
        }

        public void getListFromDatabase(ReflectionDatabase db)
        {
            List<Hashtable> dbRes = db.lastResult;
            if (dbRes != null)
            {
                Boolean first = true;
                this.listView.Items.Clear();
                foreach (Hashtable row in dbRes)
                {
                    int rowNr = 0;
                    ListViewItem adLwItm = new ListViewItem();
                    foreach (DictionaryEntry line in row){

                        if (first){
                            this.listView.Columns.Add(line.Key.ToString());
                        }
                        string val = "";
                        if (line.Value != null){
                            val = line.Value.ToString();
                        }
                        if (rowNr == 0)
                        {
                            adLwItm.Text = val;
                        }
                        else
                        {
                            adLwItm.SubItems.Add(val);
                        }
                        rowNr++;
                        
                    }
                    this.listView.Items.Add(adLwItm);
                    first = false;
                }
            }
        }

        private void exportCsvToFile(string filename)
        {
            ListViewWorker Worker = new ListViewWorker();
            this.setStatusMsg("Exporting to " + filename + " please wait");
            string csvContent = Worker.exportCsv(this.listView);            
            System.IO.File.WriteAllText(filename, csvContent);
            resetlabel();
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.onSelectScript != null)
            {
                if (this.listView.SelectedItems.Count > 0)
                {
                    int number = 0;
                    foreach (ListViewItem.ListViewSubItem lwItem in this.listView.SelectedItems[0].SubItems)
                    {
                        if (number < this.listView.Columns.Count)
                        {
                            string headName = this.listView.Columns[number].Text;
                            string[] solits = headName.Split(' ');
                            string varName = solits[0];
                            this.onSelectScript.createOrUpdateStringVar("&" + varName, lwItem.Text);
                            number++;
                            this.onSelectScript.createOrUpdateStringVar("&ROW." + number, lwItem.Text);
                        }
                    }
                    RefScriptExecute executer = new RefScriptExecute(this.onSelectScript, this);
                    executer.run();
                }
            }
        }

        public void readCsv(string filename)
        {
            this.openCsvFile(filename);
        }

        public void readCsvDlg(string filename)
        {
            fileLoader_Click(null, null);
        }

        private void openCsvFile(string filename)
        {
            CsvReader reader = new CsvReader();
            if (reader.loadFile(filename))
            {
                this.setListView (this.getListViewFromCsv(reader));
            }
            else
            {
                foreach (string msg in reader.getErrors())
                    this.execOnError(msg);
            }
        }

        private ListView getListViewFromCsv(CsvReader csv){
            ListView tmpList = new ListView();

            List<string> header = csv.getHeader();
            foreach (string headText in header)
            {
                tmpList.Columns.Add(headText);
            }

            int rowCnt = csv.getRowCount();
            int colCnt = csv.getColumnCount();

            for (int getRow = 0; getRow <= rowCnt; getRow++){
                ListViewItem addThis = new ListViewItem();

                for (int getCol = 0; getCol <= colCnt; getCol++)
                {
                    CsvContent getData = csv.getContent(getRow, getCol);
                    string addval = "";
                    if (getData != null)
                        addval = getData.Text;
                    if (getCol == 0)
                    {

                        addThis.Text = addval;
                    }
                    else
                    {
                        addThis.SubItems.Add(addval);
                    }

                }
                tmpList.Items.Add(addThis);
            }

            return tmpList;
        }


        private void fileLoader_Click(object sender, EventArgs e)
        {
            if (openFilesDlg.ShowDialog() == DialogResult.OK)
            {
                openCsvFile(openFilesDlg.FileName);
            }
        }

        public void AutoArangeColumn()
        {
            autoSort_Click(null, null);
        }

        private void autoSort_Click(object sender, EventArgs e)
        {
            columnAutoSizeMode++;
            if (columnAutoSizeMode > 1) columnAutoSizeMode = 0;
            autosortColumns();
        }
    }
}
