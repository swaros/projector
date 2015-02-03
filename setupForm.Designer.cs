namespace Projector
{
    partial class setupForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableListBox = new System.Windows.Forms.ListBox();
            this.showDataBases = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.errorLabel = new System.Windows.Forms.Label();
            this.btnChk = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.schema = new System.Windows.Forms.TextBox();
            this.host = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.diffviewparam = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.diffviewsetup = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.colorText = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.transActionCheck = new System.Windows.Forms.CheckBox();
            this.constrain_setup = new System.Windows.Forms.CheckBox();
            this.okbtn = new System.Windows.Forms.Button();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.openProfilXml = new System.Windows.Forms.OpenFileDialog();
            this.styleComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.mdiStyleComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(416, 285);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(408, 259);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MySQL";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableListBox);
            this.groupBox1.Controls.Add(this.showDataBases);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.errorLabel);
            this.groupBox1.Controls.Add(this.btnChk);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.schema);
            this.groupBox1.Controls.Add(this.host);
            this.groupBox1.Controls.Add(this.password);
            this.groupBox1.Controls.Add(this.username);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 235);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // tableListBox
            // 
            this.tableListBox.FormattingEnabled = true;
            this.tableListBox.Location = new System.Drawing.Point(131, 134);
            this.tableListBox.Name = "tableListBox";
            this.tableListBox.Size = new System.Drawing.Size(187, 95);
            this.tableListBox.TabIndex = 12;
            this.tableListBox.SelectedIndexChanged += new System.EventHandler(this.tableListBox_SelectedIndexChanged);
            // 
            // showDataBases
            // 
            this.showDataBases.Location = new System.Drawing.Point(324, 107);
            this.showDataBases.Name = "showDataBases";
            this.showDataBases.Size = new System.Drawing.Size(32, 23);
            this.showDataBases.TabIndex = 11;
            this.showDataBases.Text = "...";
            this.showDataBases.UseVisualStyleBackColor = true;
            this.showDataBases.Click += new System.EventHandler(this.showDataBases_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Import...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(6, 175);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(16, 13);
            this.errorLabel.TabIndex = 9;
            this.errorLabel.Text = "...";
            // 
            // btnChk
            // 
            this.btnChk.Location = new System.Drawing.Point(202, 144);
            this.btnChk.Name = "btnChk";
            this.btnChk.Size = new System.Drawing.Size(116, 23);
            this.btnChk.TabIndex = 8;
            this.btnChk.Text = "button1";
            this.btnChk.UseVisualStyleBackColor = true;
            this.btnChk.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // schema
            // 
            this.schema.Location = new System.Drawing.Point(131, 107);
            this.schema.Name = "schema";
            this.schema.Size = new System.Drawing.Size(187, 20);
            this.schema.TabIndex = 3;
            this.schema.TextChanged += new System.EventHandler(this.schema_TextChanged);
            // 
            // host
            // 
            this.host.Location = new System.Drawing.Point(131, 81);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(187, 20);
            this.host.TabIndex = 2;
            this.host.TextChanged += new System.EventHandler(this.host_TextChanged);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(131, 55);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(187, 20);
            this.password.TabIndex = 1;
            this.password.TextChanged += new System.EventHandler(this.password_TextChanged);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(131, 29);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(187, 20);
            this.username.TabIndex = 0;
            this.username.TextChanged += new System.EventHandler(this.username_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(408, 259);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "customize";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.mdiStyleComboBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.styleComboBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.diffviewparam);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.diffviewsetup);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.colorText);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 247);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Style";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 163);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Submitted parameters";
            // 
            // diffviewparam
            // 
            this.diffviewparam.Location = new System.Drawing.Point(23, 179);
            this.diffviewparam.Name = "diffviewparam";
            this.diffviewparam.Size = new System.Drawing.Size(329, 20);
            this.diffviewparam.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 215);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(231, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "[LEFT] is source left and [RIGHT] same for right";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(20, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Hint";
            // 
            // diffviewsetup
            // 
            this.diffviewsetup.Location = new System.Drawing.Point(23, 136);
            this.diffviewsetup.Name = "diffviewsetup";
            this.diffviewsetup.Size = new System.Drawing.Size(329, 20);
            this.diffviewsetup.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "external Diffviewer command";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Background Color";
            // 
            // colorText
            // 
            this.colorText.Location = new System.Drawing.Point(117, 29);
            this.colorText.Name = "colorText";
            this.colorText.ReadOnly = true;
            this.colorText.Size = new System.Drawing.Size(100, 20);
            this.colorText.TabIndex = 0;
            this.colorText.Click += new System.EventHandler(this.textBox1_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.transActionCheck);
            this.tabPage3.Controls.Add(this.constrain_setup);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(408, 259);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "MySQL connect Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // transActionCheck
            // 
            this.transActionCheck.AutoSize = true;
            this.transActionCheck.Location = new System.Drawing.Point(15, 48);
            this.transActionCheck.Name = "transActionCheck";
            this.transActionCheck.Size = new System.Drawing.Size(170, 17);
            this.transActionCheck.TabIndex = 1;
            this.transActionCheck.Text = "Use Transaction on any Query";
            this.transActionCheck.UseVisualStyleBackColor = true;
            // 
            // constrain_setup
            // 
            this.constrain_setup.AutoSize = true;
            this.constrain_setup.Location = new System.Drawing.Point(15, 24);
            this.constrain_setup.Name = "constrain_setup";
            this.constrain_setup.Size = new System.Drawing.Size(154, 17);
            this.constrain_setup.TabIndex = 0;
            this.constrain_setup.Text = "Disable Foreign Key Check";
            this.constrain_setup.UseVisualStyleBackColor = true;
            // 
            // okbtn
            // 
            this.okbtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okbtn.Location = new System.Drawing.Point(469, 34);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(86, 27);
            this.okbtn.TabIndex = 1;
            this.okbtn.Text = "button1";
            this.okbtn.UseVisualStyleBackColor = true;
            // 
            // cancelbtn
            // 
            this.cancelbtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelbtn.Location = new System.Drawing.Point(469, 69);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(86, 28);
            this.cancelbtn.TabIndex = 2;
            this.cancelbtn.Text = "button2";
            this.cancelbtn.UseVisualStyleBackColor = true;
            // 
            // openProfilXml
            // 
            this.openProfilXml.FileName = "openFileDialog1";
            this.openProfilXml.Filter = "Profil XML|*.xml";
            // 
            // styleComboBox
            // 
            this.styleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.styleComboBox.FormattingEnabled = true;
            this.styleComboBox.Location = new System.Drawing.Point(115, 55);
            this.styleComboBox.Name = "styleComboBox";
            this.styleComboBox.Size = new System.Drawing.Size(121, 21);
            this.styleComboBox.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Used Form Style";
            // 
            // mdiStyleComboBox
            // 
            this.mdiStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mdiStyleComboBox.FormattingEnabled = true;
            this.mdiStyleComboBox.Location = new System.Drawing.Point(115, 83);
            this.mdiStyleComboBox.Name = "mdiStyleComboBox";
            this.mdiStyleComboBox.Size = new System.Drawing.Size(121, 21);
            this.mdiStyleComboBox.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(23, 83);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Used MDI Style";
            // 
            // setupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 313);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.tabControl1);
            this.Name = "setupForm";
            this.Text = "setupForm";
            this.Load += new System.EventHandler(this.setupForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Button cancelbtn;
        public System.Windows.Forms.TextBox schema;
        public System.Windows.Forms.TextBox host;
        public System.Windows.Forms.TextBox password;
        public System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Button btnChk;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        public System.Windows.Forms.TextBox colorText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox diffviewsetup;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox diffviewparam;
        private System.Windows.Forms.OpenFileDialog openProfilXml;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.CheckBox transActionCheck;
        public System.Windows.Forms.CheckBox constrain_setup;
        private System.Windows.Forms.Button showDataBases;
        private System.Windows.Forms.ListBox tableListBox;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.ComboBox styleComboBox;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.ComboBox mdiStyleComboBox;
    }
}