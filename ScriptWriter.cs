using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Projector.Script;
using System.Reflection;

namespace Projector
{
    public partial class ScriptWriter : Form
    {
        //  ReflectionScript script = new ReflectionScript();
        //  script.setCode(code);

        public const string SCRIPT_IDENT = "ReflectionScript";

        private const int TREE_METHOD_IMG_IDENT = 4;
        private const int TREE_OBJECT_IMG_IDENT = 2;
        private const int TREE_INSTANCE_IMG_IDENT = 3;
        private const int TREE_INT_IMG_IDENT = 6;
        private const int TREE_STRING_IMG_IDENT = 7;
        private const int TREE_BOOL_IMG_IDENT = 8;


        public Object execObject;
        private int lastLineUpdate = -1;
        private string filename = null;

        private string lastSaved = "";

        private int currentLineInEdit = 0;

        public String assignedExternalScript = null;

        ReflectionScript script = new ReflectionScript();

        ReflectionScriptHighLight Highlight;

        private AutoCompletion AutoComplete;

        //public RichBox codingBox = new RichBox();

        Boolean isRunning = false;

        private Boolean showObjectListing = true;

        private RtfColoring colorize;

        RefScriptExecute executer;

        SearchReplace searchReplaceDlg = new SearchReplace();

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
                codingBox.Text = scriptText;
            }
            
            script.setCode(codingBox.Text);            

        }

        public ScriptWriter(Object targetObject)
        {
            initStuff(targetObject);

        }

        private void initStuff(Object targetObject)
        {
            
            InitializeComponent();
            /*
            codeSplitContainer.Panel1.Controls.Add(codingBox);
            codingBox.Dock = DockStyle.Fill;
            */
            codingBox.KeyDown += new KeyEventHandler(codingBox_KeyDown);
            codingBox.KeyUp += new KeyEventHandler(codingBox_KeyUp);
            codingBox.TextChanged += new EventHandler(codingBox_TextChanged);
            codingBox.MouseUp += new MouseEventHandler(codingBox_MouseUp);
            codingBox.VScroll += new EventHandler(codingBox_Vscroll);
            codingBox.TabStop = false;
            codingBox.AcceptsTab = true;
            codingBox.WordWrap = false;

            messageSplit.Panel1Collapsed = true;
            codeSplitContainer.Panel2Collapsed = true;
            statusLabel.Text = "no file";
            mainSplitContainer.Panel1Collapsed = true;
            debugView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.execObject = targetObject;
            script.setCode(codingBox.Text);
            colorize = new RtfColoring(codingBox);
            Highlight = new ReflectionScriptHighLight(script, codingBox);
            this.AutoComplete = new AutoCompletion(codingBox);
            this.AutoComplete.assignListBox(wordListing);
            this.debugToolStrip.Visible = false;

            this.updateWidgets();
        }

        private void updateWidgets()
        {
            List<ReflectNewWidget> widgets = this.script.getAllWidgets();
            foreach (ReflectNewWidget wid in widgets)
            {
                ScriptComponent tmpComp = new ScriptComponent();
                tmpComp.assignWidget(wid);
                tmpComp.assignScript(this.codingBox);
                this.addItems.Controls.Add(tmpComp);
            }
        }

        private void updateColors(int markLine)
        {
            Highlight.markLine = markLine;
            Highlight.setWordMode(switchDrawMode.Checked);
            Highlight.reDraw(false);
        }

        private void updateColors()
        {
            if (!runButton.Enabled)
            {
                return;
            }
            Highlight.markLine = -1;
            int cursorPosition = codingBox.SelectionStart;
            this.currentLineInEdit = codingBox.GetLineFromCharIndex(cursorPosition);

            int startLn = this.currentLineInEdit - 1;
            if (startLn < 0)
            {
                startLn = 0;
            }
            workerLabel.Text = startLn.ToString();
            /*
            Highlight.startLine = startLn;
            Highlight.startPos = this.codingBox.SelectionStart;
             */
            Highlight.startLine = 0;
            Highlight.startPos = 0;
            Highlight.setWordMode(switchDrawMode.Checked);
            Highlight.reDraw(true);
            workerLabel.Text = Highlight.preRuntime + " | " + Highlight.runtime + " | " + Highlight.postRuntime;
        }

        private void codingBox_Vscroll(object sender, EventArgs e)
        {
            workerLabel.Text = "invisible";
            if (codingBox.selectionIsVisible())
            {
                workerLabel.Text = "visible";
            }
        }

        private void codingBox_TextChanged(object sender, EventArgs e)
        {
            //script.setCode(codingBox.Text);
            //errorTextBox.Text = script.getErrors();
            if (!isRunning)
            {
                refreshTimer.Enabled = true;
            }
            
            
        }

        private void codingBox_KeyUp(object sender, KeyEventArgs e)
        {
            keyTrigger.Enabled = false;

            //            refreshTimer.Enabled = true;
            //this.recheckScript();

            this.AutoComplete.setSelection(e);
        }

        private void refitElements()
        {
            if (!codeSplitContainer.Panel2Collapsed && !isRunning)
            {
                int cursorPosition = codingBox.SelectionStart;
                this.currentLineInEdit = codingBox.GetLineFromCharIndex(cursorPosition);
                if (this.currentLineInEdit < debugView.Items.Count)
                {
                    debugView.TopItem = debugView.Items[this.currentLineInEdit];
                }
            }
        }




        private void recheckScript()
        {
            if (isRunning)
            {
                return;
            }

            script.setCode(codingBox.Text);
            if (script.getErrorCount() == 0)
            {
                errCount.Text = "no errors";
                errCount.ForeColor = Color.DarkGreen;
                this.assignedExternalScript = codingBox.Text;
            }
            else
            {
                errCount.Text = script.getErrorCount() + " errors";
                errCount.ForeColor = Color.DarkRed;
            }

            extractObjectInfos();
            updateLogBook();
            refitElements();
        }

        private void updateObjectTree(Hashtable objList, string parentName, int imageNr, TreeNode toNode, string binding)
        {
            foreach (string methodmask in objList.Keys)
            {
                TreeNode mNode = new TreeNode();
                mNode.Text = methodmask;
                mNode.ToolTipText = parentName + binding + methodmask;
                mNode.ImageIndex = imageNr;
                mNode.SelectedImageIndex = mNode.ImageIndex;
                toNode.Nodes.Add(mNode);
                this.AutoComplete.addWord(methodmask, parentName);

            }
        }

        private String getResultByType(string type, Boolean ask = false)
        {
            List<string> possibles = this.script.getCurrentObjectsByType(type);
            if (possibles.Count < 1)
            {
                return type;
            }

            if (possibles.Count == 1)
            {
                return possibles[0];
            }
            if (ask)
            {
                ObjectBrowser browse = new ObjectBrowser();
                browse.objectList.Items.AddRange(possibles.ToArray());
                browse.ShowDialog();
                type = browse.objectList.Text;
            }
            return type;
        }


        private void updateObjectTree(List<MethodInfo> methods, string parentName, int imageNr, TreeNode toNode, string binding)
        {
            foreach (MethodInfo Minfo in methods)
            {

                TreeNode mNode = new TreeNode();               
                
                mNode.Text = Minfo.Name;
                mNode.ToolTipText = parentName + binding + Minfo.Name;
                mNode.ImageIndex = imageNr;
                mNode.SelectedImageIndex = mNode.ImageIndex;
                mNode.Tag = Minfo;
                
                toNode.Nodes.Add(mNode);
                //this.AutoComplete.addWord(methodmask, parentName);

            }
        }

        //List<MethodInfo>
        private void updateObjectTree(List<string> objList, string parentName, int imageNr, TreeNode toNode, string binding)
        {
            foreach (string methodmask in objList)
            {
                TreeNode mNode = new TreeNode();
                mNode.Text = methodmask;
                mNode.ToolTipText = parentName + binding + methodmask;
                mNode.ImageIndex = imageNr;
                mNode.SelectedImageIndex = mNode.ImageIndex;
                toNode.Nodes.Add(mNode);
                this.AutoComplete.addWord(methodmask,parentName);

            }
        }

        private String getObjectState(Object lookAtThese)
        {
            string stateInfo = "";
            if (lookAtThese is String)
            {
                string val = (string)lookAtThese;
                if (val == "")
                {
                    val = "\"\"";
                }
                return val;
            }

            if (lookAtThese is int || lookAtThese is Double)
            {
                return lookAtThese.ToString();
            }

            if (lookAtThese is Boolean)
            {
                Boolean chk = (Boolean)lookAtThese;
                if (chk)
                {
                    return "TRUE";
                }
                return "FALSE";
            }

            if (lookAtThese is Form)
            {
                Form checkForm = (Form)lookAtThese;

                stateInfo += "(" + checkForm.Width + "x" + checkForm.Height + ")";

                if (checkForm.Visible)
                {
                    stateInfo += "VISIBLE";
                }
                else
                {
                    stateInfo += "Invisible";
                }
            }
            return stateInfo;
        }


        private void getScriptObjects()
        {
            ObjectListing.Items.Clear();
            Hashtable scriptObjects = this.script.getRuntimeObjects(true);
            if (scriptObjects != null)
            {
                foreach (DictionaryEntry obInfo in scriptObjects)
                {
                    ListViewItem addItem = new ListViewItem();
                    System.Type obType = obInfo.Value.GetType();

                    addItem.Text = obInfo.Key.ToString();
                    addItem.SubItems.Add(obType.FullName);
                    string stateInfo = this.getObjectState(obInfo.Value);
                   
                    addItem.SubItems.Add(stateInfo);

                    // finally add this
                    ObjectListing.Items.Add(addItem);
                }
            }
        }


        // updates the generic tree depending on actually used objects
        private void extractObjectInfos()
        {
            Boolean addMethods = false;
            //genericTree.Nodes.Clear();

            

            foreach(RefScrObjectStorage usedObject in Projector.ObjectInfo.getAllObjects() ){
                string nodeKey = usedObject.originObjectName;
                TreeNode objectNode;
                if (!genericTree.Nodes.ContainsKey(nodeKey))
                {
                    objectNode = new TreeNode(nodeKey);
                    objectNode.Name = nodeKey;
                    objectNode.ImageIndex = ScriptWriter.TREE_OBJECT_IMG_IDENT;
                    objectNode.SelectedImageIndex = objectNode.ImageIndex;

                    if (addMethods)
                    {
                        TreeNode methods = new TreeNode("Methods");
                        methods.ImageIndex = 3;
                        methods.SelectedImageIndex = 3;


                        foreach (string methodmask in usedObject.methodNames)
                        {
                            methods.Nodes.Add(methodmask);
                            //this.AutoComplete.addWord(methodmask);

                        }
                        objectNode.Nodes.Add(methods);
                    }

                    genericTree.Nodes.Add(objectNode);
                }
                else
                {
                    objectNode = genericTree.Nodes[nodeKey];
                }

                List<string> instances = this.script.getCurrentObjectsByType(usedObject.originObjectName);
                if (instances != null)
                {
                    TreeNode usedBy;
                    if (objectNode.Nodes.ContainsKey("Instances"))
                    {
                        usedBy = objectNode.Nodes["Instances"];
                    }
                    else
                    {
                        usedBy = new TreeNode("Instances");
                        usedBy.SelectedImageIndex = ScriptWriter.TREE_INSTANCE_IMG_IDENT;
                        usedBy.ImageIndex = ScriptWriter.TREE_INSTANCE_IMG_IDENT;
                        usedBy.Name = "Instances";
                        objectNode.Nodes.Add(usedBy);    
                    }

                    foreach (string instOf in instances)
                    {
                        // add methods
                        

                        if (!usedBy.Nodes.ContainsKey(instOf))
                        {
                            TreeNode objInstance = new TreeNode(instOf);
                            objInstance.Name = instOf;
                            updateObjectTree(usedObject.methods, instOf, ScriptWriter.TREE_METHOD_IMG_IDENT, objInstance, " ");
                            /*updateObjectTree(usedObject.Strings, instOf, ScriptWriter.TREE_STRING_IMG_IDENT, objInstance, ".");
                            updateObjectTree(usedObject.Integers, instOf, ScriptWriter.TREE_INT_IMG_IDENT, objInstance, ".");
                            updateObjectTree(usedObject.Booleans, instOf, ScriptWriter.TREE_BOOL_IMG_IDENT, objInstance, ".");*/

                            usedBy.Nodes.Add(objInstance);    
                        }

                        
                    }
                    
                    
                    
                }

            }

            getScriptObjects();
            /*
            TreeNode varibales = new TreeNode("Variables");
            varibales.ImageIndex = ScriptWriter.TREE_STRING_IMG_IDENT;
            varibales.SelectedImageIndex = varibales.ImageIndex;

            foreach (DictionaryEntry scrVars in this.script.getAllStrings())
            {

                string varText = scrVars.Key.ToString();
                if (varText[0] != '%')
                {
                    TreeNode var = new TreeNode(scrVars.Key + " " + scrVars.Value);
                    var.ImageIndex = ScriptWriter.TREE_STRING_IMG_IDENT;
                    var.SelectedImageIndex = varibales.ImageIndex;
                    varibales.Nodes.Add(var);
                    this.AutoComplete.addWord(varText);
                }
            }
            genericTree.Nodes.Add(varibales);
            */
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

        public void varChange(int lineNumber, string name, string val)
        {
            if (this.varWatchList.Items.Count > 0)
            {
                for (int i = 0; i < this.varWatchList.Items.Count; i++)
                {
                    if (this.varWatchList.Items[i].Text == name)
                    {
                        if (this.varWatchList.Items[i].SubItems.Count < 2)
                        {
                            this.varWatchList.Items[i].SubItems.Add(val);
                        }
                        else
                        {
                            this.varWatchList.Items[i].SubItems[1].Text = val;
                        }
                    }
                }
            }
            /*
            this.logbook.Items.Add("       Change:" + name + "  | (" + val + ") " + lineNumber);
            if (logbook.Items.Count > 2)
            this.logbook.TopIndex = this.logbook.Items.Count - 1;
             * */
        }

        public void watcher(ReflectionScriptDefines currentLine, int lineNumber, int State, int executionLevel)
        {
            if (this.showObjectListing)
            {
                getScriptObjects();
            }

            string objName = currentLine.name;
            if (objName == null)
            {
                objName = "(null)";
            }

            objName += currentLine.code;

            if (currentLine.namedReference != null)
            {
                objName += " :" + currentLine.namedReference;
            }


            string prompt = "";
            for (int i = 0; i < executionLevel; i++)
            {
                prompt += " - ";
            }


            this.logbook.Items.Add( prompt + "> (" + lineNumber + "|" + currentLine.lineNumber + ") " + objName);
            if (logbook.Items.Count > 2)
                this.logbook.TopIndex = this.logbook.Items.Count - 1;


             //updateColors(currentLine.lineNumber);
            if (lastLineUpdate != lineNumber)
            {
                updateColors(lineNumber);
            }            
            lastLineUpdate = lineNumber;

            if (State == RefScriptExecute.STATE_RUN)
            {
                errorLabels.Text = "Currently Running " + currentLine.lineNumber;
                errorLabels.ForeColor = Color.DarkBlue;
                errorLabels.BackColor = Color.LightBlue;
                errorLabels.ToolTipText = "";
                
            }

            if (State == RefScriptExecute.STATE_WAIT)
            {
                errorLabels.Text = "Currently Waiting " + currentLine.lineNumber;
                errorLabels.ForeColor = Color.DarkOrange;
                errorLabels.BackColor = Color.LightYellow;
                errorLabels.ToolTipText = "";
                continueBtn.Enabled = true;
            }

            if (State == RefScriptExecute.STATE_FINISHED)
            {
                errorLabels.Text = "Execution is done " + currentLine.lineNumber;
                errorLabels.ForeColor = Color.DarkGreen;
                errorLabels.BackColor = Color.LightGreen;
                errorLabels.ToolTipText = "";
                isRunning = false;

                
                runButton.Enabled = true;
                continueBtn.Enabled = false;
                updateColors();
            }

           

            if (script.getErrorCount() > 0)
            {
                errorLabels.Text = script.getErrorCount() + "Execution errors: " + script.getErrors().Replace("\n", "").Substring(0, 20);
                errorLabels.ForeColor = Color.Red;
                errorLabels.BackColor = Color.DarkOrange;
                errorLabels.ToolTipText = script.getErrors();
            }

            // Application.DoEvents();
        }

        private void executeScript()
        {
            lastLineUpdate = -1;
            this.logbook.Items.Clear();
            this.logbook.Items.Add("START ...");
            this.Highlight.clearExecutions();
            script.setCode(codingBox.Text, true);
            if (script.getErrorCount() == 0)
            {

                errorLabels.Text = "Start exec";
                errorLabels.ForeColor = Color.LightBlue;
                errorLabels.BackColor = Color.DarkBlue;
                errorLabels.ToolTipText = "";
                Application.DoEvents();

                executer = new RefScriptExecute(script, this.execObject);

                // set me as watcher
                if (inspectRunToolStripMenuItem.Checked)
                {
                    executer.setWatcher(this, "watcher");
                    script.registerDebugWatcher(this, "varChange");
                }
                isRunning = true;
                runButton.Enabled = false;
                Boolean succeed = executer.run();
                runningCheck.Enabled = true;
                if (!inspectRunToolStripMenuItem.Checked)
                {
                    errorLabels.Text = "Execution is done ";
                    errorLabels.ForeColor = Color.DarkGreen;
                    errorLabels.BackColor = Color.LightGreen;
                    errorLabels.ToolTipText = "";
                    isRunning = false;


                    runButton.Enabled = true;
                    continueBtn.Enabled = false;
                }

            }
            else
            {
                string errorMessages = script.getErrors();

                errorLabels.Text = script.getNotRuntimeErrorCount() + "parsing errors: " + script.getErrors().Replace("\n", "").Substring(0, 20);
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
                System.IO.File.WriteAllText(this.filename, codingBox.Text);
                this.lastSaved = codingBox.Text;
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
            return (lastSaved != codingBox.Text);
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
                codingBox.Text = System.IO.File.ReadAllText(openFile.FileName);
                this.lastSaved = codingBox.Text;
                this.filename = openFile.FileName;
                this.assignedExternalScript = codingBox.Text;
                this.recheckScript();
                Highlight.loadColors();
                this.updateColors();
                statusLabel.Text = this.filename;
                
            }
           
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.filename = saveFileDialog.FileName;
                this.lastSaved = codingBox.Text;
                System.IO.File.WriteAllText(saveFileDialog.FileName, codingBox.Text);
                statusLabel.Text = this.filename;
                
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.saveChanges();
        }

        private void codingBox_VScroll(object sender, EventArgs e)
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

        private void codingBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Tab)
            {
                //codingBox.SelectedText = "   ";
                CodeFormater cf = new CodeFormater();
                cf.setContent(codingBox);
                cf.tabKey();
                e.SuppressKeyPress = true;
                return;
            }

            keyTrigger.Enabled = true;
            AutoComplete.keypressHandler( e );
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

        private void codingBox_MouseUp(object sender, MouseEventArgs e)
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
            this.searchReplaceDlg.Close();
        }

        private void showToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainSplitContainer.Panel1Collapsed = !mainSplitContainer.Panel1Collapsed;
        }

        private void navigatorBtn_Click(object sender, EventArgs e)
        {
            showToolbarToolStripMenuItem_Click(sender, e);
        }

        private void genericTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private void genericTree_DoubleClick(object sender, EventArgs e)
        {
            if (genericTree.SelectedNode != null)
            {
                if ((genericTree.SelectedNode.ImageIndex == ScriptWriter.TREE_METHOD_IMG_IDENT)
                    || (genericTree.SelectedNode.ImageIndex == ScriptWriter.TREE_STRING_IMG_IDENT)
                    || (genericTree.SelectedNode.ImageIndex == ScriptWriter.TREE_BOOL_IMG_IDENT)
                    || (genericTree.SelectedNode.ImageIndex == ScriptWriter.TREE_INT_IMG_IDENT)
                    )
                {
                   
                    //codingBox.Text = "";

                    if (genericTree.SelectedNode.Tag is MethodInfo)
                    {
                        MethodInfo Minfo = (MethodInfo)genericTree.SelectedNode.Tag;
                        string parameters = "";
                        ParameterInfo[] pInfo = Minfo.GetParameters();
                        foreach (ParameterInfo inf in pInfo)
                        {
                            switch (inf.ParameterType.Name)
                            {
                                case "ReflectionScript":
                                    parameters += " { }";
                                    break;
                                case "Boolean":
                                    parameters += " false";
                                    break;
                                case "Int32":
                                case "Int":
                                    parameters += " 0";
                                    break;
                                case "String":
                                    parameters += " \"" + inf.Name + "\"";
                                    break;
                                default:
                                    string replaced = this.getResultByType(inf.ParameterType.Name, true);
                                    if (replaced == inf.ParameterType.Name)
                                    {
                                        parameters += " " + inf.Name + ":" + inf.ParameterType.Name;
                                    }
                                    else
                                    {
                                        parameters += " " + replaced;
                                    }

                                    break;
                            }

                        }

                        string pre = "";
                        string returnVar = Minfo.ReturnType.Name;
                        if (returnVar != "" && returnVar != "Void")
                        {
                            switch (returnVar)
                            {
                                /* 
                                case "Boolean":
                                    parameters += " false";
                                    break;
                                case "Int32":
                                case "Int":
                                    parameters += " 0";
                                    break;
                                case "String":
                                    pre += ;
                                    break;*/
                                default:
                                    pre += getResultByType(returnVar,true) + " = ";
                                    break;
                            }
                        }
                        //genericTree.SelectedNode.ToolTipText = pre + genericTree.SelectedNode.ToolTipText + parameters;
                        int startPos = codingBox.SelectionStart;
                        int sellength = codingBox.SelectionLength;
                        codingBox.Text = codingBox.Text.Remove(startPos, sellength);
                        codingBox.Text = codingBox.Text.Insert(startPos, pre + genericTree.SelectedNode.ToolTipText + parameters);
                        codingBox.SelectionStart = startPos + genericTree.SelectedNode.ToolTipText.Length;
                    }
                    //codingBox.SelectedRtf = genericTree.SelectedNode.ToolTipText;
                    

                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (executer != null)
            {
                executer.Next();
            }
        }

        private void stopScr_Click(object sender, EventArgs e)
        {
            ProcSync.Reset(RefScriptExecute.PROC_NAME);
            executer.StopNow();
        }

        private void switchDrawMode_Click(object sender, EventArgs e)
        {
            updateColors();
        }

        private void colorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Highlight.loadColors();
            ScriptColors setupColor = new ScriptColors();
            setupColor.objectColors.setColor(Highlight.ObjectStyle);
            setupColor.VarColor.setColor(Highlight.VaribalesStyle);
            setupColor.CommentColor.setColor(Highlight.CommentStyle);
            setupColor.CommandColor.setColor(Highlight.CommandStyle);
            setupColor.StringColor.setColor(Highlight.TextStyle);
            setupColor.ReferenceColor.setColor(Highlight.ReferenzStyle);
            setupColor.Varibale2.setColor(Highlight.VaribalesStyle);
            setupColor.KeyWordColor.setColor(Highlight.KeyWordStyle);
            setupColor.NumberColor.setColor(Highlight.NumberStyle);
            setupColor.ObjectRefColor.setColor(Highlight.ObjectStyleReferenz);

            setupColor.MainStyle.setBackColor(HighlightStyle.defaultColor);

            setupColor.FontName.Text = Highlight.defaultFontName;
            setupColor.fontSize.Value = Highlight.fontDefaultSize;

            if (setupColor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Highlight.ObjectStyle = setupColor.objectColors.getHighLight();
                Highlight.TextStyle = setupColor.StringColor.getHighLight();
                Highlight.CommentStyle = setupColor.CommentColor.getHighLight();
                Highlight.VarStyle = setupColor.VarColor.getHighLight();
                Highlight.ReferenzStyle = setupColor.ReferenceColor.getHighLight();
                Highlight.VaribalesStyle = setupColor.Varibale2.getHighLight();
                Highlight.CommandStyle = setupColor.CommandColor.getHighLight();               
                Highlight.KeyWordStyle = setupColor.KeyWordColor.getHighLight();
                Highlight.NumberStyle = setupColor.NumberColor.getHighLight();
                Highlight.ObjectStyleReferenz = setupColor.ObjectRefColor.getHighLight();

                Highlight.defaultFontName = setupColor.FontName.Text;

                HighlightStyle.defaultColor = setupColor.MainStyle.getBackColor();
                codingBox.BackColor = HighlightStyle.defaultColor;
                Highlight.fontDefaultSize = (int) setupColor.fontSize.Value;
                Highlight.resetFonts();

                Highlight.saveColors();

                updateColors();
            }
        }

        private void getRunningProcesses()
        {
            this.ProcessList.Items.Clear();
            foreach (string procName in ProcSync.getAllMainProcs())
            {
                this.ProcessList.Items.Add(procName);
            }
        }

        private void runningCheck_Tick(object sender, EventArgs e)
        {
            if (script.imRunning())
            {
                this.getRunningProcesses();
                workerLabel.Text = "Still Running";
            }
            else
            {
                workerLabel.Text = "D O N E";
                runningCheck.Enabled = false;
            }
            this.getScriptObjects();
        }

        private void refreshProcBtn_Click(object sender, EventArgs e)
        {
            getRunningProcesses();
        }

        private void execKiller_Click(object sender, EventArgs e)
        {
            if (this.ProcessList.Text != "")
            {
                string id = this.ProcessList.Text;
                if (ProcSync.isRegistered(id))
                {
                    ProcSync.removeMainProc(id);

                }
            }
            getRunningProcesses();
        }

        private void addwatchMenuEntry_Click(object sender, EventArgs e)
        {
            UserTextInput addWatchVar = new UserTextInput();
            addWatchVar.textinfo.Text = "";
            addWatchVar.groupBox.Text = "Add Variable to watch...";
            if (addWatchVar.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                varWatchList.Items.Add(addWatchVar.textinfo.Text);
            }
        }

        private void showSriptBreaksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean vis = showSriptBreaksToolStripMenuItem.Checked;
            this.debugToolStrip.Visible = vis;
            
        }

        private void duplicateLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int lNr = this.codingBox.getCurrentLineNumber();            
            this.codingBox.duplicateLine(lNr);

        }

        private void removeLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int lNr = this.codingBox.getCurrentLineNumber();
            this.codingBox.removeLine(lNr);
        }

        private void seachAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.searchReplaceDlg.Visible == false)
            {
                this.searchReplaceDlg = new SearchReplace();
                this.searchReplaceDlg.Show();
                this.searchReplaceDlg.codeRef = this.codingBox;
            }
            this.searchReplaceDlg.BringToFront();
        }
    }
}
