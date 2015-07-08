using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Projector.Data;

namespace Projector
{
    public partial class ChartDrawer : UserControl
    {

        private ResultList usedList;

        private int RowHeight = 20;



        public ChartDrawer()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            
            base.OnPaint(e);
            // Call methods of the System.Drawing.Graphics object.
            draw(e.Graphics);
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle);
        }

        public void setResultList(ResultList list)
        {
            this.usedList = list;
        }

        private void drawContent(Graphics g)
        {

        }


        private void DrawListContent(Graphics g)
        {
            if (this.usedList != null)
            {

                SolidBrush fontBrush = new SolidBrush(Color.Black);

                int col = this.usedList.getColumnCount();
                int rows = this.usedList.getRowCount();

                int yPos = this.RowHeight;
                for (int y = 0; y < rows; y++)
                {
                    g.DrawLine(new Pen(fontBrush), 0, yPos, this.Width, yPos);
                    yPos = this.RowHeight * (y + 1);
                }

            }
        }

        private void draw(Graphics gOrigin)
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bmp);
            this.drawContent(g);
            gOrigin.DrawImageUnscaled(bmp, new Point(0, 0));
        }
    }
}
