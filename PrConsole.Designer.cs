namespace Projector
{
    partial class PrConsole
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
            this.Console = new System.Windows.Forms.ListBox();
            this.ShowDetailLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Console
            // 
            this.Console.BackColor = System.Drawing.SystemColors.Info;
            this.Console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Console.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Console.FormattingEnabled = true;
            this.Console.ItemHeight = 16;
            this.Console.Items.AddRange(new object[] {
            "Projector Output Console"});
            this.Console.Location = new System.Drawing.Point(0, 0);
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(355, 315);
            this.Console.TabIndex = 0;
            this.Console.SelectedIndexChanged += new System.EventHandler(this.Console_SelectedIndexChanged);
            this.Console.DoubleClick += new System.EventHandler(this.Console_DoubleClick);
            // 
            // ShowDetailLabel
            // 
            this.ShowDetailLabel.AutoSize = true;
            this.ShowDetailLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ShowDetailLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ShowDetailLabel.Location = new System.Drawing.Point(214, 0);
            this.ShowDetailLabel.Name = "ShowDetailLabel";
            this.ShowDetailLabel.Size = new System.Drawing.Size(141, 15);
            this.ShowDetailLabel.TabIndex = 1;
            this.ShowDetailLabel.Text = "Click on a Item to show this ";
            this.ShowDetailLabel.Click += new System.EventHandler(this.ShowDetailLabel_Click);
            // 
            // PrConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ShowDetailLabel);
            this.Controls.Add(this.Console);
            this.Name = "PrConsole";
            this.Size = new System.Drawing.Size(355, 315);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox Console;
        private System.Windows.Forms.Label ShowDetailLabel;

    }
}
