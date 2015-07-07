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
    public partial class SearchReplace : Form
    {

        public RichBox codeRef;

        public SearchReplace()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.codeRef != null)
            {
                this.codeRef.replace(this.search.Text, this.replace.Text);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            }
        }
    }
}
