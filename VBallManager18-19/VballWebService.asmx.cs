﻿using System;
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
        public List<WechatMessage> WechatMessages()
        {
            IEnumerable<WechatMessage> weChatMassages = Manager.WechatNotifier.WechatMessages.FindAll(wechat=> wechat.Date.Date == DateTime.Today.Date);
            Manager.WechatNotifier.WechatMessages.Clear();
            DataAccess.Save(Manager);
            return weChatMassages.ToList();
        }

        [WebMethod]
        public void RunScheduleTasks(int hour)
        {
            String homePcIp = GetUserIP();
            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE, GetUserIP() + " - " + Manager.EastDateTimeNow.ToString("yyyy-MM-dd HH:mm:ss"));
            QueryPublishLinks(hour);
            AutoAssignCoopSpots(hour);
       }

        private void AutoAssignCoopSpots(int hour)
        {
            foreach (Pool pool in Manager.Pools)
            {
                DateTime comingGameDate = FindComingGameDate(pool);
                if (pool.AutoCoopReserve && hour == pool.ReservHourForCoop && Manager.EastDateTimeToday.AddDays(pool.DaysToReserve).Date == comingGameDate.Date)
                {
                    BasePage page = new BasePage();
                    page.AutoReserveCoopPlayers(pool, comingGameDate);
                }
            }
        }

        private void QueryPublishLinks(int hour)
        {
            //
            String reservationUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, HttpContext.Current.Request.ApplicationPath) + "/" + Constants.POOL_LINK_LIST_PAGE;
             if (hour != Manager.DropinSpotOpeningHour) return;
             foreach (Pool pool in Manager.Pools)
             {
                 DateTime comingGameDate = FindComingGameDate(pool);
                 String publishTo = null;
                 if (pool.DaysToReserve == pool.DaysToReserve4Member)
                 {
                     if (Manager.EastDateTimeToday.AddDays(pool.DaysToReserve4Member).Date == comingGameDate.Date)
                     {
                         publishTo = "every one";
                     }
                 }
                 else
                 {
                     if (Manager.EastDateTimeToday.AddDays(pool.DaysToReserve4Member).Date == comingGameDate.Date)
                     {
                         publishTo = "registered members";
                     }
                     if (Manager.EastDateTimeToday.AddDays(pool.DaysToReserve).Date == comingGameDate.Date)
                     {
                         publishTo = "every one";
                     }
                 }
                 if (publishTo != null)
                 {
                     int memberPlayers = pool.GetNumberOfAttendingMembers(comingGameDate);
                     int dropinPlayers = pool.GetNumberOfDropins(comingGameDate);
                     int availableDropinSpots = pool.MaximumPlayerNumber - memberPlayers - dropinPlayers;
                     if (availableDropinSpots < 0) availableDropinSpots = 0;
                     String message = pool.DayOfWeek.ToString() + " volleyball reservation starts now for " + publishTo + ". Currently, we have " + availableDropinSpots + (availableDropinSpots<2 ? " dropin spot" : " dropin spots") + " available in pool "+ pool.Name +". Click the link to reserve. " + reservationUrl;
                     Manager.WechatNotifier.AddNotifyWechatMessage(pool, message);
                 }
             }
        }
    

        private DateTime FindComingGameDate(Pool pool)
        {
            DateTime gameDate = Manager.EastDateTimeToday;
            IEnumerable<Game> gameQuery = pool.Games.OrderBy(game => game.Date);
            foreach (Game game in gameQuery)
            {
                if (game.Date >= gameDate)
                {
                    return game.Date;
                }
            }
            return DateTime.MaxValue;
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
