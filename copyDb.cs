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
    public partial class copyDb : Form
    {
        public Profil sourceProfil;
        private MysqlHandler database_source = null;

        public Profil targetProfil;
        private MysqlHandler database_target = null;

        private ListView sourceMemList = new ListView();

        ScriptExecuter spexec;

        /** get alter table mofification ..if true then get alter statements only by new fields */
        Boolean modifyTableAddOnly = false;

        private string selectedTable = "";
        private string selectRightTable = "";
        private string lastSeltable = "";
        private string idLimiterSQL = "";
        private string searchString = "";
        private List<string> idLimiterList = new List<string>();

        private TablePropHandler TablePropertys = new TablePropHandler();

        private List<string> macroSource = new List<string>();
        private bool watchEventForMacro = false;

        private bool tablenamePrevToField = true;

        private List<String> ignoreOnMatchCheck = new List<string>();
        private List<String> whereOnMatchCheck = new List<string>();

        private string ExportRowCompareFileName = null;

        // consts

        const int FLAG_Q_SIZE = 0;
        const int FLAG_PROGRESS_STATE = 101;
        const int FLAG_PROGRESS_MAXIMUM = 102;

        public copyDb()
        {
            InitializeComponent();
            setOptionRightNotExits();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void addMacroCommand(string macro)
        {
            if (watchEventForMacro) macroSource.Add(macro);
        }

        private void handleSoucetablSelect(string sel)
        {
            sourceProfil = new Profil(sel);
            sourceProfil.loadProfileSettings();


            // check connection

            statusResult testResult = MysqlTools.checkConnection(sourceProfil);
            if (testResult.status)
            {


                // connect
                database_source = new MysqlHandler(sourceProfil);
                database_source.connect();
                MySql.Data.MySqlClient.MySqlDataReader result = database_source.sql_select("show tables");
                database_source.sql_data2ListView(result, sourcetabs);

                if (sourcetabs.Columns.Count > 0)
                {
                    sourcetabs.Columns[0].Width = sourcetabs.Width - 2;
                    sourcetabs.Columns[0].Text = sel;
                }

                database_source.disConnect();
                database_source.resetTableList();
                checkTableExistsOnTarget();

            }
            else
            {
                MessageBox.Show(this, testResult.message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        private void enableControlls()
        {
            RangeBox.Enabled = true;
            
            CompareBox2.Enabled = true;
            toolStripLeftTable.Enabled = true;

        }

        private void sourceSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            string sel = sourceSelect.SelectedItem.ToString();
            addMacroCommand("selectSourceTable:" + sel + ";");
            handleSoucetablSelect(sel);
            if (optionAutoProp.Checked) loadPropToolStripMenuItem_Click(sender, e);
            targetSelect.Enabled = true;
            button6.Enabled = true;

        }

        private void versionCheck()
        {
            if (database_source != null && database_target != null)
            {
                string v1 = database_source.getMysqlVersion();
                string v2 = database_target.getMysqlVersion();

                if (v1 != v2)
                {
                    if (!modifyTableAddOnly)
                    {
                        if (MessageBox.Show("Target and Source Database have no equals version. Source Server-Version is  "
                            + v1 +
                            " and Target Server-Version is "
                            + v2 +
                            ". that can possibly be problems when synchronizing Database structures. Do you want disable the MODIFY option for Structure Check?",
                            "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            modifyTableAddOnly = true;
                            alterTableForNewFieldsOnlyToolStripMenuItem.Checked = true;
                        }
                    }

                }
                else
                {
                    if (modifyTableAddOnly)
                    {
                        if (MessageBox.Show("Target and Source Database Server Version is equals. The MODIFY option for Alter Tables is disabled at this moment. Do you want activate it?",
                        "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            modifyTableAddOnly = false;
                            alterTableForNewFieldsOnlyToolStripMenuItem.Checked = false;
                        }
                    }
                }

            }

        }

        private void checkTableExistsOnTarget(bool fullcheck)
        {
            if (fullcheck)
            {
                database_target.resetTableList();
            }
            checkTableExistsOnTarget();
        }


        private void checkTableExistsOnTarget()
        {
            if (database_target != null)
            {

                database_target.connect();

                for (int i = 0; i < sourcetabs.Items.Count; i++)
                {
                    string tableName = sourcetabs.Items[i].Text;
                    if (database_target.tableExists(tableName))
                    {
                        sourcetabs.Items[i].ForeColor = Color.Green;
                        sourcetabs.Items[i].ImageIndex = 1;
                    }
                    else
                    {
                        sourcetabs.Items[i].ForeColor = Color.Red;
                        sourcetabs.Items[i].ImageIndex = 3;
                    }
                }
                versionCheck();
                database_target.disConnect();

            }
        }


        private void checkTableStructs()
        {
            if (database_target != null && database_source != null)
            {
                database_target.connect();
                database_source.connect();
                for (int i = 0; i < sourcetabs.Items.Count; i++)
                {
                    string tableName = sourcetabs.Items[i].Text;
                    if (database_target.tableExists(tableName))
                    {
                        sourcetabs.Items[i].ForeColor = Color.Green;
                        /*
                        List<MysqlStruct> SourceStruct = new List<MysqlStruct>();
                        List<MysqlStruct> TargetStruct = new List<MysqlStruct>();

                        SourceStruct = database_source.getAllFieldsStruct(tableName);
                        TargetStruct = database_target.getAllFieldsStruct(tableName);
                        */
                        string crSource = database_source.getTableCreationString(tableName);
                        string crTarget = database_target.getTableCreationString(tableName);

                        if (crSource == crTarget)
                        {
                            sourcetabs.Items[i].BackColor = Color.Transparent;
                            sourcetabs.Items[i].ImageIndex = 1;
                        }
                        else
                        {
                            sourcetabs.Items[i].BackColor = Color.Orange;
                            sourcetabs.Items[i].ImageIndex = 2;
                        }

                    }
                    else
                    {
                        sourcetabs.Items[i].ForeColor = Color.Red;
                        sourcetabs.Items[i].ImageIndex = 3;
                    }
                }
                database_target.disConnect();
                database_source.disConnect();
            }
        }



        private void sourcetabs_ItemActivate(object sender, EventArgs e)
        {
            target_view.Items.Clear();
            sourceContent.Items.Clear();


            // look at proerties 


            if (database_source != null)
            {
                if (sourcetabs.SelectedItems.Count > 0)
                {
                    string sql = "";

                    string TableName = sourcetabs.SelectedItems[0].Text;
                    selectedTable = TableName;

                    selectRightTable = TableName;
                    toolStripLeftTable.Text = TableName;
                    toolStripRightTable.Text = TableName;
                    leftTabLabel.Text = TableName;

                    selectettablLable.Text = selectedTable;



                    if (lastSeltable != selectedTable) whereList.Items.Clear();
                    if (whereSql.Text != "")
                    {
                        sql = "SELECT * FROM " + TableName + " WHERE " + whereSql.Text + " LIMIT " + limitStart.Value + "," + limitEnd.Value;
                        addMacroCommand("where:" + whereSql.Text + ";");
                    }
                    else
                    {
                        sql = "SELECT * FROM " + TableName + " LIMIT " + limitStart.Value + "," + limitEnd.Value;
                    }
                    addMacroCommand("activateTable:" + TableName + ";");
                    database_source.connect();

                    List<string> Fields = database_source.selectAsList("SHOW FULL COLUMNS FROM " + TableName, "Field");
                    compareBy.Items.Clear();
                    fieldNames.Items.Clear();
                    for (int i = 0; i < Fields.Count; i++)
                    {
                        if (sourceContent.SelectedItems.Count > 0)
                        {
                            fieldNames.Items.Add(Fields[i] + " = '" + sourceContent.SelectedItems[0].SubItems[i].Text + "'");
                        }
                        else
                        {
                            fieldNames.Items.Add(Fields[i]);

                        }
                        compareBy.Items.Add(Fields[i]);
                        if (lastSeltable != selectedTable) whereList.Items.Add(Fields[i]);
                    }


                    lastSeltable = selectedTable;
                    MySql.Data.MySqlClient.MySqlDataReader result = database_source.sql_select(sql);
                    if (result != null)
                    {


                        database_source.sql_data2ListView(result, sourceContent, true);
                    }
                    else
                    {
                        MessageBox.Show("No Result on Source");
                    }
                    database_source.disConnect();






                    // check other table

                    if (database_target != null)
                    {
                        database_target.connect();

                        MySql.Data.MySqlClient.MySqlDataReader resultTarget = database_target.sql_select(sql);
                        if (resultTarget != null)
                        {
                            database_target.sql_data2ListView(resultTarget, target_view, true);
                            setOptionRightExits();
                        }
                        else
                        {
                            //MessageBox.Show("No Result on Target");
                            setOptionRightNotExits();
                        }

                        database_target.disConnect();
                    }


                }
                updateIgnoresInColum();
                // get Properties
                loadTableProperies();
                rowsRead.Text = "Rows: " + sourceContent.Items.Count + " / " + limitEnd.Value;
                if (sourceContent.Items.Count == limitEnd.Value)
                {
                    rowsRead.ForeColor = Color.Red;
                }
                else
                {
                    rowsRead.ForeColor = Color.DarkGreen;
                }


                LeftTabInfoLabel.Text = "Rows: " + target_view.Items.Count + " / " + limitEnd.Value;
                if (target_view.Items.Count == limitEnd.Value)
                {
                    LeftTabInfoLabel.ForeColor = Color.Red;
                }
                else
                {
                    LeftTabInfoLabel.ForeColor = Color.DarkGreen;
                }

            }
            autosortColumns(target_view, 1);
            autosortColumns(sourceContent, 1);
        }


        private void autosortColumns(ListView listView1, int columnAutoSizeMode)
        {
            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                switch (columnAutoSizeMode)
                {
                    case 0:
                        listView1.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                        listView1.Columns[i].ImageIndex = 1;
                        break;

                    case 1:
                        listView1.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                        break;

                    default:
                        listView1.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.None);
                        break;

                }
            }
        }

        private int findViewColumByFieldname(ListView toList, string name)
        {
            int f = 0;
            for (int i = 0; i < toList.Columns.Count; i++)
            {
                toList.Columns[i].ImageIndex = 0;
                if (name == toList.Columns[i].Text)
                {
                    f = i;
                    toList.Columns[i].ImageIndex = 1;
                }
            }
            return f;
        }


        private string findNameInColum(ListView toList, int pos)
        {
            if (toList.Columns.Count > pos)
            {
                return toList.Columns[pos].Text;
            }

            return "";
        }

        private void compareViews()
        {
            prozessMsg msg = new prozessMsg();
            msg.Show();

            int leftCompare = 0;
            int rightCompare = 0;
            if (compareBy.Text != "")
            {
                leftCompare = findViewColumByFieldname(sourceContent, compareBy.Text);
                rightCompare = findViewColumByFieldname(target_view, compareBy.Text);
            }
            msg.progressBar.Maximum = sourceContent.Items.Count;
            for (int i = 0; i < sourceContent.Items.Count; i++)
            {
                //sourceContent.Items[i].SubItems[leftCompare].ForeColor = Color.Blue;

                string compareStr = sourceContent.Items[i].SubItems[leftCompare].Text;
                msg.progressBar.Value = i;
                msg.progressBar.Refresh();


                for (int p = 0; p < target_view.Items.Count; p++)
                {
                    if (compareStr == target_view.Items[p].SubItems[rightCompare].Text)
                    {
                        bool isEqual = true;

                        // mark as not compare
                        target_view.Items[p].BackColor = Color.Yellow;

                        for (int t = 0; t < sourceContent.Items[i].SubItems.Count; t++)
                        {
                            if (target_view.Items[p].SubItems.Count > t && t != leftCompare)
                            {
                                isEqual = (sourceContent.Items[i].SubItems[t].Text == target_view.Items[p].SubItems[t].Text);
                            }
                        }
                        if (isEqual) target_view.Items[p].BackColor = Color.Transparent;
                        else target_view.Items[p].BackColor = Color.LightPink;
                    }
                }
            }

            sourceContent.Refresh();
            msg.Close();
        }

        private void fieldNames_SelectedValueChanged(object sender, EventArgs e)
        {
            string sel = fieldNames.SelectedItem.ToString();
            string placed = "";
            if (whereSql.Text != "")
            {
                placed = " AND ";
            }

            if (sourceContent.SelectedItems.Count == 1)
            {
                int columNr = findViewColumByFieldname(sourceContent, sel);
                string content = sourceContent.SelectedItems[0].SubItems[columNr].Text.Replace("'", "\'");
                if (content != null)
                {
                    sel += " = '" + content + "'";
                }
            }

            whereSql.Text += placed + sel;

        }

        private void targetSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            enableControlls();
            string sel = targetSelect.SelectedItem.ToString();
            targetProfil = new Profil(sel);
            targetProfil.loadProfileSettings();

            addMacroCommand("selectTargetTable:" + sel + ";");

            // connect
            database_target = new MysqlHandler(targetProfil);
            database_target.resetTableList();
            checkTableExistsOnTarget();
        }

        private void compareBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //compareViews();
            addMacroCommand("SetComparePrimary:" + compareBy.Text + ";");
            if (optionAutoProp.Checked) saveTablePropertiesToolStripMenuItem_Click(sender, e);
        }

        private void copyValues_Click(object sender, EventArgs e)
        {
            int leftCompare = 0;
            int rightCompare = 0;
            if (compareBy.Text != "")
            {
                leftCompare = findViewColumByFieldname(sourceContent, compareBy.Text);
                rightCompare = findViewColumByFieldname(target_view, compareBy.Text);


                for (int i = 0; i < sourceContent.Items.Count; i++)
                {
                    //sourceContent.Items[i].SubItems[leftCompare].ForeColor = Color.Blue;

                    string compareStr = sourceContent.Items[i].SubItems[leftCompare].Text;



                    for (int p = 0; p < target_view.Items.Count; p++)
                    {
                        if (compareStr == target_view.Items[p].SubItems[rightCompare].Text)
                        {
                            bool isEqual = true;

                            // mark as not compared
                            target_view.Items[p].ForeColor = Color.Black;

                            for (int t = 0; t < sourceContent.Items[i].SubItems.Count; t++)
                            {

                                if (target_view.Items[p].SubItems.Count > t && t != leftCompare)
                                {
                                    if (sourceContent.Items[i].SubItems[t].Text != target_view.Items[p].SubItems[t].Text)
                                    {
                                        target_view.Items[p].SubItems[t].Text = sourceContent.Items[i].SubItems[t].Text;
                                        isEqual = false;
                                    }
                                }
                            }

                            if (isEqual) target_view.Items[p].ForeColor = Color.Blue;
                            else target_view.Items[p].ForeColor = Color.Green;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < sourceContent.Items.Count; i++)
                {
                    for (int t = 0; t < sourceContent.Items[i].SubItems.Count; t++)
                    {
                        if (target_view.Items.Count > i && target_view.Items[i].SubItems.Count > t)
                            target_view.Items[i].SubItems[t].Text = sourceContent.Items[i].SubItems[t].Text;
                    }
                }
            }
            sourceContent.Refresh();
        }

        private void UpdateValues_Click(object sender, EventArgs e)
        {

            int leftCompare = 0;
            int rightCompare = 0;
            if (compareBy.Text != "")
            {
                leftCompare = findViewColumByFieldname(sourceContent, compareBy.Text);
                rightCompare = findViewColumByFieldname(target_view, compareBy.Text);

                if (database_target != null)
                {
                    database_target.connect();
                    List<string> sqlStatements = new List<string>();
                    for (int p = 0; p < target_view.Items.Count; p++)
                    {
                        string sql = "UPDATE " + selectRightTable + " SET ";
                        string where = " WHERE " + findNameInColum(target_view, rightCompare) + " = '" + target_view.Items[p].SubItems[rightCompare].Text + "'";
                        string Sets = "";
                        string Add = "";
                        for (int n = 0; n < target_view.Items[p].SubItems.Count; n++)
                        {
                            if (n != rightCompare)
                            {
                                Sets += Add + findNameInColum(target_view, n) + " = '" + mapSqlFields(target_view.Items[p].SubItems[n].Text) + "'";
                                Add = ", ";
                            }
                        }

                        sqlStatements.Add(sql + Sets + where);
                        database_target.sql_update(sql + Sets + where);
                    }
                    database_target.disConnect();
                }
            }
            else
            {
                MessageBox.Show(this, "i need a Field to find Rows for Update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sourcetabs_ItemActivate(sender, e);
        }

        private void deleteTargetContent()
        {
            int leftCompare = 0;
            int rightCompare = 0;
            if (compareBy.Text != "")
            {
                leftCompare = findViewColumByFieldname(sourceContent, compareBy.Text);
                rightCompare = findViewColumByFieldname(target_view, compareBy.Text);
            }

            database_target.connect();
            List<string> sqlStatements = new List<string>();
            for (int p = 0; p < target_view.Items.Count; p++)
            {
                string sql = "DELETE FROM " + selectRightTable + " ";
                string where = " WHERE " + findNameInColum(target_view, rightCompare) + " = '" + target_view.Items[p].SubItems[rightCompare].Text + "'";
                string Sets = " AND ";
                string Add = "";
                for (int n = 0; n < target_view.Items[p].SubItems.Count; n++)
                {
                    if (n != rightCompare && target_view.Items[p].SubItems[n].Text != "null")
                    {
                        Sets += Add + findNameInColum(target_view, n) + " = '" + mapSqlFields(target_view.Items[p].SubItems[n].Text) + "'";
                        Add = " AND ";
                    }
                }

                sqlStatements.Add(sql + where + Sets);
                database_target.sql_update(sql + where + Sets);
            }
            database_target.disConnect();
        }

        private void copyToRight()
        {
            int leftCompare = 0;
            int rightCompare = 0;
            if (compareBy.Text != "")
            {
                leftCompare = findViewColumByFieldname(sourceContent, compareBy.Text);
                rightCompare = findViewColumByFieldname(target_view, compareBy.Text);
            }

            database_target.connect();
            List<string> sqlStatements = new List<string>();
            for (int p = 0; p < sourceContent.Items.Count; p++)
            {
                string sql = "INSERT INTO " + selectRightTable + " ";
                //string where = " WHERE " + findNameInColum(sourceContent, leftCompare) + " = '" + sourceContent.Items[p].SubItems[leftCompare].Text + "'";
                string Sets = " SET ";
                string Add = "";
                for (int n = 0; n < sourceContent.Items[p].SubItems.Count; n++)
                {
                    //if (n != leftCompare)
                    //{
                    string FieldInsert = mapSqlFields(sourceContent.Items[p].SubItems[n].Text);
                    Sets += Add + findNameInColum(sourceContent, n) + " = '" + FieldInsert + "'";
                    Add = ", ";
                    //}
                }

                sqlStatements.Add(sql + Sets);
                database_target.sql_update(sql + Sets);
            }
            database_target.disConnect();

        }

        private string mapSqlFields(string fieldValue)
        {
            string FieldInsert = fieldValue.Replace("'", @"\'");
            FieldInsert = FieldInsert.Replace("True", "1");
            FieldInsert = FieldInsert.Replace("False", "0");
            FieldInsert = FieldInsert.Replace(",", ".");
            return FieldInsert;
        }

        private void SyncBtn_Click(object sender, EventArgs e)
        {
            deleteTargetContent();
            copyToRight();
            sourcetabs_ItemActivate(sender, e);
        }

        private void DeleteRows_Click(object sender, EventArgs e)
        {
            deleteTargetContent();

            sourcetabs_ItemActivate(sender, e);
        }

        private void showCreationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTable != "")
            {
                string diffCommand = "";
                string diffParam = "";
                if (sourceProfil.getProperty("diff_command") != null || targetProfil.getProperty("diff_command") != null)
                {

                    diffCommand = sourceProfil.getProperty("diff_command");
                    if (diffCommand == null || diffCommand == "") diffCommand = targetProfil.getProperty("diff_command");

                    diffParam = sourceProfil.getProperty("diff_param");
                    if (diffParam == null || diffParam == "") diffParam = targetProfil.getProperty("diff_param");




                }

                ShowDiff sd = new ShowDiff();
                database_source.connect();
                string createStr = database_source.getTableCreationString(selectedTable);


                database_source.disConnect();
                string createStr2 = "Not selected";
                if (database_target != null)
                {
                    database_target.connect();
                    createStr2 = database_target.getTableCreationString(selectRightTable);
                }

                /*
                StructDiffWindow strWin = new StructDiffWindow();
                strWin.source = database_source.getAllFieldsStruct(selectedTable);
                strWin.target = database_target.getAllFieldsStruct(selectRightTable);
                

                strWin.ShowDialog();
                */
                if (createStr != "")
                {
                    //if (createStr!=createStr2)  MessageBox.Show("is not equal:\n\n" + createStr + "\n \n" + createStr2);
                    //else MessageBox.Show("is equal:\n\n" + createStr + "\n \n" + createStr2);

                    if (diffCommand != null && diffCommand != "")
                    {

                        String temp1 = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + System.IO.Path.DirectorySeparatorChar + "leftdiff.sql";
                        String temp2 = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + System.IO.Path.DirectorySeparatorChar + "rightdiff.sql";

                        System.IO.File.WriteAllText(temp1, createStr);
                        System.IO.File.WriteAllText(temp2, createStr2);

                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.FileName = diffCommand;

                        if (diffParam != null && diffParam != "")
                        {
                            diffParam = diffParam.Replace("[LEFT]", temp1);
                            diffParam = diffParam.Replace("[RIGHT]", temp2);
                            p.StartInfo.Arguments = diffParam;
                        }
                        p.Start();
                    }
                    else
                    {


                        sd.leftText.Text = createStr;
                        sd.rightText.Text = createStr2;
                        sd.Show();
                        sd.compareText();
                    }
                }
                if (database_target != null) database_target.disConnect();

            }
        }

        private void createStructureInTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (database_source != null && database_target != null)
            {

                if (sourcetabs.SelectedItems.Count > 0)
                {

                    for (int i = 0; i < sourcetabs.SelectedItems.Count; i++)
                    {
                        MysqlTools tool = new MysqlTools();
                        tool.copyCreationTable(database_source, sourcetabs.SelectedItems[i].Text, database_target);

                    }
                    checkTableExistsOnTarget();
                }

                /*
                if (selectedTable != "")
                {
                    MysqlTools tool = new MysqlTools();
                    tool.copyCreationTable(database_source, selectedTable, database_target);
                    checkTableExistsOnTarget();
                }*/
            }

        }

        private void compareStructToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //checkTableStructs();

            if (!checkSync.IsBusy)
            {

                runningTask.Maximum = sourcetabs.Items.Count;
                runningTask.Visible = true;
                runningTask.Value = 0;
                sourcetabs.Items.Clear();
                DataContent dc = new DataContent();
                dc.db1 = database_source;
                dc.db2 = database_target;

                checkSync.RunWorkerAsync(dc);
            }
        }

        private void fullSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {

            copyTableToTargetToolStripMenuItem_Click(sender, e);
            /*
            if (sourcetabs.SelectedItems.Count > 0)
            {
                if (!fullSyncWorker.IsBusy)
                {
                    fullSynBTN.Enabled = false;
                    List<string> datas = new List<string>();
                    for (int i = 0; i < sourcetabs.SelectedItems.Count; i++)
                    {


                        string synctable = sourcetabs.SelectedItems[i].Text;
                        datas.Add(synctable);


                    }
                    //checkTableExistsOnTarget();
                    //fullSynBTN.Enabled = true;

                    runningTask.Maximum = sourcetabs.SelectedItems.Count;
                    runningTask.Visible = true;
                    runningTask.Value = 0;
                    DataContent dc = new DataContent();
                    dc.db1 = database_source;
                    dc.db2 = database_target;
                    dc.Source = datas;
                    fullSyncWorker.RunWorkerAsync(dc);
                }
            }

            */

            /*
            if (sourcetabs.SelectedItems.Count > 0)
            {
                fullSynBTN.Enabled = false;
                for (int i = 0; i < sourcetabs.SelectedItems.Count; i++)
                {
                    string synctable = sourcetabs.SelectedItems[i].Text;
                    database_target.connect();
                    database_source.connect();
                    MysqlTools Mtools = new MysqlTools();
                    Mtools.copyTable(database_source, synctable, database_target);
                    database_target.disConnect();
                    database_source.disConnect();
                    //sourcetabs_ItemActivate(sender, e);
                }
                checkTableExistsOnTarget();
                fullSynBTN.Enabled = true;
            }
             */
        }

        private void checkSync_DoWork(object sender, DoWorkEventArgs e)
        {
            DataContent DC = (DataContent)e.Argument;
            String Result = "Compare Finished" + Environment.NewLine;
            String MissingTables = Environment.NewLine + " ------- Missing Tables ---------" + Environment.NewLine;
            if (DC.db2 != null && DC.db1 != null)
            {
                DC.db1.connect();
                DC.db2.connect();

                ListView dummy = new ListView();

                MySql.Data.MySqlClient.MySqlDataReader result = database_source.sql_select("show tables");
                database_source.sql_data2ListView(result, dummy);

                int notMatch = 0;
                int notExists = 0;
                int match = 0;

                Result += Environment.NewLine;

                Result += "Mysql Server Version from Source:" + DC.db1.getMysqlVersion() + Environment.NewLine;
                Result += "Mysql Server Version from Target:" + DC.db2.getMysqlVersion() + Environment.NewLine;


                if (modifyTableAddOnly)
                {
                    Result += "WARNING!!! the 'check modify' Option ist disabled so you can see in this report missing fields only." + Environment.NewLine;
                }
                else
                {
                    if (DC.db1.getMysqlVersion() != DC.db2.getMysqlVersion())
                    {
                        Result += "#####################################################################################################" + Environment.NewLine;
                        Result += "WARNING!!!! Structure check between different MySQL Versions can result in a incorret Report." + Environment.NewLine;
                        Result += "      Maybe you disable the MODIFY Check. in this case it will be checked on missing fields only" + Environment.NewLine;
                        Result += "####################################################################################################" + Environment.NewLine;
                    }
                }



                for (int i = 0; i < dummy.Items.Count; i++)
                {
                    string tableName = dummy.Items[i].Text;
                    string hint = "";
                    if (DC.db2.tableExists(tableName))
                    {
                        dummy.ForeColor = Color.Green;

                        string crSource = DC.db1.getTableCreationString(tableName);
                        string crTarget = DC.db2.getTableCreationString(tableName);

                        if (crSource == crTarget)
                        {
                            dummy.Items[i].BackColor = Color.Transparent;
                            dummy.Items[i].ImageIndex = 1;
                            checkSync.ReportProgress(i, dummy.Items[i]);

                            match++;

                        }
                        else
                        {
                            List<MysqlStruct> astr = new List<MysqlStruct>();
                            List<MysqlStruct> bstr = new List<MysqlStruct>();

                            astr = DC.db1.getAllFieldsStruct(tableName);
                            bstr = DC.db2.getAllFieldsStruct(tableName);

                            dummy.Items[i].BackColor = Color.LightBlue;
                            dummy.Items[i].ImageIndex = 2;

                            bool structEquals = true;
                            bool keyEquals = true;

                            if (astr.Count != bstr.Count)
                            {


                                dummy.Items[i].BackColor = Color.LightPink;
                                dummy.Items[i].ForeColor = Color.SaddleBrown;
                                dummy.Items[i].ImageIndex = 4;
                            }

                            for (int pn = 0; pn < astr.Count; pn++)
                            {

                                string fieldName = astr[pn].name;
                                int found = -1;
                                for (int pp = 0; pp < bstr.Count; pp++)
                                {
                                    if (fieldName == bstr[pp].name)
                                    {
                                        found = pp;
                                    }
                                }

                                if (found >= 0)
                                {
                                    if (!modifyTableAddOnly)
                                    {
                                        keyEquals = (astr[pn].Key != bstr[found].Key);
                                        if (structEquals)
                                        {
                                            structEquals = !(astr[pn].param != bstr[found].param || astr[pn].realType != bstr[found].realType || astr[pn].Null != bstr[found].Null || astr[pn].type != bstr[found].type);
                                            if (!structEquals) Result += System.Environment.NewLine + System.Environment.NewLine + "================" + tableName + " ==========================" + System.Environment.NewLine;
                                        }

                                        if (astr[pn].param != bstr[found].param) hint += System.Environment.NewLine + tableName + " field[" + fieldName + "] param not equals (" + astr[pn].param + ") <-> (" + bstr[found].param + ")";
                                        if (astr[pn].realType != bstr[found].realType) hint += System.Environment.NewLine + tableName + ":: field[" + fieldName + "] RealType not equals (" + astr[pn].realType + ") <-> (" + bstr[found].realType + ")";
                                        if (astr[pn].type != bstr[found].type) hint += System.Environment.NewLine + tableName + ":: field[" + fieldName + "] Type setting not equals (" + astr[pn].type + ") <-> (" + bstr[found].type + ")";
                                        if (astr[pn].Null != bstr[found].Null) hint += System.Environment.NewLine + tableName + ":: field [" + fieldName + "] NOT NULL different setting (" + astr[pn].Null + ") <-> (" + bstr[found].Null + ")";
                                    }
                                }
                                else
                                {
                                    hint += System.Environment.NewLine + tableName + " :: field " + fieldName + " not exists!!! ";
                                }

                            }


                            if (!structEquals)
                            {
                                dummy.Items[i].BackColor = Color.LightPink;
                                dummy.Items[i].ImageIndex = 4;
                            }

                            checkSync.ReportProgress(i, dummy.Items[i]);
                            notMatch++;
                        }

                    }
                    else
                    {
                        dummy.Items[i].ForeColor = Color.Red;
                        dummy.Items[i].ImageIndex = 3;
                        notExists++;
                        MissingTables += " " + tableName + Environment.NewLine;
                        checkSync.ReportProgress(i, dummy.Items[i]);
                    }
                    Result += hint;
                    dummy.Items[i].ToolTipText = hint;
                }
                DC.db1.disConnect();
                DC.db2.disConnect();
                Result += System.Environment.NewLine + "------------------------" + System.Environment.NewLine + "Matched: " + match + System.Environment.NewLine;
                Result += "Not matched: " + notMatch + System.Environment.NewLine;
                Result += "Not exists: " + notExists + System.Environment.NewLine;
            }
            e.Result = Result + MissingTables;
        }

        private void checkSync_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int pos = e.ProgressPercentage;
            ListViewItem item = (ListViewItem)e.UserState;
            ListViewItem addItem = new ListViewItem();
            /*
            sourcetabs.Items[pos].BackColor = item.BackColor;
            sourcetabs.Items[pos].ForeColor = item.ForeColor;
            sourcetabs.Items[pos].ImageIndex = item.ImageIndex;
             */
            addItem.BackColor = item.BackColor;
            addItem.ForeColor = item.ForeColor;
            addItem.ImageIndex = item.ImageIndex;
            addItem.ToolTipText = item.ToolTipText;
            addItem.Text = item.Text;
            if (runningTask.Maximum < pos) runningTask.Maximum = pos + 1;
            runningTask.Value = pos;

            sourcetabs.Items.Add(addItem);
        }

        private void checkSync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            runningTask.Visible = false;
            String res = (String)e.Result;
            //MessageBox.Show(this,res,"Compare Struct Result",MessageBoxButtons.OK,MessageBoxIcon.Information);
            DiffResultWindow dWind = new DiffResultWindow();
            dWind.textBox.Lines = res.Split('\n');
            dWind.ShowDialog();
        }

        private void fullSyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DataContent DC = (DataContent)e.Argument;
            for (int i = 0; i < DC.Source.Count; i++)
            {
                string synctable = DC.Source[i];
                DC.db1.connect();
                DC.db2.connect();
                MysqlTools Mtools = new MysqlTools();
                Mtools.copyTable(DC.db1, synctable, DC.db2);
                fullSyncWorker.ReportProgress(i);
                DC.db1.disConnect();
                DC.db2.disConnect();
            }
        }

        private void fullSyncWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int pos = e.ProgressPercentage;
            if (runningTask.Maximum < pos) runningTask.Maximum = pos + 1;
            runningTask.Value = pos;

        }

        private void fullSyncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            runningTask.Visible = false;

            MessageBox.Show(this, "Sunc Done", "Table Sync", MessageBoxButtons.OK, MessageBoxIcon.Information);
            checkTableExistsOnTarget();
            selectSourceTable();
            fullSynBTN.Enabled = true;
        }

        private void saveDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveDump.ShowDialog() == DialogResult.OK)
            {
                if (!DumpSql.IsBusy) DumpSql.RunWorkerAsync(saveDump.FileName);
                else MessageBox.Show(this, "Dump Running", "a Dump is already Running ..", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveDump_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void DumpSql_DoWork(object sender, DoWorkEventArgs e)
        {

            string fileName = (string)e.Argument;
            if (database_source != null && selectedTable != "")
            {
                MysqlTools Mtools = new MysqlTools();
                string sqlDump = Mtools.getMysqlDump(database_source, selectedTable);
                //MessageBox.Show(sqlDump);
                System.IO.File.WriteAllText(fileName, sqlDump);
            }

        }

        private void dropTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Drop table(s)? ...are you really sure ?", "Drop table", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                List<string> datas = new List<string>();
                for (int i = 0; i < sourcetabs.SelectedItems.Count; i++)
                {
                    string synctable = sourcetabs.SelectedItems[i].Text;
                    datas.Add(synctable);

                }

                if (!dropWorker.IsBusy)
                {
                    DataContent DC = new DataContent();
                    DC.db1 = database_target;
                    DC.Source = datas;
                    dropWorker.RunWorkerAsync(DC);
                }

            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DataContent DC = new DataContent();
            DC = (DataContent)e.Argument;
            MysqlTools Mtools = new MysqlTools();
            for (int i = 0; i < DC.Source.Count; i++)
            {
                Mtools.dropTable(DC.db1, DC.Source[i], false);
            }
        }

        private void dropTableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Drop table(s)? ...are you really sure ?", "Drop table", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                List<string> datas = new List<string>();
                for (int i = 0; i < sourcetabs.SelectedItems.Count; i++)
                {
                    string synctable = sourcetabs.SelectedItems[i].Text;
                    datas.Add(synctable);

                }

                if (!dropWorker.IsBusy)
                {
                    DataContent DC = new DataContent();
                    DC.db1 = database_source;
                    DC.Source = datas;
                    dropWorker.RunWorkerAsync(DC);
                }

            }
        }

        private void selectSourceTable()
        {
            if (selectedTable != "")
            {
                selectCurrentTable();
                sourcetabs_ItemActivate(null, null);
            }
        }

        private void dropWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //sourceSelect_SelectedValueChanged(sender, e);
            database_target.resetTableList();
            checkTableExistsOnTarget();
            selectSourceTable();

        }

        private void copyTableToNewTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTable != "")
            {
                UserTextInput uti = new UserTextInput();
                uti.groupBox.Text = "Insert new Tablename";
                uti.textinfo.Text = selectedTable + "_copy";

                if (uti.ShowDialog() == DialogResult.OK)
                {
                    if (uti.textinfo.Text.Length > 2)
                    {
                        MysqlTools Mtools = new MysqlTools();
                        Mtools.copyTable(database_source, selectedTable, uti.textinfo.Text);
                        database_source.resetTableList();
                        checkTableExistsOnTarget();


                    }
                    else
                    {
                        MessageBox.Show("To short Tablename");
                    }
                }
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewWorker lw = new ListViewWorker();
            lw.copyListView(sourceContent, sourceMemList);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteListView plw = new PasteListView();
            plw.sourceField.Items.Clear();
            plw.targetField.Items.Clear();
            for (int i = 0; i < sourceContent.Columns.Count; i++)
            {
                plw.targetField.Items.Add(sourceContent.Columns[i].Text);
            }

            for (int i = 0; i < sourceMemList.Columns.Count; i++)
            {
                plw.sourceField.Items.Add(sourceMemList.Columns[i].Text);
            }
            if (plw.ShowDialog() == DialogResult.OK)
            {
                string sourceField = plw.sourceField.Text;
                string targetField = plw.targetField.Text;

                if (sourceField != "" && targetField != "")
                {
                    int leftPos = 0;
                    int rightPos = 0;

                    for (int i = 0; i < sourceContent.Columns.Count; i++)
                    {
                        if (sourceContent.Columns[i].Text == targetField) leftPos = i;
                    }

                    for (int i = 0; i < sourceMemList.Columns.Count; i++)
                    {
                        if (sourceMemList.Columns[i].Text == sourceField) rightPos = i;
                    }

                    for (int i = 0; i < sourceContent.Items.Count; i++)
                    {
                        if (i < sourceMemList.Items.Count)
                        {
                            sourceContent.Items[i].SubItems[leftPos].Text = sourceMemList.Items[i].SubItems[rightPos].Text;
                        }
                    }


                }


            }
        }

        private int getIgnoredFieldIndex(String name)
        {
            for (int i = 0; i < ignoreOnMatchCheck.Count; i++)
            {
                if (ignoreOnMatchCheck[i] == name) return i;
            }
            return -1;
        }


        private void updateIgnoresInColum()
        {
            for (int fieldNr = 0; fieldNr < sourceContent.Columns.Count; fieldNr++)
            {
                sourceContent.Columns[fieldNr].Width = 50 + sourceContent.Columns[fieldNr].Text.Length * 15;
                if (getIgnoredFieldIndex(sourceContent.Columns[fieldNr].Text) == -1)
                {
                    sourceContent.Columns[fieldNr].ImageIndex = 1;
                }
                else
                {
                    sourceContent.Columns[fieldNr].ImageIndex = 2;
                }
            }
        }

        private void sourceContent_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int columNr = e.Column;

            int stateInt = sourceContent.Columns[columNr].ImageIndex;
            string searchColum = sourceContent.Columns[columNr].Text;
            if (stateInt != 2)
            {

                if (getIgnoredFieldIndex(searchColum) == -1) ignoreOnMatchCheck.Add(searchColum);
                sourceContent.Columns[columNr].ImageIndex = 2;
                addMacroCommand("ignoreField:" + sourceContent.Columns[columNr].Text + ";");
            }
            else
            {
                int fIndex = getIgnoredFieldIndex(searchColum);
                if (fIndex != -1) ignoreOnMatchCheck.RemoveAt(fIndex);
                sourceContent.Columns[columNr].ImageIndex = 1;
                addMacroCommand("includeField:" + sourceContent.Columns[columNr].Text + ";");
            }
            sourceContent.Columns[columNr].Width = 50 + sourceContent.Columns[columNr].Text.Length * 15;

            //sourceContent.Columns[columNr].ImageKey = 1;

            //sourceContent.Columns[columNr]

        }

        private void setIgnoreField(string searchColum, bool set)
        {
            int columNr = findViewColumByFieldname(sourceContent, searchColum);
            if (columNr > -1)
            {
                if (set)
                {

                    if (getIgnoredFieldIndex(searchColum) == -1) ignoreOnMatchCheck.Add(searchColum);


                }
                else
                {
                    int fIndex = getIgnoredFieldIndex(searchColum);
                    if (fIndex != -1) ignoreOnMatchCheck.RemoveAt(fIndex);


                }
            }
            updateIgnoresInColum();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            addMacroCommand("compareRows;");
            if (alterTableIsSelected.Checked) checkAlterTable();
            spexec = new ScriptExecuter();
            spexec.usedProfile = targetProfil;
            spexec.queryList.Items.Clear();
            if (DO_autoloop.Checked)
            {
                while (compareByRow())
                {
                    button4_Click(sender, e);

                }
            }
            else compareByRow();
            if (spexec.queryList.Items.Count > 0)
            {
                if (writeResultInExportToolStripMenuItem.Checked)
                {
                    exportRowCompareResultInFile(spexec.createSqlExport());
                }
                else
                {
                    spexec.ShowDialog();
                }

            }
        }

        private bool compareByRow()
        {
            int refreshTick = 0;
            List<string> finalInsertSQL = new List<string>();
            List<string> finalUpdateSQL = new List<string>();

            string addPrev = "";
            if (tablenamePrevToField) addPrev = selectRightTable + ".";


            int leftCompare = 0;
            int rightCompare = 0;
            if (compareBy.Text != "")
            {
                leftCompare = findViewColumByFieldname(sourceContent, compareBy.Text);
                rightCompare = findViewColumByFieldname(target_view, compareBy.Text);
            }
            else
            {

                // trying to set by primary and unique indizies
                MysqlKeys MKeys = new MysqlKeys();
                MKeys.init(lastSeltable, database_source);

                List<MysqlKeySet> keys = MKeys.getUniqueKeys();
                if (keys.Count > 0)
                {
                    for (int op = 0; op < keys.Count; op++)
                    {
                        if (op == 0)
                        {
                            leftCompare = findViewColumByFieldname(sourceContent, keys[op].columnName);
                            rightCompare = findViewColumByFieldname(target_view, keys[op].columnName);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Comparefield selected ");
                    return false;
                }
            }
            updateIgnoresInColum();
            prozessMsg msg = new prozessMsg();
            msg.Show();

            rowsRead.Text = "Rows: " + sourceContent.Items.Count + " / " + limitEnd.Value;
            msg.progressBar.Maximum = sourceContent.Items.Count;

            string whereAdds = "";




            if (DIFF_clear.Checked) target_view.Items.Clear();
            Application.DoEvents();
            for (int i = 0; i < sourceContent.Items.Count; i++)
            {


                //sourceContent.Items[i].SubItems[leftCompare].ForeColor = Color.Blue;
                whereAdds = "";
                String update_SQL = "UPDATE " + selectRightTable + " SET ";
                string compareStr = sourceContent.Items[i].SubItems[leftCompare].Text;


                if (idLimiterList == null || idLimiterList.Count == 0 || idLimiterList.Contains(compareStr))
                {
                    string checkOnTargetSql = "";
                    if (compareStr == "null")
                    {
                        checkOnTargetSql = "SELECT * FROM " + selectRightTable + " WHERE (`" + compareBy.Text + "` <=> NULL OR `" + compareBy.Text + "` = '') ";
                        whereAdds = " WHERE (`" + compareBy.Text + "` <=> NULL OR `" + compareBy.Text + "` = '') ";
                    }
                    else
                    {
                        checkOnTargetSql = "SELECT * FROM " + selectRightTable + " WHERE `" + compareBy.Text + "` = '" + compareStr + "' ";
                        whereAdds = " WHERE `" + compareBy.Text + "` = '" + compareStr + "' ";
                    }
                    
                    for (int z = 0; z < whereList.CheckedItems.Count; z++)
                    {
                        String comStr = whereList.CheckedItems[z].ToString();
                        int leftCompareSub = findViewColumByFieldname(sourceContent, comStr);
                        String compval = sourceContent.Items[i].SubItems[leftCompareSub].Text;

                        if (compval == "null")
                        {
                            checkOnTargetSql += " AND (`" + comStr + "` <=> NULL OR `"+ comStr +"` = '') ";
                            whereAdds += " AND (`" + comStr + "` <=> NULL  OR `"+ comStr +"` = '') ";
                        }
                        else
                        {
                            checkOnTargetSql += " AND `" + comStr + "` = '" + compval + "'";
                            whereAdds += " AND `" + comStr + "`= '" + compval + "'";
                        }

                        
                    }

                    if (!database_target.isConnected())
                    {
                        database_target.connect();
                    }

                    MySql.Data.MySqlClient.MySqlDataReader reader = database_target.sql_select(checkOnTargetSql);

                    string chkdata = "";
                    string fieldname = "";
                    bool isEqual = true;


                    if (reader == null || !reader.HasRows)
                    {
                        String insertSQL = "INSERT INTO " + selectRightTable + " SET ";
                        String add = "";
                        ListViewItem insertListViewItem = new ListViewItem();
                        insertListViewItem.BackColor = Color.Red;
                        for (int p = 0; p < sourceContent.Columns.Count; p++)
                        {
                            String insertFieldname = "`" + sourceContent.Columns[p].Text + "`";
                            String insertValue = sourceContent.Items[i].SubItems[p].Text;
                            if (getIgnoredFieldIndex(insertFieldname) == -1 && insertValue != "null")
                            {
                                insertSQL += add + insertFieldname + " = '" + insertValue + "'";
                                add = ",";
                                insertListViewItem.SubItems.Add( insertValue + " Missing");
                            }

                        }
                        finalInsertSQL.Add(insertSQL);
                        
                        insertListViewItem.ToolTipText = insertSQL;
                        insertListViewItem.Text = "MISSING ROW";
                        target_view.Items.Add(insertListViewItem);
                    }

                    if (reader != null)
                    {
                        ListViewItem tmpListViewItem = new ListViewItem();
                        tmpListViewItem.UseItemStyleForSubItems = false;

                        while (reader.Read())
                        {




                            isEqual = true;
                            string where_add = "";

                            for (int fieldNr = 0; fieldNr < sourceContent.Columns.Count; fieldNr++)
                            {
                                fieldname = sourceContent.Columns[fieldNr].Text;

                                if (getIgnoredFieldIndex(fieldname) == -1)
                                {
                                    int dataOntarget = findViewColumByFieldname(target_view, fieldname);

                                    chkdata = database_target.getMysqlValue(reader, dataOntarget);//  getMysqlValue
                                    // compare data

                                    String compLeft = sourceContent.Items[i].SubItems[fieldNr].Text;
                                    if (compLeft != chkdata)
                                    {
                                        isEqual = false;

                                        // new ListViewItem.ListViewSubItem( lvi,"subitem", Color.White, Color.Blue, lvi.Font )
                                        string fieldDiffTxt = "DIFF: " + compLeft + " != " + chkdata;

                                        if (fieldNr > 0) tmpListViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(tmpListViewItem, fieldDiffTxt, Color.White, Color.Blue, tmpListViewItem.Font));
                                        else tmpListViewItem.Text = fieldDiffTxt;


                                        if (compLeft == "null") update_SQL += where_add + addPrev + fieldname + " = NULL";
                                        else update_SQL += where_add + addPrev + fieldname + " = '" + compLeft + "'";
                                        where_add = ",";
                                    }
                                    else
                                    {
                                        if (fieldNr > 0) tmpListViewItem.SubItems.Add(chkdata);
                                        else tmpListViewItem.Text = chkdata;
                                    }



                                }

                            }

                            if (isEqual)
                            {
                                if (!DIFF_only.Checked)
                                {
                                    tmpListViewItem.BackColor = Color.Aqua;
                                    target_view.Items.Add(tmpListViewItem);
                                }
                            }
                            else
                            {
                                update_SQL += whereAdds;
                                finalUpdateSQL.Add(update_SQL);

                                tmpListViewItem.BackColor = Color.Fuchsia;
                                try
                                {
                                    tmpListViewItem.ToolTipText = update_SQL;
                                    target_view.Items.Add(tmpListViewItem);
                                }
                                catch (Exception)
                                {
                                    target_view.Items.Add("error adding:" + update_SQL);
                                    //throw;
                                }
                                sourceContent.Items[i].BackColor = Color.ForestGreen;

                            }


                        }
                        reader.Close();
                    }
                    refreshTick++;
                    if (refreshTick > 100)
                    {
                        Application.DoEvents();
                        refreshTick = 0;
                        if (i < msg.progressBar.Maximum) msg.progressBar.Value = i;
                        msg.progressBar.Refresh();
                        msg.msg.Text = "compare: " + i + " / " + msg.progressBar.Maximum;
                    }

                }
            }

            if (DO_INSERTS.Checked)
            {
                for (int p = 0; p < finalInsertSQL.Count; p++)
                {
                    database_target.sql_update(finalInsertSQL[p]);
                    msg.progressBar.Maximum = finalInsertSQL.Count;
                    refreshTick++;
                    if (refreshTick > 20)
                    {
                        if (p < msg.progressBar.Maximum) msg.progressBar.Value = p;
                        Application.DoEvents();
                        refreshTick = 0;

                        msg.progressBar.Refresh();
                        msg.msg.Text = "writing inserts: " + p + " / " + msg.progressBar.Maximum;

                    }

                }

                for (int p = 0; p < finalUpdateSQL.Count; p++)
                {
                    database_target.sql_update(finalUpdateSQL[p]);
                    msg.progressBar.Maximum = finalUpdateSQL.Count;
                    refreshTick++;
                    if (refreshTick > 20)
                    {
                        if (p < msg.progressBar.Maximum) msg.progressBar.Value = p;
                        Application.DoEvents();
                        refreshTick = 0;

                        msg.progressBar.Refresh();
                        msg.msg.Text = "writing updates: " + p + " / " + msg.progressBar.Maximum;

                    }

                }
            }





            database_target.disConnect();
            database_source.disConnect();

            updateIgnoresInColum();

            Application.DoEvents();
            if (!DO_INSERTS.Checked)
            {


                msg.progressBar.Maximum = finalInsertSQL.Count;
                for (int p = 0; p < finalInsertSQL.Count; p++)
                {
                    ListViewItem tmpAdd = new ListViewItem();
                    tmpAdd.Text = "INSERT";
                    tmpAdd.SubItems.Add(finalInsertSQL[p]);
                    spexec.queryList.Items.Add(tmpAdd);

                    refreshTick++;
                    if (refreshTick > 20)
                    {
                        if (p < msg.progressBar.Maximum) msg.progressBar.Value = p;
                        Application.DoEvents();
                        refreshTick = 0;

                        msg.progressBar.Refresh();
                        msg.msg.Text = "Refresh Script List: " + p + " / " + msg.progressBar.Maximum;

                    }

                }

                msg.progressBar.Maximum = finalUpdateSQL.Count;
                for (int p = 0; p < finalUpdateSQL.Count; p++)
                {
                    ListViewItem tmpAdd = new ListViewItem();
                    tmpAdd.Text = "UPDATE";
                    tmpAdd.SubItems.Add(finalUpdateSQL[p]);
                    spexec.queryList.Items.Add(tmpAdd);

                    refreshTick++;
                    if (refreshTick > 20)
                    {
                        if (p < msg.progressBar.Maximum) msg.progressBar.Value = p;
                        Application.DoEvents();
                        refreshTick = 0;

                        msg.progressBar.Refresh();
                        msg.msg.Text = "Refresh Script List: " + p + " / " + msg.progressBar.Maximum;

                    }

                }

            }
            msg.Close();



            return (sourceContent.Items.Count == limitEnd.Value);
        }

        private void Next_Click(object sender, EventArgs e)
        {
            addMacroCommand("gotoNext;");
            String compSet = compareBy.Text;
            limitStart.Value += limitEnd.Value;
            sourcetabs_ItemActivate(sender, e);
            compareBy.Text = compSet;
        }

        private void switchOrientationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (splitContainer2.Orientation == Orientation.Horizontal) splitContainer2.Orientation = Orientation.Vertical;
            else splitContainer2.Orientation = Orientation.Horizontal;

            /*
            for (int i = 0; i < sourcetabs.Columns.Count; i++)
            {
                if (i < target_view.Columns.Count) target_view.Columns[i].Width = sourcetabs.Columns[i].Width;
            }*/

        }

        private void button4_Click(object sender, EventArgs e)
        {
            addMacroCommand("gotoNextAndCompare;");
            Next_Click(sender, e);
            compareByRow();
        }

        private void DO_useLimiter_Click(object sender, EventArgs e)
        {
            UserTextInput ti = new UserTextInput();
            ti.groupBox.Text = "QUERY width ONE field as check";
            ti.textinfo.Text = idLimiterSQL;
            if (ti.ShowDialog() == DialogResult.OK)
            {
                idLimiterList.Clear();
                if (!database_source.isConnected()) database_source.connect();
                idLimiterList = database_source.selectFirstAsList(ti.textinfo.Text);
                database_source.disConnect();
            }
        }

        private void findCompare(ListViewItem item)
        {
            if (compareBy.Text != "")
            {
                int leftCompare = 0;
                int rightCompare = 0;
                int foundCompares = 0;
                if (compareBy.Text != "")
                {
                    leftCompare = findViewColumByFieldname(sourceContent, compareBy.Text);
                    rightCompare = findViewColumByFieldname(target_view, compareBy.Text);
                }

                for (int i = 0; i < target_view.Items.Count; i++)
                {
                    ListViewItem findIn = target_view.Items[i];

                    bool isEqual = true;
                    for (int z = 0; z < whereList.CheckedItems.Count; z++)
                    {
                        String comStr = whereList.CheckedItems[z].ToString();
                        int leftCompareSub = findViewColumByFieldname(sourceContent, comStr);
                        int rightCompareSub = findViewColumByFieldname(target_view, comStr);
                        String compvalLeft = item.SubItems[leftCompareSub].Text;
                        String compvalRight = target_view.Items[i].SubItems[rightCompareSub].Text;

                        isEqual = (isEqual && compvalLeft == compvalRight);

                    }


                    if (isEqual && findIn.SubItems[rightCompare].Text == item.SubItems[leftCompare].Text)
                    {
                        target_view.Items[i].BackColor = Color.Aqua;
                        foundCompares++;
                    }
                    else
                    {
                        target_view.Items[i].BackColor = Color.Transparent;
                    }
                }

                LeftTabInfoLabel.Text = "Rows: " + target_view.Items.Count + " / " + limitEnd.Value + " equals on: " + foundCompares;
                if (target_view.Items.Count == limitEnd.Value)
                {
                    LeftTabInfoLabel.ForeColor = Color.Red;
                }
                else
                {
                    LeftTabInfoLabel.ForeColor = Color.DarkGreen;
                }

            }
        }

        private void sourceContent_ItemActivate(object sender, EventArgs e)
        {
            if (sourceContent.SelectedItems.Count > 0) findCompare(sourceContent.SelectedItems[0]);
        }

        private void sourcetabs_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && othertablenameEnabled.Checked)
            {
                if (sourcetabs.SelectedItems.Count > 0)
                {
                    selectRightTable = sourcetabs.SelectedItems[0].Text;
                    toolStripRightTable.Text = selectRightTable;
                    string sql = "";
                    if (whereSql.Text != "")
                    {
                        sql = "SELECT * FROM " + selectRightTable + " WHERE " + whereSql.Text + " LIMIT " + limitStart.Value + "," + limitEnd.Value;
                    }
                    else
                    {
                        sql = "SELECT * FROM " + selectRightTable + " LIMIT " + limitStart.Value + "," + limitEnd.Value;
                    }



                    if (database_target != null)
                    {
                        database_target.connect();

                        MySql.Data.MySqlClient.MySqlDataReader resultTarget = database_target.sql_select(sql);
                        if (resultTarget != null)
                        {
                            database_target.sql_data2ListView(resultTarget, target_view, true);
                        }
                        else
                        {
                            MessageBox.Show("No Result on Target");
                        }

                        database_target.disConnect();
                    }




                }
            }
        }

        private void checkAlterTable()
        {
            if (database_target != null)
            {
                bool reDisConnect = false;
                if (!database_target.isConnected())
                {
                    database_target.connect();
                    reDisConnect = true;
                }
                MysqlTools mTool = new MysqlTools();



                if (selectedTable.Length > 0)
                {
                    string alterSql = mTool.alterTableFromSource(database_source, selectedTable, database_target, modifyTableAddOnly);
                    if (alterSql.Length > 0 /* && MessageBox.Show(this, alterSql, "Confirm Alter Table", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes*/)
                    {

                        if (writeResultInExportToolStripMenuItem.Checked)
                        {
                            exportRowCompareResultInFile(alterSql);
                        }
                        else
                        {

                            SqlExecConfirm execConfirm = new SqlExecConfirm();
                            //execConfirm.parse(alterSql);
                            execConfirm.sqlTextBox.Text = alterSql;

                            if (!database_source.getMysqlVersion().Equals(database_target.getMysqlVersion()))
                            {
                                execConfirm.WarningText.Text = "the version from source is not equals to Target Mysql server " + database_source.getMysqlVersion() + " <=> " + database_target.getMysqlVersion() + " !";
                            }

                            if (execConfirm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {

                                database_target.lastSqlErrorMessage = "";
                                database_target.sql_update(alterSql);
                                if (database_target.lastSqlErrorMessage.Length > 0) MessageBox.Show(this, database_target.lastSqlErrorMessage, "Mysql Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }


                }
                if (reDisConnect) database_target.disConnect();
            }
        }

        private void alterTableFromSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkAlterTable();
        }

        private void searchAndSelectTable()
        {
            bool topItemIsSet = false;
            for (int i = 0; i < sourcetabs.Items.Count; i++)
            {
                int searchLen = searchString.Length;
                if (sourcetabs.Items[i].Text.Length >= searchLen)
                {
                    string leftStr = sourcetabs.Items[i].Text.Substring(0, searchLen);
                    if (leftStr == searchString)
                    {
                        sourcetabs.Items[i].Selected = true;
                        if (!topItemIsSet) sourcetabs.TopItem = sourcetabs.Items[i];
                        topItemIsSet = true;
                    }
                    else
                    {
                        sourcetabs.Items[i].Selected = false;
                    }
                }
                else
                {
                    sourcetabs.Items[i].Selected = false;
                }
            }
        }

        private void searchAndFilterTable()
        {

            for (int i = sourcetabs.Items.Count - 1; i >= 0; i--)
            {
                int searchLen = searchString.Length;
                if (sourcetabs.Items[i].Text.Length >= searchLen)
                {
                    string leftStr = sourcetabs.Items[i].Text.Substring(0, searchLen);
                    if (leftStr == searchString)
                    {
                        //um ... can i do anything here ?
                    }
                    else
                    {
                        sourcetabs.Items.Remove(sourcetabs.Items[i]);
                    }
                }
                else
                {
                    sourcetabs.Items.Remove(sourcetabs.Items[i]);

                }
            }
        }

        private void selectCurrentTable()
        {
            bool topItemIsSet = false;
            for (int i = 0; i < sourcetabs.Items.Count; i++)
            {
                //int searchLen = searchString.Length;

                //string leftStr = sourcetabs.Items[i].Text.Substring(0, searchLen);
                if (selectedTable == sourcetabs.Items[i].Text)
                {
                    sourcetabs.Items[i].Selected = true;
                    if (!topItemIsSet) sourcetabs.TopItem = sourcetabs.Items[i];
                    topItemIsSet = true;
                }
                else
                {
                    sourcetabs.Items[i].Selected = false;
                }

            }
        }

        private int searchTableInList(string what)
        {

            for (int i = 0; i < sourcetabs.Items.Count; i++)
            {
                int searchLen = what.Length;
                if (sourcetabs.Items[i].Text.Length >= searchLen)
                {
                    string leftStr = sourcetabs.Items[i].Text.Substring(0, searchLen);
                    if (leftStr == what)
                    {
                        sourcetabs.TopItem = sourcetabs.Items[i];
                        return i;
                    }

                }

            }
            return -1;
        }

        private void copyDb_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void filterStart_Click(object sender, EventArgs e)
        {
            searchString = filterTables.Text;
            searchAndSelectTable();
        }

        private void recordMacroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            macroRecordBtn.Checked = !recordMacroToolStripMenuItem.Checked;
            recordMacroToolStripMenuItem.Checked = macroRecordBtn.Checked;
            watchEventForMacro = recordMacroToolStripMenuItem.Checked;
        }

        private void editMacroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string textStr = "";
            for (int i = 0; i < macroSource.Count; i++)
            {
                textStr += macroSource[i].Replace(System.Environment.NewLine, "") + System.Environment.NewLine;
            }
            EditMacro em = new EditMacro();
            em.sourceCode.Text = textStr;

            for (int i = 0; i < sourcetabs.Items.Count; i++)
            {
                em.addTables(sourcetabs.Items[i].Text);
            }

            em.parse();
            if (em.ShowDialog() == DialogResult.OK)
            {
                macroSource.Clear();
                for (int i = 0; i < em.sourceCode.Lines.Length; i++)
                {
                    if (em.sourceCode.Lines[i] != "") macroSource.Add(em.sourceCode.Lines[i].Replace(System.Environment.NewLine,""));
                }
            }
            //MessageBox.Show(textStr);
        }


        private void runMacroArray(List<String> macroArray, object sender, EventArgs e)
        {
            List<MacroFunctions> FunctionsList = new List<MacroFunctions>();
            MacroFunctions currentFuntion = new MacroFunctions();
            bool functionFound = false;
            for (int i = 0; i < macroArray.Count; i++)
            {

                string fullText = macroArray[i];

                bool runsAsCommand = true;

                //string functionName = "";
                if (fullText.Length >= 1)
                {
                    /* check on specials at start like comment*/



                    char checkStart = fullText[0];
                    /** a comment*/
                    if (checkStart == '#') runsAsCommand = false;

                    /** end define function */
                    if (functionFound && checkStart == '}')
                    {
                        functionFound = false;

                        FunctionsList.Add(currentFuntion);
                    }

                    /** define a function */
                    if (checkStart == '{' && !functionFound)
                    {
                        functionFound = true;
                    }



                    /** all code is for function*/
                    if (functionFound) runsAsCommand = false;


                    string[] commandString = fullText.Split(';');

                    for (int line = 0; line < commandString.Length; line++)
                    {
                        string commStr = commandString[line];
                        if (commStr.Length > 1)
                        {
                            string[] comAndParam = commStr.Split(':');

                            string commando = comAndParam[0];
                            if (runsAsCommand)
                            {
                                switch (commando)
                                {
                                    /** call a defined function*/
                                    case "call":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];

                                            for (int fu = 0; fu < FunctionsList.Count; fu++)
                                            {
                                                if (sel == FunctionsList[fu].name)
                                                {
                                                    runMacroArray(FunctionsList[fu].source, sender, e);
                                                }
                                            }

                                        }
                                        break;

                                    case "alert":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];
                                            MessageBox.Show(sel);
                                            //handleSoucetablSelect(sel);
                                        }
                                        break;

                                    case "ignoreField":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];
                                            setIgnoreField(sel, true);
                                            //handleSoucetablSelect(sel);
                                        }
                                        break;

                                    case "includeField":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];
                                            setIgnoreField(sel, false);
                                            //handleSoucetablSelect(sel);
                                        }
                                        break;
                                    case "where":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];
                                            whereSql.Text = sel;
                                            //handleSoucetablSelect(sel);
                                        }
                                        break;

                                    case "setLimit":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];

                                            try
                                            {
                                                limitEnd.Value = int.Parse(sel);
                                            }
                                            catch (Exception)
                                            {

                                                MessageBox.Show("ERROR: Invalid argument for setLimit [" + sel + "] must be an Integer",
                                                     "Script Error",
                                                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }


                                        }
                                        break;
                                    case "setStartLimit":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];

                                            try
                                            {
                                                limitStart.Value = int.Parse(sel);
                                            }
                                            catch (Exception)
                                            {

                                                MessageBox.Show("ERROR: Invalid argument for setLimit [" + sel + "] must be an Integer",
                                                     "Script Error",
                                                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }


                                        }
                                        break;
                                    case "SetSecondCheck":
                                        //SetSecondCheck:adminID:Checked;
                                        //SetSecondCheck:adminID:Unchecked;
                                        if (comAndParam.Length >= 2)
                                        {
                                            string sel = comAndParam[1];
                                            string sel2 = comAndParam[2];


                                            for (int sc = 0; sc < whereList.Items.Count; sc++)
                                            {
                                                if (whereList.Items[sc].ToString() == sel)
                                                {
                                                    if (sel2 == "Checked") whereList.SetItemChecked(sc, true);
                                                    else whereList.SetItemChecked(sc, false);
                                                }
                                            }

                                            //handleSoucetablSelect(sel);
                                        }
                                        break;
                                    case "selectSourceTable":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];
                                            sourceSelect.Text = sel;
                                            //handleSoucetablSelect(sel);
                                        }
                                        break;
                                    case "selectTargetTable":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];
                                            targetSelect.Text = sel;
                                            whereSql.Text = "";
                                            //handleSoucetablSelect(sel);
                                        }
                                        break;
                                    case "activateTable":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];
                                            //targetSelect.Text = sel;
                                            int selIndex = searchTableInList(sel);
                                            if (selIndex > -1)
                                            {
                                                sourcetabs.SelectedItems.Clear();
                                                sourcetabs.Items[selIndex].Selected = true;
                                                sourcetabs_ItemActivate(sender, e);
                                                whereSql.Text = "";
                                            }
                                            //handleSoucetablSelect(sel);

                                        }
                                        break;
                                    case "SetComparePrimary":
                                        if (comAndParam.Length >= 1)
                                        {
                                            string sel = comAndParam[1];
                                            compareBy.Text = sel;
                                        }
                                        break;
                                    case "compareRows":
                                        button3_Click(sender, e);
                                        break;
                                }


                            }
                            else
                            {
                                switch (commando)
                                {
                                    case "{function":
                                        if (functionFound)
                                        {
                                            if (comAndParam.Length >= 1)
                                            {
                                                string sel = comAndParam[1];
                                                //MessageBox.Show("Function " + sel);
                                                currentFuntion = new MacroFunctions();
                                                currentFuntion.name = sel;
                                            }
                                        }
                                        break;
                                    default:
                                        if (functionFound)
                                        {
                                            currentFuntion.source.Add(fullText);
                                        }
                                        break;
                                }
                            }
                        }

                    }

                }





            }
        }

        /**
         * Runs current Macro
         * */
        private void runMacroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runMacroToolStripMenuItem.Enabled = false;
            watchEventForMacro = false;

            runMacroArray(macroSource, sender, e);

            watchEventForMacro = recordMacroToolStripMenuItem.Checked;
            runMacroToolStripMenuItem.Enabled = true;
        }



        /**
         * Reset the active macro in Memory
         */
        private void resetMacroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            macroSource.Clear();
            MessageBox.Show("Macro Removed from Memory", "Macro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void limitStart_ValueChanged(object sender, EventArgs e)
        {
            addMacroCommand("setStartLimit:" + limitStart.Value + ";");
        }

        private void limitEnd_ValueChanged(object sender, EventArgs e)
        {
            addMacroCommand("setLimit:" + limitEnd.Value + ";");
        }

        private void loadMacroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openMacro.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBox t1 = new TextBox();
                t1.Text = System.IO.File.ReadAllText(openMacro.FileName);
                macroSource.Clear();
                for (int i = 0; i < t1.Lines.Length; i++)
                {
                    macroSource.Add(t1.Lines[i]);
                }

            }
        }

        private void loadTableProperies()
        {
            if (selectedTable.Length > 0)
            {
                if (useDefinedKeysForCompareToolStripMenuItem.Checked)
                {
                    buildTableKeyChoice();
                }
                else
                {

                    TableProp tmpProp = new TableProp();
                    tmpProp = TablePropertys.getProp(selectedTable);
                    if (tmpProp != null)
                    {
                        if (tmpProp.primaryField.Length > 0) compareBy.Text = tmpProp.primaryField;
                        tmpProp.setCheckedListBox(whereList);
                    }
                }
            }
        }


        private void saveTablePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTable != "")
            {
                TableProp tabProp = new TableProp();
                tabProp.assignedTable = selectedTable;
                tabProp.primaryField = compareBy.Text;
                tabProp.getWhereFieldsFromCheckedListBox(whereList);
                TablePropertys.setTableProp(tabProp);
            }
        }

        private string getPropFilename()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (sourceProfil != null)
            {
                string propSaveFilename = sourceProfil.getProperty("db_host") + '_' + sourceProfil.getProperty("db_schema");
                propSaveFilename = path + @"\" + propSaveFilename.Replace('.', '_') + ".prp";
                //MessageBox.Show(propSaveFilename);
                return propSaveFilename;
            }

            return null;
        }

        private void savePropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = getPropFilename();
            if (filename != null)
            {
                Serializer serializer = new Serializer();
                serializer.SerializeObject(filename, TablePropertys);

            }
        }

        private void loadPropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = getPropFilename();
            if (filename != null && System.IO.File.Exists(filename))
            {
                Serializer serializer = new Serializer();
                TablePropertys = serializer.DeSerializeObject(filename);
            }

        }

        private void whereList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if (optionAutoProp.Checked) saveTablePropertiesToolStripMenuItem_Click(sender, e);
            string itemName = whereList.Items[e.Index].ToString();
            //string value = e.NewValue;
            addMacroCommand("SetSecondCheck:" + itemName + ':' + e.NewValue.ToString() + ";");
        }

        private void copyDb_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (optionAutoProp.Checked) savePropToolStripMenuItem_Click(null, null);
        }

        private void whereList_Leave(object sender, EventArgs e)
        {
            if (optionAutoProp.Checked) saveTablePropertiesToolStripMenuItem_Click(sender, e);
        }

        private void setOptionRightExits()
        {
            fullSynBTN.Enabled = false;
            button3.Enabled = true;
            button2.Enabled = true;
            target_view.Enabled = true;
            splitContainer2.Panel2Collapsed = false;
            targetToolStripMenuItem.Enabled = true;
            createStructureInTargetToolStripMenuItem.Enabled = false;
            fullSyncAtGroupsToolStripMenuItem.Enabled = true;
        }

        private void setOptionRightNotExits()
        {
            if (sourcetabs.SelectedItems.Count > 0) fullSynBTN.Enabled = true;
            button3.Enabled = false;
            button2.Enabled = false;
            target_view.Enabled = false;
            splitContainer2.Panel2Collapsed = true;
            targetToolStripMenuItem.Enabled = false;
            createStructureInTargetToolStripMenuItem.Enabled = true;
            fullSyncAtGroupsToolStripMenuItem.Enabled = true;
        }

        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !controlsToolStripMenuItem.Checked;
        }

        private void macrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            macroToolStrip.Visible = macrosToolStripMenuItem.Checked;
        }

        private void macroToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void macroRecordBtn_Click(object sender, EventArgs e)
        {
            recordMacroToolStripMenuItem_Click(sender, e);
        }

        private void selectettablLable_DoubleClick(object sender, EventArgs e)
        {
            queryBrowser qb = new queryBrowser();
            qb.Name = "QueryBrowser ";
            qb.Text = "Query Browser " + " [" + sourceProfil.getProperty("db_username") + '@' + sourceProfil.getProperty("db_schema") + "]";
            if (sourceProfil.getProperty("set_bgcolor") != null && sourceProfil.getProperty("set_bgcolor").Length > 2)
            {
                qb.BackColor = Color.FromArgb(int.Parse(sourceProfil.getProperty("set_bgcolor")));
            }
            //watcher.profil = profil;
            qb.sensorProfil = sourceProfil;
            qb.startTable = selectedTable;
            qb.loadPlaceHolder();
            qb.listTables();
            qb.Show();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            sourceSelect.Enabled = !sourceSelect.Enabled;
            if (sourceSelect.Enabled) button5.Image = Projector.Properties.Resources.icon_key;
            else button5.Image = Projector.Properties.Resources.lock_unlock;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            targetSelect.Enabled = !targetSelect.Enabled;
            if (targetSelect.Enabled) button6.Image = Projector.Properties.Resources.icon_key;
            else button6.Image = Projector.Properties.Resources.lock_unlock;
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            searchAndFilterTable();
        }

        private void fullSyncAtGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sourcetabs.SelectedItems.Count > 0)
            {
                GroupQuery group = new GroupQuery();


                if (!fullSyncWorker.IsBusy)
                {
                    fullSynBTN.Enabled = false;
                    List<string> datas = new List<string>();
                    for (int i = 0; i < sourcetabs.SelectedItems.Count; i++)
                    {


                        string synctable = sourcetabs.SelectedItems[i].Text;


                        MysqlTools mtools = new MysqlTools();

                        List<string> querys = mtools.getCopyTableStatements(database_source, synctable, database_target);
                        string add = "";
                        for (int p = 0; p < querys.Count; p++)
                        {
                            if (useMassQuery.Checked)
                            {
                                group.richTextBox1.Text += add + querys[p];
                                add = ";" + System.Environment.NewLine;
                            }
                            else
                            {
                                group.queryListing.Items.Add(querys[p]);
                            }

                        }

                    }

                }
                group.ShowDialog();
                checkTableExistsOnTarget();
            }

        }



        private void alterTableForNewFieldsOnlyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            modifyTableAddOnly = alterTableForNewFieldsOnlyToolStripMenuItem.Checked;
        }

        private void copyTableToTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sourcetabs.SelectedItems.Count > 0)
            {



                if (!tableCopyWorker.IsBusy)
                {

                    List<string> datas = new List<string>();
                    for (int i = 0; i < sourcetabs.SelectedItems.Count; i++)
                    {


                        string synctable = sourcetabs.SelectedItems[i].Text;
                        bool canAdd = true;
                        if (database_target.tableExists(synctable))
                        {
                            if (fullsyncAllwasOverwriteTablesToolStripMenuItem.Checked ||
                                MessageBox.Show("Table " + synctable + " allready exists in source!. Overwrite?", "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                            {
                                canAdd = false;
                            }
                        }

                        if (canAdd)
                        {
                            sourcetabs.SelectedItems[i].ImageIndex = 6;
                            datas.Add(synctable);
                        }
                    }

                    if (datas.Count > 0)
                    {
                        DataContent dc = new DataContent();
                        dc.db1 = database_source;
                        dc.db2 = database_target;
                        dc.Source = datas;
                        dc.massInsertQuerys = massInsertQuerysToolStripMenuItem.Checked;
                        sourcetabs.Update();
                        tableCopyWorker.RunWorkerAsync(dc);
                    }
                }

            }
        }

        private void tableCopyWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DataContent dc = (DataContent)e.Argument;
            MysqlTools mtools = new MysqlTools();
            mtools.useMassInsertQuerys = dc.massInsertQuerys;
            Int64 maxAllowedpacket = database_target.getMaxAllowPacketSize();



            for (int i = 0; i < dc.Source.Count; i++)
            {
                string tableName = dc.Source[i];
                tableCopyWorker.ReportProgress(7, tableName);
                List<string> querys = mtools.getCopyTableStatements(database_source, tableName, database_target);


                //String massSql = "";
                StringBuilder massSql = new StringBuilder("");
                Int64 hardMemoryUseLimit = 10000000; // this is the hard set max amout of used bytes

                tableCopyWorker.ReportProgress(FLAG_PROGRESS_MAXIMUM, querys.Count.ToString());

                for (int p = 0; p < querys.Count; p++)
                {

                    tableCopyWorker.ReportProgress(FLAG_PROGRESS_STATE, p.ToString());

                    Int64 neededSize = massSql.Length + querys[p].Length;
                    tableCopyWorker.ReportProgress(0, massSql.Length.ToString());
                    if (neededSize >= maxAllowedpacket || neededSize >= massSql.MaxCapacity || neededSize >= hardMemoryUseLimit)
                    {
                        if (database_target.isConnected()) database_target.disConnect();
                        database_target.connect();
                        tableCopyWorker.ReportProgress(8, tableName);

                        // fire query
                        database_target.sql_select(massSql.ToString());
                        

                        // firing send
                        tableCopyWorker.ReportProgress(5, tableName);
                        if (database_target.lastSqlErrorMessage != "")
                        {
                            MessageBox.Show(database_target.lastSqlErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        massSql = new StringBuilder("");
                        database_target.disConnect();
                    }


                    if (massSql.Length > 0)
                    {
                        string lastChar = massSql.ToString().Substring(massSql.Length - 1);
                        if (lastChar != ";") massSql.Append(";" + querys[p]);
                        else massSql.Append(querys[p]);
                    }
                    else
                    {
                        massSql.Append(querys[p]);
                    }


                }
                // disconnect for new possible resultset

                if (database_target.isConnected()) database_target.disConnect();
                database_target.connect();
                tableCopyWorker.ReportProgress(8, tableName);
                database_target.sql_select(massSql.ToString());
                tableCopyWorker.ReportProgress(5, tableName);
                if (database_target.lastSqlErrorMessage != "")
                {
                    MessageBox.Show(database_target.lastSqlErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void tableCopyWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = "CopyDb";
            runningTask.Visible = false;
            checkTableExistsOnTarget(true);
        }

        private void tableCopyWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            String updateTableName = (String)e.UserState;



            if (e.ProgressPercentage == 0)
            {
                this.Text = "Copydb [ build Query : size = " + updateTableName + "]";
            }
            else
            {

                switch (e.ProgressPercentage)
                {
                    case FLAG_PROGRESS_MAXIMUM:
                        runningTask.Maximum = int.Parse(updateTableName);
                        runningTask.Visible = true;
                        break;
                    case FLAG_PROGRESS_STATE:
                        runningTask.Value = int.Parse(updateTableName);                        
                        break;

                    default:
                        for (int i = 0; i < sourcetabs.Items.Count; i++)
                        {
                            if (sourcetabs.Items[i].SubItems[0].Text == updateTableName)
                            {
                                sourcetabs.Items[i].ImageIndex = e.ProgressPercentage;
                                sourcetabs.Update();
                                return;
                            }

                        }
                        break;
                }
            }
        }

        private void DO_useLimiter_Click_1(object sender, EventArgs e)
        {

        }

        private void writeResultInExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (writeResultInExportToolStripMenuItem.Enabled)
            {
                addMacroCommand("compareRowsExportToLog:true;");
            }
            else
            {
                addMacroCommand("compareRowsExportToLog:false;");
            }
        }

        private void exportRowCompareResultInFile(string sql)
        {
            if (ExportRowCompareFileName == null)
            {
                if (saveExportFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ExportRowCompareFileName = saveExportFileDialog.FileName;
                }
            }

            if (ExportRowCompareFileName != null)
            {

                sql = QueryComposer.addSemikolon(sql);

                if (System.IO.File.Exists(ExportRowCompareFileName))
                {
                    System.IO.File.AppendAllText(ExportRowCompareFileName, sql);
                }
                else
                {
                    System.IO.File.WriteAllText(ExportRowCompareFileName, sql);
                }

            }

        }

        private void buildTableKeyChoice()
        {
            MysqlKeys MKeys = new MysqlKeys();
            MKeys.init(lastSeltable, database_source);

            List<MysqlKeySet> keys = MKeys.getUniqueKeys();
            if (keys.Count > 0)
            {
                for (int op = 0; op < keys.Count; op++)
                {
                    if (op == 0)
                    {
                        compareBy.Text = keys[op].columnName;
                    }
                    else
                    {
                        for (int i = 0; i < whereList.Items.Count; i++)
                        {
                            if (whereList.Items[i].ToString() == keys[op].columnName)
                            {
                                whereList.SetItemChecked(i, true);
                            }
                        }
                    }
                }
            }
        }


        private void useDefinedKeysForCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (useDefinedKeysForCompareToolStripMenuItem.Checked)
            {
                //buildTableKeyChoice();
            }
            loadTableProperies();
        }

        private void leftTabLabel_DoubleClick(object sender, EventArgs e)
        {
            queryBrowser qb = new queryBrowser();
            qb.Name = "QueryBrowser_Target ";
            qb.Text = "Query Browser " + " [" + targetProfil.getProperty("db_username") + '@' + targetProfil.getProperty("db_schema") + "]";
            if (targetProfil.getProperty("set_bgcolor") != null && targetProfil.getProperty("set_bgcolor").Length > 2)
            {
                qb.BackColor = Color.FromArgb(int.Parse(targetProfil.getProperty("set_bgcolor")));
            }
            //watcher.profil = profil;
            qb.sensorProfil = targetProfil;
            qb.startTable = selectedTable;
            qb.loadPlaceHolder();
            qb.listTables();
            qb.Show();
        }

        private void exportAsCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportCsvFileDlg.FileName = "db_diff_" + selectRightTable + ".csv";
            if (exportCsvFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewWorker lww = new ListViewWorker();
                string csvExport = lww.exportCsv(target_view,true);
                System.IO.File.WriteAllText(exportCsvFileDlg.FileName, csvExport);
            }
        }

        private void buildCompareScriptFromSelectedTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
             * activateTable:Conf_Events;               
               compareRows;
             */
            if (sourcetabs.SelectedItems.Count > 0)
            {
               // macroSource.Add("#compare script for " + sourcetabs.SelectedItems.Count + " selected tables;");
                for (int i = 0; i < sourcetabs.SelectedItems.Count; i++)
                {
                    macroSource.Add("activateTable:"+ sourcetabs.SelectedItems[i].Text +";");
                    macroSource.Add("compareRows;");
                }
                macroSource.Add("alert:Check Done... ;");
            }
        }

        private void fullsyncAllwasOverwriteTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
