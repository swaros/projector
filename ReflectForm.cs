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
    public partial class ReflectForm : Form
    {

        public string ScriptIdent = "";

        private ReflectionScript Script;

        private ReflectionScript onCloseScript;

        public ReflectForm()
        {
            InitializeComponent();
        }

        public void invoke (ReflectionScript script)
        {
            this.Script = script;
            if (this.Script.getErrorCount() == 0)
            {
                RefScriptExecute executer = new RefScriptExecute(this.Script,this);
                executer.run();
            }
        }

        public void OnCloseForm(ReflectionScript script)
        {
            this.onCloseScript = script;
        }

        public void setLeft(int left)
        {
            this.Left = left;
        }

        public void setTop(int top)
        {
            this.Top = top;
        }

        public void setWidth(int w)
        {
            this.Width = w;
        }

        public void setHeight(int h)
        {
            this.Height = h;
        }

        public void CloseForm()
        {
            this.Close();
        }

        public void setCaption(string caption)
        {
            this.Text = caption;
        }

        public String getCaption()
        {
            return this.Text;
        }

        public void setEnabled(Boolean setit)
        {
            this.Enabled = setit;
        }

        private void closeRequestAction()
        {
            if (this.onCloseScript != null)
            {

                foreach (Control element in this.Controls)
                {
                    //this.onCloseScript.updateVarByObject()

                    string nameOfObject = element.Name;
                    string objectValue = element.Text;

                    this.onCloseScript.createOrUpdateStringVar("&" + nameOfObject + "." + "Text", objectValue);

                    if (this.onCloseScript.Parent != null)
                    {
                        this.onCloseScript.Parent.createOrUpdateStringVar("&" + this.ScriptIdent + "." + nameOfObject + "." + "Text", objectValue);
                    }

                }

                if (this.onCloseScript.getErrorCount() == 0)
                {
                    RefScriptExecute executer = new RefScriptExecute(this.onCloseScript, this);
                    executer.run();
                }
            }
        }

        private void ReflectForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            this.Visible = false;  
        }

        private void ReflectForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.closeRequestAction();
            Application.DoEvents();
            ReflectNew.removeMe(this);
        }
    }
}
