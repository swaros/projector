namespace Projector
{
    partial class StructDiffWindow
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.DiffView = new System.Windows.Forms.ListView();
            this.Source = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Target = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1.Panel1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.DiffView);
            this.splitContainer1.Size = new System.Drawing.Size(836, 608);
            this.splitContainer1.SplitterDistance = 406;
            this.splitContainer1.TabIndex = 0;
            // 
            // DiffView
            // 
            this.DiffView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Source,
            this.Target});
            this.DiffView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DiffView.Location = new System.Drawing.Point(0, 0);
            this.DiffView.Name = "DiffView";
            this.DiffView.Size = new System.Drawing.Size(406, 608);
            this.DiffView.TabIndex = 0;
            this.DiffView.UseCompatibleStateImageBehavior = false;
            this.DiffView.View = System.Windows.Forms.View.Details;
            this.DiffView.SelectedIndexChanged += new System.EventHandler(this.DiffView_SelectedIndexChanged);
            // 
            // Source
            // 
            this.Source.Text = "Source";
            // 
            // Target
            // 
            this.Target.Text = "Target";
            // 
            // StructDiffWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 608);
            this.Controls.Add(this.splitContainer1);
            this.Name = "StructDiffWindow";
            this.Text = "StructDiffWindow";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView DiffView;
        private System.Windows.Forms.ColumnHeader Source;
        private System.Windows.Forms.ColumnHeader Target;
    }
}