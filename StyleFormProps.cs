using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    public class StyleFormProps
    {
        public Color MainColor = Color.Gray;
        public Color BackgroundControlColor = SystemColors.Control;
        public Color ForeGroundContentColor = SystemColors.ControlText;
        public Color ButtonBackColor = SystemColors.Control;
        public Color ButtonForeColor = SystemColors.ControlText;
        public Color LineColor = SystemColors.ControlDark;
        public Color ElBackColor = SystemColors.Control;
        public Color ELForeColor = SystemColors.ControlText;
        public FlatStyle ButtonStyle = FlatStyle.Standard;
        public BorderStyle ElBorderStyle = BorderStyle.Fixed3D;
        public Color itemTextColor = SystemColors.ControlText;
        public Color itemRowA = Color.LightCoral;
        public Color itemRowB = Color.White;

        public void composePaper()
        {
            ColorCalc colorMix = new ColorCalc();
            colorMix.setBaseColor(this.MainColor);
            this.BackgroundControlColor = colorMix.PaperLight(45);
            this.ForeGroundContentColor = colorMix.DarkenBy(70);
            this.ButtonBackColor = colorMix.PaperDark(50);
            this.ButtonForeColor = colorMix.DarkenBy(80);
            this.LineColor = colorMix.DarkenBy(55);
            this.ButtonStyle = FlatStyle.Flat;
            this.ElBackColor = colorMix.PaperDark(50);
            this.ELForeColor = colorMix.DarkenBy(60);
            this.ElBorderStyle = BorderStyle.FixedSingle;
            this.itemRowA = colorMix.PaperDark(50);
            this.itemRowB = colorMix.PaperLight(50);
            this.itemTextColor = colorMix.DarkenBy(70);

        }


        public void composeFlatLight()
        {
            ColorCalc colorMix = new ColorCalc();
            colorMix.setBaseColor(this.MainColor);
            this.BackgroundControlColor = colorMix.LightenBy(45);
            this.ForeGroundContentColor = colorMix.DarkenBy(70);
            this.ButtonBackColor = colorMix.LightenBy(50);
            this.ButtonForeColor = colorMix.DarkenBy(50);
            this.LineColor = colorMix.DarkenBy(40);
            this.ButtonStyle = FlatStyle.Flat;
            this.ElBackColor = colorMix.DarkenBy(50);
            this.ELForeColor = colorMix.LightenBy(60);
            this.ElBorderStyle = BorderStyle.FixedSingle;
            this.itemRowA = colorMix.DarkenBy(60);
            this.itemRowB = colorMix.DarkenBy(40);
            this.itemTextColor = colorMix.LightenBy(70);
        }

        public void composeFlatDark()
        {
            ColorCalc colorMix = new ColorCalc();
            colorMix.setBaseColor(this.MainColor);
            this.BackgroundControlColor = colorMix.DarkenBy(45);
            this.ForeGroundContentColor = colorMix.LightenBy(70);
            this.ButtonBackColor = colorMix.DarkenBy(50);
            this.ButtonForeColor = colorMix.LightenBy(50);
            this.LineColor = colorMix.LightenBy(40);
            this.ButtonStyle = FlatStyle.Flat;
            this.ElBackColor = colorMix.LightenBy(50);
            this.ELForeColor = colorMix.DarkenBy(60);
            this.ElBorderStyle = BorderStyle.FixedSingle;
            this.itemRowA = colorMix.LightenBy(60);
            this.itemRowB = colorMix.LightenBy(40);
            this.itemTextColor = colorMix.DarkenBy(70);
        }
    }
}
