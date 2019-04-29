using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MosiacArtEditor
{
    public partial class MosaicWorkshops : Form
    {
        public MosaicWorkshops()
        {
            InitializeComponent();
            this.sizeMode.Items.Add(PictureBoxSizeMode.AutoSize);
            this.sizeMode.Items.Add(PictureBoxSizeMode.CenterImage);
            this.sizeMode.Items.Add(PictureBoxSizeMode.Normal);
            this.sizeMode.Items.Add(PictureBoxSizeMode.StretchImage);
            this.sizeMode.Items.Add(PictureBoxSizeMode.Zoom);
        }

        //Load picture and show it in before panel
        private void OpenBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                Bitmap bitmap = new Bitmap(openFileDialog.FileName);
                 this.sourceImage.Image = bitmap;
                //this.AfterImage.Image = bitmap;
                this.MessageTb.Text = $"Bitmap: {bitmap.Width} / {bitmap.Height}, Image: {this.sourceImage.Image.Width} / {this.sourceImage.Image.Height}, Image location {this.sourceImage.Location.X}/{this.sourceImage.Location.Y}\r\n";
            }
        }

        private void RGBToGray(Bitmap bitmap)
        {
            for(int x=0;x<bitmap.Width;x++)
             for(int y=0;y<bitmap.Height;y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    int gray = Convert.ToInt32(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
                    bitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
        }

        private Picture LoadPicture(Bitmap bitmap, bool toGrey)
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
        private Picture LoadPicture(List<Mosaic> mosaics, int width, int height)
        {
            Picture pic = new Picture(width, height);
            foreach (Mosaic mosaic in mosaics)
            {
                //if (mosaic.Dots.Count < 101) continue;
                foreach (Dot dot in mosaic.Dots)
                {
                    //Color color = Color.FromArgb(0, 0, 0);// mosaic.Color.Red, mosaic.Color.Green, mosaic.Color.Blue);
                    pic.SetPixel(dot, new RGBColor(0,0,0));
                }
            }

            return pic;
        }

        private void MosaicBtn_Click(object sender, EventArgs e)
        {
            if (sourceImage.Image == null) return;
            Picture pic = LoadPicture(new Bitmap(this.sourceImage.Image), this.greyCb.Checked);
            var mosaics = new List<Mosaic>();
            for(int x=1; x<pic.Width; x++)
                for (int y=1; y<pic.Height; y++)
                {
                    Dot dot = new Dot(x, y);
                    if (pic.IsUnscanned(dot))
                    {
                        Queue<Dot> queue = new Queue<Dot>();
                        queue.Enqueue(dot);
                        var mosaic = new Mosaic();
                        mosaic.Color =pic.GetColor(dot);
                        pic.GetPixel(dot).Scanned = true;
                        int multiple = 255 / Convert.ToInt32(this.redOffset.Value) /5 ;
                        RGBColor offset = new RGBColor(Convert.ToInt32(this.redOffset.Value) * multiple, Convert.ToInt32(this.greenOffset.Value) * multiple, Convert.ToInt32(this.blueOffset.Value) * multiple);
                        while (!ScanLiner(pic, mosaic, queue, offset, Convert.ToInt32(this.maxPixels.Value)))
                        {
                            multiple--;
                            offset = new RGBColor(Convert.ToInt32(this.redOffset.Value) * multiple, Convert.ToInt32(this.greenOffset.Value) * multiple, Convert.ToInt32(this.blueOffset.Value) * multiple);
                            mosaic.Dots.Clear();
                            queue.Clear();
                            queue.Enqueue(dot);
                        }
                        mosaics.Add(mosaic);
                    }
                }
            //Form entire mosaic picture
            ShowMosaicInPictureBox(this.MosaicImage, mosaics);
            this.MessageTb.Text = $"Picture is mosaic-ed {DateTime.Now.ToLongTimeString()}";
        }

        private void ScanNextDot(Picture pic, Mosaic mosaic, Dot dot, RGBColor color)
        {
            if (mosaic.IsSimilarColor(dot, color, Convert.ToInt32(this.redOffset.Value), Convert.ToInt32(this.greenOffset.Value), Convert.ToInt32(this.blueOffset.Value)))
            {
                mosaic.AddIntoMosaicPiece(dot, color);
                pic.GetPixel(dot).Scanned = true;
                if (mosaic.Dots.Count >= 1000) return;
                ScanAround(pic, dot, mosaic);
            }
        }

        private void ScanAround(Picture pic, Dot dot, Mosaic mosaic)
        {
            //Scan the up dot if it is wthin the picture and unscanned
            if (pic.IsInside(dot.GetUpDot()) && pic.IsUnscanned(dot.GetUpDot()))
            {
                Dot nextDot = dot.GetUpDot();
                RGBColor color = pic.GetColor(nextDot);
                ScanNextDot(pic, mosaic, nextDot, color);
            }
            //Scan the right dot if it is wthin the picture and unscanned
            if (pic.IsInside(dot.GetRightDot()) && pic.IsUnscanned(dot.GetRightDot()))
            {
                Dot nextDot = dot.GetRightDot();
                RGBColor color = pic.GetColor(nextDot);
                ScanNextDot(pic, mosaic, nextDot, color);
            }
            //Scan the down dot if it is wthin the picture and unscanned
            if (pic.IsInside(dot.GetDownDot()) && pic.IsUnscanned(dot.GetDownDot()))
            {
                Dot nextDot = dot.GetDownDot();
                RGBColor color = pic.GetColor(nextDot);
                ScanNextDot(pic, mosaic, nextDot, color);
            }
            //Scan the left dot if it is wthin the picture and unscanned
            if (pic.IsInside(dot.GetLeftDot()) && pic.IsUnscanned(dot.GetLeftDot()))
            {
                Dot nextDot = dot.GetLeftDot();
                RGBColor color = pic.GetColor(nextDot);
                ScanNextDot(pic, mosaic, nextDot, color);
            }
        }

        //Return true if reaching the picture edge or liner
        private bool ScanDot1(Picture pic, Mosaic mosaic, Dot dot)
        {
            if (!pic.IsInside(dot) || pic.IsScanned(dot)) return true;
            pic.GetPixel(dot).Scanned = true;
            if (!mosaic.IsSimilarColor(dot, pic.GetColor(dot), this.redCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0, this.greenCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0, this.blueCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0))
            {
                mosaic.Dots.Add(dot);
                return true;
            }
            return false;

        }

        //Check if the dot is inside of picture and not scanned yet, and has similar color
        private bool CheckIfDotValidAndSimilarColor(Picture pic, Mosaic mosaic, Dot dot, RGBColor offset)
        {
            if (pic.IsInside(dot))
                if (pic.IsUnscanned(dot))
                {
                    pic.GetPixel(dot).Scanned = true;
                    if (IsSimilarColor(mosaic, dot, pic.GetColor(dot), offset))
                    {
                        return true;
                    }
                    else 
                    {
                         if (!mosaic.Dots.Exists(d=>d.X == dot.X && d.Y == dot.Y))
                            mosaic.Dots.Add(dot);
                   }
                }
            return false;
        }
        private bool CheckIfDotValidAndSimilarColor(Picture pic, Mosaic mosaic, Dot dot, RGBColor offset, List<Pixel> scannedPixels)
        {
            if (pic.IsInside(dot))
                if (pic.IsUnscanned(dot))
                {
                    pic.GetPixel(dot).Scanned = true;
                    scannedPixels.Add(pic.GetPixel(dot));
                    if (IsSimilarColor(mosaic, dot, pic.GetColor(dot), offset))
                    {
                        return true;
                    }
                    else
                    {
                        if (!mosaic.Dots.Exists(d => d.X == dot.X && d.Y == dot.Y))
                            mosaic.Dots.Add(dot);
                    }
                }
            return false;
        }
        private void ScanLiner(Picture pic, Mosaic mosaic, Queue<Dot> queue)
        {
            ScanLiner(pic, mosaic, queue, null);
        }

        private void ScanLiner(Picture pic, Mosaic mosaic, Queue<Dot> queue, RGBColor offset)
        {
            while (queue.Count > 0)
            {
                Dot dot = queue.Dequeue();
                //Scan the up dot if it is wthin the picture and unscanned
                Dot nextDot = dot.GetLeftDot();
                if (CheckIfDotValidAndSimilarColor(pic, mosaic, nextDot, offset)) queue.Enqueue(nextDot);
                nextDot = dot.GetUpDot();
                if (CheckIfDotValidAndSimilarColor(pic, mosaic, nextDot, offset)) queue.Enqueue(nextDot);
                nextDot = dot.GetRightDot();
                if (CheckIfDotValidAndSimilarColor(pic, mosaic, nextDot, offset)) queue.Enqueue(nextDot);
                nextDot = dot.GetDownDot();
                if (CheckIfDotValidAndSimilarColor(pic, mosaic, nextDot, offset)) queue.Enqueue(nextDot);
            }
        }

        private bool ScanLiner(Picture pic, Mosaic mosaic, Queue<Dot> queue, RGBColor offset, int maxPixels)
        {
            var scannedPixels = new List<Pixel>();
            while (queue.Count > 0)
            {
                Dot dot = queue.Dequeue();
                //Scan the up dot if it is wthin the picture and unscanned
                Dot nextDot = dot.GetLeftDot();
                if (CheckIfDotValidAndSimilarColor(pic, mosaic, nextDot, offset, scannedPixels)) queue.Enqueue(nextDot);
                nextDot = dot.GetUpDot();
                if (CheckIfDotValidAndSimilarColor(pic, mosaic, nextDot, offset, scannedPixels)) queue.Enqueue(nextDot);
                nextDot = dot.GetRightDot();
                if (CheckIfDotValidAndSimilarColor(pic, mosaic, nextDot, offset, scannedPixels)) queue.Enqueue(nextDot);
                nextDot = dot.GetDownDot();
                if (CheckIfDotValidAndSimilarColor(pic, mosaic, nextDot, offset, scannedPixels)) queue.Enqueue(nextDot);
                //Check to see if current mosaic piece is too big
                if (scannedPixels.Count > maxPixels && queue.Count > 0)
                {
                    //Reset the scanned pixels
                    foreach (Pixel pixel in scannedPixels) pixel.Scanned = false;
                    return false;
                }
            }
            return true;
        }

        private void SourceImage_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Bitmap bitmap = new Bitmap(this.sourceImage.Image);
          // this.SourceImage.ImageLocation
            Color color = bitmap.GetPixel(e.X, e.Y);
            Picture pic = LoadPicture(new Bitmap(this.sourceImage.Image), false);
            Mosaic mosaic = new Mosaic();
            Queue<Dot> queue = new Queue<Dot>();
            queue.Enqueue(new Dot(e.X, e.Y));
            mosaic.Color = new RGBColor(color.R, color.G, color.B);
            ScanLiner(pic, mosaic, queue);
            ShowMosaic(mosaic);
            this.MessageTb.Text = this.MessageTb.Text + $"Mouse location {e.Location}, Color {color.R}/{color.G}/{color.B}\r\n";
        }

       
        private void AfterImage_MouseClick(object sender, MouseEventArgs e)
        {
              Bitmap bitmap = new Bitmap(this.MosaicImage.Image);
            Color color = bitmap.GetPixel(e.X, e.Y);
            this.MessageTb.Text = this.MessageTb.Text + $"Mouse location {e.Location}, Color {color.R}/{color.G}/{color.B}\r\n";
        }

        private void LinerBtn_Click(object sender, EventArgs e)
        {
            Picture pic = LoadPicture(new Bitmap(this.sourceImage.Image), this.greyCb.Checked);
            var mosaic = new Mosaic();
            //Vertical process
            for (int x = 0; x < pic.Width; x++)
            {
                mosaic.Color = pic.GetColor(x, 0);
                for (int y = 1; y < pic.Height; y++)
                {
                    Dot dot = new Dot(x, y);
                    var color = pic.GetColor(dot);
                    if (!IsSimilarColor(mosaic, dot, color, null))
                    {
                        mosaic.Dots.Add(dot);
                        mosaic.Color = color;
                    }
                    mosaic.Color = color;
                }
            }
            //Horizontal process
            for (int y = 0; y < pic.Height; y++)
            {
                mosaic.Color = pic.GetColor(0, y);
                for (int x = 1; x < pic.Width; x++)
                {
                    Dot dot = new Dot(x, y);
                    var color = pic.GetColor(dot);
                    if (!IsSimilarColor(mosaic, dot, color, null))
                    {
                        mosaic.Dots.Add(dot);
                        mosaic.Color = color;
                    }
                    mosaic.Color = color;
                }
            }
            //Show
            ShowMosaic(mosaic);
        }

        private void ShowMosaic(Mosaic mosaic)
        { 
            //Form entire mosaic picture
            Bitmap bitmap = new Bitmap(this.sourceImage.Image.Width, this.sourceImage.Image.Height);
            foreach (Dot dot in mosaic.Dots)
            {
                Color color = Color.FromArgb(0,0,0);
                bitmap.SetPixel(dot.X, dot.Y, color);
            }
            //this.AfterImage.Image = bitmap;
            //MessageBox.Show("Next!");
            //Display mosaic picture 
            this.MosaicImage.Image = bitmap;
        }

        private void SizeCb_CheckedChanged(object sender, EventArgs e)
        {
       //     if (this.sizeType.GetItemText == "Auto Size")
            {
                this.sourceImage.SizeMode = PictureBoxSizeMode.AutoSize;
                this.MosaicImage.SizeMode = PictureBoxSizeMode.AutoSize;
            }
         //   else
            {
                this.sourceImage.SizeMode = PictureBoxSizeMode.Zoom;
                this.MosaicImage.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private bool IsSimilarColor(Mosaic mosaic, Dot dot, RGBColor color, RGBColor offset)
        {
            if (offset == null) //Use pre-fined offset
            {
                if (this.greyCb.Checked)
                {
                    if (mosaic.IsSimilarColor(dot, color, Convert.ToInt32(this.greyOffset.Value), Convert.ToInt32(this.greyOffset.Value), Convert.ToInt32(this.greyOffset.Value)))
                    {
                        return true;
                    }
                }
                else
                {
                    if (mosaic.IsSimilarColor(dot, color, this.redCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0, this.greenCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0, this.blueCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0))
                    // if (!mosaic.IsSimilarColor(dot, color, Convert.ToInt32(this.redOffset.Value), Convert.ToInt32(this.greenOffset.Value), Convert.ToInt32(this.blueOffset.Value)))
                    {
                        return true;
                    }
                }
            }
            else if (offset.Red == 0 && offset.Green == 0 && offset.Blue == 0) //Use optimize offset
            {
                if (mosaic.IsSimilarColor(dot, color, optimizeOffset(color.Red), optimizeOffset(color.Green), optimizeOffset(color.Blue)))
                {
                    return true;
                }
            }
            else if (mosaic.IsSimilarColor(dot, color, offset.Red, offset.Green, offset.Blue))
            {
                return true;
            }
            return false;
        }

        private int optimizeOffset(int colorValue)
        {
            return colorValue * 2 /10 + 5;
        }

        private void OffsetMosaic_DoubleClick(object sender, EventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            this.MosaicImage.Image = picBox.Image;
        }

        private void MosaicAllBtn_Click(object sender, EventArgs e)
        {
            if (sourceImage.Image == null) return;
            DateTime checkPoint = DateTime.Now;
            //Small offset mosaic
            var mosaics = CreateMosaic(new RGBColor(Convert.ToInt32(this.redOffset.Value), Convert.ToInt32(this.greenOffset.Value), Convert.ToInt32(this.blueOffset.Value)), false);
            ShowMosaicInPictureBox(this.smallOffsetMosaic, mosaics);
            //Big offset mosaic
            mosaics = CreateMosaic(new RGBColor(Convert.ToInt32(this.redOffset.Value * this.amplify.Value), Convert.ToInt32(this.greenOffset.Value * this.amplify.Value), Convert.ToInt32(this.blueOffset.Value * this.amplify.Value)), false);
            ShowMosaicInPictureBox(this.bigOffsetMasaic, mosaics);
            //Optimize offset mosaic
            mosaics = CreateMosaic(new RGBColor(0,0,0), false);
            ShowMosaicInPictureBox(this.optimizeOffsetMosaic, mosaics);
            //Small grey offset mosaic
            mosaics = CreateMosaic(new RGBColor(Convert.ToInt32(this.greyOffset.Value), Convert.ToInt32(this.greyOffset.Value), Convert.ToInt32(this.greyOffset.Value)), false);
            ShowMosaicInPictureBox(this.greySmallOffsetMosaic, mosaics);
            //Big grey offset mosaic
            mosaics = CreateMosaic(new RGBColor(Convert.ToInt32(this.greyOffset.Value * this.amplify.Value), Convert.ToInt32(this.greyOffset.Value * this.amplify.Value), Convert.ToInt32(this.greyOffset.Value * this.amplify.Value)), false);
            ShowMosaicInPictureBox(this.greyBigOffsetMosaic, mosaics);
            //Optimize grey offset mosaic
            mosaics = CreateMosaic(new RGBColor(0, 0, 0), false);
            ShowMosaicInPictureBox(this.greyOptimizeOffsetMosaic, mosaics);
            this.MessageTb.Text = $"It took {(DateTime.Now - checkPoint).TotalMilliseconds}";
        }

        private void ShowMosaicInPictureBox(PictureBox picBox, List<Mosaic> mosaics)
        {
            //Form entire mosaic picture
            Bitmap bitmap = null;
            if (this.showSourceImageCb.Checked)
            {
                bitmap = new Bitmap(this.sourceImage.Image);
            }
            else
            {
                bitmap = new Bitmap(this.sourceImage.Image.Width, this.sourceImage.Image.Height);
            }
            //Call filter of isolated liner
            var filterdMosaics = mosaics;// FilterIsolatedDots(mosaics);
            foreach (Mosaic mosaic in filterdMosaics)
            {
                //if (mosaic.Dots.Count < 101) continue;
                foreach (Dot dot in mosaic.Dots)
                {
                    Color color = Color.FromArgb(0, 0, 0);// mosaic.Color.Red, mosaic.Color.Green, mosaic.Color.Blue);
                    bitmap.SetPixel(dot.X, dot.Y, color);
                }
              }

            //Display mosaic picture 
            picBox.Image = bitmap;
           // picBox.Image = FilterIsolatedDots(new Bitmap(picBox.Image));
        }

        private List<Mosaic> FilterIsolatedDots(List<Mosaic> mosaics)
        {
            //Create Picture object that only contains the liner dots
            Picture pic = LoadPicture(mosaics, this.sourceImage.Image.Width, this.sourceImage.Image.Height);
            mosaics = new List<Mosaic>();
            for (int x = 0; x < pic.Width; x++)
                for (int y = 0; y < pic.Height; y++)
                {
                    Dot dot = new Dot(x, y);
                    if (pic.GetPixel(dot) != null && pic.IsUnscanned(dot))
                    {
                        Queue<Dot> queue = new Queue<Dot>();
                        queue.Enqueue(dot);
                        var mosaic = new Mosaic(dot, pic.GetColor(dot));
                        pic.GetPixel(dot).Scanned = true;
                        ValidateLiner(pic, mosaic, queue);
                        if (mosaic.Dots.Count>10)
                        mosaics.Add(mosaic);
                    }
                }
            return mosaics;
        }

        private void ValidateLiner(Picture pic, Mosaic mosaic, Queue<Dot> queue)
        {
            while (queue.Count > 0)
            {
                Dot dot = queue.Dequeue();
                //Scan the up dot if it is wthin the picture and unscanned
                Dot nextDot = dot.GetLeftDot();
                if (CheckIfDotValidAndIsLinerColor(pic, mosaic, nextDot)) queue.Enqueue(nextDot);
                nextDot = dot.GetUpDot();
                if (CheckIfDotValidAndIsLinerColor(pic, mosaic, nextDot)) queue.Enqueue(nextDot);
                nextDot = dot.GetRightDot();
                if (CheckIfDotValidAndIsLinerColor(pic, mosaic, nextDot)) queue.Enqueue(nextDot);
                nextDot = dot.GetDownDot();
                if (CheckIfDotValidAndIsLinerColor(pic, mosaic, nextDot)) queue.Enqueue(nextDot);
              /*  nextDot = dot.GetUpLeftDot();
                if (CheckIfDotValidAndIsLinerColor(pic, mosaic, nextDot)) queue.Enqueue(nextDot);
                nextDot = dot.GetUpRightDot();
                if (CheckIfDotValidAndIsLinerColor(pic, mosaic, nextDot)) queue.Enqueue(nextDot);
                nextDot = dot.GetDownRightDot();
                if (CheckIfDotValidAndIsLinerColor(pic, mosaic, nextDot)) queue.Enqueue(nextDot);
                nextDot = dot.GetDownLeftDot();
                if (CheckIfDotValidAndIsLinerColor(pic, mosaic, nextDot)) queue.Enqueue(nextDot);
                */
            }
        }

        private bool CheckIfDotValidAndIsLinerColor(Picture pic, Mosaic mosaic, Dot dot)
        {
            if (pic.IsInside(dot) && pic.GetPixel(dot) != null)
                if (pic.IsUnscanned(dot))
                {
                    pic.GetPixel(dot).Scanned = true;
                    var dotColor = pic.GetColor(dot);
                    if (!mosaic.Dots.Exists(d => d.X == dot.X && d.Y == dot.Y))
                        mosaic.Dots.Add(dot);
                    return true;
                }
            return false;
        }

        private List<Mosaic> CreateMosaic(RGBColor offset, bool toGrey)
        { 
            Picture pic = LoadPicture(new Bitmap(this.sourceImage.Image), toGrey);
            var mosaics = new List<Mosaic>();
            for (int x = 1; x < pic.Width; x++)
                for (int y = 1; y < pic.Height; y++)
                {
                    Dot dot = new Dot(x, y);
                    if (pic.IsUnscanned(dot))
                    {
                        Queue<Dot> queue = new Queue<Dot>();
                        queue.Enqueue(dot);
                        var mosaic = new Mosaic();
                        mosaic.Color =pic.GetColor(dot);
                        pic.GetPixel(dot).Scanned = true;
                        ScanLiner(pic, mosaic, queue, offset);
                        mosaics.Add(mosaic);
                    }
                }
            return mosaics;
        }

        private void SizeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.sourceImage.SizeMode = (PictureBoxSizeMode)this.sizeMode.SelectedItem;
            this.MosaicImage.SizeMode = (PictureBoxSizeMode)this.sizeMode.SelectedItem;

        }
    }
}
