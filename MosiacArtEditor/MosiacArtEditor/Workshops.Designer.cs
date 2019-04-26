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
            this.MessageTb = new System.Windows.Forms.TextBox();
            this.MosaicImage = new System.Windows.Forms.PictureBox();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.sizeCb = new System.Windows.Forms.CheckBox();
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
            this.mosaicPanel = new System.Windows.Forms.Panel();
            this.sourceImage = new System.Windows.Forms.PictureBox();
            this.StatusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MosaicImage)).BeginInit();
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greyOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicturePanel)).BeginInit();
            this.PicturePanel.Panel1.SuspendLayout();
            this.PicturePanel.Panel2.SuspendLayout();
            this.PicturePanel.SuspendLayout();
            this.sourcePanel.SuspendLayout();
            this.mosaicPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceImage)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusPanel
            // 
            this.StatusPanel.Controls.Add(this.MessageTb);
            this.StatusPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StatusPanel.Location = new System.Drawing.Point(0, 683);
            this.StatusPanel.Name = "StatusPanel";
            this.StatusPanel.Size = new System.Drawing.Size(1470, 100);
            this.StatusPanel.TabIndex = 1;
            // 
            // MessageTb
            // 
            this.MessageTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageTb.Location = new System.Drawing.Point(0, 0);
            this.MessageTb.Multiline = true;
            this.MessageTb.Name = "MessageTb";
            this.MessageTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MessageTb.Size = new System.Drawing.Size(1470, 100);
            this.MessageTb.TabIndex = 0;
            // 
            // MosaicImage
            // 
            this.MosaicImage.Location = new System.Drawing.Point(0, 0);
            this.MosaicImage.Name = "MosaicImage";
            this.MosaicImage.Size = new System.Drawing.Size(663, 683);
            this.MosaicImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.MosaicImage.TabIndex = 1;
            this.MosaicImage.TabStop = false;
            this.MosaicImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AfterImage_MouseClick);
            // 
            // ControlPanel
            // 
            this.ControlPanel.Controls.Add(this.sizeCb);
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
            this.ControlPanel.Location = new System.Drawing.Point(1344, 0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(126, 683);
            this.ControlPanel.TabIndex = 1;
            // 
            // sizeCb
            // 
            this.sizeCb.AutoSize = true;
            this.sizeCb.Checked = true;
            this.sizeCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sizeCb.Location = new System.Drawing.Point(18, 647);
            this.sizeCb.Name = "sizeCb";
            this.sizeCb.Size = new System.Drawing.Size(71, 17);
            this.sizeCb.TabIndex = 15;
            this.sizeCb.Text = "Auto Size";
            this.sizeCb.UseVisualStyleBackColor = true;
            this.sizeCb.CheckedChanged += new System.EventHandler(this.SizeCb_CheckedChanged);
            // 
            // blueCb
            // 
            this.blueCb.AutoSize = true;
            this.blueCb.Checked = true;
            this.blueCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.blueCb.Location = new System.Drawing.Point(16, 224);
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
            this.greenCb.Location = new System.Drawing.Point(17, 199);
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
            this.redCb.Location = new System.Drawing.Point(17, 176);
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
            this.greyOffset.Location = new System.Drawing.Point(72, 147);
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
            this.linerBtn.Location = new System.Drawing.Point(22, 260);
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
            this.greyCb.Location = new System.Drawing.Point(18, 150);
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
            this.splitter1.Size = new System.Drawing.Size(3, 683);
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
            this.greenOffset.Location = new System.Drawing.Point(71, 198);
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
            this.redOffset.Location = new System.Drawing.Point(71, 173);
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
            this.blueOffset.Location = new System.Drawing.Point(71, 223);
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
            this.mosaicBtn.Location = new System.Drawing.Point(22, 39);
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
            this.PicturePanel.Size = new System.Drawing.Size(1344, 683);
            this.PicturePanel.SplitterDistance = 677;
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
            this.sourcePanel.Size = new System.Drawing.Size(677, 683);
            this.sourcePanel.TabIndex = 0;
            // 
            // mosaicPanel
            // 
            this.mosaicPanel.AutoScroll = true;
            this.mosaicPanel.AutoSize = true;
            this.mosaicPanel.Controls.Add(this.MosaicImage);
            this.mosaicPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mosaicPanel.Location = new System.Drawing.Point(0, 0);
            this.mosaicPanel.Name = "mosaicPanel";
            this.mosaicPanel.Size = new System.Drawing.Size(663, 683);
            this.mosaicPanel.TabIndex = 0;
            // 
            // sourceImage
            // 
            this.sourceImage.Location = new System.Drawing.Point(0, 3);
            this.sourceImage.Name = "sourceImage";
            this.sourceImage.Size = new System.Drawing.Size(675, 680);
            this.sourceImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.sourceImage.TabIndex = 0;
            this.sourceImage.TabStop = false;
            // 
            // MosaicWorkshops
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1470, 783);
            this.Controls.Add(this.PicturePanel);
            this.Controls.Add(this.ControlPanel);
            this.Controls.Add(this.StatusPanel);
            this.Name = "MosaicWorkshops";
            this.Text = "Mosaic Workshops";
            this.StatusPanel.ResumeLayout(false);
            this.StatusPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MosaicImage)).EndInit();
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
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
            this.mosaicPanel.ResumeLayout(false);
            this.mosaicPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceImage)).EndInit();
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
        private System.Windows.Forms.CheckBox sizeCb;
        private System.Windows.Forms.SplitContainer PicturePanel;
        private System.Windows.Forms.Panel sourcePanel;
        private System.Windows.Forms.Panel mosaicPanel;
        private System.Windows.Forms.PictureBox sourceImage;
    }
}

