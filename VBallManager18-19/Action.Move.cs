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
        #region Auto Move
        public void AutoMoveCoopPlayers(DayOfWeek day, DateTime gameDate)
        {
            List<Pool> pools = Manager.Pools.FindAll(pool => pool.DayOfWeek == day);
            if (pools.Count != 2) return;
            Pool highPool = Manager.Pools.Find(pool => pool.DayOfWeek == day && !pool.IsLowPool);
            Pool lowPool = Manager.Pools.Find(pool => pool.DayOfWeek == day && pool.IsLowPool);
            if (highPool.AutoCoopReserve && Manager.EastDateTimeToday == gameDate.Date && Manager.EastDateTimeNow.Hour >= highPool.ReservHourForCoop)
            {
                //Set last coop date for each coop player
                CalculatelastCoopDate(highPool, gameDate);
                Game highPoolGame = highPool.FindGameByDate(gameDate);
                Game lowPoolGame = lowPool.FindGameByDate(gameDate);
                int moveIntern = CalculateMoveIntern(highPool, lowPool, highPoolGame, lowPoolGame);
                if (moveIntern == -1 && Manager.EastDateTimeNow.Hour < highPool.SettleHourForCoop)
                {
                    Dropin coopDropin = FindBestCoopCandidateToMoveBackOrignalPool(highPool, highPoolGame, lowPoolGame);
                    if (coopDropin != null )
                    {
                        Player coopPlayer = Manager.FindPlayerById(coopDropin.PlayerId);
                        MoveReservation(lowPool, lowPoolGame, coopPlayer);
                        Manager.ReCalculateFactor(lowPool, gameDate);
                        //Manager.ReCalculateFactor(highPool, gameDate);
                        String wechatMessage = String.Format("Sorry, but we had to move your spot back to pool {0} for tonight's volleyball in order to balance the players in each pool. However we may move your spot again later when things change.", lowPool.Name);
                        Manager.WechatNotifier.AddNotifyWechatMessage(coopPlayer, wechatMessage);
                        Manager.AddReservationNotifyWechatMessage(coopPlayer.Id, CurrentUser.Id, Constants.MOVED, lowPool, highPool, lowPoolGame.Date);
                        LogHistory log = CreateLog(Manager.EastDateTimeNow, gameDate.Date, GetUserIP(), lowPool.Name, coopPlayer.Name, "Moved from " + highPool.Name, "Admin");
                        Manager.Logs.Add(log);
                        DataAccess.Save(Manager);
                    }
                    return;
                }
                //Check to see if moving coop to high level pool required, and number of reserved coop players already reaches maximum
                while (moveIntern == 1 && DropinSpotAvailableForCoop(highPool, gameDate))
                {
                    //Find the best coop 
                    Dropin coopDropin = FindNextCoopCandidateToMoveHighPool(highPool, highPoolGame, lowPoolGame);
                    if (coopDropin == null)
                    {
                        break;
                    }
                    Player coopPlayer = Manager.FindPlayerById(coopDropin.PlayerId);
                    MoveReservation(highPool, highPoolGame, coopPlayer);
                    Manager.ReCalculateFactor(lowPool, gameDate);
                    //Manager.ReCalculateFactor(highPool, gameDate);
                    String wechatMessage = String.Format("We have moved your spot from pool {0} to pool {1} for tonight's volleyball in order to balance the players in each pool. However we may move you back later when things change." //
                    + " if you don't receive any further notification by {2} o'clock, then this is the final arrangement.", lowPool.Name, highPool.Name, highPool.SettleHourForCoop);
                    Manager.WechatNotifier.AddNotifyWechatMessage(coopPlayer, wechatMessage);
                    Manager.AddReservationNotifyWechatMessage(coopPlayer.Id, null, Constants.MOVED, highPool, lowPool, lowPoolGame.Date);
                    LogHistory log = CreateLog(Manager.EastDateTimeNow, gameDate.Date, GetUserIP(), highPool.Name, coopPlayer.Name, "Moved from " + lowPool.Name, "Admin");
                    Manager.Logs.Add(log);
                    DataAccess.Save(Manager);
                    moveIntern = CalculateMoveIntern(highPool, lowPool, highPoolGame, lowPoolGame);
                }
            }
        }

        public int CalculateMoveIntern(Pool highPool, Pool lowPool, Game highPoolGame, Game lowPoolGame)
        {
            int highPoolNumber = highPoolGame.NumberOfReservedPlayers;
            int highPoolWaiting = highPoolGame.WaitingList.Count;
            int intern = highPoolGame.Dropins.Items.FindAll(d => d.IsCoop && d.Status == InOutNoshow.In).Count;
            int lowPoolNumber = lowPoolGame.NumberOfReservedPlayers;
            int lowPoolWaiting = lowPoolGame.WaitingList.Count;
            int hour = Manager.EastDateTimeNow.Hour;
            MoveRule moveRule = Manager.MoveRules.Find(mr => hour>= mr.Hour && mr.LowPoolName == lowPool.Name && mr.LowPoolNumberFrom <= lowPoolNumber && lowPoolNumber <= mr.LowPoolNumberTo &&//
                lowPoolWaiting >= mr.LowPoolWaiting && mr.CoopNumberFrom <= intern && intern <= mr.CoopNumberTo && mr.HighPoolName == highPool.Name &&//
                mr.HighPoolNumberFrom <= highPoolNumber && highPoolNumber <= mr.HighPoolNumberTo && highPoolWaiting >= mr.HighPoolWaiting);
            if (moveRule != null) return moveRule.ToMove;
            return 0;
        }

        public void CalculatelastCoopDate(Pool pool, DateTime gameDate)
        {
            //Find all the coop player
            List<Dropin> coops = pool.Dropins.Items.FindAll(coop => coop.IsCoop);
            //Find the date of last coop for each candidate
            foreach (Dropin dropin in coops)
            {
                IEnumerable<Game> games = pool.Games.FindAll(game => game.Date < Manager.EastDateTimeToday).OrderByDescending(game => game.Date);
                foreach (Game game in games)
                {
                    Attendee att = game.Dropins.FindByPlayerId(dropin.PlayerId);
                    if (att != null && att.Status == InOutNoshow.In)
                    {
                        dropin.LastCoopDate = game.Date;
                        break;
                    }
                }
            }
        }

        public Dropin FindNextCoopCandidateToMoveHighPool(Pool highPool, Game highPoolGame, Game lowPoolGame)
        {
            Dropin coopCandidate = null;
            //Find the best candidate of coop
            //Excluding the coop who cancelled his spot for coming game in his original pool, but including the intern is the first one in waiting list
            List<Dropin> interns = highPool.Dropins.Items.FindAll(coop => coop.IsCoop && //
                highPoolGame.Dropins.FindByPlayerId(coop.PlayerId) != null && highPoolGame.Dropins.FindByPlayerId(coop.PlayerId).Status == InOutNoshow.Out &&//
                lowPoolGame.AllPlayers.FindByPlayerId(coop.PlayerId) != null && (lowPoolGame.AllPlayers.FindByPlayerId(coop.PlayerId).Status == InOutNoshow.In||//
                (lowPoolGame.WaitingList.Count >0 && lowPoolGame.WaitingList[0].PlayerId==coop.PlayerId))); //intern is the first peron in waiting list 
            //If search result is empty, 
            //Compare the last coop date to find the best one
            List<Dropin> candidates = new List<Dropin>();
            foreach (Dropin dropin in interns)
            {
                //ignore the intern who did not attend the game in low pool last time
                Game previewGame = Manager.FindSameDayPool(highPool).Games.OrderByDescending(game => game.Date).ToList<Game>().Find(game=> game.Date < lowPoolGame.Date);
                //Game previewHighPoolGame = highPool.FindGameByDate(previewGame.Date);
                if (previewGame != null && previewGame.AllPlayers.Items.Exists(p => p.PlayerId == dropin.PlayerId && p.Status == InOutNoshow.In))// || //
                    //previewHighPoolGame.AllPlayers.Items.Exists(p => p.PlayerId == dropin.PlayerId && p.Status == InOutNoshow.In)))
                {
                    candidates.Add(dropin);
                }
            }
            //if no one in the result, then all the interns will be candidetes
            if (candidates.Count == 0) candidates = interns;
            foreach(Dropin dropin in candidates)
            {
               if (coopCandidate == null || coopCandidate.LastCoopDate > dropin.LastCoopDate) coopCandidate = dropin;
            }
            return coopCandidate;
        }

        private Dropin FindBestCoopCandidateToMoveBackOrignalPool(Pool highPool, Game highPoolGame, Game lowPoolGame)
        {
            Dropin coopCandidate = null;
            //Find the best candidate of coop
            //Filter the coop who cancelled his spot for coming game in his original pool
            List<Dropin> coopPickups = highPool.Dropins.Items.FindAll(coop => coop.IsCoop && //
                highPoolGame.Dropins.FindByPlayerId(coop.PlayerId) != null && highPoolGame.Dropins.FindByPlayerId(coop.PlayerId).Status == InOutNoshow.In &&//
                lowPoolGame.AllPlayers.FindByPlayerId(coop.PlayerId) != null && lowPoolGame.AllPlayers.FindByPlayerId(coop.PlayerId).Status == InOutNoshow.Out);
            //Compare the last coop date to find the best one
            foreach (Dropin dropin in coopPickups)
            {
                if (coopCandidate == null || coopCandidate.LastCoopDate <= dropin.LastCoopDate) coopCandidate = dropin;
            }
            return coopCandidate;
        }

        private bool PlayerAttendedLastWeekGame(Pool thePool, String playerId)
        {
            foreach (Pool pool in Manager.Pools)
            {
                if (pool.DayOfWeek == thePool.DayOfWeek)
                {
                    Game previousGame = null;
                    List<Game> games = pool.Games;
                    IEnumerable<Game> gameQuery = games.OrderBy(game => game.Date);

                    foreach (Game game in gameQuery)
                    {
                        if (game.Date < TargetGameDate)
                        {
                            previousGame = game;
                        }
                        else
                        {
                            break;
                        }
                    }
                    //Return true if current game is the first one in a season
                    if (previousGame == null)
                    {
                        return true;
                    }
                    //Return true if the player is member and attend previous game
                    if (pool.Members.Exists(playerId) && previousGame.Members.Items.Exists(a => a.PlayerId == playerId && a.Status == InOutNoshow.In))
                    {
                        return true;
                    }
                    //Return true if the player is dropin and attend previous game
                    if (pool.Dropins.Exists(playerId) && previousGame.Dropins.Items.Exists(a => a.PlayerId == playerId && a.Status == InOutNoshow.In))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        #endregion

    }
}