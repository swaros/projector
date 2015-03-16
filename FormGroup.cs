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
    public partial class FormGroup : UserControl
    {

        //private ReflectionScript invokeScr;

        public FormGroup()
        {
            InitializeComponent();
        }

        public void invoke(ReflectionScript invokeScript)
        {
            RefScriptExecute executer = new RefScriptExecute(invokeScript, this);
            executer.run();
        }

        public void setEnabled(Boolean onoff)
        {
            this.Enabled = onoff;
        }

        public void setVisible(Boolean onoff)
        {
            this.Visible = onoff;
        }

        public void setLeft(int left)
        {
            this.Left = left;
        }

        public void setTop(int top)
        {
            this.Top = top;
        }

        public void setWidth(int width)
        {
            this.Width = width;
        }

        public void setHeight(int height)
        {
            this.Height = height;
        }

        public void setLabel(string nameOfGroup)
        {
            this.formGroupBox.Text = nameOfGroup;
        }

        
    }
}
