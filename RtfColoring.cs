using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

namespace Projector
{
    class RtfColoring
    {
        private const string REGEX_STRING = "\"([^\"]*)\"";
        public const int MODE_DIRECT = 0;
        public const int MODE_WORD = 1;

        private RichTextBox intRtf;

        public int startPosition = 0;
        public int startLine = 0;

        private int cursorPosition = 0;
        private int currentLine = 0;

        public HighlightStyle mainStyle = new HighlightStyle();
        public HighlightStyle stringStyle = new HighlightStyle();

        private int Mode = 1;

        private Hashtable wordColors = new Hashtable();

        

        public RtfColoring(RichTextBox rtf)
        {
            this.intRtf = rtf;
            
        }

        public void reAssign(RichTextBox rtf)
        {
            this.intRtf = rtf;
        }

        public void setMode(int mode)
        {
            this.Mode = mode;
        }

        private void storePositions()
        {
            this.cursorPosition = this.intRtf.SelectionStart;
            this.currentLine = this.intRtf.GetLineFromCharIndex(cursorPosition);
        }

        private void resetPositions()
        {
            this.intRtf.Select(this.cursorPosition, 1);
        }

        private void regColorWord(string word, HighlightStyle col)
        {
            if (!this.wordColors.ContainsKey(word))
            {
                this.wordColors.Add(word, col);
            }
        }

        public void markWordsAll(string[] words, HighlightStyle color)
        {
            foreach (string word in words)
            {
                if (this.Mode == RtfColoring.MODE_DIRECT)
                    markWordsAll(word, color);
                else
                {
                    regColorWord(word, color);
                }

            }
        }


        public void markWordsAll(string word, HighlightStyle color)
        {

            if (this.Mode == RtfColoring.MODE_WORD)
            {
                regColorWord(word, color);
                return;
            }

            this.storePositions();
            int searchPos = this.startPosition;
            while (searchPos != -1)
            {
                searchPos = this.markWord(searchPos, word, color);
            }
            this.resetPositions();
        }

        public void wordMode()
        {
            if (this.Mode == RtfColoring.MODE_WORD)
            {
                int fistRunPosition = this.getFistPositionByLine(this.startLine);

                if (fistRunPosition >= this.intRtf.Text.Length)
                {
                    return;
                }
                // first extract strings so this are counting as one word

                string code = this.intRtf.Text.Substring(fistRunPosition);
                MatchCollection match = Regex.Matches(code, RtfColoring.REGEX_STRING);

                Hashtable stringMem = new Hashtable();

                for (int i = 0; i < match.Count; i++)
                {

                    string str = match[i].Value;
                    string key = "%str_" + i + "%";
                    stringMem.Add(key, str);
                    code = code.Replace(str, key);
                        
                }



                string[] words = code.Split(new char[] { ' ', '\n' });
                int pos = 0;
                foreach (string colWord in words)
                {
                    string rWord = colWord;
                    if (stringMem.ContainsKey(colWord))
                    {
                        rWord = (string)stringMem[colWord];
                        colorizeWord(rWord, pos + fistRunPosition, this.stringStyle);
                    }
                    else
                    {
                        if (rWord != "")
                        {
                            colorizeWord(rWord, pos + fistRunPosition);
                        }
                    }
                    pos++; // for any splitchar
                    pos += rWord.Length;
                }

            }
        }

        private void colorizeWord(string word, int pos, HighlightStyle color)
        {
            this.intRtf.Select(pos, word.Length);
            this.intRtf.SelectionColor = color.ForeColor;
            if (this.intRtf.SelectionBackColor != Color.Transparent)
            {
                this.intRtf.SelectionBackColor = color.BackColor;
            }
            this.intRtf.SelectionFont = color.Font;
        }

        private void colorizeWord(string word, int pos)
        {
            if (this.wordColors.ContainsKey(word))
            {
                HighlightStyle color = (HighlightStyle)this.wordColors[word];
                colorizeWord(word, pos, color);
            }
            else
            {                
                colorizeWord(word, pos, this.mainStyle);
            }

        }

        public int markWord(int start,string word, HighlightStyle color)
        {
            int retInt = -1;

            if (start > this.intRtf.Text.Length)
            {
                return -1;
            }

            int end = this.intRtf.Find(word, start, RichTextBoxFinds.WholeWord);
            if (end > -1 )
            {
                this.intRtf.Select(end, word.Length);
                this.intRtf.SelectionColor = color.ForeColor;
                if (this.intRtf.SelectionBackColor != Color.Transparent)
                {
                    this.intRtf.SelectionBackColor = color.BackColor;
                }
                this.intRtf.SelectionFont = color.Font;
                retInt = end + word.Length + 1;

                if (retInt > this.intRtf.Text.Length || retInt <= start)
                {
                    return -1;
                }
            }
            
            return retInt;
        }

        public int markTextLine(string word, HighlightStyle color)
        {
            int retInt = -1;
            string[] words = word.Split('\n');
            this.markWordsAll(words, color);
            /*
            int end = this.intRtf.Find(words[0], 0, RichTextBoxFinds.None);
            if (end > -1)
            {
                this.intRtf.Select(end, word.Length);
                this.intRtf.SelectionColor = color.ForeColor;
                this.intRtf.SelectionBackColor = color.BackColor;
                this.intRtf.SelectionFont = color.Font;
                retInt = end + 1;
            } 
             */
            return retInt;
        }

        private int getFistPositionByLine(int lineNumber)
        {
            if (lineNumber >= this.intRtf.Lines.Count())
            {
                return 0;
            }

            int existingLinesCharCount = 0;
            for (int ln = this.startLine; ln < lineNumber; ln++)
            {
                existingLinesCharCount += this.intRtf.Lines[ln].Length + 1; // +1 for line ending
            }
            return existingLinesCharCount;
        }

        public void markFullLine(int lineNumber, HighlightStyle color)
        {
            if (lineNumber >= this.intRtf.Lines.Count())
            {
                return;
            }

            int existingLinesCharCount = 0;
            for (int ln = this.startLine; ln < lineNumber; ln++)
            {
                existingLinesCharCount += this.intRtf.Lines[ln].Length + 1; // +1 for line ending
            }

            int index = this.intRtf.GetFirstCharIndexFromLine(lineNumber);
            string line = this.intRtf.Lines[lineNumber];

            if (null == line)
            {
                return;
            }

            int end = line.Length;
            
            //string word = this.intRtf.Lines[lineNumber];
            //this.markWordsAll(word, color);

            
            if (end > 0)
            {
                //this.intRtf.Select(index, end);
                this.intRtf.SelectionStart = existingLinesCharCount;
                this.intRtf.SelectionLength = end;
                this.intRtf.SelectionColor = color.ForeColor;
                this.intRtf.SelectionBackColor = color.BackColor;
                this.intRtf.SelectionFont = color.Font;
              
            } 
            
        }

        

        public Boolean markWordInCurrentLine(string word, Color color)
        {
            Boolean found = false;
            this.storePositions();
            int start = this.intRtf.GetFirstCharIndexOfCurrentLine();
            int end = this.intRtf.Find(word, start, RichTextBoxFinds.WholeWord);
            if (end > -1)
            {
                this.intRtf.Select(end, word.Length);

                this.intRtf.SelectionColor = color;
                found = true;
            } 

            this.resetPositions();
            return found;
        }

    }
}
