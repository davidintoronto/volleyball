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
                 this.SourceImage.Image = bitmap;
                //this.AfterImage.Image = bitmap;
                this.MessageTb.Text = $"Bitmap: {bitmap.Width} / {bitmap.Height}, Image: {this.SourceImage.Image.Width} / {this.SourceImage.Image.Height}, Image location {this.SourceImage.Location.X}/{this.SourceImage.Location.Y}\r\n";
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

        private Picture LoadPicture(Bitmap bitmap)
        {
            Picture pic = new Picture(bitmap.Width, bitmap.Height);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    RGBColor color;
                    if (this.greyCb.Checked)
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
            Picture pic = LoadPicture(new Bitmap(this.SourceImage.Image));
            var mosaics = new List<Mosaic>();
            for(int x=1; x<pic.Width; x++)
                for (int y=1; y<pic.Height; y++)
                {
                    Dot dot = new Dot(x, y);
                    if (pic.IsUnscanned(dot))
                    {
                        var mosaic = new Mosaic(dot, pic.GetColor(dot));
                        ScanLiner(pic, mosaic, dot);
                        mosaics.Add(mosaic);
                    }
                }
            //Form entire mosaic picture
            Bitmap bitmap = new Bitmap(this.SourceImage.Image.Width, this.SourceImage.Image.Height);
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
            MessageBox.Show("Done!");
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

        private void ScanDot(Picture pic, Mosaic mosaic, Dot dot)
        {
            if (pic.IsInside(dot))
                if (pic.IsUnscanned(dot))
                {
                    pic.GetPixel(dot).Scanned = true;
                    if (!mosaic.IsSimilarColor(dot, pic.GetColor(dot), this.redCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0, this.greenCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0, this.blueCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0))
                    {
                        mosaic.Dots.Add(dot);
                    }
                    else
                    {
                        ScanLiner(pic, mosaic, dot);
                    }

                }
        }
        private void ScanLiner(Picture pic, Mosaic mosaic, Dot dot)
        {
            //Scan the up dot if it is wthin the picture and unscanned
            Dot nextDot = dot.GetLeftDot();
            ScanDot(pic, mosaic, nextDot);
            nextDot = dot.GetUpDot();
            ScanDot(pic, mosaic, nextDot);
            nextDot = dot.GetRightDot();
            ScanDot(pic, mosaic, nextDot);
            nextDot = dot.GetDownDot();
            ScanDot(pic, mosaic, nextDot);
        }

        private void SourceImage_MouseClick(object sender, MouseEventArgs e)
        {
            Bitmap bitmap = new Bitmap(this.SourceImage.Image);
          // this.SourceImage.ImageLocation
            Color color = bitmap.GetPixel(e.X, e.Y);
            Picture pic = LoadPicture(new Bitmap(this.SourceImage.Image));
            Mosaic mosaic = new Mosaic();
            mosaic.Color = new RGBColor(color.R, color.G, color.B);
            ScanLiner(pic, mosaic, new Dot(e.X, e.Y));
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
            Picture pic = LoadPicture(new Bitmap(this.SourceImage.Image));
            var mosaic = new Mosaic();
            //Vertical process
            for (int x = 0; x < pic.Width; x++)
            {
                mosaic.Color = pic.GetColor(x, 0);
                for (int y = 1; y < pic.Height; y++)
                {
                    Dot dot = new Dot(x, y);
                    var color = pic.GetColor(dot);
                    if (this.greenCb.Checked)
                    {
                        if (!mosaic.IsSimilarColor(dot, color, Convert.ToInt32(this.greyOffset.Value), Convert.ToInt32(this.greyOffset.Value), Convert.ToInt32(this.greyOffset.Value)))
                        {
                            mosaic.Dots.Add(dot);
                        }
                    }
                    else
                    {
                        if (!mosaic.IsSimilarColor(dot, color, this.redCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0, this.greenCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0, this.blueCb.Checked ? Convert.ToInt32(this.redOffset.Value) : 0))
                        // if (!mosaic.IsSimilarColor(dot, color, Convert.ToInt32(this.redOffset.Value), Convert.ToInt32(this.greenOffset.Value), Convert.ToInt32(this.blueOffset.Value)))
                        {
                            mosaic.Dots.Add(dot);
                        }
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
                    if (!mosaic.IsSimilarColor(dot, color, Convert.ToInt32(this.redOffset.Value), Convert.ToInt32(this.greenOffset.Value), Convert.ToInt32(this.blueOffset.Value)))
                    {
                        mosaic.Dots.Add(dot);
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
            Bitmap bitmap = new Bitmap(this.SourceImage.Image.Width, this.SourceImage.Image.Height);
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

        private void NormalSizeCb_CheckedChanged(object sender, EventArgs e)
        {
            if (this.normalSizeCb.Checked)
                this.SourceImage.SizeMode = PictureBoxSizeMode.Normal;
            else
                this.SourceImage.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
