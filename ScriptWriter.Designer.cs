namespace Projector
{
    partial class ScriptWriter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptWriter));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.errorLabels = new System.Windows.Forms.ToolStripStatusLabel();
            this.errCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.workerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftTools = new System.Windows.Forms.SplitContainer();
            this.logbook = new System.Windows.Forms.ListBox();
            this.genericTree = new System.Windows.Forms.TreeView();
            this.TreeImages = new System.Windows.Forms.ImageList(this.components);
            this.codeSplitContainer = new System.Windows.Forms.SplitContainer();
            this.wordListing = new System.Windows.Forms.ListBox();
            this.messageSplit = new System.Windows.Forms.SplitContainer();
            this.errorTextBox = new System.Windows.Forms.RichTextBox();
            this.debugView = new System.Windows.Forms.ListView();
            this.lineHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.messageHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.loadButton = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showDebug = new System.Windows.Forms.ToolStripButton();
            this.navigatorBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.runButton = new System.Windows.Forms.ToolStripButton();
            this.continueBtn = new System.Windows.Forms.ToolStripButton();
            this.stopScr = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.switchDrawMode = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.keyTrigger = new System.Windows.Forms.Timer(this.components);
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.messageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.codeBox = new Projector.RichBox();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftTools)).BeginInit();
            this.leftTools.Panel1.SuspendLayout();
            this.leftTools.Panel2.SuspendLayout();
            this.leftTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.codeSplitContainer)).BeginInit();
            this.codeSplitContainer.Panel1.SuspendLayout();
            this.codeSplitContainer.Panel2.SuspendLayout();
            this.codeSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.messageSplit)).BeginInit();
            this.messageSplit.Panel1.SuspendLayout();
            this.messageSplit.Panel2.SuspendLayout();
            this.messageSplit.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.errorLabels,
            this.errCount,
            this.workerLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(985, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(28, 17);
            this.statusLabel.Text = "- - -";
            // 
            // errorLabels
            // 
            this.errorLabels.BackColor = System.Drawing.SystemColors.ControlDark;
            this.errorLabels.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.errorLabels.Name = "errorLabels";
            this.errorLabels.Size = new System.Drawing.Size(75, 17);
            this.errorLabels.Text = "not Executed";
            this.errorLabels.Click += new System.EventHandler(this.errorLabels_Click);
            // 
            // errCount
            // 
            this.errCount.Name = "errCount";
            this.errCount.Size = new System.Drawing.Size(22, 17);
            this.errCount.Text = "---";
            // 
            // workerLabel
            // 
            this.workerLabel.Name = "workerLabel";
            this.workerLabel.Size = new System.Drawing.Size(26, 17);
            this.workerLabel.Text = "idle";
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.leftTools);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.codeSplitContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(985, 592);
            this.mainSplitContainer.SplitterDistance = 217;
            this.mainSplitContainer.TabIndex = 2;
            // 
            // leftTools
            // 
            this.leftTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftTools.Location = new System.Drawing.Point(0, 0);
            this.leftTools.Name = "leftTools";
            this.leftTools.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // leftTools.Panel1
            // 
            this.leftTools.Panel1.Controls.Add(this.logbook);
            // 
            // leftTools.Panel2
            // 
            this.leftTools.Panel2.Controls.Add(this.genericTree);
            this.leftTools.Size = new System.Drawing.Size(217, 592);
            this.leftTools.SplitterDistance = 236;
            this.leftTools.TabIndex = 0;
            // 
            // logbook
            // 
            this.logbook.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.logbook.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logbook.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logbook.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.logbook.FormattingEnabled = true;
            this.logbook.ItemHeight = 14;
            this.logbook.Items.AddRange(new object[] {
            "idle..."});
            this.logbook.Location = new System.Drawing.Point(0, 0);
            this.logbook.Name = "logbook";
            this.logbook.Size = new System.Drawing.Size(217, 236);
            this.logbook.TabIndex = 0;
            // 
            // genericTree
            // 
            this.genericTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genericTree.ImageIndex = 0;
            this.genericTree.ImageList = this.TreeImages;
            this.genericTree.Location = new System.Drawing.Point(0, 0);
            this.genericTree.Name = "genericTree";
            this.genericTree.SelectedImageIndex = 0;
            this.genericTree.Size = new System.Drawing.Size(217, 352);
            this.genericTree.TabIndex = 0;
            this.genericTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.genericTree_AfterSelect);
            this.genericTree.DoubleClick += new System.EventHandler(this.genericTree_DoubleClick);
            // 
            // TreeImages
            // 
            this.TreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeImages.ImageStream")));
            this.TreeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.TreeImages.Images.SetKeyName(0, "share-16.png");
            this.TreeImages.Images.SetKeyName(1, "external.png");
            this.TreeImages.Images.SetKeyName(2, "objects.png");
            this.TreeImages.Images.SetKeyName(3, "sec_link.png");
            this.TreeImages.Images.SetKeyName(4, "tag.png");
            this.TreeImages.Images.SetKeyName(5, "stock_macro-stop-after-procedure.png");
            this.TreeImages.Images.SetKeyName(6, "edit-number.png");
            this.TreeImages.Images.SetKeyName(7, "blue-document-number.png");
            this.TreeImages.Images.SetKeyName(8, "button.png");
            this.TreeImages.Images.SetKeyName(9, "file.png");
            this.TreeImages.Images.SetKeyName(10, "icon-document-file-numbers.png");
            this.TreeImages.Images.SetKeyName(11, "link-add.png");
            this.TreeImages.Images.SetKeyName(12, "link-delete.png");
            this.TreeImages.Images.SetKeyName(13, "link-error.png");
            this.TreeImages.Images.SetKeyName(14, "Numbers-16.png");
            this.TreeImages.Images.SetKeyName(15, "stock_page-number.png");
            this.TreeImages.Images.SetKeyName(16, "stock_record-number.png");
            this.TreeImages.Images.SetKeyName(17, "text-16.png");
            this.TreeImages.Images.SetKeyName(18, "text-editor.png");
            // 
            // codeSplitContainer
            // 
            this.codeSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.codeSplitContainer.Name = "codeSplitContainer";
            this.codeSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // codeSplitContainer.Panel1
            // 
            this.codeSplitContainer.Panel1.Controls.Add(this.codeBox);
            this.codeSplitContainer.Panel1.Controls.Add(this.wordListing);
            // 
            // codeSplitContainer.Panel2
            // 
            this.codeSplitContainer.Panel2.Controls.Add(this.messageSplit);
            this.codeSplitContainer.Size = new System.Drawing.Size(764, 592);
            this.codeSplitContainer.SplitterDistance = 423;
            this.codeSplitContainer.TabIndex = 0;
            // 
            // wordListing
            // 
            this.wordListing.BackColor = System.Drawing.SystemColors.Info;
            this.wordListing.FormattingEnabled = true;
            this.wordListing.Location = new System.Drawing.Point(194, 99);
            this.wordListing.Name = "wordListing";
            this.wordListing.Size = new System.Drawing.Size(159, 173);
            this.wordListing.TabIndex = 1;
            // 
            // messageSplit
            // 
            this.messageSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageSplit.Location = new System.Drawing.Point(0, 0);
            this.messageSplit.Name = "messageSplit";
            // 
            // messageSplit.Panel1
            // 
            this.messageSplit.Panel1.Controls.Add(this.errorTextBox);
            // 
            // messageSplit.Panel2
            // 
            this.messageSplit.Panel2.Controls.Add(this.debugView);
            this.messageSplit.Size = new System.Drawing.Size(764, 165);
            this.messageSplit.SplitterDistance = 253;
            this.messageSplit.TabIndex = 1;
            // 
            // errorTextBox
            // 
            this.errorTextBox.BackColor = System.Drawing.SystemColors.Info;
            this.errorTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorTextBox.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorTextBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.errorTextBox.Location = new System.Drawing.Point(0, 0);
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.Size = new System.Drawing.Size(253, 165);
            this.errorTextBox.TabIndex = 0;
            this.errorTextBox.Text = "";
            this.errorTextBox.WordWrap = false;
            // 
            // debugView
            // 
            this.debugView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.debugView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lineHeader,
            this.messageHeader});
            this.debugView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugView.FullRowSelect = true;
            this.debugView.GridLines = true;
            this.debugView.Location = new System.Drawing.Point(0, 0);
            this.debugView.Name = "debugView";
            this.debugView.Size = new System.Drawing.Size(507, 165);
            this.debugView.TabIndex = 0;
            this.debugView.UseCompatibleStateImageBehavior = false;
            this.debugView.View = System.Windows.Forms.View.Details;
            this.debugView.ItemActivate += new System.EventHandler(this.debugView_ItemActivate);
            // 
            // lineHeader
            // 
            this.lineHeader.Text = "Line";
            // 
            // messageHeader
            // 
            this.messageHeader.Text = "Message";
            this.messageHeader.Width = 600;
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
            this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStripContainer2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(985, 617);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(985, 663);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.mainSplitContainer);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(985, 592);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(985, 617);
            this.toolStripContainer2.TabIndex = 3;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadButton,
            this.saveButton,
            this.toolStripSeparator1,
            this.showDebug,
            this.navigatorBtn,
            this.toolStripSeparator2,
            this.runButton,
            this.continueBtn,
            this.stopScr,
            this.toolStripSeparator3,
            this.switchDrawMode});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(214, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // loadButton
            // 
            this.loadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadButton.Image = global::Projector.Properties.Resources.folder_open_16;
            this.loadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(23, 22);
            this.loadButton.Text = "Load Script";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButton.Image = global::Projector.Properties.Resources.SAVE_16;
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(23, 22);
            this.saveButton.Text = "Save Script";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // showDebug
            // 
            this.showDebug.CheckOnClick = true;
            this.showDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showDebug.Image = global::Projector.Properties.Resources.application_view_list;
            this.showDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showDebug.Name = "showDebug";
            this.showDebug.Size = new System.Drawing.Size(23, 22);
            this.showDebug.Text = "toolStripButton1";
            this.showDebug.Click += new System.EventHandler(this.showDebug_Click);
            // 
            // navigatorBtn
            // 
            this.navigatorBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorBtn.Image = global::Projector.Properties.Resources.application_side_tree;
            this.navigatorBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.navigatorBtn.Name = "navigatorBtn";
            this.navigatorBtn.Size = new System.Drawing.Size(23, 22);
            this.navigatorBtn.Text = "toolStripButton1";
            this.navigatorBtn.Click += new System.EventHandler(this.navigatorBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // runButton
            // 
            this.runButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.runButton.Image = global::Projector.Properties.Resources.stock_tools_macro;
            this.runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(23, 22);
            this.runButton.Text = "runButton";
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // continueBtn
            // 
            this.continueBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.continueBtn.Enabled = false;
            this.continueBtn.Image = global::Projector.Properties.Resources.forward;
            this.continueBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.continueBtn.Name = "continueBtn";
            this.continueBtn.Size = new System.Drawing.Size(23, 22);
            this.continueBtn.Text = "continue";
            this.continueBtn.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // stopScr
            // 
            this.stopScr.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopScr.Image = global::Projector.Properties.Resources.delete;
            this.stopScr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopScr.Name = "stopScr";
            this.stopScr.Size = new System.Drawing.Size(23, 22);
            this.stopScr.Text = "Stop";
            this.stopScr.Click += new System.EventHandler(this.stopScr_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // switchDrawMode
            // 
            this.switchDrawMode.Checked = true;
            this.switchDrawMode.CheckOnClick = true;
            this.switchDrawMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.switchDrawMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.switchDrawMode.Image = global::Projector.Properties.Resources.edit_replace;
            this.switchDrawMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.switchDrawMode.Name = "switchDrawMode";
            this.switchDrawMode.Size = new System.Drawing.Size(23, 22);
            this.switchDrawMode.Text = "switch Highlighting mode";
            this.switchDrawMode.Click += new System.EventHandler(this.switchDrawMode_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.setupToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(985, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.runToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.fileToolStripMenuItem.Text = "&Script Editor";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Image = global::Projector.Properties.Resources.folder_open_16;
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.loadToolStripMenuItem.Text = "&Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveToolStripMenuItem.Text = "&Save As...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Image = global::Projector.Properties.Resources.SAVE_16;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Image = global::Projector.Properties.Resources.stock_tools_macro;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.runToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolbarToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // showToolbarToolStripMenuItem
            // 
            this.showToolbarToolStripMenuItem.Image = global::Projector.Properties.Resources.application_side_tree;
            this.showToolbarToolStripMenuItem.Name = "showToolbarToolStripMenuItem";
            this.showToolbarToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.showToolbarToolStripMenuItem.Text = "&Show Toolbar";
            this.showToolbarToolStripMenuItem.Click += new System.EventHandler(this.showToolbarToolStripMenuItem_Click);
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorsToolStripMenuItem});
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.setupToolStripMenuItem.Text = "&Setup";
            // 
            // colorsToolStripMenuItem
            // 
            this.colorsToolStripMenuItem.Name = "colorsToolStripMenuItem";
            this.colorsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.colorsToolStripMenuItem.Text = "&Highlighting Style";
            this.colorsToolStripMenuItem.Click += new System.EventHandler(this.colorsToolStripMenuItem_Click);
            // 
            // openFile
            // 
            this.openFile.FileName = "default.pscr";
            this.openFile.Filter = "Projector Script|*.pscr|Alle Dateien|*.*";
            this.openFile.Title = "Open Script File";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "pscr";
            this.saveFileDialog.Filter = "Projector Script|*.pscr|Alle Dateien|*.*";
            this.saveFileDialog.Title = "Load Scriptfile";
            // 
            // keyTrigger
            // 
            this.keyTrigger.Tick += new System.EventHandler(this.keyTrigger_Tick);
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 400;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // codeBox
            // 
            this.codeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBox.HideSelection = false;
            this.codeBox.Location = new System.Drawing.Point(0, 0);
            this.codeBox.Name = "codeBox";
            this.codeBox.Size = new System.Drawing.Size(764, 423);
            this.codeBox.TabIndex = 2;
            this.codeBox.Text = "";
            this.codeBox.WordWrap = false;
            // 
            // ScriptWriter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 663);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ScriptWriter";
            this.Text = "ScriptWriter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScriptWriter_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.leftTools.Panel1.ResumeLayout(false);
            this.leftTools.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftTools)).EndInit();
            this.leftTools.ResumeLayout(false);
            this.codeSplitContainer.Panel1.ResumeLayout(false);
            this.codeSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.codeSplitContainer)).EndInit();
            this.codeSplitContainer.ResumeLayout(false);
            this.messageSplit.Panel1.ResumeLayout(false);
            this.messageSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.messageSplit)).EndInit();
            this.messageSplit.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer codeSplitContainer;
        private System.Windows.Forms.RichTextBox errorTextBox;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton runButton;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.SplitContainer messageSplit;
        private System.Windows.Forms.ListView debugView;
        private System.Windows.Forms.ColumnHeader lineHeader;
        private System.Windows.Forms.ColumnHeader messageHeader;
        private System.Windows.Forms.Timer keyTrigger;
        private System.Windows.Forms.ToolStripButton loadButton;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton showDebug;
        private System.Windows.Forms.ToolStripStatusLabel errorLabels;
        private System.Windows.Forms.ToolTip messageToolTip;
        private System.Windows.Forms.ToolStripStatusLabel errCount;
        private System.Windows.Forms.SplitContainer leftTools;
        private System.Windows.Forms.TreeView genericTree;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolbarToolStripMenuItem;
        private System.Windows.Forms.ListBox wordListing;
        private System.Windows.Forms.ImageList TreeImages;
        private System.Windows.Forms.ToolStripButton navigatorBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton continueBtn;
        private System.Windows.Forms.ListBox logbook;
        private System.Windows.Forms.ToolStripButton stopScr;
        private System.Windows.Forms.ToolStripStatusLabel workerLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton switchDrawMode;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorsToolStripMenuItem;
        public RichBox codeBox;
    }
}