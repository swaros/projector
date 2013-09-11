using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Projector
{
    public partial class MysqlSensor : Form
    {
        public Profil sensorProfil;
        private MysqlHandler database=null;
        Hashtable usedSensors = new Hashtable();
        
        private bool isRunning = false;

        private int lastHitX =0;

       

        List<PanelDrawing> panelSensors = new List<PanelDrawing>();

        public MysqlSensor()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


        public void saveSensors(string filename)
        {
            string saveStr = "";
            string add = "";
            for (int i = 0; i < sensorListing.CheckedItems.Count; i++)
            {
                string data = sensorListing.CheckedItems[i].ToString();
                saveStr += add + data;
                add = ";";


            }
            System.IO.File.WriteAllText(filename, saveStr);
        }


        public void loadSensors(string filename)
        {
            StreamReader streamReader = new StreamReader(filename);
            string sensorLists = streamReader.ReadToEnd();
            streamReader.Close();

            string[] sensors = sensorLists.Split(';');
            for (int i = 0; i < sensorListing.Items.Count; i++)
            {
                string data = sensorListing.Items[i].ToString();
                if (sensors.Contains<string>(data))
                {
                    sensorListing.SetItemChecked(i, true);
                }


            }
 
        }

        public void getStartupData()
        {
            database = new MysqlHandler(sensorProfil);
            database.connect();
            
            if (database != null)
            {
                MySql.Data.MySqlClient.MySqlDataReader reader = database.sql_select("SHOW STATUS");

                string name   = "";
                string value = "";
                sensorListing.Items.Clear();
                while (reader!=null && reader.Read())
                {
                    //userid = atAll.GetInt64(atAll.GetOrdinal("gladi_userid"));
                    //Data.Add();
                    name = reader.GetString(0);
                    value = reader.GetString(1);

                    sensorListing.Items.Add(name);
                    addToSensorTree(name);

                    PanelDrawing sensView = new PanelDrawing();
                    sensView.Name = name;
                    sensView.Width = flowLayout.Width - 6;
                    sensView.Height = 120;
                    sensView.BackColor = Color.FromArgb(0,33,0);
                    sensView.BorderStyle = BorderStyle.Fixed3D;
                    sensView.lineColor = Color.FromArgb(0,254,0);
                    sensView.positivColor = Color.FromArgb(0, 64, 0);
                    sensView.negativColor = Color.FromArgb(54,0,0);
                    sensView.Visible = false;
                    sensView.Dock = DockStyle.Top;

                    panelSensors.Add(sensView);
                    flowLayout.Controls.Add(panelSensors[panelSensors.Count-1]);

                }
                if (reader!=null) reader.Close();

            }
        }

        public void addToSensorTree(string what)
        {
            string[] sensors = what.Split('_');
            bool added = false;
            for (int i = 0; i < SensorTree.Nodes.Count; i++){
                if (SensorTree.Nodes[i].Text == sensors[0] && sensors.Length>0)
                {
                    SensorTree.Nodes[i].Nodes.Add(what);
                    added = true;
                }
            }

            if (!added)
            {
                TreeNode tmp = new TreeNode();
                tmp.Text = sensors[0];
                tmp.Nodes.Add(what);
                tmp.ImageIndex = 1;
                tmp.SelectedImageIndex = 1;
                SensorTree.Nodes.Add(tmp);
            }
          
        }

        private PanelDrawing FindSensorByName(string searchingDrawing)
        {
            for (int i = 0; i < panelSensors.Count; i++)
            {
                if (panelSensors[i].Name == searchingDrawing) return panelSensors[i];
            }
            return null;
        }

        private int FindSensorIndexByName(string searchingDrawing)
        {
            for (int i = 0; i < panelSensors.Count; i++)
            {
                if (panelSensors[i].Name == searchingDrawing) return i;
            }
            return -1;
        }

        public void refreshSensors()
        {
            isRunning = true;
            usedSensors.Clear();
            for (int i = 0; i < sensorListing.CheckedItems.Count; i++)
            {
                string data = sensorListing.CheckedItems[i].ToString();
                usedSensors.Add(data, i);
                
            }

            int sensorCount = sensorListing.CheckedItems.Count - 1;
            int viewheight = 35;
            if (sensorCount > 0)
            {
                viewheight = (flowLayout.Height-10) / sensorCount;
            }
            if (database != null)
            {
                MySql.Data.MySqlClient.MySqlDataReader reader = database.sql_select("SHOW STATUS");

                string name = "";
                string value = "";
                //triggerView.Items.Clear();
                for (int i = 0; i < triggerView.Items.Count; i++)
                {
                    triggerView.Items[i].ImageIndex = 100;
                    triggerView.Items[i].ForeColor = Color.Navy;
                }
                if (reader != null)
                {
                    while (reader.Read())
                    {

                        name = reader.GetString(0);
                        value = reader.GetString(1);
                        if (usedSensors.ContainsKey(name))
                        {
                            ListViewItem tmp = new ListViewItem();
                            tmp.Text = name;
                            tmp.ImageIndex = 1;
                            tmp.SubItems.Add(value);
                            tmp.SubItems.Add("0");
                            bool found = false;
                            for (int i = 0; i < triggerView.Items.Count; i++)
                            {
                                if (triggerView.Items[i].Text == tmp.Text)
                                {
                                    found = true;
                                    Double aval =0;
                                    try
                                    {
                                         aval = Double.Parse(tmp.SubItems[1].Text);
                                    }
                                    catch (Exception)
                                    {
                                        
                                            //throw;
                                    }
                                    Double bval = 0;
                                    try
                                    {
                                        bval = Double.Parse(triggerView.Items[i].SubItems[1].Text);
                                    }
                                    catch (Exception)
                                    {
                                        
                                        //throw;
                                    }
                                    triggerView.Items[i].SubItems[1].Text = tmp.SubItems[1].Text;
                                    Double cval = aval - bval;
                                    triggerView.Items[i].SubItems[2].Text = cval + "";

                                    triggerView.Items[i].ImageIndex = 1;

                                    if (cval < 0) triggerView.Items[i].ForeColor = Color.Red;
                                    if (cval > 0) triggerView.Items[i].ForeColor = Color.Blue;
                                    if (cval == 0) triggerView.Items[i].ForeColor = Color.Orange;


                                    int t = FindSensorIndexByName(tmp.Text);
                                    if (t > -1)
                                    {
                                        panelSensors[i].Height = viewheight;
                                        panelSensors[t].addValue(cval);
                                        panelSensors[t].Visible = true;
                                        panelSensors[t].Label = tmp.Text + " : " + cval;
                                        panelSensors[t].Invalidate();

                                    }


                                }
                            }


                            if (!found) triggerView.Items.Add(tmp);

                        }

                    }



                    reader.Close();
                }
                // removing deactivated sensors
                for (int i = 0; i < triggerView.Items.Count; i++)
                {
                    if (triggerView.Items[i].ForeColor == Color.Navy)
                    {
                        int t = FindSensorIndexByName(triggerView.Items[i].Text);
                        if (t > -1) panelSensors[t].Visible = false;
                        triggerView.Items[i].Remove();
                    }
                }

            }
            isRunning = false;

        }

        private void MysqlSensor_FormClosing(object sender, FormClosingEventArgs e)
        {
            database.disConnect();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!isRunning) refreshSensors();
        }

        private void sensorListing_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //string tmp = e.CurrentValue.ToString();
            //usedSensors.Add(

            

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !toolStripButton1.Checked;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                saveSensors(saveFile.FileName);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                loadSensors(openFile.FileName);
            }
        }

        private void triggerView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        }

       

        private void SensorTree_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            if (SensorTree.SelectedNode != null && SensorTree.SelectedNode.Level == 1 && lastHitX!=e.X)
               {
                    DoDragDrop(SensorTree.SelectedNode.Text, DragDropEffects.Copy | DragDropEffects.Move);
                    
               }*/
            
        }


        private void setSensor(string name)
        {
            for (int i = 0; i < sensorListing.Items.Count; i++)
            {
                string data = sensorListing.Items[i].ToString();
                if (data==name)
                {
                    sensorListing.SetItemChecked(i, true);
                }


            }
        }

        private void triggerView_DragDrop(object sender, DragEventArgs e)
        {
            string res =  e.Data.GetData(DataFormats.Text).ToString();
            setSensor(res);
        }

        private void SensorTree_Click(object sender, EventArgs e)
        {

        }

        private void sensorRefresh_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void SensorTree_MouseMove(object sender, MouseEventArgs e)
        {
            if (SensorTree.SelectedNode != null && SensorTree.SelectedNode.Level == 1 && e.Button==MouseButtons.Left)
            {
                DoDragDrop(SensorTree.SelectedNode.Text, DragDropEffects.Copy | DragDropEffects.Move);

            }
        }

        private void flowLayout_Resize(object sender, EventArgs e)
        {
            int sensorCount = sensorListing.CheckedItems.Count;
            int viewheight = 35;
            if (sensorCount > 0)
            {
                viewheight = (flowLayout.Height - 10) / sensorCount;
            }
            for (int i = 0; i < panelSensors.Count; i++)
            {
                panelSensors[i].Width = flowLayout.Width - (flowLayout.Margin.Left + flowLayout.Margin.Right);
                panelSensors[i].Height = viewheight - (flowLayout.Margin.Top + flowLayout.Margin.Bottom);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SetTimer st = new SetTimer();
            st.timerLabel.Text = (task.Interval / 1000) + " sec";
            st.trackBar.Value = task.Interval / 1000;
            if (st.ShowDialog() == DialogResult.OK)
            {
                task.Stop();
                task.Interval = st.trackBar.Value * 1000;
                task.Start();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel1Collapsed = !splitContainer2.Panel1Collapsed;
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel1Collapsed;
           /* if (splitContainer2.Panel1Collapsed)
            {

            }*/
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            splitContainer3.Panel1Collapsed = !toolStripButton6.Checked;
        }


    }
}
