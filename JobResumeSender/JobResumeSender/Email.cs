using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace JobResumeSender
{
    public partial class Email : Form
    {
        public Email()
        {
            InitializeComponent();
            ReloadEmailList();
        }

        private void ReloadEmailList()
        {
            ((ListBox)this.EmailListClb).DataSource = DataAcess.LoadCompanyList().CompanyList.FindAll(com=>com.Emails.Count>0 && !com.EmailSent);
            ((ListBox)this.EmailListClb).DisplayMember = "EmailsToString";
           // ((ListBox)this.EmailListClb).ValueMember = "Url";
          /*  foreach (Company company in this.EmailListClb.Items)
            {
                this.EmailListClb.SetItemChecked(this.EmailListClb.Items.IndexOf(company), company.EmailSent);
            }*/
        }

        private void FileBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                //openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    this.AttachedFile.Text = openFileDialog.FileName;

                  
                }
            }
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            String testEmail = "tn131191@gmail.com";
            Companies companies = DataAcess.LoadCompanyList();
            //SendEmail(testEmail);
            this.LogTb.Text = this.LogTb.Text + testEmail  + " sent\r\n";
            try
            {
                foreach (Company company in this.EmailListClb.Items)
                {
                    foreach (String email in company.Emails)
                    {
                        SendEmail(email);
                        this.LogTb.Text = this.LogTb.Text + email + " sent\r\n";
                    }
                    Company sentCompany = companies.CompanyList.Find(com => com.Url == company.Url);
                    if (sentCompany != null)
                    {
                        sentCompany.EmailSent = true;
                        DataAcess.Save(companies);
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogTb.Text = this.LogTb.Text + ex.Message + "\r\n";
            }
            MessageBox.Show("Email was sent!");
            ReloadEmailList();
        }

        private void SendEmail(String email)
        {
            if (this.SenderTb.Text == "" || this.SubjectTb.Text == "" || this.BodyTb.Text == "")
            {
                MessageBox.Show("Please specifiy sender email, subject, email body and attached file path");
                return;
            }
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress(this.SenderTb.Text);
            mail.To.Add(email);
            mail.Subject = this.SubjectTb.Text;
            mail.Body = this.BodyTb.Text;

            System.Net.Mail.Attachment attachment;
            if (this.AttachedFile.Text != ""){
            attachment = new System.Net.Mail.Attachment(this.AttachedFile.Text);
            mail.Attachments.Add(attachment);
            }

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(this.SenderTb.Text, "G160903e");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
    }
}
