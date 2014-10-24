using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions; 


namespace Projector
{
    public partial class queryBrowser : Form
    {
        private Pattern patternHandle = new Pattern();

        public Profil sensorProfil;
        public String startTable = "";
        public String startSql = "";

        public bool dlgResult = false;

        private bool subselectMode = false;

        private MysqlHandler database = null;
        private List<string> autoCompleteList = new List<string>();
        private List<string> fieldList = new List<string>();

        private List<string> orderListing = new List<string>();

        public Boolean orderDesc = false;


        private string cellvalue = "";
        ListViewHitTestInfo lastSelectedItemInfo;

        private ListView TriggerList = new ListView();

        private bool forceUpdateWithoutKeys = false;

        private List<string> sqlComands = new List<string>();

        private bool autoGet = false;
        private int autoGetLimit = 100;

        private int columnAutoSizeMode = 0;
        private string lastSelectedtable = ""; // for check on second klick on same table

        private bool addAllFieldsToAutoComplete = false;

        MysqlProcedures myFuns = new MysqlProcedures();

        HighlighterMysql highlight = new HighlighterMysql();
        AutoCompletion AutoText = new AutoCompletion();

        private bool showProcedures = true;

        // elemets for table search mask
        List<TextBox> SearchVarchar = new List<TextBox>();
        List<Label> SearchLabels = new List<Label>();
        List<ComboBox> SearchEnums = new List<ComboBox>();
        List<DateTimePicker> serachDateTime = new List<DateTimePicker>();
        List<NumericUpDown> SearchInts = new List<NumericUpDown>();

        private QueryComposer maskQuery;

        private PlacerHolder placeHolder = new PlacerHolder();

        // bookmarks
        private BookmarkManager bookMarks = new BookmarkManager();


        public queryBrowser()
        {
            InitializeComponent();
            splitContainer1.Panel1Collapsed = true;
            tabControl1.SelectedIndex = 1;
            BookMarkSerializer serializer = new BookMarkSerializer();
            if (System.IO.File.Exists(bookMarks.getDefaultFilename()))
            {
                bookMarks = serializer.DeSerializeObject(bookMarks.getDefaultFilename());
                updateBookmarks();
            }
            
           

            textBox1.Text = startSql;
            cellEditField.Visible = false;
            searchTableTextBox.Visible = false;

            dialogToolStrip.Visible = false;
            editBox.Visible = false;
        }

        public void loadPlaceHolder()
        {
            PlaceHolderSerializer pSerializer = new PlaceHolderSerializer();
            if (System.IO.File.Exists(pSerializer.getDefaultFilename(sensorProfil)))
            {
                placeHolder = pSerializer.DeSerializeObject(pSerializer.getDefaultFilename(sensorProfil));
            }
        }

        private String getOrder()
        {
            string orderStr = "";
            bool add = false;
            for (int i = 0; i < this.orderListing.Count; i++)
            {
                if (add == true) orderStr = orderStr + ",";
                orderStr = orderStr + this.orderListing[i];
                add = true;

            }

            if (this.orderDesc && add) orderStr = orderStr + " DESC "; 
            return orderStr;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        public void enableSubSelectMode()
        {
            subselectMode = true;
           
        }


        public void enableDialogMode()
        {
            dialogToolStrip.Visible = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
        }

        public void enableSqlView()
        {
            toolStripButton3_Click(null, null);
        }

        void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            MaskedTextBox maskedTextBox1 = (MaskedTextBox)sender;
            if (maskedTextBox1.MaskFull)
            {
                toolTip1.ToolTipTitle = "Input Rejected - Too Much Data";
                toolTip1.Show("You cannot enter any more data into the date field. Delete some characters in order to insert more data.", maskedTextBox1, maskedTextBox1.Location.X, maskedTextBox1.Location.Y, 5000);
            }
            else if (e.Position == maskedTextBox1.Mask.Length)
            {
                toolTip1.ToolTipTitle = "Input Rejected - End of Field";
                toolTip1.Show("You cannot add extra characters to the end of this date field.", maskedTextBox1, maskedTextBox1.Location.X, maskedTextBox1.Location.Y, 5000);
            }
            else
            {
                toolTip1.ToolTipTitle = "Input Rejected";
                toolTip1.Show("You can only add numeric characters (0-9) into this date field.", maskedTextBox1, maskedTextBox1.Location.X, maskedTextBox1.Location.Y, 5000);
            }
        }

        void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // The balloon tip is visible for five seconds; if the user types any data before it disappears, collapse it ourselves.
            //toolTip1.Hide(maskedTextBox1);
        }

        public string composeQueryMask(string input)
        {
            string query = input;
            return query;
        }

        public void fireQuery()
        {
            database = new MysqlHandler(sensorProfil);
            database.connect();
            
            if (database != null)
            {
                if (!mysqlWorker.IsBusy)
                {
                    /*
                    MySql.Data.MySqlClient.MySqlDataReader reader = database.sql_select(textBox1.Text);

                    database.sql_data2ListView(reader, listView1,true);
                    reader.Close();
                    */
                    listView1.Items.Clear();
                    ListviewForWorker copy = new ListviewForWorker(new ListView(), textBox1.Text);
                    tableView.Enabled = false;
                    button1.Enabled = false;
                    if (!mysqlWorker.IsBusy)
                    {
                        mysqlWorker.RunWorkerAsync(copy);
                        rowResultCount.Text = "Reading...";
                    }
                    else
                    {
                        MessageBox.Show("Progress is busy");
                    }
                }
                else
                {
                    MessageBox.Show("Progress is busy");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            resetExplainMsg();
            fireQuery();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        

        public void listTables()
        {
            database = new MysqlHandler(sensorProfil);
            database.connect();
            tableView.Items.Clear();
            tableView.Groups.Clear();
            if (database != null)
            {
                MySql.Data.MySqlClient.MySqlDataReader reader = database.sql_select("show tables");
                
                // error ?
                if (reader == null)
                {
                    MessageBox.Show(this, "Error on Reading ", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                trigger_Tables.Items.Clear();
                database.sql_data2ListView(reader, tableView);
                reader.Close();
                leftJoinTables.Items.Clear();

                string[] mysqlWords = highlight.getReservedWords();
                if (null != mysqlWords)
                {
                    for (int uf = 0; uf < mysqlWords.Length; uf++)
                    {
                        AutoText.addWord(mysqlWords[uf]);
                    }
                }
                

                if (tableView.Columns.Count > 0)
                {
                    int w = tableView.Width / tableView.Columns.Count;

                    tableView.Columns[0].Width = w;

                    for (int i = 0; i < tableView.Items.Count; i++)
                    {
                        //textBox1.AutoCompleteCustomSource.Add(tableView.Items[i].Text);
                        TablesAutoComplete.Items.Add(tableView.Items[i].Text);
                        autoCompleteList.Add(tableView.Items[i].Text);
                        trigger_Tables.Items.Add(tableView.Items[i].Text);
                        leftJoinTables.Items.Add(tableView.Items[i].Text);

                        // tablenames to highlighter 
                        highlight.addTableName(tableView.Items[i].Text);

                        // tablenames to autocomplete
                        AutoText.addWord(tableView.Items[i].Text);

                        if (this.addAllFieldsToAutoComplete)
                        {
                            List<MysqlStruct> strInfo = database.getAllFieldsStruct(tableView.Items[i].Text);

                            for (int stc = 0; stc < strInfo.Count; stc++)
                            {
                                AutoText.addWord(tableView.Items[i].Text + '.' + strInfo[stc].name);
                                highlight.addFieldName(tableView.Items[i].Text + '.' + strInfo[stc].name);
                                highlight.addFieldName(strInfo[stc].name);
                            }
                        }

                    }

                }
                ListViewWorker work = new ListViewWorker();

                // add groups to tableview
                work.buildGroups(tableView);

                work.setImageIndex(tableView, 1, 2);
                if (startTable != "") work.searchAndSelect(startTable, tableView, 0);
                TablesAutoComplete.Visible = false;

                MySql.Data.MySqlClient.MySqlDataReader triggerReader = database.sql_select("show triggers");
                database.sql_data2ListView(triggerReader, TriggerList);

                
                for (int i = 0; i < TriggerList.Items.Count; i++)
                {
                    ListViewItem tmpItm = new ListViewItem();
                    tmpItm.Text = TriggerList.Items[i].Text;
                    tmpItm.ImageIndex = 2;
                    tmpItm.ToolTipText = i + "";
                    tableView.Items.Add(tmpItm);
                    

                }
                work.setRowColors(tableView, Color.LightBlue, Color.LightCyan);
                work.autoSizeColumns(tableView, ColumnHeaderAutoResizeStyle.ColumnContent);
                database.disConnect();
                
                myFuns.getProcedures(database);
                ListViewGroup procGroup = new ListViewGroup();
                procGroup.Header = "Procedures";
                procGroup.Name = "Procedures";

                tableView.Groups.Add(procGroup);
                if (!myFuns.isError)
                {
                    
                    StoredProcedure tmpStored = new StoredProcedure();
                    tmpStored = myFuns.get();
                    while (tmpStored != null)
                    {
                        tmpStored = myFuns.get();
                        if (tmpStored != null && tmpStored.db == sensorProfil.getProperty("db_schema"))
                        {
                            ListViewItem spView = new ListViewItem();
                            spView.BackColor = Color.LightYellow;
                            spView.ImageIndex = 3;
                            spView.Text =  tmpStored.name;
                            spView.Group = procGroup;
                            tableView.Items.Add(spView);
                        }
                    }
                }
                else
                {
                   // MessageBox.Show(this, myFuns.errorMessage, "Error on Reading Mysql Procedures", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void getAutoCompleteList(string str)
        {
            TablesAutoComplete.Items.Clear();

            for (int i = 0; i < fieldList.Count; i++)
            {
                if (fieldList[i].ToLower().Contains(str.ToLower()))
                {

                    ListViewItem tmp = new ListViewItem();
                    tmp.Text = fieldList[i];
                    tmp.ToolTipText = "Field: " + tmp.Text;
                    tmp.ForeColor = Color.DarkGoldenrod;
                    tmp.Group = TablesAutoComplete.Groups[0];
                    TablesAutoComplete.Items.Add(tmp);
                }
            }

            for (int i = 0; i < autoCompleteList.Count; i++)
            {

                if (autoCompleteList[i].ToLower().Contains(str.ToLower()))
                {
                    ListViewItem tmp = new ListViewItem();
                    tmp.Text = autoCompleteList[i];
                    tmp.ToolTipText = "Table: " + tmp.Text;
                    tmp.ForeColor = Color.DarkBlue;
                    tmp.Group = TablesAutoComplete.Groups[1];
                    TablesAutoComplete.Items.Add(tmp);
                }
            }

           

        }

        private void tableView_ItemActivate(object sender, EventArgs e)
        {
            
        }

        private void clearmask()
        {
            
            SearchLabels.Clear();
            SearchVarchar.Clear();
            maskBox.Controls.Clear();
            

        }

        private void getTableMask(string tableName)
        {
            this.getTableMask(tableName, false);
        }

        private int getPlaceHolderValue(int defaultValue,string name)
        {
            int res = defaultValue;
            if (placeholderReplace.Checked && placeHolder.exists(name))
            {
                string val = placeHolder.value(name);
                try
                {
                    res = int.Parse(val);
                    maskQuery.addWhere(name, val);
                    
                }
                catch
                {
                    // nothing to catch ... if it not wirks res allready have the right response
                }              
            }
            return res;
        }

        private void findInputeElementbyFieldNameUpdate(string fieldName, string value)
        {
            string cpmStr = "DYNAMIC_PROJ_" + fieldName;
            for (int i = 0; i < maskBox.Controls.Count; i++)
            {
                if (maskBox.Controls[i].Name == cpmStr)
                {
                    Type objType = maskBox.Controls[i].GetType();
                    if (objType.Name == "NumericUpDown")
                    {
                        int numval = int.Parse(value);
                        NumericUpDown tmpas = (NumericUpDown) maskBox.Controls[i];
                        tmpas.Value = numval;
                    }

                    else if (objType.Name == "TextBox")
                    {
                        TextBox tmpas = (TextBox)maskBox.Controls[i];
                        tmpas.Text = value;
                    }
                    //MaskedTextBox
                    else if (objType.Name == "MaskedTextBox")
                    {
                        MaskedTextBox tmpas = (MaskedTextBox)maskBox.Controls[i];
                        tmpas.Text = value;
                    }

                    else if (objType.Name == "ComboBox")
                    {
                        ComboBox tmpas = (ComboBox)maskBox.Controls[i];
                        tmpas.Text = value;
                    }
                }
            }
        }


        private void getTableMask(string tableName, bool adAsLeftJoin)
        {
            //if (maskQuery == null) maskQuery = new QueryComposer(tableName);
            //else maskQuery.TableName = tableName;

            maskQuery = new QueryComposer(tableName);
            if (subselectMode) maskQuery.useLimit = false;

            List<MysqlStruct> structInfo = new List<MysqlStruct>();

            MysqlHandler stdatabase = new MysqlHandler(sensorProfil);
            stdatabase.connect();
            
            structInfo = stdatabase.getAllFieldsStruct(tableName);
            stdatabase.disConnect();

            maskBox.Text = tableName;
            if (!adAsLeftJoin) clearmask();
            int addedTop = 20;
            int elementHeight = 21;

            // combobox for selecting one resultfield
            ComboBox subselectResult = new ComboBox();

            MysqlKeys tableKeys = new MysqlKeys();
            tableKeys.init(tableName, database);

            Font primFont = new Font("ARIAL", 9, FontStyle.Bold);
            Font uniqueKeyFont = new Font("ARIAL", 9, FontStyle.Italic);

            int addQueryBtnEveryCnt = 10;
            int rowCnt = 0;

            for (int i = 0; i < structInfo.Count; i++)
            {
                elementHeight = 23;
                Label infoStr = new Label();
                infoStr.Text = structInfo[i].name + " [" + structInfo[i].realType + "]";


                if (tableKeys.columnIsUnique(structInfo[i].name))
                    infoStr.Font = uniqueKeyFont;

                if (tableKeys.columnIsPrimary(structInfo[i].name))                                    
                    infoStr.Font = primFont;

                if (tableKeys.getKeyByFieldName(structInfo[i].name) != null)
                    infoStr.ForeColor = Color.Blue;

                if (subselectMode) subselectResult.Items.Add(structInfo[i].name);


                //infoStr.Text = structInfo[i].name;
                infoStr.Left = 10;
                infoStr.Top = addedTop;
                infoStr.AutoSize = true;
                              
              //  infoStr.Visible = true;

                SearchLabels.Add(infoStr);
                maskBox.Controls.Add(infoStr);

                // compareType choice
                ComboBox compareType = new ComboBox();
                compareType.Top = addedTop;
                compareType.Left = 170;
                compareType.Text = "=";
                compareType.Width = 40;
                compareType.Items.Add("=");
                compareType.Items.Add(">");
                compareType.Items.Add(">=");
                compareType.Items.Add("<");
                compareType.Items.Add("<=");
                compareType.Items.Add("!=");
                compareType.Items.Add("LIKE");
                compareType.Items.Add("SUBSELECT");
                compareType.Name = "DYNAMIC_WHERE_" + structInfo[i].name;
                compareType.TextChanged += new System.EventHandler(dynWhereCompare);

                maskBox.Controls.Add(compareType);

                rowCnt++;
                if (rowCnt >= addQueryBtnEveryCnt)
                {
                    rowCnt = 0;
                    Button smfireSQL = new Button();
                    smfireSQL.Top = addedTop;
                    smfireSQL.Left = 580;
                    smfireSQL.Text = "";
                    smfireSQL.Height = 36;
                    smfireSQL.Width = 36;
                    smfireSQL.ImageAlign = ContentAlignment.TopCenter;
                    smfireSQL.TextAlign = ContentAlignment.BottomCenter;
                    smfireSQL.Image = Projector.Properties.Resources.view_refresh;

                    smfireSQL.Click += new System.EventHandler(dynRunQuery);
                    maskBox.Controls.Add(smfireSQL);

                }

                switch (structInfo[i].realType)
                {
                    case "varchar":
                        TextBox tmpBox = new TextBox();
                        tmpBox.Left = 250;
                        tmpBox.Top = addedTop;
                        tmpBox.Width = 200;
                        tmpBox.MaxLength = structInfo[i].len;
                        tmpBox.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpBox.TextChanged += new System.EventHandler(dynInput);
                        maskBox.Controls.Add(tmpBox);

                        maskBox.Controls.Add(addWhereDel(addedTop, structInfo[i].name));

                        break;
                    case "text":
                        TextBox tmpBox2 = new TextBox();
                        tmpBox2.Left = 250;
                        tmpBox2.Top = addedTop;
                        tmpBox2.Width = 200;
                        tmpBox2.Height = 100;
                        tmpBox2.Multiline = true;

                        elementHeight = 110;

                        tmpBox2.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpBox2.TextChanged += new System.EventHandler(dynInput);
                        maskBox.Controls.Add(tmpBox2);

                        maskBox.Controls.Add(addWhereDel(addedTop, structInfo[i].name));
                        break;
                    case "int":
                    case "tinyint":
                    case "bigint":
                    case "mediumint":
                    case "smallint":
                        NumericUpDown tmpNumber = new NumericUpDown();
                        tmpNumber.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpNumber.Left = 250;
                        tmpNumber.Top = addedTop;
                        tmpNumber.Minimum = structInfo[i].minIntVal;
                        tmpNumber.Maximum = structInfo[i].maxIntval;
                        tmpNumber.ValueChanged += new System.EventHandler(dynNumeric);

                        tmpNumber.Value = getPlaceHolderValue(0, structInfo[i].name);
                        maskBox.Controls.Add(tmpNumber);

                        maskBox.Controls.Add(addWhereDel(addedTop, structInfo[i].name));
                        break;

                    case "timestamp":
                    case "datetime":
                        MaskedTextBox tmpTimePicker = new MaskedTextBox();
                        tmpTimePicker.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpTimePicker.Left = 250;
                        tmpTimePicker.Top = addedTop;

                        tmpTimePicker.Mask = "0000-00-00 00:00:00";
                        tmpTimePicker.Width = 110;

                        tmpTimePicker.TextChanged += new System.EventHandler(dynMaskInput);
                        maskBox.Controls.Add(tmpTimePicker);

                        maskBox.Controls.Add(addWhereDel(addedTop, structInfo[i].name));
                        break;

                    case "enum":
                        ComboBox tmpEnum = new ComboBox();
                        tmpEnum.Top = addedTop;
                        tmpEnum.Left = 250;
                        tmpEnum.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        if (structInfo[i].enums != null)
                        {
                            for (int b = 0; b < structInfo[i].enums.Length; b++)
                            {
                                tmpEnum.Items.Add(structInfo[i].enums[b]);
                            }
                        }
                        tmpEnum.TextChanged += new System.EventHandler(dynComboBoxInput);
                        maskBox.Controls.Add(tmpEnum);

                        maskBox.Controls.Add(addWhereDel(addedTop, structInfo[i].name));
                        break;

                    case "set":
                        CheckedListBox tmpChkBox = new CheckedListBox();
                        tmpChkBox.Top = addedTop;
                        tmpChkBox.Left = 250;
                        tmpChkBox.Height = 20;
                        tmpChkBox.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        if (structInfo[i].enums != null)
                        {
                            for (int b = 0; b < structInfo[i].enums.Length; b++)
                            {
                                tmpChkBox.Items.Add(structInfo[i].enums[b]);
                                tmpChkBox.Height += 15;
                            }
                        }
                        tmpChkBox.Click += new System.EventHandler(dynCheckBoxInput);
                        tmpChkBox.Leave += new System.EventHandler(dynCheckBoxInput);
                        maskBox.Controls.Add(tmpChkBox);

                        maskBox.Controls.Add(addWhereDel(addedTop, structInfo[i].name));
                        addedTop += tmpChkBox.Height;
                        break; 

                    default:
                        TextBox tmpBox3 = new TextBox();
                        tmpBox3.Left = 250;
                        tmpBox3.Top = addedTop;
                        tmpBox3.Width = 200;
                        tmpBox3.MaxLength = structInfo[i].len;
                        tmpBox3.Name = "DYNAMIC_PROJ_" + structInfo[i].name;
                        tmpBox3.TextChanged += new System.EventHandler(dynInput);
                        maskBox.Controls.Add(tmpBox3);

                        maskBox.Controls.Add(addWhereDel(addedTop, structInfo[i].name));
                        break;
                }
                

                // finaly calc new height
                addedTop += elementHeight;


            }

            // adding controlls

            if (subselectMode)
            {
                addedTop += 20;
                subselectResult.Top = addedTop;
                subselectResult.Left = 300;
                subselectResult.BackColor = Color.LightYellow;
                subselectResult.SelectedIndexChanged += new System.EventHandler(dynSelectResult);
                subselectResult.DropDownStyle = ComboBoxStyle.DropDownList;
                maskBox.Controls.Add(subselectResult);

                Label tLabel = new Label();
                tLabel.Text = "Select Field for Subquery";
                tLabel.Left = 100;
                tLabel.Top = addedTop;
                tLabel.AutoSize = true;
                maskBox.Controls.Add(tLabel);

                addedTop += 20;
            }
            else
            {
                addedTop += 20;
                NumericUpDown tmpNumber = new NumericUpDown();
                tmpNumber.Name = "limitSetter";
                tmpNumber.Left = 90;
                tmpNumber.Top = addedTop;
                tmpNumber.Minimum = 1;
                tmpNumber.Maximum = 500000;
                tmpNumber.Increment = 100;
                tmpNumber.Value = autoGetLimit;
                tmpNumber.ValueChanged += new System.EventHandler(setLimitFunc);
                maskBox.Controls.Add(tmpNumber);

                Label infoStr = new Label();
                infoStr.Text = "LIMIT ";
                infoStr.Left = 10;
                infoStr.Top = addedTop;
                infoStr.AutoSize = true;
                maskBox.Controls.Add(infoStr);
                addedTop += 10;
            }

            addedTop += 10;
            
            Button fireSQL = new Button();
            fireSQL.Top = addedTop;
            fireSQL.Left = 300;
            fireSQL.Text = "Start";
            fireSQL.Height = 46;
            fireSQL.Width = 96;
            fireSQL.ImageAlign = ContentAlignment.TopCenter;
            fireSQL.TextAlign = ContentAlignment.BottomCenter;
            fireSQL.Image = Projector.Properties.Resources.view_refresh;
            
            fireSQL.Click += new System.EventHandler(dynRunQuery);
            maskBox.Controls.Add(fireSQL);

        }

        private void setLimitFunc(object sender, System.EventArgs e)
        {
            NumericUpDown limitSetter = (NumericUpDown)sender;
            autoGetLimit = (int) limitSetter.Value;
            maskQuery.setLimitRange(autoGetLimit);
            textBox1.Text = maskQuery.getSelect();
            parseSqlTextBox();
        }

        private void dynSelectResult(object sender, System.EventArgs e)
        {
            ComboBox VarCharAdd = (ComboBox)sender;

            maskQuery.setResultField(VarCharAdd.Text);
            textBox1.Text = maskQuery.getSelect();
            parseSqlTextBox();
                        
        }

        private Button addWhereDel(int y,string name)
        {
            Button addingDelBtn = new Button();
            addingDelBtn.Name = "DYNAMIC_WHERE_" + name;
            addingDelBtn.Left = 470;
            addingDelBtn.Top = y;
            addingDelBtn.Width = 100;
            addingDelBtn.Text = "ignore on Where";
            addingDelBtn.FlatStyle = FlatStyle.System;
            addingDelBtn.Click += new System.EventHandler(delWhere);
            return addingDelBtn;
        }

        // dynamic events
        private void dynInput(object sender, System.EventArgs e)
        {
            TextBox VarCharAdd = (TextBox)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");            
            maskQuery.addWhere(name, VarCharAdd.Text);            
            textBox1.Text = maskQuery.getSelect();
            parseSqlTextBox();
            VarCharAdd.BackColor = Color.LightGreen;
        }

        private void dynMaskInput(object sender, System.EventArgs e)
        {
            MaskedTextBox VarCharAdd = (MaskedTextBox)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");

            maskQuery.addWhere(name, VarCharAdd.Text);            
            textBox1.Text = maskQuery.getSelect();
            parseSqlTextBox(); 
            VarCharAdd.BackColor = Color.LightGreen;
        }


        private void delWhere(object sender, System.EventArgs e)
        {
            Button VarCharAdd = (Button)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_WHERE_", "");
            maskQuery.removeWhere(name);
            textBox1.Text = maskQuery.getSelect();
            parseSqlTextBox(); 
            for (int i = 0; i < maskBox.Controls.Count; i++)
            {
                if (maskBox.Controls[i].Name == "DYNAMIC_PROJ_" + name)
                {
                    maskBox.Controls[i].BackColor = Color.LightYellow;
                }
            }

        }
        //CheckedListBox
        private void dynCheckBoxInput(object sender, System.EventArgs e)
        {
            CheckedListBox VarCharAdd = (CheckedListBox)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");

            string whereFieldAdd = "";
            VarCharAdd.BackColor = Color.LightSteelBlue;
            string add = "";
            for (int i = 0; i < VarCharAdd.CheckedItems.Count; i++)
            {
                whereFieldAdd += add + VarCharAdd.CheckedItems[i].ToString();
                add = ",";
            }

            maskQuery.addWhere(name, whereFieldAdd);
            if (VarCharAdd.Text == "") maskQuery.removeWhere(name);
            textBox1.Text = maskQuery.getSelect();
            parseSqlTextBox();
            VarCharAdd.BackColor = Color.LightGreen;
        }


        private void dynComboBoxInput(object sender, System.EventArgs e)
        {
            ComboBox VarCharAdd = (ComboBox)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");
            
            VarCharAdd.BackColor = Color.LightSteelBlue;

            maskQuery.addWhere(name, VarCharAdd.Text);
            if (VarCharAdd.Text == "") maskQuery.removeWhere(name);
            textBox1.Text = maskQuery.getSelect();
            parseSqlTextBox(); 
            VarCharAdd.BackColor = Color.LightGreen;
        }

        private void dynNumeric(object sender, System.EventArgs e)
        {
            NumericUpDown VarCharAdd = (NumericUpDown)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_PROJ_", "");
            
            maskQuery.addWhere(name, VarCharAdd.Value.ToString());
            textBox1.Text = maskQuery.getSelect();
            parseSqlTextBox(); 
            VarCharAdd.BackColor = Color.LightGreen;
        }

        private void dynWhereCompare(object sender, System.EventArgs e)
        {
            ComboBox VarCharAdd = (ComboBox)sender;
            String name = VarCharAdd.Name.Replace("DYNAMIC_WHERE_", "");

            //maskQuery.addWhere(name, VarCharAdd.Value.ToString());

            if (VarCharAdd.Text.ToUpper() == "SUBSELECT"){
                string subselect = helperGetSubSelect(maskQuery.getSetValue(name));
                maskQuery.addWhere(name, subselect);
            }

            maskQuery.addWhereComp(name,VarCharAdd.Text);
            textBox1.Text = maskQuery.getSelect();
            parseSqlTextBox(); 
            VarCharAdd.BackColor = Color.LightGreen;
        }

        private String helperGetSubSelect(string oldValue)
        {
            queryBrowser subQBrowser = new queryBrowser();
            subQBrowser.sensorProfil = this.sensorProfil;
            subQBrowser.setSqlStatement(oldValue);
            subQBrowser.listTables();
            subQBrowser.enableDialogMode();
            
            subQBrowser.enableSubSelectMode();
            subQBrowser.ShowDialog();
            

            if (subQBrowser.dlgResult) return subQBrowser.getSqlStatement();
            else return "";
        }

        private void checkExplain()
        {
            if (explainEnabled.Checked)
            {
                rowResultCount.Text = "EXPLAIN QUERY...";
                MysqlExplain explain = new MysqlExplain(maskQuery, database);

                if (explain.useKeys())
                {
                    
                    explainStatusLabel.ForeColor = Color.DarkGreen;
                    explainStatusLabel.BackColor = Color.LightGreen;
                    explainStatusLabel.Text = "Using Keys ";
                    explainStatusLabel.ToolTipText = explain.getExplainReadableInfo();

                    toolTip1.SetToolTip(statusStrip1, explain.getExplainReadableInfo());
                    toolTip1.ToolTipTitle = "Explain Result";
                    toolTip1.IsBalloon = true;
                    toolTip1.AutoPopDelay = 10000;
                    toolTip1.ShowAlways = true;
                    toolTip1.ReshowDelay = 500;
                    

                }
                else
                {
                    explainStatusLabel.ForeColor = Color.DarkRed;
                    explainStatusLabel.BackColor = Color.LightCoral;
                    explainStatusLabel.Text = "No keys used";
                    explainStatusLabel.ToolTipText = "this Query ist not optimized for using indizies";

                    toolTip1.SetToolTip(statusStrip1, "this Query ist not optimized for using indizies");
                   
                }
                rowResultCount.Text = "EXPLAIN QUERY...DONE";
            }
        }

        private void resetExplainMsg()
        {
            explainStatusLabel.ForeColor = Color.Black;
            explainStatusLabel.BackColor = Color.LightGray;
            explainStatusLabel.Text = "EXPLAIN";
        }

        private void dynRunQuery(object sender, System.EventArgs e)
        {

            if (subselectMode && maskQuery.ifAllfieldsSelected())
            {
                MessageBox.Show("Please Select a Result Field for Subselect", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            

            tabControl1.SelectedIndex = 0;
            checkExplain();
            fireQuery();
            //checkExplain();
        }

        private void tableView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // only simple click allowed
            if (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.Alt)
            {
                return;
            }

            searchTableTextBox.Visible = false;
                                    
            if (e.Item.ImageIndex == 1)
            {
                
                if (lastSelectedtable != e.Item.Text || lastSelectedtable == "")
                {
                    if (tabControl1.SelectedIndex > 1) tabControl1.SelectedIndex = 0;

                    resetExplainMsg();             
                    getTableMask(e.Item.Text);
                    lastSelectedtable = e.Item.Text;
                    this.Name = lastSelectedtable;
                    TableNameView.Text = lastSelectedtable;
                    selectedTableLabel.Text = lastSelectedtable;
                    /** get indizies */
                    //MysqlKeys mKeys = new MysqlKeys(lastSelectedtable, database);


                    textBox1.Text = maskQuery.getSelect();
                    parseSqlTextBox(); 
                    
                    List<MysqlStruct> structs = database.getAllFieldsStruct(e.Item.Text);
                    fieldList.Clear();
                    leftJoinSource.Items.Clear();

                    for (int i = 0; i < structs.Count; i++)
                    {
                        fieldList.Add(structs[i].name);
                        leftJoinSource.Items.Add(structs[i].name);

                    }

                    if (autoGet && !mysqlWorker.IsBusy)
                    {
                        tableView.Enabled = false;
                        //textBox1.Text = "SELECT * FROM " + e.Item.Text + " LIMIT " + autoGetLimit;
                        //parseSqlTextBox(); 
                        fireQuery();
                    }
                }
                else
                {
                    //fireQuery();
                    //lastSelectedtable = "";
                }

            } else if (e.Item.ImageIndex==2) {
                // triggers
                tabControl1.SelectedIndex = 1;
                int getInfo = int.Parse(e.Item.ToolTipText);
                if (TriggerList.Items.Count > getInfo)
                {
                    triggerCode.Text = TriggerList.Items[getInfo].SubItems[3].Text;
                    trigger_Tables.Text = TriggerList.Items[getInfo].SubItems[2].Text;
                    trigger_event.Text = TriggerList.Items[getInfo].SubItems[1].Text;
                    trigger_timing.Text = TriggerList.Items[getInfo].SubItems[4].Text;
                    trigger_name.Text = TriggerList.Items[getInfo].SubItems[0].Text;
                }
            }  else if (e.Item.ImageIndex==3) {
              // functions and stored procedures
                tabControl1.SelectedIndex = 3;
                procName.Text = e.Item.Text;
                StoredProcedure proc = myFuns.getProcByName(e.Item.Text);
                if (proc != null)
                {
                    procSource.Text = proc.created;
                    if (sqlHighlighting.Checked) highlight.parse(procSource);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
           // TabPage AddPage = new TabPage();
           // ObjectCloner cloner = new ObjectCloner();
           // AddPage =(TabPage) cloner.CloneObject(tabControl1.TabPages[0]);
            /*
            foreach (Control control in tabControl1.TabPages[0].Controls)
            {
                Control tmp = new Control();
                tmp =(Control) cloner.CloneObject(control);   
                AddPage.Controls.Add(tmp);
            }*/
            
            // tabControl1.TabPages.Add(AddPage);
        }

        private void mysqlWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ListviewForWorker tmpObj = (ListviewForWorker)e.Argument;
            MySql.Data.MySqlClient.MySqlDataReader reader = database.sql_select(tmpObj.sql);

            if (database.lastSqlErrorMessage != "")
            {
                tmpObj.errorMessage = database.lastSqlErrorMessage;
            }
            

            //runningLabel.Text = "Start";
            database.listViewColumAddRowInfo = true;
            database.listViewAddImageIndex = 0;
            database.sql_data2ListView(reader, tmpObj.listView, true);
            database.listViewColumAddRowInfo = false;
            database.listViewAddImageIndex = 2;
            if (reader != null)
            {

                reader.Close();
            }
            e.Result = tmpObj;
        }

        private void mysqlWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           ListviewForWorker tmpObj = (ListviewForWorker)e.Result;
           ListViewWorker worker = new ListViewWorker();
           if (tmpObj.listView.Items.Count > 0)
           {
               worker.copyListView(tmpObj.listView, listView1, -1, 0);
               worker.setRowColors(listView1, Color.Transparent, Color.LightYellow);
               worker.searchAndmark("null", listView1, 0, Color.SkyBlue);
               autosortColumns();

           }
           else
           {
               if (tmpObj.errorMessage != null && tmpObj.errorMessage != "")
               {
                   MessageBox.Show(tmpObj.errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
               else
               {
                   MessageBox.Show("No Results", "Query", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               
           }
           //listView1.Items = tmpObj.listView.Items;
           //runningLabel.Text = "Finished";
           tableView.Enabled = true;
           
           button1.Enabled = true;
           rowResultCount.Text = "Rows " + tmpObj.listView.Items.Count;
        }

        private void mysqlWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            /*if (listView1.SelectedItems.Count > 0)
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    listView1.SelectedItems[i].ForeColor = Color.Blue;
                }
            }*/
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            /*
            if (listView1.SelectedItems.Count > 0)
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    if (listView1.SelectedItems[i].BackColor == Color.LightBlue) listView1.SelectedItems[i].BackColor = Color.Transparent;
                    else listView1.SelectedItems[i].BackColor = Color.LightBlue;
                }
            }*/
            updateCellToolStripMenuItem_Click(sender, e);
        }

        private string getlastSelectedWord(RichTextBox textBox1)
        {
            int selPos = textBox1.SelectionStart;
            string leftTxt = textBox1.Text.Substring(0, selPos);
            //string[] allwords = leftTxt.Split(' ');
            string[] allwords = Regex.Split(leftTxt, "[ ,=+-/*]");
            leftTxt = allwords[allwords.Length - 1];
            //this.Text = selPos + " " + leftTxt;
            return leftTxt;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Return)
            {
                button1_Click(sender, e);
                return;
            }

            if (e.Control && e.KeyCode == Keys.Space || e.Control && e.Shift)
            {
                
                /*
                getAutoCompleteList("admin");

                TablesAutoComplete.Visible = true;
                
                Point pos = textBox1.GetPositionFromCharIndex(textBox1.Text.Length - 1);
                TablesAutoComplete.Left = pos.X;
                TablesAutoComplete.Top = pos.Y + textBox1.Font.Height + 5;
                */

                string leftTxt = getlastSelectedWord(textBox1);
                
                    getAutoCompleteList(leftTxt);
                    if (TablesAutoComplete.Items.Count > 0)
                    {
                        TablesAutoComplete.Visible = true;
                        Point pos = textBox1.GetPositionFromCharIndex(textBox1.SelectionStart);
                        TablesAutoComplete.Left = pos.X;
                        TablesAutoComplete.Top = pos.Y + textBox1.Font.Height + 5;
                        
                    }
                


            }
            else
            {
                
                /*
                int selPos = textBox1.SelectionStart;
                string leftTxt = textBox1.Text.Substring(0,selPos);
                string[] allwords = leftTxt.Split(' ');
                leftTxt = allwords[allwords.Length-1];
                this.Text = selPos + " " + leftTxt;
                */
                string leftTxt = getlastSelectedWord(textBox1);
                if (leftTxt.Length > 2)
                {
                    getAutoCompleteList(leftTxt);
                    if (TablesAutoComplete.Items.Count > 0)
                    {
                        TablesAutoComplete.Visible = true;
                        
                        //Point pos = textBox1.GetPositionFromCharIndex(textBox1.Text.Length - 1);
                        Point pos = textBox1.GetPositionFromCharIndex(textBox1.SelectionStart);
                        TablesAutoComplete.Left = pos.X;
                        TablesAutoComplete.Top = pos.Y + textBox1.Font.Height + 5;
                        //if (!TablesAutoComplete.DroppedDown) TablesAutoComplete.Show();
                    }
                }
            }
        }

        private void TablesAutoComplete_SelectedIndexChanged(object sender, EventArgs e)
        {
            string leftTxt = getlastSelectedWord(textBox1);

            int selPos = textBox1.SelectionStart;
            int insertPos = selPos - leftTxt.Length;

            textBox1.Text = textBox1.Text.Remove(insertPos, leftTxt.Length);
            textBox1.Text = textBox1.Text.Insert(insertPos, TablesAutoComplete.Text);

            //textBox1.Text += TablesAutoComplete.Text;
            TablesAutoComplete.Text = "";
            TablesAutoComplete.Visible = false;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            TablesAutoComplete.Visible = false;
        }

        private void doAutoInsert(string newText)
        {
            int insertPos = AutoText.getSelectionStart();
            
            textBox1.Text = textBox1.Text.Remove(insertPos, AutoText.getSelectionLength());
            textBox1.Text = textBox1.Text.Insert(insertPos, newText);
            checkAndLoadTableFromString(newText);

            TablesAutoComplete.Text = "";
            TablesAutoComplete.Visible = false;
            parseSqlTextBox(); 
            textBox1.SelectionStart = insertPos + newText.Length;
        }

        private void insertAutoComplete()
        {   
            /*
            int insertPos = AutoText.getSelectionStart();

            textBox1.Text = textBox1.Text.Remove(insertPos, AutoText.getSelectionLength());
            textBox1.Text = textBox1.Text.Insert(insertPos, TablesAutoComplete.SelectedItems[0].Text);

            checkAndLoadTableFromString(TablesAutoComplete.SelectedItems[0].Text);

            TablesAutoComplete.Text = "";
            TablesAutoComplete.Visible = false;
            highlight.parse(textBox1);
             */
            doAutoInsert(TablesAutoComplete.SelectedItems[0].Text);
        }

        private void checkAndLoadTableFromString(string chkString)
        {
            for (int i = 0; i < tableView.Items.Count; i++)
            {
                if (chkString == tableView.Items[i].Text)
                {
                    List<MysqlStruct> sqlStrct = database.getAllFieldsStruct(chkString);
                    for (int z = 0; z < sqlStrct.Count; z++)
                    {
                        AutoText.addWord(sqlStrct[z].name);
                        highlight.addFieldName(sqlStrct[z].name);
                    }
                }
            }
        }

       

        private void TablesAutoComplete_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string leftTxt = getlastSelectedWord(textBox1);

            int selPos = textBox1.SelectionStart;
            int insertPos = selPos - leftTxt.Length;

            textBox1.Text = textBox1.Text.Remove(insertPos, leftTxt.Length);
            textBox1.Text = textBox1.Text.Insert(insertPos, TablesAutoComplete.Text);

            //textBox1.Text += TablesAutoComplete.Text;
            TablesAutoComplete.Text = "";
            TablesAutoComplete.Visible = false;
            parseSqlTextBox();
        }

        private void TablesAutoComplete_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            
        }

        private void TablesAutoComplete_ItemActivate(object sender, EventArgs e)
        {
            insertAutoComplete();
        }

        private void TablesAutoComplete_MouseClick(object sender, MouseEventArgs e)
        {
            //insertAutoComplete();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (trigger_Tables.Text != "" && trigger_name.Text != "" && trigger_timing.Text != "" && triggerCode.Text != "" && trigger_event.Text != "")
            {
                database.lastSqlErrorMessage = "";
                mysqlTrigger triggerHandler = new mysqlTrigger();
                triggerHandler.Name = trigger_name.Text;
                triggerHandler.sql = triggerCode.Text;
                triggerHandler.Timing = trigger_timing.Text;
                triggerHandler.Event = trigger_event.Text;
                triggerHandler.Table = trigger_Tables.Text;
                MessageBox.Show(triggerHandler.getCreateStatement());

                string dropSql = "DROP TRIGGER " + trigger_name.Text;
                if (database != null)
                {
                    Int64 dd = database.sql_update(dropSql);
                    //database.sql_update("DELIMITER |");
                    dd = database.sql_procedureUpdate(triggerHandler.getCreateStatement());

                    if (database.lastSqlErrorMessage != "")
                    {
                        MessageBox.Show(this, database.lastSqlErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        triggerCode.Text = triggerHandler.getCreateStatement();
                    }
                    //database.sql_update("DELIMITER ;");


                }

            }
        }

        private void autosortColumns()
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            columnAutoSizeMode++;
            if (columnAutoSizeMode > 1) columnAutoSizeMode = 0;
            autosortColumns();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            textBox1.Enabled = toolStripButton3.Checked;
            //if (textBox1.Enabled) splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel1Collapsed = !textBox1.Enabled;
        }

        private void reloadtoolBtn_Click(object sender, EventArgs e)
        {
            listTables();
            
            
        }

        private void procName_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateBookmarks()
        {
            string[] bList = bookMarks.listBookmarks();
            bookmarkList.Items.Clear();
            bookmarkList.Items.AddRange(bList);
        }


        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            UserTextInput uInput = new UserTextInput();
            uInput.groupBox.Text = "Name for Query";
            uInput.textinfo.Text = bookmarkList.Text;
            if (uInput.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string bookMarkName = uInput.textinfo.Text;
                int found = bookMarks.existsInIndex(bookMarkName);
                if (found > -1)
                {
                    if (MessageBox.Show(this, "Warning! a Bookmark with this name allready Exists. Overwrite ?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
                    {
                        bookMarks.addOrReplace(bookMarkName, textBox1.Text);
                    }
                }
                else
                {
                    bookMarks.addOrReplace(bookMarkName,textBox1.Text);
                }
                updateBookmarks();
                BookMarkSerializer serializer = new BookMarkSerializer();
                serializer.SerializeObject(bookMarks.getDefaultFilename(), bookMarks);
            }

        }

        private void bookmarkList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bookmarkList.Text != "")
            {
                QueryBookMark tmpMark = new QueryBookMark();
                tmpMark = bookMarks.getByName(bookmarkList.Text);
                if (tmpMark != null)
                {
                    textBox1.Text = tmpMark.query;
                    parseSqlTextBox(); 
                }
            }
        }

        private void equalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compareSetting.Text = "=";
        }

        private void differentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compareSetting.Text = "!=";
        }

        private void greaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compareSetting.Text = ">";
        }

        private void lowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compareSetting.Text = "<";
        }

        private void greaterEqualsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compareSetting.Text = ">=";
        }

        private void lowerEqualsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compareSetting.Text = "<=";
        }

        private void leftJoinTables_TextChanged(object sender, EventArgs e)
        {
            List<MysqlStruct> structs = database.getAllFieldsStruct(leftJoinTables.Text);
           
            leftJoinTarget.Items.Clear();

            for (int i = 0; i < structs.Count; i++)
            {

                leftJoinTarget.Items.Add(structs[i].name);

            }
            canStartJoin();
        }

        private bool canStartJoin()
        {
            bool startJoin = (leftJoinTables.Text != "" && leftJoinTarget.Text != "" && leftJoinSource.Text != "");
            leftJoinRunBtn.Enabled = startJoin;
            return startJoin;
        }


        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (canStartJoin())
            {
                //QueryComposer query = new QueryComposer(lastSelectedtable);
                maskQuery.setLimitRange(autoGetLimit);

                maskQuery.addJoinTable(leftJoinTables.Text, leftJoinSource.Text, leftJoinTarget.Text);

                textBox1.Text = maskQuery.getSelect();
                parseSqlTextBox();
                fireQuery();
            }
        }

        private void leftJoinSource_Click(object sender, EventArgs e)
        {

        }

        private void leftJoinSource_TextChanged(object sender, EventArgs e)
        {
            canStartJoin();
        }

        private void leftJoinTarget_TextChanged(object sender, EventArgs e)
        {
            canStartJoin();
        }

        private void textBox1_KeyUp_1(object sender, KeyEventArgs e)
        {
            // ---- start test -----
            //patternHandle.setText(textBox1.Text);
            // ---end test ---------

            if (e.Control && e.KeyCode == Keys.Return)
            {
                fireQuery();
            }

            if (TablesAutoComplete.Visible)
            {
                AutoText.setSelection(textBox1);
            }
            parseSqlTextBox();

            if (e.KeyCode == Keys.Escape)
            {
                TablesAutoComplete.Visible = false;
            }

        }

        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Space)
            {
                List<string> codes = AutoText.getListFromPart(textBox1);
                TablesAutoComplete.Items.Clear();
                bool show = false;
                bool doReplaceNow = false;
                for (int i = 0; i < codes.Count; i++)
                {
                    TablesAutoComplete.Items.Add(codes[i]);
                    if (i>1) show = true;
                    doReplaceNow = true;
                }
                if (show)
                {
                    Point pos = textBox1.GetPositionFromCharIndex(textBox1.SelectionStart);
                    TablesAutoComplete.Left = pos.X;
                    TablesAutoComplete.Top = pos.Y + textBox1.Font.Height + 5;
                    TablesAutoComplete.Visible = true;
                }
                else if (doReplaceNow)
                {
                    doAutoInsert(codes[0]);
                }
            }
        }

        private void TablesAutoComplete_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void TablesAutoComplete_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                TablesAutoComplete.Visible = false;
            }
        }

        private void queryBrowser_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                TablesAutoComplete.Visible = false;
            }
        }

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                GroupQuery grQuery = new GroupQuery();
                grQuery.richTextBox1.Text = textBox1.Text;
                if (sqlHighlighting.Checked) this.highlight.parse(grQuery.richTextBox1);
                grQuery.Show();
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            /*
            ListViewExport lvExport = new ListViewExport("Export");
            lvExport.assignListView(listView1);
            lvExport.GenerateDynamicExcelSheet();
             */
        }

        private void editMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {           
            Clipboard.SetData(DataFormats.Text, textBox1.SelectedText);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //textBox1.SelectedRtf = Clipboard.GetData(DataFormats.Rtf).ToString();
            Object clipData = Clipboard.GetData(DataFormats.Text);
            if (clipData != null)
            {
                textBox1.SelectedText = clipData.ToString();
                parseSqlTextBox(); 
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string result = "";
                string rowAdd = "";
               
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    string fieldAdd = "";
                    result = result + rowAdd;
                    for (int r = 0; r < listView1.SelectedItems[i].SubItems.Count; r++ )
                    {
                        result = result + fieldAdd + listView1.SelectedItems[i].SubItems[r].Text;
                        fieldAdd = "\t";
                    }
                    rowAdd = System.Environment.NewLine;
                }
                Clipboard.SetData(DataFormats.Text, result);
            }            
        }

        private void copyCellValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, cellvalue);
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            lastSelectedItemInfo = listView1.HitTest(e.X, e.Y);
            cellEditField.Visible = false;
            if (null != lastSelectedItemInfo && null != lastSelectedItemInfo.SubItem)
            {                
                cellvalue = lastSelectedItemInfo.SubItem.Text;

                if (cellvalue.Contains(System.Environment.NewLine) || cellvalue.Contains("\n"))
                {
                    //string[] rows = cellvalue.Split(System.Environment.NewLine.ToArray());
                    //int lineCount = rows.Length;
                    cellEditField.Height = 80;
                    cellEditField.AcceptsReturn = true;
                    cellEditField.Multiline = true;
                    cellEditField.ScrollBars = ScrollBars.Vertical;
                }
                else
                {
                    cellEditField.AcceptsReturn = false;
                    cellEditField.Multiline = false;
                    cellEditField.ScrollBars = ScrollBars.None;
                }
                
                cellEditField.Top = e.Y;
                cellEditField.Left = e.X;

                cellEditField.Top = lastSelectedItemInfo.SubItem.Bounds.Top;
                cellEditField.Left = lastSelectedItemInfo.SubItem.Bounds.Left;
                cellEditField.Width = lastSelectedItemInfo.SubItem.Bounds.Width;

                // editbox
                if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    editBox.Top = e.Y + 20;
                    editBox.Left = lastSelectedItemInfo.SubItem.Bounds.Left;
                    showEditBox();
                }
                else
                {
                    editBox.Visible = false;
                }
            }

        }

        private string getSelectedRowAsWhere()
        {
            return getSelectedRowAsWhere(!forceUpdateWithoutKeys,0);
        }

        private string getSelectedRowAsWhere(bool useKeys, int itemNumber)
        {
            bool useNullFields = false;

            MysqlKeys tableKeys = new MysqlKeys();

            

            if (listView1.SelectedItems.Count > 0)
            {
                string result = "";

                for (int i = 0; i < listView1.SelectedItems.Count && i <= itemNumber; i++)
                {
                    if (i == itemNumber)
                    {
                        string fieldAdd = " WHERE ";

                        for (int r = 0; r < listView1.SelectedItems[i].SubItems.Count; r++)
                        {
                            int ip = r + 1;

                            string fieldname = listView1.Columns[r].Text;
                            string[] part = fieldname.Split(' ');
                            string fieldValue = listView1.SelectedItems[i].SubItems[r].Text;

                            fieldValue = fieldValue.Replace("'", "\'");
                            if (useNullFields)
                            {
                                if (fieldValue == "null") fieldValue = " is null";
                                else fieldValue = " = '" + fieldValue + "'";
                            }
                            else
                            {
                                if (fieldValue == "null") fieldValue = "";
                                else fieldValue = " = '" + fieldValue + "'";
                            }

                            // if usekeys then ignore no unique keys
                            if (useKeys)
                            {
                                if (!tableKeys.initialized)
                                {
                                    tableKeys.init(lastSelectedtable, database);
                                }
                                if (!tableKeys.columnIsUnique(part[0])) fieldValue = "";
                            }

                            if (fieldValue != "")
                            {
                                result = result + fieldAdd + "`" +  part[0] + "`" + fieldValue;
                                fieldAdd = " AND ";
                            }
                        }
                        return result;
                    }
                    //Clipboard.SetData(DataFormats.Text, result);
                    
                }
            }
            return null;
        }

        private void updateCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cellEditField.Text = cellvalue;
            
            cellEditField.Visible = true;
            
        }

        private void cellEditField_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter && !cellEditField.AcceptsReturn) 
                || (e.KeyCode == Keys.Escape 
                    && cellEditField.AcceptsReturn 
                    && MessageBox.Show("Update Value ?","Confirm Update",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                cellEditField.Visible = false;
                string FieldName = lastSelectedItemInfo.SubItem.Name;
                string value = cellEditField.Text.Replace("'","\'");
                string updateSql = "UPDATE " + lastSelectedtable + " SET `" + FieldName + "` = '" + value + "'" + getSelectedRowAsWhere() + " LIMIT 1";
                string checkSql = "SELECT count(*) as chkcount FROM " + lastSelectedtable  + getSelectedRowAsWhere();

                List<string> result = database.selectAsList(checkSql,"chkcount");

                if (result.Count > 0 && result[0] == "1")
                {
                    database.sql_update(updateSql);

                    if (database.lastSqlErrorMessage != "")
                    {
                        MessageBox.Show( database.lastSqlErrorMessage ,"Error" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    fireQuery();
                }
                else
                {
                    MessageBox.Show( "Can not update Cell. Field is not unique. This means this Table hase no unique keys. Normaly is not possible to edit But you can use 'forse Update Cell' from Context menu to try a update without keys ","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }



        }

        private void forceUpdateCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Use this function for tables without unique keys only. But the table can only be updated if no duplicate entrys where found (this part will check this). Proceed ?", "Warning",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                forceUpdateWithoutKeys = true;
                updateCellToolStripMenuItem_Click(sender, e);
                forceUpdateWithoutKeys = false;
            }
        }

        private void addValueToWhereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastSelectedItemInfo != null && maskQuery != null && lastSelectedItemInfo.SubItem != null)
            {
                string value = lastSelectedItemInfo.SubItem.Text;
                string field = lastSelectedItemInfo.SubItem.Name;
                maskQuery.addWhere(field, value);
                findInputeElementbyFieldNameUpdate(field,value);
                textBox1.Text = maskQuery.getSelect();
                parseSqlTextBox();
            }
        }

        private void tableMenu_Opening(object sender, CancelEventArgs e)
        {
            addValueToWhereToolStripMenuItem.Enabled = (maskQuery != null && lastSelectedItemInfo != null && lastSelectedItemInfo.SubItem != null);
            forceUpdateCellToolStripMenuItem.Enabled = (lastSelectedItemInfo != null && lastSelectedItemInfo.SubItem != null);
            updateCellToolStripMenuItem.Enabled = (lastSelectedItemInfo != null && lastSelectedItemInfo.SubItem != null);
            Copy.Enabled = (lastSelectedItemInfo != null && lastSelectedItemInfo.SubItem != null);
            copyCellValueToolStripMenuItem.Enabled = Copy.Enabled = (lastSelectedItemInfo != null && lastSelectedItemInfo.SubItem != null);
        }

        private void parseSqlTextBox()
        {
            //if (sqlHighlighting.Checked) highlight.parse(textBox1);
            if (sqlHighlighting.Checked)
            {
                if (textBox1.Text.Length < 2000)
                {
                    highlight.useTimer = false;
                    ParsingTimer.Stop();
                    highlight.parse(textBox1);                    
                }
                else
                {
                    highlight.useTimer = true;
                    ParsingTimer.Stop();
                    ParsingTimer.Start();
                }
            }
        }

        private void ParsingTimer_Tick(object sender, EventArgs e)
        {
            highlight.parse(textBox1);
            ParsingTimer.Stop();
        }

        private void insertRowMenuItem_Click(object sender, EventArgs e)
        {
            TableStructForm tsf = new TableStructForm();
            bool closeCon = false;
            if (!database.isConnected())
            {
                database.connect();
                closeCon = true;
            }

            TableToForm tabFormHandler = new TableToForm(lastSelectedtable, database);
            tabFormHandler.show(tsf.controllBox);

            if (tsf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                QueryComposer tmpComp = tabFormHandler.getComposer();
                string sqlToInsert = tmpComp.getInsert();

                if (null != sqlToInsert)
                {
                    
                    database.sql_update(sqlToInsert);

                    string selSql = maskQuery.getSelect();
                    if (selSql != null)
                    {
                        textBox1.Text = selSql;
                        fireQuery();
                        if (sqlHighlighting.Checked) this.highlight.parse(textBox1);
                    }
                }

            }


            if (closeCon) database.disConnect();

        }


        private void showEditBox()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                TableStructForm tsf = new TableStructForm();
                bool closeCon = false;
                if (!database.isConnected())
                {
                    database.connect();
                    closeCon = true;
                }

                List<string> values = new List<string>();

                for (int i = 0; i < listView1.SelectedItems[0].SubItems.Count; i++)
                {
                    values.Add(listView1.SelectedItems[0].SubItems[i].Text);
                }

                TableToForm tabFormHandler = new TableToForm(lastSelectedtable, database,values);
                tabFormHandler.show(editBox);
                editBox.Visible = true;

                if (closeCon) database.disConnect();
            }
        }

        private void deleteSelectedRow(int itemNumber)
        {
            string where = getSelectedRowAsWhere(true,itemNumber);
            string checkSql = "SELECT count(*) as chkcount FROM " + lastSelectedtable + where;
            string deleteSql = "DELETE FROM " + lastSelectedtable + where;
            List<string> result = database.selectAsList(checkSql, "chkcount");

            if (result.Count > 0 && result[0] == "1")
            {
                database.sql_update(deleteSql);

                if (database.lastSqlErrorMessage != "")
                {
                    MessageBox.Show(database.lastSqlErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //fireQuery();
            }
            else
            {
                MessageBox.Show("Can not Delete Row." + checkSql, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DeleteRowMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you Sure? Deleting " + listView1.SelectedItems.Count + " rows From " + lastSelectedtable + "? This action can not be undone ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    rowResultCount.Text = "Deleting Row " + i + " / " + listView1.SelectedItems.Count.ToString();
                    Application.DoEvents();
                    deleteSelectedRow(i);
                    
                }
                string selSql = maskQuery.getSelect();
                if (selSql != null)
                {
                    textBox1.Text = selSql;
                    fireQuery();
                    if (sqlHighlighting.Checked) this.highlight.parse(textBox1);
                }

            }
        }

        private void csvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveCvsFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListViewWorker lw = new ListViewWorker();
                string csv = lw.exportCsv(listView1);

                System.IO.File.WriteAllText(saveCvsFile.FileName, csv);
            }

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void leftJoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            leftJoinToolStrip.Visible = leftJoinToolStripMenuItem.Checked;
        }

        private void rowEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rowOptTtoolStrip.Visible = rowEditToolStripMenuItem.Checked;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                autoGet = true;
            }
            else
            {
                autoGet = false;

            }
        }

        private void tableView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt && searchTableTextBox.Visible == false)
            {
                if (tableView.ShowGroups)
                {
                   // tableView.ShowGroups = false;
                    showGrouplabelInTableListToolStripMenuItem.Checked = false;
                }
                searchTableTextBox.Visible = true;
                
                searchTableTextBox.Focus();
                searchTableTextBox.Text = "" + (Char)e.KeyCode;
                searchTableTextBox.SelectionStart = 1;
            }

        }

        private void searchTableTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape && e.KeyCode != Keys.Enter)
            {
                ListViewWorker lw = new ListViewWorker();
                int select = lw.quickFind(searchTableTextBox.Text, tableView);
            }
            else
            {
                searchTableTextBox.Visible = false;
                //showGrouplabelInTableListToolStripMenuItem.Checked = true;
               // tableView.ShowGroups = showGrouplabelInTableListToolStripMenuItem.Checked;
            }
            
        }

        private void showGrouplabelInTableListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void showGrouplabelInTableListToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            tableView.ShowGroups = showGrouplabelInTableListToolStripMenuItem.Checked;
            if (tableView.SelectedItems.Count > 0)
            {
                tableView.TopItem = tableView.SelectedItems[0];
                tableView.TopItem.EnsureVisible();
            }
        }

        public void setSqlStatement(string sql)
        {
            this.textBox1.Text = sql;
            parseSqlTextBox();
        }

        public string getSqlStatement()
        {
            return textBox1.Text;
        }

        private void editSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            queryBrowser subQBrowser = new queryBrowser();
            subQBrowser.sensorProfil = this.sensorProfil;
            subQBrowser.loadPlaceHolder();
            subQBrowser.setSqlStatement(textBox1.SelectedText);
            subQBrowser.listTables();
            subQBrowser.enableDialogMode();
            subQBrowser.enableSqlView();

            subQBrowser.ShowDialog();

            if (subQBrowser.dlgResult) textBox1.SelectedText = subQBrowser.getSqlStatement();
        }

        private void DialogOKBtn_Click(object sender, EventArgs e)
        {
            

            if (subselectMode)
            {
                // first check on untestet query
                if (listView1.Items.Count < 1)
                {
                    if (MessageBox.Show("I have no Result to check. Are you Sure youre SQL Statement works as Subquery?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                        return;
                }

                // check on one row as result
                if (listView1.Items.Count > 0 && listView1.Columns.Count > 1)
                {
                    if (MessageBox.Show("You have more then one Columns in Result. Subquerys needs ONE Column as result. Click YES if you want Continue. Elsewhere click NO to go back an fix the Statement", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                        return;
                }
            }
            dlgResult = true;
            this.Close();
        }

        private void dialogCancelBtn_Click(object sender, EventArgs e)
        {
            dlgResult = false;
            this.Close();
        }

        
        private void createInsertSelectToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (lastSelectedtable != "")
            {
                CretaeInsertSelect inSelForm = new CretaeInsertSelect();
                inSelForm.setTables(sensorProfil, lastSelectedtable);

                if (inSelForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox1.Text = inSelForm.buildSql();
                    parseSqlTextBox();
                }
            }
            else
            {
                MessageBox.Show("Select a Table First","Need Table Selection",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private String getSelectedAsInsertStatement()
        {
            return getSelectedAsInsertStatement(false);
        }

        private String getSelectedAsInsertStatement(Boolean onDuplicateKeyInsert)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string result = "";
                string rowAdd = "";

                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    string fieldAdd = "";

                    result = result + rowAdd + "INSERT INTO " + lastSelectedtable + " SET ";
                    string fieldSets = "";
                    for (int r = 0; r < listView1.SelectedItems[i].SubItems.Count; r++)
                    {

                        string fieldname = listView1.Columns[r].Text;
                        string[] part = fieldname.Split(' ');
                        string fieldValue = listView1.SelectedItems[i].SubItems[r].Text;

                        fieldValue = fieldValue.Replace("'", "\'");

                        if (fieldValue == "null") fieldValue = null;
                        else fieldValue = " = '" + fieldValue + "'";

                        if (fieldValue != null)
                        {
                            fieldSets = fieldSets + fieldAdd + "`" + part[0] + "`" + fieldValue;
                            fieldAdd = ",";
                        }
                    }
                    rowAdd = ";" + System.Environment.NewLine;

                    if (onDuplicateKeyInsert)
                    {
                        fieldSets = fieldSets + " ON DUPLICATE KEY UPDATE " + fieldSets;
                    }
                    result = result + fieldSets;
                }

                

                return result;
            }
            return null;
        }


        private void copySelectedAsInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            string statement = getSelectedAsInsertStatement();
            if (null != statement) Clipboard.SetData(DataFormats.Text, statement);
        }

        private void explainStatusLabel_DoubleClick(object sender, EventArgs e)
        {
                rowResultCount.Text = "EXPLAIN QUERY...";
                MysqlExplain explain = new MysqlExplain(textBox1.Text, database);

                if (explain.useKeys())
                {
                    
                    explainStatusLabel.ForeColor = Color.DarkGreen;
                    explainStatusLabel.BackColor = Color.LightGreen;
                    explainStatusLabel.Text = "Using Keys ";
                    explainStatusLabel.ToolTipText = explain.getExplainReadableInfo();

                    toolTip1.SetToolTip(statusStrip1, explain.getExplainReadableInfo());
                    toolTip1.ToolTipTitle = "Explain Result";
                    toolTip1.IsBalloon = true;
                    toolTip1.AutoPopDelay = 10000;
                    toolTip1.ShowAlways = true;
                    toolTip1.ReshowDelay = 500;
                    

                }
                else
                {
                    explainStatusLabel.ForeColor = Color.DarkRed;
                    explainStatusLabel.BackColor = Color.LightCoral;
                    explainStatusLabel.Text = "No keys used " + explain.getNotification();
                    explainStatusLabel.ToolTipText = "this Query ist not optimized for using indizies";

                    toolTip1.SetToolTip(statusStrip1, "this Query ist not optimized for using indizies");
                   
                }
                rowResultCount.Text = "EXPLAIN QUERY...DONE";
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            
            if (bookmarkList.Text != "")
            {
                string bookMarkName = bookmarkList.Text;
                int found = bookMarks.existsInIndex(bookMarkName);
                if (found > -1 && MessageBox.Show("Removing Bookmark " + bookMarkName,"Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.Yes )
                {
                    if (bookMarks.removeBookMark(bookMarkName))
                    {
                        updateBookmarks();
                        BookMarkSerializer serializer = new BookMarkSerializer();
                        serializer.SerializeObject(bookMarks.getDefaultFilename(), bookMarks);
                    }
                }
                
               
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            string columnName = listView1.Columns[e.Column].Text;
            string[] splits = columnName.Split(' ');

            if (Control.ModifierKeys == Keys.Shift && textBox1.Enabled && splits.Length > 0)
            {
                textBox1.SelectedText = splits[0];
                parseSqlTextBox();
            }
            else if (Control.ModifierKeys == Keys.Alt && textBox1.Enabled && splits.Length > 0)
            {
                maskQuery.addUsedFieldNames(splits[0]);
                textBox1.Text = maskQuery.getSelect();
                parseSqlTextBox();
            }
            else
            {
                if (splits.Length > 0)
                {
                    maskQuery.autoHandleSingleOrder(splits[0]);
                    textBox1.Text = maskQuery.getSelect();
                    parseSqlTextBox();
                    button1_Click(sender, e);

                }
            }

        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            CreateStructForm CrForm = new CreateStructForm();

            if (CrForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String query = CrForm.sqlBox.Text;
                if (query.Length > 0)
                {
                    SqlExecConfirm confirmExec = new SqlExecConfirm();

                    confirmExec.sqlTextBox.Text = query;
                    if (confirmExec.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        
                        database = new MysqlHandler(sensorProfil);
                        database.connect();
                        database.sql_select(query);
                        database.disConnect();
                        if (database.lastSqlErrorMessage.Length > 0)
                        {
                            MessageBox.Show(database.lastSqlErrorMessage, "Mysql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            listTables();
                        }
                    }
                    
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void rememberAsPlaceholderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastSelectedItemInfo != null && maskQuery != null && lastSelectedItemInfo.SubItem != null)
            {
                string value = lastSelectedItemInfo.SubItem.Text;
                string field = lastSelectedItemInfo.SubItem.Name;

                bool saved = false;

                if (!placeHolder.exists(field))
                {
                    placeHolder.addPlaceHolder(field, value);
                    saved = true;
                }
                else
                {
                    if (MessageBox.Show("PlaceHolder for " + field + " allready Exists. Replace Content for Placeholder with value:" + value, "Allready used", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        placeHolder.update(field, value);
                        saved = true;
                    }
                }


                if (saved == true)
                {

                    PlaceHolderSerializer pSerializer = new PlaceHolderSerializer();                    
                    pSerializer.SerializeObject(pSerializer.getDefaultFilename(sensorProfil),placeHolder);
                   
                    MessageBox.Show("PlaceHolder for [" + field + "] created with value [" + value + "]", "Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            splitContainer3.Panel2Collapsed = !toolStripButton10.Checked;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (placeHolder.Count() > 0)
            {
                PlaceholderEditForm edPl = new PlaceholderEditForm();
                List<string> keyList = placeHolder.getKeys();
                edPl.keyList.Items.Clear();
                for (int i = 0; i < keyList.Count; i++)
                {
                    ListViewItem addRow = new ListViewItem();
                    addRow.Text = placeHolder.value(keyList[i]);
                    addRow.SubItems.Add(keyList[i]);
                    edPl.keyList.Items.Add(addRow);
                }

                if (edPl.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Boolean changed = false;
                    for (int i = 0; i < edPl.keyList.Items.Count; i++)
                    {
                        string key = edPl.keyList.Items[i].SubItems[1].Text;
                        string value = edPl.keyList.Items[i].Text;

                        if (placeHolder.value(key) != value)
                        {
                            changed = true;
                            placeHolder.update(key, value);
                        }

                        if (changed)
                        {
                            PlaceHolderSerializer pSerializer = new PlaceHolderSerializer();
                            pSerializer.SerializeObject(pSerializer.getDefaultFilename(sensorProfil), placeHolder);
                        }

                    }
                }


            }
            else
            {
                MessageBox.Show("No Placeholder currently defined.");
            }

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Resetting all Placeholder?", "Are yu sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                placeHolder.clear();
                PlaceHolderSerializer pSerializer = new PlaceHolderSerializer();
                if (System.IO.File.Exists(pSerializer.getDefaultFilename(sensorProfil)))
                {
                    System.IO.File.Delete(pSerializer.getDefaultFilename(sensorProfil));
                }
            }
        }

        private void copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string statement = getSelectedAsInsertStatement(true);
            if (null != statement) Clipboard.SetData(DataFormats.Text, statement);
        }

        private void tableContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        /**
         * executes an query in an new database connection and refreshs 
         * the table list
         */
        private Boolean execTableReleatedSql(string query)
        {
            database = new MysqlHandler(sensorProfil);
            database.connect();
            database.sql_select(query);
            database.disConnect();
            if (database.lastSqlErrorMessage.Length > 0)
            {
                MessageBox.Show(database.lastSqlErrorMessage, "Mysql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            listTables();
            return true;
            
        }


        private void copyTablessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tableView.SelectedItems.Count > 0)
            {
                UserTextInput prefixInput = new UserTextInput();
                prefixInput.groupBox.Text = "Set an Prefix";
                prefixInput.textinfo.Text = "copy_from_";
                if (prefixInput.ShowDialog() == DialogResult.OK)
                {
                    string sql = "";
                    for (int i = 0; i < tableView.SelectedItems.Count; i++)
                    {                       
                        sql += "CREATE TABLE " + prefixInput.textinfo.Text + tableView.SelectedItems[i].Text + " LIKE " + tableView.SelectedItems[i].Text + ";" + System.Environment.NewLine;
                        sql += "INSERT " + prefixInput.textinfo.Text + tableView.SelectedItems[i].Text + " SELECT * FROM " + tableView.SelectedItems[i].Text+ ";" + System.Environment.NewLine;
                    }

                    SqlExecConfirm copyConfirm = new SqlExecConfirm();
                    copyConfirm.sqlTextBox.Text = sql;
                    if (copyConfirm.ShowDialog() == DialogResult.OK)
                    {
                        this.execTableReleatedSql(sql);
                    }


                }
  
            }

        }

        private void dropTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tableView.SelectedItems.Count > 0)
            {
                string sql = "";
                for (int i = 0; i < tableView.SelectedItems.Count; i++)
                {
                    sql += "DROP TABLE " + tableView.SelectedItems[i].Text + ";" + System.Environment.NewLine;                    
                }

                SqlExecConfirm copyConfirm = new SqlExecConfirm();
                copyConfirm.sqlTextBox.Text = sql;
                if (copyConfirm.ShowDialog() == DialogResult.OK)
                {
                    this.execTableReleatedSql(sql);
                }
            }
        }

        private void selectUnion_Click(object sender, EventArgs e)
        {

            if (tableView.SelectedItems.Count > 0)
            {
                string sql = "";
                string union = "";
                for (int i = 0; i < tableView.SelectedItems.Count; i++)
                {
                    sql += union + "(SELECT * FROM  " + tableView.SelectedItems[i].Text + ")" + System.Environment.NewLine;
                    union = "UNION" + System.Environment.NewLine;
                }

                SqlExecConfirm copyConfirm = new SqlExecConfirm();
                copyConfirm.sqlTextBox.Text = sql;
                if (copyConfirm.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = sql;
                    fireQuery();
                    if (sqlHighlighting.Checked) this.highlight.parse(textBox1);
                }
            }
        }

        private void truncateTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tableView.SelectedItems.Count > 0)
            {
                string sql = "";
                for (int i = 0; i < tableView.SelectedItems.Count; i++)
                {
                    sql += "TRUNCATE TABLE " + tableView.SelectedItems[i].Text + ";" + System.Environment.NewLine;
                }

                SqlExecConfirm copyConfirm = new SqlExecConfirm();
                copyConfirm.sqlTextBox.Text = sql;
                if (copyConfirm.ShowDialog() == DialogResult.OK)
                {
                    this.execTableReleatedSql(sql);
                }
            }
        }

        private void renameTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tableView.SelectedItems.Count > 0)
            {
                string sql = "RENAME TABLE ";
                // RENAME TABLE tbl_name TO new_tbl_name
                UserTextInput prefixInput = new UserTextInput();
                string add = "";
                for (int i = 0; i < tableView.SelectedItems.Count; i++)
                {
                    prefixInput.groupBox.Text = "Rename Table " + tableView.SelectedItems[i].Text + " into ...";
                    prefixInput.textinfo.Text = tableView.SelectedItems[i].Text;
                    if (prefixInput.ShowDialog() == DialogResult.OK && prefixInput.textinfo.Text.Length > 3)
                    {
                        sql += add + " " + tableView.SelectedItems[i].Text + " TO " + prefixInput.textinfo.Text + System.Environment.NewLine;
                        add = ",";
                    }
                }

                SqlExecConfirm copyConfirm = new SqlExecConfirm();
                copyConfirm.sqlTextBox.Text = sql;
                if (copyConfirm.ShowDialog() == DialogResult.OK)
                {
                    this.execTableReleatedSql(sql);
                }
            }
        }
        

    }
}
