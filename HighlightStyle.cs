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
        public Font Font = new Font("Courier New", 8, FontStyle.Regular);
    }
}
