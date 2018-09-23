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
    public partial class BasePage
    {
        #region Reserve
        protected bool ReserveSpot(Pool pool, Game game, Player player)
        {
            if (game.Members.Exists(player.Id))
            {
                return ReservePromarySpot(pool, game, player);
            }
            else// if (game.Dropins.Exists(player.Id))
            {
                return ReserveDropinSpot(pool, game, player);
            }
            //return false;
        }


        protected bool ReservePromarySpot(Pool pool, Game game, Player player)
        {
            Attendee attendee = game.Members.FindByPlayerId(player.Id);
            //Cancel spots for members
            if (attendee != null && attendee.Status == InOutNoshow.Out)
            {
                attendee.Status = InOutNoshow.In;
                attendee.OperatorId = CurrentUser.Id;
                if (!Manager.ClubMemberMode)
                {
                    if (attendee.CostReference == null && attendee.CostReference.CostType == CostType.TRANSFER)
                    {
                        Transfer transfer = player.Transfers.Find(tran => tran.TransferId == attendee.CostReference.ReferenceId);
                        player.Transfers.Remove(transfer);
                    }
                }
                return true;
             }
            return false;
        }

        protected bool ReserveDropinSpot(Pool pool, Game game, Player player)
        {
            Pickup dropin = game.Dropins.FindByPlayerId(player.Id);
            //Add pickup if it is not regular dropin player
            if (dropin == null)
            {
                dropin = new Pickup(player.Id);
                dropin.Status = InOutNoshow.Out;
                game.Dropins.Add(dropin);
            }
            if (dropin != null && dropin.Status == InOutNoshow.Out)
            {
                dropin.Status = InOutNoshow.In;
                //Create fee
                CostReference reference = CreateDropinFee(pool, game.Date, player.Id);
                dropin.CostReference = reference;
                if (CurrentUser != null ) dropin.OperatorId = CurrentUser.Id;
                return true;
            }
            return false;
        }

        //Return the original pool where spot is moved from
        protected Pool MoveReservation(Pool pool, Game game, Player player)
        {
            ReserveSpot(pool, game, player);
            //Cancel the spot in other pool
            Pool sameDayPool = Manager.Pools.Find(p => p.DayOfWeek == pool.DayOfWeek && p.Name != pool.Name);
            if (CancelSpot(sameDayPool, sameDayPool.FindGameByDate(game.Date), player))
            {
                return sameDayPool;
            }
            else
            {
                return null;
            }
          }
        #endregion

        #region Waiting list
        protected void AddToWaitingList(Pool pool, Game game, Player player)
        {
            Waiting waiting = new Waiting(player.Id);
            waiting.OperatorId = GetOperatorId();
            game.WaitingList.Add(waiting);
            //Manager.AddReservationNotifyWechatMessage(player.Id, waiting.OperatorId, Constants.WAITING, pool, pool, game.Date);
            //LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), pool.Name, player.Name, "Add to Waiting List");
           // Manager.Logs.Add(log);
        }

        protected void AssignDropinSpotToWaiting(Pool thePool, Game theGame)
        {
            if (IsReservationLocked(theGame.Date) || theGame.WaitingList.Count == 0 || !IsSpotAvailable(thePool, theGame.Date))
            {
                return;
            }
            Waiting waiting = theGame.WaitingList[0];
            String playerId = waiting.PlayerId;
            Player player = Manager.FindPlayerById(playerId);
            ReserveSpot(thePool, theGame, player);
            theGame.WaitingList.Remove(playerId);
            Manager.AddReservationNotifyWechatMessage(playerId, null, Constants.WAITING_TO_RESERVED, thePool, thePool, theGame.Date);
            LogHistory log = CreateLog(Manager.EastDateTimeNow, theGame.Date, GetUserIP(), thePool.Name, Manager.FindPlayerById(playerId).Name, "Reserved", "Admin");
            Manager.Logs.Add(log);
            theGame.WaitingList.Remove(playerId);
            //Cancel the member spot in another pool on same day
            Pool sameDayPool = Manager.Pools.Find(pool => pool.Name != thePool.Name && pool.DayOfWeek == thePool.DayOfWeek);
            if (CancelSpot(sameDayPool, sameDayPool.FindGameByDate(theGame.Date), player))
            {
                Manager.AddReservationNotifyWechatMessage(playerId, null, Constants.CANCELLED, sameDayPool, sameDayPool, theGame.Date);
                AssignDropinSpotToWaiting(sameDayPool, sameDayPool.FindGameByDate(theGame.Date));
            }
        }


        #endregion

        #region Fee

        private CostReference CreateDropinFee(Pool pool, DateTime gameDate, String playerId)
        {
            Player player = Manager.FindPlayerById(playerId);
            //No cost if clust member mode and the player is the registered member
            if (Manager.ClubMemberMode && player.IsRegisterdMember)
            {
                return new CostReference(CostType.CLUB_MEMBER, null);
            }
            //No fee created and remove one transfer for the dropin player who is the member with cancelled spot in another pool on same day.
            foreach (Pool thePool in Manager.Pools)
            {
                if (thePool.Name != thePool.Name && thePool.DayOfWeek == thePool.DayOfWeek && thePool.Members.Exists(playerId))
                {
                    player.RemoveTransferByGameDate(gameDate);
                    return new CostReference(CostType.TRANSFER, null);
                }
            }
            //Check to see if the player has free of charge of dropin
            if (player.FreeDropin > 0)
            {
                player.FreeDropin--;
                /*Fee fee = new Fee(0);
                fee.Date = gameDate;
                fee.FeeType = "Free -" + String.Format(Fee.FEETYPE_DROPIN, pool.Name);
                fee.IsPaid = false;
                player.Fees.Add(fee);*/
                return new CostReference(CostType.FREE, null);
            }
            //Check to see if the player has paid total amount that reaches the membership fee
            if (ReachMaxDropinFeePaid(player))
            {
                return new CostReference(CostType.REACH_MAX, null);
            }
            if (player.TransferUsed < Manager.MaxTransfers)
            {
                Transfer transfer = player.GetAvailableTransfer(gameDate);
                if (transfer != null)
                {
                    transfer.IsUsed = true;
                    transfer.ApplyGameDate = gameDate;
                    return new CostReference(CostType.TRANSFER, transfer.TransferId);
                }
            }
            //Deduct from prepaid balance if it is enough
            if (player.PrePaidBalance >= Manager.DropinFee)
            {
                player.PrePaidBalance -= Manager.DropinFee;
                return new CostReference(CostType.PRE_PAID, null);
            }
            //last case is to create dropin fee
            Fee fee = new Fee(Manager.DropinFee);
            fee.Date = gameDate;
            fee.FeeType = FeeTypeEnum.Dropin.ToString();
            fee.FeeDesc = String.Format(Fee.FEETYPE_DROPIN, pool.Name);
            player.Fees.Add(fee);
            //Send wechat reminder if dropin fee reaches the max allow
            if (!player.IsRegisterdMember && IsDropinOwesExceedMax(player))
            {
                String message = "[System Info] Hi, " + player.Name + ". According to our records, the total amount you unpaid dropin fee reaches the maximum ($" + Manager.MaxDropinFeeOwe + "). Please make the payment ASAP, in order to continue making reservation in the future.";
                Manager.WechatNotifier.AddNotifyWechatMessage(player, message);
            }
            return new CostReference(CostType.FEE, fee.FeeId);
        }

        protected bool IsDropinOwesExceedMax(Player player)
        {
            decimal total = 0;
            foreach (Fee fee in player.Fees)
            {
                if (!fee.IsPaid) total = total + fee.Amount;
            }
            return total >= Manager.MaxDropinFeeOwe;
        }

        private bool ReachMaxDropinFeePaid(Player player)
        {
            //No dropin fee cap
            if (!Manager.IsDropinFeeWithCap)
            {
                return false;
            }
            //Check to see if the player has paid total amount that reaches the membership fee
            int amountPaid = 0;
            foreach (Pool pool in Manager.Pools)
            {
                if (pool.DayOfWeek == pool.DayOfWeek)
                {
                    foreach (Game game in pool.Games)
                    {
                        if (game.Dropins.Items.Exists(dropin => dropin.PlayerId == player.Id && dropin.Status == InOutNoshow.In))
                        {
                            foreach (Fee fee in player.Fees)
                            {
                                if (fee.Date == game.Date && (fee.Amount == Manager.DropinFee || fee.Amount == 0))
                                {
                                    amountPaid += Manager.DropinFee;
                                }
                            }
                        }
                    }
                }
            }
            return amountPaid >= Manager.RegisterMembeshipFee;
        }
        #endregion

         protected void MarkNoShow(Pool pool, Game game, Player player)
        {
            Attendee attendee = game.Members.Items.Find(member => member.PlayerId == player.Id && member.Status == InOutNoshow.In);
            if (attendee != null)
            {
                attendee.Status = InOutNoshow.NoShow;
            }
            else
            {
                attendee = game.Dropins.Items.Find(dropin => dropin.PlayerId == player.Id && dropin.Status == InOutNoshow.In);
                if (attendee != null)
                {
                    attendee.Status = InOutNoshow.NoShow;
                }
            }
            //LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), pool.Name, player.Name, "Marked as No-Show");
            //Manager.Logs.Add(log);
        }
    }
}