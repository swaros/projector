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


        private RtfColoring colorize;

        public ScriptWriter(Object targetObject, Boolean openFile)
        {                      
            initStuff(targetObject);

            if (openFile)
            {
                loadToolStripMenuItem_Click(null, null);            
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
            messageSplit.Panel1Collapsed = true;
            codeSplitContainer.Panel2Collapsed = true;
            statusLabel.Text = "no file";
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
            refreshTimer.Enabled = true;
            
            
        }

        private void codeBox_KeyUp(object sender, KeyEventArgs e)
        {
            keyTrigger.Enabled = false;
            //            refreshTimer.Enabled = true;
            //this.recheckScript();
        }

        private void refitElements()
        {
            if (!codeSplitContainer.Panel2Collapsed)
            {
                int cursorPosition = codeBox.SelectionStart;
                this.currentLineInEdit = codeBox.GetLineFromCharIndex(cursorPosition);
                if (this.currentLineInEdit < debugView.Items.Count)
                {
                    debugView.TopItem = debugView.Items[this.currentLineInEdit];
                }
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
            
        }

        private void updateLogBookRow(ListViewItem row)
        {
            if (!codeSplitContainer.Panel2Collapsed)
            {
                int posNumber = int.Parse(row.Text);
                if (debugView.Items.Count > posNumber)
                {
                    if (posNumber == this.currentLineInEdit)
                    {
                        row.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
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
        }


        private void executeScript()
        {


            

            script.setCode(codeBox.Text);
            if (script.getErrorCount() == 0)
            {

                errorLabels.Text = "Start exec";
                errorLabels.ForeColor = Color.LightBlue;
                errorLabels.BackColor = Color.DarkBlue;
                errorLabels.ToolTipText = "";
                Application.DoEvents();

                RefScriptExecute executer = new RefScriptExecute(script, this.execObject);
                Boolean succeed = executer.run();

                if (succeed == false)
                {
                    updateColors();
                    Application.DoEvents();
                    string errorMessages = script.getErrors();

                    errorLabels.Text = script.getErrorCount() + "Execution errors: " + script.getErrors().Replace("\n", "").Substring(0, 20);
                    errorLabels.ForeColor = Color.Red;
                    errorLabels.BackColor = Color.DarkOrange;
                    errorLabels.ToolTipText = script.getErrors();


                }
                else
                {
                    errorLabels.Text = "executed without errors";
                    errorLabels.ForeColor = Color.LightGreen;
                    errorLabels.BackColor = Color.DarkGreen;
                }

            }
            else
            {
                string errorMessages = script.getErrors();

                errorLabels.Text = script.getErrorCount() + "parsing errors: " + script.getErrors().Replace("\n", "").Substring(0, 20);
                errorLabels.ForeColor = Color.Red;
                errorLabels.BackColor = Color.LightGoldenrodYellow;
                errorLabels.ToolTipText = script.getErrors();
            }

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
            else
            {
                saveToolStripMenuItem_Click(null, null);
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
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes )
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
                this.recheckScript();
                this.updateColors();
                statusLabel.Text = this.filename;
                
            }
           
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.filename = saveFileDialog.FileName;
                this.lastSaved = codeBox.Text;
                System.IO.File.WriteAllText(saveFileDialog.FileName, codeBox.Text);
                statusLabel.Text = this.filename;
                
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.saveChanges();
        }

        private void codeBox_VScroll(object sender, EventArgs e)
        {
            refreshTimer.Enabled = false;
            refitElements();
        }

        private void keyTrigger_Tick(object sender, EventArgs e)
        {
            refitElements();
           
        }

        private void resetRedrawTick()
        {
            refreshTimer.Enabled = false;
            refreshTimer.Enabled = true;
        }

        private void codeBox_KeyDown(object sender, KeyEventArgs e)
        {
            keyTrigger.Enabled = true;
            resetRedrawTick();
        }

        private void enlargeContent()
        {
            debugView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            
        }

        private void debugView_ItemActivate(object sender, EventArgs e)
        {
            debugView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            messageSplit.Panel1Collapsed = true;
            foreach (ListViewItem lItem in debugView.SelectedItems)
            {
                if (lItem.Name == Projector.ScriptWriter.SCRIPT_IDENT)
                {
                    ReflectionScriptDefines assignedScript = (ReflectionScriptDefines)lItem.Tag;
                    errorTextBox.Text = script.dump(assignedScript);
                    messageSplit.Panel1Collapsed = false;
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

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            this.recheckScript();
            this.updateColors();
            refreshTimer.Enabled = false;
        }

        private void showDebug_Click(object sender, EventArgs e)
        {
            codeSplitContainer.Panel2Collapsed = !showDebug.Checked;
            if (codeSplitContainer.Panel2Collapsed)
            {
                this.recheckScript();
                this.updateColors();
                resetRedrawTick();
            }
        }

        private void errorLabels_Click(object sender, EventArgs e)
        {
            if (errorLabels.ToolTipText.Length > 15)
            {
                MessageBox.Show(errorLabels.ToolTipText,
                    "Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void ScriptWriter_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkBeforeOpen();
        }
    }
}
