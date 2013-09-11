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
    public partial class MdiForm : Form
    {
        public Profil currentProfil;

        private int windowID =0;
     
        public MdiForm()
        {
            InitializeComponent();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void dBWatchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void querysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windowID++;
            mysqlWatch watcher = new mysqlWatch();
            watcher.Name = "MysqlWatcher " + windowID;
            if (currentProfil.getProperty("set_bgcolor") != null && currentProfil.getProperty("set_bgcolor").Length > 2)
            {
                watcher.BackColor = Color.FromArgb(int.Parse(currentProfil.getProperty("set_bgcolor")));
            }
            //watcher.profil = profil;
            watcher.watchProfil = currentProfil;
            watcher.MdiParent = this;
            watcher.Show();
          
            listWindows();
        }

        private void sensorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            windowID++;
            MysqlSensor sensor = new MysqlSensor();
            sensor.Name = "Sensor " + windowID;
            if (currentProfil.getProperty("set_bgcolor") != null && currentProfil.getProperty("set_bgcolor").Length > 2)
            {
                sensor.BackColor = Color.FromArgb(int.Parse(currentProfil.getProperty("set_bgcolor")));
            }
            sensor.sensorProfil = currentProfil;
            sensor.getStartupData();
            sensor.MdiParent = this;
            sensor.Show();
            
            listWindows();
        }

        private void querysToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            windowID++;
            queryBrowser qb = new queryBrowser();
            qb.Name = "QueryBrowser " + windowID;
            if (currentProfil.getProperty("set_bgcolor") != null && currentProfil.getProperty("set_bgcolor").Length > 2)
            {
                qb.BackColor = Color.FromArgb(int.Parse(currentProfil.getProperty("set_bgcolor")));
            }
            //watcher.profil = profil;
            qb.sensorProfil = currentProfil;
            qb.loadPlaceHolder();
            qb.MdiParent = this;
            qb.listTables();
            qb.Show();

            listWindows();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);
            /*
            Form[] charr = this.MdiChildren;

            //For each child form set the window state to Maximized 

            foreach (Form chform in charr)
                chform.WindowState = FormWindowState.Maximized;
             */
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileVertical);
        }

        private void maximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form[] charr = this.MdiChildren;

            //For each child form set the window state to Maximized 

            foreach (Form chform in charr)
                chform.WindowState = FormWindowState.Maximized;
        }
        private void listWindows()
        {
            listWindows(Projector.Properties.Resources.computer_16);
        }
        
        private void listWindows(Image pic)
        {
            Form[] charr = this.MdiChildren;

            //For each child form set the window state to Maximized 
            

            for (int i = MainTools.Items.Count-1; i > 3; i--)
            {
                MainTools.Items.RemoveAt(i);
                
            }
            HashSet<string> check = new HashSet<string>();
            int nr = 1;
            foreach (Form chform in charr)
            {
                String displayname = chform.Name;
                
                if (displayname.Length > 15) displayname = displayname.Substring(0, 14) + "...";                

                if (check.Contains(displayname)) displayname = nr.ToString() + displayname;
                check.Add(displayname);
                chform.Name = displayname;

                if (chform.Text == "queryBrowser") MainTools.Items.Add(displayname,  Projector.Properties.Resources.application_view_list, windowSelect);
                else MainTools.Items.Add(displayname, pic, windowSelect);
                nr++;
            }
        }


        

        private void windowSelect(object sender, EventArgs e)
        {
            if (e != null)
            {
                ToolStripButton tmpBtn = (ToolStripButton)sender;
                //MessageBox.Show(tmpBtn.Text);
                Form[] charr = this.MdiChildren;

                //For each child form set the window state to Maximized 

                foreach (Form chform in charr)
                {
                    //MainTools.Items.Add(chform.Name, Projector.Properties.Resources.applications_16, windowSelect);
                    if (chform.Name == tmpBtn.Text)
                    {
                        chform.BringToFront();
                    }
                }

            }
        }

        private void arrangeIcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.ArrangeIcons);
            listWindows();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            copyDb qb = new copyDb();
            if (currentProfil.getProperty("set_bgcolor") != null && currentProfil.getProperty("set_bgcolor").Length > 2)
            {
                qb.BackColor = Color.FromArgb(int.Parse(currentProfil.getProperty("set_bgcolor")));
            }
            //watcher.profil = profil;
            //qb.sensorProfil = currentProfil;
            //qb.MdiParent = this;
            //qb.listTables();
            //qb.Show();
        }

        private void MdiForm_ControlRemoved(object sender, ControlEventArgs e)
        {
            listWindows();
        }

        private void MdiForm_MdiChildActivate(object sender, EventArgs e)
        {
            listWindows();
        }

        private void toolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainTools.Visible = toolbarToolStripMenuItem.Checked;
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            listWindows();
        }

        private void MainTools_MouseHover(object sender, EventArgs e)
        {
            refreshTimer.Stop();
        }

        private void MainTools_MouseLeave(object sender, EventArgs e)
        {
            refreshTimer.Start();
        }
    }
}
