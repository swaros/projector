using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Projector
{
    class ColorCalc
    {
        private Color baseCol = SystemColors.Control;
        
        public void setBaseColor(Color col)
        {
            this.baseCol = col;
        }

        public Color getDarker(byte amount)
        {
            
            byte R = this.darken(this.baseCol.R, amount);
            byte G = this.darken(this.baseCol.G, amount);
            byte B = this.darken(this.baseCol.B, amount);

            return Color.FromArgb(R, G, B);
        }

        public Color getLighter(byte amount)
        {

            byte R = this.lighten(this.baseCol.R, amount);
            byte G = this.lighten(this.baseCol.G, amount);
            byte B = this.lighten(this.baseCol.B, amount);

            return Color.FromArgb(R, G, B);
        }

        private byte darken(byte current, byte amount)
        {
            byte result = current;

            if (current < 2)
            {
                return current;
            }

            result = (byte) (current / amount);

            return result;
        }

        private byte lighten(byte current, byte amount)
        {
            byte result = current;

            if (current == 255)
            {
                return current;
            }
            byte diff = (byte)(255 - current);
            result = (byte)((diff / amount) + current);

            return result;
        }
    }
}
