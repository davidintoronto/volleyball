namespace Wechat_Notifier
{
    partial class Notifier
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
            this.components = new System.ComponentModel.Container();
            this.StartBtn = new System.Windows.Forms.Button();
            this.MessageLb = new System.Windows.Forms.Label();
            this.HourSharpTimer = new System.Windows.Forms.Timer(this.components);
            this.WechatTimer = new System.Windows.Forms.Timer(this.components);
            this.HourSharpBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.LogTb = new System.Windows.Forms.TextBox();
            this.WechatStatusLb = new System.Windows.Forms.Label();
            this.MessageNumberLb = new System.Windows.Forms.Label();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(8, 6);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(75, 23);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // MessageLb
            // 
            this.MessageLb.AutoSize = true;
            this.MessageLb.Location = new System.Drawing.Point(89, 11);
            this.MessageLb.Name = "MessageLb";
            this.MessageLb.Size = new System.Drawing.Size(0, 13);
            this.MessageLb.TabIndex = 2;
            // 
            // HourSharpTimer
            // 
            this.HourSharpTimer.Interval = 3600000;
            this.HourSharpTimer.Tick += new System.EventHandler(this.ScheduleTaskTimer_Tick);
            // 
            // WechatTimer
            // 
            this.WechatTimer.Interval = 60000;
            this.WechatTimer.Tick += new System.EventHandler(this.WechatTimer_Tick);
            // 
            // HourSharpBtn
            // 
            this.HourSharpBtn.Location = new System.Drawing.Point(749, 6);
            this.HourSharpBtn.Name = "HourSharpBtn";
            this.HourSharpBtn.Size = new System.Drawing.Size(75, 23);
            this.HourSharpBtn.TabIndex = 3;
            this.HourSharpBtn.Text = "HourSharp";
            this.HourSharpBtn.UseVisualStyleBackColor = true;
            this.HourSharpBtn.Click += new System.EventHandler(this.HourSharpBtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(643, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Location";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.ShowWindowStartLocation_Click);
            // 
            // LogTb
            // 
            this.LogTb.Location = new System.Drawing.Point(8, 104);
            this.LogTb.Multiline = true;
            this.LogTb.Name = "LogTb";
            this.LogTb.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogTb.Size = new System.Drawing.Size(812, 327);
            this.LogTb.TabIndex = 5;
            // 
            // WechatStatusLb
            // 
            this.WechatStatusLb.AutoSize = true;
            this.WechatStatusLb.Location = new System.Drawing.Point(12, 43);
            this.WechatStatusLb.Name = "WechatStatusLb";
            this.WechatStatusLb.Size = new System.Drawing.Size(45, 13);
            this.WechatStatusLb.TabIndex = 6;
            this.WechatStatusLb.Text = "Wechat";
            // 
            // MessageNumberLb
            // 
            this.MessageNumberLb.AutoSize = true;
            this.MessageNumberLb.Location = new System.Drawing.Point(12, 71);
            this.MessageNumberLb.Name = "MessageNumberLb";
            this.MessageNumberLb.Size = new System.Drawing.Size(88, 13);
            this.MessageNumberLb.TabIndex = 7;
            this.MessageNumberLb.Text = "Message number";
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(745, 61);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearBtn.TabIndex = 8;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // Notifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 443);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.MessageNumberLb);
            this.Controls.Add(this.WechatStatusLb);
            this.Controls.Add(this.LogTb);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.HourSharpBtn);
            this.Controls.Add(this.MessageLb);
            this.Controls.Add(this.StartBtn);
            this.Name = "Notifier";
            this.Text = "Wechat Notifier";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Label MessageLb;
        private System.Windows.Forms.Timer HourSharpTimer;
        private System.Windows.Forms.Timer WechatTimer;
        private System.Windows.Forms.Button HourSharpBtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox LogTb;
        private System.Windows.Forms.Label WechatStatusLb;
        private System.Windows.Forms.Label MessageNumberLb;
        private System.Windows.Forms.Button ClearBtn;
    }
}

