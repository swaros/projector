namespace Projector
{
    partial class ProfilSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfilSelect));
            this.profileName = new System.Windows.Forms.TextBox();
            this.newProflie = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.profil_label = new System.Windows.Forms.Label();
            this.listViewProfiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.okbtn = new System.Windows.Forms.Button();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // profileName
            // 
            this.profileName.Location = new System.Drawing.Point(6, 326);
            this.profileName.Name = "profileName";
            this.profileName.Size = new System.Drawing.Size(169, 20);
            this.profileName.TabIndex = 0;
            // 
            // newProflie
            // 
            this.newProflie.Location = new System.Drawing.Point(181, 323);
            this.newProflie.Name = "newProflie";
            this.newProflie.Size = new System.Drawing.Size(85, 23);
            this.newProflie.TabIndex = 1;
            this.newProflie.Text = "[NEW]";
            this.newProflie.UseVisualStyleBackColor = true;
            this.newProflie.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.profil_label);
            this.groupBox1.Controls.Add(this.listViewProfiles);
            this.groupBox1.Controls.Add(this.newProflie);
            this.groupBox1.Controls.Add(this.profileName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 360);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // profil_label
            // 
            this.profil_label.AutoSize = true;
            this.profil_label.Location = new System.Drawing.Point(6, 24);
            this.profil_label.Name = "profil_label";
            this.profil_label.Size = new System.Drawing.Size(35, 13);
            this.profil_label.TabIndex = 4;
            this.profil_label.Text = "label1";
            // 
            // listViewProfiles
            // 
            this.listViewProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewProfiles.HideSelection = false;
            this.listViewProfiles.LargeImageList = this.imageList1;
            this.listViewProfiles.Location = new System.Drawing.Point(6, 49);
            this.listViewProfiles.Name = "listViewProfiles";
            this.listViewProfiles.Size = new System.Drawing.Size(260, 268);
            this.listViewProfiles.SmallImageList = this.imageList1;
            this.listViewProfiles.TabIndex = 3;
            this.listViewProfiles.UseCompatibleStateImageBehavior = false;
            this.listViewProfiles.SelectedIndexChanged += new System.EventHandler(this.listViewProfiles_SelectedIndexChanged);
            this.listViewProfiles.DoubleClick += new System.EventHandler(this.listViewProfiles_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Profil";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Keyname";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "bonobo-component-browser.png");
            this.imageList1.Images.SetKeyName(1, "computer_16.png");
            // 
            // okbtn
            // 
            this.okbtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okbtn.Location = new System.Drawing.Point(313, 31);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(86, 23);
            this.okbtn.TabIndex = 3;
            this.okbtn.Text = "[OK]";
            this.okbtn.UseVisualStyleBackColor = true;
            // 
            // cancelbtn
            // 
            this.cancelbtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbtn.Location = new System.Drawing.Point(313, 61);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(86, 23);
            this.cancelbtn.TabIndex = 4;
            this.cancelbtn.Text = "[CANCEL]";
            this.cancelbtn.UseVisualStyleBackColor = true;
            // 
            // ProfilSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 382);
            this.ControlBox = false;
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProfilSelect";
            this.Text = "ProfilSelect";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox profileName;
        private System.Windows.Forms.Button newProflie;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewProfiles;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label profil_label;
    }
}