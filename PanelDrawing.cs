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

   
    class PanelDrawing : Panel
    {

        public string Label = "";
        public Color positivColor = Color.Green;
        public Color negativColor = Color.Red;
        public Color labelColor = Color.White;
        public Color lineColor = Color.White;
        public Color elementsColor = Color.Gray;
        public int infoLabelMinLength = 150;

        public Boolean useLabels = false;

        public bool showTime = true;
        public bool showValues = false;

        private NumberFlow flow = new NumberFlow();
        private NumberFlow dateFlow = new NumberFlow();

       
        //public String Text = "";
        protected override void OnPaint(PaintEventArgs e)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(e);
            // Call methods of the System.Drawing.Graphics object.
            //Int64 start = getMsTime();
            draw(e.Graphics);
            //Int64 end = getMsTime();
            //Int64 diff = end - start;
            e.Graphics.DrawString(Label , Font, new SolidBrush(labelColor), ClientRectangle);
        }

        private Int64 getMsTime(){
            DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
            DateTime endDate = new DateTime();
            // Find the difference between two dates
            TimeSpan diff = endDate.Subtract(startDate);
            return diff.Ticks;
        }

        public void Clear()
        {
            this.flow.Clear();
        }

        public void addValue(Double value)
        {
            flow.add(value);
           
        }

        public void addValue(Double value, string label)
        {
            flow.add(value, label);

        }

        public void draw(Graphics gOrigin)
        {

            Bitmap bmp = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bmp);



            int startX = 0;
            int step = 8;
            int space = 1;
            if (flow.Count > 2)
            {
                double w = this.Width;
                double t = flow.Count - 1;
                step = (int) Math.Round(w / t);
                if (step < 2) space = 0;
                step = step - space;
            }

            SolidBrush fontBrush = new SolidBrush(lineColor);

            int Middle = this.Height / 2;
            flow.getmaxDivisor = 2;
            if (flow.getMin() >= 0)
            {
                Middle = this.Height;
                flow.getmaxDivisor = 1;
            }
            RectangleF drawRect;
            Brush BG = new System.Drawing.SolidBrush(positivColor);
            Brush BGred = new System.Drawing.SolidBrush(negativColor);
            flow.MaxOutput = this.Height;
            int LastX =this.Width;
            int LastY =Middle;
            for (int i = 0; i < flow.Count; i++)
            {
                Int64 val = (Int64)flow.getAtMax(i);
                

                int leftStart = this.Width - startX;

                

                if (val > 0)
                {
                    drawRect = new RectangleF(leftStart, Middle - val, step, val);
                    g.FillRectangle(BG, drawRect);

                    if (showValues)
                    {
                       
                        Font drawFont = new Font("Arial", 7);
                        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat(StringFormatFlags.DirectionVertical);
                        long yoffset = Middle - (val - 15);
                        if (yoffset < 20) yoffset = 20;
                       
                        g.DrawString(flow.getAt(i) + "", drawFont, fontBrush, leftStart,yoffset, drawFormat);
                        if (useLabels)
                        {
                            string displayLabel = flow.getLabelAt(i);
                            int minWidth = this.infoLabelMinLength;
                            
                            if (step > minWidth)
                            {
                                minWidth = step;
                            }

                            
                            g.DrawString(displayLabel, drawFont, fontBrush, leftStart + 10, Middle - (val - 15), drawFormat);

                            

                        }

                    }

                }
                if (val<0)
                {
                    long bval = 0;
                    try
                    {
                        bval = Math.Abs(val);
                    }
                    catch (Exception)
                    {
                        bval = -1;
                        //throw;
                    }

                    drawRect = new RectangleF(leftStart, Middle, step, bval);
                    
                    g.FillRectangle(BGred, drawRect);

                    if (showValues)
                    {

                        Font drawFont = new Font("Arial", 7);
                        drawRect.Height = 15;
                        g.DrawString(flow.getAt(i) + "", drawFont, fontBrush, drawRect);

                         

                    }

                }
                int halfBar = (step / 2);
                
                //g.DrawLine(new Pen(lineColor), new Point(leftStart + halfBar, (int)(Middle - val)), new Point(LastX + halfBar, LastY));
                g.DrawBezier(new Pen(lineColor)
                     , new Point(leftStart + halfBar, (int)(Middle - val))
                     , new Point(leftStart + halfBar + step/2, (int)(Middle - val))
                     , new Point(LastX + halfBar - step / 2, LastY)
                     , new Point(LastX + halfBar, LastY));

               

                startX += step + space;
                LastX = leftStart;
                LastY = (int) (Middle - val);

                

            }

            // elements
            g.DrawLine(new Pen(elementsColor), new Point(0, Middle), new Point(this.Width, Middle));

            Font drawFontLabel = new Font("Arial", 7);
            drawRect = new RectangleF(this.Width / 3, 0, this.Width / 2, 20);
            g.DrawString(flow.getmax() + "", drawFontLabel, new SolidBrush(elementsColor), drawRect);

            drawRect.Y = this.Height - 15;
            g.DrawString(flow.getMin() + "", drawFontLabel, new SolidBrush(elementsColor), drawRect);

            // copy bitmap
            gOrigin.DrawImageUnscaled(bmp,new Point(0,0));

        }
    }
}
