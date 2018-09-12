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
            if (IsReservationLocked(TargetGameDate) && !Manager.ActionPermitted(Actions.Change_After_Locked, CurrentUser.Role))
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
                if (dropin.IsCoop && !IsDropinSpotOpeningForCoop(CurrentPool, TargetGameDate, player))
                {
                    ShowMessage("Sorry, but Pool " + CurrentPool.Name + " reservation starts at  " + CurrentPool.ReservHourForCoop + " O'clock on game day for co-op players. Check back later");
                    return;
                }
                else if (!dropin.IsCoop && !IsDropinSpotOpening(CurrentPool, TargetGameDate, player))
                {
                    DateTime reserveDate = TargetGameDate.AddDays(-1 * CurrentPool.DaysToReserve4Member);
                    if (!player.IsRegisterdMember)
                    {
                        reserveDate = TargetGameDate.AddDays(-1 * CurrentPool.DaysToReserve);
                    }
                    ShowMessage("Sorry, But drop-in reservations cannot be made until " + Manager.DropinSpotOpeningHour + " on " + reserveDate.ToLongDateString() + ". Please check back later.");
                    return;
                }
                if (!player.IsRegisterdMember && IsDropinOwesExceedMax(player))
                {
                    ShowMessage("According to our records, the total amount you unpaid dropin fee reaches the maximum ($" + Manager.MaxDropinFeeOwe + "). Please make the payment prior to new reservations. If you would like e-transfer, send to " + Manager.AdminEmail + ". Thanks");
                    return;
                }
            }
            if (IsSpotAvailable(CurrentPool, TargetGameDate))
            {
                //Check to see if the player has dropin spot in another pool on same day
                if (IsReservedInAnotherPool(playerId)) return;
            }
            else
            {
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

        //Cancel primary members and dropn pickup, not for waiting list
        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (IsReservationLocked(TargetGameDate) && !Manager.ActionPermitted(Actions.Change_After_Locked, CurrentUser.Role))
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
            if (IsReservationLocked(TargetGameDate) && !Manager.ActionPermitted(Actions.Change_After_Locked, CurrentUser.Role))
            {
                ShowMessage(appLockedMessage);
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            String playerId = lbtn.ID;
            Game game = CurrentPool.FindGameByDate(TargetGameDate);
            game.WaitingList.Remove(playerId);
            LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Cancel waitinglist", CurrentUser.Name);
            Manager.Logs.Add(log);
            DataAccess.Save(Manager);
            this.PopupModal.Hide();
            Response.Redirect(Constants.RESERVE_PAGE);
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
            if (ReserveSpot(CurrentPool, game, player))
            {
                Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.RESERVED, CurrentPool, CurrentPool, TargetGameDate);
                LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Reserved", CurrentUser.Name);
                Manager.Logs.Add(log);
                AutoMoveCoopPlayers(CurrentPool.DayOfWeek, game.Date);
                DataAccess.Save(Manager);
            }
            Response.Redirect(Constants.RESERVE_PAGE);
        }

        protected void Cancel_Confirm_Click(object sender, EventArgs e)
        {
            if (IsReservationLocked(TargetGameDate))
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
            if (CancelSpot(CurrentPool, game, player))
            {
                Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.CANCELLED, CurrentPool, CurrentPool, TargetGameDate);
                LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Cancelled", CurrentUser.Name);
                Manager.Logs.Add(log);
            }
            AssignDropinSpotToWaiting(CurrentPool, game);
            AutoMoveCoopPlayers(CurrentPool.DayOfWeek, game.Date);
            DataAccess.Save(Manager);
            Response.Redirect(Constants.RESERVE_PAGE);
       }

        protected void Move_Confirm_Click(object sender, EventArgs e)
        {
            Game game = CurrentPool.FindGameByDate(TargetGameDate);
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            Player player = Manager.FindPlayerById(playerId);
            Pool originalPool = MoveReservation(CurrentPool, game, player);
            if (originalPool == null)
            {
                Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.RESERVED, CurrentPool, CurrentPool, game.Date);
                LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Reserved", CurrentUser.Name);
                Manager.Logs.Add(log);
            }
            else
            {
                Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.MOVED, CurrentPool, originalPool, game.Date);
                LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Moved from " + originalPool.Name, CurrentUser.Name);
                Manager.Logs.Add(log);
            }
            AutoMoveCoopPlayers(CurrentPool.DayOfWeek, game.Date);
            DataAccess.Save(Manager);
            Response.Redirect(Constants.RESERVE_PAGE);
        }

       private void No_Show_Confirm_Click(object sender, EventArgs e)
       {
           String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
           Game game = CurrentPool.FindGameByDate(TargetGameDate);
           Player player = Manager.FindPlayerById(playerId);
           MarkNoShow(CurrentPool, game, player);
           String message = String.Format("[System Info] Hi, {0}. Admin marked you as no-show on the reservation of {1}. If you have any question, contact the admin", player.Name, game.Date.ToString("MM/dd/yyyy"));
           Manager.WechatNotifier.AddNotifyWechatMessage(player, message);
           LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Marked no-show", CurrentUser.Name);
           Manager.Logs.Add(log);
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
           AddToWaitingList(CurrentPool, game, Manager.FindPlayerById(playerId));
           Manager.AddReservationNotifyWechatMessage(playerId, CurrentUser.Id, Constants.WAITING, CurrentPool, CurrentPool, TargetGameDate);
           LogHistory log = CreateLog(Manager.EastDateTimeNow, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Added to waiting list", CurrentUser.Name);
           Manager.Logs.Add(log);
           AutoMoveCoopPlayers(CurrentPool.DayOfWeek, game.Date);
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
        #endregion
    }
 }