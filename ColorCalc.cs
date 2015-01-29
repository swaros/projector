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

        private int percent(int percent, int max)
        {
            if (percent > 100)            
                return max;

            if (percent < 0)
                return 0;

            int p = (percent / 100) * max;
            return p;

        }

        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        public Color LightenBy( int percent)
        {
            return ChangeColorBrightness(this.baseCol,(float) (percent / 100.0));
        }

        public Color DarkenBy( int percent)
        {
            return ChangeColorBrightness(this.baseCol,(float) (-1 * percent / 100.0));
        }

        public Color PaperLight(int percent)
        {
            return this.whiteBlend(this.LightenBy(percent));
        }

        public Color PaperDark(int percent)
        {
            return this.whiteBlend(this.DarkenBy(percent));
        }

        private Color whiteBlend(Color refCol)
        {            
            int R = calcBlend(255, refCol.R);
            int B = calcBlend(255, refCol.B);
            int G = calcBlend(255, refCol.G);

            return Color.FromArgb(R, G, B);
        }

        private int calcBlend(byte valBase, byte valFak)
        {
            float percent = (float)100 / (float)valBase;
            float diff = valBase - valFak;
            float offset = diff * percent;
            float val = valBase - offset;
            return (int) Math.Round(val);
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
