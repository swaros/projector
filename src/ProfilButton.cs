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

        public Boolean Selected = false;


        private Boolean growing = false;

        private int defaultPercent = 70;
        private int HighlightPercent = 10;
        private int currentPercent = 70;

        private int ForeDefaultPercent = 10;
        private int ForeCurrentPercent = 10;

        private ProjectorForm myParent;

        public ProfilButton(ProjectorForm parent)
        {
            InitializeComponent();
            this.myParent = parent;
            this.Description.Text = "";

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
            if (this.Selected)
            {
                this.BackColor = colCalc.LightenBy(this.defaultPercent);
                this.HeadLabel.ForeColor = colCalc.DarkenBy(this.ForeDefaultPercent);
                this.Description.ForeColor = this.HeadLabel.ForeColor;
            }
            else
            {
                this.BackColor = colCalc.DarkenBy(this.defaultPercent);
                this.HeadLabel.ForeColor = colCalc.DarkenBy(this.ForeDefaultPercent);
                this.Description.ForeColor = this.HeadLabel.ForeColor;
            }
          
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

        public void flush()
        {
            this.unFlushTimer.Enabled = true;
            ProfilButton_MouseHover(null, null);           
        }

        public void unflush()
        {
            ProfilButton_MouseLeave(null, null);
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
            if (this.Selected)
            {
                this.defaultColors();
                return;
            }

            if (growing)
            {
                if (this.currentPercent < this.defaultPercent - 4)
                {
                    this.currentPercent+=4;
                    this.ForeCurrentPercent -= 4;
                }
                else
                {
                    this.animTimer.Enabled = false;
                    this.defaultColors();
                    return;

                }
               
            }
            else
            {
                if (this.currentPercent > this.HighlightPercent - 15)
                {
                    this.currentPercent-=15;
                    this.ForeCurrentPercent += 15;
                }
                else
                {
                    this.animTimer.Enabled = false;
                }
            }
            ColorCalc colCalc = new ColorCalc();
            colCalc.setBaseColor(this.profilColor);

            this.BackColor = colCalc.DarkenBy(this.currentPercent);
            this.HeadLabel.ForeColor = colCalc.DarkenBy(this.ForeCurrentPercent);
            this.Description.ForeColor = this.HeadLabel.ForeColor;
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

        private void changeSelect()
        {
            if (this.Selected)
                this.myParent.setSelected(this);
            else
                this.myParent.setUnSelected(this);
        }

        private void colorPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this, DragDropEffects.Move | DragDropEffects.Copy);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.Selected = true;
                this.changeSelect();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Selected = !this.Selected;
                this.changeSelect();
            } 
        }

        private void ProfilButton_Enter(object sender, EventArgs e)
        {
           
        }

        private void ProfilButton_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Projector.ProfilButton"))
            {
                ProfilButton groupWidth = (ProfilButton)e.Data.GetData("Projector.ProfilButton");
                if (groupWidth != null && groupWidth.assignedGroup == null && this.assignedGroup != null)
                    e.Effect = DragDropEffects.Copy;
                else if (groupWidth != null && groupWidth.assignedGroup == null && this.assignedGroup == null)
                    e.Effect = DragDropEffects.Move;
                else if (groupWidth != null && groupWidth.assignedGroup != null && this.assignedGroup != null && groupWidth.assignedGroup == this.assignedGroup)
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void ProfilButton_DragDrop(object sender, DragEventArgs e)
        {
            ProfilButton groupWidth = (ProfilButton) e.Data.GetData("Projector.ProfilButton");
            if (groupWidth != null)
            {
                // booth have no group so make an new one
                if (groupWidth.assignedGroup == null && this.assignedGroup == null)
                {
                    this.myParent.joinToNewGoup(this.profilName, groupWidth.profilName);
                }

                if (groupWidth.assignedGroup != null && this.assignedGroup != null && groupWidth.assignedGroup == this.assignedGroup)
                {
                    this.myParent.reOrderButtonsInGroup(this, groupWidth);
                }


            }
        }

        private void ProfilButton_DoubleClick(object sender, EventArgs e)
        {

        }

        private void HeadLabel_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void unFlushTimer_Tick(object sender, EventArgs e)
        {
            this.unFlushTimer.Enabled = false;
            this.unflush();

        }
    }
}