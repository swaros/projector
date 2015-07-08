namespace Projector
{
    partial class PathSelector
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
            this.FolderPath = new System.Windows.Forms.TextBox();
            this.PathOpen = new System.Windows.Forms.Button();
            this.PathInfo = new System.Windows.Forms.Label();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // FolderPath
            // 
            this.FolderPath.Location = new System.Drawing.Point(4, 31);
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.Size = new System.Drawing.Size(242, 20);
            this.FolderPath.TabIndex = 0;
            this.FolderPath.TextChanged += new System.EventHandler(this.FolderPath_TextChanged);
            this.FolderPath.Leave += new System.EventHandler(this.FolderPath_Leave);
            // 
            // PathOpen
            // 
            this.PathOpen.Image = global::Projector.Properties.Resources.folder_open_16;
            this.PathOpen.Location = new System.Drawing.Point(253, 25);
            this.PathOpen.Name = "PathOpen";
            this.PathOpen.Size = new System.Drawing.Size(30, 31);
            this.PathOpen.TabIndex = 1;
            this.PathOpen.UseVisualStyleBackColor = true;
            this.PathOpen.Click += new System.EventHandler(this.PathOpen_Click);
            // 
            // PathInfo
            // 
            this.PathInfo.AutoSize = true;
            this.PathInfo.Location = new System.Drawing.Point(4, 12);
            this.PathInfo.Name = "PathInfo";
            this.PathInfo.Size = new System.Drawing.Size(19, 13);
            this.PathInfo.TabIndex = 2;
            this.PathInfo.Text = "....";
            // 
            // PathSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PathInfo);
            this.Controls.Add(this.PathOpen);
            this.Controls.Add(this.FolderPath);
            this.Name = "PathSelector";
            this.Size = new System.Drawing.Size(286, 63);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FolderPath;
        private System.Windows.Forms.Button PathOpen;
        private System.Windows.Forms.Label PathInfo;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
    }
}
