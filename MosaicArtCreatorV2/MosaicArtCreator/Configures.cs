using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MosaicArtCreator.Mosaic;

namespace MosaicArtCreator.Settings
{
    public class Configures
    {
        private List<RGBColor> colorDiffList = new List<RGBColor>();
        public Configures()
        {
            colorDiffList.Add(new RGBColor(5, 5, 5));
        }

     }
    public class ColorDiffs : IEnumerator
    {
        public RGBColor[] diffs = new RGBColor[] { new RGBColor(100, 100, 100), new RGBColor(80, 80, 80), //
            new RGBColor(60, 60, 60), new RGBColor(40, 40, 40), new RGBColor(30, 30, 30), new RGBColor(20, 20, 20),//
            new RGBColor(15, 15, 15) , new RGBColor(10, 10, 10) , new RGBColor(5, 5, 5) };

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public ColorDiffs(RGBColor[] list)
        {
            diffs = list;
        } 

        public bool MoveNext()
        {
            position++;
            return (position < diffs.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public RGBColor Current
        {
            get
            {
                try
                {
                    return diffs[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
