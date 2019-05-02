using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicArtCreator.Mosaic
{
    public class Processor
    {
        //Check if the dot is inside of picture and not scanned yet, and has similar color
        private bool CheckIfDotValidAndSimilarColor(Picture pic, Outline outline, Dot dot, RGBColor diff, List<Pixel> scannedPixels)
        {
            if (pic.IsInside(dot))
                if (pic.IsUnscanned(dot))
                {
                    pic.GetPixel(dot).Scanned = true;
                    scannedPixels.Add(pic.GetPixel(dot));
                    if (outline.Color.IsSimilar(pic.GetColor(dot), diff))
                    {
                        return true;
                    }
                    else
                    {
                        if (!outline.Dots.Exists(d => d.X == dot.X && d.Y == dot.Y))
                            outline.Dots.Add(dot);
                    }
                }
            return false;
        }

        //Scan the pixels around the starting pixel and outline the area that consists of similiar colors
        public void OutlinePieceFromMinDiff(Picture pic, Outline outline, Dot startDot, Profile.Profile profile)
        {
            Queue<Dot> queue = new Queue<Dot>();
            queue.Enqueue(startDot);
            RGBColor diff = new RGBColor(profile.MinDiff);
            while (diff.Red <= profile.MaxDiff)
            {
                var scannedPixels = new List<Pixel>();
                while (queue.Count > 0)
                {
                    Dot dot = queue.Dequeue();
                    //outline.Color = pic.GetColor(dot);
                    //Scan the up dot if it is wthin the picture and unscanned
                    Dot nextDot = dot.GetLeftDot();
                    if (CheckIfDotValidAndSimilarColor(pic, outline, nextDot, diff, scannedPixels)) queue.Enqueue(nextDot);
                    nextDot = dot.GetUpDot();
                    if (CheckIfDotValidAndSimilarColor(pic, outline, nextDot, diff, scannedPixels)) queue.Enqueue(nextDot);
                    nextDot = dot.GetRightDot();
                    if (CheckIfDotValidAndSimilarColor(pic, outline, nextDot, diff, scannedPixels)) queue.Enqueue(nextDot);
                    nextDot = dot.GetDownDot();
                    if (CheckIfDotValidAndSimilarColor(pic, outline, nextDot, diff, scannedPixels)) queue.Enqueue(nextDot);
                }
                if (scannedPixels.Count < profile.Piece.MinSize)
                {
                    //Re-do it if the diff is less max diff
                    //Exit if diff is already max diff
                    if (diff.Red >= profile.MaxDiff) return;
                    //Recalculate diff
                    diff = new RGBColor(diff.Red + profile.DiffIncement);
                    //Reset the pixels and queue
                    outline.Dots.Clear();
                    foreach (Pixel pixel in scannedPixels) pixel.Scanned = false;
                    queue.Clear();
                    queue.Enqueue(startDot);
                }
                else
                {
                    return;
                }
            }
        }

        //Scan the pixels around the starting pixel and outline the area that consists of similiar colors
        public void OutlinePieceFromMaxDiff(Picture pic, Outline outline, Dot startDot, Profile.Profile profile)
        {
            Queue<Dot> queue = new Queue<Dot>();
            queue.Enqueue(startDot);
            RGBColor diff = new RGBColor(profile.MaxDiff);
            while (queue.Count > 0)
            {
                var scannedPixels = new List<Pixel>();
                bool keepBigPiece = false;
                while (queue.Count > 0)
                {
                    Dot dot = queue.Dequeue();
                    //Scan the up dot if it is wthin the picture and unscanned
                    Dot nextDot = dot.GetLeftDot();
                    if (CheckIfDotValidAndSimilarColor(pic, outline, nextDot, diff, scannedPixels)) queue.Enqueue(nextDot);
                    nextDot = dot.GetUpDot();
                    if (CheckIfDotValidAndSimilarColor(pic, outline, nextDot, diff, scannedPixels)) queue.Enqueue(nextDot);
                    nextDot = dot.GetRightDot();
                    if (CheckIfDotValidAndSimilarColor(pic, outline, nextDot, diff, scannedPixels)) queue.Enqueue(nextDot);
                    nextDot = dot.GetDownDot();
                    if (CheckIfDotValidAndSimilarColor(pic, outline, nextDot, diff, scannedPixels)) queue.Enqueue(nextDot);
                    //Check to see if current mosaic piece is too big, means diff is too big. Decrease the diff and re-do outline
                    if (!keepBigPiece && scannedPixels.Count > profile.Piece.MaxSize && queue.Count > 0)
                    {
                        //Re-do it if the diff is not close to min diff
                        if (diff.Red > profile.MinDiff)
                        {
                            //Recalculate diff
                            diff = new RGBColor((diff.Red - profile.DiffIncement) > profile.MinDiff ? diff.Red - profile.DiffIncement : profile.MinDiff);
                            //Reset the pixels and queue
                            outline.Dots.Clear();
                            foreach (Pixel pixel in scannedPixels) pixel.Scanned = false;
                            queue.Clear();
                            queue.Enqueue(startDot);
                            break;
                        }
                        else
                        {
                            keepBigPiece = true;
                        }
                    }
                }
                if (keepBigPiece)
                {
                    //Todo - Break the current big piece into multiple smaller pieces
                }
            }
        }
    }
}
