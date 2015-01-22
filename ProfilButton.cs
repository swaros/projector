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
    public partial class ProfilButton : UserControl
    {
        public string profilName = "NONE";
        public Color profilColor = Color.LightSteelBlue;

        private int startWidth = 0;
        private int startHeight = 0;

        private int minWidth = 220;
        private int minHeight = 30;

        private Boolean growing = false;

        private ProjectorForm myParent;

        public ProfilButton(ProjectorForm parent)
        {
            InitializeComponent();
            this.myParent = parent;
            this.Description.Text = "";
            /*
            this.startHeight = this.Height;
            this.startWidth = this.Width;

            this.Width = minWidth;
            this.Height = minHeight;
             */
        }

        public void setColor(Color pCol)
        {
            this.profilColor = pCol;
            

            this.defaultColors();
            
        }

        private void defaultColors()
        {

            this.colorPanel.BackColor = this.profilColor;
            ColorCalc colCalc = new ColorCalc();
            colCalc.setBaseColor(this.profilColor);

            this.BackColor = colCalc.getDarker(3);
            //this.BackColor = Color.FromArgb(80, 80, 80);
            this.HeadLabel.ForeColor = colCalc.getLighter(2);
            //this.HeadLabel.ForeColor = Color.FromArgb(220, 220, 220);
            /*
            this.StartBtn.BackColor = colCalc.getLighter(3);
            this.StartBtn.ForeColor = colCalc.getDarker(3);
            this.button1.BackColor = colCalc.getLighter(3);
            this.button1.ForeColor = colCalc.getDarker(3);
             */
        }

        public void setDescription(string desc)
        {
            this.Description.Text = desc;
        }

        public void setName(string name)
        {
            this.profilName = name;
            this.HeadLabel.Text = name;
            this.StartBtn.Name = name;
        }

        private void ProfilButton_MouseHover(object sender, EventArgs e)
        {
            ColorCalc colCalc = new ColorCalc();
            this.colorPanel.BackColor = Color.White;
            colCalc.setBaseColor(this.profilColor);

            this.BackColor = colCalc.getDarker(2);
            //this.BackColor = Color.FromArgb(80, 80, 80);
            this.HeadLabel.ForeColor = Color.White;
            //this.HeadLabel.ForeColor = Color.FromArgb(220, 220, 220);
            /*
            this.StartBtn.BackColor = colCalc.getLighter(3);
            this.StartBtn.ForeColor = colCalc.getDarker(3);
            this.button1.BackColor = colCalc.getLighter(3);
            this.button1.ForeColor = colCalc.getDarker(3);
             *
             */
           // this.animTimer.Enabled = true;
            this.growing = true;
        }

        private void ProfilButton_MouseLeave(object sender, EventArgs e)
        {
            this.defaultColors();
            //this.animTimer.Enabled = true;
            this.growing = false;
        }

        private void animTimer_Tick(object sender, EventArgs e)
        {
            if (growing)
            {
                
                if (this.Height < this.startHeight)
                {
                    int diff = (this.startHeight - this.Height) / 2;
                    if (diff < 1) diff = 1;
                    this.Height+=diff;
                }

                if (this.Width < this.startWidth)
                {
                    int diff = (this.startWidth - this.Width) / 2;
                    if (diff < 1) diff = 1;
                    this.Width+=diff;
                }

                if (this.Width == startWidth && this.Height == this.startHeight)
                {
                    this.animTimer.Enabled = false;
                }
            }
            else
            {               
                if (this.Height > this.minHeight)
                {
                    int diff = (this.Height - this.minHeight) / 2;
                    if (diff < 1) diff = 1;
                    this.Height-=diff;
                }

                if (this.Width < this.minWidth)
                {
                    int diff = (this.Width - this.minWidth) / 2;
                    if (diff < 1) diff = 1;
                    this.Width-=diff;
                }

                if (this.Width == minWidth && this.Height == this.minHeight)
                {
                    this.animTimer.Enabled = false;
                }
            }
        }

        private void Description_Click(object sender, EventArgs e)
        {
            myParent.setCurrentProfile(this.profilName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myParent.setCurrentProfile(this.profilName);
            myParent.callSetup();
        }
    }
}