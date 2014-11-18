using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    class RtfColoring
    {
        private RichTextBox intRtf;


        private int cursorPosition = 0;
        private int currentLine = 0;

        public RtfColoring(RichTextBox rtf)
        {
            this.intRtf = rtf;
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


        public void markWordsAll(string word, HighlightStyle color)
        {
            this.storePositions();
            int searchPos = 0;
            while (searchPos != -1)
            {
                searchPos = this.markWord(searchPos, word, color);
            }
            this.resetPositions();
        }

        public int markWord(int start,string word, HighlightStyle color)
        {
            int retInt = -1;
                     
            int end = this.intRtf.Find(word, start, RichTextBoxFinds.None);
            if (end > -1)
            {
                this.intRtf.Select(end, word.Length);
                this.intRtf.SelectionColor = color.ForeColor;
                this.intRtf.SelectionBackColor = color.BackColor;
                this.intRtf.SelectionFont = color.Font;
                retInt = end + 1;
            }

            this.resetPositions();
            return retInt;
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
