namespace Projector
{
    partial class MainSetup
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.OnlyNamedScripts = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bgSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MainMDIStyle = new System.Windows.Forms.ComboBox();
            this.MainFormStyle = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.displayNamedScript = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ScriptPath = new Projector.PathSelector();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.OnlyNamedScripts.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.OnlyNamedScripts);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(424, 238);
            this.tabControl1.TabIndex = 0;
            // 
            // OnlyNamedScripts
            // 
            this.OnlyNamedScripts.Controls.Add(this.groupBox3);
            this.OnlyNamedScripts.Controls.Add(this.groupBox2);
            this.OnlyNamedScripts.Location = new System.Drawing.Point(4, 22);
            this.OnlyNamedScripts.Name = "OnlyNamedScripts";
            this.OnlyNamedScripts.Padding = new System.Windows.Forms.Padding(3);
            this.OnlyNamedScripts.Size = new System.Drawing.Size(416, 212);
            this.OnlyNamedScripts.TabIndex = 0;
            this.OnlyNamedScripts.Text = "Main Settings";
            this.OnlyNamedScripts.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.bgSelect);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.MainMDIStyle);
            this.groupBox3.Controls.Add(this.MainFormStyle);
            this.groupBox3.Location = new System.Drawing.Point(9, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(401, 91);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Styles";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(265, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Worbench Background";
            // 
            // bgSelect
            // 
            this.bgSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bgSelect.FormattingEnabled = true;
            this.bgSelect.Items.AddRange(new object[] {
            "Plain (no Image)",
            "Substance",
            "Plates",
            "Grid",
            "Stripes",
            "Metall",
            "Metall Grid",
            "Metall Grid (Darker)"});
            this.bgSelect.Location = new System.Drawing.Point(265, 42);
            this.bgSelect.Name = "bgSelect";
            this.bgSelect.Size = new System.Drawing.Size(121, 21);
            this.bgSelect.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Default MDI Style";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Default Form Style";
            // 
            // MainMDIStyle
            // 
            this.MainMDIStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MainMDIStyle.FormattingEnabled = true;
            this.MainMDIStyle.Location = new System.Drawing.Point(108, 42);
            this.MainMDIStyle.Name = "MainMDIStyle";
            this.MainMDIStyle.Size = new System.Drawing.Size(132, 21);
            this.MainMDIStyle.TabIndex = 1;
            // 
            // MainFormStyle
            // 
            this.MainFormStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MainFormStyle.FormattingEnabled = true;
            this.MainFormStyle.Location = new System.Drawing.Point(108, 14);
            this.MainFormStyle.Name = "MainFormStyle";
            this.MainFormStyle.Size = new System.Drawing.Size(132, 21);
            this.MainFormStyle.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.displayNamedScript);
            this.groupBox2.Location = new System.Drawing.Point(9, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(207, 54);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Reflection Script";
            // 
            // displayNamedScript
            // 
            this.displayNamedScript.AutoSize = true;
            this.displayNamedScript.Location = new System.Drawing.Point(7, 20);
            this.displayNamedScript.Name = "displayNamedScript";
            this.displayNamedScript.Size = new System.Drawing.Size(156, 17);
            this.displayNamedScript.TabIndex = 0;
            this.displayNamedScript.Text = "Display Named Scripts Only";
            this.displayNamedScript.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(416, 212);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Paths";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ScriptPath);
            this.groupBox1.Location = new System.Drawing.Point(9, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 163);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Default Pats";
            // 
            // ScriptPath
            // 
            this.ScriptPath.Location = new System.Drawing.Point(17, 19);
            this.ScriptPath.Name = "ScriptPath";
            this.ScriptPath.Size = new System.Drawing.Size(364, 63);
            this.ScriptPath.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(453, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.button2.Location = new System.Drawing.Point(453, 52);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Abort";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // MainSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 250);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.MinimizeBox = false;
            this.Name = "MainSetup";
            this.ShowIcon = false;
            this.Text = "Main Setup";
            this.tabControl1.ResumeLayout(false);
            this.OnlyNamedScripts.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        public PathSelector ScriptPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TabPage OnlyNamedScripts;
        public System.Windows.Forms.CheckBox displayNamedScript;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox MainMDIStyle;
        public System.Windows.Forms.ComboBox MainFormStyle;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox bgSelect;
    }
}