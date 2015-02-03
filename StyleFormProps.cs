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
        public const string STYLE_DEFAULT = "Default";
        public const string STYLE_LIGHT_COLOR = "Color Lighten";
        public const string STYLE_DARK_COLOR = "Color Darken";
        public const string STYLE_NIGHT = "Night";
        public const string STYLE_PAPER = "Paper";


        private string currentStyle = StyleFormProps.STYLE_DEFAULT;
        private Boolean changeStyle = false;

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

        /// <summary>
        /// returns all possible style identifier
        /// </summary>
        /// <returns></returns>
        public static List<String> getAllStyles()
        {
            List<string> result = new List<string>();
            result.Add(StyleFormProps.STYLE_DEFAULT);
            result.Add(StyleFormProps.STYLE_LIGHT_COLOR);
            result.Add(StyleFormProps.STYLE_DARK_COLOR);
            result.Add(StyleFormProps.STYLE_NIGHT);
            result.Add(StyleFormProps.STYLE_PAPER);
            return result;
        }

        public Boolean containsChanges()
        {
            return this.changeStyle;
        }


        public void composeStyle(string name)
        {
            List<string> possibles = StyleFormProps.getAllStyles();
            this.changeStyle = false;
            if (possibles.Contains(name))
            {
                this.currentStyle = name;
                this.changeStyle = true;
                switch (name)
                {
                        
                    case StyleFormProps.STYLE_DEFAULT:
                        this.changeStyle = false;
                        break;
                    case StyleFormProps.STYLE_LIGHT_COLOR:
                        this.composeFlatLight();
                        break;
                    case StyleFormProps.STYLE_NIGHT:
                        this.composeNight();
                        break;
                    case StyleFormProps.STYLE_DARK_COLOR:
                        this.composeFlatDark();
                        break;
                    case StyleFormProps.STYLE_PAPER:
                        this.composePaper();
                        break;
                }
            }
        }


        public void composeNight()
        {
  
            this.BackgroundControlColor = Color.FromArgb(90,90,90);
            this.ForeGroundContentColor = Color.FromArgb(57, 191, 248);
            this.ButtonBackColor = Color.FromArgb(143,164,179);
            this.ButtonForeColor = Color.FromArgb(225,234,240);
            this.LineColor = Color.FromArgb(132,198,245);
            this.ButtonStyle = FlatStyle.Flat;
            this.ElBackColor = Color.FromArgb(120,120,120);
            this.ELForeColor = Color.FromArgb(57, 191, 248);
            this.ElBorderStyle = BorderStyle.FixedSingle;
            this.itemRowA = Color.FromArgb(60, 60, 60);
            this.itemRowB = Color.FromArgb(35,35,35);
            this.itemTextColor = Color.FromArgb(57, 191, 248);

        }

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
