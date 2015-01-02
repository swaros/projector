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
    public partial class ScriptColors : Form
    {
        public ScriptColors()
        {
            InitializeComponent();
        }

        private void objectColors_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mainFontDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FontName.Text = mainFontDlg.Font.FontFamily.Name;
            }
        }

        
    }
}
