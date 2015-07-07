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
    public partial class mysqlWatch : Form
    {
        ListView tmpListView = new ListView();
        public Profil watchProfil;
        private string dbusername = null;
        private string dbpassword = null;
        private string dbschema = null;
        private string dbhost = null;
        private string lastSelectedId = "";
        public mysqlWatch()
        {
            
            InitializeComponent();
            
            connectionLabel.Text = "waiting...";   
        }

        private void mysql_watcher_DoWork(object sender, DoWorkEventArgs e)
        {
            ListView retObj = (ListView)e.Argument;
            //ListView retObj = new ListView();

            MysqlHandler mysql = new MysqlHandler(watchProfil);

            mysql.connect();

            string sql = "show full processlist";
            MySql.Data.MySqlClient.MySqlDataReader rankings = mysql.sql_select(sql);
            if (retObj != null) mysql.sql_data2ListView(rankings, retObj);

            mysql.disConnect();
        }

        private void traceView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void mysql_watcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mysql_watch_list.Items.Clear();
            mysql_watch_list.Columns.Clear();

            int group_at = 5;
            int group_at_source = 2;

            if (host_group.Checked)
            {
                group_at = 5;
                group_at_source = 2;
            }
            else
            {
                group_at = 4;
                group_at_source = 0;
            }

            if (!sqL_store_data.Checked) sql_view.Items.Clear();

            for (int a = 0; a < tmpListView.Columns.Count; a++)
            {
                ColumnHeader tmp = new ColumnHeader();
                tmp.Text = tmpListView.Columns[a].Text;
                mysql_watch_list.Columns.Add(tmp);
            }

            int queryTime = 0;

            bool lastIdFound = false;

            for (int i = 0; i < tmpListView.Items.Count; i++)
            {
                ListViewItem tmp2 = new ListViewItem();
                tmp2 = (ListViewItem)tmpListView.Items[i].Clone();

                if (show_sleeps.Checked || tmp2.SubItems[4].Text != "Sleep") mysql_watch_list.Items.Add(tmp2);

                if (tmp2.SubItems[6].Text.Length > 1 && tmp2.SubItems[6].Text != "null")
                {
                    ListViewItem tmp3 = new ListViewItem();
                    tmp3.Text = tmp2.SubItems[6].Text;
                    tmp3.SubItems.Add(tmp2.SubItems[7].Text);
                    tmp3.SubItems.Add(tmp2.SubItems[5].Text);
                    tmp3.SubItems.Add(DateTime.Now.ToLongTimeString());
                    tmp3.SubItems.Add(tmp2.SubItems[0].Text);
                    tmp3.SubItems.Add(tmp2.SubItems[2].Text, Color.DarkGreen, Color.Transparent, new Font("Arial", 8));
                    tmp3.SubItems.Add("1");


                    if (tmp3.SubItems[4].Text == lastSelectedId)
                    {
                        tmp3.BackColor = Color.PaleGoldenrod;
                        lastIdFound = true;
                    }
                   


                    try
                    {
                        queryTime = int.Parse(tmp2.SubItems[5].Text);
                    }
                    catch (Exception)
                    {
                        queryTime = 0;
                        //throw;
                    }

                    if (queryTime >= slow_querys.Value)
                    {
                        if (group.Checked && sql_view.Items.Count>0)
                        {
                            bool found = false;
                            for (int m = 0; m < sql_view.Items.Count; m++)
                            {
                                
                                string id = sql_view.Items[m].SubItems[group_at].Text;
                                string compare = tmp2.SubItems[group_at_source].Text;
                                int count = int.Parse(sql_view.Items[m].SubItems[6].Text);

                                if (count > 1)
                                {
                                    sql_view.Items[m].BackColor = Color.Cyan;
                                }

                                


                                // hostname aufgliedern
                                if (group_at_source == 2)
                                {
                                    string[] compareAr = compare.Split('.');
                                    compare = compareAr[0];
                                }

                                if (group_at == 5)
                                {
                                    string[] idAr = id.Split('.');
                                    id = idAr[0];
                                }

                                if (id.Length > 0 && id == compare)
                                {
                                    //sql_view.Items[m].SubItems[3].Text = "replaced";
                                    
                                    tmp3.SubItems[6].Text = "" + (count + 1);
                                    tmp3.BackColor = Color.Gold;
                                    sql_view.Items[m]= tmp3;
                                    found = true;
                                }
                                
                            }

                            if (!found)
                            {
                                sql_view.Items.Add(tmp3);
                            }

                        } else   sql_view.Items.Add(tmp3);
                    }
                }
            }

            if (!lastIdFound)
            {
                lastSelectedId = "";
                kill_btn.Text = "KILL";
            }
        }

        private void getProfilSettings()
        {
            dbusername = watchProfil.getProperty("db_username");
            dbpassword = watchProfil.getProperty("db_password");
            dbhost =  watchProfil.getProperty("db_host");
            dbschema = watchProfil.getProperty("db_schema");
            connectionLabel.Text = dbusername +"@"+ dbschema;
        }

        private void button73_Click(object sender, EventArgs e)
        {
            if (!mysql_watcher.IsBusy)
            {
                getProfilSettings();
                mysql_watcher.RunWorkerAsync(tmpListView);
            }
        }

        private void button74_Click(object sender, EventArgs e)
        {
            mysql_watch.Interval = (int)timerTickValue.Value;
            mysql_watch.Enabled = !mysql_watch.Enabled;
            getProfilSettings();
            if (mysql_watch.Enabled)
            {
                button74.Image = Projector.Properties.Resources.player_stop;
            }
            else
            {
                button74.Image = Projector.Properties.Resources.player_play_green;
            }
        }

        private void mysql_watch_Tick(object sender, EventArgs e)
        {
            if (!mysql_watcher.IsBusy)
            {
                mysql_watcher.RunWorkerAsync(tmpListView);
            }
        }

        private void show_processlist_CheckedChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = show_processlist.Checked;
        }

        private void sql_view_ItemActivate(object sender, EventArgs e)
        {
            
        }

        private void sql_view_Click(object sender, EventArgs e)
        {
            if (sql_view.SelectedItems.Count > 0)
            {
                show_sql.Text = sql_view.SelectedItems[0].SubItems[1].Text;
                lastSelectedId = sql_view.SelectedItems[0].SubItems[4].Text;
                kill_btn.Text = "KILL " + lastSelectedId;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lastSelectedId!="")
            {
               

               MysqlHandler mysql = new MysqlHandler(watchProfil);

               mysql.connect();

               string sql = "KILL " + lastSelectedId;

               mysql.sql_update(sql);               
               mysql.disConnect();

               kill_btn.Text = "KILL ";
                
            }
        }
    }
}
