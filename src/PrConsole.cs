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
    public partial class PrConsole : UserControl
    {

        public PrConsole()
        {
            InitializeComponent();
            this.ShowDetailLabel.Visible = false;
        }

        public void setWidth(int val)
        {
            this.Width = val;
        }

        public void setHeight(int val)
        {
            this.Height = val;
        }

        public void setTop(int val)
        {
            this.Top = val;
        }

        public void setLeft(int val)
        {
            this.Left = val;
        }

        public void addMessage(string message)
        {

            this.Console.Items.Add(message);
            this.Console.TopIndex = Console.Items.Count - 1;
            this.Console.Update();
            Application.DoEvents();
        }

        public void fillParent()
        {
            this.Dock = DockStyle.Fill;          
        }

        public void unFillParent()
        {
            this.Dock = DockStyle.None;
        }

        private void Console_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowDetailLabel.Visible = true;
            this.ShowDetailLabel.Text = this.Console.Items[this.Console.SelectedIndex].ToString();
        }

        private void ShowDetailLabel_Click(object sender, EventArgs e)
        {
            this.ShowDetailLabel.Visible = false;
        }

        private void Console_DoubleClick(object sender, EventArgs e)
        {
            if (this.Dock == DockStyle.None) this.Dock = DockStyle.Fill;
            else this.Dock = DockStyle.None;
        }



    }
}
