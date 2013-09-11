using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MysqlManager
{
    public partial class SqlSensor : Form
    {
        public SqlSensor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem tmp = new ListViewItem();
            tmp.Text = sensorname.Text;
            tmp.SubItems.Add(sensortime.Value + "");
            tmp.SubItems.Add(sensorsql.Text);

            sqlSensorList.Items.Add(tmp);
        }
    }
}
