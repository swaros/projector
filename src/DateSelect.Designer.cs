namespace Projector
{
    partial class DateSelect
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
            this.selectdate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.HoureBox = new System.Windows.Forms.ComboBox();
            this.MinBox = new System.Windows.Forms.ComboBox();
            this.SecBox = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectdate
            // 
            this.selectdate.Location = new System.Drawing.Point(39, 3);
            this.selectdate.Name = "selectdate";
            this.selectdate.Size = new System.Drawing.Size(200, 20);
            this.selectdate.TabIndex = 0;
            this.selectdate.ValueChanged += new System.EventHandler(this.selectdate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label1.Size = new System.Drawing.Size(30, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Date";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.selectdate);
            this.flowLayoutPanel1.Controls.Add(this.HoureBox);
            this.flowLayoutPanel1.Controls.Add(this.MinBox);
            this.flowLayoutPanel1.Controls.Add(this.SecBox);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(380, 27);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // HoureBox
            // 
            this.HoureBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HoureBox.FormattingEnabled = true;
            this.HoureBox.Location = new System.Drawing.Point(245, 3);
            this.HoureBox.Name = "HoureBox";
            this.HoureBox.Size = new System.Drawing.Size(40, 21);
            this.HoureBox.TabIndex = 2;
            this.HoureBox.SelectedIndexChanged += new System.EventHandler(this.HoureBox_SelectedIndexChanged);
            // 
            // MinBox
            // 
            this.MinBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MinBox.FormattingEnabled = true;
            this.MinBox.Location = new System.Drawing.Point(291, 3);
            this.MinBox.Name = "MinBox";
            this.MinBox.Size = new System.Drawing.Size(40, 21);
            this.MinBox.TabIndex = 3;
            this.MinBox.SelectedIndexChanged += new System.EventHandler(this.MinBox_SelectedIndexChanged);
            // 
            // SecBox
            // 
            this.SecBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SecBox.FormattingEnabled = true;
            this.SecBox.Location = new System.Drawing.Point(337, 3);
            this.SecBox.Name = "SecBox";
            this.SecBox.Size = new System.Drawing.Size(40, 21);
            this.SecBox.TabIndex = 4;
            this.SecBox.SelectedIndexChanged += new System.EventHandler(this.SecBox_SelectedIndexChanged);
            // 
            // DateSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "DateSelect";
            this.Size = new System.Drawing.Size(386, 33);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker selectdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox HoureBox;
        private System.Windows.Forms.ComboBox MinBox;
        private System.Windows.Forms.ComboBox SecBox;
    }
}
