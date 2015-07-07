namespace Projector
{
    partial class ProfilButton
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
            this.HeadLabel = new System.Windows.Forms.Label();
            this.colorPanel = new System.Windows.Forms.Panel();
            this.Description = new System.Windows.Forms.Label();
            this.StartBtn = new System.Windows.Forms.Button();
            this.animTimer = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.unFlushTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // HeadLabel
            // 
            this.HeadLabel.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeadLabel.ForeColor = System.Drawing.Color.White;
            this.HeadLabel.Location = new System.Drawing.Point(7, 7);
            this.HeadLabel.Name = "HeadLabel";
            this.HeadLabel.Size = new System.Drawing.Size(310, 25);
            this.HeadLabel.TabIndex = 0;
            this.HeadLabel.Text = "Profil";
            this.HeadLabel.Click += new System.EventHandler(this.Description_Click);
            this.HeadLabel.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProfilButton_DragEnter);
            this.HeadLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HeadLabel_MouseClick);
            this.HeadLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorPanel_MouseDown);
            this.HeadLabel.MouseLeave += new System.EventHandler(this.ProfilButton_MouseLeave);
            this.HeadLabel.MouseHover += new System.EventHandler(this.ProfilButton_MouseHover);
            // 
            // colorPanel
            // 
            this.colorPanel.BackColor = System.Drawing.Color.LightSkyBlue;
            this.colorPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.colorPanel.Location = new System.Drawing.Point(0, 0);
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new System.Drawing.Size(10, 83);
            this.colorPanel.TabIndex = 1;
            this.colorPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorPanel_MouseDown);
            // 
            // Description
            // 
            this.Description.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Description.ForeColor = System.Drawing.Color.White;
            this.Description.Location = new System.Drawing.Point(12, 32);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(210, 48);
            this.Description.TabIndex = 2;
            this.Description.Text = "Some Informations";
            this.Description.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.Description.Click += new System.EventHandler(this.Description_Click);
            this.Description.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProfilButton_DragEnter);
            this.Description.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorPanel_MouseDown);
            this.Description.MouseLeave += new System.EventHandler(this.ProfilButton_MouseLeave);
            this.Description.MouseHover += new System.EventHandler(this.ProfilButton_MouseHover);
            // 
            // StartBtn
            // 
            this.StartBtn.BackColor = System.Drawing.Color.Gray;
            this.StartBtn.FlatAppearance.BorderSize = 0;
            this.StartBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartBtn.ForeColor = System.Drawing.Color.White;
            this.StartBtn.Image = global::Projector.Properties.Resources.voyager_badge;
            this.StartBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StartBtn.Location = new System.Drawing.Point(273, 39);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(44, 41);
            this.StartBtn.TabIndex = 3;
            this.StartBtn.Text = "Start";
            this.StartBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.StartBtn.UseVisualStyleBackColor = false;
            this.StartBtn.MouseLeave += new System.EventHandler(this.ProfilButton_MouseLeave);
            this.StartBtn.MouseHover += new System.EventHandler(this.ProfilButton_MouseHover);
            // 
            // animTimer
            // 
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Gray;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::Projector.Properties.Resources.communicator;
            this.button1.Location = new System.Drawing.Point(215, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 41);
            this.button1.TabIndex = 4;
            this.button1.Text = "Setup";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // unFlushTimer
            // 
            this.unFlushTimer.Interval = 1500;
            this.unFlushTimer.Tick += new System.EventHandler(this.unFlushTimer_Tick);
            // 
            // ProfilButton
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.colorPanel);
            this.Controls.Add(this.HeadLabel);
            this.Name = "ProfilButton";
            this.Size = new System.Drawing.Size(320, 83);
            this.Click += new System.EventHandler(this.Description_Click);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProfilButton_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProfilButton_DragEnter);
            this.DoubleClick += new System.EventHandler(this.ProfilButton_DoubleClick);
            this.Enter += new System.EventHandler(this.ProfilButton_Enter);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HeadLabel_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorPanel_MouseDown);
            this.MouseLeave += new System.EventHandler(this.ProfilButton_MouseLeave);
            this.MouseHover += new System.EventHandler(this.ProfilButton_MouseHover);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label HeadLabel;
        private System.Windows.Forms.Panel colorPanel;
        public System.Windows.Forms.Label Description;
        public System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Timer animTimer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer unFlushTimer;
    }
}
