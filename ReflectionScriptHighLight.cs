using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace Projector
{
    class ReflectionScriptHighLight
    {
        private RtfColoring RtfColors;
        private ReflectionScript Srcipt;

        private Hashtable KeyWords = new Hashtable();       

        private RichTextBox assignedRtf;
        private RichBox drawingRtf;

        private HighlightStyle ObjectStyle = new HighlightStyle();
        private HighlightStyle CommandStyle = new HighlightStyle();
        private HighlightStyle ReferenzStyle = new HighlightStyle();
        private HighlightStyle VarStyle = new HighlightStyle();
        private HighlightStyle TextStyle = new HighlightStyle();
        private HighlightStyle CommentStyle = new HighlightStyle();
        private HighlightStyle ErrorStyle = new HighlightStyle();
        private HighlightStyle VaribalesStyle = new HighlightStyle();


        private HighlightStyle executionStyle = new HighlightStyle();

        private HighlightStyle KeyWordStyle = new HighlightStyle();

        private int fontDefaultSize = 10;
        private string defaultFontName = "Courier New";

        private Boolean elementsReaded = false;

        public int markLine = -1;

        public int startPos = 0;
        public int startLine = 0;
        public Boolean updateScrols = false;

        public string runtime = "0";
        public string postRuntime = "0";
        public string preRuntime = "0";

        private int drawMode = 0;



        public ReflectionScriptHighLight(ReflectionScript script, RichTextBox rtf){
            this.Srcipt = script;
            this.assignedRtf = rtf;
            this.drawingRtf = new RichBox();
            drawingRtf.Rtf = assignedRtf.Rtf;
            this.RtfColors = new RtfColoring(drawingRtf);
            drawingRtf.highlighting = true;
           


            this.ObjectStyle.ForeColor = Color.DarkMagenta;
            this.ObjectStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold );

            this.VaribalesStyle.ForeColor = Color.DarkGreen;
            this.VaribalesStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold);
            this.VaribalesStyle.BackColor = Color.Transparent;


            this.CommandStyle.ForeColor = Color.DarkOliveGreen;
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


            this.executionStyle.ForeColor = Color.LightGreen;
            this.executionStyle.BackColor = Color.DarkGreen;
            this.executionStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);

            this.TextStyle.ForeColor = Color.SlateBlue;
            this.TextStyle.BackColor = Color.LightCyan;
            
            this.TextStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);

            this.RtfColors.stringStyle = this.TextStyle;
            
        }

        public void setWordMode(Boolean onoff)
        {
            this.drawMode = RtfColoring.MODE_DIRECT;
            if (onoff)
            {
                this.drawMode = RtfColoring.MODE_WORD;
            }
        }

        public int reDraw(Boolean reNewElements)
        {
            
            this.RtfColors.setMode(this.drawMode);
            if (this.assignedRtf.Text.Length < 1)
            {
                return 0;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();

            this.drawingRtf.Rtf = this.assignedRtf.Rtf;
            this.drawingRtf.highlighting = true;
            int startpos = this.assignedRtf.SelectionStart;
            int endPos = this.assignedRtf.SelectionLength;
            int start = this.assignedRtf.GetCharIndexFromPosition(new Point(0, 0));
            int end = this.assignedRtf.GetCharIndexFromPosition(new Point(this.assignedRtf.ClientSize.Width, this.assignedRtf.ClientSize.Height));

            this.RtfColors.reAssign(this.drawingRtf);
            this.RtfColors.startLine = this.startLine;
            this.RtfColors.startPosition = this.startPos;


            if (reNewElements == true || this.elementsReaded == false)
            {
                this.getElements();
            }

            this.drawingRtf.Select(this.startPos, this.drawingRtf.TextLength);
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

            //if (this.drawMode == RtfColoring.MODE_DIRECT) { };
            string[] variables = new String[this.Srcipt.getAllStrings().Count];

            int it = 0;
            foreach (DictionaryEntry de in this.Srcipt.getAllStrings())
            {
                string word = de.Value.ToString();
                string keyWord = de.Key.ToString();
                if (this.drawMode == RtfColoring.MODE_DIRECT)
                {
                    this.RtfColors.markTextLine("\"" + word + "\"", TextStyle);
                }
                
                //this.RtfColors.markTextLine(keyWord, VaribalesStyle);
                variables[it] = keyWord;
                it++;
            }

             watch.Stop();

            this.preRuntime = watch.ElapsedMilliseconds.ToString();
            watch.Restart();

            this.RtfColors.markWordsAll(variables, VaribalesStyle);

            this.RtfColors.wordMode();

            // full lines works in booth modes as the same

            foreach (int lino in this.Srcipt.getCommentLines())
            {
                this.RtfColors.markFullLine(lino, CommentStyle);
            }

            foreach (ScriptErrors err in this.Srcipt.getAllErrors())
            {
                this.RtfColors.markFullLine(err.lineNumber, ErrorStyle);
                
            }

            if (this.markLine > -1)
            {
                this.RtfColors.markFullLine(markLine, executionStyle);
            }


            watch.Stop();
            this.runtime = watch.ElapsedMilliseconds.ToString();
            
            watch.Restart();
            // ------------ end drawing ----------------
            this.drawingRtf.highlighting = false;
            this.assignedRtf.Rtf = this.drawingRtf.Rtf;
            if (assignedRtf is RichBox)
            {
                RichBox rbt = (RichBox)assignedRtf;
                rbt.redmarker = drawingRtf.redmarker;
            }

            // try to scroll last visible position
         
            
            if (this.markLine < 0)
            {
                this.assignedRtf.SelectionLength = 0;
                this.assignedRtf.SelectionStart = end;
                if (this.updateScrols) this.assignedRtf.ScrollToCaret();
                this.assignedRtf.SelectionStart = start + this.assignedRtf.Lines[this.assignedRtf.GetLineFromCharIndex(start)].Length + 1;
                if (this.updateScrols) this.assignedRtf.ScrollToCaret();

                this.assignedRtf.SelectionStart = startpos;
                this.assignedRtf.SelectionLength = endPos;
            }
            else
            {
                int index = this.assignedRtf.GetFirstCharIndexFromLine(this.markLine);
                this.assignedRtf.SelectionStart = index;
                this.assignedRtf.SelectionLength = 1;
                if (this.updateScrols) this.assignedRtf.ScrollToCaret();

            }

            watch.Stop();
            this.postRuntime = watch.ElapsedMilliseconds.ToString();
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
