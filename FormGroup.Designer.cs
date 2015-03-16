namespace Projector
{
    partial class FormGroup
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
            this.formGroupBox = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // formGroupBox
            // 
            this.formGroupBox.AutoSize = true;
            this.formGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formGroupBox.Location = new System.Drawing.Point(0, 0);
            this.formGroupBox.Name = "formGroupBox";
            this.formGroupBox.Size = new System.Drawing.Size(150, 150);
            this.formGroupBox.TabIndex = 0;
            this.formGroupBox.TabStop = false;
            this.formGroupBox.Text = "UnNamed";
            // 
            // FormGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.formGroupBox);
            this.Name = "FormGroup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox formGroupBox;
    }
}
