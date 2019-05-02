using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MosaicArtCreator.Mosaic;

namespace MosaicArtCreator
{
    public class ImageUtils
    {
        //Load bitmap image into Picture object
        public static Picture LoadPicture(Bitmap bitmap)
        {
            return LoadPicture(bitmap, false);
        }

        //Load bitmap image into Picture object
        public static Picture LoadPicture(Bitmap bitmap, bool toGrey)
        {
            Picture pic = new Picture(bitmap.Width, bitmap.Height);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    RGBColor color;
                    if (toGrey)
                    {
                        int gray = Convert.ToInt32(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                        color = new RGBColor(gray, gray, gray);
                    }
                    else
                    {
                        color = new RGBColor(pixelColor.R, pixelColor.G, pixelColor.B);
                    }
                    pic.SetPixel(new Dot(x, y), color);
                }
            }
            return pic;
        }

        private void RGBToGray(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    int gray = Convert.ToInt32(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
                    bitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
        }
    }
}
