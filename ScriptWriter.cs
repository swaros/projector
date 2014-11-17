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

        public Object execObject;

        private string filename = null;

        private string lastSaved = "";

        public String assignedExternalScript = null;

        ReflectionScript script = new ReflectionScript();


        private Boolean autClose = false;

        public ScriptWriter(Object targetObject, Boolean openFile)
        {

            
            InitializeComponent();
            this.execObject = targetObject;
            script.setCode(codeBox.Text);

            if (openFile)
            {
                loadToolStripMenuItem_Click(null, null);
                this.autClose = true;
            }
            

        }

        public ScriptWriter(Object targetObject, string scriptText)
        {


            InitializeComponent();
            this.execObject = targetObject;
            if (scriptText != null)
            {
                codeBox.Text = scriptText;
            }
            
            script.setCode(codeBox.Text);            

        }

        public ScriptWriter(Object targetObject)
        {
            InitializeComponent();
            this.execObject = targetObject;
            script.setCode(codeBox.Text);

        }

        private void codeBox_TextChanged(object sender, EventArgs e)
        {
            //script.setCode(codeBox.Text);
            //errorTextBox.Text = script.getErrors();
        }

        private void codeBox_KeyUp(object sender, KeyEventArgs e)
        {
            script.setCode(codeBox.Text);
            if (script.getErrorCount() == 0)
            {
                this.assignedExternalScript = codeBox.Text;
            }

            errorTextBox.Text = script.getErrors() + System.Environment.NewLine + "============================" + System.Environment.NewLine + script.getSourceInfo();
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
    }
}
