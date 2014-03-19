using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    public partial class ConfimSync : Form
    {
        public ConfimSync()
        {
            InitializeComponent();
        }

        public bool isConfirmed(string fileName) {
            return this.confirmOverWrite.CheckedItems.Contains(fileName);
        }

        private void okBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
