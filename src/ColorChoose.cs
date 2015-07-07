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
    public partial class ColorChoose : UserControl
    {
        public int colorValue;
        public int backGroundColorValue;

        public Boolean BackGroundMode = true;

        private HighlightStyle currentHighlight;

        public ColorChoose()
        {
            InitializeComponent();
        }

        public ColorChoose(Boolean backGroundMode)
        {
            InitializeComponent();
            this.BackGroundMode = backGroundMode;
        }

        public void setColor(HighlightStyle highlight)
        {
            this.currentHighlight = highlight;
            this.setColor(currentHighlight.ForeColor);
            this.setBackColor(currentHighlight.BackColor);
        }


        public void setColor(Color col)
        {
            
            this.exampleLabel.Visible = true;
            this.exampleLabel.ForeColor = col;                        
            colorValue = col.ToArgb();
        }

        public void setBackColor(Color col)
        {
            this.BackColor = col;
            backGroundColorValue = col.ToArgb();
        }


        public Color getColor()
        {
            return Color.FromArgb(this.colorValue);
        }

        public Color getBackColor()
        {
            return Color.FromArgb(this.backGroundColorValue);
        }

        public HighlightStyle getHighLight()
        {
            this.currentHighlight.BackColor = this.getBackColor();
            this.currentHighlight.ForeColor = this.getColor();
            return this.currentHighlight;
        }

        private void colorBtn_Click(object sender, EventArgs e)
        {
            if (colorSelectDialog.ShowDialog() == DialogResult.OK)
            {
                this.setBackColor(colorSelectDialog.Color);
            }
        }

        private void exampleLabel_DoubleClick(object sender, EventArgs e)
        {
            if (colorSelectDialog.ShowDialog() == DialogResult.OK)
            {
                this.setColor(colorSelectDialog.Color);
            }
        }
    }
}
