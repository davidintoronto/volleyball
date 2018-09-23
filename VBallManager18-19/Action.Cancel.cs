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

        protected void CancelPromarySpot(Pool pool, Game game, Player player)
        {
            Attendee attendee = game.Members.FindByPlayerId(player.Id);
            //Cancel spots for members
            if (attendee != null && attendee.Status != InOutNoshow.Out)
            {
                attendee.Status = InOutNoshow.Out;
                if (!Manager.ClubMemberMode)
                {
                    Transfer transfer = new Transfer(game.Date);
                    player.Transfers.Add(transfer);
                    CostReference costRef = new CostReference(CostType.TRANSFER, transfer.TransferId);
                    attendee.CostReference = costRef;
                }
                //Log and save
                //LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), pool.Name, player.Name, "Cancel member");
                //Manager.Logs.Add(log);
                //Assgin a spot to the first one on waiting list
                if (!IsReservationLocked(game.Date) && game.WaitingList.Count > 0 && IsSpotAvailable(pool, game.Date))
                {
                    //AssignDropinSpotToWaiting(pool, game);
                }
                DataAccess.Save(Manager);
                return;
            }
        }

        protected void CancelDropinSpot(Pool pool, Game game, Player player)
        {
            //Cancel spots for dropin
            Pickup dropin = game.Dropins.FindByPlayerId(player.Id);
            //Cancel spots for members
            if (dropin != null && dropin.Status != InOutNoshow.Out)
            {
                dropin.Status = InOutNoshow.Out;
                //Cancel dropin fee
                CancelDropinFee(dropin);
                Game comingGame = Manager.FindComingGame(pool);
                if (game.Date.Date == comingGame.Date.Date && !pool.Dropins.Exists(player.Id))
                {
                    game.Dropins.Remove(dropin);
                }
                //Move first one in waiting list into dropin list
                if (!IsReservationLocked(game.Date) && game.WaitingList.Count > 0 && IsSpotAvailable(pool, game.Date))
                {
                    //AssignDropinSpotToWaiting(pool, game);
                }
                DataAccess.Save(Manager);
            }
        }



       private void CancelDropinFee(Attendee attendee)
        {
            if (attendee.CostReference == null) return;
            Player player = Manager.FindPlayerById(attendee.PlayerId);
            CostType type = attendee.CostReference.CostType;
            if (type == CostType.CLUB_MEMBER || type == CostType.REACH_MAX)
            {
                return;
            }
            if (type == CostType.TRANSFER)
            {
                //Find the original transfer used when reserving, and set it to unused
                Transfer transfer = player.FindTransferById(attendee.CostReference.ReferenceId);
                if (transfer != null)
                {
                    transfer.IsUsed = false;
                }
                return;
            }
            if (type == CostType.PRE_PAID)
            {
                player.PrePaidBalance += Manager.DropinFee;
                return;
            }
            if (type == CostType.FEE)
            {
                Fee fee = player.FindFeeById(attendee.CostReference.ReferenceId);
                player.Fees.Remove(fee);
                return;
            }
            if (type == CostType.FREE)
            {
                player.FreeDropin++;
            }
            if (player.TransferUsed == 0)
            {
                player.FreeDropin++;
                return;
            }
        }

       protected bool CancelSpot(Pool pool, Game game, Player player)
       {
           if (pool == null) return false;

           if (game.Members.Items.Exists(member => member.PlayerId == player.Id && member.Status != InOutNoshow.Out))
           {
               CancelPromarySpot(pool, game, player);
               return true;
           }
           else if (game.Dropins.Items.Exists(dropin => dropin.PlayerId == player.Id && dropin.Status != InOutNoshow.Out))
           {
               CancelDropinSpot(pool, game, player);
               return true;
           }
           else if (game.WaitingList.Exists(player.Id))
           {
               game.WaitingList.Remove(player.Id);
           }
           return false;
       }
    }
}