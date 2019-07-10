namespace JobResumeSender
{
    partial class JP
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
            this.label1 = new System.Windows.Forms.Label();
            this.ListUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.IndexRunBtn = new System.Windows.Forms.Button();
            this.LogTb = new System.Windows.Forms.TextBox();
            this.ConsolePanel = new System.Windows.Forms.Panel();
            this.MergeBtn = new System.Windows.Forms.Button();
            this.CAComBtn = new System.Windows.Forms.Button();
            this.ViewListBtn = new System.Windows.Forms.Button();
            this.RunEmailBtn = new System.Windows.Forms.Button();
            this.RunForUrlBtn = new System.Windows.Forms.Button();
            this.CategoryRunBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.resetSearchEmailBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.ConsolePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Index Url";
            // 
            // ListUrl
            // 
            this.ListUrl.Location = new System.Drawing.Point(66, 8);
            this.ListUrl.Name = "ListUrl";
            this.ListUrl.Size = new System.Drawing.Size(577, 20);
            this.ListUrl.TabIndex = 1;
            this.ListUrl.Text = "https://www.toronto.com/toronto-directory/importers/?q=&pageindex=";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(524, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Depth";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(566, 46);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(36, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // IndexRunBtn
            // 
            this.IndexRunBtn.Location = new System.Drawing.Point(661, 6);
            this.IndexRunBtn.Name = "IndexRunBtn";
            this.IndexRunBtn.Size = new System.Drawing.Size(101, 23);
            this.IndexRunBtn.TabIndex = 4;
            this.IndexRunBtn.Text = "Show Searched";
            this.IndexRunBtn.UseVisualStyleBackColor = true;
            this.IndexRunBtn.Click += new System.EventHandler(this.RunBtn_Click);
            // 
            // LogTb
            // 
            this.LogTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogTb.Location = new System.Drawing.Point(0, 0);
            this.LogTb.Multiline = true;
            this.LogTb.Name = "LogTb";
            this.LogTb.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogTb.Size = new System.Drawing.Size(951, 497);
            this.LogTb.TabIndex = 5;
            // 
            // ConsolePanel
            // 
            this.ConsolePanel.Controls.Add(this.resetSearchEmailBtn);
            this.ConsolePanel.Controls.Add(this.MergeBtn);
            this.ConsolePanel.Controls.Add(this.CAComBtn);
            this.ConsolePanel.Controls.Add(this.ViewListBtn);
            this.ConsolePanel.Controls.Add(this.RunEmailBtn);
            this.ConsolePanel.Controls.Add(this.RunForUrlBtn);
            this.ConsolePanel.Controls.Add(this.CategoryRunBtn);
            this.ConsolePanel.Controls.Add(this.label1);
            this.ConsolePanel.Controls.Add(this.ListUrl);
            this.ConsolePanel.Controls.Add(this.IndexRunBtn);
            this.ConsolePanel.Controls.Add(this.label2);
            this.ConsolePanel.Controls.Add(this.numericUpDown1);
            this.ConsolePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ConsolePanel.Location = new System.Drawing.Point(0, 0);
            this.ConsolePanel.Name = "ConsolePanel";
            this.ConsolePanel.Size = new System.Drawing.Size(951, 123);
            this.ConsolePanel.TabIndex = 6;
            // 
            // MergeBtn
            // 
            this.MergeBtn.Location = new System.Drawing.Point(752, 80);
            this.MergeBtn.Name = "MergeBtn";
            this.MergeBtn.Size = new System.Drawing.Size(75, 23);
            this.MergeBtn.TabIndex = 11;
            this.MergeBtn.Text = "Merge";
            this.MergeBtn.UseVisualStyleBackColor = true;
            this.MergeBtn.Click += new System.EventHandler(this.MergeBtn_Click);
            // 
            // CAComBtn
            // 
            this.CAComBtn.Location = new System.Drawing.Point(844, 41);
            this.CAComBtn.Name = "CAComBtn";
            this.CAComBtn.Size = new System.Drawing.Size(82, 23);
            this.CAComBtn.TabIndex = 10;
            this.CAComBtn.Text = "Varify Email";
            this.CAComBtn.UseVisualStyleBackColor = true;
            this.CAComBtn.Click += new System.EventHandler(this.CaComListBtn_Click);
            // 
            // ViewListBtn
            // 
            this.ViewListBtn.Location = new System.Drawing.Point(661, 80);
            this.ViewListBtn.Name = "ViewListBtn";
            this.ViewListBtn.Size = new System.Drawing.Size(75, 23);
            this.ViewListBtn.TabIndex = 9;
            this.ViewListBtn.Text = "View";
            this.ViewListBtn.UseVisualStyleBackColor = true;
            this.ViewListBtn.Click += new System.EventHandler(this.ViewListBtn_Click);
            // 
            // RunEmailBtn
            // 
            this.RunEmailBtn.Location = new System.Drawing.Point(752, 43);
            this.RunEmailBtn.Name = "RunEmailBtn";
            this.RunEmailBtn.Size = new System.Drawing.Size(82, 23);
            this.RunEmailBtn.TabIndex = 8;
            this.RunEmailBtn.Text = "Run for Email";
            this.RunEmailBtn.UseVisualStyleBackColor = true;
            this.RunEmailBtn.Click += new System.EventHandler(this.RunEmailBtn_Click);
            // 
            // RunForUrlBtn
            // 
            this.RunForUrlBtn.Location = new System.Drawing.Point(775, 9);
            this.RunForUrlBtn.Name = "RunForUrlBtn";
            this.RunForUrlBtn.Size = new System.Drawing.Size(75, 23);
            this.RunForUrlBtn.TabIndex = 7;
            this.RunForUrlBtn.Text = "Run for Url";
            this.RunForUrlBtn.UseVisualStyleBackColor = true;
            this.RunForUrlBtn.Click += new System.EventHandler(this.RunUrlBtn_Click);
            // 
            // CategoryRunBtn
            // 
            this.CategoryRunBtn.Location = new System.Drawing.Point(864, 10);
            this.CategoryRunBtn.Name = "CategoryRunBtn";
            this.CategoryRunBtn.Size = new System.Drawing.Size(75, 23);
            this.CategoryRunBtn.TabIndex = 6;
            this.CategoryRunBtn.Text = "Verify Url";
            this.CategoryRunBtn.UseVisualStyleBackColor = true;
            this.CategoryRunBtn.Click += new System.EventHandler(this.LinkRunBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LogTb);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 123);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(951, 497);
            this.panel1.TabIndex = 7;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // resetSearchEmailBtn
            // 
            this.resetSearchEmailBtn.Location = new System.Drawing.Point(608, 43);
            this.resetSearchEmailBtn.Name = "resetSearchEmailBtn";
            this.resetSearchEmailBtn.Size = new System.Drawing.Size(128, 23);
            this.resetSearchEmailBtn.TabIndex = 12;
            this.resetSearchEmailBtn.Text = "Reset Search Email";
            this.resetSearchEmailBtn.UseVisualStyleBackColor = true;
            this.resetSearchEmailBtn.Click += new System.EventHandler(this.ResetSearchEmailBtn_Click);
            // 
            // JP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 620);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ConsolePanel);
            this.Name = "JP";
            this.Text = "Console";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ConsolePanel.ResumeLayout(false);
            this.ConsolePanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ListUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button IndexRunBtn;
        private System.Windows.Forms.TextBox LogTb;
        private System.Windows.Forms.Panel ConsolePanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CategoryRunBtn;
        private System.Windows.Forms.Button RunForUrlBtn;
        private System.Windows.Forms.Button RunEmailBtn;
        private System.Windows.Forms.Button ViewListBtn;
        private System.Windows.Forms.Button CAComBtn;
        private System.Windows.Forms.Button MergeBtn;
        private System.Windows.Forms.Button resetSearchEmailBtn;
    }
}

