using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    public partial class LabelInteger : UserControl
    {
        public LabelInteger()
        {
            InitializeComponent();
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.Text = this.numericUpDown.Value.ToString();
        }

        public void setLabel(string name)
        {
            this.TextLabel.Text = name;
        }

        public void setValue(Decimal value)
        {
            this.numericUpDown.Value = value;
        }

        public int getInt()
        {
            return (int)this.numericUpDown.Value;
        }

        public void setTop(int top)
        {
            this.Top = top;
        }

        public void setLeft(int left)
        {
            this.Left = left;
        }
    }
}
