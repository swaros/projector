using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projector
{
    public partial class BaseDrawing : UserControl
    {
        public BaseDrawing()
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


        private void drawContent(Graphics g)
        {

        }


        private void DrawListContent(Graphics g)
        {
           
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
