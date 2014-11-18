using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Projector
{
    class ReflectionScriptHighLight
    {
        private RtfColoring RtfColors;
        private ReflectionScript Srcipt;

        private Hashtable KeyWords = new Hashtable();       

        private RichTextBox assignedRtf;
        private RichTextBox drawingRtf;

        private HighlightStyle ObjectStyle = new HighlightStyle();
        private HighlightStyle CommandStyle = new HighlightStyle();
        private HighlightStyle ReferenzStyle = new HighlightStyle();
        private HighlightStyle VarStyle = new HighlightStyle();
        private HighlightStyle TextStyle = new HighlightStyle();

        private int fontDefaultSize = 8;
        private string defaultFontName = "Arial";

        private Boolean elementsReaded = false;

        public ReflectionScriptHighLight(ReflectionScript script, RichTextBox rtf){
            this.Srcipt = script;
            this.assignedRtf = rtf;
            this.drawingRtf = new RichTextBox();
            drawingRtf.Rtf = assignedRtf.Rtf;
            this.RtfColors = new RtfColoring(drawingRtf);


            this.ObjectStyle.ForeColor = Color.DarkMagenta;
            this.ObjectStyle.Font = new Font(defaultFontName,this.fontDefaultSize,FontStyle.Italic);

            this.CommandStyle.ForeColor = Color.DarkGreen;
            this.CommandStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold);


            this.ReferenzStyle.ForeColor = Color.DarkGoldenrod;
            this.ReferenzStyle.Font = new Font(defaultFontName, this.fontDefaultSize , FontStyle.Bold);

            this.VarStyle.ForeColor = Color.DarkOrange;
            this.VarStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);

            this.TextStyle.ForeColor = Color.SlateBlue;
            //this.TextStyle.BackColor = Color.LightSlateGray;
            
            this.TextStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);
        }

        public void reDraw(Boolean reNewElements)
        {
            this.drawingRtf.Rtf = this.assignedRtf.Rtf;
            int startpos = this.assignedRtf.SelectionStart;
            int endPos = this.assignedRtf.SelectionLength;           

            if (reNewElements == true || this.elementsReaded == false)
            {
                this.getElements();
            }

            foreach (DictionaryEntry de in this.KeyWords)
            {
                string word = de.Key.ToString();
                HighlightStyle setColor = (HighlightStyle)de.Value;
                this.RtfColors.markWordsAll(word, setColor);
            }

            foreach (DictionaryEntry de in this.Srcipt.getAllStrings())
            {
                string word = de.Value.ToString();
                this.RtfColors.markWordsAll(word, TextStyle);

            }

            this.assignedRtf.Rtf = this.drawingRtf.Rtf;
            this.assignedRtf.SelectionStart = startpos;
            this.assignedRtf.SelectionLength = endPos;

        }


        public void getElements()
        {
            foreach (ReflectionScriptDefines scriptLine in this.Srcipt.getScript())
            {
                if (scriptLine.namedReference != null)
                {
                    this.addKeyWord(scriptLine.namedReference,ObjectStyle);
                }

                if (scriptLine.originCode != null)
                {
                    this.addKeyWord(scriptLine.originCode, CommandStyle);
                }

                if (scriptLine.typeOfObject != null)
                {
                    this.addKeyWord(scriptLine.typeOfObject, ReferenzStyle);
                }

                if (scriptLine.isVariable == true && scriptLine.name != null)
                {
                    this.addKeyWord(scriptLine.name, VarStyle);
                }
            }
            /*
            foreach (DictionaryEntry de in this.Srcipt.getAllStrings())
            {
                string word = de.Value.ToString();
                this.addKeyWord(word, TextStyle);
                
            }
            */

            this.elementsReaded = true;
        }


        private void addKeyWord(string word, HighlightStyle color)
        {
            if (!KeyWords.ContainsKey(word))
            {
                KeyWords.Add(word, color);
            }
        }
        
    }
}
