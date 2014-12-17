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

        public List<RichBoxMarker> redmarker = new List<RichBoxMarker>();

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
                        foreach (RichBoxMarker dRec in redmarker){
                            Point tl = this.PointToClient(dRec.topLeft);

                            Point mcheck = this.GetPositionFromCharIndex(dRec.firstCharIndex);

                            Rectangle dRecTangle = new Rectangle(tl.X, tl.Y, 10, 10);

                            Rectangle oldRecTangle = new Rectangle(dRec.topLeft.X, dRec.topLeft.Y, 10, 10);

                            Rectangle bychar = new Rectangle(1, mcheck.Y, this.ClientSize.Width - 1, 18);

                            g.DrawRectangle(redPen, dRecTangle);
                            g.DrawRectangle(new Pen(dRec.ForeColor), dRecTangle);
                            g.DrawRectangle(new Pen(dRec.ForeColor), bychar);
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
