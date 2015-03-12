namespace Projector
{
    partial class ScriptComponent
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ToolImage = new System.Windows.Forms.PictureBox();
            this.NameOfTool = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ToolImage)).BeginInit();
            this.SuspendLayout();
            // 
            // ToolImage
            // 
            this.ToolImage.Image = global::Projector.Properties.Resources.url;
            this.ToolImage.InitialImage = global::Projector.Properties.Resources.applications_161;
            this.ToolImage.Location = new System.Drawing.Point(0, 0);
            this.ToolImage.Name = "ToolImage";
            this.ToolImage.Size = new System.Drawing.Size(16, 16);
            this.ToolImage.TabIndex = 0;
            this.ToolImage.TabStop = false;
            this.ToolImage.DoubleClick += new System.EventHandler(this.ToolImage_DoubleClick);
            // 
            // NameOfTool
            // 
            this.NameOfTool.AutoSize = true;
            this.NameOfTool.Location = new System.Drawing.Point(22, 0);
            this.NameOfTool.Name = "NameOfTool";
            this.NameOfTool.Size = new System.Drawing.Size(76, 13);
            this.NameOfTool.TabIndex = 1;
            this.NameOfTool.Text = "COMPONENT";
            // 
            // ScriptComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NameOfTool);
            this.Controls.Add(this.ToolImage);
            this.Name = "ScriptComponent";
            this.Size = new System.Drawing.Size(133, 21);
            ((System.ComponentModel.ISupportInitialize)(this.ToolImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox ToolImage;
        public System.Windows.Forms.Label NameOfTool;

    }
}
