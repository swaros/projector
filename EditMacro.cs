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
    public partial class EditMacro : Form
    {
        TextParser tp = new TextParser();
        public EditMacro()
        {
            InitializeComponent();
            tp.addKeyWord("SetComparePrimary", 1);
            tp.addKeyWord("activateTable", 1);
            tp.addKeyWord("compareRows", 4);            
            tp.addKeyWord("function", 2);
            tp.addKeyWord("call", 2);
            tp.addKeyWord("selectSourceTable", 1);
            tp.addKeyWord("selectTargetTable", 1);
            tp.addKeyWord("alert", 1);
            tp.addKeyWord("SetSecondCheck",1);
            tp.addKeyWord("ignoreField", 1);
            tp.addKeyWord("includeField", 1);

            tp.addKeyWord("Checked",5);
            tp.addKeyWord("Unchecked", 5);

            tp.addKeyWord("setLimit",1);

            tp.addKeyWord("{", 3);
            tp.addKeyWord("}", 3);
            tp.addKeyWord(":", 3);
            tp.addKeyWord(";", 3);
        }

        public void addTables(string name)
        {
            tp.addKeyWord(name, 6);
        }

        public void parse()
        {
            tp.checkWords(sourceCode);
            tp.markTags(sourceCode, "{", "}", Color.Transparent, Color.LightGoldenrodYellow, 0);
            tp.markTags(sourceCode, ":", ";", Color.Transparent, Color.FromArgb(238,245,189),0);
            tp.markTags(sourceCode, "#", ";", Color.Blue, Color.FromArgb(211, 211, 211), 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, sourceCode.Text);   
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sourceCode.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);


                
                tp.checkWords(sourceCode);
                tp.markTags(sourceCode, "{", "}", Color.Transparent, Color.LightGoldenrodYellow, 0);
                //tp.markTags(sourceCode, "#", ":", Color.Gray, Color.LightCyan, 0);
                
            }
        }

        private void sourceCode_KeyUp(object sender, KeyEventArgs e)
        {
           
            /*
            int startpos = sourceCode.SelectionStart;
            int endPos = sourceCode.SelectionLength;

            if (e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.Return)
            {
                tp.checkWords(sourceCode);
                tp.markTags(sourceCode, "{", "}", Color.Transparent, Color.LightGoldenrodYellow, 0);
            }
                               

            sourceCode.SelectionStart = startpos;
            */
        }
    }
}
