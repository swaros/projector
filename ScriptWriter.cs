using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    public partial class ScriptWriter : Form
    {
        //  ReflectionScript script = new ReflectionScript();
        //  script.setCode(code);

        public const string SCRIPT_IDENT = "ReflectionScript";

        public Object execObject;

        private string filename = null;

        private string lastSaved = "";

        private int currentLineInEdit = 0;

        public String assignedExternalScript = null;

        ReflectionScript script = new ReflectionScript();

        ReflectionScriptHighLight Highlight;

        private Boolean autClose = false;

        private RtfColoring colorize;

        public ScriptWriter(Object targetObject, Boolean openFile)
        {                      
            initStuff(targetObject);

            if (openFile)
            {
                loadToolStripMenuItem_Click(null, null);
                this.autClose = true;
            }
            

        }

        public ScriptWriter(Object targetObject, string scriptText)
        {
            initStuff(targetObject);
            if (scriptText != null)
            {
                codeBox.Text = scriptText;
            }
            
            script.setCode(codeBox.Text);            

        }

        public ScriptWriter(Object targetObject)
        {
            initStuff(targetObject);

        }

        private void initStuff(Object targetObject)
        {
            InitializeComponent();
            mainSplitContainer.Panel1Collapsed = true;
            debugView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.execObject = targetObject;
            script.setCode(codeBox.Text);
            colorize = new RtfColoring(codeBox);
            Highlight = new ReflectionScriptHighLight(script, codeBox);
        }

        private void updateColors()
        {
            
            Highlight.reDraw(true);
        }

        private void codeBox_TextChanged(object sender, EventArgs e)
        {
            //script.setCode(codeBox.Text);
            //errorTextBox.Text = script.getErrors();
            
        }

        private void codeBox_KeyUp(object sender, KeyEventArgs e)
        {
            keyTrigger.Enabled = false;
            this.recheckScript();
        }

        private void refitElements()
        {
            int cursorPosition = codeBox.SelectionStart;
            this.currentLineInEdit = codeBox.GetLineFromCharIndex(cursorPosition);
            if (this.currentLineInEdit < debugView.Items.Count)
            {
                debugView.TopItem = debugView.Items[this.currentLineInEdit];
            }
        }

        private void recheckScript()
        {
            script.setCode(codeBox.Text);
            if (script.getErrorCount() == 0)
            {
                this.assignedExternalScript = codeBox.Text;
            }
           
            updateLogBook();
            refitElements();
        }


        private void updateLogBook()
        {
            // first look at spaces
            List<int> spaces = script.getEmptyLines();
            foreach (int emptyLineNr in spaces)
            {
                ListViewItem debugRow = new ListViewItem();
                debugRow.Text = emptyLineNr.ToString();
                debugRow.ForeColor = Color.Blue;
                debugRow.BackColor = Color.LightCyan;

                ListViewItem.ListViewSubItem emptyMessage = new ListViewItem.ListViewSubItem();
                emptyMessage.Text = "";

                debugRow.SubItems.Add(emptyMessage);

                updateLogBookRow(debugRow);
            }

            //print all errors
            List<ScriptErrors> errors = script.getAllErrors();
            foreach (ScriptErrors err in errors)
            {
                ListViewItem debugRow = new ListViewItem();
                debugRow.Text = err.lineNumber.ToString();
                debugRow.ForeColor = Color.DarkRed;
                debugRow.BackColor = Color.LightCoral;

                ListViewItem.ListViewSubItem errMessage = new ListViewItem.ListViewSubItem();
                errMessage.Text = err.errorMessage;

                debugRow.SubItems.Add(errMessage);

                updateLogBookRow(debugRow);
                //debugView.Items.Add(debugRow);
            }

            // get all code
            List<ReflectionScriptDefines> scriptInfo = script.getScript();
            foreach (ReflectionScriptDefines scriptLine in scriptInfo)
            {
                ListViewItem debugRow = new ListViewItem();
                debugRow.Text = scriptLine.lineNumber.ToString();
                debugRow.ForeColor = Color.DarkGreen;
                debugRow.BackColor = Color.LightYellow;
                debugRow.Tag = scriptLine;
                debugRow.Name = Projector.ScriptWriter.SCRIPT_IDENT;
                //debugRow.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);



                ListViewItem.ListViewSubItem scriptMessage = new ListViewItem.ListViewSubItem();
                scriptMessage.Text = script.getSrcInfo(scriptLine);

                debugRow.SubItems.Add(scriptMessage);

                updateLogBookRow(debugRow);

                // if some line frome source that used in this one command, the lineCount should be set
                if (scriptLine.lineCount > 1)
                {
                    for (int i = 1; i < scriptLine.lineCount; i++)
                    {
                        ListViewItem scriptBlock = new ListViewItem();
                        int shiftedNr = i + scriptLine.lineNumber;
                        scriptBlock.Text = shiftedNr.ToString();
                        scriptBlock.SubItems.Add("(CODE) depending on line " + scriptLine.lineNumber);
                        scriptBlock.ForeColor = Color.DarkGreen;
                        scriptBlock.BackColor = Color.LightYellow;
                        updateLogBookRow(scriptBlock);
                    }
                }

            }
            this.updateColors();
        }

        private void updateLogBookRow(ListViewItem row)
        {
            int posNumber = int.Parse(row.Text);
            if (debugView.Items.Count > posNumber)
            {
                if (posNumber == this.currentLineInEdit)
                {
                    row.Font =  new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
                }
                debugView.Items[posNumber] = row;
            }
            else
            {
                int diff = posNumber - debugView.Items.Count;
                for (int i = debugView.Items.Count; i <= posNumber; i++)
                {

                    ListViewItem empty = new ListViewItem(i.ToString());
                    ListViewItem.ListViewSubItem emptyMessage = new ListViewItem.ListViewSubItem();
                    emptyMessage.Text = "...";
                    empty.SubItems.Add(emptyMessage);
                    debugView.Items.Add(empty);
                    

                }
            }
        }


        private void executeScript()
        {
            this.Visible = false;
            this.Refresh();
            script.setCode(codeBox.Text);
            if (script.getErrorCount() == 0)
            {
                RefScriptExecute executer = new RefScriptExecute(script, this.execObject);
                executer.run();
            }
            this.Visible = true;
        }


        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            executeScript();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            executeScript();
        }

        private Boolean saveChanges()
        {
            if (this.filename != null && this.filename != "" && System.IO.File.Exists(this.filename))
            {
                System.IO.File.WriteAllText(this.filename, codeBox.Text);
                this.lastSaved = codeBox.Text;
                return true;
            }
            return false;
        }

        private Boolean checkChanges()
        {
            return (lastSaved != codeBox.Text);
        }

        private void checkBeforeOpen()
        {
            if (checkChanges() &&  MessageBox.Show("The Source Have changed. would you save this changes?", "Changed Source", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK )
            {
                saveChanges();
            }
        }


        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.checkBeforeOpen();

            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                codeBox.Text = System.IO.File.ReadAllText(openFile.FileName);
                this.lastSaved = codeBox.Text;
                this.filename = openFile.FileName;
                this.assignedExternalScript = codeBox.Text;
                
            }

            if (this.autClose)
            {
                this.Close();
                DialogResult = DialogResult.OK;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.filename = saveFileDialog.FileName;
                this.lastSaved = codeBox.Text;
                System.IO.File.WriteAllText(saveFileDialog.FileName, codeBox.Text);
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.saveChanges();
        }

        private void codeBox_VScroll(object sender, EventArgs e)
        {
            refitElements();
        }

        private void keyTrigger_Tick(object sender, EventArgs e)
        {
            refitElements();
        }

        private void codeBox_KeyDown(object sender, KeyEventArgs e)
        {
            keyTrigger.Enabled = true;
        }

        private void enlargeContent()
        {
            debugView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            
        }

        private void debugView_ItemActivate(object sender, EventArgs e)
        {
            debugView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

            foreach (ListViewItem lItem in debugView.SelectedItems)
            {
                if (lItem.Name == Projector.ScriptWriter.SCRIPT_IDENT)
                {
                    ReflectionScriptDefines assignedScript = (ReflectionScriptDefines)lItem.Tag;
                    errorTextBox.Text = script.dump(assignedScript);                                       

                }
            }
        }

        private void codeBox_MouseUp(object sender, MouseEventArgs e)
        {
            refitElements();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            loadToolStripMenuItem_Click(sender, e);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.saveChanges();
        }
    }
}
