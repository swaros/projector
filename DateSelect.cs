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
    public partial class DateSelect : UserControl
    {

        public String Date = "0000-00-00";
        public String Time = "00:00:00";


        private String DateFormat = "yyyy-MM-dd";

        public DateSelect()
        {
            InitializeComponent();

            for (int i = 0; i < 25; i++)
            {
                string tm = i.ToString();
                if (i < 10)
                {
                    tm = "0" + tm;
                }
                this.HoureBox.Items.Add(tm);
            }

            for (int i = 0; i < 61; i++)
            {
                string tm = i.ToString();
                if (i < 10)
                {
                    tm = "0" + tm;
                }
                this.MinBox.Items.Add(tm);
                this.SecBox.Items.Add(tm);
            }
            this.MinBox.Text = "00";
            this.HoureBox.Text = "00";
            this.SecBox.Text = "00";

            this.Text = getDate();
        }

        private void selectdate_ValueChanged(object sender, EventArgs e)
        {            
            this.Text = getDate();
        }


        public void setDateFormat(string format)
        {
            this.DateFormat = format;
            this.Text = getDate();
        }

        public String getDate()
        {
            if (this.MinBox.Visible)
            {
                this.Time = " " + this.HoureBox.Text + ":" + this.MinBox.Text + ":" + this.SecBox.Text;
            }
            else
            {
                this.Time = "";
            }

            
            this.Date = selectdate.Value.ToString(this.DateFormat);
            return this.Date + this.Time;
        }

        public void setTimeUsage(Boolean set)
        {
            this.MinBox.Visible = set;
            this.SecBox.Visible = set;
            this.HoureBox.Visible = set;
            this.Text = getDate();
        }

        public void setTop(int top)
        {
            this.Top = top;
        }

        public void setLeft(int left)
        {
            this.Left = left;
        }

        public void setLabel(string label)
        {
            this.label1.Text = label;
        }

        private void HoureBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = getDate();
        }

        private void MinBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = getDate();
        }

        private void SecBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = getDate();
        }
    }
}
