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

        private void MosaicBtn_Click(object sender, EventArgs e)
        {
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
                        var mosaic = new Mosaic(dot, pic.GetColor(dot));
                        pic.GetPixel(dot).Scanned = true;
                        ScanLiner(pic, mosaic, queue);
                        mosaics.Add(mosaic);
                    }
                }
            //Form entire mosaic picture
            Bitmap bitmap = new Bitmap(this.sourceImage.Image.Width, this.sourceImage.Image.Height);
            foreach (Mosaic mosaic in mosaics)
            {
                foreach(Dot dot in mosaic.Dots)
                {
                    Color color = Color.FromArgb(0, 0, 0);// mosaic.Color.Red, mosaic.Color.Green, mosaic.Color.Blue);
                    bitmap.SetPixel(dot.X, dot.Y, color);
                }
                //this.AfterImage.Image = bitmap;
                //MessageBox.Show("Next!");
            }
            //Display mosaic picture 
            this.MosaicImage.Image = bitmap;
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
            if (this.sizeCb.Checked)
            {
                this.sourceImage.SizeMode = PictureBoxSizeMode.AutoSize;
                this.MosaicImage.SizeMode = PictureBoxSizeMode.AutoSize;
            }
            else
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
            long checkPoint = DateTime.Now.Ticks;
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
            this.MessageTb.Text = $"It took {DateTime.Now.Ticks - checkPoint}";
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
            foreach (Mosaic mosaic in mosaics)
            {
                if (mosaic.Dots.Count < 101) continue;
                foreach (Dot dot in mosaic.Dots)
                {
                    Color color = Color.FromArgb(0, 0, 0);// mosaic.Color.Red, mosaic.Color.Green, mosaic.Color.Blue);
                    bitmap.SetPixel(dot.X, dot.Y, color);
                }
              }
            //Display mosaic picture 
            picBox.Image = bitmap;
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
                        var mosaic = new Mosaic(dot, pic.GetColor(dot));
                        pic.GetPixel(dot).Scanned = true;
                        ScanLiner(pic, mosaic, queue, offset);
                        mosaics.Add(mosaic);
                    }
                }
            return mosaics;
        }
    }
}
