using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net; 
using System.Text.RegularExpressions;

namespace JobResumeSender
{
    public partial class Console : Form
    {
        private const String DIRECTORY_URL_PATTERN = "http://www.toronto.net/directory/.*\\.html";
        private const String SUB_CATEGORY_URL_PATTERN = "http://www.toronto.net/[^/]*\\.html\" title";
        private const String URL_PATTERN = "http\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)([a-zA-Z0-9\\-\\.\\?\\,\\'\\/\\\\\\+&%\\$#_]*)?";
        private const String EMAIL_PATTERN = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        private const String EXCLUDED_IMAGE_PATTERN = ".*\\.(?i)(jpg|jpeg|png|gif|pif|pdf)$";
        private const String PREFIX = "Web: </b><a href=\"";
        private const String WWW = "www.";
        public Console()
        {
            InitializeComponent();
        }

        private void RunBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.LogTb.Text = GetHttpResponse(this.ListUrl.Text);
            }
            catch (Exception ex)
            {
                this.LogTb.Text = ex.Message;
            }
        }

        private String GetHttpResponse(String url)
        {
            try
            {
                // Create a request for the URL.   
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.  
                //request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CategoryRunBtn_Click(object sender, EventArgs e)
        {
         /*   if (CategaryLb.SelectedIndex < 0)
            {
                MessageBox.Show("No category selected!");
                return;
            }
         */
            Companies companyList = DataAcess.LoadCompanyList();
            List<String> companyLinkUrls = new List<string>();
            for (int i = 0; i < CategaryLb.Items.Count; i++)
            {
                String categoryUrl = CategaryLb.Items[i].ToString();
                companyLinkUrls.AddRange(GetUrls(categoryUrl, DIRECTORY_URL_PATTERN, ""));
                List<String> subCategoryUrls = GetUrls(categoryUrl, SUB_CATEGORY_URL_PATTERN, "");
                foreach (String subCategoryUrl in subCategoryUrls)
                {
                    List<String> urls = GetUrls(subCategoryUrl, DIRECTORY_URL_PATTERN, "");
                    foreach (String companyLinkUrl in urls)
                    {
                        if (companyLinkUrls.Contains(companyLinkUrl))
                        {
                            companyLinkUrls.Add(companyLinkUrl);
                        }
                    }
                }
            }

            foreach (String companyLinkUrl in companyLinkUrls)
            {
                Company company = new Company();
                company.LinkUrl = companyLinkUrl;
                companyList.CompanyList.Add(company);
                this.LogTb.Text = this.LogTb.Text + companyLinkUrl + "\r\n";
            }
             DataAcess.Save(companyList);
             MessageBox.Show("Done, " + companyLinkUrls.Count + " found");
        }

    private void RunUrlBtn_Click(object sender, EventArgs e)
    {
        Companies companyList = DataAcess.LoadCompanyList();
        foreach (Company company in companyList.CompanyList)
        {
            if (company.Searched || company.LinkUrl == null || company.LinkUrl == "") continue;
            company.Url = GetUrl(company.LinkUrl);
            //company.Searched = true;
            DataAcess.Save(companyList);
            this.LogTb.Text = this.LogTb.Text + company.LinkUrl + "  |  " + company.Url + "\r\n";
        }
         MessageBox.Show("Done, ");
        }

        private List<String> GetUrls(String categoryUrl, String regex, String replaceString)
        {
            String response = GetHttpResponse(categoryUrl);
            //Parse response string
            Regex rx = new Regex(regex,
          RegexOptions.Compiled | RegexOptions.Multiline);


            // Find matches.
            MatchCollection matches = rx.Matches(response);

            List<String> urls = new List<string>();
            // Report on each match.
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                if (groups.Count>0)
                {
                    String url = replaceString == "" ? groups[0].Value : groups[0].Value.Replace(replaceString, "");
                    if (!urls.Contains(url))
                    {
                        urls.Add(url);
                    }
                }
            }
            return urls;
        }

        private String GetUrl(String linkUrl)
        {
            String response = GetHttpResponse(linkUrl);
            int index = response.IndexOf(PREFIX);
            if (index < 0) return null;
            response = response.Substring(index + PREFIX.Length);
            index = response.IndexOf("\"");
            String url = response.Substring(0, index);
            return url;      
        }

        private List<String> GetEmails(List<String> searchedUrlList, String baseUrl, String url, int level, int maxLevel)
        {
            searchedUrlList.Add(url);
            this.LogTb.Text = "Working on email from " + url;
            String response = GetHttpResponse(url);
            //Parse response string
            Regex rx = new Regex(EMAIL_PATTERN,
          RegexOptions.Compiled | RegexOptions.Multiline);


            // Find matches.
            MatchCollection matches = rx.Matches(response);

            List<String> emails = new List<string>();
            // Report on each match.
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                if (groups.Count > 0 || !emails.Contains(groups[0].Value))
                {
                    emails.Add(groups[0].Value);
                }
            }
            if (level >= maxLevel) return emails;
            //looking for links
            Uri uri = new Uri(baseUrl);
            List<String> subUrls = GetUrls(url, URL_PATTERN, "");
            foreach (String subUrl in subUrls)
            {
                try
                {
                    if (searchedUrlList.Contains(subUrl)) continue;
                    Uri subUri = new Uri(subUrl);
                    if (subUri.Host.Replace(WWW, "") == uri.Host.Replace(WWW, "") || subUri.Host.Replace(WWW, "").IndexOf(uri.Host.Replace(WWW, "")) > 0 || uri.Host.Replace(WWW, "").IndexOf(subUri.Host.Replace(WWW, "")) > 0)
                    {
                        Match m = Regex.Match(subUrl, EXCLUDED_IMAGE_PATTERN, RegexOptions.IgnoreCase);
                        if (m.Success) continue;
                        emails.AddRange(GetEmails(searchedUrlList, baseUrl, subUrl, level + 1, maxLevel));
                    }
                }
                catch (Exception) { }
            }
            return emails;
        }

        private void RunEmailBtn_Click(object sender, EventArgs e)
        {
            Companies companies = DataAcess.LoadCompanyList();
            foreach (Company company in companies.CompanyList)
            {
                if (company.Searched) continue;
                company.Url = company.Url.Replace("%20", "");
                List<String> emails = GetEmails(new List<String>(), company.Url, company.Url, 0, 3);
                company.Emails = emails;
                company.Searched = true;
                DataAcess.Save(companies);
            }
        }

        private void ViewListBtn_Click(object sender, EventArgs e)
        {
            Companies companies = DataAcess.LoadCompanyList();
            int total = 0;
            foreach (Company company in companies.CompanyList)
            {
                //List<String> emails = new List<string>();
                if (company.Emails.Count > 0)
                {
                    for(int i=0; i<company.Emails.Count; i++)
                    {
                        if (company.Emails[i].EndsWith("ca") || company.Emails[i].StartsWith("hr"))
                        {
                            //if (!emails.Contains(company.Emails[i]))
                            //{
                                //emails.Add(company.Emails[i]);
                            if (company.Emails[i].StartsWith("job") || company.Emails[i].EndsWith("com"))
                            {
                                this.LogTb.Text = this.LogTb.Text + company.Emails[i] + "\r\n";
                                total++;
                            }
                           // }

                          
                        }else
                        {
                            company.Emails[i] = "";
                        }
                    }
                    //company.Emails = emails;
                }
                //String companyView = company.LinkUrl + " | " + company.Url + " | Searched:" + company.Searched + " | " + company.EmailsToString;
                //this.LogTb.Text = this.LogTb.Text + companyView + "\r\n";
            }
           // DataAcess.Save(companies);
            this.LogTb.Text = this.LogTb.Text + "Total: " + total;
        }
    }
}