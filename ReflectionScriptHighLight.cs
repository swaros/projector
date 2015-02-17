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

        public HighlightStyle ObjectStyle = new HighlightStyle();
        public HighlightStyle ObjectStyleReferenz = new HighlightStyle();
        public HighlightStyle CommandStyle = new HighlightStyle();
        public HighlightStyle ReferenzStyle = new HighlightStyle();
        public HighlightStyle VarStyle = new HighlightStyle();
        public HighlightStyle TextStyle = new HighlightStyle();
        public HighlightStyle CommentStyle = new HighlightStyle();
        public HighlightStyle ErrorStyle = new HighlightStyle();
        public HighlightStyle VaribalesStyle = new HighlightStyle();
        public HighlightStyle KeyWordStyle = new HighlightStyle();
        public HighlightStyle executionStyle = new HighlightStyle();
        public HighlightStyle NumberStyle = new HighlightStyle();
        public HighlightStyle InProgressStyle = new HighlightStyle();

        private string styleFileName = "pr_styles.xml";

        private Hashtable LastExecutedLines = new Hashtable();

        public int fontDefaultSize = 10;
        public string defaultFontName = "Courier New";

        public Boolean multiMarkLine = false;

        private Boolean elementsReaded = false;

        public int markLine = -1;

        public int startPos = 0;
        public int startLine = 0;
        public Boolean updateScrols = false;

        public string runtime = "0";
        public string postRuntime = "0";
        public string preRuntime = "0";

        private int drawMode = 0;

        Color[] execColors = new Color[10];

        public void clearExecutions()
        {
            this.LastExecutedLines.Clear();
        }

        public void resetFonts()
        {
       
            this.ObjectStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold);
            this.ObjectStyleReferenz.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold);
            this.VaribalesStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold);
            this.CommandStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);
            this.ReferenzStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold);
            this.VarStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);
            this.KeyWordStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Bold);
            this.CommentStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);
            this.ErrorStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);
            this.InProgressStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);
            this.executionStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);       
            this.TextStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);
            this.NumberStyle.Font = new Font(defaultFontName, this.fontDefaultSize, FontStyle.Regular);
        }

        public void loadColors()
        {
            XmlSetup tmpSetup = new XmlSetup();
            tmpSetup.setFileName(this.styleFileName);
            tmpSetup.loadXml();

            this.getSavedStyle(tmpSetup, "script_color_object", ObjectStyle);
            this.getSavedStyle(tmpSetup, "script_color_object_ref", ObjectStyleReferenz);
            this.getSavedStyle(tmpSetup, "script_color_var1", VaribalesStyle);
            this.getSavedStyle(tmpSetup, "script_color_var2", VarStyle);
            this.getSavedStyle(tmpSetup, "script_color_keyword", KeyWordStyle);
            this.getSavedStyle(tmpSetup, "script_color_command", CommandStyle);
            this.getSavedStyle(tmpSetup, "script_color_ref", ReferenzStyle);
            this.getSavedStyle(tmpSetup, "script_color_comment", CommentStyle);
            this.getSavedStyle(tmpSetup, "script_color_string", TextStyle);
            this.getSavedStyle(tmpSetup, "script_color_number", NumberStyle);

            HighlightStyle defaultStyle = new HighlightStyle();
            defaultStyle.BackColor = HighlightStyle.defaultColor;
            defaultStyle.Font = new Font(this.defaultFontName, this.fontDefaultSize, FontStyle.Regular);

            this.getSavedStyle(tmpSetup, "script_color_default", defaultStyle);

            this.fontDefaultSize = (int) defaultStyle.Font.Size;
            this.defaultFontName = defaultStyle.Font.Name;
            HighlightStyle.defaultColor = defaultStyle.BackColor;
            this.RtfColors.numberStyle = this.NumberStyle;

        }

        private void getSavedStyle(XmlSetup setup, string name, HighlightStyle toThis)
        {
            String val = setup.getValue(name);
            if (val != null)
            {
                toThis.getStyleFromvalueString(val);
            }
        }

        public void saveColors()
        {
            this.saveColors(this.styleFileName);
        }

        public void saveColors(string fileName)
        {
            XmlSetup tmpSetup = new XmlSetup();
            tmpSetup.setFileName(fileName);
            tmpSetup.loadXml();

            tmpSetup.addSetting("script_color_object", ObjectStyle.toSetupValue());
            tmpSetup.addSetting("script_color_object_ref", ObjectStyleReferenz.toSetupValue());
            tmpSetup.addSetting("script_color_var1", VaribalesStyle.toSetupValue());
            tmpSetup.addSetting("script_color_var2", VarStyle.toSetupValue());
            tmpSetup.addSetting("script_color_keyword", KeyWordStyle.toSetupValue());
            tmpSetup.addSetting("script_color_command", CommandStyle.toSetupValue());
            tmpSetup.addSetting("script_color_ref", ReferenzStyle.toSetupValue());
            tmpSetup.addSetting("script_color_comment", CommentStyle.toSetupValue());
            tmpSetup.addSetting("script_color_string", TextStyle.toSetupValue());
            tmpSetup.addSetting("script_color_number", NumberStyle.toSetupValue());

            HighlightStyle defaultStyle = new HighlightStyle();
            defaultStyle.BackColor = HighlightStyle.defaultColor;
            defaultStyle.Font = new Font(this.defaultFontName, this.fontDefaultSize, FontStyle.Regular);
            tmpSetup.addSetting("script_color_default", defaultStyle.toSetupValue());

            tmpSetup.saveXml();
        }


        public ReflectionScriptHighLight(ReflectionScript script, RichTextBox rtf){


            execColors[0] = Color.FromArgb(75, 20, 35);
            execColors[1] = Color.FromArgb(80, 40, 45);
            execColors[2] = Color.FromArgb(85, 60, 55);
            execColors[3] = Color.FromArgb(90, 80, 65);
            execColors[4] = Color.FromArgb(95, 100, 75);
            execColors[5] = Color.FromArgb(100, 120, 85);
            execColors[6] = Color.FromArgb(105, 140, 95);
            execColors[7] = Color.FromArgb(115, 160, 105);
            execColors[8] = Color.FromArgb(120, 180, 115);
            execColors[9] = Color.FromArgb(125, 200, 125); 

            this.Srcipt = script;
            this.assignedRtf = rtf;
            this.drawingRtf = new RichBox();
            drawingRtf.Rtf = assignedRtf.Rtf;
            this.RtfColors = new RtfColoring(drawingRtf);
            
            drawingRtf.highlighting = true;

            loadColors();
            rtf.BackColor = HighlightStyle.defaultColor;
            this.ObjectStyle.ForeColor = Color.DarkMagenta;

            this.ObjectStyleReferenz.ForeColor = Color.SteelBlue;

            this.VaribalesStyle.ForeColor = Color.DarkGreen;
            //this.VaribalesStyle.BackColor = Color.Transparent;


            this.CommandStyle.ForeColor = Color.Blue;


            this.ReferenzStyle.ForeColor = Color.DarkGoldenrod;

            this.VarStyle.ForeColor = Color.DarkOrange;

            this.KeyWordStyle.ForeColor = Color.DarkBlue;      


            this.CommentStyle.ForeColor = Color.DarkOliveGreen;
            this.CommentStyle.BackColor = Color.LightGray;

            this.ErrorStyle.ForeColor = Color.DarkRed;
            this.ErrorStyle.BackColor = Color.LightPink;


            this.InProgressStyle.ForeColor = Color.OrangeRed;
            this.InProgressStyle.BackColor = Color.Yellow;



            this.executionStyle.ForeColor = Color.LightGreen;
            this.executionStyle.BackColor = Color.DarkGreen;


            this.TextStyle.ForeColor = Color.SlateBlue;
            this.TextStyle.BackColor = Color.LightCyan;

            this.NumberStyle.ForeColor = Color.DarkGoldenrod;

            this.RtfColors.stringStyle = this.TextStyle;
            this.resetFonts();
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
            try
            {
                if (this.assignedRtf.Text.Length < 1)
                {
                    return 0;
                }
            }
            catch (Exception ex)
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
                if (this.markLine < 0)
                    this.getElements();
            }

          

            if (this.markLine < 0)
            {
                this.drawingRtf.Select(this.startPos, this.drawingRtf.TextLength);
                this.drawingRtf.SelectionColor = drawingRtf.ForeColor;
                this.drawingRtf.SelectionBackColor = HighlightStyle.defaultColor;

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
                    if (err.wordPosition < 1)
                    {
                        this.RtfColors.markFullLine(err.lineNumber, ErrorStyle, true, false, err.errorMessage + " @" + err.lineNumber);
                    }
                    else
                    {
                        this.RtfColors.markFullLine(err.lineNumber, InProgressStyle, true, false, err.errorMessage + " @" + err.lineNumber, err.wordPosition);
                    }
                    

                }
            }
            // marks an line 
            if (this.markLine > -1)
            {

                if (multiMarkLine)
                {
                    if (!this.LastExecutedLines.ContainsKey(markLine))
                    {
                        this.LastExecutedLines.Add(markLine, 10);
                    }
                    else
                    {
                        this.LastExecutedLines[markLine] = 10;
                    }

                    Hashtable copyThat = new Hashtable();
                    foreach (DictionaryEntry lastExecs in this.LastExecutedLines)
                    {
                        int execCount = (int)lastExecs.Value;
                        execCount--;

                        if (execCount > 0)
                        {

                            copyThat.Add(lastExecs.Key, execCount);
                            if (execCount > 8)
                            {
                                this.RtfColors.markFullLine((int)lastExecs.Key, executionStyle, true, false);
                            }
                            else
                            {
                                HighlightStyle hStyle = new HighlightStyle();

                                hStyle.ForeColor = execColors[(int)lastExecs.Value];
                                this.RtfColors.markFullLine((int)lastExecs.Key, hStyle, true, false);
                            }

                        }
                    }
                    LastExecutedLines = copyThat;

                    
                }
                else
                {
                    this.RtfColors.markFullLine(markLine, executionStyle, true, false);
                }

                if (assignedRtf is RichBox)
                {
                    RichBox rbt = (RichBox)assignedRtf;
                    if (!rbt.selectionIsVisible())
                    {
                        this.assignedRtf.ScrollToCaret();
                    }
                }
                else
                {
                    this.assignedRtf.ScrollToCaret();
                }
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
                rbt.LineMarker = drawingRtf.LineMarker;
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
                if (index > -1)
                {
                    this.assignedRtf.SelectionStart = index;
                    this.assignedRtf.SelectionLength = 1;
                    if (this.updateScrols) this.assignedRtf.ScrollToCaret();
                }

            }

            watch.Stop();
            this.postRuntime = watch.ElapsedMilliseconds.ToString();
            return 1;
        }


        public void getElements()
        {
            this.addKeyWord("{", ObjectStyle);
            this.addKeyWord("}", ObjectStyle);
            this.addKeyWord("(", ObjectStyle);
            this.addKeyWord(")", ObjectStyle);
            this.addKeyWord("=", ObjectStyle);
            this.addKeyWord("+", ObjectStyle);
            this.addKeyWord("-", ObjectStyle);            
            this.addKeyWord("true", ObjectStyle);
            this.addKeyWord("false", ObjectStyle);



            foreach (ReflectionScriptDefines scriptLine in this.Srcipt.getFullScript())
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

                if (scriptLine.isObject == true && scriptLine.ReflectObject != null)
                {
                    this.addKeyWord(scriptLine.name, ObjectStyleReferenz);
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
