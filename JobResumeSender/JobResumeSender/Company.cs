using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobResumeSender
{
    public class Companies
    {
        private List<Company> companyList = new List<Company>();

        public List<Company> CompanyList
        {
            get { return companyList; }
            set { companyList = value; }
        }

        public bool LinkUrlExist(String linkUrl)
        {
            return companyList.Exists(c => c.LinkUrl == linkUrl);
        }
        public bool UrlExist(String url)
        {
            return companyList.Exists(c => c.Url == url);
        }

    }

    public class Company
    {
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private String linkUrl;

        public String LinkUrl
        {
            get { return linkUrl; }
            set { linkUrl = value; }
        }
        private String url;

        public String Url
        {
            get { return url; }
            set { url = value; }
        }
        private List<String> emails = new List<string>();

        public List<String> Emails
        {
            get { return emails; }
            set { emails = value; }
        }
        private bool searched;

        public bool Searched
        {
            get { return searched; }
            set { searched = value; }
        }
        private bool emailSent;

        public bool EmailSent
        {
            get { return emailSent; }
            set { emailSent = value; }
        }

        public String EmailsToString
        {
            get
            {
                String emailString = "";
                foreach (String email in emails)
                {
                    if (emailString == "")
                    {
                        emailString = email;
                    }
                    else
                    {
                        emailString = emailString + ", " + email;
                    }
                }
                    return emailString;
            }

        }
                
    }
}
