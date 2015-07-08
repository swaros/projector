namespace Projector
{
    partial class ColorChoose
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
            this.colorBtn = new System.Windows.Forms.Button();
            this.colorSelectDialog = new System.Windows.Forms.ColorDialog();
            this.exampleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // colorBtn
            // 
            this.colorBtn.Location = new System.Drawing.Point(4, 4);
            this.colorBtn.Name = "colorBtn";
            this.colorBtn.Size = new System.Drawing.Size(99, 23);
            this.colorBtn.TabIndex = 0;
            this.colorBtn.Text = "Choose Color";
            this.colorBtn.UseVisualStyleBackColor = true;
            this.colorBtn.Click += new System.EventHandler(this.colorBtn_Click);
            // 
            // exampleLabel
            // 
            this.exampleLabel.AutoSize = true;
            this.exampleLabel.Location = new System.Drawing.Point(114, 9);
            this.exampleLabel.Name = "exampleLabel";
            this.exampleLabel.Size = new System.Drawing.Size(47, 13);
            this.exampleLabel.TabIndex = 1;
            this.exampleLabel.Text = "Example";
            this.exampleLabel.DoubleClick += new System.EventHandler(this.exampleLabel_DoubleClick);
            // 
            // ColorChoose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.exampleLabel);
            this.Controls.Add(this.colorBtn);
            this.Name = "ColorChoose";
            this.Size = new System.Drawing.Size(185, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button colorBtn;
        private System.Windows.Forms.ColorDialog colorSelectDialog;
        private System.Windows.Forms.Label exampleLabel;
    }
}
