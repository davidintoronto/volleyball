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
using System.Text.RegularExpressions;

namespace JobResumeSender
{
    public partial class Email : Form
    {
        private const String JOB_PATTERN = "job|career|hr|recruit";
        private const String INFO_PATTERN = "info";
        public Email()
        {
            InitializeComponent();
            ReloadEmailList();
        }

        private void ReloadEmailList()
        {
            ((ListBox)this.EmailListClb).DataSource = DataAcess.LoadCompanyList().CompanyList.FindAll(com=>com.Emails.Count>0 && !com.EmailSent);
            //((ListBox)this.EmailListClb).DataSource = DataAcess.LoadCompanyList().ComEmails.FindAll(e => !e.Sent);
            ((ListBox)this.EmailListClb).DisplayMember = "EmailsToString";
            // ((ListBox)this.EmailListClb).ValueMember = "Url";
            /*  foreach (Company company in this.EmailListClb.Items)
              {
                  this.EmailListClb.SetItemChecked(this.EmailListClb.Items.IndexOf(company), company.EmailSent);
              }*/
            this.LogTb.Text = this.LogTb.Text + $"List = {this.EmailListClb.Items.Count} \r\n";
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

        private bool SendToRegexEmail(Company company, String pattern)
        {
            foreach (String email in company.Emails)
            {
                Match m = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    SendEmail(email);
                    this.LogTb.Text = this.LogTb.Text + email + " sent\r\n";
                    return true;
                }
            }
            return false;
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            //String testEmail = "tn131191@gmail.com";
            Companies companies = DataAcess.LoadCompanyList();
            //SendEmail(testEmail);
            // this.LogTb.Text = this.LogTb.Text + testEmail  + " sent\r\n";
            int sent = 0;
            foreach (Company company in this.EmailListClb.Items)
            {
                //Try send to job, hr, career email
                try
                {
                    if (!SendToRegexEmail(company, JOB_PATTERN))
                    {
                        if (!SendToRegexEmail(company, INFO_PATTERN))
                        {
                            //Send to the first email on the list
                            foreach (String email in company.Emails) {
                                if (email.Contains("sentry.wixpress.com")) continue;
                                SendEmail(email);
                                this.LogTb.Text = this.LogTb.Text + email + " sent\r\n";
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.LogTb.Text = this.LogTb.Text + company.EmailsToString + " sent\r\n";
                    }
                    Company sentCompany = companies.CompanyList.Find(com => com.Url == company.Url);
                    if (sentCompany != null)
                    {
                        sentCompany.EmailSent = true;
                        DataAcess.Save(companies);
                    }
                    sent++;
                }
                catch (Exception ex)
                {
                    this.LogTb.Text = this.LogTb.Text + company.EmailsToString +  ex.Message + "\r\n";
                }
            }
            MessageBox.Show($"{sent} Emails were sent!");
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
            mail.ReplyTo = new MailAddress(this.replyToTb.Text);
            mail.To.Add(email);
            mail.Subject = this.SubjectTb.Text;
            mail.Body = this.BodyTb.Text;

            System.Net.Mail.Attachment attachment;
            if (this.AttachedFile.Text != ""){
            attachment = new System.Net.Mail.Attachment(this.AttachedFile.Text);
            mail.Attachments.Add(attachment);
            }

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(this.SenderTb.Text, this.PasswordTb.Text);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            if (this.TestEmailTb.Text == "")
            {
                MessageBox.Show("Please input dest email address");
                return;
            }
            try
            {
                SendEmail(this.TestEmailTb.Text);
                this.LogTb.Text = this.LogTb.Text + this.TestEmailTb.Text + " sent\r\n";

            }
            catch (Exception ex)
            {
                this.LogTb.Text = this.LogTb.Text + ex.Message + "\r\n";
            }
        }

        private void ConvertEmailListBtn_Click(object sender, EventArgs e)
        {
            String pattern = "job";
            Companies companies = DataAcess.LoadCompanyList();
            try
            {
                foreach (Company company in this.EmailListClb.Items)
                {
                    foreach (String email in company.Emails)
                    {
                        ComEmail comEmail = new ComEmail();
                        comEmail.Email = email;
                        Match m = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
                        if (m.Success)
                        {
                            //comEmail.Selected = true;
                        }
                        companies.ComEmails.Add(comEmail);
                    }
                }
                DataAcess.Save(companies);
            }
            catch (Exception ex)
            {
                this.LogTb.Text = this.LogTb.Text + ex.Message + "\r\n";
            }
            MessageBox.Show("Email was sent!");
            ReloadEmailList();
        }
    }
}
