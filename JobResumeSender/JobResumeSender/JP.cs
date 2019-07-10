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
    public partial class JP : Form
    {
        private const String DIRECTORY_URL_PATTERN = "https://www.toronto.com/toronto-directory/importers/?q=&pageindex=";
        //private const String GOOGLE_URL = "https://www.googleapis.com/customsearch/v1?cx=009399559288465221048:upj7nxz8nbg&num=1&key=AIzaSyBndwD6rYZhU7-Y5LGwiU3YfQv4QboRmgM&q="; //hitmen
        private const String GOOGLE_URL = "https://www.googleapis.com/customsearch/v1?cx=003689026316507884853:gvecrjbewxm&num=1&key=AIzaSyB94sFiPXL_pBejd3fsthyzACI_O8loaA0&q="; //van
                                                                                                                                                                                  // private const String GOOGLE_URL = "https://www.googleapis.com/customsearch/v1?cx=018418324288658183346:2lzokngxwji&num=1&key=AIzaSyDFva4HX6VBFJZ6h5bKCY6VAMBOT851A9M&q="; //david.zzh
        private const String URL_BEGIN_PATTERN = "?redirect=";
        private const String URL_END_PATTERN = "\"";
        private const String NAME_BEGIN_PATTERN = "title=\"";
        private const String NAME_END_PATTERN = " - Business Website";
        private const String URL_PATTERN = "<cite class=[^>]*>([^<]*)<\\/cite>";
        private const String EMAIL_PATTERN = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        private const String EXCLUDED_IMAGE_PATTERN = ".*\\.(?i)(jpg|jpeg|png|gif|pif|pdf|ico)$";
        private const String PREFIX = "<td width=\"50%\"><a href=\"";
        private const String WWW = "www.";
        public JP()
        {
            InitializeComponent();
        }

        private void RunBtn_Click(object sender, EventArgs e)
        {
            Companies list = DataAcess.LoadCompanyList();
            foreach(Company com in list.CompanyList)
            {
                //this.LogTb.Text = this.LogTb.Text + "\r\n" + com.Name + " - " + com.Url;
            }
            this.LogTb.Text = this.LogTb.Text + "\r\nTotal: " + list.CompanyList.Count;
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

        private void LinkRunBtn_Click(object sender, EventArgs e)
        {
            Companies companyList = DataAcess.LoadCompanyList();
            List<Company> companies = new List<Company>();
            foreach (Company com in companyList.CompanyList)
            {
                    if (!companies.Exists(c => c.Url == com.Url))
                        companies.Add(com);
                else
                {
                    Company existCom = companies.Find(c => c.Url == com.Url);
                    if (existCom.EmailSearched) continue;
                    if (com.EmailSearched)
                    {
                        companies.Remove(existCom);
                        companies.Add(com);
                    }
                }
            }
            foreach (Company com in companies)
                if (!com.Url.EndsWith("/"))
                    com.Url = com.Url + "/";
            companyList.CompanyList = companies;
            DataAcess.Save(companyList);
        }

        private void RunUrlBtn_Click(object sender, EventArgs e)
        {
            List<String> textFileList = new List<string> { "jp-exporters.txt", //
                "jp-importer-markham.txt", "jp-importer-mississauga.txt", "jp-importer-richmondhill.txt",//
                "jp-importers-toronto.txt", "jp-insurance-toronto.txt", "jp-trade-markham.txt", "jp-trade-mississauga.txt", //
                "jp-trade-richmondhill.txt", "jp-trade-toronto.txt", "jp-insurance-markham.txt", "jp-insurance-richmondhill.txt", "jp-insurance-mississauga.txt"};
            Companies companyList = DataAcess.LoadCompanyList();
            foreach (String textFile in textFileList)
            {
                StreamReader sr = File.OpenText(textFile);
                String allText = sr.ReadToEnd();
                int index = -1;
                while ((index = allText.IndexOf(URL_BEGIN_PATTERN)) > 0)
                {
                    //Url
                    allText = allText.Substring(index + URL_BEGIN_PATTERN.Length);
                    index = allText.IndexOf(URL_END_PATTERN);
                    String url = Uri.UnescapeDataString(allText.Substring(0, index));
                    url = url.EndsWith("/") ? url : url + "/";
                    //Name
                    index = allText.IndexOf(NAME_BEGIN_PATTERN);
                    allText = allText.Substring(index + NAME_BEGIN_PATTERN.Length);
                    index = allText.IndexOf(NAME_END_PATTERN);
                    String name = allText.Substring(0, index);
                    if (!url.Contains("facebook") && !companyList.UrlExist(url))
                    {
                        Company company = new Company();
                        company.Name = name;
                        company.Url = url;
                        companyList.CompanyList.Add(company);
                    }
                }
            }
            DataAcess.Save(companyList);
            MessageBox.Show("Done, ");
        }

        private String GetUrlFromGoogleSearch(String name)
        {
            String searchUrl = GOOGLE_URL + name.Replace(" ", "+");
            try
            {
                String resp = GetHttpResponse(searchUrl);
                Result result = DataAcess.GetResult(resp);
                /*
                Regex rx = new Regex(URL_PATTERN,
             RegexOptions.Compiled | RegexOptions.Multiline);


                // Find matches.
                MatchCollection matches = rx.Matches(resp);

                // Report on each match.
                if (matches.Count >0)
                {
                    url = matches[0].Groups[1].Value;                   
                }
                return url;
                */
                if (result.items.Count >= 0)
                {
                    this.LogTb.Text = this.LogTb.Text + "\r\n" + name + " - " + result.items[0].link;
                    return result.items[0].link;
                }
                this.LogTb.Text = this.LogTb.Text + "\r\n" + name + " - ";
                return null;
            }
            catch(Exception ex)
            {
                this.LogTb.Text = this.LogTb.Text + "\r\n" + ex.Message;
                return null;
            }
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
                        emails.AddRange(GetEmails(searchedUrlList, Uri.UnescapeDataString(baseUrl), Uri.UnescapeDataString(subUrl), level + 1, maxLevel));
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
                if (company.EmailSearched) continue;
                company.Url = company.Url.Replace("%20", "");
                List<String> emails = GetEmails(new List<String>(), Uri.UnescapeDataString(company.Url), Uri.UnescapeDataString(company.Url), 0, 3);
                company.Emails = emails;
                company.EmailSearched = true;
                DataAcess.Save(companies);
            }
            MessageBox.Show("Done");
        }

        private void CaComListBtn_Click(object sender, EventArgs e)
        {
            Companies companies = DataAcess.LoadCompanyList();
            int total = 0;
            foreach (Company company in companies.CompanyList)
            {
                List<String> emails = new List<string>();
                if (company.Emails.Count > 0)
                {
                    for(int i=0; i<company.Emails.Count; i++)
                    {
                        if (company.Emails[i].EndsWith("wixpress.com") || company.Emails[i].EndsWith("411.ca")) continue;
                        if (company.Emails[i].EndsWith("ca") || company.Emails[i].EndsWith("com"))
                        {
                            if (!emails.Contains(company.Emails[i]))
                            {
                                emails.Add(company.Emails[i]);
                           // if (company.Emails[i].StartsWith("job") || company.Emails[i].EndsWith("com"))
                            {
                                this.LogTb.Text = this.LogTb.Text + company.Emails[i] + "\r\n";
                                total++;
                            }
                           }

                          
                        }else
                        {
                            company.Emails[i] = "";
                        }
                    }
                    company.Emails = emails;
                }
                //String companyView = company.LinkUrl + " | " + company.Url + " | Searched:" + company.Searched + " | " + company.EmailsToString;
                //this.LogTb.Text = this.LogTb.Text + companyView + "\r\n";
            }
            DataAcess.Save(companies);
            this.LogTb.Text = this.LogTb.Text + "Total: " + total;
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
                    for (int i = 0; i < company.Emails.Count; i++)
                    {
                                     this.LogTb.Text = this.LogTb.Text + company.Emails[i] + "\r\n";
                                    total++;
                    }
                    //company.Emails = emails;
                }
                //String companyView = company.LinkUrl + " | " + company.Url + " | Searched:" + company.Searched + " | " + company.EmailsToString;
                //this.LogTb.Text = this.LogTb.Text + companyView + "\r\n";
            }
            //DataAcess.Save(companies);
            this.LogTb.Text = this.LogTb.Text + "Total: " + total;
        }

        private void MergeBtn_Click(object sender, EventArgs e)
        {
            Companies companies = DataAcess.LoadCompanyList();
            Companies companies500 = DataAcess.LoadCompanyList500();
            foreach (Company company in companies500.CompanyList)
            {
                if (companies.UrlExist(company.Url)) continue;
                companies.CompanyList.Add(company);
            }
            DataAcess.Save(companies);
        }

        private void ResetSearchEmailBtn_Click(object sender, EventArgs e)
        {
            Companies companies = DataAcess.LoadCompanyList();
            foreach (Company company in companies.CompanyList)
            {
                company.EmailSearched = false;
            }
            DataAcess.Save(companies);

        }
    }
}