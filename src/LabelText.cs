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
    public partial class LabelText : UserControl
    {

        public Boolean doNotSend = false;


        private ReflectionScript onLeaveScr;
        private ReflectionScript onEnterScr;
        private ReflectionScript onChangeScr;

        public LabelText()
        {
            InitializeComponent();
        }

        public void OnTextChange(ReflectionScript scr)
        {
            this.onChangeScr = scr;
        }

        public void OnLeave(ReflectionScript scr)
        {
            this.onLeaveScr = scr;
        }

        public void OnEnter(ReflectionScript scr)
        {
            this.onEnterScr = scr;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            this.Text = TextBox.Text;
            if (this.onChangeScr != null)
            {
                RefScriptExecute exec = new RefScriptExecute(this.onChangeScr, this);
                exec.run();
            }
        }

        public void setLabel(string name)
        {
            this.TextLabel.Text = name;
        }

        public void setText(string value)
        {
            this.TextBox.Text = value;
        }

        public void setTop(int top)
        {
            this.Top = top;
        }
        public String getText()
        {
            return this.Text;
        }
        public void setLeft(int left)
        {
            this.Left = left;
        }
        public void setEnabled(Boolean onOff)
        {
            this.Enabled = onOff;
        }

        public void setVisibility(Boolean onOff)
        {
            this.Visible = onOff;
        }
        public void setInputWidth(int width)
        {
            this.TextBox.Width = width;
        }

        public void setLabelWidth(int width)
        {
            this.TextLabel.AutoSize = false;
            this.TextLabel.Width = width;
        }

        public void isPostElement(Boolean onOff)
        {
            this.doNotSend = !onOff;
        }

        public void setHeight(int height)
        {
            if (height > 20)
            {
                this.TextBox.Multiline = true;
                
                this.TextBox.Height = height;
                
            }
        }

        public void setpasswordChars()
        {
            this.TextBox.UseSystemPasswordChar = true;
            this.doNotSend = true;
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            if (this.onLeaveScr != null)
            {
                RefScriptExecute exec = new RefScriptExecute(this.onLeaveScr, this);
                exec.run();
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            if (this.onEnterScr != null)
            {
                RefScriptExecute exec = new RefScriptExecute(this.onEnterScr, this);
                exec.run();
            }
        }

    }
}
