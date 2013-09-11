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
    public partial class SqlExecConfirm : Form
    {
        HighlighterMysql highlight = new HighlighterMysql();
        public SqlExecConfirm()
        {
            InitializeComponent();
            this.WarningText.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, sqlTextBox.Text);
        }

        public void parse(string text)
        {
            this.sqlTextBox.Text = text;
            highlight.parse(sqlTextBox);
        }

        private void SqlExecConfirm_Activated(object sender, EventArgs e)
        {
            highlight.parse(sqlTextBox);
        }
    }
}
