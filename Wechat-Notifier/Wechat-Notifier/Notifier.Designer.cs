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
            this.HomePcIpTimer = new System.Windows.Forms.Timer(this.components);
            this.WechatTimer = new System.Windows.Forms.Timer(this.components);
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
            // HomePcIpTimer
            // 
            this.HomePcIpTimer.Interval = 3600000;
            this.HomePcIpTimer.Tick += new System.EventHandler(this.HomePcIpTimer_Tick);
            // 
            // WechatTimer
            // 
            this.WechatTimer.Interval = 60000;
            this.WechatTimer.Tick += new System.EventHandler(this.WechatTimer_Tick);
            // 
            // Notifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 32);
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
        private System.Windows.Forms.Timer HomePcIpTimer;
        private System.Windows.Forms.Timer WechatTimer;
    }
}

