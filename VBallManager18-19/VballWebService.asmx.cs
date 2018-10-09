using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Services;
using System.Web.Script.Serialization;

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
        private List<String> birthdayWishes = new List<string>() {"Wishing you a day that is as special in every way as you are. Happy Birthday!!",//
            "Set the world on fire with your dreams and use the flame to light a birthday candle. HAPPY BIRTHDAY!!",//
            "Thinking of you on your birthday, and wishing you all the best! I hope it is as fantastic as you are, you deserve the best and nothing less. Happy Birthday!!",//
            "A birthday is a most special day in one’s life. Enjoy yours to its fullest. HAPPY BIRTHDAY!!",//
            "My birthday wish for you is that you continue to love life and never stop dreaming. May beauty and happiness surround you, not only on your special day, but always. Happy Birthday!!",//
            "Have a wonderful, happy, healthy birthday now and forever. Happy Birthday!",//
            "Another year has passed, and let me just say how much we count on you, rather than count the years. I wish you a wonderful birthday. Happy Birthday!!",//
            "Blowing out another candle should mean that you have lived another year of joy, and that you’ve made this world a better place. Make every day of your life, and every candle, count. Have a delightful birthday!",//
            "Happy Birthday!! Wishing you a wonderful year ahead. Your birthday deserves to be a national holiday, because you are a special, national treasure",//
            "May your birthday and every day be filled with the warmth of sunshine, the happiness of smiles, the sounds of laughter, the feeling of love and the sharing of good cheer. Happy Birthday!!",//
            "I hope you have a wonderful day and that the year ahead is filled with much love, many wonderful surprises and gives you lasting memories that you will cherish in all the days ahead. Happy Birthday.",//
            "On this special day, I wish you all the very best, all the joy you can ever have and may you be blessed abundantly today, tomorrow and the days to come! May you have a fantastic birthday and many more to come; HAPPY BIRTHDAY!!!!"};

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
            NoonRemainder(hour);
            BirthdayWishes(hour);
       }

        private void AutoAssignCoopSpots(int hour)
        {
            String slash = "/";
            String autoReserveUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, HttpContext.Current.Request.ApplicationPath);
            if (!autoReserveUrl.EndsWith(slash)) autoReserveUrl = autoReserveUrl + slash;
            autoReserveUrl = autoReserveUrl + Constants.AUTO_RESERVE + "?hour=" + hour;
            var http = (HttpWebRequest)WebRequest.Create(autoReserveUrl);
            var response = http.GetResponse();
        }

        private void QueryPublishLinks(int hour)
        {
            //
            String reservationUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, HttpContext.Current.Request.ApplicationPath);// +"/" + Constants.POOL_LINK_LIST_PAGE;
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
                         publishTo = "register members";
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
                     Manager.WechatNotifier.AddNotifyWechatMessage(pool, "{ReservationLink}");
                 }
             }
        }

        private void NoonRemainder(int hour)
        {
            String reservationUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, HttpContext.Current.Request.ApplicationPath);// +"/" + Constants.POOL_LINK_LIST_PAGE;
            if (hour != 12) return;
            foreach (Pool pool in Manager.Pools)
            {
                DateTime comingGameDate = FindComingGameDate(pool);
                if (comingGameDate.Date == Manager.EastDateTimeToday.Date)
                {
                    Game game = pool.FindGameByDate(comingGameDate);
                    String message = String.Format("Hi, all. Currently we have {0} players for tonight volleyball. If you are holding a spot but cannot make it , please cancel your spot, thanks. ", game.NumberOfReservedPlayers);
                    int availableSpots = pool.MaximumPlayerNumber - game.NumberOfReservedPlayers;
                    message = message + availableSpots + (availableSpots > 1 ? " spots are" : " spot is") + " available. Click the link to reserve. " + reservationUrl;
                    Manager.WechatNotifier.AddNotifyWechatMessage(pool, message);
                }
            }
        }

        [WebMethod]
        public String RetrieveData(int hour)
        {
            if (hour != 23) return null;
            var jss = new JavaScriptSerializer();
            return jss.Serialize(Manager);

        }

        private DateTime FindComingGameDate(Pool pool)
        {
            DateTime gameDate = Manager.EastDateTimeToday;
            Game targetGame = pool.Games.OrderBy(game => game.Date).ToList<Game>().Find(game => game.Date >= gameDate);
            if (targetGame != null)
            {
                return targetGame.Date;
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

        private void BirthdayWishes(int hour)
        {
            if (hour != 9) return;
            List<Player> birthdayPlayers = Manager.Players.FindAll(player => !String.IsNullOrEmpty(player.Birthday) && player.Birthday.Split('/')[0] == Manager.EastDateTimeToday.Month.ToString() && player.Birthday.Split('/')[1] == Manager.EastDateTimeToday.Day.ToString());
            String message = " 🎉[Cake]🍾💰🎻[Rose]🎁🍻[Packet][Hug]";
            foreach (Player player in birthdayPlayers)
            {
                String[] birthday = player.Birthday.Split('/');
                String poolName = "B";
                if (birthday.Length == 3) poolName = birthday[2];
                String wish = birthdayWishes[new Random().Next(birthdayWishes.Count)];
                Manager.WechatNotifier.AddNotifyWechatMessage(Manager.FindPoolByName(poolName), player, wish + message);
            }
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
