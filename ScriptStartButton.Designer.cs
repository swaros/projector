namespace Projector
{
    partial class ScriptStartButton
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MainLabel = new System.Windows.Forms.Label();
            this.StartBtn = new System.Windows.Forms.Button();
            this.EditBtn = new System.Windows.Forms.Button();
            this.DescLabel = new System.Windows.Forms.Label();
            this.runTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // MainLabel
            // 
            this.MainLabel.AutoSize = true;
            this.MainLabel.BackColor = System.Drawing.Color.Transparent;
            this.MainLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.MainLabel.Location = new System.Drawing.Point(0, 0);
            this.MainLabel.Name = "MainLabel";
            this.MainLabel.Size = new System.Drawing.Size(134, 24);
            this.MainLabel.TabIndex = 0;
            this.MainLabel.Text = "Unamed Script";
            // 
            // StartBtn
            // 
            this.StartBtn.Image = global::Projector.Properties.Resources.player_play_green;
            this.StartBtn.Location = new System.Drawing.Point(4, 28);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(43, 38);
            this.StartBtn.TabIndex = 2;
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // EditBtn
            // 
            this.EditBtn.Location = new System.Drawing.Point(238, 3);
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.Size = new System.Drawing.Size(33, 23);
            this.EditBtn.TabIndex = 3;
            this.EditBtn.Text = "Edit";
            this.EditBtn.UseVisualStyleBackColor = true;
            // 
            // DescLabel
            // 
            this.DescLabel.AutoSize = true;
            this.DescLabel.BackColor = System.Drawing.Color.Transparent;
            this.DescLabel.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DescLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.DescLabel.Location = new System.Drawing.Point(53, 28);
            this.DescLabel.Name = "DescLabel";
            this.DescLabel.Size = new System.Drawing.Size(73, 16);
            this.DescLabel.TabIndex = 4;
            this.DescLabel.Text = "Description";
            this.DescLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.DescLabel.MouseLeave += new System.EventHandler(this.DescLabel_MouseLeave);
            this.DescLabel.MouseHover += new System.EventHandler(this.DescLabel_MouseHover);
            // 
            // runTimer
            // 
            this.runTimer.Interval = 2000;
            this.runTimer.Tick += new System.EventHandler(this.runTimer_Tick);
            // 
            // ScriptStartButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BackgroundImage = global::Projector.Properties.Resources.yellow_bar;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.DescLabel);
            this.Controls.Add(this.EditBtn);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.MainLabel);
            this.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.Margin = new System.Windows.Forms.Padding(3, 3, 15, 15);
            this.MinimumSize = new System.Drawing.Size(270, 75);
            this.Name = "ScriptStartButton";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.Size = new System.Drawing.Size(274, 78);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MainLabel;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button EditBtn;
        private System.Windows.Forms.Label DescLabel;
        private System.Windows.Forms.Timer runTimer;
    }
}
