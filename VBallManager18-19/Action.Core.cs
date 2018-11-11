using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

namespace VballManager
{
    public partial class BasePage : System.Web.UI.Page
    {

        protected VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }

        protected Pool CurrentPool
        {
            get
            {
                String poolName = (String)Session[Constants.POOL];
                return Manager.FindPoolByName(poolName);
            }
            set { }
        }

        protected Player CurrentUser
        {
            get
            {
                return (Player)Session[Constants.CURRENT_USER];
            }
            set { }
        }

        protected DateTime TargetGameDate
        {
            get
            {
                return (DateTime)Session[Constants.GAME_DATE];

            }
            set { }
        }

        protected bool IsPermitted(Actions action, Player player)
        {
            if ((Session[Constants.GAME_DATE] == null || TargetGameDate.AddDays(3) < Manager.EastDateTimeToday) && !Manager.ActionPermitted(Actions.Change_Past_Games, CurrentUser.Role))
            {
                return false;
            }
            if (player.Role == (int)Roles.Guest || Manager.ActionPermitted(action, CurrentUser.Role) || CurrentUser.Id == player.Id || player.AuthorizedUsers.Contains(CurrentUser.Id))
            {
                return true;
            }
            return false;
        }

        protected String GetOperatorId()
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null && Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]) != null)
            {
                return Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]).Id;
            }
            return null;
        }
        private LogHistory CreateLog(DateTime date, DateTime gameDate, String userInfo, String poolName, String playerName, String type)
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null && Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]) != null)
            {
                return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]).Name);
            }
            return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, "Unknown");
        }

        protected LogHistory CreateLog(DateTime date, DateTime gameDate, String userInfo, String poolName, String playerName, String type, String operater)
        {
            return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, operater);
        }
        public string GetUserIP()
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

        protected String GetStatusImageUrl(InOutNoshow status)
        {
            if (status == InOutNoshow.In) return "~/Icons/In.png";
            else if (status == InOutNoshow.Out) return "~/Icons/Out.png";
            else return "~/Icons/noShow.png";
        }

        protected bool IsReservationLocked(DateTime gameDate)
        {
            return Manager.IsReservationLocked(gameDate);
        }

        protected DateTime DropinSpotOpeningDate(Pool pool, DateTime gameDate, Player player)
        {
            DateTime reserveDate = TimeZoneInfo.ConvertTimeFromUtc(gameDate, TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName));
            if (player.IsRegisterdMember && !pool.Dropins.FindByPlayerId(player.Id).WaiveBenefit)
            {
                //If this is Friday pool, check to see if player attend most recent monday game
                if (pool.DayOfWeek == DayOfWeek.Friday && Manager.IsPlayerAttendedThisWeekMondayGame(gameDate, player))
                {
                   Pool anotherDayLowPool = Manager.Pools.Find(p => p.DayOfWeek != pool.DayOfWeek && p.IsLowPool);
                   Pool anotherDayhighPool = Manager.Pools.Find(p => p.DayOfWeek != pool.DayOfWeek && !p.IsLowPool);
                   decimal factor = CalculateFactor(pool, anotherDayLowPool, anotherDayhighPool, gameDate);
                   if (factor >= pool.FactorForAdvancedReserve)
                   {
                       return reserveDate.AddDays(-1 * pool.DaysToReserve4MondayPlayer).AddHours(-1 * reserveDate.Hour + Manager.DropinSpotOpeningHour);
                   }
                }
                return reserveDate.AddDays(-1 * pool.DaysToReserve4Member).AddHours(-1 * reserveDate.Hour + Manager.DropinSpotOpeningHour);
            }
            else
            {
                return reserveDate.AddDays(-1 * pool.DaysToReserve).AddHours(-1 * reserveDate.Hour + Manager.DropinSpotOpeningHour);
            }
        }


        //Calculate factor for next reservation
        protected decimal CalculateNextFactor(Pool pool, DateTime gameDate)
        {
            Pool anotherDayPool = Manager.Pools.Find(p => p.DayOfWeek != pool.DayOfWeek && p.IsLowPool == pool.IsLowPool);
            Game game = pool.FindGameByDate(gameDate);
            int currentPoolNumberOfPlayer = game.NumberOfReservedPlayers;
            Pool sameDayPool = Manager.Pools.Find(p => p.DayOfWeek == pool.DayOfWeek && p.Name != pool.Name);
            Game sameDayPoolGame = sameDayPool.FindGameByDate(gameDate);
            int sameDayPoolNumberOfPlayers = sameDayPoolGame.NumberOfReservedPlayers;
            Factor factor = null;
            if (pool.IsLowPool)
            {
                int coopNumberOfPlayers = sameDayPool.FindGameByDate(gameDate).Dropins.Items.FindAll(pickup => pickup.IsCoop).Count;
                if (MoveToHighPoolRequired(sameDayPool, pool, sameDayPoolGame, game) && DropinSpotAvailableForCoop(sameDayPool, gameDate))
                {
                    coopNumberOfPlayers++;
                    sameDayPoolNumberOfPlayers++;
                }
                else
                {
                    currentPoolNumberOfPlayer++;
                }
                factor = Manager.Factors.Find(f => f.PoolName == anotherDayPool.Name && f.LowPoolName == pool.Name && f.LowPoolNumberFrom <= currentPoolNumberOfPlayer && currentPoolNumberOfPlayer <= f.LowPoolNumberTo &&//
                    f.CoopNumberFrom <= coopNumberOfPlayers && coopNumberOfPlayers <= f.CoopNumberTo && f.HighPoolName == sameDayPool.Name && f.HighPoolNumberFrom <= sameDayPoolNumberOfPlayers &&//
                   sameDayPoolNumberOfPlayers <= f.HighPoolNumberTo);
            }
            else
            {
                int coopNumberOfPlayers = pool.FindGameByDate(gameDate).Dropins.Items.FindAll(pickup => pickup.IsCoop).Count;
                if (MoveToHighPoolRequired(pool, sameDayPool, game, sameDayPoolGame) && DropinSpotAvailableForCoop(pool, gameDate))
                {
                    coopNumberOfPlayers++;
                    currentPoolNumberOfPlayer++;
                }
                else
                {
                    sameDayPoolNumberOfPlayers++;
                }
                factor = Manager.Factors.Find(f => f.PoolName == anotherDayPool.Name && f.LowPoolName == sameDayPool.Name && f.LowPoolNumberFrom <= sameDayPoolNumberOfPlayers && sameDayPoolNumberOfPlayers <= f.LowPoolNumberTo &&//
                     f.CoopNumberFrom <= coopNumberOfPlayers && coopNumberOfPlayers <= f.CoopNumberTo && f.HighPoolName == pool.Name && f.HighPoolNumberFrom <= currentPoolNumberOfPlayer &&//
                     currentPoolNumberOfPlayer <= f.HighPoolNumberTo);
            }
            if (factor == null) return 0;
            return factor.Value;
        }

        protected decimal CalculateFactor(Pool pool, Pool lowPool, Pool highPool, DateTime gameDate)
        {
            int lowPoolNumberOfPlayer = lowPool.FindGameByDate(gameDate).NumberOfReservedPlayers;
            int highPoolNumberOfPlayer = highPool.FindGameByDate(gameDate).NumberOfReservedPlayers;
            int coopNumberOfPlayers = highPool.FindGameByDate(gameDate).Dropins.Items.FindAll(pickup => pickup.IsCoop && pickup.Status == InOutNoshow.In).Count;
            Factor factor = Manager.Factors.Find(f => f.PoolName == pool.Name && f.LowPoolName == lowPool.Name && f.LowPoolNumberFrom <= lowPoolNumberOfPlayer && lowPoolNumberOfPlayer <= f.LowPoolNumberTo &&//
                f.CoopNumberFrom <= coopNumberOfPlayers && coopNumberOfPlayers <= f.CoopNumberTo && f.HighPoolName == highPool.Name && f.HighPoolNumberFrom <= highPoolNumberOfPlayer &&//
               highPoolNumberOfPlayer <= f.HighPoolNumberTo);
            if (factor == null) return 0;
            return factor.Value;
        }

        protected bool IsDropinSpotOpeningForCoop(Pool pool, DateTime gameDate, Player player)
        {
            DateTime reserveDate = TimeZoneInfo.ConvertTimeFromUtc(gameDate, TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName));
            reserveDate = reserveDate.AddDays(-1 * pool.DaysToReserve).AddHours(-1 * reserveDate.Hour + pool.ReservHourForCoop);
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName));
            return now >= reserveDate;
        }

        protected bool IsSpotAvailable(Pool pool, DateTime gameDate)
        {
            if (!pool.HasCap)
            {
                return true;
            }
            int memberPlayers = pool.GetNumberOfAttendingMembers(gameDate);
            int dropinPlayers = pool.GetNumberOfDropins(gameDate);
            return memberPlayers + dropinPlayers < pool.MaximumPlayerNumber;
        }

        protected static bool DropinSpotAvailableForCoop(Pool pool, DateTime gameDate)
        {
            if (!pool.HasCap)
            {
                return true;
            }
            int reservedCoop = pool.GetNumberOfReservedCoops(gameDate);
            if (reservedCoop >= pool.MaxCoopPlayers)
            {
                return false;
            }
            int memberPlayers = pool.GetNumberOfAttendingMembers(gameDate);
            int dropinPlayers = pool.GetNumberOfDropins(gameDate);
            return memberPlayers + dropinPlayers < pool.LessThanPayersForCoop;
        }

    }
}