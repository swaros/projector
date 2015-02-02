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
    public partial class ProfilGroupBox : UserControl
    {
        ProjectorForm parent;


        public void setParent(ProjectorForm par)
        {
            this.parent = par;
        }

        public void setText(string text)
        {
            if (text.Length > 3)
            {
                this.grpName.Text = text;
                this.OpenLabel.Text = text;
            }
        }

        public String getText()
        {
            return this.grpName.Text;
        }

        public ProfilGroupBox()
        {
            InitializeComponent();
            this.grpFlow.Visible = false;
            this.nameChange.Visible = false;
        }

        private void grpName_Click(object sender, EventArgs e)
        {
            
           
          
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OpenLabel_Click(object sender, EventArgs e)
        {
            this.grpFlow.Visible = false;
            this.Dock = DockStyle.None;
        }

        private void grpName_DoubleClick(object sender, EventArgs e)
        {
          
        }

        private void clickWatcher_Tick(object sender, EventArgs e)
        {
           
        }

        public void expand()
        {
            this.grpFlow.Visible = true;
            this.Dock = DockStyle.Fill;
            this.grpFlow.Dock = DockStyle.Fill;
            this.grpFlow.AutoSize = true;
            this.clickWatcher.Enabled = false;
        }

        public void setProfileNewPosition(ProfilButton btn, int pos)
        {
            pos++; // because of the first label that not should be reordered and always stay on place one. 
            //also  the new index is calculated by ProfilButtons only. For1 don't know here is an label on top
            if (this.grpFlow.Controls.ContainsKey(btn.Name))
            {
                Object curr = this.grpFlow.Controls[btn.Name];
                if (curr is ProfilButton)
                {
                    this.grpFlow.Controls.SetChildIndex(btn, pos);
                }
            }
        }

        private void grpName_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (!this.nameChange.Visible) {
                    this.expand();
                }
                
            }
            else
            {
                this.nameChange.Text = this.grpName.Text;
                this.nameChange.Visible = true;
            }
        }

        private void nameChange_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                string oldName = this.getText();
                this.nameChange.Visible = false;
                this.setText(this.nameChange.Text);

                if (!this.parent.renameGroup(oldName, this.getText()))
                {
                    this.setText(oldName);
                }

                e.SuppressKeyPress = true;
            }
        }

        private void grpFlow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Projector.ProfilButton"))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void grpFlow_DragDrop(object sender, DragEventArgs e)
        {
            ProfilButton pButton = (ProfilButton)e.Data.GetData("Projector.ProfilButton");
            if (pButton.assignedGroup == null)
            {
                this.grpFlow.Controls.Add(pButton);
                this.parent.joinToGroup(this.getText(), pButton.profilName);
               
            }
        }
    }
}
