using System;
using System.Collections;
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

        private Hashtable notTouchableForeColor = new Hashtable();
        private Hashtable notTouchableBackColor = new Hashtable();

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

        public void forgetUntoucedColors(){
            this.notTouchableForeColor.Clear();
            this.notTouchableBackColor.Clear();
        }


        private string getColorString(Color col)
        {
            return col.A.ToString() + ":" + col.R + ":" + col.G + ":" + col.B;
        }

        public void setNotTouchedForeColor(Color col)
        {
            if (!this.notTouchableForeColor.ContainsKey(getColorString(col)))
                this.notTouchableForeColor.Add(getColorString(col), col);
        }

        public void setNotTouchedBackColor(Color col)
        {
            if (!this.notTouchableBackColor.ContainsKey(getColorString(col)))
                this.notTouchableBackColor.Add(getColorString(col), col);
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
                Boolean foreColChangeable = !this.notTouchableForeColor.ContainsKey(getColorString(controlCollection[i].ForeColor));
                Boolean backColChangeable = !this.notTouchableBackColor.ContainsKey(getColorString(controlCollection[i].BackColor));

                if (backColChangeable)
                    controlCollection[i].BackColor = Style.BackgroundControlColor;
                if (foreColChangeable)
                    controlCollection[i].ForeColor = Style.ForeGroundContentColor;


                

                if (controlCollection[i] is Button)
                {
                    Button tmpBtn = (Button)controlCollection[i];
                    tmpBtn.FlatStyle = Style.ButtonStyle;
                    if (backColChangeable)
                        tmpBtn.BackColor = Style.ButtonBackColor;
                    if (backColChangeable)
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
                    if (backColChangeable)
                        tmpEl.BackColor = Style.ElBackColor;
                    if (foreColChangeable)
                        tmpEl.ForeColor = Style.ELForeColor;
                }

                if (controlCollection[i] is ComboBox)
                {
                    ComboBox tmpEl = (ComboBox)controlCollection[i];
                    tmpEl.FlatStyle = Style.ButtonStyle;
                    if (backColChangeable)
                        tmpEl.BackColor = Style.ElBackColor;
                    if (foreColChangeable)
                        tmpEl.ForeColor = Style.ELForeColor;
                }

                if (controlCollection[i] is GroupBox)
                {
                    GroupBox tmpEl = (GroupBox)controlCollection[i];
                    tmpEl.FlatStyle = Style.ButtonStyle;                    
                    if (backColChangeable)
                        tmpEl.BackColor = Style.BackgroundControlColor;
                    if (foreColChangeable)
                        tmpEl.ForeColor = Style.ForeGroundContentColor;
                    
                }

                if (controlCollection[i] is ListView)
                {
                    ListView tmpEl = (ListView)controlCollection[i];
                    tmpEl.BorderStyle = Style.ElBorderStyle;
                    if (backColChangeable)
                        tmpEl.BackColor = Style.ElBackColor;
                    if (foreColChangeable)
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
