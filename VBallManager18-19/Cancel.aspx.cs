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
    public partial class Default : System.Web.UI.Page
    {

        private void CancelPromarySpot(Pool pool, Game game, Player player)
        {
            Attendee attendee = game.Members.FindByPlayerId(player.Id);
            //Cancel spots for members
            if (attendee != null && attendee.Status == InOutNoshow.In)
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
                LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), pool.Name, player.Name, "Cancel member");
                Manager.Logs.Add(log);
                //Assgin a spot to the first one on waiting list
                if (!this.lockReservation && game.WaitingList.Count > 0 && Validation.IsSpotAvailable(pool, ComingGameDate))
                {
                    AssignDropinSpotToWaiting(pool, game);
                }
                DataAccess.Save(Manager);
                return;
            }
        }

        private void CancelDropinSpot(Pool pool, Game game, Player player)
        {
            //Cancel spots for dropin
            Attendee attendee = game.Dropins.FindByPlayerId(player.Id);
            //Cancel spots for members
            if (attendee != null && attendee.Status == InOutNoshow.In)
            {
                attendee.Status = InOutNoshow.Out;
                //Cancel dropin fee
                CancelDropinFee(attendee);
                LogHistory log = CreateLog(DateTime.Now, game.Date, GetUserIP(), pool.Name, Manager.FindPlayerById(player.Id).Name, "Cancel dropin");
                Manager.Logs.Add(log);
                //reset last dropin time for coop
                Dropin dropin = pool.Dropins.FindByPlayerId(player.Id);
                if (dropin.IsCoop) dropin.LastCoopDate = new DateTime();
                //Move first one in waiting list into dropin list
                if (!this.lockReservation && game.WaitingList.Count > 0 && Validation.IsSpotAvailable(pool, ComingGameDate))
                {
                    AssignDropinSpotToWaiting(pool, game);
                }
                DataAccess.Save(Manager);
               // this.PopupModal.Hide();
            }
          // Response.Redirect(Constants.DEFAULT_PAGE);
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

       private bool CancelSpot(Pool pool, Game game, Player player)
       {
           if (pool == null) return false;

           Game gameInOtherPool = pool.FindGameByDate(ComingGameDate);
           if (gameInOtherPool.Members.Items.Exists(member => member.PlayerId == player.Id && member.Status != InOutNoshow.Out))
           {
               CancelPromarySpot(pool, gameInOtherPool, player);
           }
           else if (gameInOtherPool.Dropins.Items.Exists(dropin => dropin.PlayerId == player.Id && dropin.Status != InOutNoshow.Out))
           {
               CancelDropinSpot(pool, gameInOtherPool, player);
           }
           else if (gameInOtherPool.WaitingList.Exists(player.Id))
           {
               gameInOtherPool.WaitingList.Remove(player.Id);
               return false;
           }
           return true;
       }
    }
}