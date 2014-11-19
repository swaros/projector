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

        public ReflectForm()
        {
            InitializeComponent();
        }

        public void setScript (ReflectionScript script)
        {
            this.Script = script;
        }

    }
}
