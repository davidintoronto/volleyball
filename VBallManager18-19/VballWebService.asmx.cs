using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Services;

namespace VballManager
{
    /// <summary>
    /// Summary description for VballWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class VballWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public List<String> WechatMessages()
        {
            List<String> weChatMassages = new List<string>(); 
            foreach (WechatMessage message in Manager.WechatMessages)
            {
                if (message.Date.Date == DateTime.Today.Date)
                {
                    weChatMassages.Add(message.Message);
                }
            }
            Manager.WechatMessages.Clear();
            DataAccess.Save(Manager);
            return weChatMassages;
        }

        [WebMethod]
        public void HomePcIP()
        {
            String homePcIp = GetUserIP();
            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE, GetUserIP() + " - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Application[Constants.HOME_PC_IP] = homePcIp;
        }

        private string GetUserIP()
        {
            string strIP = String.Empty;
            HttpRequest httpReq = HttpContext.Current.Request;

            //test for non-standard proxy server designations of client's IP
            if (httpReq.ServerVariables["HTTP_CLIENT_IP"] != null)
            {
                strIP = httpReq.ServerVariables["HTTP_CLIENT_IP"].ToString();
            }
            else if (httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                strIP = httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            //test for host address reported by the server
            else if
            (
                //if exists
                (httpReq.UserHostAddress.Length != 0)
                &&
                //and if not localhost IPV6 or localhost name
                ((httpReq.UserHostAddress != "::1") || (httpReq.UserHostAddress != "localhost"))
            )
            {
                strIP = httpReq.UserHostAddress;
            }
            //finally, if all else fails, get the IP from a web scrape of another server
            else
            {
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    strIP = sr.ReadToEnd();
                }
                //scrape ip from the html
                int i1 = strIP.IndexOf("Address: ") + 9;
                int i2 = strIP.LastIndexOf("</body>");
                strIP = strIP.Substring(i1, i2 - i1);
            }
            return strIP;
        }



        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }
    }
}
