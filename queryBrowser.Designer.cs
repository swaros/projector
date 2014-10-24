namespace Projector
{
    partial class queryBrowser
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Fields", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Tables", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Syntax", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(queryBrowser));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.selectedTableLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.rowResultCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.explainStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.TablesAutoComplete = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.editMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.addToBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createInsertSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.editBox = new System.Windows.Forms.GroupBox();
            this.cellEditField = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tableMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.copyCellValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedAsInsertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.updateCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceUpdateCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.insertRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.addValueToWhereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.rememberAsPlaceholderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.DbColumnPics = new System.Windows.Forms.ImageList(this.components);
            this.MaskTab = new System.Windows.Forms.TabPage();
            this.maskBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.trigger_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trigger_timing = new System.Windows.Forms.ComboBox();
            this.trigger_event = new System.Windows.Forms.ComboBox();
            this.trigger_Tables = new System.Windows.Forms.ComboBox();
            this.triggerCode = new System.Windows.Forms.RichTextBox();
            this.tabPageProcedures = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.procSource = new System.Windows.Forms.RichTextBox();
            this.values = new System.Windows.Forms.GroupBox();
            this.procName = new System.Windows.Forms.TextBox();
            this.searchTableTextBox = new System.Windows.Forms.TextBox();
            this.tableView = new System.Windows.Forms.ListView();
            this.tableContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectUnion = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.dropTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyTablessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.truncateTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.createTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dbPic = new System.Windows.Forms.ImageList(this.components);
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.dialogToolStrip = new System.Windows.Forms.ToolStrip();
            this.DialogOKBtn = new System.Windows.Forms.ToolStripButton();
            this.dialogCancelBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TableNameView = new System.Windows.Forms.ToolStripLabel();
            this.reloadtoolBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.bookmarkList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.sqlHighlighting = new System.Windows.Forms.ToolStripButton();
            this.placeholderReplace = new System.Windows.Forms.ToolStripButton();
            this.explainEnabled = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.csvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.leftJoinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rowEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.showGrouplabelInTableListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.leftJoinToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.leftJoinTables = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.leftJoinSource = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.compareSetting = new System.Windows.Forms.ToolStripDropDownButton();
            this.equalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.differentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greaterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greaterEqualsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowerEqualsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.leftJoinTarget = new System.Windows.Forms.ToolStripComboBox();
            this.leftJoinRunBtn = new System.Windows.Forms.ToolStripButton();
            this.rowOptTtoolStrip = new System.Windows.Forms.ToolStrip();
            this.rowOptions = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.mysqlWorker = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ParsingTimer = new System.Windows.Forms.Timer(this.components);
            this.saveCvsFile = new System.Windows.Forms.SaveFileDialog();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinTryMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.RightToolStripPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.editMenu.SuspendLayout();
            this.tableMenu.SuspendLayout();
            this.MaskTab.SuspendLayout();
            this.maskBox.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.tabPageProcedures.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.values.SuspendLayout();
            this.tableContextMenu.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.dialogToolStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.leftJoinToolStrip.SuspendLayout();
            this.rowOptTtoolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedTableLabel,
            this.rowResultCount,
            this.explainStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1116, 24);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // selectedTableLabel
            // 
            this.selectedTableLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.selectedTableLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.selectedTableLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.selectedTableLabel.Name = "selectedTableLabel";
            this.selectedTableLabel.Size = new System.Drawing.Size(20, 19);
            this.selectedTableLabel.Text = "...";
            // 
            // rowResultCount
            // 
            this.rowResultCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.rowResultCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.rowResultCount.Name = "rowResultCount";
            this.rowResultCount.Size = new System.Drawing.Size(54, 19);
            this.rowResultCount.Text = "No Data";
            // 
            // explainStatusLabel
            // 
            this.explainStatusLabel.AutoToolTip = true;
            this.explainStatusLabel.DoubleClickEnabled = true;
            this.explainStatusLabel.Name = "explainStatusLabel";
            this.explainStatusLabel.Size = new System.Drawing.Size(44, 19);
            this.explainStatusLabel.Text = "Explain";
            this.explainStatusLabel.DoubleClick += new System.EventHandler(this.explainStatusLabel_DoubleClick);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer3);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStripContainer2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1052, 478);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            this.toolStripContainer1.RightToolStripPanel.Controls.Add(this.dialogToolStrip);
            this.toolStripContainer1.Size = new System.Drawing.Size(1116, 527);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.leftJoinToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.rowOptTtoolStrip);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.searchTableTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.tableView);
            this.splitContainer3.Size = new System.Drawing.Size(1052, 478);
            this.splitContainer3.SplitterDistance = 817;
            this.splitContainer3.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.MaskTab);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPageProcedures);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(817, 478);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(809, 452);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DB";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.editBox);
            this.splitContainer1.Panel2.Controls.Add(this.cellEditField);
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(803, 446);
            this.splitContainer1.SplitterDistance = 124;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.TablesAutoComplete);
            this.splitContainer2.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Size = new System.Drawing.Size(803, 124);
            this.splitContainer2.SplitterDistance = 719;
            this.splitContainer2.TabIndex = 2;
            // 
            // TablesAutoComplete
            // 
            this.TablesAutoComplete.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.TablesAutoComplete.BackColor = System.Drawing.SystemColors.Info;
            this.TablesAutoComplete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TablesAutoComplete.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.TablesAutoComplete.FullRowSelect = true;
            listViewGroup1.Header = "Fields";
            listViewGroup1.Name = "Fields";
            listViewGroup2.Header = "Tables";
            listViewGroup2.Name = "Tables";
            listViewGroup3.Header = "Syntax";
            listViewGroup3.Name = "Syntax";
            this.TablesAutoComplete.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.TablesAutoComplete.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.TablesAutoComplete.Location = new System.Drawing.Point(276, 12);
            this.TablesAutoComplete.MultiSelect = false;
            this.TablesAutoComplete.Name = "TablesAutoComplete";
            this.TablesAutoComplete.ShowGroups = false;
            this.TablesAutoComplete.ShowItemToolTips = true;
            this.TablesAutoComplete.Size = new System.Drawing.Size(218, 97);
            this.TablesAutoComplete.TabIndex = 1;
            this.TablesAutoComplete.UseCompatibleStateImageBehavior = false;
            this.TablesAutoComplete.View = System.Windows.Forms.View.Details;
            this.TablesAutoComplete.ItemActivate += new System.EventHandler(this.TablesAutoComplete_ItemActivate);
            this.TablesAutoComplete.SelectedIndexChanged += new System.EventHandler(this.TablesAutoComplete_SelectedIndexChanged_2);
            this.TablesAutoComplete.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TablesAutoComplete_KeyPress);
            this.TablesAutoComplete.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TablesAutoComplete_KeyUp);
            this.TablesAutoComplete.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TablesAutoComplete_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Fields";
            this.columnHeader1.Width = 200;
            // 
            // textBox1
            // 
            this.textBox1.ContextMenuStrip = this.editMenu;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(719, 124);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "";
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown_1);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp_1);
            // 
            // editMenu
            // 
            this.editMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripSeparator9,
            this.addToBookmarkToolStripMenuItem,
            this.runQueryToolStripMenuItem,
            this.editSelectedToolStripMenuItem,
            this.createInsertSelectToolStripMenuItem});
            this.editMenu.Name = "editMenu";
            this.editMenu.Size = new System.Drawing.Size(187, 142);
            this.editMenu.Opening += new System.ComponentModel.CancelEventHandler(this.editMenu_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem1.Text = "&Copy";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = global::Projector.Properties.Resources.clipboard_paste_document_text;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem2.Text = "&Paste";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(183, 6);
            // 
            // addToBookmarkToolStripMenuItem
            // 
            this.addToBookmarkToolStripMenuItem.Image = global::Projector.Properties.Resources.bookmark_book_open;
            this.addToBookmarkToolStripMenuItem.Name = "addToBookmarkToolStripMenuItem";
            this.addToBookmarkToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.addToBookmarkToolStripMenuItem.Text = "Add to Bookmarks...";
            this.addToBookmarkToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // runQueryToolStripMenuItem
            // 
            this.runQueryToolStripMenuItem.Image = global::Projector.Properties.Resources.applications_161;
            this.runQueryToolStripMenuItem.Name = "runQueryToolStripMenuItem";
            this.runQueryToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.runQueryToolStripMenuItem.Text = "Run Query";
            this.runQueryToolStripMenuItem.Click += new System.EventHandler(this.button1_Click);
            // 
            // editSelectedToolStripMenuItem
            // 
            this.editSelectedToolStripMenuItem.Name = "editSelectedToolStripMenuItem";
            this.editSelectedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.editSelectedToolStripMenuItem.Text = "Edit Selected";
            this.editSelectedToolStripMenuItem.Click += new System.EventHandler(this.editSelectedToolStripMenuItem_Click);
            // 
            // createInsertSelectToolStripMenuItem
            // 
            this.createInsertSelectToolStripMenuItem.Name = "createInsertSelectToolStripMenuItem";
            this.createInsertSelectToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.createInsertSelectToolStripMenuItem.Text = "Create Insert Select ...";
            this.createInsertSelectToolStripMenuItem.Click += new System.EventHandler(this.createInsertSelectToolStripMenuItem_Click_1);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::Projector.Properties.Resources.applications_32;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 41);
            this.button1.TabIndex = 1;
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // editBox
            // 
            this.editBox.AutoSize = true;
            this.editBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.editBox.Location = new System.Drawing.Point(757, 39);
            this.editBox.Name = "editBox";
            this.editBox.Size = new System.Drawing.Size(6, 5);
            this.editBox.TabIndex = 2;
            this.editBox.TabStop = false;
            this.editBox.Text = "Edit Values";
            // 
            // cellEditField
            // 
            this.cellEditField.BackColor = System.Drawing.Color.NavajoWhite;
            this.cellEditField.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cellEditField.Location = new System.Drawing.Point(20, 29);
            this.cellEditField.Name = "cellEditField";
            this.cellEditField.Size = new System.Drawing.Size(160, 20);
            this.cellEditField.TabIndex = 1;
            this.cellEditField.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cellEditField_KeyUp);
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.tableMenu;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(803, 318);
            this.listView1.SmallImageList = this.DbColumnPics;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            // 
            // tableMenu
            // 
            this.tableMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Copy,
            this.copyCellValueToolStripMenuItem,
            this.copySelectedAsInsertToolStripMenuItem,
            this.copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem,
            this.toolStripSeparator10,
            this.updateCellToolStripMenuItem,
            this.forceUpdateCellToolStripMenuItem,
            this.toolStripSeparator11,
            this.insertRowMenuItem,
            this.toolStripSeparator13,
            this.DeleteRowMenuItem,
            this.toolStripSeparator12,
            this.addValueToWhereToolStripMenuItem,
            this.toolStripSeparator17,
            this.rememberAsPlaceholderToolStripMenuItem,
            this.toolStripMenuItem3});
            this.tableMenu.Name = "tableMenu";
            this.tableMenu.Size = new System.Drawing.Size(317, 276);
            this.tableMenu.Opening += new System.ComponentModel.CancelEventHandler(this.tableMenu_Opening);
            // 
            // Copy
            // 
            this.Copy.Image = global::Projector.Properties.Resources.clipboard_paste_document_text;
            this.Copy.Name = "Copy";
            this.Copy.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.Copy.Size = new System.Drawing.Size(316, 22);
            this.Copy.Text = "Copy Row";
            this.Copy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // copyCellValueToolStripMenuItem
            // 
            this.copyCellValueToolStripMenuItem.Name = "copyCellValueToolStripMenuItem";
            this.copyCellValueToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyCellValueToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.copyCellValueToolStripMenuItem.Text = "Copy Cell Value";
            this.copyCellValueToolStripMenuItem.Click += new System.EventHandler(this.copyCellValueToolStripMenuItem_Click);
            // 
            // copySelectedAsInsertToolStripMenuItem
            // 
            this.copySelectedAsInsertToolStripMenuItem.Name = "copySelectedAsInsertToolStripMenuItem";
            this.copySelectedAsInsertToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.copySelectedAsInsertToolStripMenuItem.Text = "Copy Selected as Insert";
            this.copySelectedAsInsertToolStripMenuItem.Click += new System.EventHandler(this.copySelectedAsInsertToolStripMenuItem_Click);
            // 
            // copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem
            // 
            this.copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem.Name = "copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem";
            this.copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem.Text = "Copy Selected as Insert (duplicate key update)";
            this.copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem.Click += new System.EventHandler(this.copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(313, 6);
            // 
            // updateCellToolStripMenuItem
            // 
            this.updateCellToolStripMenuItem.Image = global::Projector.Properties.Resources.database_table;
            this.updateCellToolStripMenuItem.Name = "updateCellToolStripMenuItem";
            this.updateCellToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.updateCellToolStripMenuItem.Text = "Update Cell...";
            this.updateCellToolStripMenuItem.Click += new System.EventHandler(this.updateCellToolStripMenuItem_Click);
            // 
            // forceUpdateCellToolStripMenuItem
            // 
            this.forceUpdateCellToolStripMenuItem.Name = "forceUpdateCellToolStripMenuItem";
            this.forceUpdateCellToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.forceUpdateCellToolStripMenuItem.Text = "Force Update Cell ...";
            this.forceUpdateCellToolStripMenuItem.Click += new System.EventHandler(this.forceUpdateCellToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(313, 6);
            // 
            // insertRowMenuItem
            // 
            this.insertRowMenuItem.Image = global::Projector.Properties.Resources.add_16;
            this.insertRowMenuItem.Name = "insertRowMenuItem";
            this.insertRowMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.insertRowMenuItem.Size = new System.Drawing.Size(316, 22);
            this.insertRowMenuItem.Text = "Insert Row ...";
            this.insertRowMenuItem.Click += new System.EventHandler(this.insertRowMenuItem_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(313, 6);
            // 
            // DeleteRowMenuItem
            // 
            this.DeleteRowMenuItem.Image = global::Projector.Properties.Resources.delete;
            this.DeleteRowMenuItem.Name = "DeleteRowMenuItem";
            this.DeleteRowMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.DeleteRowMenuItem.Size = new System.Drawing.Size(316, 22);
            this.DeleteRowMenuItem.Text = "Delete selected Rows";
            this.DeleteRowMenuItem.Click += new System.EventHandler(this.DeleteRowMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(313, 6);
            // 
            // addValueToWhereToolStripMenuItem
            // 
            this.addValueToWhereToolStripMenuItem.Name = "addValueToWhereToolStripMenuItem";
            this.addValueToWhereToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.addValueToWhereToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.addValueToWhereToolStripMenuItem.Text = "Add value to Where";
            this.addValueToWhereToolStripMenuItem.Click += new System.EventHandler(this.addValueToWhereToolStripMenuItem_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(313, 6);
            // 
            // rememberAsPlaceholderToolStripMenuItem
            // 
            this.rememberAsPlaceholderToolStripMenuItem.Name = "rememberAsPlaceholderToolStripMenuItem";
            this.rememberAsPlaceholderToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.rememberAsPlaceholderToolStripMenuItem.Text = "Remember as Placeholder";
            this.rememberAsPlaceholderToolStripMenuItem.Click += new System.EventHandler(this.rememberAsPlaceholderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(316, 22);
            this.toolStripMenuItem3.Text = "Placeholder";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenuItem4.Text = "Show / Edit Placeholders";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(204, 22);
            this.toolStripMenuItem5.Text = "Reset All Placeholders";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // DbColumnPics
            // 
            this.DbColumnPics.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DbColumnPics.ImageStream")));
            this.DbColumnPics.TransparentColor = System.Drawing.Color.Transparent;
            this.DbColumnPics.Images.SetKeyName(0, "1260371515_database_table_16x16.gif");
            this.DbColumnPics.Images.SetKeyName(1, "1260371448_stock_data-tables.png");
            this.DbColumnPics.Images.SetKeyName(2, "1260371569_key.png");
            this.DbColumnPics.Images.SetKeyName(3, "1279103131_padlock_closed.png");
            // 
            // MaskTab
            // 
            this.MaskTab.AutoScroll = true;
            this.MaskTab.Controls.Add(this.maskBox);
            this.MaskTab.Location = new System.Drawing.Point(4, 22);
            this.MaskTab.Name = "MaskTab";
            this.MaskTab.Padding = new System.Windows.Forms.Padding(3);
            this.MaskTab.Size = new System.Drawing.Size(809, 452);
            this.MaskTab.TabIndex = 2;
            this.MaskTab.Text = "Search";
            this.MaskTab.UseVisualStyleBackColor = true;
            // 
            // maskBox
            // 
            this.maskBox.AutoSize = true;
            this.maskBox.Controls.Add(this.label5);
            this.maskBox.Controls.Add(this.panel1);
            this.maskBox.Location = new System.Drawing.Point(8, 6);
            this.maskBox.Name = "maskBox";
            this.maskBox.Size = new System.Drawing.Size(647, 193);
            this.maskBox.TabIndex = 0;
            this.maskBox.TabStop = false;
            this.maskBox.Text = "Search";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(191, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(450, 25);
            this.label5.TabIndex = 2;
            this.label5.Text = "Select Table ----------------------------------------- ->";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Projector.Properties.Resources.bigsql;
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(132, 125);
            this.panel1.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(809, 452);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Trigger";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.button2);
            this.splitContainer4.Panel1.Controls.Add(this.label4);
            this.splitContainer4.Panel1.Controls.Add(this.trigger_name);
            this.splitContainer4.Panel1.Controls.Add(this.label3);
            this.splitContainer4.Panel1.Controls.Add(this.label2);
            this.splitContainer4.Panel1.Controls.Add(this.label1);
            this.splitContainer4.Panel1.Controls.Add(this.trigger_timing);
            this.splitContainer4.Panel1.Controls.Add(this.trigger_event);
            this.splitContainer4.Panel1.Controls.Add(this.trigger_Tables);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.triggerCode);
            this.splitContainer4.Size = new System.Drawing.Size(803, 446);
            this.splitContainer4.SplitterDistance = 139;
            this.splitContainer4.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(278, 97);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Trigger";
            // 
            // trigger_name
            // 
            this.trigger_name.AcceptsReturn = true;
            this.trigger_name.Location = new System.Drawing.Point(103, 17);
            this.trigger_name.Name = "trigger_name";
            this.trigger_name.Size = new System.Drawing.Size(153, 20);
            this.trigger_name.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Timing";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Event";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Affected Table";
            // 
            // trigger_timing
            // 
            this.trigger_timing.FormattingEnabled = true;
            this.trigger_timing.Location = new System.Drawing.Point(103, 99);
            this.trigger_timing.Name = "trigger_timing";
            this.trigger_timing.Size = new System.Drawing.Size(153, 21);
            this.trigger_timing.TabIndex = 2;
            // 
            // trigger_event
            // 
            this.trigger_event.FormattingEnabled = true;
            this.trigger_event.Items.AddRange(new object[] {
            "INSERT",
            "UPDATE"});
            this.trigger_event.Location = new System.Drawing.Point(103, 70);
            this.trigger_event.Name = "trigger_event";
            this.trigger_event.Size = new System.Drawing.Size(153, 21);
            this.trigger_event.TabIndex = 1;
            // 
            // trigger_Tables
            // 
            this.trigger_Tables.FormattingEnabled = true;
            this.trigger_Tables.Location = new System.Drawing.Point(103, 43);
            this.trigger_Tables.Name = "trigger_Tables";
            this.trigger_Tables.Size = new System.Drawing.Size(153, 21);
            this.trigger_Tables.TabIndex = 0;
            // 
            // triggerCode
            // 
            this.triggerCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerCode.Location = new System.Drawing.Point(0, 0);
            this.triggerCode.Name = "triggerCode";
            this.triggerCode.Size = new System.Drawing.Size(803, 303);
            this.triggerCode.TabIndex = 0;
            this.triggerCode.Text = "";
            // 
            // tabPageProcedures
            // 
            this.tabPageProcedures.Controls.Add(this.groupBox1);
            this.tabPageProcedures.Controls.Add(this.values);
            this.tabPageProcedures.Location = new System.Drawing.Point(4, 22);
            this.tabPageProcedures.Name = "tabPageProcedures";
            this.tabPageProcedures.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProcedures.Size = new System.Drawing.Size(809, 452);
            this.tabPageProcedures.TabIndex = 3;
            this.tabPageProcedures.Text = "Stored Procedures";
            this.tabPageProcedures.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.procSource);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(803, 387);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source";
            // 
            // procSource
            // 
            this.procSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.procSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.procSource.Location = new System.Drawing.Point(3, 16);
            this.procSource.Name = "procSource";
            this.procSource.Size = new System.Drawing.Size(797, 368);
            this.procSource.TabIndex = 0;
            this.procSource.Text = "";
            // 
            // values
            // 
            this.values.Controls.Add(this.procName);
            this.values.Dock = System.Windows.Forms.DockStyle.Top;
            this.values.Location = new System.Drawing.Point(3, 3);
            this.values.Name = "values";
            this.values.Size = new System.Drawing.Size(803, 59);
            this.values.TabIndex = 0;
            this.values.TabStop = false;
            this.values.Text = "Properies";
            // 
            // procName
            // 
            this.procName.Location = new System.Drawing.Point(7, 20);
            this.procName.Name = "procName";
            this.procName.Size = new System.Drawing.Size(154, 20);
            this.procName.TabIndex = 0;
            this.procName.TextChanged += new System.EventHandler(this.procName_TextChanged);
            // 
            // searchTableTextBox
            // 
            this.searchTableTextBox.BackColor = System.Drawing.Color.Yellow;
            this.searchTableTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.searchTableTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.searchTableTextBox.Location = new System.Drawing.Point(0, 458);
            this.searchTableTextBox.Name = "searchTableTextBox";
            this.searchTableTextBox.Size = new System.Drawing.Size(231, 20);
            this.searchTableTextBox.TabIndex = 1;
            this.searchTableTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTableTextBox_KeyUp);
            // 
            // tableView
            // 
            this.tableView.ContextMenuStrip = this.tableContextMenu;
            this.tableView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableView.FullRowSelect = true;
            this.tableView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.tableView.HideSelection = false;
            this.tableView.LabelWrap = false;
            this.tableView.Location = new System.Drawing.Point(0, 0);
            this.tableView.Name = "tableView";
            this.tableView.Size = new System.Drawing.Size(231, 478);
            this.tableView.SmallImageList = this.dbPic;
            this.tableView.TabIndex = 0;
            this.tableView.UseCompatibleStateImageBehavior = false;
            this.tableView.View = System.Windows.Forms.View.Details;
            this.tableView.ItemActivate += new System.EventHandler(this.tableView_ItemActivate);
            this.tableView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.tableView_ItemSelectionChanged);
            this.tableView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tableView_KeyUp);
            // 
            // tableContextMenu
            // 
            this.tableContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectUnion,
            this.toolStripSeparator18,
            this.dropTablesToolStripMenuItem,
            this.copyTablessToolStripMenuItem,
            this.truncateTablesToolStripMenuItem,
            this.toolStripSeparator19,
            this.createTableToolStripMenuItem,
            this.renameTableToolStripMenuItem,
            this.joinTryMenu});
            this.tableContextMenu.Name = "tableContextMenu";
            this.tableContextMenu.Size = new System.Drawing.Size(167, 192);
            this.tableContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.tableContextMenu_Opening);
            // 
            // selectUnion
            // 
            this.selectUnion.Name = "selectUnion";
            this.selectUnion.Size = new System.Drawing.Size(166, 22);
            this.selectUnion.Text = "Select Union";
            this.selectUnion.Click += new System.EventHandler(this.selectUnion_Click);
            // 
            // toolStripSeparator18
            // 
            this.toolStripSeparator18.Name = "toolStripSeparator18";
            this.toolStripSeparator18.Size = new System.Drawing.Size(163, 6);
            // 
            // dropTablesToolStripMenuItem
            // 
            this.dropTablesToolStripMenuItem.Image = global::Projector.Properties.Resources.TRASH_16;
            this.dropTablesToolStripMenuItem.Name = "dropTablesToolStripMenuItem";
            this.dropTablesToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.dropTablesToolStripMenuItem.Text = "Drop Table(s)";
            this.dropTablesToolStripMenuItem.Click += new System.EventHandler(this.dropTablesToolStripMenuItem_Click);
            // 
            // copyTablessToolStripMenuItem
            // 
            this.copyTablessToolStripMenuItem.Name = "copyTablessToolStripMenuItem";
            this.copyTablessToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.copyTablessToolStripMenuItem.Text = "Copy Tables(s)";
            this.copyTablessToolStripMenuItem.Click += new System.EventHandler(this.copyTablessToolStripMenuItem_Click);
            // 
            // truncateTablesToolStripMenuItem
            // 
            this.truncateTablesToolStripMenuItem.Name = "truncateTablesToolStripMenuItem";
            this.truncateTablesToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.truncateTablesToolStripMenuItem.Text = "Truncate Table(s)";
            this.truncateTablesToolStripMenuItem.Click += new System.EventHandler(this.truncateTablesToolStripMenuItem_Click);
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            this.toolStripSeparator19.Size = new System.Drawing.Size(163, 6);
            // 
            // createTableToolStripMenuItem
            // 
            this.createTableToolStripMenuItem.Name = "createTableToolStripMenuItem";
            this.createTableToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.createTableToolStripMenuItem.Text = "Create Table...";
            this.createTableToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton9_Click);
            // 
            // renameTableToolStripMenuItem
            // 
            this.renameTableToolStripMenuItem.Name = "renameTableToolStripMenuItem";
            this.renameTableToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.renameTableToolStripMenuItem.Text = "Rename Table";
            this.renameTableToolStripMenuItem.Click += new System.EventHandler(this.renameTableToolStripMenuItem_Click);
            // 
            // dbPic
            // 
            this.dbPic.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("dbPic.ImageStream")));
            this.dbPic.TransparentColor = System.Drawing.Color.Transparent;
            this.dbPic.Images.SetKeyName(0, "stock_connect-to-url.png");
            this.dbPic.Images.SetKeyName(1, "database_table.png");
            this.dbPic.Images.SetKeyName(2, "database_add.png");
            this.dbPic.Images.SetKeyName(3, "applications_16.png");
            this.dbPic.Images.SetKeyName(4, "database_table.png");
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.AutoScroll = true;
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(1052, 453);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(1052, 478);
            this.toolStripContainer2.TabIndex = 2;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // dialogToolStrip
            // 
            this.dialogToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.dialogToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DialogOKBtn,
            this.dialogCancelBtn});
            this.dialogToolStrip.Location = new System.Drawing.Point(0, 5);
            this.dialogToolStrip.Name = "dialogToolStrip";
            this.dialogToolStrip.Size = new System.Drawing.Size(64, 57);
            this.dialogToolStrip.TabIndex = 5;
            // 
            // DialogOKBtn
            // 
            this.DialogOKBtn.Image = global::Projector.Properties.Resources.stock_dataeditsqlquery;
            this.DialogOKBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DialogOKBtn.Name = "DialogOKBtn";
            this.DialogOKBtn.Size = new System.Drawing.Size(62, 20);
            this.DialogOKBtn.Text = "OK";
            this.DialogOKBtn.Click += new System.EventHandler(this.DialogOKBtn_Click);
            // 
            // dialogCancelBtn
            // 
            this.dialogCancelBtn.Image = global::Projector.Properties.Resources.delete_16;
            this.dialogCancelBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dialogCancelBtn.Name = "dialogCancelBtn";
            this.dialogCancelBtn.Size = new System.Drawing.Size(62, 20);
            this.dialogCancelBtn.Text = "Cancel";
            this.dialogCancelBtn.Click += new System.EventHandler(this.dialogCancelBtn_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TableNameView,
            this.reloadtoolBtn,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.toolStripButton3,
            this.toolStripSeparator3,
            this.toolStripButton4,
            this.bookmarkList,
            this.toolStripButton8,
            this.toolStripSeparator8,
            this.toolStripButton5,
            this.sqlHighlighting,
            this.placeholderReplace,
            this.explainEnabled,
            this.toolStripButton10,
            this.toolStripSeparator15,
            this.toolStripSplitButton1,
            this.toolStripDropDownButton1,
            this.toolStripSeparator16,
            this.toolStripButton9});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(544, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TableNameView
            // 
            this.TableNameView.Name = "TableNameView";
            this.TableNameView.Size = new System.Drawing.Size(36, 22);
            this.TableNameView.Text = "None";
            // 
            // reloadtoolBtn
            // 
            this.reloadtoolBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.reloadtoolBtn.Image = global::Projector.Properties.Resources.sql_execute;
            this.reloadtoolBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reloadtoolBtn.Name = "reloadtoolBtn";
            this.reloadtoolBtn.Size = new System.Drawing.Size(23, 22);
            this.reloadtoolBtn.Text = "Reload Tables";
            this.reloadtoolBtn.Click += new System.EventHandler(this.reloadtoolBtn_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Projector.Properties.Resources.reload3;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Run Query";
            this.toolStripButton1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::Projector.Properties.Resources.resizecol;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Auto Resize Columns";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.CheckOnClick = true;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::Projector.Properties.Resources.sql_editor;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Enable SQL Editor";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::Projector.Properties.Resources.bookmark_book_open;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "save Query";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // bookmarkList
            // 
            this.bookmarkList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bookmarkList.DropDownWidth = 300;
            this.bookmarkList.Name = "bookmarkList";
            this.bookmarkList.Size = new System.Drawing.Size(121, 25);
            this.bookmarkList.SelectedIndexChanged += new System.EventHandler(this.bookmarkList_SelectedIndexChanged);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = global::Projector.Properties.Resources.delete;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton8.Text = "Remove selected Bookmark...";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::Projector.Properties.Resources.servers_network;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "Open in Group Query ";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click_1);
            // 
            // sqlHighlighting
            // 
            this.sqlHighlighting.Checked = true;
            this.sqlHighlighting.CheckOnClick = true;
            this.sqlHighlighting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sqlHighlighting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sqlHighlighting.Image = global::Projector.Properties.Resources.highlighter_text;
            this.sqlHighlighting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sqlHighlighting.Name = "sqlHighlighting";
            this.sqlHighlighting.Size = new System.Drawing.Size(23, 22);
            this.sqlHighlighting.Text = "Syntax Highlighting";
            // 
            // placeholderReplace
            // 
            this.placeholderReplace.Checked = true;
            this.placeholderReplace.CheckOnClick = true;
            this.placeholderReplace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.placeholderReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.placeholderReplace.Image = global::Projector.Properties.Resources.edit_replace;
            this.placeholderReplace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.placeholderReplace.Name = "placeholderReplace";
            this.placeholderReplace.Size = new System.Drawing.Size(23, 22);
            this.placeholderReplace.Text = "Replace Placeholder";
            // 
            // explainEnabled
            // 
            this.explainEnabled.Checked = true;
            this.explainEnabled.CheckOnClick = true;
            this.explainEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.explainEnabled.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.explainEnabled.Image = global::Projector.Properties.Resources.explain;
            this.explainEnabled.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.explainEnabled.Name = "explainEnabled";
            this.explainEnabled.Size = new System.Drawing.Size(23, 22);
            this.explainEnabled.Text = "Explain";
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.Checked = true;
            this.toolStripButton10.CheckOnClick = true;
            this.toolStripButton10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::Projector.Properties.Resources.ui_list_box_blue;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton10.Text = "Show Tables";
            this.toolStripButton10.Click += new System.EventHandler(this.toolStripButton10_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.csvToolStripMenuItem,
            this.excelToolStripMenuItem});
            this.toolStripSplitButton1.Image = global::Projector.Properties.Resources.SAVE_16;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButton1.Text = "Export Result";
            // 
            // csvToolStripMenuItem
            // 
            this.csvToolStripMenuItem.Name = "csvToolStripMenuItem";
            this.csvToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.csvToolStripMenuItem.Text = "csv ...";
            this.csvToolStripMenuItem.Click += new System.EventHandler(this.csvToolStripMenuItem_Click);
            // 
            // excelToolStripMenuItem
            // 
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.excelToolStripMenuItem.Text = "Excel ...";
            this.excelToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftJoinToolStripMenuItem,
            this.rowEditToolStripMenuItem,
            this.toolStripSeparator14,
            this.showGrouplabelInTableListToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::Projector.Properties.Resources.search;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "Toolbars";
            // 
            // leftJoinToolStripMenuItem
            // 
            this.leftJoinToolStripMenuItem.CheckOnClick = true;
            this.leftJoinToolStripMenuItem.Name = "leftJoinToolStripMenuItem";
            this.leftJoinToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.leftJoinToolStripMenuItem.Text = "Left Join";
            this.leftJoinToolStripMenuItem.Click += new System.EventHandler(this.leftJoinToolStripMenuItem_Click);
            // 
            // rowEditToolStripMenuItem
            // 
            this.rowEditToolStripMenuItem.CheckOnClick = true;
            this.rowEditToolStripMenuItem.Name = "rowEditToolStripMenuItem";
            this.rowEditToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.rowEditToolStripMenuItem.Text = "Row Edit";
            this.rowEditToolStripMenuItem.Click += new System.EventHandler(this.rowEditToolStripMenuItem_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(227, 6);
            // 
            // showGrouplabelInTableListToolStripMenuItem
            // 
            this.showGrouplabelInTableListToolStripMenuItem.Checked = true;
            this.showGrouplabelInTableListToolStripMenuItem.CheckOnClick = true;
            this.showGrouplabelInTableListToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGrouplabelInTableListToolStripMenuItem.Name = "showGrouplabelInTableListToolStripMenuItem";
            this.showGrouplabelInTableListToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.showGrouplabelInTableListToolStripMenuItem.Text = "Show Grouplabel in Table List";
            this.showGrouplabelInTableListToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.showGrouplabelInTableListToolStripMenuItem_CheckStateChanged);
            this.showGrouplabelInTableListToolStripMenuItem.Click += new System.EventHandler(this.showGrouplabelInTableListToolStripMenuItem_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::Projector.Properties.Resources.application_view_list;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton9.Text = "Create Table";
            this.toolStripButton9.Click += new System.EventHandler(this.toolStripButton9_Click);
            // 
            // leftJoinToolStrip
            // 
            this.leftJoinToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.leftJoinToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator4,
            this.leftJoinTables,
            this.toolStripSeparator5,
            this.leftJoinSource,
            this.toolStripSeparator6,
            this.compareSetting,
            this.toolStripSeparator7,
            this.leftJoinTarget,
            this.leftJoinRunBtn});
            this.leftJoinToolStrip.Location = new System.Drawing.Point(3, 0);
            this.leftJoinToolStrip.Name = "leftJoinToolStrip";
            this.leftJoinToolStrip.Size = new System.Drawing.Size(507, 25);
            this.leftJoinToolStrip.TabIndex = 3;
            this.leftJoinToolStrip.Visible = false;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(51, 22);
            this.toolStripLabel1.Text = "Left Join";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // leftJoinTables
            // 
            this.leftJoinTables.Name = "leftJoinTables";
            this.leftJoinTables.Size = new System.Drawing.Size(121, 25);
            this.leftJoinTables.TextChanged += new System.EventHandler(this.leftJoinTables_TextChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // leftJoinSource
            // 
            this.leftJoinSource.Name = "leftJoinSource";
            this.leftJoinSource.Size = new System.Drawing.Size(121, 25);
            this.leftJoinSource.Click += new System.EventHandler(this.leftJoinSource_Click);
            this.leftJoinSource.TextChanged += new System.EventHandler(this.leftJoinSource_TextChanged);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // compareSetting
            // 
            this.compareSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.compareSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.equalsToolStripMenuItem,
            this.differentToolStripMenuItem,
            this.greaterToolStripMenuItem,
            this.lowerToolStripMenuItem,
            this.greaterEqualsToolStripMenuItem,
            this.lowerEqualsToolStripMenuItem});
            this.compareSetting.Image = global::Projector.Properties.Resources.sql_editor;
            this.compareSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.compareSetting.Name = "compareSetting";
            this.compareSetting.Size = new System.Drawing.Size(28, 22);
            this.compareSetting.Text = "=";
            // 
            // equalsToolStripMenuItem
            // 
            this.equalsToolStripMenuItem.Name = "equalsToolStripMenuItem";
            this.equalsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.equalsToolStripMenuItem.Text = "equals =";
            this.equalsToolStripMenuItem.Click += new System.EventHandler(this.equalsToolStripMenuItem_Click);
            // 
            // differentToolStripMenuItem
            // 
            this.differentToolStripMenuItem.Name = "differentToolStripMenuItem";
            this.differentToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.differentToolStripMenuItem.Text = "different !=";
            this.differentToolStripMenuItem.Click += new System.EventHandler(this.differentToolStripMenuItem_Click);
            // 
            // greaterToolStripMenuItem
            // 
            this.greaterToolStripMenuItem.Name = "greaterToolStripMenuItem";
            this.greaterToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.greaterToolStripMenuItem.Text = "Greater >";
            this.greaterToolStripMenuItem.Click += new System.EventHandler(this.greaterToolStripMenuItem_Click);
            // 
            // lowerToolStripMenuItem
            // 
            this.lowerToolStripMenuItem.Name = "lowerToolStripMenuItem";
            this.lowerToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.lowerToolStripMenuItem.Text = "Lower <";
            this.lowerToolStripMenuItem.Click += new System.EventHandler(this.lowerToolStripMenuItem_Click);
            // 
            // greaterEqualsToolStripMenuItem
            // 
            this.greaterEqualsToolStripMenuItem.Name = "greaterEqualsToolStripMenuItem";
            this.greaterEqualsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.greaterEqualsToolStripMenuItem.Text = "Greater Equals >=";
            this.greaterEqualsToolStripMenuItem.Click += new System.EventHandler(this.greaterEqualsToolStripMenuItem_Click);
            // 
            // lowerEqualsToolStripMenuItem
            // 
            this.lowerEqualsToolStripMenuItem.Name = "lowerEqualsToolStripMenuItem";
            this.lowerEqualsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.lowerEqualsToolStripMenuItem.Text = "Lower Equals <=";
            this.lowerEqualsToolStripMenuItem.Click += new System.EventHandler(this.lowerEqualsToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // leftJoinTarget
            // 
            this.leftJoinTarget.Name = "leftJoinTarget";
            this.leftJoinTarget.Size = new System.Drawing.Size(121, 25);
            this.leftJoinTarget.TextChanged += new System.EventHandler(this.leftJoinTarget_TextChanged);
            // 
            // leftJoinRunBtn
            // 
            this.leftJoinRunBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.leftJoinRunBtn.Enabled = false;
            this.leftJoinRunBtn.Image = global::Projector.Properties.Resources.applications_16;
            this.leftJoinRunBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.leftJoinRunBtn.Name = "leftJoinRunBtn";
            this.leftJoinRunBtn.Size = new System.Drawing.Size(23, 22);
            this.leftJoinRunBtn.Text = "Left Join Run";
            this.leftJoinRunBtn.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // rowOptTtoolStrip
            // 
            this.rowOptTtoolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.rowOptTtoolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rowOptions,
            this.toolStripButton6,
            this.toolStripButton7});
            this.rowOptTtoolStrip.Location = new System.Drawing.Point(386, 0);
            this.rowOptTtoolStrip.Name = "rowOptTtoolStrip";
            this.rowOptTtoolStrip.Size = new System.Drawing.Size(81, 25);
            this.rowOptTtoolStrip.TabIndex = 4;
            this.rowOptTtoolStrip.Visible = false;
            // 
            // rowOptions
            // 
            this.rowOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rowOptions.Image = global::Projector.Properties.Resources.add_16;
            this.rowOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rowOptions.Name = "rowOptions";
            this.rowOptions.Size = new System.Drawing.Size(23, 22);
            this.rowOptions.Text = "Add a new Row";
            this.rowOptions.Click += new System.EventHandler(this.insertRowMenuItem_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::Projector.Properties.Resources.delete_16;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "Delete Selected Row(s)";
            this.toolStripButton6.Click += new System.EventHandler(this.DeleteRowMenuItem_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::Projector.Properties.Resources.clipboard_paste_document_text;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton7.Text = "Copy Selected Rows to ClipBoard";
            this.toolStripButton7.Click += new System.EventHandler(this.Copy_Click);
            // 
            // mysqlWorker
            // 
            this.mysqlWorker.WorkerReportsProgress = true;
            this.mysqlWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mysqlWorker_DoWork);
            this.mysqlWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.mysqlWorker_ProgressChanged);
            this.mysqlWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mysqlWorker_RunWorkerCompleted);
            // 
            // ParsingTimer
            // 
            this.ParsingTimer.Interval = 1000;
            this.ParsingTimer.Tick += new System.EventHandler(this.ParsingTimer_Tick);
            // 
            // saveCvsFile
            // 
            this.saveCvsFile.DefaultExt = "csv";
            this.saveCvsFile.FileName = "export.csv";
            this.saveCvsFile.Filter = "csv Export|*.csv|all Files|*.*";
            this.saveCvsFile.Title = "Export CSV";
            this.saveCvsFile.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.selectToolStripMenuItem.Text = "Select";
            // 
            // joinTryMenu
            // 
            this.joinTryMenu.Name = "joinTryMenu";
            this.joinTryMenu.Size = new System.Drawing.Size(166, 22);
            this.joinTryMenu.Text = "Try to Join...";
            this.joinTryMenu.Click += new System.EventHandler(this.joinTryMenu_Click);
            // 
            // queryBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 527);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "queryBrowser";
            this.Text = "queryBrowser";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.queryBrowser_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.RightToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.RightToolStripPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.editMenu.ResumeLayout(false);
            this.tableMenu.ResumeLayout(false);
            this.MaskTab.ResumeLayout(false);
            this.MaskTab.PerformLayout();
            this.maskBox.ResumeLayout(false);
            this.maskBox.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.tabPageProcedures.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.values.ResumeLayout(false);
            this.values.PerformLayout();
            this.tableContextMenu.ResumeLayout(false);
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.dialogToolStrip.ResumeLayout(false);
            this.dialogToolStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.leftJoinToolStrip.ResumeLayout(false);
            this.leftJoinToolStrip.PerformLayout();
            this.rowOptTtoolStrip.ResumeLayout(false);
            this.rowOptTtoolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ListView tableView;
        private System.Windows.Forms.ImageList dbPic;
        private System.ComponentModel.BackgroundWorker mysqlWorker;
        private System.Windows.Forms.ToolStripStatusLabel selectedTableLabel;
        private System.Windows.Forms.ListView TablesAutoComplete;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.RichTextBox triggerCode;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ComboBox trigger_Tables;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox trigger_timing;
        private System.Windows.Forms.ComboBox trigger_event;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox trigger_name;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ImageList DbColumnPics;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.TabPage MaskTab;
        private System.Windows.Forms.GroupBox maskBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton reloadtoolBtn;
        private System.Windows.Forms.TabPage tabPageProcedures;
        private System.Windows.Forms.GroupBox values;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox procSource;
        private System.Windows.Forms.TextBox procName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripComboBox bookmarkList;
        private System.Windows.Forms.ToolStrip leftJoinToolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox leftJoinTables;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripComboBox leftJoinSource;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripDropDownButton compareSetting;
        private System.Windows.Forms.ToolStripMenuItem equalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem differentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greaterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greaterEqualsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowerEqualsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripComboBox leftJoinTarget;
        private System.Windows.Forms.ToolStripButton leftJoinRunBtn;
        private System.Windows.Forms.RichTextBox textBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ContextMenuStrip editMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem addToBookmarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runQueryToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip tableMenu;
        private System.Windows.Forms.ToolStripMenuItem Copy;
        private System.Windows.Forms.ToolStripMenuItem copyCellValueToolStripMenuItem;
        private System.Windows.Forms.TextBox cellEditField;
        private System.Windows.Forms.ToolStripMenuItem updateCellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceUpdateCellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addValueToWhereToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripButton sqlHighlighting;
        private System.Windows.Forms.Timer ParsingTimer;
        private System.Windows.Forms.ToolStripMenuItem insertRowMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem DeleteRowMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem csvToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveCvsFile;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem leftJoinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rowEditToolStripMenuItem;
        private System.Windows.Forms.ToolStrip rowOptTtoolStrip;
        private System.Windows.Forms.ToolStripButton rowOptions;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripLabel TableNameView;
        private System.Windows.Forms.ToolStripStatusLabel rowResultCount;
        private System.Windows.Forms.TextBox searchTableTextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem showGrouplabelInTableListToolStripMenuItem;
        private System.Windows.Forms.ToolStrip dialogToolStrip;
        private System.Windows.Forms.ToolStripButton DialogOKBtn;
        private System.Windows.Forms.ToolStripButton dialogCancelBtn;
        private System.Windows.Forms.ToolStripMenuItem editSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createInsertSelectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySelectedAsInsertToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel explainStatusLabel;
        private System.Windows.Forms.ToolStripButton explainEnabled;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.GroupBox editBox;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripMenuItem rememberAsPlaceholderToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton placeholderReplace;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem copySelectedAsInsertduplicateKeyUpdateToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip tableContextMenu;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dropTablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyTablessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectUnion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem truncateTablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripMenuItem renameTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joinTryMenu;
    }
}