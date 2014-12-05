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

        private void ReflectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.onCloseScript != null)
            {
                if (this.onCloseScript.getErrorCount() == 0)
                {
                    RefScriptExecute executer = new RefScriptExecute(this.onCloseScript, this);
                    executer.run();
                }
            }
        }
    }
}
