namespace Projector
{
    partial class ProjectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectorForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseWatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mDIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.loadProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.einstellungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.profilToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCurrentProfilToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchButtonModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupButtonsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ProfilInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectionState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.MainContainer = new System.Windows.Forms.Panel();
            this.mainSlitter = new System.Windows.Forms.SplitContainer();
            this.flowLayoutControllPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.showProfilLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.profilSelector = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.chooseGroup = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.scriptRunButton = new System.Windows.Forms.ToolStripButton();
            this.exportProfileDlg = new System.Windows.Forms.SaveFileDialog();
            this.openProjectDlg = new System.Windows.Forms.OpenFileDialog();
            this.saveProject = new System.Windows.Forms.SaveFileDialog();
            this.openScript = new System.Windows.Forms.OpenFileDialog();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.style_0 = new System.Windows.Forms.ToolStripButton();
            this.style_1 = new System.Windows.Forms.ToolStripButton();
            this.style_2 = new System.Windows.Forms.ToolStripButton();
            this.style_3 = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.MainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSlitter)).BeginInit();
            this.mainSlitter.Panel1.SuspendLayout();
            this.mainSlitter.Panel2.SuspendLayout();
            this.mainSlitter.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.einstellungenToolStripMenuItem,
            this.displayToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(685, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseWatchToolStripMenuItem,
            this.mDIToolStripMenuItem,
            this.groupQueryToolStripMenuItem,
            this.toolStripSeparator3,
            this.loadProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.editScriptToolStripMenuItem});
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.startToolStripMenuItem.Text = "&Project";
            // 
            // databaseWatchToolStripMenuItem
            // 
            this.databaseWatchToolStripMenuItem.Image = global::Projector.Properties.Resources.applications_161;
            this.databaseWatchToolStripMenuItem.Name = "databaseWatchToolStripMenuItem";
            this.databaseWatchToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.databaseWatchToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.databaseWatchToolStripMenuItem.Text = "Sync Database...";
            this.databaseWatchToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // mDIToolStripMenuItem
            // 
            this.mDIToolStripMenuItem.Name = "mDIToolStripMenuItem";
            this.mDIToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.mDIToolStripMenuItem.Text = "Show Database...";
            this.mDIToolStripMenuItem.Click += new System.EventHandler(this.mDIToolStripMenuItem_Click);
            // 
            // groupQueryToolStripMenuItem
            // 
            this.groupQueryToolStripMenuItem.Name = "groupQueryToolStripMenuItem";
            this.groupQueryToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.groupQueryToolStripMenuItem.Text = "Group Query...";
            this.groupQueryToolStripMenuItem.Click += new System.EventHandler(this.groupQueryToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(201, 6);
            // 
            // loadProjectToolStripMenuItem
            // 
            this.loadProjectToolStripMenuItem.Image = global::Projector.Properties.Resources.folder_closed_16;
            this.loadProjectToolStripMenuItem.Name = "loadProjectToolStripMenuItem";
            this.loadProjectToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.loadProjectToolStripMenuItem.Text = "Load Project...";
            this.loadProjectToolStripMenuItem.Click += new System.EventHandler(this.loadProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Image = global::Projector.Properties.Resources.SAVE_16;
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project...";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.backupProfilesToolStripMenuItem_Click);
            // 
            // editScriptToolStripMenuItem
            // 
            this.editScriptToolStripMenuItem.Name = "editScriptToolStripMenuItem";
            this.editScriptToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.editScriptToolStripMenuItem.Text = "Edit Script...";
            this.editScriptToolStripMenuItem.Click += new System.EventHandler(this.editScriptToolStripMenuItem_Click);
            // 
            // einstellungenToolStripMenuItem
            // 
            this.einstellungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.profilToolStripMenuItem,
            this.setupToolStripMenuItem,
            this.exportCurrentProfilToolStripMenuItem,
            this.groupsToolStripMenuItem,
            this.mainSettingsToolStripMenuItem});
            this.einstellungenToolStripMenuItem.Name = "einstellungenToolStripMenuItem";
            this.einstellungenToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.einstellungenToolStripMenuItem.Text = "&Options";
            // 
            // profilToolStripMenuItem
            // 
            this.profilToolStripMenuItem.Name = "profilToolStripMenuItem";
            this.profilToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.profilToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.profilToolStripMenuItem.Text = "&Profil...";
            this.profilToolStripMenuItem.Click += new System.EventHandler(this.profilToolStripMenuItem_Click);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.setupToolStripMenuItem.Text = "Setup...";
            this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
            // 
            // exportCurrentProfilToolStripMenuItem
            // 
            this.exportCurrentProfilToolStripMenuItem.Name = "exportCurrentProfilToolStripMenuItem";
            this.exportCurrentProfilToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.exportCurrentProfilToolStripMenuItem.Text = "Export Current Profil...";
            this.exportCurrentProfilToolStripMenuItem.Click += new System.EventHandler(this.exportCurrentProfilToolStripMenuItem_Click);
            // 
            // groupsToolStripMenuItem
            // 
            this.groupsToolStripMenuItem.Name = "groupsToolStripMenuItem";
            this.groupsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.groupsToolStripMenuItem.Text = "Groups...";
            this.groupsToolStripMenuItem.Click += new System.EventHandler(this.groupsToolStripMenuItem_Click);
            // 
            // mainSettingsToolStripMenuItem
            // 
            this.mainSettingsToolStripMenuItem.Name = "mainSettingsToolStripMenuItem";
            this.mainSettingsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.mainSettingsToolStripMenuItem.Text = "Main Settings...";
            this.mainSettingsToolStripMenuItem.Click += new System.EventHandler(this.mainSettingsToolStripMenuItem_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groupedToolStripMenuItem,
            this.switchButtonModeToolStripMenuItem,
            this.groupButtonsToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.displayToolStripMenuItem.Text = "&Display";
            // 
            // groupedToolStripMenuItem
            // 
            this.groupedToolStripMenuItem.CheckOnClick = true;
            this.groupedToolStripMenuItem.Name = "groupedToolStripMenuItem";
            this.groupedToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.groupedToolStripMenuItem.Text = "&Grouped";
            this.groupedToolStripMenuItem.Click += new System.EventHandler(this.groupedToolStripMenuItem_Click);
            // 
            // switchButtonModeToolStripMenuItem
            // 
            this.switchButtonModeToolStripMenuItem.Name = "switchButtonModeToolStripMenuItem";
            this.switchButtonModeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.switchButtonModeToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.switchButtonModeToolStripMenuItem.Text = "Switch Button Mode";
            this.switchButtonModeToolStripMenuItem.Click += new System.EventHandler(this.switchButtonModeToolStripMenuItem_Click);
            // 
            // groupButtonsToolStripMenuItem
            // 
            this.groupButtonsToolStripMenuItem.Checked = true;
            this.groupButtonsToolStripMenuItem.CheckOnClick = true;
            this.groupButtonsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.groupButtonsToolStripMenuItem.Name = "groupButtonsToolStripMenuItem";
            this.groupButtonsToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.groupButtonsToolStripMenuItem.Text = "Group Buttons";
            this.groupButtonsToolStripMenuItem.Click += new System.EventHandler(this.groupButtonsToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProfilInfo,
            this.connectionState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 414);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(685, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProfilInfo
            // 
            this.ProfilInfo.Name = "ProfilInfo";
            this.ProfilInfo.Size = new System.Drawing.Size(45, 17);
            this.ProfilInfo.Text = "Default";
            // 
            // connectionState
            // 
            this.connectionState.Name = "connectionState";
            this.connectionState.Size = new System.Drawing.Size(16, 17);
            this.connectionState.Text = "...";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.MainContainer);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(685, 365);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(685, 390);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // MainContainer
            // 
            this.MainContainer.Controls.Add(this.mainSlitter);
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(0, 0);
            this.MainContainer.Name = "MainContainer";
            this.MainContainer.Size = new System.Drawing.Size(685, 365);
            this.MainContainer.TabIndex = 0;
            // 
            // mainSlitter
            // 
            this.mainSlitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSlitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSlitter.IsSplitterFixed = true;
            this.mainSlitter.Location = new System.Drawing.Point(0, 0);
            this.mainSlitter.Name = "mainSlitter";
            // 
            // mainSlitter.Panel1
            // 
            this.mainSlitter.Panel1.Controls.Add(this.flowLayoutControllPanel);
            // 
            // mainSlitter.Panel2
            // 
            this.mainSlitter.Panel2.Controls.Add(this.flowLayout);
            this.mainSlitter.Size = new System.Drawing.Size(685, 365);
            this.mainSlitter.SplitterDistance = 172;
            this.mainSlitter.TabIndex = 1;
            // 
            // flowLayoutControllPanel
            // 
            this.flowLayoutControllPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutControllPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutControllPanel.Name = "flowLayoutControllPanel";
            this.flowLayoutControllPanel.Size = new System.Drawing.Size(172, 365);
            this.flowLayoutControllPanel.TabIndex = 0;
            // 
            // flowLayout
            // 
            this.flowLayout.AutoScroll = true;
            this.flowLayout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayout.Location = new System.Drawing.Point(0, 0);
            this.flowLayout.Name = "flowLayout";
            this.flowLayout.Size = new System.Drawing.Size(509, 365);
            this.flowLayout.TabIndex = 0;
            this.flowLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayout_Paint);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showProfilLabel,
            this.toolStripSeparator1,
            this.profilSelector,
            this.toolStripSeparator2,
            this.toolStripButton2,
            this.toolStripButton1,
            this.toolStripButton3,
            this.chooseGroup,
            this.toolStripSeparator4,
            this.scriptRunButton,
            this.toolStripSeparator5,
            this.style_0,
            this.style_1,
            this.style_2,
            this.style_3});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(509, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // showProfilLabel
            // 
            this.showProfilLabel.Name = "showProfilLabel";
            this.showProfilLabel.Size = new System.Drawing.Size(45, 22);
            this.showProfilLabel.Text = "Default";
            this.showProfilLabel.Click += new System.EventHandler(this.showProfilLabel_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // profilSelector
            // 
            this.profilSelector.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.profilSelector.Image = global::Projector.Properties.Resources.arrow_down_16;
            this.profilSelector.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.profilSelector.Name = "profilSelector";
            this.profilSelector.Size = new System.Drawing.Size(29, 22);
            this.profilSelector.Text = "Choose Profil";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::Projector.Properties.Resources.database_table;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Db Sync Tool";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Projector.Properties.Resources.servers_network;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Group Query";
            this.toolStripButton1.Click += new System.EventHandler(this.groupQueryToolStripMenuItem_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::Projector.Properties.Resources.search;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Show Status";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // chooseGroup
            // 
            this.chooseGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chooseGroup.Name = "chooseGroup";
            this.chooseGroup.Size = new System.Drawing.Size(121, 25);
            this.chooseGroup.SelectedIndexChanged += new System.EventHandler(this.chooseGroup_SelectedIndexChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // scriptRunButton
            // 
            this.scriptRunButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.scriptRunButton.Image = global::Projector.Properties.Resources.stock_tools_macro;
            this.scriptRunButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scriptRunButton.Name = "scriptRunButton";
            this.scriptRunButton.Size = new System.Drawing.Size(23, 22);
            this.scriptRunButton.Text = "Execute Projector Script File";
            this.scriptRunButton.Click += new System.EventHandler(this.scriptRunButton_Click);
            // 
            // exportProfileDlg
            // 
            this.exportProfileDlg.Filter = "Xml Profil|*.xml";
            this.exportProfileDlg.Title = "Profil Export";
            // 
            // openProjectDlg
            // 
            this.openProjectDlg.DefaultExt = "xml";
            this.openProjectDlg.FileName = "Project.xmlpr";
            this.openProjectDlg.Filter = "Xml Profil|*.xmlpr";
            this.openProjectDlg.Title = "Open Project";
            // 
            // saveProject
            // 
            this.saveProject.Filter = "Xml Profil|*.xmlpr";
            this.saveProject.Title = "Project Save";
            // 
            // openScript
            // 
            this.openScript.DefaultExt = "pscr";
            this.openScript.Filter = "Projector Script|*.pscr";
            this.openScript.Tag = "LoadScript";
            this.openScript.Title = "Load Script File";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // style_0
            // 
            this.style_0.CheckOnClick = true;
            this.style_0.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.style_0.Image = ((System.Drawing.Image)(resources.GetObject("style_0.Image")));
            this.style_0.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.style_0.Name = "style_0";
            this.style_0.Size = new System.Drawing.Size(28, 22);
            this.style_0.Text = "Big";
            this.style_0.Click += new System.EventHandler(this.style_0_Click);
            // 
            // style_1
            // 
            this.style_1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.style_1.Image = ((System.Drawing.Image)(resources.GetObject("style_1.Image")));
            this.style_1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.style_1.Name = "style_1";
            this.style_1.Size = new System.Drawing.Size(40, 22);
            this.style_1.Text = "Small";
            this.style_1.Click += new System.EventHandler(this.style_1_Click);
            // 
            // style_2
            // 
            this.style_2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.style_2.Image = ((System.Drawing.Image)(resources.GetObject("style_2.Image")));
            this.style_2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.style_2.Name = "style_2";
            this.style_2.Size = new System.Drawing.Size(39, 22);
            this.style_2.Text = "Black";
            this.style_2.Click += new System.EventHandler(this.style_2_Click);
            // 
            // style_3
            // 
            this.style_3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.style_3.Image = ((System.Drawing.Image)(resources.GetObject("style_3.Image")));
            this.style_3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.style_3.Name = "style_3";
            this.style_3.Size = new System.Drawing.Size(46, 22);
            this.style_3.Text = "Scripts";
            this.style_3.Click += new System.EventHandler(this.style_3_Click);
            // 
            // ProjectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 436);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ProjectorForm";
            this.Text = "Projector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectorForm_FormClosing);
            this.Shown += new System.EventHandler(this.ProjectorForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.MainContainer.ResumeLayout(false);
            this.mainSlitter.Panel1.ResumeLayout(false);
            this.mainSlitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSlitter)).EndInit();
            this.mainSlitter.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem einstellungenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem profilToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseWatchToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ProfilInfo;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel showProfilLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton profilSelector;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mDIToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Panel MainContainer;
        private System.Windows.Forms.FlowLayoutPanel flowLayout;
        private System.Windows.Forms.ToolStripMenuItem exportCurrentProfilToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog exportProfileDlg;
        private System.Windows.Forms.ToolStripMenuItem groupsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupQueryToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripStatusLabel connectionState;
        private System.Windows.Forms.ToolStripMenuItem loadProjectToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openProjectDlg;
        private System.Windows.Forms.SaveFileDialog saveProject;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem groupedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchButtonModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox chooseGroup;
        private System.Windows.Forms.SplitContainer mainSlitter;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutControllPanel;
        private System.Windows.Forms.ToolStripMenuItem groupButtonsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton scriptRunButton;
        private System.Windows.Forms.OpenFileDialog openScript;
        private System.Windows.Forms.ToolStripMenuItem editScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton style_0;
        private System.Windows.Forms.ToolStripButton style_1;
        private System.Windows.Forms.ToolStripButton style_2;
        private System.Windows.Forms.ToolStripButton style_3;


    }
}

