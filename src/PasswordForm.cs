using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Projector.Crypt;

namespace Projector
{
    public partial class PasswordForm : Form
    {

        private Boolean passwordRequired = false;

        public PasswordForm()
        {
            InitializeComponent();
            this.passwordText.Focus();
        }

        public void setPasswordRequired(Boolean set = true)
        {
            this.passwordRequired = set;
            button1.Enabled = !set;
        }

        public void setLabel(string txt)
        {
            this.label1.Text = txt;
        }

        private void passwordText_TextChanged(object sender, EventArgs e)
        {
            if (passwordText.Text.Length < PrCrypt.MIN_LENGTH)
            {
                passwordText.BackColor = Color.LightPink;
                if (passwordRequired)
                {
                    button1.Enabled = false;
                }
            }
            else
            {
                passwordText.BackColor = Color.LightGreen;
                if (passwordRequired)
                {
                    button1.Enabled = true;
                }
            }
        }

        private void passwordText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && passwordText.Text.Length >= PrCrypt.MIN_LENGTH)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
    }
}
