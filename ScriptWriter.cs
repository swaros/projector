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

        ReflectionScript script = new ReflectionScript();

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
            errorTextBox.Text = script.getErrors() + System.Environment.NewLine + "============================" + System.Environment.NewLine + script.getSourceInfo();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            script.setCode(codeBox.Text);
            if (script.getErrorCount() == 0)
            {
                RefScriptExecute executer = new RefScriptExecute(script, this.execObject);
                executer.run();
            }
        }
    }
}
