using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Services;

namespace Reservation
{
    /// <summary>
    /// Summary description for VballWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ActivityWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public List<WechatMessage> WechatMessages()
        {
            List<WechatMessage> weChatMassages = Reservations.WechatMessages.ToList();
            foreach (WechatMessage message in weChatMassages)
            {
                Reservations.WechatMessages.Remove(Reservations.WechatMessages.Find(m => m.WechatName == message.WechatName && m.Name == message.Name && m.Message == message.Message));
            }
            DataAccess.Save(Reservations);
            return weChatMassages.ToList();
        }

        private Reservation Reservations
        {
            get
            {
                return (Reservation)Application[Constants.RESERVATION];

            }
            set { }
        }
   
    }
}
