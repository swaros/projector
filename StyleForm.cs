using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Projector
{
    public partial class StyleForm : Form
    {

        public StyleFormProps currentStyle;

        private Boolean FlatMode = false;

        public StyleForm()
        {
            InitializeComponent();
        }

        public void setStyle(StyleFormProps Style)
        {
            this.BackColor = Style.BackgroundControlColor;
            this.ForeColor = Style.ForeGroundContentColor;
            this.currentStyle = Style;
            this.FlatMode = (this.currentStyle.ButtonStyle == FlatStyle.Flat) ;
            this.updateContent(Style);

           
        }

        public void updateContent(Control.ControlCollection controlCollection)
        {
            if (this.currentStyle != null)
            {
                this.updateContent(this.currentStyle, controlCollection);
            }
        }

        private void updateContent(StyleFormProps Style)
        {      
            
            this.updateContent(Style, this.Controls);
        }

        private void updateContent(StyleFormProps Style, Control.ControlCollection controlCollection)
        {
            for (int i = 0; i < controlCollection.Count; i++)
            {              
                controlCollection[i].BackColor = Style.BackgroundControlColor;
                controlCollection[i].ForeColor = Style.ForeGroundContentColor;


                

                if (controlCollection[i] is Button)
                {
                    Button tmpBtn = (Button)controlCollection[i];
                    tmpBtn.FlatStyle = Style.ButtonStyle;
                    tmpBtn.BackColor = Style.ButtonBackColor;
                    tmpBtn.ForeColor = Style.ButtonForeColor;
                    if (tmpBtn.FlatStyle == FlatStyle.Flat)
                    {
                        tmpBtn.FlatAppearance.BorderColor = Style.ButtonForeColor;
                    }
                }
                
                if (controlCollection[i] is TextBoxBase)
                {
                    TextBoxBase tmpEl = (TextBoxBase)controlCollection[i];
                    tmpEl.BorderStyle = Style.ElBorderStyle;
                    tmpEl.BackColor = Style.ElBackColor;
                    tmpEl.ForeColor = Style.ELForeColor;
                }

                if (controlCollection[i] is ComboBox)
                {
                    ComboBox tmpEl = (ComboBox)controlCollection[i];
                    tmpEl.FlatStyle = Style.ButtonStyle;
                    tmpEl.BackColor = Style.ElBackColor;
                    tmpEl.ForeColor = Style.ELForeColor;
                }

                if (controlCollection[i] is GroupBox)
                {
                    GroupBox tmpEl = (GroupBox)controlCollection[i];
                    tmpEl.FlatStyle = Style.ButtonStyle;                    
                    tmpEl.BackColor = Style.BackgroundControlColor;
                    tmpEl.ForeColor = Style.ForeGroundContentColor;
                    
                }

                if (controlCollection[i] is ListView)
                {
                    ListView tmpEl = (ListView)controlCollection[i];
                    tmpEl.BorderStyle = Style.ElBorderStyle;
                    tmpEl.BackColor = Style.ElBackColor;
                    tmpEl.ForeColor = Style.ELForeColor; 
                    
                }

                if (controlCollection[i].Controls.Count > 0)
                {
                    this.updateContent(Style, controlCollection[i].Controls);
                }
            }
        }

        private void StyleForm_MouseCaptureChanged(object sender, EventArgs e)
        {
            
        }

        private void StyleForm_MouseHover(object sender, EventArgs e)
        {
          
        }

        private void StyleForm_MouseLeave(object sender, EventArgs e)
        {
           
        }

        private void StyleForm_Activated(object sender, EventArgs e)
        {
            
        }

        private void StyleForm_Leave(object sender, EventArgs e)
        {
            
        }


    }
}
