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
        public string assignedGroup;

        private int startWidth = 0;
        private int startHeight = 0;

        private int minWidth = 220;
        private int minHeight = 30;

        private Boolean growing = false;

        private int defaultPercent = 70;
        private int HighlightPercent = 10;
        private int currentPercent = 70;

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

        public String getName()
        {
            return this.profilName;
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

            this.BackColor = colCalc.DarkenBy(this.defaultPercent);
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
            this.HeadLabel.ForeColor = Color.White;
          
            this.animTimer.Enabled = true;
            this.growing = false;
        }

        private void ProfilButton_MouseLeave(object sender, EventArgs e)
        {
            //this.defaultColors();
            this.animTimer.Enabled = true;
            this.growing = true;
        }

        private void animTimer_Tick(object sender, EventArgs e)
        {
            if (growing)
            {
                if (this.currentPercent < this.defaultPercent - 4)
                {
                    this.currentPercent+=4;
                }
                else
                {
                    this.animTimer.Enabled = false;
                    this.defaultColors();
                }
               
            }
            else
            {
                if (this.currentPercent > this.HighlightPercent - 15)
                {
                    this.currentPercent-=15;
                }
                else
                {
                    this.animTimer.Enabled = false;
                }
            }
            ColorCalc colCalc = new ColorCalc();
            colCalc.setBaseColor(this.profilColor);

            this.BackColor = colCalc.DarkenBy(this.currentPercent);
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

        private void colorPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this, DragDropEffects.Move | DragDropEffects.Copy);
        }

        private void ProfilButton_Enter(object sender, EventArgs e)
        {
           
        }

        private void ProfilButton_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Projector.ProfilButton"))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ProfilButton_DragDrop(object sender, DragEventArgs e)
        {
            ProfilButton groupWidth = (ProfilButton) e.Data.GetData("Projector.ProfilButton");
            if (groupWidth != null)
            {
                this.myParent.joinToNewGoup(this.profilName, groupWidth.profilName);
            }
        }
    }
}