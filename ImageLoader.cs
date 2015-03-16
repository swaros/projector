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
    public partial class ImageLoader : UserControl
    {
        public ImageLoader()
        {
            InitializeComponent();
        }

        public void loadPicture(string path)
        {
            try
            {
                //this.loadImage.LoadAsync(path);
                this.loadImage.ImageLocation = path;
            }
            catch (Exception)
            {
                return;
            }

            this.Width = this.loadImage.Size.Width;
            this.Height = this.loadImage.Size.Height;

        }


        public void setWidth(int val)
        {
            this.Width = val;
            
        }

        public void setHeight(int val)
        {
            this.Height = val;
        }
        public void setEnabled(Boolean onoff)
        {
            this.Enabled = onoff;
        }

        public void setVisible(Boolean onoff)
        {
            this.Visible = onoff;
        }

        public void setLeft(int left)
        {
            this.Left = left;
        }

        public void setTop(int top)
        {
            this.Top = top;
        }

       
        public void reDraw()
        {
            this.loadImage.Update();
            this.loadImage.Invalidate();
        }
    }
}
