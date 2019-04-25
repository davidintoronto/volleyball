using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosiacArtEditor
{
    //This class represents the orignal picture
    public class Picture
    {
        private Pixel[,] pixels;
        private int width;
        private int height;

        public Picture(int width, int height)
        {
            this.pixels = new Pixel[width, height];
            this.width = width;
            this.height = height;
        }

        public Pixel GetPixel(Dot dot)
        {
            return this.pixels[dot.X, dot.Y];
        }

        public void SetPixel(Dot dot, RGBColor color)
        {
            if (this.pixels[dot.X, dot.Y] == null)
            {
                this.pixels[dot.X, dot.Y] = new Pixel(color);
            }
            else
            {
                this.pixels[dot.X, dot.Y].Color = color;
            }
        }

        public int Height
        {
            get { return height; }
        }
        public int Width
        {
            get
            {
                return width;
            }
        }

        public RGBColor GetColor(int x, int y)
        {
            return pixels[x, y].Color;
        }
        public RGBColor GetColor(Dot dot)
        {
            return pixels[dot.X, dot.Y].Color;
        }
        public bool IsScanned(Dot dot)
        {
            return pixels[dot.X, dot.Y].Scanned;
        }
        public bool IsUnscanned(Dot dot)
        {
            return !pixels[dot.X, dot.Y].Scanned;
        }

        public bool IsInside(Dot dot)
        {
            if (dot.X >=0 && dot.X<width && dot.Y>=0 && dot.Y<height)
            {
                return true;
            }
            return false;
        }
    }

    //The class represents the area with same color in the picture 
    public class Mosaic
    {
        RGBColor baseColor = new RGBColor();
        List<Dot> dots = new List<Dot>();

        public Mosaic() { }
        public Mosaic(Dot dot, RGBColor color)
        {
            this.baseColor = color;
            this.dots.Add(dot);
        }

        public RGBColor Color { get => baseColor; set => baseColor = value; }
        public List<Dot> Dots { get => dots; set => dots = value; }
        private int Count
        {
            get { return this.dots.Count; }
        }

        /*
         * Compare the current color to this mosaic piece average color
         * 
         * 
        */
        public bool IsSimilarColor(Dot dot, RGBColor color, int redOffset, int greenOffset, int blueOffset)
        {
            if (Math.Abs(baseColor.Red - color.Red) > redOffset || Math.Abs(baseColor.Green - color.Green) > greenOffset || Math.Abs(baseColor.Blue - color.Blue) > blueOffset)
            {
                return false;
            }
            return true;
        }
        /*
          * Add dot into this mosaic piece
          * 
          * 
         */
        public void AddIntoMosaicPiece(Dot dot, RGBColor color)
        {
            baseColor.Red = (baseColor.Red * Count + color.Red) / (Count + 1);
            baseColor.Green = (baseColor.Green * Count + color.Green) / (Count + 1);
            baseColor.Blue = (baseColor.Blue * Count + color.Blue) / (Count + 1);
            Dots.Add(dot);
        }
    }


    public class Dot
    {
        private int x;
        private int y;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        public Dot()
        { }

        public Dot(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
       

        public Dot GetUpDot()
        {
            return new Dot(X, Y - 1);
        }
        public Dot GetLeftDot()
        {
            return new Dot(X - 1, Y);
        }
        public Dot GetDownDot()
        {
            return new Dot(X, Y + 1);
        }
        public Dot GetRightDot()
        {
            return new Dot(X + 1, Y);
        }
    }

    //Pixel in picture
    public class Pixel
    {
        private RGBColor color = new RGBColor();
        private bool scanned;

        public Pixel(RGBColor color)
        {
            this.color = color;
        }
        public bool Scanned { get => scanned; set => scanned = value; }
        public RGBColor Color { get => color; set => color = value; }
    }

    public class RGBColor
    {
        private int red;
        private int green;
        private int blue;

        public RGBColor() { }

        public RGBColor(int r, int g, int b)
        {
            this.red = r;
            this.green = g;
            this.blue = b;
        }
        public int Red { get => red; set => red = value; }
        public int Green { get => green; set => green = value; }
        public int Blue { get => blue; set => blue = value; }
    }
}
