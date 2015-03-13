namespace Projector
{
    partial class DiffSensor
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
            this.sensorPanel = new Projector.PanelDrawing();
            this.SuspendLayout();
            // 
            // sensorPanel
            // 
            this.sensorPanel.BackColor = System.Drawing.Color.Black;
            this.sensorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sensorPanel.Location = new System.Drawing.Point(0, 0);
            this.sensorPanel.Name = "sensorPanel";
            this.sensorPanel.Size = new System.Drawing.Size(324, 102);
            this.sensorPanel.TabIndex = 0;
            // 
            // DiffSensor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sensorPanel);
            this.Name = "DiffSensor";
            this.Size = new System.Drawing.Size(324, 102);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelDrawing sensorPanel;
    }
}
