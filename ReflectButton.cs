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
    public partial class ReflectButton : UserControl
    {

        private ReflectionScript invokeOnClickScr;

        private Boolean autoCloseParent = true;

        public ReflectButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (this.invokeOnClickScr != null)
            {
                RefScriptExecute executer = new RefScriptExecute(this.invokeOnClickScr, this);
                executer.run();
            }

            if (autoCloseParent)
            {
                closeForm();
            }
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

        public void setText(string label)
        {
            this.Button.Text = label;
        }

        public void autoclose(Boolean onOff)
        {
            autoCloseParent = onOff;
        }

        public void closeForm()
        {
            if (Parent is ReflectForm)
            {
                ReflectForm parentForm = (ReflectForm)Parent;
                parentForm.Close();
            }
        }

        public void OnClick(ReflectionScript invkoeOnClick){
            this.invokeOnClickScr = invkoeOnClick;
        }
    }
}
