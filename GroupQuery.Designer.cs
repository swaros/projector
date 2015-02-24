namespace Projector
{
    partial class GroupQuery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupQuery));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.databasesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.switchAssignemts = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.MainView = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.groupSelectBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.queryListing = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.sqlProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.runAsync = new System.Windows.Forms.ToolStripButton();
            this.exportEnable = new System.Windows.Forms.ToolStripSplitButton();
            this.cSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.loadAndFireToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSplitAndFireToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolHighlight = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.excelExport = new System.ComponentModel.BackgroundWorker();
            this.saveCvsFile = new System.Windows.Forms.SaveFileDialog();
            this.openSqlFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.csvExporter = new System.ComponentModel.BackgroundWorker();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.switchAssignemts.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1021, 600);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1021, 661);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1021, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(26, 17);
            this.statusLabel.Text = "Idle";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.MainView);
            this.splitContainer1.Size = new System.Drawing.Size(1021, 600);
            this.splitContainer1.SplitterDistance = 192;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer2.Size = new System.Drawing.Size(1021, 192);
            this.splitContainer2.SplitterDistance = 172;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.databasesListView);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 192);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Command";
            // 
            // databasesListView
            // 
            this.databasesListView.CheckBoxes = true;
            this.databasesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.databasesListView.ContextMenuStrip = this.switchAssignemts;
            this.databasesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.databasesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.databasesListView.Location = new System.Drawing.Point(3, 16);
            this.databasesListView.Name = "databasesListView";
            this.databasesListView.Size = new System.Drawing.Size(166, 173);
            this.databasesListView.TabIndex = 0;
            this.databasesListView.UseCompatibleStateImageBehavior = false;
            this.databasesListView.View = System.Windows.Forms.View.Details;
            this.databasesListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.databasesListView_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Profil";
            this.columnHeader1.Width = 160;
            // 
            // switchAssignemts
            // 
            this.switchAssignemts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.switchAssignemts.Name = "switchAssignemts";
            this.switchAssignemts.Size = new System.Drawing.Size(105, 70);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem1.Text = "Invert";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem2.Text = "All";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem3.Text = "None";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(845, 192);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyUp);
            this.richTextBox1.Leave += new System.EventHandler(this.richTextBox1_Leave);
            // 
            // MainView
            // 
            this.MainView.AutoScroll = true;
            this.MainView.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.MainView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainView.Location = new System.Drawing.Point(0, 0);
            this.MainView.Name = "MainView";
            this.MainView.Size = new System.Drawing.Size(1021, 404);
            this.MainView.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groupSelectBox,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.queryListing,
            this.toolStripSeparator2,
            this.sqlProgress,
            this.runAsync,
            this.exportEnable,
            this.toolStripSplitButton1,
            this.toolHighlight,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(387, 39);
            this.toolStrip1.TabIndex = 0;
            // 
            // groupSelectBox
            // 
            this.groupSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupSelectBox.Name = "groupSelectBox";
            this.groupSelectBox.Size = new System.Drawing.Size(121, 39);
            this.groupSelectBox.DropDownClosed += new System.EventHandler(this.groupSelectBox_TextChanged);
            this.groupSelectBox.TextUpdate += new System.EventHandler(this.groupSelectBox_TextChanged);
            this.groupSelectBox.TextChanged += new System.EventHandler(this.groupSelectBox_TextChanged);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Enabled = false;
            this.toolStripButton1.Image = global::Projector.Properties.Resources.applications_32;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton1.Text = "Run Query at all Databases";
            this.toolStripButton1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::Projector.Properties.Resources.bonobo_component_browser;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton2.Text = "Run Listing";
            this.toolStripButton2.Visible = false;
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // queryListing
            // 
            this.queryListing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.queryListing.DropDownWidth = 300;
            this.queryListing.Name = "queryListing";
            this.queryListing.Size = new System.Drawing.Size(121, 39);
            this.queryListing.Visible = false;
            this.queryListing.SelectedIndexChanged += new System.EventHandler(this.queryListing_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // sqlProgress
            // 
            this.sqlProgress.Name = "sqlProgress";
            this.sqlProgress.Size = new System.Drawing.Size(100, 36);
            this.sqlProgress.Visible = false;
            // 
            // runAsync
            // 
            this.runAsync.Checked = true;
            this.runAsync.CheckOnClick = true;
            this.runAsync.CheckState = System.Windows.Forms.CheckState.Checked;
            this.runAsync.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.runAsync.Image = global::Projector.Properties.Resources.network_workgroup;
            this.runAsync.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runAsync.Name = "runAsync";
            this.runAsync.Size = new System.Drawing.Size(36, 36);
            this.runAsync.Text = "toolStripButton3";
            this.runAsync.ToolTipText = "Run in Async mode";
            // 
            // exportEnable
            // 
            this.exportEnable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exportEnable.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cSVToolStripMenuItem,
            this.excelToolStripMenuItem});
            this.exportEnable.Enabled = false;
            this.exportEnable.Image = global::Projector.Properties.Resources.SAVE_32;
            this.exportEnable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportEnable.Name = "exportEnable";
            this.exportEnable.Size = new System.Drawing.Size(48, 36);
            this.exportEnable.Text = "Export Result";
            this.exportEnable.ButtonClick += new System.EventHandler(this.exportEnable_ButtonClick);
            // 
            // cSVToolStripMenuItem
            // 
            this.cSVToolStripMenuItem.Name = "cSVToolStripMenuItem";
            this.cSVToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.cSVToolStripMenuItem.Text = "CSV...";
            this.cSVToolStripMenuItem.Click += new System.EventHandler(this.cSVToolStripMenuItem_Click);
            // 
            // excelToolStripMenuItem
            // 
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.excelToolStripMenuItem.Text = "Excel ...";
            this.excelToolStripMenuItem.Click += new System.EventHandler(this.exportEnable_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadAndFireToolStripMenuItem,
            this.loadSplitAndFireToolStripMenuItem});
            this.toolStripSplitButton1.Image = global::Projector.Properties.Resources.folder_open_32;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(48, 36);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // loadAndFireToolStripMenuItem
            // 
            this.loadAndFireToolStripMenuItem.Name = "loadAndFireToolStripMenuItem";
            this.loadAndFireToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.loadAndFireToolStripMenuItem.Text = "Load and Fire";
            // 
            // loadSplitAndFireToolStripMenuItem
            // 
            this.loadSplitAndFireToolStripMenuItem.Name = "loadSplitAndFireToolStripMenuItem";
            this.loadSplitAndFireToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.loadSplitAndFireToolStripMenuItem.Text = "Load, split and fire ...";
            this.loadSplitAndFireToolStripMenuItem.Click += new System.EventHandler(this.loadSplitAndFireToolStripMenuItem_Click);
            // 
            // toolHighlight
            // 
            this.toolHighlight.CheckOnClick = true;
            this.toolHighlight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolHighlight.Image = global::Projector.Properties.Resources.highlighter_text;
            this.toolHighlight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolHighlight.Name = "toolHighlight";
            this.toolHighlight.Size = new System.Drawing.Size(36, 36);
            this.toolHighlight.Text = "highlighter";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::Projector.Properties.Resources.documents_32;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // excelExport
            // 
            this.excelExport.DoWork += new System.ComponentModel.DoWorkEventHandler(this.excelExport_DoWork);
            // 
            // saveCvsFile
            // 
            this.saveCvsFile.DefaultExt = "csv";
            this.saveCvsFile.FileName = "export.csv";
            this.saveCvsFile.Filter = "csv Export|*.csv|all Files|*.*";
            this.saveCvsFile.Title = "Export CSV";
            // 
            // openSqlFileDialog
            // 
            this.openSqlFileDialog.DefaultExt = "sql";
            this.openSqlFileDialog.Filter = "sql Files|*.sql|Alle Dateien|*.*";
            this.openSqlFileDialog.Title = "Load Sql";
            // 
            // csvExporter
            // 
            this.csvExporter.WorkerReportsProgress = true;
            this.csvExporter.DoWork += new System.ComponentModel.DoWorkEventHandler(this.csvExporter_DoWork);
            // 
            // GroupQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 661);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GroupQuery";
            this.Text = "GroupQuery";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GroupQuery_FormClosed);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.switchAssignemts.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel MainView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox groupSelectBox;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripProgressBar sqlProgress;
        private System.Windows.Forms.ListView databasesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.ComponentModel.BackgroundWorker excelExport;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        public System.Windows.Forms.ToolStripComboBox queryListing;
        private System.Windows.Forms.ToolStripButton runAsync;
        private System.Windows.Forms.ToolStripSplitButton exportEnable;
        private System.Windows.Forms.ToolStripMenuItem cSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveCvsFile;
        private System.Windows.Forms.OpenFileDialog openSqlFileDialog;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem loadAndFireToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSplitAndFireToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker csvExporter;
        private System.Windows.Forms.ContextMenuStrip switchAssignemts;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripButton toolHighlight;
        private System.Windows.Forms.ToolStripButton toolStripButton3;

    }
}