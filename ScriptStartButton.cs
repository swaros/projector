using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Projector.Script;

namespace Projector
{
    public partial class ScriptStartButton : UserControl
    {

        private RefScrAutoScrContainer script;
        private Object parent;
        public ScriptStartButton(Object parentObject)
        {
            this.parent = parentObject;
            InitializeComponent();
            this.StartBtn.Enabled = false;
            this.EditBtn.Visible = false;
            this.DescLabel.BackColor = Color.Transparent;

        }

        public void setScript(RefScrAutoScrContainer cont)
        {

            this.script = cont;
            this.updateMe();
        }

        private void updateMe()
        {
            if (this.script.Script.getNotRuntimeErrorCount() == 0)
            {
                this.MainLabel.Text = this.script.Label;
                if (this.script.Description != null)
                {
  
                    this.DescLabel.Text = this.script.Description;
                }
                this.StartBtn.Enabled = true;
            }
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (this.script != null)
            {
                runTimer.Enabled = true;
                this.StartBtn.Enabled = false;
                this.BackgroundImage = Projector.Properties.Resources.green_bar;
                RefScriptExecute exec = new RefScriptExecute(this.script.Script, this.parent);
                exec.run();

            }
        }

        private void runTimer_Tick(object sender, EventArgs e)
        {
            if (this.script.Script.imRunning())
            {
                this.StartBtn.Enabled = false;
            }
            else
            {
                
                this.runTimer.Enabled = false;
                this.StartBtn.Enabled = true;
                if (this.script.Script.getErrorCount() == 0)
                    this.BackgroundImage = Projector.Properties.Resources.yellow_bar;
                else
                    this.BackgroundImage = Projector.Properties.Resources.red_bar;
            }
        }

        private void DescLabel_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void DescLabel_MouseLeave(object sender, EventArgs e)
        {
            
        }

    }
}
