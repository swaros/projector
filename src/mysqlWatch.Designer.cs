namespace Projector
{
    partial class mysqlWatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mysqlWatch));
            this.mysql_watcher = new System.ComponentModel.BackgroundWorker();
            this.button73 = new System.Windows.Forms.Button();
            this.show_sql = new System.Windows.Forms.TextBox();
            this.sql_view = new System.Windows.Forms.ListView();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.ID = new System.Windows.Forms.ColumnHeader();
            this.Host = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.mysql_watch_list = new System.Windows.Forms.ListView();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.prozess_id = new System.Windows.Forms.RadioButton();
            this.host_group = new System.Windows.Forms.RadioButton();
            this.group = new System.Windows.Forms.CheckBox();
            this.show_sleeps = new System.Windows.Forms.CheckBox();
            this.show_processlist = new System.Windows.Forms.CheckBox();
            this.sqL_store_data = new System.Windows.Forms.CheckBox();
            this.label71 = new System.Windows.Forms.Label();
            this.slow_querys = new System.Windows.Forms.NumericUpDown();
            this.timerTickValue = new System.Windows.Forms.NumericUpDown();
            this.label70 = new System.Windows.Forms.Label();
            this.button74 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.mysql_watch = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.connectionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.kill_btn = new System.Windows.Forms.Button();
            this.groupBox27.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slow_querys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerTickValue)).BeginInit();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mysql_watcher
            // 
            this.mysql_watcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mysql_watcher_DoWork);
            this.mysql_watcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mysql_watcher_RunWorkerCompleted);
            // 
            // button73
            // 
            this.button73.Image = global::Projector.Properties.Resources.applications_24;
            this.button73.Location = new System.Drawing.Point(3, 3);
            this.button73.Name = "button73";
            this.button73.Size = new System.Drawing.Size(75, 64);
            this.button73.TabIndex = 5;
            this.button73.Text = "Refresh";
            this.button73.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button73.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.button73.UseVisualStyleBackColor = true;
            this.button73.Click += new System.EventHandler(this.button73_Click);
            // 
            // show_sql
            // 
            this.show_sql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.show_sql.Location = new System.Drawing.Point(0, 0);
            this.show_sql.Multiline = true;
            this.show_sql.Name = "show_sql";
            this.show_sql.Size = new System.Drawing.Size(645, 68);
            this.show_sql.TabIndex = 4;
            // 
            // sql_view
            // 
            this.sql_view.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.ID,
            this.Host,
            this.columnHeader1});
            this.sql_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sql_view.FullRowSelect = true;
            this.sql_view.GridLines = true;
            this.sql_view.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.sql_view.HideSelection = false;
            this.sql_view.Location = new System.Drawing.Point(0, 0);
            this.sql_view.Name = "sql_view";
            this.sql_view.Size = new System.Drawing.Size(645, 383);
            this.sql_view.TabIndex = 7;
            this.sql_view.UseCompatibleStateImageBehavior = false;
            this.sql_view.View = System.Windows.Forms.View.Details;
            this.sql_view.ItemActivate += new System.EventHandler(this.sql_view_ItemActivate);
            this.sql_view.Click += new System.EventHandler(this.sql_view_Click);
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "State";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Query";
            this.columnHeader9.Width = 260;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Time";
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Zeit";
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 70;
            // 
            // Host
            // 
            this.Host.Text = "Host";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "count";
            // 
            // mysql_watch_list
            // 
            this.mysql_watch_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mysql_watch_list.FullRowSelect = true;
            this.mysql_watch_list.Location = new System.Drawing.Point(0, 0);
            this.mysql_watch_list.Name = "mysql_watch_list";
            this.mysql_watch_list.Size = new System.Drawing.Size(324, 455);
            this.mysql_watch_list.TabIndex = 3;
            this.mysql_watch_list.UseCompatibleStateImageBehavior = false;
            this.mysql_watch_list.View = System.Windows.Forms.View.Details;
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.prozess_id);
            this.groupBox27.Controls.Add(this.host_group);
            this.groupBox27.Controls.Add(this.group);
            this.groupBox27.Controls.Add(this.show_sleeps);
            this.groupBox27.Controls.Add(this.show_processlist);
            this.groupBox27.Controls.Add(this.sqL_store_data);
            this.groupBox27.Controls.Add(this.label71);
            this.groupBox27.Controls.Add(this.slow_querys);
            this.groupBox27.Controls.Add(this.timerTickValue);
            this.groupBox27.Controls.Add(this.label70);
            this.groupBox27.Location = new System.Drawing.Point(165, 3);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Size = new System.Drawing.Size(491, 64);
            this.groupBox27.TabIndex = 8;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "Einstellungen";
            // 
            // prozess_id
            // 
            this.prozess_id.AutoSize = true;
            this.prozess_id.Location = new System.Drawing.Point(436, 37);
            this.prozess_id.Name = "prozess_id";
            this.prozess_id.Size = new System.Drawing.Size(33, 17);
            this.prozess_id.TabIndex = 9;
            this.prozess_id.TabStop = true;
            this.prozess_id.Text = "id";
            this.prozess_id.UseVisualStyleBackColor = true;
            // 
            // host_group
            // 
            this.host_group.AutoSize = true;
            this.host_group.Checked = true;
            this.host_group.Location = new System.Drawing.Point(383, 37);
            this.host_group.Name = "host_group";
            this.host_group.Size = new System.Drawing.Size(47, 17);
            this.host_group.TabIndex = 8;
            this.host_group.TabStop = true;
            this.host_group.Text = "Host";
            this.host_group.UseVisualStyleBackColor = true;
            // 
            // group
            // 
            this.group.AutoSize = true;
            this.group.Location = new System.Drawing.Point(305, 38);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(72, 17);
            this.group.TabIndex = 7;
            this.group.Text = "Gruppiere";
            this.group.UseVisualStyleBackColor = true;
            // 
            // show_sleeps
            // 
            this.show_sleeps.AutoSize = true;
            this.show_sleeps.Location = new System.Drawing.Point(159, 38);
            this.show_sleeps.Name = "show_sleeps";
            this.show_sleeps.Size = new System.Drawing.Size(81, 17);
            this.show_sleeps.TabIndex = 6;
            this.show_sleeps.Text = "zeige Sleep";
            this.show_sleeps.UseVisualStyleBackColor = true;
            // 
            // show_processlist
            // 
            this.show_processlist.AutoSize = true;
            this.show_processlist.Location = new System.Drawing.Point(141, 17);
            this.show_processlist.Name = "show_processlist";
            this.show_processlist.Size = new System.Drawing.Size(127, 17);
            this.show_processlist.TabIndex = 5;
            this.show_processlist.Text = "Verberge Prozessliste";
            this.show_processlist.UseVisualStyleBackColor = true;
            this.show_processlist.CheckedChanged += new System.EventHandler(this.show_processlist_CheckedChanged);
            // 
            // sqL_store_data
            // 
            this.sqL_store_data.AutoSize = true;
            this.sqL_store_data.Location = new System.Drawing.Point(289, 17);
            this.sqL_store_data.Name = "sqL_store_data";
            this.sqL_store_data.Size = new System.Drawing.Size(125, 17);
            this.sqL_store_data.TabIndex = 4;
            this.sqL_store_data.Text = "Querys nicht löschen";
            this.sqL_store_data.UseVisualStyleBackColor = true;
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Location = new System.Drawing.Point(4, 42);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(60, 13);
            this.label71.TabIndex = 3;
            this.label71.Text = "ab Time >=";
            // 
            // slow_querys
            // 
            this.slow_querys.Location = new System.Drawing.Point(70, 40);
            this.slow_querys.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.slow_querys.Name = "slow_querys";
            this.slow_querys.Size = new System.Drawing.Size(56, 20);
            this.slow_querys.TabIndex = 2;
            this.slow_querys.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // timerTickValue
            // 
            this.timerTickValue.Location = new System.Drawing.Point(70, 19);
            this.timerTickValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.timerTickValue.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.timerTickValue.Name = "timerTickValue";
            this.timerTickValue.Size = new System.Drawing.Size(56, 20);
            this.timerTickValue.TabIndex = 1;
            this.timerTickValue.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(7, 21);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(57, 13);
            this.label70.TabIndex = 0;
            this.label70.Text = "Timer Tick";
            // 
            // button74
            // 
            this.button74.Image = global::Projector.Properties.Resources.player_play_green;
            this.button74.Location = new System.Drawing.Point(84, 3);
            this.button74.Name = "button74";
            this.button74.Size = new System.Drawing.Size(75, 64);
            this.button74.TabIndex = 6;
            this.button74.Text = "Timer";
            this.button74.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button74.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.button74.UseVisualStyleBackColor = true;
            this.button74.Click += new System.EventHandler(this.button74_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.button73);
            this.panel1.Controls.Add(this.button74);
            this.panel1.Controls.Add(this.groupBox27);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(973, 77);
            this.panel1.TabIndex = 9;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mysql_watch_list);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(973, 455);
            this.splitContainer1.SplitterDistance = 324;
            this.splitContainer1.TabIndex = 10;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.sql_view);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.show_sql);
            this.splitContainer2.Size = new System.Drawing.Size(645, 455);
            this.splitContainer2.SplitterDistance = 383;
            this.splitContainer2.TabIndex = 0;
            // 
            // mysql_watch
            // 
            this.mysql_watch.Tick += new System.EventHandler(this.mysql_watch_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(973, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // connectionLabel
            // 
            this.connectionLabel.Name = "connectionLabel";
            this.connectionLabel.Size = new System.Drawing.Size(109, 17);
            this.connectionLabel.Text = "toolStripStatusLabel1";
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(973, 455);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 77);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(973, 502);
            this.toolStripContainer1.TabIndex = 12;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.kill_btn);
            this.groupBox1.Location = new System.Drawing.Point(662, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 63);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actions";
            // 
            // kill_btn
            // 
            this.kill_btn.Image = global::Projector.Properties.Resources.delete;
            this.kill_btn.Location = new System.Drawing.Point(6, 14);
            this.kill_btn.Name = "kill_btn";
            this.kill_btn.Size = new System.Drawing.Size(145, 43);
            this.kill_btn.TabIndex = 0;
            this.kill_btn.Text = "Kill";
            this.kill_btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.kill_btn.UseVisualStyleBackColor = true;
            this.kill_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // mysqlWatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 579);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mysqlWatch";
            this.Text = "mysqlWatch";
            this.groupBox27.ResumeLayout(false);
            this.groupBox27.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slow_querys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerTickValue)).EndInit();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker mysql_watcher;
        private System.Windows.Forms.Button button73;
        private System.Windows.Forms.TextBox show_sql;
        private System.Windows.Forms.ListView sql_view;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        public System.Windows.Forms.ListView mysql_watch_list;
        private System.Windows.Forms.GroupBox groupBox27;
        private System.Windows.Forms.CheckBox show_sleeps;
        private System.Windows.Forms.CheckBox show_processlist;
        private System.Windows.Forms.CheckBox sqL_store_data;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.NumericUpDown slow_querys;
        private System.Windows.Forms.NumericUpDown timerTickValue;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Button button74;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Timer mysql_watch;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel connectionLabel;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader Host;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.CheckBox group;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.RadioButton prozess_id;
        private System.Windows.Forms.RadioButton host_group;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button kill_btn;
    }
}