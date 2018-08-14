namespace WechatHolder
{
    partial class Form1
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
            this.BtnPanel = new System.Windows.Forms.Panel();
            this.WechatPanel = new System.Windows.Forms.Panel();
            this.Dockbtn = new System.Windows.Forms.Button();
            this.UndockBtn = new System.Windows.Forms.Button();
            this.BtnPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnPanel
            // 
            this.BtnPanel.Controls.Add(this.UndockBtn);
            this.BtnPanel.Controls.Add(this.Dockbtn);
            this.BtnPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnPanel.Location = new System.Drawing.Point(667, 0);
            this.BtnPanel.Name = "BtnPanel";
            this.BtnPanel.Size = new System.Drawing.Size(130, 458);
            this.BtnPanel.TabIndex = 0;
            // 
            // WechatPanel
            // 
            this.WechatPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WechatPanel.Location = new System.Drawing.Point(0, 0);
            this.WechatPanel.Name = "WechatPanel";
            this.WechatPanel.Size = new System.Drawing.Size(667, 458);
            this.WechatPanel.TabIndex = 1;
            // 
            // Dockbtn
            // 
            this.Dockbtn.Location = new System.Drawing.Point(34, 32);
            this.Dockbtn.Name = "Dockbtn";
            this.Dockbtn.Size = new System.Drawing.Size(75, 23);
            this.Dockbtn.TabIndex = 0;
            this.Dockbtn.Text = "Dock it";
            this.Dockbtn.UseVisualStyleBackColor = true;
            this.Dockbtn.Click += new System.EventHandler(this.Dockbtn_Click);
            // 
            // UndockBtn
            // 
            this.UndockBtn.Location = new System.Drawing.Point(34, 76);
            this.UndockBtn.Name = "UndockBtn";
            this.UndockBtn.Size = new System.Drawing.Size(75, 23);
            this.UndockBtn.TabIndex = 1;
            this.UndockBtn.Text = "Undock";
            this.UndockBtn.UseVisualStyleBackColor = true;
            this.UndockBtn.Click += new System.EventHandler(this.UndockBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 458);
            this.Controls.Add(this.WechatPanel);
            this.Controls.Add(this.BtnPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.BtnPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BtnPanel;
        private System.Windows.Forms.Button Dockbtn;
        private System.Windows.Forms.Panel WechatPanel;
        private System.Windows.Forms.Button UndockBtn;


    }
}

