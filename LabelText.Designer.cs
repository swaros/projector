namespace Projector
{
    partial class LabelText
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextLabel = new System.Windows.Forms.Label();
            this.FlowControll = new System.Windows.Forms.FlowLayoutPanel();
            this.TextBox = new System.Windows.Forms.TextBox();
            this.FlowControll.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextLabel
            // 
            this.TextLabel.AutoSize = true;
            this.TextLabel.Location = new System.Drawing.Point(3, 0);
            this.TextLabel.Name = "TextLabel";
            this.TextLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.TextLabel.Size = new System.Drawing.Size(33, 18);
            this.TextLabel.TabIndex = 0;
            this.TextLabel.Text = "Label";
            // 
            // FlowControll
            // 
            this.FlowControll.AutoSize = true;
            this.FlowControll.Controls.Add(this.TextLabel);
            this.FlowControll.Controls.Add(this.TextBox);
            this.FlowControll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FlowControll.Location = new System.Drawing.Point(0, 0);
            this.FlowControll.Name = "FlowControll";
            this.FlowControll.Size = new System.Drawing.Size(145, 26);
            this.FlowControll.TabIndex = 1;
            // 
            // TextBox
            // 
            this.TextBox.Location = new System.Drawing.Point(42, 3);
            this.TextBox.Name = "TextBox";
            this.TextBox.Size = new System.Drawing.Size(100, 20);
            this.TextBox.TabIndex = 1;
            this.TextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // LabelText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.FlowControll);
            this.Name = "LabelText";
            this.Size = new System.Drawing.Size(145, 26);
            this.FlowControll.ResumeLayout(false);
            this.FlowControll.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label TextLabel;
        public System.Windows.Forms.FlowLayoutPanel FlowControll;
        public System.Windows.Forms.TextBox TextBox;
    }
}
