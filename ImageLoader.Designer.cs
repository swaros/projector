namespace Projector
{
    partial class ImageLoader
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
            this.loadImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadImage)).BeginInit();
            this.SuspendLayout();
            // 
            // loadImage
            // 
            this.loadImage.Location = new System.Drawing.Point(3, 3);
            this.loadImage.Name = "loadImage";
            this.loadImage.Size = new System.Drawing.Size(100, 50);
            this.loadImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.loadImage.TabIndex = 0;
            this.loadImage.TabStop = false;
            // 
            // ImageLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.loadImage);
            this.Name = "ImageLoader";
            this.Size = new System.Drawing.Size(106, 56);
            ((System.ComponentModel.ISupportInitialize)(this.loadImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox loadImage;
    }
}
