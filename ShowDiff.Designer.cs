namespace Projector
{
    partial class ShowDiff
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.leftText = new System.Windows.Forms.RichTextBox();
            this.rightText = new System.Windows.Forms.RichTextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.leftText);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rightText);
            this.splitContainer1.Size = new System.Drawing.Size(865, 481);
            this.splitContainer1.SplitterDistance = 401;
            this.splitContainer1.TabIndex = 0;
            // 
            // leftText
            // 
            this.leftText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftText.Location = new System.Drawing.Point(0, 0);
            this.leftText.Name = "leftText";
            this.leftText.Size = new System.Drawing.Size(401, 481);
            this.leftText.TabIndex = 0;
            this.leftText.Text = "";
            this.leftText.TextChanged += new System.EventHandler(this.leftText_TextChanged);
            // 
            // rightText
            // 
            this.rightText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rightText.Location = new System.Drawing.Point(0, 0);
            this.rightText.Name = "rightText";
            this.rightText.Size = new System.Drawing.Size(460, 481);
            this.rightText.TabIndex = 0;
            this.rightText.Text = "";
            this.rightText.TextChanged += new System.EventHandler(this.rightText_TextChanged);
            // 
            // ShowDiff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 481);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ShowDiff";
            this.Text = "ShowDiff";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.RichTextBox leftText;
        public System.Windows.Forms.RichTextBox rightText;
    }
}