namespace JobResumeSender
{
    partial class Email
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Email));
            this.label1 = new System.Windows.Forms.Label();
            this.SenderTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SubjectTb = new System.Windows.Forms.TextBox();
            this.BodyTb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AttachedFile = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ConvertEmailListBtn = new System.Windows.Forms.Button();
            this.TestBtn = new System.Windows.Forms.Button();
            this.TestEmailTb = new System.Windows.Forms.TextBox();
            this.FilterBtn = new System.Windows.Forms.Button();
            this.EmailRegexlb = new System.Windows.Forms.Label();
            this.EmailRegexTb = new System.Windows.Forms.TextBox();
            this.ShowAllCb = new System.Windows.Forms.CheckBox();
            this.PasswordTb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SendBtn = new System.Windows.Forms.Button();
            this.FileBrowse = new System.Windows.Forms.Button();
            this.EmailListClb = new System.Windows.Forms.CheckedListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LogTb = new System.Windows.Forms.TextBox();
            this.replyToTb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sender";
            // 
            // SenderTb
            // 
            this.SenderTb.Location = new System.Drawing.Point(61, 13);
            this.SenderTb.Name = "SenderTb";
            this.SenderTb.Size = new System.Drawing.Size(129, 20);
            this.SenderTb.TabIndex = 1;
            this.SenderTb.Text = "kevnuyen@gmail.com";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Subject";
            // 
            // SubjectTb
            // 
            this.SubjectTb.Location = new System.Drawing.Point(63, 43);
            this.SubjectTb.Name = "SubjectTb";
            this.SubjectTb.Size = new System.Drawing.Size(514, 20);
            this.SubjectTb.TabIndex = 3;
            this.SubjectTb.Text = "Looking for entry level office job !";
            // 
            // BodyTb
            // 
            this.BodyTb.Location = new System.Drawing.Point(61, 75);
            this.BodyTb.Multiline = true;
            this.BodyTb.Name = "BodyTb";
            this.BodyTb.Size = new System.Drawing.Size(898, 87);
            this.BodyTb.TabIndex = 4;
            this.BodyTb.Text = resources.GetString("BodyTb.Text");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "File Attached";
            // 
            // AttachedFile
            // 
            this.AttachedFile.Location = new System.Drawing.Point(87, 166);
            this.AttachedFile.Name = "AttachedFile";
            this.AttachedFile.Size = new System.Drawing.Size(659, 20);
            this.AttachedFile.TabIndex = 6;
            this.AttachedFile.Text = "D:\\Users\\david\\Documents\\mb\\Kevin Nguyen\'s Resume-May 8.docx";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.replyToTb);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.ConvertEmailListBtn);
            this.panel1.Controls.Add(this.TestBtn);
            this.panel1.Controls.Add(this.TestEmailTb);
            this.panel1.Controls.Add(this.FilterBtn);
            this.panel1.Controls.Add(this.EmailRegexlb);
            this.panel1.Controls.Add(this.EmailRegexTb);
            this.panel1.Controls.Add(this.ShowAllCb);
            this.panel1.Controls.Add(this.PasswordTb);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.SendBtn);
            this.panel1.Controls.Add(this.FileBrowse);
            this.panel1.Controls.Add(this.AttachedFile);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.BodyTb);
            this.panel1.Controls.Add(this.SubjectTb);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.SenderTb);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1456, 233);
            this.panel1.TabIndex = 0;
            // 
            // ConvertEmailListBtn
            // 
            this.ConvertEmailListBtn.Location = new System.Drawing.Point(884, 165);
            this.ConvertEmailListBtn.Name = "ConvertEmailListBtn";
            this.ConvertEmailListBtn.Size = new System.Drawing.Size(75, 23);
            this.ConvertEmailListBtn.TabIndex = 17;
            this.ConvertEmailListBtn.Text = "Convert";
            this.ConvertEmailListBtn.UseVisualStyleBackColor = true;
            this.ConvertEmailListBtn.Click += new System.EventHandler(this.ConvertEmailListBtn_Click);
            // 
            // TestBtn
            // 
            this.TestBtn.Location = new System.Drawing.Point(752, 195);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(75, 23);
            this.TestBtn.TabIndex = 16;
            this.TestBtn.Text = "Test";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this.TestBtn_Click);
            // 
            // TestEmailTb
            // 
            this.TestEmailTb.Location = new System.Drawing.Point(426, 198);
            this.TestEmailTb.Name = "TestEmailTb";
            this.TestEmailTb.Size = new System.Drawing.Size(320, 20);
            this.TestEmailTb.TabIndex = 15;
            this.TestEmailTb.Text = "david_zzh@hotmail.com";
            // 
            // FilterBtn
            // 
            this.FilterBtn.Location = new System.Drawing.Point(320, 196);
            this.FilterBtn.Name = "FilterBtn";
            this.FilterBtn.Size = new System.Drawing.Size(75, 23);
            this.FilterBtn.TabIndex = 14;
            this.FilterBtn.Text = "Filter";
            this.FilterBtn.UseVisualStyleBackColor = true;
            // 
            // EmailRegexlb
            // 
            this.EmailRegexlb.AutoSize = true;
            this.EmailRegexlb.Location = new System.Drawing.Point(102, 202);
            this.EmailRegexlb.Name = "EmailRegexlb";
            this.EmailRegexlb.Size = new System.Drawing.Size(66, 13);
            this.EmailRegexlb.TabIndex = 13;
            this.EmailRegexlb.Text = "Email Regex";
            // 
            // EmailRegexTb
            // 
            this.EmailRegexTb.Location = new System.Drawing.Point(177, 199);
            this.EmailRegexTb.Name = "EmailRegexTb";
            this.EmailRegexTb.PasswordChar = '*';
            this.EmailRegexTb.Size = new System.Drawing.Size(129, 20);
            this.EmailRegexTb.TabIndex = 12;
            // 
            // ShowAllCb
            // 
            this.ShowAllCb.AutoSize = true;
            this.ShowAllCb.Location = new System.Drawing.Point(29, 202);
            this.ShowAllCb.Name = "ShowAllCb";
            this.ShowAllCb.Size = new System.Drawing.Size(67, 17);
            this.ShowAllCb.TabIndex = 11;
            this.ShowAllCb.Text = "Show All";
            this.ShowAllCb.UseVisualStyleBackColor = true;
            // 
            // PasswordTb
            // 
            this.PasswordTb.Location = new System.Drawing.Point(258, 13);
            this.PasswordTb.Name = "PasswordTb";
            this.PasswordTb.PasswordChar = '*';
            this.PasswordTb.Size = new System.Drawing.Size(129, 20);
            this.PasswordTb.TabIndex = 10;
            this.PasswordTb.Text = "kevin20190508";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(199, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password";
            // 
            // SendBtn
            // 
            this.SendBtn.Location = new System.Drawing.Point(884, 196);
            this.SendBtn.Name = "SendBtn";
            this.SendBtn.Size = new System.Drawing.Size(75, 23);
            this.SendBtn.TabIndex = 8;
            this.SendBtn.Text = "Send";
            this.SendBtn.UseVisualStyleBackColor = true;
            this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
            // 
            // FileBrowse
            // 
            this.FileBrowse.Location = new System.Drawing.Point(752, 166);
            this.FileBrowse.Name = "FileBrowse";
            this.FileBrowse.Size = new System.Drawing.Size(75, 23);
            this.FileBrowse.TabIndex = 7;
            this.FileBrowse.Text = "Browse";
            this.FileBrowse.UseVisualStyleBackColor = true;
            this.FileBrowse.Click += new System.EventHandler(this.FileBrowse_Click);
            // 
            // EmailListClb
            // 
            this.EmailListClb.ColumnWidth = 500;
            this.EmailListClb.Dock = System.Windows.Forms.DockStyle.Left;
            this.EmailListClb.FormattingEnabled = true;
            this.EmailListClb.Location = new System.Drawing.Point(0, 0);
            this.EmailListClb.Name = "EmailListClb";
            this.EmailListClb.ScrollAlwaysVisible = true;
            this.EmailListClb.Size = new System.Drawing.Size(1216, 594);
            this.EmailListClb.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.LogTb);
            this.panel2.Controls.Add(this.EmailListClb);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 233);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1456, 594);
            this.panel2.TabIndex = 2;
            // 
            // LogTb
            // 
            this.LogTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogTb.Location = new System.Drawing.Point(1216, 0);
            this.LogTb.Multiline = true;
            this.LogTb.Name = "LogTb";
            this.LogTb.Size = new System.Drawing.Size(240, 594);
            this.LogTb.TabIndex = 15;
            // 
            // replyToTb
            // 
            this.replyToTb.Location = new System.Drawing.Point(448, 16);
            this.replyToTb.Name = "replyToTb";
            this.replyToTb.Size = new System.Drawing.Size(129, 20);
            this.replyToTb.TabIndex = 19;
            this.replyToTb.Text = "tn131191@gmail.com";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(401, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Reply to";
            // 
            // Email
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1456, 827);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Email";
            this.Text = "Email";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SenderTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SubjectTb;
        private System.Windows.Forms.TextBox BodyTb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AttachedFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button SendBtn;
        private System.Windows.Forms.Button FileBrowse;
        private System.Windows.Forms.TextBox PasswordTb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button FilterBtn;
        private System.Windows.Forms.Label EmailRegexlb;
        private System.Windows.Forms.TextBox EmailRegexTb;
        private System.Windows.Forms.CheckBox ShowAllCb;
        private System.Windows.Forms.CheckedListBox EmailListClb;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox LogTb;
        private System.Windows.Forms.Button TestBtn;
        private System.Windows.Forms.TextBox TestEmailTb;
        private System.Windows.Forms.Button ConvertEmailListBtn;
        private System.Windows.Forms.TextBox replyToTb;
        private System.Windows.Forms.Label label5;
    }
}