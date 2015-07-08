namespace Projector
{
    partial class StyleForm
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
            this.SuspendLayout();
            // 
            // StyleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 314);
            this.Name = "StyleForm";
            this.Text = "StyleForm";
            this.Activated += new System.EventHandler(this.StyleForm_Activated);
            this.Leave += new System.EventHandler(this.StyleForm_Leave);
            this.MouseCaptureChanged += new System.EventHandler(this.StyleForm_MouseCaptureChanged);
            this.MouseLeave += new System.EventHandler(this.StyleForm_MouseLeave);
            this.MouseHover += new System.EventHandler(this.StyleForm_MouseHover);
            this.ResumeLayout(false);

        }

        #endregion
    }
}