using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicArtCreator.Profile
{
    public class Profile
    {
        //Name
        private string name = "Mosaic Art Picture V2";
        //Source pic file
        private string sourceFile;
        //Created mosaic pic file
        private string mosaicFile;
        //Picture width in mm
        private int width = 1000;
        //Picture height in mm
        private int height = 1000;
        //Tile properties
        private Tile tile = new Tile();
        //Piece settings
        private Piece piece = new Piece();
        //Grout color
        private Color groutColor = Color.Goldenrod;
        //Seam / gap line width in mm
        private int seamLineWidth = 2;
        //Maximum color diff
        private int maxDiff = 30;
        //Minimum color diff
        private int minDiff = 5;
        //Color diff increment
        private int diffInce = 5;

        public int MaxDiff { get => maxDiff; set => maxDiff = value; }
        public int MinDiff { get => minDiff; set => minDiff = value; }
        public int DiffIncement { get => diffInce; set => diffInce = value; }

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public Tile Tile { get => tile; set => tile = value; }
        public Piece Piece { get => piece; set => piece = value; }
        public System.Drawing.Color GroutColor { get => groutColor; set => groutColor = value; }
        public string Name { get => name; set => name = value; }
        public int SeamLineWidth { get => seamLineWidth; set => seamLineWidth = value; }

    }
    public class Tile
    {
        //Tile width in mm
        private int width = 300;
        //Tile height in mm
        private int height = 300;
        //Tile thinkness in mm
        private int thick = 5;
        //Tile original color
        private Color color = Color.Green;
        //Tile hardness level
        //todo

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public int Thick { get => thick; set => thick = value; }
        public Color Color { get => color; set => color = value; }
    }

    public class Piece
    {
        //Maximum width / height in mm
        private int maxLength = 30;
        //Minimum width / height in mm
        private int minLength = 5;
        //Maximum size in sq mm
        private int maxSize = 5000;
        //Minimum size in sq mm
        private int minSize = 50;

        public int MaxLength { get => maxLength; set => maxLength = value; }
        public int MinLength { get => minLength; set => minLength = value; }
        public int MaxSize { get => maxSize; set => maxSize = value; }
        public int MinSize { get => minSize; set => minSize = value; }
    }
}