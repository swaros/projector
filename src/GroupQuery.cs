using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Projector.Script;
//using Microsoft.Office.Interop.Excel;


namespace Projector
{

    public partial class GroupQuery : Form
    {

        public struct asyncSql
        {
            public Profil dbProfil;
            public string sql;
            public MySql.Data.MySqlClient.MySqlDataReader result;
            public int workerID;
            public string lastError;
            public ListView resultListView;
            public int status;
            public bool haseRows;
            public bool fireSingleShots; // if true querys was seperated by ';' and fired
            //public List<string> singleShots;
        }

        private List<string> knownGroups;
        private Profil profil = new Profil();
        private string currentProfileName = "";
        HighlighterMysql highlight = new HighlighterMysql();

        public Boolean imHere = true;
        // helper for async
        int runningMaxEvents = 0;
        int runningCurrentEvents = 0;

        
        private GroupProfilWorker GroupSetup = new GroupProfilWorker(new PConfig());

        //ListViewExport lvExporter;

        List<ListView> listViewExports;
        List<string> listLabelExports;
        //System.ComponentModel.BackgroundWorker parserThread = new System.ComponentModel.BackgroundWorker();
        List<BackgroundWorker> ThreadList = new List<BackgroundWorker>();
        List<asyncSql> ThreadParams = new List<asyncSql>();

        public GroupQuery()
        {
            InitializeComponent();
            this.knownGroups = GroupSetup.getAllGroups();
            foreach (string grpName in this.knownGroups)
            {
                groupSelectBox.Items.Add(grpName);
            }
            /*
            XmlSetup pSetup = new XmlSetup();
            pSetup.setFileName("profileGroups.xml");
            pSetup.loadXml();

            settings = pSetup.getHashMap();

            foreach (DictionaryEntry de in settings)
            {
                //de.Key.ToString();                
                //de.Value.ToString();
                groupSelectBox.Items.Add(de.Key.ToString());
            }
             */
        }

        public void setGroup(string groupname)
        {
            try
            {
                groupSelectBox.Text = groupname;
            }
            catch
            {

            }
            
        }

        public void setSql(string sql)
        {
            try
            {
                this.richTextBox1.Text = sql;
            }
            catch
            {

            }
            
        }

        //###################   reflection script #################

        private ReflectionScript onDoneScript;
        private ReflectionScript onDoneExportCsv;
        private ReflectionScript onCloseScript;
        private ReflectionScript onStatusScript;

        private string statusMessageVar;

        public void execute()
        {
            button1_Click(null, null);
        }

        private void iterateOverProfiles()
        {
            iterateOverProfiles(false);
        }

        public void OnStatusChange(string statusVar,ReflectionScript onDoneReading)
        {
            this.statusMessageVar = statusVar;
            this.onStatusScript = onDoneReading;
        }

        public void OnDoneReading(ReflectionScript onDoneReading){
            this.onDoneScript = onDoneReading;
        }

        public void OnDoneCsvExport(ReflectionScript onDoneReading)
        {
            this.onDoneExportCsv = onDoneReading;
        }

        public void OnClose(ReflectionScript onDoneReading)
        {
            this.onCloseScript = onDoneReading;
        }

        public void exportCsv()
        {
            if (saveCvsFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                exportCsvFiles(saveCvsFile.FileName);

            }
            onDoneCsv();
        }


        public void exportCsvToFile(string filename)
        {

            try
            {
                exportCsvFiles(filename);
               
            }
            catch
            {

            }

            onDoneCsv();
            
        }

        public void showMe()
        {
            this.Show();
        }

        public void closeMe()
        {
            this.Close();
        }

        private void onStatusChange(string message)
        {
            if (this.onStatusScript != null)
            {
                if (this.statusMessageVar != null)
                {
                    this.onStatusScript.createOrUpdateStringVar("&" + this.statusMessageVar, message);
                }
                RefScriptExecute executer = new RefScriptExecute(this.onStatusScript, this);
                executer.run();
            }
        }

        private void onDoneCsv()
        {
            if (this.onDoneExportCsv!= null)
            {
                RefScriptExecute executer = new RefScriptExecute(this.onDoneExportCsv, this);
                executer.run();
            }
        }

        private void onCloseFomrmScr()
        {
            if (this.onCloseScript != null)
            {
                RefScriptExecute executer = new RefScriptExecute(this.onCloseScript, this);
                executer.run();
            }
        }

        private void onDone()
        {
            if (this.onDoneScript != null)
            {
                RefScriptExecute executer = new RefScriptExecute(this.onDoneScript, this);
                executer.run();
            }
        }
        //########################
        private void iterateOverProfiles(bool singleFire)
        {
            toolStripButton1.Enabled = false;
            richTextBox1.Enabled = false;
            MainView.Controls.Clear();
            sqlProgress.Visible = true;
            listViewExports = new List<ListView>();
            listLabelExports = new List<string>();

            sqlProgress.Maximum = databasesListView.Items.Count;

            runningMaxEvents = 0;
            runningCurrentEvents = 0;
            for (int i = 0; i < databasesListView.Items.Count; i++)
            {
                if (databasesListView.Items[i].Checked)
                {
                    currentProfileName = databasesListView.Items[i].Text;
                    profil.changeProfil(databasesListView.Items[i].Text);
                   
                    toolStrip1.Refresh();
                    statusLabel.Text = "Running SQL @ " + currentProfileName + "  " + i + " / " + (databasesListView.Items.Count -1);
                    
                    
                    //fireSql(profil, richTextBox1.Text);
                    if (runAsync.Checked) fireAsyncQuery(profil, richTextBox1.Text, singleFire);
                    else
                    {
                        sqlProgress.Value = i + 1;
                        Application.DoEvents();
                        fireSql(profil, richTextBox1.Text);
                    }
                }
            }
            statusLabel.Text = "Idle";
            if (!runAsync.Checked)
            {
                sqlProgress.Visible = false;
                toolStripButton1.Enabled = true;
                richTextBox1.Enabled = true;
            }

            /*
            if (lvExporter != null)
            {
                lvExporter.GenerateDynamicExcelSheet();
                lvExporter = null;
            }
            */

        }


        private void fireAsyncQuery(Profil dbProfil, string sql)
        {
            fireAsyncQuery(dbProfil, sql, false);
        }

        private void fireAsyncQuery(Profil dbProfil, string sql, bool fireSingle)
        {
            asyncSql bgWorkerObj = new asyncSql();
            bgWorkerObj.dbProfil = new Profil(dbProfil.getName());
            bgWorkerObj.dbProfil.setProperty("db_username", dbProfil.getProperty("db_username"));
            bgWorkerObj.dbProfil.setProperty("db_password", dbProfil.getProperty("db_password"));
            bgWorkerObj.dbProfil.setProperty("db_host", dbProfil.getProperty("db_host"));
            bgWorkerObj.dbProfil.setProperty("db_shema", dbProfil.getProperty("db_shema"));
            bgWorkerObj.fireSingleShots = fireSingle;

            bgWorkerObj.sql = sql;
            int curr = ThreadParams.Count;
            ThreadParams.Add(bgWorkerObj);
            addNewAsync(ThreadParams[curr]);
        }

        private void addNewAsync(asyncSql data)
        {
            BackgroundWorker worker = new BackgroundWorker();

            

            worker.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            worker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            worker.ProgressChanged +=
                new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            worker.WorkerReportsProgress = true;

            int id = ThreadList.Count;

            data.workerID = id;

            ThreadList.Add(worker);

            ThreadList[id].RunWorkerAsync(data);
            runningMaxEvents++;
            //worker.RunWorkerAsync(data);

        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            sqlProgress.Maximum = runningMaxEvents;
            sqlProgress.Value = runningCurrentEvents;
           
            toolStrip1.Refresh();

            runningCurrentEvents++;
            statusLabel.Text = "Done SQL @ " + currentProfileName + "  " + runningCurrentEvents + " / " + runningMaxEvents;
            statusStrip1.Refresh();

            onStatusChange(statusLabel.Text);
            if (runningCurrentEvents == runningMaxEvents)
            {
                sqlProgress.Visible = false;
                toolStripButton1.Enabled = true;
                richTextBox1.Enabled = true;
                exportEnable.Enabled = true;
                ThreadList = new List<BackgroundWorker>();

                this.onDone();
            }
            Application.DoEvents();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            asyncSql bgWorkerObj = (asyncSql)e.Argument;
            Profil dbProfil = bgWorkerObj.dbProfil;
            string sql = bgWorkerObj.sql;

            MysqlHandler database = new MysqlHandler(dbProfil);
            database.connect();
            if (database.isConnected())
            {
                //database.beginTransaction();
                bgWorkerObj.status = 1;
                //database.query(sql);
                MySql.Data.MySqlClient.MySqlDataReader dbResult;
                if (bgWorkerObj.fireSingleShots)
                {
                    dbResult = null;
                    string[] singleShots = sql.Split(';');
                    for (int i = 0; i < singleShots.Length; i++)
                    {
                        dbResult = database.sql_select(singleShots[i]);
                    }

                    
                }
                else
                {
                    dbResult = database.sql_select(sql);
                }
                bgWorkerObj.result = dbResult;
                bgWorkerObj.haseRows = (dbResult != null && dbResult.HasRows == true);
                bgWorkerObj.lastError = database.lastSqlErrorMessage;
/*
                if (database.lastSqlErrorMessage != "")
                {
                    // database.rollBack();
                }
                else
                {
                   // database.commit();
                }
*/
                bgWorkerObj.status = 2;
                bgWorkerObj.resultListView = new ListView();
                if (bgWorkerObj.haseRows)
                    database.sql_data2ListView(dbResult, bgWorkerObj.resultListView);
                
                ThreadList[bgWorkerObj.workerID].ReportProgress(bgWorkerObj.workerID, bgWorkerObj);
            }
            
            database.disConnect();
        }

        //ProgressChangedEventArgs
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                asyncSql bgWorkerObj = (asyncSql)e.UserState;
                Profil dbProfil = bgWorkerObj.dbProfil;
                string sql = bgWorkerObj.sql;


                ListViewWorker listTool = new ListViewWorker();
                GroupBox instGroup = new GroupBox();
                instGroup.Width = MainView.Width - 30;
                instGroup.BackColor = SystemColors.Info;
                
                currentProfileName = dbProfil.getName();
                instGroup.Text = dbProfil.getName();
                instGroup.AutoSize = true;

                Label infoData = new Label();
                infoData.AutoSize = true;
                infoData.Text = dbProfil.getProperty("db_username") + "@" + dbProfil.getProperty("db_host") + "/" + dbProfil.getProperty("db_schema");
                infoData.Top = 15;
                infoData.Left = 15;

                ListView resultView = bgWorkerObj.resultListView;

                resultView.View = View.Details;

                resultView.Width = MainView.Width - 60;
                resultView.Height = 300;
                resultView.Left = 10;
                resultView.Top = 30;

                resultView.FullRowSelect = true;

                resultView.GridLines = true;

                if (bgWorkerObj.lastError != "")
                {
                    Label noData = new Label();
                    noData.Top = 30;
                    noData.Left = 15;
                    noData.Text = bgWorkerObj.lastError;
                    noData.AutoSize = true;
                    noData.Font = new Font("Courier New", 10, FontStyle.Bold);
                    instGroup.Controls.Add(noData);
                    instGroup.Controls.Add(infoData);
                    instGroup.Height = 60;
                    instGroup.BackColor = Color.LightPink;
                    this.onStatusChange("Error on " + infoData.Text);
                }
                else
                {

                    if (bgWorkerObj.haseRows)
                    {

                       
                        for (int i = 0; i < resultView.Columns.Count; i++)
                        {
                            resultView.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

                        }
                        listTool.setRowColors(resultView, Color.LightBlue, Color.LightGreen);

                        resultView.Height = (resultView.Items.Count * 13) + 50;
                        if (resultView.Height > 700) resultView.Height = 700;
                       
                        listLabelExports.Add(instGroup.Text);
                        listViewExports.Add(resultView);
                        instGroup.Controls.Add(resultView);
                        instGroup.Controls.Add(infoData);

                        this.onStatusChange(resultView.Items.Count + " rows from " + infoData.Text);

                    }
                    else
                    {
                        Label noData = new Label();
                        noData.Top = 30;
                        noData.Left = 15;
                        noData.Text = "No Result for this query ";
                        noData.AutoSize = true;
                        instGroup.Controls.Add(noData);
                        instGroup.Height = 60;
                        instGroup.BackColor = Color.LightYellow;
                        instGroup.Controls.Add(infoData);

                        this.onStatusChange(" no rows reported from " + infoData.Text);
                    }

                }



                MainView.Controls.Add(instGroup);
                MainView.Refresh();
                Application.DoEvents();

            }
    
            
        }

        private void fireSql(Profil dbProfil, string sql)
        {
            MysqlHandler database = new MysqlHandler(dbProfil);
            database.connect();

            if (database.isConnected())
            {
                ListViewWorker listTool = new ListViewWorker();
                GroupBox instGroup = new GroupBox();
                instGroup.Width = MainView.Width - 30;
                instGroup.BackColor = SystemColors.Info;
                //instGroup.Height = 350;
                instGroup.Text = currentProfileName;
                instGroup.AutoSize = true;

                Label infoData = new Label();
                infoData.AutoSize = true;
                infoData.Text = dbProfil.getProperty("db_username") + "@" + dbProfil.getProperty("db_host") + "/" + dbProfil.getProperty("db_schema");
                infoData.Top = 15;
                infoData.Left = 15;

                ListView resultView = new ListView();
                resultView.View = View.Details;

                resultView.Width = MainView.Width - 60;
                resultView.Height = 300;
                resultView.Left = 10;
                resultView.Top = 30;

                resultView.FullRowSelect = true;

                resultView.GridLines = true;
                
                MySql.Data.MySqlClient.MySqlDataReader dbResult = database.sql_select(sql);

                if (database.lastSqlErrorMessage != "")
                {
                    Label noData = new Label();
                    noData.Top = 30;
                    noData.Left = 15;
                    noData.Text = database.lastSqlErrorMessage;
                    noData.AutoSize = true;
                    noData.Font = new Font("Courier New", 10, FontStyle.Bold);
                    instGroup.Controls.Add(noData);
                    instGroup.Controls.Add(infoData);
                    instGroup.Height = 60;
                    instGroup.BackColor = Color.LightPink;

                }
                else
                {

                    if (dbResult != null && dbResult.HasRows)
                    {
                        database.sql_data2ListView(dbResult, resultView);
                        for (int i = 0; i < resultView.Columns.Count; i++)
                        {
                            resultView.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

                        }
                        listTool.setRowColors(resultView, Color.LightBlue, Color.LightGreen);

                        resultView.Height = (resultView.Items.Count * 13) + 50;
                        if (resultView.Height > 700) resultView.Height = 700;
                     
                        listLabelExports.Add(instGroup.Text);
                        listViewExports.Add(resultView);
                        instGroup.Controls.Add(resultView);
                        instGroup.Controls.Add(infoData);



                    }
                    else
                    {
                        Label noData = new Label();
                        noData.Top = 30;
                        noData.Left = 15;
                        noData.Text = "No Result for this query ";
                        noData.AutoSize = true;
                        instGroup.Controls.Add(noData);
                        instGroup.Height = 60;
                        instGroup.BackColor = Color.LightYellow;
                        instGroup.Controls.Add(infoData);
                    }

                }



                MainView.Controls.Add(instGroup);
                MainView.Refresh();


            }


            database.disConnect();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (toolHighlight.Checked) highlight.parse(richTextBox1);
            if (this.canRun()) iterateOverProfiles();
            exportEnable.Enabled = (listViewExports != null && listViewExports.Count > 0);
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
           if (toolHighlight.Checked) highlight.parse(richTextBox1);
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            if (toolHighlight.Checked) highlight.parse(richTextBox1);
        }

        private void groupSelectBox_TextChanged(object sender, EventArgs e)
        {
            if (knownGroups.Contains(groupSelectBox.Text))
            {
                List<string> profiles = GroupSetup.getGroupMember(groupSelectBox.Text);               
                databasesListView.Items.Clear();
                for (int i = 0; i < profiles.Count; i++)
                {
                    ListViewItem dbProfil = new ListViewItem();
                    dbProfil.Text = profiles[i];
                    dbProfil.Checked = true;
                    databasesListView.Items.Add(dbProfil);

                }

            }
            toolStripButton1.Enabled = (canRun());
        }

        private void exportCsvFiles(string filename)
        {
            

            int pp = 0;
            ListViewWorker lw = new ListViewWorker();

            if (listViewExports == null)
            {
                MessageBox.Show("No Data to export. Make sure youre Query is executed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (ListView resultView in listViewExports)
            {
                resultView.Columns.Add("Group");
                statusLabel.Text = "writing group no.: " + pp;
                onStatusChange("csv ... write group no.: " + pp + "  " + filename);
                statusLabel.Invalidate();
                Application.DoEvents();
                for (int ai = 0; ai < resultView.Items.Count; ai++)
                {
                    resultView.Items[ai].SubItems.Add(listLabelExports[pp]);
                }

                if (pp == 0)
                {
                    string firstCsv = lw.exportCsv(resultView, true);
                    System.IO.File.WriteAllText(filename, firstCsv);
                }
                else
                {
                    string nextCsv = lw.exportCsv(resultView, false);
                    System.IO.File.AppendAllText(filename, nextCsv);
                }

                pp++;
            }
            statusLabel.Text = "writing DONE ("+ filename +") ";
            
        }

        private ListView buildSumList()
        {
            int pp = 0;

            ListView sumerizeListView = new ListView();

            ListViewWorker lw = new ListViewWorker();
            foreach (ListView resultView in listViewExports)
            {
                resultView.Columns.Add("Group");
                statusLabel.Text = "writing group no.: " + pp;
                statusLabel.Invalidate();
                Application.DoEvents();

                for (int ai = 0; ai < resultView.Items.Count; ai++)
                {
                    resultView.Items[ai].SubItems.Add(listLabelExports[pp]);
                }

                if (sumerizeListView.Columns.Count == 0)
                {
                    lw.copyListView(resultView, sumerizeListView);
                }
                else
                {
                    lw.addListView(resultView, sumerizeListView, 0, 0);
                }



                
                statusLabel.Text = "writing group no.: " + pp;
                statusLabel.Invalidate();
                Application.DoEvents();
                for (int ai = 0; ai < resultView.Items.Count; ai++)
                {
                    resultView.Items[ai].SubItems.Add(listLabelExports[pp]);
                }

                

                pp++;
            }
            statusLabel.Text = "build done ";
            return sumerizeListView;
        }



        private void exportEnable_Click(object sender, EventArgs e)
        {
            /*
            splitContainer1.Enabled = false;
            ListViewExport lvExporter = new ListViewExport("export groups");
            int pp = 0;
            foreach (ListView resultView in listViewExports)
            {
                resultView.Columns.Add("Group");

                for (int ai = 0; ai < resultView.Items.Count; ai++)
                {
                    resultView.Items[ai].SubItems.Add(listLabelExports[pp]);
                }
                if (pp == 0) lvExporter.assignListView(resultView);
                else lvExporter.addListView(resultView, listLabelExports[pp]);
                pp++;
            }
            
            lvExporter.GenerateDynamicExcelSheet();
            lvExporter = null;
            splitContainer1.Enabled = true; 
            */
            /*
            ListViewReportContainer data = new ListViewReportContainer();
            data.views = listViewExports;
            data.labels = listLabelExports;

            excelExport.RunWorkerAsync(data);
             */
        }

        private void excelExport_DoWork(object sender, DoWorkEventArgs e)
        {
            /*
            ListViewReportContainer data = (ListViewReportContainer) e.Argument;
            ListViewExport lvExporter = new ListViewExport("export groups");
            List<ListView> lViewExport = data.views;
            List<string> lLabels = data.labels;

            ListViewWorker lvWorker = new ListViewWorker();

            int pp = 0;
            foreach (ListView origLiView in lViewExport)
            {
                ListView resultView = new ListView();
                lvWorker.copyListView(origLiView, resultView);
                resultView.Columns.Add("Group");

                for (int ai = 0; ai < resultView.Items.Count; ai++)
                {
                    resultView.Items[ai].SubItems.Add(lLabels[pp]);
                }
                if (pp == 0) lvExporter.assignListView(resultView);
                else lvExporter.addListView(resultView, lLabels[pp]);
                pp++;
                Application.DoEvents();
            }

            lvExporter.GenerateDynamicExcelSheet();
            lvExporter = null;
             */
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < queryListing.Items.Count; i++)
            {
                richTextBox1.Text = queryListing.Items[i].ToString();
                iterateOverProfiles();
            }
        }

        private void queryListing_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = queryListing.Text;
            if (toolHighlight.Checked)  highlight.parse(richTextBox1);
        }

        private void cSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveCvsFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                exportCsvFiles(saveCvsFile.FileName);
            }
        }

        private void exportEnable_ButtonClick(object sender, EventArgs e)
        {
            if (saveCvsFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                exportCsvFiles(saveCvsFile.FileName);
               
            }
        }

        private void LoadSqlButton_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            if (openSqlFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sql = System.IO.File.ReadAllText(openSqlFileDialog.FileName);
                richTextBox1.Text = sql;
                if (toolHighlight.Checked) highlight.parse(richTextBox1);
            }
        }

        private void loadSplitAndFireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openSqlFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sql = System.IO.File.ReadAllText(openSqlFileDialog.FileName);
                string massSqlScript = this.buildScriptByMassQuery(sql);
                ScriptWriter scrWriter = new ScriptWriter(this);
                //scrWriter.codeBox.Text = massSqlScript;
                scrWriter.setCode(massSqlScript);

                scrWriter.Show();
                //string[] queryListing = sql.Split(';');
                /*
                if (MessageBox.Show("firing " + queryListing.Length + " querys ? Async will be temporary disabled", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    
                    richTextBox1.Text = sql;
                    iterateOverProfiles(true);
                    richTextBox1.Text = "";
                }*/

            }
        }

        private string buildScriptByMassQuery(string massSql)
        {
            string scrpt = "NEW GroupQuery MassSend" + System.Environment.NewLine;
            scrpt += "SET MAX_WAIT 0 " + System.Environment.NewLine;
            scrpt += "MassSend setGroup " + groupBox1.Text + System.Environment.NewLine;
            scrpt += "MassSend showMe " + System.Environment.NewLine;
            string[] queryListing = massSql.Split(';');
            
            foreach (string sql in queryListing)
            {
                scrpt += "#next query " + System.Environment.NewLine;
                scrpt += "MassSend setSql " + "\"" + sql + "\"" +  System.Environment.NewLine;
                scrpt += "REG \"MASS_SEND\" " +  System.Environment.NewLine;
                scrpt += "MassSend OnDoneReading { UNREG \"MASS_SEND\" } " + System.Environment.NewLine;
                scrpt += "MassSend execute " + System.Environment.NewLine;
                scrpt += "waitfor \"MASS_SEND\" " +  System.Environment.NewLine;
                scrpt += System.Environment.NewLine;
            }

            return scrpt;
        }


        private void csvExporter_DoWork(object sender, DoWorkEventArgs e)
        {
            string filename = (string)e.Argument;

            int pp = 0;
            ListViewWorker lw = new ListViewWorker();
            foreach (ListView resultView in listViewExports)
            {
                resultView.Columns.Add("Group");

                for (int ai = 0; ai < resultView.Items.Count; ai++)
                {
                    resultView.Items[ai].SubItems.Add(listLabelExports[pp]);
                }

                if (pp == 0)
                {
                    string firstCsv = lw.exportCsv(resultView, true);
                    System.IO.File.WriteAllText(filename, firstCsv);
                }
                else
                {
                    string nextCsv = lw.exportCsv(resultView, false);
                    System.IO.File.AppendAllText(filename, nextCsv);
                }

                pp++;
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < databasesListView.Items.Count; i++)
            {
                databasesListView.Items[i].Checked = !databasesListView.Items[i].Checked;
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < databasesListView.Items.Count; i++)
            {
                databasesListView.Items[i].Checked = true;
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < databasesListView.Items.Count; i++)
            {
                databasesListView.Items[i].Checked = false;
            }
        }

        private Boolean canRun()
        {
            
            for (int i = 0; i < databasesListView.Items.Count; i++)
            {
                if (databasesListView.Items[i].Checked) return true;
            }
            return false;
        }

        private void databasesListView_MouseClick(object sender, MouseEventArgs e)
        {
            toolStripButton1.Enabled = (canRun());
        }

        private void GroupQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            onCloseFomrmScr();
            
        }

        public ListView getResultAsListView()
        {
            ListView resultView = buildSumList();

            resultView.View = View.Details;

            resultView.Width = MainView.Width - 60;
            resultView.Height = MainView.Width;
            resultView.Left = 10;
            resultView.Top = 30;
            resultView.AutoArrange = true;

            resultView.FullRowSelect = true;

            resultView.GridLines = true;
            ListView CopyThat = new ListView();
            ListViewWorker Worker = new ListViewWorker();
            Worker.copyListView(resultView, CopyThat);
            //return resultView;
            return CopyThat;
        }


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ListView resultView = this.getResultAsListView();
            MainView.Controls.Clear();
            MainView.Controls.Add(resultView);
        }
    }
}
