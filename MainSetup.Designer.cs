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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.displayNamedScript = new System.Windows.Forms.CheckBox();
            this.ScriptPath = new Projector.PathSelector();
            this.tabControl1.SuspendLayout();
            this.OnlyNamedScripts.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.OnlyNamedScripts.Controls.Add(this.groupBox2);
            this.OnlyNamedScripts.Location = new System.Drawing.Point(4, 22);
            this.OnlyNamedScripts.Name = "OnlyNamedScripts";
            this.OnlyNamedScripts.Padding = new System.Windows.Forms.Padding(3);
            this.OnlyNamedScripts.Size = new System.Drawing.Size(416, 212);
            this.OnlyNamedScripts.TabIndex = 0;
            this.OnlyNamedScripts.Text = "Main Settings";
            this.OnlyNamedScripts.UseVisualStyleBackColor = true;
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.displayNamedScript);
            this.groupBox2.Location = new System.Drawing.Point(9, 7);
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
            // ScriptPath
            // 
            this.ScriptPath.Location = new System.Drawing.Point(17, 19);
            this.ScriptPath.Name = "ScriptPath";
            this.ScriptPath.Size = new System.Drawing.Size(364, 63);
            this.ScriptPath.TabIndex = 0;
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
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
    }
}