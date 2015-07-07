namespace Projector
{
    partial class ConfimSync
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfimSync));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.confirmOverWrite = new System.Windows.Forms.CheckedListBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.confirmOverWrite);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cancelBtn);
            this.splitContainer1.Panel2.Controls.Add(this.okBtn);
            this.splitContainer1.Size = new System.Drawing.Size(686, 477);
            this.splitContainer1.SplitterDistance = 494;
            this.splitContainer1.TabIndex = 0;
            // 
            // confirmOverWrite
            // 
            this.confirmOverWrite.CheckOnClick = true;
            this.confirmOverWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.confirmOverWrite.FormattingEnabled = true;
            this.confirmOverWrite.Location = new System.Drawing.Point(0, 0);
            this.confirmOverWrite.MultiColumn = true;
            this.confirmOverWrite.Name = "confirmOverWrite";
            this.confirmOverWrite.Size = new System.Drawing.Size(494, 477);
            this.confirmOverWrite.TabIndex = 0;
            this.confirmOverWrite.ThreeDCheckBoxes = true;
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(21, 13);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(155, 31);
            this.okBtn.TabIndex = 0;
            this.okBtn.Text = "&OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(21, 50);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(155, 35);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "&Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // ConfimSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 477);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfimSync";
            this.Text = "ConfimSync";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.CheckedListBox confirmOverWrite;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;

    }
}