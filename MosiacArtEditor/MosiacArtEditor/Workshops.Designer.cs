namespace MosiacArtEditor
{
    partial class MosaicWorkshops
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StatusPanel = new System.Windows.Forms.Panel();
            this.greyOptimizeOffsetMosaic = new System.Windows.Forms.PictureBox();
            this.greyBigOffsetMosaic = new System.Windows.Forms.PictureBox();
            this.greySmallOffsetMosaic = new System.Windows.Forms.PictureBox();
            this.optimizeOffsetMosaic = new System.Windows.Forms.PictureBox();
            this.bigOffsetMasaic = new System.Windows.Forms.PictureBox();
            this.smallOffsetMosaic = new System.Windows.Forms.PictureBox();
            this.MessageTb = new System.Windows.Forms.TextBox();
            this.MosaicImage = new System.Windows.Forms.PictureBox();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.showSourceImageCb = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.amplify = new System.Windows.Forms.NumericUpDown();
            this.MosaicAllBtn = new System.Windows.Forms.Button();
            this.blueCb = new System.Windows.Forms.CheckBox();
            this.greenCb = new System.Windows.Forms.CheckBox();
            this.redCb = new System.Windows.Forms.CheckBox();
            this.greyOffset = new System.Windows.Forms.NumericUpDown();
            this.linerBtn = new System.Windows.Forms.Button();
            this.greyCb = new System.Windows.Forms.CheckBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.greenOffset = new System.Windows.Forms.NumericUpDown();
            this.redOffset = new System.Windows.Forms.NumericUpDown();
            this.blueOffset = new System.Windows.Forms.NumericUpDown();
            this.mosaicBtn = new System.Windows.Forms.Button();
            this.openBtn = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.PicturePanel = new System.Windows.Forms.SplitContainer();
            this.sourcePanel = new System.Windows.Forms.Panel();
            this.sourceImage = new System.Windows.Forms.PictureBox();
            this.mosaicPanel = new System.Windows.Forms.Panel();
            this.sizeMode = new System.Windows.Forms.ComboBox();
            this.maxPixels = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.StatusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greyOptimizeOffsetMosaic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greyBigOffsetMosaic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greySmallOffsetMosaic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optimizeOffsetMosaic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bigOffsetMasaic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.smallOffsetMosaic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MosaicImage)).BeginInit();
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amplify)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greyOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicturePanel)).BeginInit();
            this.PicturePanel.Panel1.SuspendLayout();
            this.PicturePanel.Panel2.SuspendLayout();
            this.PicturePanel.SuspendLayout();
            this.sourcePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceImage)).BeginInit();
            this.mosaicPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxPixels)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusPanel
            // 
            this.StatusPanel.Controls.Add(this.greyOptimizeOffsetMosaic);
            this.StatusPanel.Controls.Add(this.greyBigOffsetMosaic);
            this.StatusPanel.Controls.Add(this.greySmallOffsetMosaic);
            this.StatusPanel.Controls.Add(this.optimizeOffsetMosaic);
            this.StatusPanel.Controls.Add(this.bigOffsetMasaic);
            this.StatusPanel.Controls.Add(this.smallOffsetMosaic);
            this.StatusPanel.Controls.Add(this.MessageTb);
            this.StatusPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StatusPanel.Location = new System.Drawing.Point(0, 810);
            this.StatusPanel.Name = "StatusPanel";
            this.StatusPanel.Size = new System.Drawing.Size(1762, 173);
            this.StatusPanel.TabIndex = 1;
            // 
            // greyOptimizeOffsetMosaic
            // 
            this.greyOptimizeOffsetMosaic.Dock = System.Windows.Forms.DockStyle.Left;
            this.greyOptimizeOffsetMosaic.Location = new System.Drawing.Point(930, 0);
            this.greyOptimizeOffsetMosaic.Name = "greyOptimizeOffsetMosaic";
            this.greyOptimizeOffsetMosaic.Size = new System.Drawing.Size(186, 173);
            this.greyOptimizeOffsetMosaic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.greyOptimizeOffsetMosaic.TabIndex = 6;
            this.greyOptimizeOffsetMosaic.TabStop = false;
            this.greyOptimizeOffsetMosaic.DoubleClick += new System.EventHandler(this.OffsetMosaic_DoubleClick);
            // 
            // greyBigOffsetMosaic
            // 
            this.greyBigOffsetMosaic.Dock = System.Windows.Forms.DockStyle.Left;
            this.greyBigOffsetMosaic.Location = new System.Drawing.Point(744, 0);
            this.greyBigOffsetMosaic.Name = "greyBigOffsetMosaic";
            this.greyBigOffsetMosaic.Size = new System.Drawing.Size(186, 173);
            this.greyBigOffsetMosaic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.greyBigOffsetMosaic.TabIndex = 5;
            this.greyBigOffsetMosaic.TabStop = false;
            this.greyBigOffsetMosaic.DoubleClick += new System.EventHandler(this.OffsetMosaic_DoubleClick);
            // 
            // greySmallOffsetMosaic
            // 
            this.greySmallOffsetMosaic.Dock = System.Windows.Forms.DockStyle.Left;
            this.greySmallOffsetMosaic.Location = new System.Drawing.Point(558, 0);
            this.greySmallOffsetMosaic.Name = "greySmallOffsetMosaic";
            this.greySmallOffsetMosaic.Size = new System.Drawing.Size(186, 173);
            this.greySmallOffsetMosaic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.greySmallOffsetMosaic.TabIndex = 4;
            this.greySmallOffsetMosaic.TabStop = false;
            this.greySmallOffsetMosaic.DoubleClick += new System.EventHandler(this.OffsetMosaic_DoubleClick);
            // 
            // optimizeOffsetMosaic
            // 
            this.optimizeOffsetMosaic.Dock = System.Windows.Forms.DockStyle.Left;
            this.optimizeOffsetMosaic.Location = new System.Drawing.Point(372, 0);
            this.optimizeOffsetMosaic.Name = "optimizeOffsetMosaic";
            this.optimizeOffsetMosaic.Size = new System.Drawing.Size(186, 173);
            this.optimizeOffsetMosaic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.optimizeOffsetMosaic.TabIndex = 3;
            this.optimizeOffsetMosaic.TabStop = false;
            this.optimizeOffsetMosaic.DoubleClick += new System.EventHandler(this.OffsetMosaic_DoubleClick);
            // 
            // bigOffsetMasaic
            // 
            this.bigOffsetMasaic.Dock = System.Windows.Forms.DockStyle.Left;
            this.bigOffsetMasaic.Location = new System.Drawing.Point(186, 0);
            this.bigOffsetMasaic.Name = "bigOffsetMasaic";
            this.bigOffsetMasaic.Size = new System.Drawing.Size(186, 173);
            this.bigOffsetMasaic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bigOffsetMasaic.TabIndex = 2;
            this.bigOffsetMasaic.TabStop = false;
            this.bigOffsetMasaic.DoubleClick += new System.EventHandler(this.OffsetMosaic_DoubleClick);
            // 
            // smallOffsetMosaic
            // 
            this.smallOffsetMosaic.Dock = System.Windows.Forms.DockStyle.Left;
            this.smallOffsetMosaic.Location = new System.Drawing.Point(0, 0);
            this.smallOffsetMosaic.Name = "smallOffsetMosaic";
            this.smallOffsetMosaic.Size = new System.Drawing.Size(186, 173);
            this.smallOffsetMosaic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.smallOffsetMosaic.TabIndex = 1;
            this.smallOffsetMosaic.TabStop = false;
            this.smallOffsetMosaic.DoubleClick += new System.EventHandler(this.OffsetMosaic_DoubleClick);
            // 
            // MessageTb
            // 
            this.MessageTb.Dock = System.Windows.Forms.DockStyle.Right;
            this.MessageTb.Location = new System.Drawing.Point(1463, 0);
            this.MessageTb.Multiline = true;
            this.MessageTb.Name = "MessageTb";
            this.MessageTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MessageTb.Size = new System.Drawing.Size(299, 173);
            this.MessageTb.TabIndex = 0;
            // 
            // MosaicImage
            // 
            this.MosaicImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MosaicImage.Location = new System.Drawing.Point(0, 0);
            this.MosaicImage.Name = "MosaicImage";
            this.MosaicImage.Size = new System.Drawing.Size(808, 810);
            this.MosaicImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.MosaicImage.TabIndex = 1;
            this.MosaicImage.TabStop = false;
            // 
            // ControlPanel
            // 
            this.ControlPanel.Controls.Add(this.label2);
            this.ControlPanel.Controls.Add(this.maxPixels);
            this.ControlPanel.Controls.Add(this.sizeMode);
            this.ControlPanel.Controls.Add(this.showSourceImageCb);
            this.ControlPanel.Controls.Add(this.label1);
            this.ControlPanel.Controls.Add(this.amplify);
            this.ControlPanel.Controls.Add(this.MosaicAllBtn);
            this.ControlPanel.Controls.Add(this.blueCb);
            this.ControlPanel.Controls.Add(this.greenCb);
            this.ControlPanel.Controls.Add(this.redCb);
            this.ControlPanel.Controls.Add(this.greyOffset);
            this.ControlPanel.Controls.Add(this.linerBtn);
            this.ControlPanel.Controls.Add(this.greyCb);
            this.ControlPanel.Controls.Add(this.splitter1);
            this.ControlPanel.Controls.Add(this.greenOffset);
            this.ControlPanel.Controls.Add(this.redOffset);
            this.ControlPanel.Controls.Add(this.blueOffset);
            this.ControlPanel.Controls.Add(this.mosaicBtn);
            this.ControlPanel.Controls.Add(this.openBtn);
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ControlPanel.Location = new System.Drawing.Point(1636, 0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(126, 810);
            this.ControlPanel.TabIndex = 1;
            // 
            // showSourceImageCb
            // 
            this.showSourceImageCb.AutoSize = true;
            this.showSourceImageCb.Location = new System.Drawing.Point(22, 758);
            this.showSourceImageCb.Name = "showSourceImageCb";
            this.showSourceImageCb.Size = new System.Drawing.Size(85, 17);
            this.showSourceImageCb.TabIndex = 19;
            this.showSourceImageCb.Text = "Show Image";
            this.showSourceImageCb.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Amplify";
            // 
            // amplify
            // 
            this.amplify.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.amplify.Location = new System.Drawing.Point(72, 143);
            this.amplify.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.amplify.Name = "amplify";
            this.amplify.Size = new System.Drawing.Size(37, 20);
            this.amplify.TabIndex = 17;
            this.amplify.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // MosaicAllBtn
            // 
            this.MosaicAllBtn.Location = new System.Drawing.Point(22, 326);
            this.MosaicAllBtn.Name = "MosaicAllBtn";
            this.MosaicAllBtn.Size = new System.Drawing.Size(75, 23);
            this.MosaicAllBtn.TabIndex = 16;
            this.MosaicAllBtn.Text = "MosaicAll";
            this.MosaicAllBtn.UseVisualStyleBackColor = true;
            this.MosaicAllBtn.Click += new System.EventHandler(this.MosaicAllBtn_Click);
            // 
            // blueCb
            // 
            this.blueCb.AutoSize = true;
            this.blueCb.Checked = true;
            this.blueCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.blueCb.Location = new System.Drawing.Point(17, 119);
            this.blueCb.Name = "blueCb";
            this.blueCb.Size = new System.Drawing.Size(47, 17);
            this.blueCb.TabIndex = 14;
            this.blueCb.Text = "Blue";
            this.blueCb.UseVisualStyleBackColor = true;
            // 
            // greenCb
            // 
            this.greenCb.AutoSize = true;
            this.greenCb.Checked = true;
            this.greenCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.greenCb.Location = new System.Drawing.Point(18, 93);
            this.greenCb.Name = "greenCb";
            this.greenCb.Size = new System.Drawing.Size(55, 17);
            this.greenCb.TabIndex = 13;
            this.greenCb.Text = "Green";
            this.greenCb.UseVisualStyleBackColor = true;
            // 
            // redCb
            // 
            this.redCb.AutoSize = true;
            this.redCb.Checked = true;
            this.redCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.redCb.Location = new System.Drawing.Point(18, 70);
            this.redCb.Name = "redCb";
            this.redCb.Size = new System.Drawing.Size(46, 17);
            this.redCb.TabIndex = 12;
            this.redCb.Text = "Red";
            this.redCb.UseVisualStyleBackColor = true;
            // 
            // greyOffset
            // 
            this.greyOffset.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.greyOffset.Location = new System.Drawing.Point(73, 41);
            this.greyOffset.Name = "greyOffset";
            this.greyOffset.Size = new System.Drawing.Size(37, 20);
            this.greyOffset.TabIndex = 11;
            this.greyOffset.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // linerBtn
            // 
            this.linerBtn.Location = new System.Drawing.Point(22, 268);
            this.linerBtn.Name = "linerBtn";
            this.linerBtn.Size = new System.Drawing.Size(75, 23);
            this.linerBtn.TabIndex = 10;
            this.linerBtn.Text = "Liner";
            this.linerBtn.UseVisualStyleBackColor = true;
            this.linerBtn.Click += new System.EventHandler(this.LinerBtn_Click);
            // 
            // greyCb
            // 
            this.greyCb.AutoSize = true;
            this.greyCb.Location = new System.Drawing.Point(19, 44);
            this.greyCb.Name = "greyCb";
            this.greyCb.Size = new System.Drawing.Size(48, 17);
            this.greyCb.TabIndex = 9;
            this.greyCb.Text = "Grey";
            this.greyCb.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 810);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // greenOffset
            // 
            this.greenOffset.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.greenOffset.Location = new System.Drawing.Point(72, 92);
            this.greenOffset.Name = "greenOffset";
            this.greenOffset.Size = new System.Drawing.Size(37, 20);
            this.greenOffset.TabIndex = 4;
            this.greenOffset.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // redOffset
            // 
            this.redOffset.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.redOffset.Location = new System.Drawing.Point(72, 67);
            this.redOffset.Name = "redOffset";
            this.redOffset.Size = new System.Drawing.Size(37, 20);
            this.redOffset.TabIndex = 3;
            this.redOffset.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // blueOffset
            // 
            this.blueOffset.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.blueOffset.Location = new System.Drawing.Point(72, 117);
            this.blueOffset.Name = "blueOffset";
            this.blueOffset.Size = new System.Drawing.Size(37, 20);
            this.blueOffset.TabIndex = 2;
            this.blueOffset.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // mosaicBtn
            // 
            this.mosaicBtn.Location = new System.Drawing.Point(22, 297);
            this.mosaicBtn.Name = "mosaicBtn";
            this.mosaicBtn.Size = new System.Drawing.Size(75, 23);
            this.mosaicBtn.TabIndex = 1;
            this.mosaicBtn.Text = "Mosaic";
            this.mosaicBtn.UseVisualStyleBackColor = true;
            this.mosaicBtn.Click += new System.EventHandler(this.MosaicBtn_Click);
            // 
            // openBtn
            // 
            this.openBtn.Location = new System.Drawing.Point(22, 10);
            this.openBtn.Name = "openBtn";
            this.openBtn.Size = new System.Drawing.Size(75, 23);
            this.openBtn.TabIndex = 0;
            this.openBtn.Text = "Open";
            this.openBtn.UseVisualStyleBackColor = true;
            this.openBtn.Click += new System.EventHandler(this.OpenBtn_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.pn" +
    "g";
            // 
            // PicturePanel
            // 
            this.PicturePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PicturePanel.Location = new System.Drawing.Point(0, 0);
            this.PicturePanel.Name = "PicturePanel";
            // 
            // PicturePanel.Panel1
            // 
            this.PicturePanel.Panel1.AutoScroll = true;
            this.PicturePanel.Panel1.Controls.Add(this.sourcePanel);
            // 
            // PicturePanel.Panel2
            // 
            this.PicturePanel.Panel2.AutoScroll = true;
            this.PicturePanel.Panel2.Controls.Add(this.mosaicPanel);
            this.PicturePanel.Size = new System.Drawing.Size(1636, 810);
            this.PicturePanel.SplitterDistance = 824;
            this.PicturePanel.TabIndex = 2;
            // 
            // sourcePanel
            // 
            this.sourcePanel.AutoScroll = true;
            this.sourcePanel.AutoSize = true;
            this.sourcePanel.Controls.Add(this.sourceImage);
            this.sourcePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourcePanel.Location = new System.Drawing.Point(0, 0);
            this.sourcePanel.Name = "sourcePanel";
            this.sourcePanel.Size = new System.Drawing.Size(824, 810);
            this.sourcePanel.TabIndex = 0;
            // 
            // sourceImage
            // 
            this.sourceImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceImage.Location = new System.Drawing.Point(0, 0);
            this.sourceImage.Name = "sourceImage";
            this.sourceImage.Size = new System.Drawing.Size(824, 810);
            this.sourceImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.sourceImage.TabIndex = 0;
            this.sourceImage.TabStop = false;
            this.sourceImage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SourceImage_MouseDoubleClick);
            // 
            // mosaicPanel
            // 
            this.mosaicPanel.AutoScroll = true;
            this.mosaicPanel.AutoSize = true;
            this.mosaicPanel.Controls.Add(this.MosaicImage);
            this.mosaicPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mosaicPanel.Location = new System.Drawing.Point(0, 0);
            this.mosaicPanel.Name = "mosaicPanel";
            this.mosaicPanel.Size = new System.Drawing.Size(808, 810);
            this.mosaicPanel.TabIndex = 0;
            // 
            // sizeMode
            // 
            this.sizeMode.FormattingEnabled = true;
            this.sizeMode.Location = new System.Drawing.Point(17, 540);
            this.sizeMode.Name = "sizeMode";
            this.sizeMode.Size = new System.Drawing.Size(93, 21);
            this.sizeMode.TabIndex = 23;
            this.sizeMode.SelectedIndexChanged += new System.EventHandler(this.SizeMode_SelectedIndexChanged);
            // 
            // maxPixels
            // 
            this.maxPixels.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.maxPixels.Location = new System.Drawing.Point(56, 169);
            this.maxPixels.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.maxPixels.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.maxPixels.Name = "maxPixels";
            this.maxPixels.Size = new System.Drawing.Size(51, 20);
            this.maxPixels.TabIndex = 24;
            this.maxPixels.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Max";
            // 
            // MosaicWorkshops
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1762, 983);
            this.Controls.Add(this.PicturePanel);
            this.Controls.Add(this.ControlPanel);
            this.Controls.Add(this.StatusPanel);
            this.Name = "MosaicWorkshops";
            this.Text = "Mosaic Workshops";
            this.StatusPanel.ResumeLayout(false);
            this.StatusPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greyOptimizeOffsetMosaic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greyBigOffsetMosaic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greySmallOffsetMosaic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optimizeOffsetMosaic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bigOffsetMasaic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.smallOffsetMosaic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MosaicImage)).EndInit();
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amplify)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greyOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueOffset)).EndInit();
            this.PicturePanel.Panel1.ResumeLayout(false);
            this.PicturePanel.Panel1.PerformLayout();
            this.PicturePanel.Panel2.ResumeLayout(false);
            this.PicturePanel.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicturePanel)).EndInit();
            this.PicturePanel.ResumeLayout(false);
            this.sourcePanel.ResumeLayout(false);
            this.sourcePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceImage)).EndInit();
            this.mosaicPanel.ResumeLayout(false);
            this.mosaicPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxPixels)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel StatusPanel;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.Button openBtn;
        private System.Windows.Forms.PictureBox MosaicImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button mosaicBtn;
        private System.Windows.Forms.TextBox MessageTb;
        private System.Windows.Forms.NumericUpDown blueOffset;
        private System.Windows.Forms.NumericUpDown greenOffset;
        private System.Windows.Forms.NumericUpDown redOffset;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.CheckBox greyCb;
        private System.Windows.Forms.Button linerBtn;
        private System.Windows.Forms.CheckBox blueCb;
        private System.Windows.Forms.CheckBox greenCb;
        private System.Windows.Forms.CheckBox redCb;
        private System.Windows.Forms.NumericUpDown greyOffset;
        private System.Windows.Forms.SplitContainer PicturePanel;
        private System.Windows.Forms.Panel sourcePanel;
        private System.Windows.Forms.Panel mosaicPanel;
        private System.Windows.Forms.PictureBox sourceImage;
        private System.Windows.Forms.PictureBox greyOptimizeOffsetMosaic;
        private System.Windows.Forms.PictureBox greyBigOffsetMosaic;
        private System.Windows.Forms.PictureBox greySmallOffsetMosaic;
        private System.Windows.Forms.PictureBox optimizeOffsetMosaic;
        private System.Windows.Forms.PictureBox bigOffsetMasaic;
        private System.Windows.Forms.PictureBox smallOffsetMosaic;
        private System.Windows.Forms.Button MosaicAllBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown amplify;
        private System.Windows.Forms.CheckBox showSourceImageCb;
        private System.Windows.Forms.ComboBox sizeMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown maxPixels;
    }
}

