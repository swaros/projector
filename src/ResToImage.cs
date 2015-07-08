using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Projector
{
    /// <summary>
    /// Helper class for mono development
    /// to evade mono errors on image assigments
    /// </summary>
    class ResToImage
    {
        public static void addImageToList(ImageList imgList, Image img) 
        {
            if (img != null)
            {
                imgList.Images.Add(img);
                imgList.Images.SetKeyName(imgList.Images.Count - 1, "Image" + imgList.Images.Count);
            }            
        }

        public static void addResToImageList(ImageList imgList, string resName)
        {
            Object rm = Properties.Resources.ResourceManager.GetObject(resName);
            Bitmap myImage = (Bitmap)rm;
            imgList.Images.Add(myImage);
        }
    }
}
