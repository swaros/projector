using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Projector
{
    
    public class HighlightStyle
    {
        public static Color defaultColor = Color.White;

        public Color ForeColor = Color.Gray;
        public Color BackColor = HighlightStyle.defaultColor;
        public Font Font = new Font("Courier New", 10, FontStyle.Regular);

        public String toSetupValue()
        {
            String setval = this.ForeColor.ToArgb().ToString();
            setval += ";";
            setval += this.BackColor.ToArgb().ToString() + ";";
            setval += this.Font.Name + ";";
            setval += this.Font.Size.ToString();

            return setval;
        }

        public Boolean getStyleFromvalueString(string value)
        {
            string[] values = value.Split(';');
            if (values.Length >= 4)
            {
                int fCol = int.Parse(values[0]);
                int bCol = int.Parse(values[1]);
                string FontName = values[2];
                int fHeight = int.Parse(values[3]);
                this.ForeColor = Color.FromArgb(fCol);
                this.BackColor = Color.FromArgb(bCol);
                this.Font = new Font(FontName, fHeight, FontStyle.Regular);
                return true;
            }
            return false;
        }
    }
}
