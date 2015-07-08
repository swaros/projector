namespace Projector
{
    partial class ProfilGroupBox
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.nameChange = new System.Windows.Forms.TextBox();
            this.grpName = new System.Windows.Forms.Label();
            this.grpFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.OpenLabel = new System.Windows.Forms.Label();
            this.clickWatcher = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            this.grpFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(16, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 68);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.OrangeRed;
            this.panel2.Controls.Add(this.nameChange);
            this.panel2.Controls.Add(this.grpName);
            this.panel2.Location = new System.Drawing.Point(9, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 67);
            this.panel2.TabIndex = 1;
            // 
            // nameChange
            // 
            this.nameChange.Location = new System.Drawing.Point(7, 25);
            this.nameChange.Name = "nameChange";
            this.nameChange.Size = new System.Drawing.Size(190, 20);
            this.nameChange.TabIndex = 1;
            this.nameChange.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameChange_KeyDown);
            // 
            // grpName
            // 
            this.grpName.AllowDrop = true;
            this.grpName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpName.ForeColor = System.Drawing.Color.White;
            this.grpName.Image = global::Projector.Properties.Resources.folder_601;
            this.grpName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.grpName.Location = new System.Drawing.Point(0, 0);
            this.grpName.Name = "grpName";
            this.grpName.Size = new System.Drawing.Size(200, 67);
            this.grpName.TabIndex = 0;
            this.grpName.Text = "Unnamed";
            this.grpName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.grpName.Click += new System.EventHandler(this.grpName_Click);
            this.grpName.DragDrop += new System.Windows.Forms.DragEventHandler(this.grpFlow_DragDrop);
            this.grpName.DragEnter += new System.Windows.Forms.DragEventHandler(this.grpFlow_DragEnter);
            this.grpName.DoubleClick += new System.EventHandler(this.grpName_DoubleClick);
            this.grpName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grpName_MouseClick);
            // 
            // grpFlow
            // 
            this.grpFlow.AllowDrop = true;
            this.grpFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpFlow.Controls.Add(this.OpenLabel);
            this.grpFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.grpFlow.Location = new System.Drawing.Point(9, 86);
            this.grpFlow.Name = "grpFlow";
            this.grpFlow.Size = new System.Drawing.Size(759, 421);
            this.grpFlow.TabIndex = 2;
            this.grpFlow.DragDrop += new System.Windows.Forms.DragEventHandler(this.grpFlow_DragDrop);
            this.grpFlow.DragEnter += new System.Windows.Forms.DragEventHandler(this.grpFlow_DragEnter);
            // 
            // OpenLabel
            // 
            this.OpenLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenLabel.ForeColor = System.Drawing.Color.White;
            this.OpenLabel.Image = global::Projector.Properties.Resources.folder_601;
            this.OpenLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OpenLabel.Location = new System.Drawing.Point(3, 0);
            this.OpenLabel.Name = "OpenLabel";
            this.OpenLabel.Size = new System.Drawing.Size(307, 64);
            this.OpenLabel.TabIndex = 0;
            this.OpenLabel.Text = "label1";
            this.OpenLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.OpenLabel.Click += new System.EventHandler(this.OpenLabel_Click);
            // 
            // clickWatcher
            // 
            this.clickWatcher.Interval = 500;
            this.clickWatcher.Tick += new System.EventHandler(this.clickWatcher_Tick);
            // 
            // ProfilGroupBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.grpFlow);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.OrangeRed;
            this.Name = "ProfilGroupBox";
            this.Size = new System.Drawing.Size(771, 510);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.grpFlow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label grpName;
        public System.Windows.Forms.FlowLayoutPanel grpFlow;
        private System.Windows.Forms.Label OpenLabel;
        private System.Windows.Forms.TextBox nameChange;
        private System.Windows.Forms.Timer clickWatcher;
    }
}
