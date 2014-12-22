using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Projector
{
    public class RichBoxMarker
    {
        public Color ForeColor;
        public Point topLeft;
        public Point BottomRight;
        public int firstCharIndex = 0;
        public int lastCharIndex = 0;
        public int displayedTimes = 1;
        
    }
}
