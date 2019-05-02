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
using MosaicArtCreator.Mosaic;
using MosaicArtCreator.Settings;
using MosaicArtCreator.Profile;

namespace MosaicArtCreator
{
    public partial class MosaicWorkshop : Form
    {
        private Configures config = new Configures();
        private Processor processor = new Processor();
        public MosaicWorkshop()
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
                this.sourceImageFile.Text = openFileDialog.FileName;
                // display image in picture box  
                Bitmap bitmap = new Bitmap(openFileDialog.FileName);
                 this.sourcePicBox.Image = bitmap;
                //this.AfterImage.Image = bitmap;
                this.MessageTb.Text = $"Bitmap: {bitmap.Width} / {bitmap.Height}, Image: {this.sourcePicBox.Image.Width} / {this.sourcePicBox.Image.Height}, Image location {this.sourcePicBox.Location.X}/{this.sourcePicBox.Location.Y}\r\n";
            }
        }

        private Profile.Profile GetProfile()
        {
            Profile.Profile profile = new Profile.Profile();
            profile.MaxDiff = int.Parse(this.maxColorDiff.Text);
            profile.MinDiff = int.Parse(this.minColorDiff.Text);
            profile.DiffIncement = int.Parse(this.colorDiffIncrement.Text);
            profile.Piece.MaxSize = int.Parse(this.pieceMaxSize.Text);
            profile.Piece.MinSize = int.Parse(this.pieceMinSize.Text);
            return profile;
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            if (sourcePicBox.Image == null)
            {
                MessageBox.Show("Please open the source image !", "Error");
                return;
            }
            Picture pic = ImageUtils.LoadPicture(new Bitmap(this.sourcePicBox.Image));
            var profile = GetProfile();
            var outlines = new List<Outline>();
            for(int x=1; x<pic.Width; x++)
                for (int y=1; y<pic.Height; y++)
                {
                    Dot dot = new Dot(x, y);
                    if (pic.IsUnscanned(dot))
                    {
                        var outline = new Outline();
                        outline.Color =pic.GetColor(dot);
                        pic.GetPixel(dot).Scanned = true;
                        processor.OutlinePieceFromMinDiff(pic, outline, dot, profile);
                        outlines.Add(outline);
                    }
                }
            //Form entire mosaic picture
            ShowMosaicInPictureBox(this.mosaicPicBox, outlines);
            ShowMosaicInPictureBox(this.savedMosaicPicBox1, outlines);
            this.MessageTb.Text = $"Picture is mosaic-ed {DateTime.Now.ToLongTimeString()}";
        }
        private void CreateBtn_ClickbigToSmall(object sender, EventArgs e)
        {
            if (sourcePicBox.Image == null)
            {
                MessageBox.Show("Please open the source image !", "Error");
                return;
            }
            Picture pic = ImageUtils.LoadPicture(new Bitmap(this.sourcePicBox.Image));
            var profile = GetProfile();
            var outlines = new List<Outline>();
            for (int x = 1; x < pic.Width; x++)
                for (int y = 1; y < pic.Height; y++)
                {
                    Dot dot = new Dot(x, y);
                    if (pic.IsUnscanned(dot))
                    {
                        var outline = new Outline();
                        outline.Color = pic.GetColor(dot);
                        pic.GetPixel(dot).Scanned = true;
                        processor.OutlinePieceFromMaxDiff(pic, outline, dot, profile);
                        outlines.Add(outline);
                    }
                }
            //Form entire mosaic picture
            ShowMosaicInPictureBox(this.mosaicPicBox, outlines);
            ShowMosaicInPictureBox(this.savedMosaicPicBox2, outlines);
            this.MessageTb.Text = $"Picture is mosaic-ed {DateTime.Now.ToLongTimeString()}";
        }


        private void SourceImage_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

       
        private void MosaicImage_MouseClick(object sender, MouseEventArgs e)
        {
        }

 
        private void OffsetMosaic_DoubleClick(object sender, EventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            this.mosaicPicBox.Image = picBox.Image;
        }


        private void ShowMosaicInPictureBox(PictureBox picBox, List<Outline> outlines)
        {
            //Form entire mosaic picture
            Bitmap bitmap = null;
            if (this.showSourceImageCb.Checked)
            {
                bitmap = new Bitmap(this.sourcePicBox.Image);
            }
            else
            {
                bitmap = new Bitmap(this.sourcePicBox.Image.Width, this.sourcePicBox.Image.Height);
            }
            Color color = groutColor.BackColor;//.FromArgb(0, 0, 0);// mosaic.Color.Red, mosaic.Color.Green, mosaic.Color.Blue);
            foreach (Outline outline in outlines)
            {
                foreach (Dot dot in outline.Dots)
                {
                    bitmap.SetPixel(dot.X, dot.Y, color);
                }
            }
            //picBox.Tag = bitmap;
            //Display mosaic picture 
            picBox.Image = bitmap;
            // picBox.Image = FilterIsolatedDots(new Bitmap(picBox.Image));
        }

        private void SizeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.sourcePicBox.SizeMode = (PictureBoxSizeMode)this.sizeMode.SelectedItem;
            this.mosaicPicBox.SizeMode = (PictureBoxSizeMode)this.sizeMode.SelectedItem;

        }

        private void GroutColor_DoubleClick(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.groutColor.Text = dlg.Color.Name;
                this.groutColor.BackColor = dlg.Color;
            }
        }

        private void NewBtn_Click(object sender, EventArgs e)
        {
            this.basicInfoGb.Visible = true;
            this.filesGb.Visible = true;
            this.MosaicGb.Visible = true;
            this.PiecesGb.Visible = true;
            this.tilesGb.Visible = true;
            this.colorDiffGb.Visible = true;
        }

        private void MosaicPicBox_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
