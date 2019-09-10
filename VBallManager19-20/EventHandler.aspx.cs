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
    public partial class Reserve : BasePage
    {

        #region Reserve or cancel click
        protected void Reserve_Click(object sender, EventArgs e)
        {
            if (Handler.IsReservationLocked(TargetGameDate) && !Manager.ActionPermitted(Actions.Change_After_Locked, CurrentUser.Role))
            {
                ShowMessage(appLockedMessage);
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            Session[Constants.CURRENT_PLAYER_ID] = lbtn.ID;
            String playerId = lbtn.ID;
            Player player = Manager.FindPlayerById(playerId);
            Game game = CurrentPool.FindGameByDate(TargetGameDate);
            //Handle dropin
            if (game.Dropins.Exists(playerId))
            {
                Pickup dropin = game.Dropins.FindByPlayerId(playerId);
                if (dropin.IsCoop)
                {
                    if (CurrentPool.AutoCoopReserve && !Manager.ActionPermitted(Actions.Reserve_Coop, CurrentUser.Role))
                    {
                        ShowMessage("Sorry, but the reservation for " + player.Name + " is not permitted in this pool, contact admin for advise");
                        return;
                    }
                    if (Manager.EastDateTimeNow < TargetGameDate.AddHours(CurrentPool.ReservHourForCoop))
                    {
                        ShowMessage("Sorry, but the reservation for " + player.Name + " starts at " + CurrentPool.ReservHourForCoop + " O'clock on game day. Check back later");
                        return;
                    }
                }
                else
                {
                    DateTime dropinSpotOpeningDate = Handler.DropinSpotOpeningDateTime(CurrentPool, TargetGameDate, player);
                    if (Manager.EastDateTimeNow < dropinSpotOpeningDate)
                    {
                        ShowMessage("Sorry, But drop-in reservation for " + player.Name + " cannot be made until " + Manager.DropinSpotOpeningHour + " on " + dropinSpotOpeningDate.ToLongDateString() + ". Please check back later.");
                        return;
                    }
                }
                if (!player.IsRegisterdMember && Handler.IsDropinOwesExceedMax(player))
                {
                    ShowMessage("According to our records, the total amount you unpaid dropin fee reaches the maximum ($" + Manager.MaxDropinFeeOwe + "). Please make the payment prior to new reservations. If you would like e-transfer, send to " + Manager.AdminEmail + ". Thanks");
                    return;
                }
            }
            if (Handler.IsSpotAvailable(CurrentPool, TargetGameDate) || CurrentPool.Members.Exists(player.Id) && Handler.AllowMemberAddReservation(CurrentPool, TargetGameDate, player))
            {
                //Check to see if the player has dropin spot in another pool on same day
                if (IsReservedInAnotherPool(playerId)) return;
            }
            else
            {
                //Todo : Power reserve for Monday player with high factor

                //Power reserve
                if (Manager.ActionPermitted(Actions.Power_Reserve, CurrentUser.Role))
                {
                    Session[Constants.ACTION_TYPE] = Constants.ACTION_POWER_RESERVE;
                    ShowPopupModal("Sorry, But all spots are already filled up. Would you like to reserve an EXTRA spot?");
                }
                else
                {
                    Session[Constants.ACTION_TYPE] = Constants.ACTION_ADD_WAITING_LIST;
                    ShowPopupModal("Sorry, But all spots are already filled up. Would you like to put onto the waiting list?");
                }
                return;
            } 
            Session[Constants.ACTION_TYPE] = Constants.ACTION_RESERVE;
            ShowPopupModal("Are you sure to reserve?");
        }

        private bool IsReservedInAnotherPool(String playerId)
        {
            //Check to see if the player has dropin spot in another pool on same day
            Pool sameDayPool = Manager.Pools.Find(pool => pool.Name != CurrentPool.Name && pool.DayOfWeek == CurrentPool.DayOfWeek);
            if (sameDayPool != null)
            {
                if (sameDayPool.FindGameByDate(TargetGameDate).Members.Items.Exists(member => member.PlayerId == playerId && member.Status == InOutNoshow.In) ||
                    sameDayPool.FindGameByDate(TargetGameDate).Dropins.Items.Exists(dropin => dropin.PlayerId == playerId && dropin.Status == InOutNoshow.In))
                {
                    Session[Constants.ACTION_TYPE] = Constants.ACTION_MOVE_RESERVATION;
                    ShowPopupModal("You have aleady reserved a spot in pool " + sameDayPool.Name + " on the same day. Would you like to cancel that and reserve this?");
                    return true;
                }
                else if (sameDayPool.FindGameByDate(TargetGameDate).WaitingList.Exists(playerId))
                {
                    Session[Constants.ACTION_TYPE] = Constants.ACTION_MOVE_RESERVATION;
                    ShowPopupModal("You are aleady on the waiting list of pool " + sameDayPool.Name + " on the same day. Would you like to cancel that and reserve this?");
                    return true;
                }
            }
            return false;
        }

        //Move spot to high pool
        protected void MoveFromCurrentPool_Click(object sender, EventArgs e)
        {
            if (Manager.ActionPermitted(Actions.Move_To_High_Pool, CurrentUser.Role))
            {
                ImageButton lbtn = (ImageButton)sender;
                Session[Constants.CURRENT_PLAYER_ID] = lbtn.ID.Split(',')[0];
                Session[Constants.ACTION_TYPE] = Constants.ACTION_MOVE_RESERVATION;
                ShowPopupModal("Are you sure to move?");
            }
        }

        //Cancel primary members and dropn pickup, not for waiting list
        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (Handler.IsReservationLocked(TargetGameDate) && !Manager.ActionPermitted(Actions.Change_After_Locked, CurrentUser.Role))
            {
                ShowMessage(appLockedMessage);
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            Session[Constants.CURRENT_PLAYER_ID] = lbtn.ID;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_CANCEL;
            ShowPopupModal("Are you sure to cancel?");
        }

        //Cancel waiting
        protected void Cancel_Waiting_Click(object sender, EventArgs e)
        {
            if (Handler.IsReservationLocked(TargetGameDate) && !Manager.ActionPermitted(Actions.Change_After_Locked, CurrentUser.Role))
            {
                ShowMessage(appLockedMessage);
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            String playerId = lbtn.ID;
            Game game = CurrentPool.FindGameByDate(TargetGameDate);
            game.WaitingList.Remove(playerId);
            LogHistory log = Handler.CreateLog(Manager.EastDateTimeNow, game.Date, Handler.GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Cancel waitinglist", CurrentUser.Name);
            Manager.Logs.Add(log);
            DataAccess.Save(Manager);
            this.PopupModal.Hide();
            Response.Redirect(Constants.RESERVE_PAGE);
        }

        protected void Confirm_Click(object sender, EventArgs e)
        {
            if (Handler.IsReservationLocked(TargetGameDate) && !Manager.ActionPermitted(Actions.Change_After_Locked, CurrentUser.Role))
            {
                ShowMessage(appLockedMessage);
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            String playerId = lbtn.ID;
            Game game = CurrentPool.FindGameByDate(TargetGameDate);
            Attendee member = game.Members.FindByPlayerId(playerId);
            member.Confirmed = true;
            LogHistory log = Handler.CreateLog(Manager.EastDateTimeNow, game.Date, Handler.GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Confirmed", CurrentUser.Name);
            Manager.Logs.Add(log);
            String message = String.Format("You confirmed your reservation for the volleyball game on {0}. If you change your mind, please cancel it. Thanks", game.Date.ToString("MM/dd/yyyy"));
            Manager.WechatNotifier.AddNotifyWechatMessage(Manager.FindPlayerById(playerId), message);
            DataAccess.Save(Manager);
            ShowMessage("Your reservation is confirmed !");
            //Response.Redirect(Constants.RESERVE_PAGE);
        }

        #endregion

        #region Confirmed click
        protected void PowerReserve_Confirm_Click(object sender, EventArgs e)
        {
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            if (IsReservedInAnotherPool(playerId)) return;
            Reserve_Confirm_Click(sender, e);       
        }
        protected void Reserve_Confirm_Click(object sender, EventArgs e)
        {
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            Player player = Manager.FindPlayerById(playerId);
            Game game = CurrentPool.FindGameByDate(TargetGameDate);
            if (Handler.ReserveSpot(CurrentPool, game, player))
            {
                Manager.ReCalculateFactor(CurrentPool, TargetGameDate);
                Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.RESERVED, CurrentPool, CurrentPool, TargetGameDate);
                LogHistory log = Handler.CreateLog(Manager.EastDateTimeNow, game.Date, Handler.GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Reserved", CurrentUser.Name);
                Manager.Logs.Add(log);
                Handler.AutoMoveCoopPlayers(CurrentPool.DayOfWeek, game.Date);
                DataAccess.Save(Manager);
            }
            Response.Redirect(Constants.RESERVE_PAGE);
        }

        protected void Cancel_Confirm_Click(object sender, EventArgs e)
        {
            if (Handler.IsReservationLocked(TargetGameDate))
            {
                Session[Constants.ACTION_TYPE] = Constants.ACTION_NO_SHOW;
                Session[Constants.CONTROL] = sender;
                ShowPopupModal("Is this no-show cancellation?");
                return;
            }
            Continue_Cancel_Click(sender, e);
        }

        protected void Continue_Cancel_Click(object sender, EventArgs e)
        {
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            Player player = Manager.FindPlayerById(playerId);
            Game game = CurrentPool.FindGameByDate(TargetGameDate);
            if (Handler.CancelSpot(CurrentPool, game, player))
            {
                Manager.ReCalculateFactor(CurrentPool, TargetGameDate);
                Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.CANCELLED, CurrentPool, CurrentPool, TargetGameDate);
                LogHistory log = Handler.CreateLog(Manager.EastDateTimeNow, game.Date, Handler.GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Cancelled", CurrentUser.Name);
                Manager.Logs.Add(log);
                Handler.AssignDropinSpotToWaiting(CurrentPool, game);
                Handler.AutoMoveCoopPlayers(CurrentPool.DayOfWeek, game.Date);
            }
            DataAccess.Save(Manager);
            Response.Redirect(Constants.RESERVE_PAGE);
       }

        protected void Move_Confirm_Click(object sender, EventArgs e)
        {
            Game game = CurrentPool.FindGameByDate(TargetGameDate);
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            Player player = Manager.FindPlayerById(playerId);
            if (game.Members.Items.Exists(member => member.PlayerId == playerId && member.Status == InOutNoshow.In) ||
                game.Dropins.Items.Exists(dropin => dropin.PlayerId == playerId && dropin.Status == InOutNoshow.In) ||
                game.WaitingList.Exists(playerId))
            {
                Pool sameDayPool = Manager.Pools.Find(p => p.DayOfWeek == CurrentPool.DayOfWeek && p.Name != CurrentPool.Name);
                game = sameDayPool.FindGameByDate(TargetGameDate);
                MoveSpot(sameDayPool, game, player);
                return;
            }

            MoveSpot(CurrentPool, game, player);
        }

  
        private void MoveSpot(Pool pool, Game game, Player player)
        {
            Pool originalPool = Handler.MoveReservation(pool, game, player);
            Manager.ReCalculateFactor(CurrentPool, TargetGameDate);
            if (originalPool == null)
            {
                Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.RESERVED, pool, pool, game.Date);
                LogHistory log = Handler.CreateLog(Manager.EastDateTimeNow, game.Date, Handler.GetUserIP(), pool.Name, Manager.FindPlayerById(player.Id).Name, "Reserved", CurrentUser.Name);
                Manager.Logs.Add(log);
            }
            else
            {
                Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.MOVED, pool, originalPool, game.Date);
                LogHistory log = Handler.CreateLog(Manager.EastDateTimeNow, game.Date, Handler.GetUserIP(), pool.Name, Manager.FindPlayerById(player.Id).Name, "Moved from " + originalPool.Name, CurrentUser.Name);
                Manager.Logs.Add(log);
            }
            Handler.AutoMoveCoopPlayers(pool.DayOfWeek, game.Date);
            DataAccess.Save(Manager);
            Response.Redirect(Constants.RESERVE_PAGE);
        }

       private void No_Show_Confirm_Click(object sender, EventArgs e)
       {
           String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
           Game game = CurrentPool.FindGameByDate(TargetGameDate);
           Player player = Manager.FindPlayerById(playerId);
           Handler.MarkNoShow(CurrentPool, game, player);
           String message = String.Format("[System Info] Hi, {0}. Admin marked you as no-show on the reservation of {1}. If you have any question, contact the admin", player.Name, game.Date.ToString("MM/dd/yyyy"));
           Manager.WechatNotifier.AddNotifyWechatMessage(player, message);
           LogHistory log = Handler.CreateLog(Manager.EastDateTimeNow, game.Date, Handler.GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Marked no-show", CurrentUser.Name);
           Manager.Logs.Add(log);
           Manager.ReCalculateFactor(CurrentPool, TargetGameDate);
           DataAccess.Save(Manager);
           Response.Redirect(Constants.RESERVE_PAGE);
       }

       protected void AddWaitingList_Confirm_Click(object sender, ImageClickEventArgs e)
       {
           String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
           Game game = CurrentPool.FindGameByDate(TargetGameDate);
           if (game.WaitingList.Exists(playerId))
           {
               return;
           }
           Pool otherPool = Manager.Pools.Find(p => p.Name != CurrentPool.Name && p.DayOfWeek == CurrentPool.DayOfWeek);
           if (otherPool != null && otherPool.Members.Exists(playerId) && !otherPool.FindGameByDate(TargetGameDate).Members.Items.Exists(member => member.PlayerId == playerId && member.Status == InOutNoshow.In))
           {
               //Changed to allow holding a member spot in another pool when putting into waiting list  
               // ShowMessage("Sorry, But you have aleady reserved a spot in pool " + pool.Name + " on the same day. Cancel that spot before adding onto the waiting list");
               //return;
           }
           if (otherPool != null && otherPool.Dropins.Exists(playerId) && otherPool.FindGameByDate(TargetGameDate).Dropins.Items.Exists(dropin => dropin.PlayerId == playerId && dropin.Status == InOutNoshow.In))
           {
               ShowMessage("Sorry, But you have aleady reserved a dropin spot in pool " + otherPool.Name + " on the same day. Cancel that spot beforeadding onto the waiting list");
               return;
           }
           else if (otherPool.FindGameByDate(TargetGameDate).WaitingList.Exists(playerId))
           {
               ShowMessage("Sorry, But you are on waiting list in pool " + otherPool.Name + " on the same day. Cancel that before adding onto the waiting list");
               return;
           }
           Handler.AddToWaitingList(CurrentPool, game, Manager.FindPlayerById(playerId));
           Manager.AddReservationNotifyWechatMessage(playerId, CurrentUser.Id, Constants.WAITING, CurrentPool, CurrentPool, TargetGameDate);
           LogHistory log = Handler.CreateLog(Manager.EastDateTimeNow, game.Date, Handler.GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Added to waiting list", CurrentUser.Name);
           Manager.Logs.Add(log);
           Handler.AutoMoveCoopPlayers(CurrentPool.DayOfWeek, game.Date);
           DataAccess.Save(Manager);
           Response.Redirect(Constants.RESERVE_PAGE);
       }

       protected void InquireAddingToWaitingList_Click(object sender, ImageClickEventArgs e)
       {
           Session[Constants.ACTION_TYPE] = Constants.ACTION_ADD_WAITING_LIST;
           ShowPopupModal("Would you like to put onto the waiting list?");
       }

        #endregion

       #region Add new dropin 
       protected void AddNewDropin_Click(object sender, EventArgs e)
       {
           this.AddDropinLb.Items.Clear();
           List<Player> players = new List<Player>();
           foreach (Player player in Manager.Players)
           {
               if (player.IsActive && !CurrentPool.Members.Exists(player.Id) && !CurrentPool.Dropins.Exists(player.Id))
               {
                   players.Add(player);
               }
           }
           this.AddDropinLb.DataSource = players.OrderBy(player => player.Name);
           this.AddDropinLb.DataTextField = "Name";
           this.AddDropinLb.DataValueField = "Id";
           this.AddDropinLb.DataBind();
           //this.AddDropinImageBtn.Click += new ImageClickEventHandler(AddDropinImageBtn_Click);
           this.AddDropinPopup.Show();
       }

       void AddDropinImageBtn_Click(object sender, ImageClickEventArgs e)
       {
           if (this.AddDropinLb.SelectedIndex >= 0 && !CurrentPool.Dropins.Exists(this.AddDropinLb.SelectedValue))
           {
               Player player = Manager.FindPlayerById(this.AddDropinLb.SelectedValue);
               CurrentPool.AddDropin(player);
               DataAccess.Save(Manager);
               this.AddDropinPopup.Hide();
               Response.Redirect(Request.RawUrl);
           }
       }

       void CreateNewPlayerBtn_Click(object sender, ImageClickEventArgs e)
       {
           String playerName = this.NewPlayerTb.Text.Trim();
           if (String.IsNullOrEmpty(playerName))
           {
               return;
           }
           String operatorId = Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID];
           Player player = Manager.FindPlayerByName(playerName);
           if (player != null)
           {
               if (!CurrentPool.Dropins.Exists(player.Id))
               {
                   CurrentPool.AddDropin(player);
                   player.AuthorizedUsers.Add(operatorId);
                   DataAccess.Save(Manager);
               }
               this.AddDropinPopup.Hide();
               Response.Redirect(Request.RawUrl);
               return;
           }
           player = new Player(playerName, null, false);
           player.AuthorizedUsers.Add(operatorId);
           Manager.Players.Add(player);
           CurrentPool.AddDropin(player);
           DataAccess.Save(Manager);
           this.AddDropinPopup.Hide();
           Response.Redirect(Request.RawUrl);
       }
       #endregion

        #region Others
       protected void ToReadmeBtn_Click(object sender, EventArgs e)
       {
           Response.Redirect("Readme.aspx");
       }
       protected void RateBtn_Click(object sender, EventArgs e)
       {
           Response.Redirect("ParticipationRate.aspx?Pool=" + CurrentPool.Name);
       }
        #endregion
    }
 }