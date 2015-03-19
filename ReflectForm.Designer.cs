namespace Projector
{
    partial class ReflectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReflectForm));
            this.httpWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // httpWorker
            // 
            this.httpWorker.WorkerReportsProgress = true;
            this.httpWorker.WorkerSupportsCancellation = true;
            this.httpWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.httpWorker_DoWork);
            this.httpWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.httpWorker_ProgressChanged);
            this.httpWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.httpWorker_RunWorkerCompleted);
            // 
            // ReflectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 224);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReflectForm";
            this.ShowIcon = false;
            this.Text = "Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReflectForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReflectForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker httpWorker;
    }
}