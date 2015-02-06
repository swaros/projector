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
    public partial class PathSelector : UserControl
    {
        private string path = "";
        private string lastPath = "";


        public PathSelector()
        {
            InitializeComponent();
        }

        public void setPath(string spath)
        {
            this.lastPath = this.path;
            this.path = spath;
            this.FolderPath.Text = spath;
            this.folderBrowser.SelectedPath = spath;
        }

        public void setInfo(string inf)
        {
            this.PathInfo.Text = inf;
        }

        public String getPath()
        {
            return this.path;
        }

        public Boolean isvalidPath()
        {
            return System.IO.Directory.Exists(this.path);
        }

        private void PathOpen_Click(object sender, EventArgs e)
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                this.setPath(folderBrowser.SelectedPath);
            }
        }

        private void FolderPath_Leave(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(this.FolderPath.Text))
            {
                this.FolderPath.ForeColor = Color.Red;
            }
            else
            {
                this.FolderPath.ForeColor = Color.DarkGreen;
            }
        }

        private void FolderPath_TextChanged(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(this.FolderPath.Text))
            {
                this.FolderPath.ForeColor = Color.Red;
            }
            else
            {
                this.FolderPath.ForeColor = Color.DarkGreen;
            }
        }
    }
}
