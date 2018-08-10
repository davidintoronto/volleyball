using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
