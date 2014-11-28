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
        private HighlightStyle CommentStyle = new HighlightStyle();
        private HighlightStyle ErrorStyle = new HighlightStyle();
        private HighlightStyle VaribalesStyle = new HighlightStyle();

        private HighlightStyle KeyWordStyle = new HighlightStyle();

        private int fontDefaultSize = 10;
        private string defaultFontName = "Courier New";

        private Boolean elementsReaded = false;

        public ReflectionScriptHighLight(ReflectionScript script, RichTextBox rtf){
            this.Srcipt = script;
            this.assignedRtf = rtf;
            this.drawingRtf = new RichTextBox();
            drawingRtf.Rtf = assignedRtf.Rtf;
            this.RtfColors = new RtfColoring(drawingRtf);


            this.ObjectStyle.ForeColor = Color.DarkMagenta;
            this.ObjectStyle.Font = new Font("Times New Roman",this.fontDefaultSize,FontStyle.Bold | FontStyle.Italic);

            this.VaribalesStyle.ForeColor = Color.DarkKhaki;
            this.VaribalesStyle.Font = new Font("Times New Roman", this.fontDefaultSize, FontStyle.Bold | FontStyle.Italic);


            this.CommandStyle.ForeColor = Color.DarkGreen;
            this.CommandStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold);


            this.ReferenzStyle.ForeColor = Color.DarkGoldenrod;
            this.ReferenzStyle.Font = new Font(defaultFontName, this.fontDefaultSize , FontStyle.Bold);

            this.VarStyle.ForeColor = Color.DarkOrange;
            this.VarStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);

            this.KeyWordStyle.ForeColor = Color.DarkBlue;      
            this.KeyWordStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold);


            this.CommentStyle.ForeColor = Color.DarkOliveGreen;
            this.CommentStyle.BackColor = Color.LightGray;
            this.CommentStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);

            this.ErrorStyle.ForeColor = Color.DarkRed;
            this.ErrorStyle.BackColor = Color.LightPink;
            this.ErrorStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);


            this.TextStyle.ForeColor = Color.SlateBlue;
            //this.TextStyle.BackColor = Color.LightSlateGray;
            
            this.TextStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);
        }

        public int reDraw(Boolean reNewElements)
        {
            if (this.assignedRtf.Text.Length < 1)
            {
                return 0;
            }
            this.drawingRtf.Rtf = this.assignedRtf.Rtf;
            int startpos = this.assignedRtf.SelectionStart;
            int endPos = this.assignedRtf.SelectionLength;
            int start = this.assignedRtf.GetCharIndexFromPosition(new Point(0, 0));
            int end = this.assignedRtf.GetCharIndexFromPosition(new Point(this.assignedRtf.ClientSize.Width, this.assignedRtf.ClientSize.Height));

            if (reNewElements == true || this.elementsReaded == false)
            {
                this.getElements();
            }

            this.drawingRtf.Select(0, this.drawingRtf.TextLength);
            this.drawingRtf.SelectionColor = drawingRtf.ForeColor;
            this.drawingRtf.SelectionBackColor = drawingRtf.BackColor;
            


            foreach (DictionaryEntry de in this.KeyWords)
            {
                string word = de.Key.ToString();
                HighlightStyle setColor = (HighlightStyle)de.Value;
                this.RtfColors.markWordsAll(word, setColor);
            }

            // must be at the end

            foreach (String keyWord in Projector.RefScriptMaskMatch.KeyWords)
            {

                this.RtfColors.markTextLine(keyWord, KeyWordStyle);
            }


            string[] variables = new String[this.Srcipt.getAllStrings().Count];

            int it = 0;
            foreach (DictionaryEntry de in this.Srcipt.getAllStrings())
            {
                string word = de.Value.ToString();
                string keyWord = de.Key.ToString();
                this.RtfColors.markTextLine(word, TextStyle);
                //this.RtfColors.markTextLine(keyWord, VaribalesStyle);
                variables[it] = keyWord;
                it++;
            }

            this.RtfColors.markWordsAll(variables, VaribalesStyle);

            foreach (int lino in this.Srcipt.getCommentLines())
            {
                this.RtfColors.markFullLine(lino, CommentStyle);
            }

            foreach (ScriptErrors err in this.Srcipt.getAllErrors())
            {
                this.RtfColors.markFullLine(err.lineNumber, ErrorStyle);
            }

            this.assignedRtf.Rtf = this.drawingRtf.Rtf;

            // try to scroll last visible position
            this.assignedRtf.SelectionLength = 0;
            this.assignedRtf.SelectionStart = end;
            this.assignedRtf.ScrollToCaret();
            this.assignedRtf.SelectionStart = start + this.assignedRtf.Lines[this.assignedRtf.GetLineFromCharIndex(start)].Length + 1;
            this.assignedRtf.ScrollToCaret();

            this.assignedRtf.SelectionStart = startpos;
            this.assignedRtf.SelectionLength = endPos;
           
            return 1;
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
