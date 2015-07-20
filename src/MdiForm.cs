﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Projector.Script;

namespace Projector
{
    public partial class MdiForm : StyleForm
    {
        public Profil currentProfil;

        private int windowID =0;

        private String scriptText = null;
     
        public MdiForm()
        {
            InitializeComponent();
            scriptExec.Enabled = (this.scriptText != null);
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
            
            queryBrowser qb = new queryBrowser();
            this.addQueryWindow(qb);
        }

        public void addQueryWindow(queryBrowser qb)
        {
            windowID++;
            qb.Name = "QueryBrowser " + windowID;
            if (currentProfil.getProperty(ProfilProps.STYLE_COLOR) != null && currentProfil.getProperty(ProfilProps.STYLE_COLOR).Length > 2)
            {
                StyleFormProps profilStyle = new StyleFormProps();
                profilStyle.MainColor = Color.FromArgb(int.Parse(currentProfil.getProperty(ProfilProps.STYLE_COLOR)));

                string styleName = currentProfil.getProperty(ProfilProps.WINDOW_STYLE);
                if (styleName == null)
                {
                    styleName = ProjectorForm.getMainFormStyle();

                }

                profilStyle.composeStyle(styleName );
                qb.setStyle(profilStyle);

            }
            qb.sensorProfil = currentProfil;
            qb.loadPlaceHolder();
            qb.MdiParent = this;
            
            qb.Show();
            qb.listTables();
            listWindows();
        }


        public void addSubWindow(Form qb, string prefix)
        {
            windowID++;
            qb.Name = prefix + windowID;
            if (currentProfil.getProperty("set_bgcolor") != null && currentProfil.getProperty("set_bgcolor").Length > 2)
            {
                qb.BackColor = Color.FromArgb(int.Parse(currentProfil.getProperty("set_bgcolor")));
            }
            //watcher.profil = profil;                        
            qb.MdiParent = this;            
            qb.Show();
            listWindows();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.Cascade);
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

            foreach (Form chform in charr)
                chform.WindowState = FormWindowState.Maximized;
        }
        private void listWindows()
        {
            listWindows(Projector.Properties.Resources.computer_16);
        }


        private void updateQueryWindows()
        {
            Form[] charr = this.MdiChildren;
            foreach (Form chform in charr)
            {
                String displayname = chform.Name;

                if (chform.Text == "queryBrowser")
                {
                    Type queryWinType = chform.GetType();
                    MethodInfo myMethodInfo = queryWinType.GetMethod("fireQuery");
                    object[] mParam = new object[] {};
                    myMethodInfo.Invoke(chform, null);
                }
                
            }
        }

        private String getWindowScripts()
        {
            string code = "# Projector Query Script" + System.Environment.NewLine;

            Form[] charr = this.MdiChildren;
            foreach (Form chform in charr)
            {
                String displayname = chform.Name;
                
                if (chform.Text == "queryBrowser")
                {
                    string varName = displayname.Replace(" ", "_");
                    string objectname = varName.Replace(".", "");
                    code += "New queryBrowser " + objectname + System.Environment.NewLine;

                    code += objectname + " showTableList false " + System.Environment.NewLine;

                    code += objectname + " setCoords " + chform.Left + " " + chform.Top + " " + chform.Width + " " + chform.Height + System.Environment.NewLine;

                    Type queryWinType = chform.GetType();
                    MethodInfo myMethodInfo = queryWinType.GetMethod("getCurrentTable");
                    object[] mParam = new object[] { };
                    string table = (string) myMethodInfo.Invoke(chform, null);
                    code += objectname + " selectTable \"" + table + "\"" + System.Environment.NewLine;


                    myMethodInfo = queryWinType.GetMethod("getCurrentSql");
                    string sql = (string)myMethodInfo.Invoke(chform, null);
                    code += objectname + " setSql \"" + sql + "\"" + System.Environment.NewLine;
                   

                }

            }

          //  ReflectionScript script = new ReflectionScript();
          //  script.setCode(code);
            ScriptWriter scriptEditor = new ScriptWriter(this);
            scriptEditor.setCode(code);
            scriptEditor.ShowDialog();


            return code;
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

                if (chform.Text == "queryBrowser") MainTools.Items.Add(displayname,  Projector.Properties.Resources.applicationviewlist, windowSelect);
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

        private void refreshAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateQueryWindows();
        }

        private void makeSnapshotAsScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getWindowScripts();
        }

        private void editScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptWriter scriptEditor = new ScriptWriter(this,this.scriptText);
            this.addSubWindow(scriptEditor, "P-Script");
            //scriptEditor.MdiParent = this;
            //qb.MdiParent = this;
            //scriptEditor.Show();
        }

        public Form getQueryForm(string name)
        {
            foreach (Form chform in this.MdiChildren)
            {
                String displayname = chform.Name;

                if (chform.Text == "queryBrowser")
                {
                    queryBrowser querBrw = (queryBrowser)chform;

                    if (querBrw.ScriptIdent == name)
                    {
                        return chform;
                    }
                }

            }
            return null;
        }


        public Form getExistsForm(string name)
        {
            foreach (Form chform in this.MdiChildren)
            {
                String displayname = chform.Name;

                if (displayname == name)
                {
                    
                   return chform;
                    
                }

            }
            return null;
        }


        private void scriptExec_Click(object sender, EventArgs e)
        {
            if (this.scriptText != null)
            {
                ReflectionScript script = new ReflectionScript();
                script.setCode(this.scriptText);
                if (script.getNotRuntimeErrorCount() == 0)
                {
                    RefScriptExecute executer = new RefScriptExecute(script, this);
                    executer.run();
                }
            }
        }

        private void manageToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            scriptExec.Enabled = (this.scriptText != null);
        }

        private void loadScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openScript.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.scriptText = System.IO.File.ReadAllText(openScript.FileName);
            }
        }
    }
}
