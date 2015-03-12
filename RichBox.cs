using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Projector
{
    public class RichBox : RichTextBox
    {
        private const int WM_PAINT = 15;

        public Boolean highlighting = false;

        public List<RichBoxMarker> LineMarker = new List<RichBoxMarker>();
        public List<RichBoxMarker> ExecutionMarker = new List<RichBoxMarker>();


        public Boolean selectionIsVisible()
        {
            return this.selectionIsVisible(0);
        }

        public Boolean selectionIsVisible(int offset)
        {
            int startVisibleChar = this.GetCharFromPosition(new Point(0, 0));

            if (startVisibleChar > this.SelectionStart)
            {
                return false;
            }


            int lineNumber = this.GetLineFromCharIndex(startVisibleChar);
            int currentLineNumber = this.GetLineFromCharIndex(this.SelectionStart);
            if (lineNumber + offset > currentLineNumber)
            {
                return false;
            }

            int maxLines = (this.ClientSize.Height / this.getLineHeight()) + 1;

            if (maxLines > this.Lines.Count())
            {
                maxLines = this.Lines.Count()-1;
            }

            int checkInt = startVisibleChar;
            for (int i = lineNumber; i < maxLines; i++)
            {
                checkInt += this.Lines[i].Length + 1;
                if (checkInt > this.SelectionStart)
                {
                    return true;
                }
            }
            return false;
        }

        public int getLineHeight()
        {
            return this.FontHeight + 2;
        }

        /// <summary>
        /// returns the current line as string
        /// </summary>
        /// <returns></returns>
        public string getSelectedLine()
        {
            int cursorPosition = this.SelectionStart;
            int lineIndex = this.GetLineFromCharIndex(cursorPosition);
            string lineText = this.Lines[lineIndex];
            return lineText;
        }

        /// <summary>
        /// get the selected line number
        /// </summary>
        /// <returns></returns>
        public int getCurrentLineNumber()
        {
            int cursorPosition = this.SelectionStart;
            int lineIndex = this.GetLineFromCharIndex(cursorPosition);            
            return lineIndex;
        }

        /// <summary>
        /// duplicates the given line Number
        /// </summary>
        /// <param name="nr"></param>
        public void duplicateLine(int nr)
        {
            int cursorPosition = this.SelectionStart;
            string newText = "";
            for (int i = 0; i < this.Lines.Count(); i++)
            {
                newText += this.Lines[i] + "\n";
                if (i == nr)
                {
                    newText += this.Lines[i] + "\n";
                    cursorPosition += this.Lines[i].Length + 1;
                }
            }
            this.Text = newText;
            this.SelectionStart = cursorPosition;
        }

        /// <summary>
        /// removes the given line Number
        /// </summary>
        /// <param name="nr"></param>
        public void removeLine(int nr)
        {
            int cursorPosition = this.SelectionStart;
            string newText = "";
            for (int i = 0; i < this.Lines.Count(); i++)
            {                
                if (i != nr)
                {
                    newText += this.Lines[i] + "\n";

                }
                else
                {
                    cursorPosition -= this.Lines[i].Length + 1;
                }
            }
            this.Text = newText;
            if (cursorPosition >= 0)
            {
                this.SelectionStart = cursorPosition;
            }
        }


        public void addLine(string line)
        {
            if (this.SelectionStart >= 0)
            {
                this.SelectedText += "\n" + line;
                                
                
            }
        }

        /// <summary>
        /// replace string in the whole text
        /// </summary>
        /// <param name="old"></param>
        /// <param name="newstr"></param>
        public void replace(string old, string newstr)
        {
            if (old == "")
            {
                return;
            }
            this.Text = this.Text.Replace(old, newstr);
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT)
            {
               
                    this.Invalidate();
                    base.WndProc(ref m);
                    using (Graphics g = Graphics.FromHwnd(this.Handle))
                    {
                        /*
                        g.DrawLine(Pens.Red, Point.Empty,
                                   new Point(this.ClientSize.Width - 1,
                                             this.ClientSize.Height - 1));
                         */
                        Pen redPen = new Pen(Color.Blue);
                        Brush solidRedBrush = new SolidBrush(Color.DarkRed);
                        foreach (RichBoxMarker dRec in LineMarker){
                            Point tl = this.PointToClient(dRec.topLeft);
                            
                            Point mcheck = this.GetPositionFromCharIndex(dRec.firstCharIndex);
                            Rectangle bychar = new Rectangle(mcheck.X, mcheck.Y, this.ClientSize.Width - 1 - mcheck.X, this.getLineHeight());                           
                            Brush mBrush =  new SolidBrush(Color.FromArgb(45, dRec.ForeColor.R, dRec.ForeColor.G, dRec.ForeColor.B));
                            Brush SolidBrush = new SolidBrush(Color.FromArgb(dRec.ForeColor.R, dRec.ForeColor.G, dRec.ForeColor.B));

                            g.FillRectangle(mBrush, bychar);
                            g.DrawRectangle(new Pen(dRec.ForeColor), bychar);

                            if (dRec.displayText != null)
                            {

                                StringFormat stringFormat = new StringFormat();
                                stringFormat.Alignment = StringAlignment.Far;
                                stringFormat.LineAlignment = StringAlignment.Center;
                                g.DrawString(dRec.displayText, new Font("Courier New", 8, FontStyle.Regular), SolidBrush, bychar, stringFormat);
                            }
                        }
                    }
                    
           
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        
    }
}
