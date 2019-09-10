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
    public partial class ActionHandler
    {
        private VolleyballClub manager;
        private Pool currentPool;
        private Player currentUser;
        private DateTime targetGameDate;

        public DateTime TargetGameDate
        {
            get { return targetGameDate; }
            set { targetGameDate = value; }
        }

        public Player CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        public Pool CurrentPool
        {
            get { return currentPool; }
            set { currentPool = value; }
        }

        public VolleyballClub Manager
        {
            get { return manager; }
            set { manager = value; }
        }


        public bool IsPermitted(Actions action, Player player)
        {
            if ((TargetGameDate == null || TargetGameDate.AddDays(3) < Manager.EastDateTimeToday) && !Manager.ActionPermitted(Actions.Change_Past_Games, CurrentUser.Role))
            {
                return false;
            }
            if (player.Role == (int)Roles.Guest || Manager.ActionPermitted(action, CurrentUser.Role) || CurrentUser.Id == player.Id || player.AuthorizedUsers.Contains(CurrentUser.Id))
            {
                return true;
            }
            return false;
        }

        private LogHistory CreateLog(DateTime date, DateTime gameDate, String userInfo, String poolName, String playerName, String type)
        {
            if (CurrentUser != null)
            {
                return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, CurrentUser.Name);
            }
            return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, "Unknown");
        }

        public LogHistory CreateLog(DateTime date, DateTime gameDate, String userInfo, String poolName, String playerName, String type, String operater)
        {
            return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, operater);
        }
        public string GetUserIP()
        {
            return null;
        }

        public bool IsReservationLocked(DateTime gameDate)
        {
            return Manager.IsReservationLocked(gameDate);
        }

        public DateTime DropinSpotOpeningDateTime(Pool pool, DateTime gameDate, Player player)
        {
            DateTime reserveDate = gameDate;
            if (player.IsRegisterdMember && !pool.Dropins.FindByPlayerId(player.Id).WaiveBenefit)
            {
                //If this is Friday pool, check to see if player attend most recent monday game
                if (pool.DayOfWeek == DayOfWeek.Friday && Manager.IsPlayerAttendedThisWeekMondayGame(gameDate, player))
                {
                    Pool anotherDaySameLevelPool = Manager.Pools.Find(p => p.DayOfWeek != pool.DayOfWeek && p.IsLowPool == pool.IsLowPool);
                    Game mondayOfSameWeek = Manager.FindMondayGameOfSameLevelInSameWeek(pool, gameDate);
                    if (mondayOfSameWeek != null && mondayOfSameWeek.Factor >= pool.FactorForAdvancedReserve)
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
        public bool AllowMemberAddReservation(Pool pool, DateTime gameDate, Player player)
        {
            DateTime reserveDate = gameDate;
            //Allow member to reserve before the time of dropin reservation
            if (Manager.EastDateTimeNow < reserveDate.AddDays(-1 * pool.DaysToReserve).AddHours(-1 * reserveDate.Hour + Manager.DropinSpotOpeningHour)) {
                return true;
            }
            else //Allow member to add back to reservation list if no dropin yet
            {
                Game game = pool.FindGameByDate(gameDate);
                if (game.Dropins.Items.FindAll(d=>d.Status == InOutNoshow.In).Count == 0) return true;
            }
            return false;
        }


        //Calculate factor for next reservation
        public decimal CalculateNextFactor(Pool pool, DateTime gameDate)
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
                int coopNumberOfPlayers = sameDayPool.FindGameByDate(gameDate).Dropins.Items.FindAll(pickup => pickup.IsCoop && pickup.Status == InOutNoshow.In).Count;
                //sameDayPoolNumberOfPlayers = sameDayPoolNumberOfPlayers - coopNumberOfPlayers;
                int moveIntern = CalculateMoveIntern(sameDayPool, pool, sameDayPoolGame, game);
                if (gameDate == Manager.EastDateTimeToday && Manager.EastDateTimeNow.Hour >= pool.ReservHourForCoop && Manager.EastDateTimeNow.Hour < pool.SettleHourForCoop &&//
                    moveIntern == 1 && DropinSpotAvailableForCoop(sameDayPool, gameDate))//Move to high pool
                {
                    coopNumberOfPlayers++;
                    sameDayPoolNumberOfPlayers++; //A
                }
                else
                {
                    currentPoolNumberOfPlayer++; //B
                }
                factor = Manager.Factors.Find(f => f.PoolName == anotherDayPool.Name && f.LowPoolName == pool.Name && f.LowPoolNumberFrom <= currentPoolNumberOfPlayer && currentPoolNumberOfPlayer <= f.LowPoolNumberTo &&//
                    f.CoopNumberFrom <= coopNumberOfPlayers && coopNumberOfPlayers <= f.CoopNumberTo && f.HighPoolName == sameDayPool.Name && f.HighPoolNumberFrom <= sameDayPoolNumberOfPlayers &&//
                   sameDayPoolNumberOfPlayers <= f.HighPoolNumberTo);
            }
            else
            {
                int coopNumberOfPlayers = pool.FindGameByDate(gameDate).Dropins.Items.FindAll(pickup => pickup.IsCoop && pickup.Status == InOutNoshow.In).Count;
                //currentPoolNumberOfPlayer = currentPoolNumberOfPlayer - coopNumberOfPlayers;
                int moveIntern = CalculateMoveIntern(pool, sameDayPool, game, sameDayPoolGame);
                if (gameDate == Manager.EastDateTimeToday && Manager.EastDateTimeNow.Hour >= pool.ReservHourForCoop && Manager.EastDateTimeNow.Hour < pool.SettleHourForCoop &&//
                    coopNumberOfPlayers > 0 && moveIntern == -1 && sameDayPoolGame.NumberOfReservedPlayers < sameDayPool.MaximumPlayerNumber) //Move back to low pool
                {
                    coopNumberOfPlayers--;
                    sameDayPoolNumberOfPlayers++;//B
                }
                else
                {
                    currentPoolNumberOfPlayer++;//A
                }
                factor = Manager.Factors.Find(f => f.PoolName == anotherDayPool.Name && f.LowPoolName == sameDayPool.Name && f.LowPoolNumberFrom <= sameDayPoolNumberOfPlayers && sameDayPoolNumberOfPlayers <= f.LowPoolNumberTo &&//
                     f.CoopNumberFrom <= coopNumberOfPlayers && coopNumberOfPlayers <= f.CoopNumberTo && f.HighPoolName == pool.Name && f.HighPoolNumberFrom <= currentPoolNumberOfPlayer &&//
                     currentPoolNumberOfPlayer <= f.HighPoolNumberTo);
            }
            if (factor == null) return 0;
            return factor.Value;
        }

        public decimal CalculateFactor(Pool pool, Pool lowPool, Pool highPool, DateTime gameDate)
        {
            int lowPoolNumberOfPlayer = lowPool.FindGameByDate(gameDate).NumberOfReservedPlayers;
            int highPoolNumberOfPlayer = highPool.FindGameByDate(gameDate).NumberOfReservedPlayers;
            int coopNumberOfPlayers = highPool.FindGameByDate(gameDate).Dropins.Items.FindAll(pickup => pickup.IsCoop && pickup.Status == InOutNoshow.In).Count;
            //highPoolNumberOfPlayer = highPoolNumberOfPlayer - coopNumberOfPlayers;
            Factor factor = Manager.Factors.Find(f => f.PoolName == pool.Name && f.LowPoolName == lowPool.Name && f.LowPoolNumberFrom <= lowPoolNumberOfPlayer && lowPoolNumberOfPlayer <= f.LowPoolNumberTo &&//
                f.CoopNumberFrom <= coopNumberOfPlayers && coopNumberOfPlayers <= f.CoopNumberTo && f.HighPoolName == highPool.Name && f.HighPoolNumberFrom <= highPoolNumberOfPlayer &&//
               highPoolNumberOfPlayer <= f.HighPoolNumberTo);
            if (factor == null) return 0;
            return factor.Value;
        }

        public bool IsSpotAvailable(Pool pool, DateTime gameDate)
        {
            if (!pool.HasCap)
            {
                return true;
            }
            int memberPlayers = pool.GetNumberOfAttendingMembers(gameDate);
            int dropinPlayers = pool.GetNumberOfDropins(gameDate);
            return memberPlayers + dropinPlayers < pool.MaximumPlayerNumber;
        }

        public static bool DropinSpotAvailableForCoop(Pool pool, DateTime gameDate)
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