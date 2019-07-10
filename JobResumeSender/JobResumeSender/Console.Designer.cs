namespace JobResumeSender
{
    partial class Console
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
            this.canBusiBtn = new System.Windows.Forms.Button();
            this.torontoCaBtn = new System.Windows.Forms.Button();
            this.emailSenderBtn = new System.Windows.Forms.Button();
            this.jpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // canBusiBtn
            // 
            this.canBusiBtn.Location = new System.Drawing.Point(12, 28);
            this.canBusiBtn.Name = "canBusiBtn";
            this.canBusiBtn.Size = new System.Drawing.Size(133, 23);
            this.canBusiBtn.TabIndex = 0;
            this.canBusiBtn.Text = "Canada Business";
            this.canBusiBtn.UseVisualStyleBackColor = true;
            this.canBusiBtn.Click += new System.EventHandler(this.CanBusiBtn_Click);
            // 
            // torontoCaBtn
            // 
            this.torontoCaBtn.Location = new System.Drawing.Point(12, 75);
            this.torontoCaBtn.Name = "torontoCaBtn";
            this.torontoCaBtn.Size = new System.Drawing.Size(133, 23);
            this.torontoCaBtn.TabIndex = 1;
            this.torontoCaBtn.Text = "Toronto.ca";
            this.torontoCaBtn.UseVisualStyleBackColor = true;
            this.torontoCaBtn.Click += new System.EventHandler(this.TorontoCaBtn_Click);
            // 
            // emailSenderBtn
            // 
            this.emailSenderBtn.Location = new System.Drawing.Point(18, 189);
            this.emailSenderBtn.Name = "emailSenderBtn";
            this.emailSenderBtn.Size = new System.Drawing.Size(133, 23);
            this.emailSenderBtn.TabIndex = 2;
            this.emailSenderBtn.Text = "Email Sender";
            this.emailSenderBtn.UseVisualStyleBackColor = true;
            this.emailSenderBtn.Click += new System.EventHandler(this.EmailSenderBtn_Click);
            // 
            // jpBtn
            // 
            this.jpBtn.Location = new System.Drawing.Point(12, 127);
            this.jpBtn.Name = "jpBtn";
            this.jpBtn.Size = new System.Drawing.Size(133, 23);
            this.jpBtn.TabIndex = 3;
            this.jpBtn.Text = "JP";
            this.jpBtn.UseVisualStyleBackColor = true;
            this.jpBtn.Click += new System.EventHandler(this.JpBtn_Click);
            // 
            // Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(168, 400);
            this.Controls.Add(this.jpBtn);
            this.Controls.Add(this.emailSenderBtn);
            this.Controls.Add(this.torontoCaBtn);
            this.Controls.Add(this.canBusiBtn);
            this.Name = "Console";
            this.Text = "Console";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button canBusiBtn;
        private System.Windows.Forms.Button torontoCaBtn;
        private System.Windows.Forms.Button emailSenderBtn;
        private System.Windows.Forms.Button jpBtn;
    }
}