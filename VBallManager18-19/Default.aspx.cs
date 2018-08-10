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
        private bool lockReservation = false;
        private String appLockedMessage = "Reservation app is locked at this moment, contact admin to make changes";
 
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PopupModal.Hide();
            if (Manager.CookieAuthRequired && Request.Cookies[Constants.PRIMARY_USER] == null)
            {
                Response.Redirect(Constants.REQUEST_REGISTER_LINK_PAGE);
                return;
            }
            Player currentUser = Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID]);
            if (currentUser == null || !currentUser.IsActive)
            {
                ShowMessage("Sorry, but your device is no longer linked to any account, Please contact admin for advice");
                return;
            }
            Session[Constants.CURRENT_USER] = currentUser;
             String operatorId = Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID];

            String poolId = this.Request.Params[Constants.POOL_ID];
            String poolName = this.Request.Params[Constants.POOL];

            if (poolId != null)
            {
                poolName = Manager.FindPoolById(poolId).Name;
                Session[Constants.POOL] = poolName;
                this.ToReadmeBtn.Visible = false;
            }
            else if (poolName != null)
            {
                Session[Constants.POOL] = poolName;
            }
            if (CurrentPool == null)
            {
                Response.Redirect("LinkDevice.aspx");
                return;
            }
            //Check to see if user is quilified to view current pool
            if (!Manager.ActionPermitted(Actions.View_All_Pools, currentUser.Role) && !CurrentPool.Members.Exists(attendee => attendee.Id == currentUser.Id) && !CurrentPool.Dropins.Exists(attendee => attendee.Id == currentUser.Id))
            {
                Response.Redirect("LinkDevice.aspx");
                return;
            }
            // 
            DateTime gameDate = DateTime.Today;
            String gameDateString = this.Request.Params[Constants.GAME_DATE];
            if (gameDateString != null)
            {
                gameDate = DateTime.Parse(gameDateString);
            }
            Application[Constants.DATA] = DataAccess.LoadReservation();
            Session[Constants.GAME_DATE] = null;
            List<Game> games = CurrentPool.Games;
            IEnumerable<Game> gameQuery = games.OrderBy(game => game.Date);

            foreach (Game game in gameQuery)
            {
                if (game.Date >= gameDate)
                {
                    Session[Constants.GAME_DATE] = game.Date;
                    break;
                }
            }
            //Fill message board
            if (!String.IsNullOrEmpty(CurrentPool.MessageBoard))
            {
                this.MessageTextTable.Visible = true;
                TableCell messageCell = new TableCell();
                messageCell.Text = CurrentPool.MessageBoard;
                TableRow messageRow = new TableRow();
                messageRow.Cells.Add(messageCell);
                this.MessageTextTable.Rows.Add(messageRow);
            }
            else
            {
                this.MessageTextTable.Visible = false;
            }
            if (Session[Constants.GAME_DATE] == null)
            {
                if (String.IsNullOrEmpty(CurrentPool.MessageBoard))
                {
                    GameInfoTable.Caption = "No Game Available !!!";
                }
                this.GameInfoPanel.Visible = false;
                this.MemberPanel.Visible = false;
                this.DropinPanel.Visible = false;
                return;
            }
            String unlock = this.Request.Params[Constants.UNLOCK];
            if (unlock == null && Validation.IsReservationLocked(ComingGameDate, Manager))
            {
                this.lockReservation = true;
            }
            //Check if there is dropin spots available for the players on waiting list
            Game comingGame = CurrentPool.FindGameByDate(ComingGameDate);
            while (!this.lockReservation && Validation.DropinSpotAvailable(CurrentPool, ComingGameDate) && comingGame.WaitingList.Count > 0)
            {
                AssignDropinSpotToWaiting(comingGame);
            }
            //Move coop reservation if current pool needs more players
            if (!this.lockReservation)
            {
                AutoReserveCoopPlayers();
            }
            //Fill game information
            FillGameInfoTable(CurrentPool, ComingGameDate);

            //Fill member table
            FillMemberTable(CurrentPool, ComingGameDate);

            //Fill dropin table
            FillDropinTable(CurrentPool, ComingGameDate);

            SetConfirmButtonHandlder();
            this.AddDropinImageBtn.Click += new ImageClickEventHandler(AddDropinImageBtn_Click);
            this.CreateNewPlayerBtn.Click += new ImageClickEventHandler(CreateNewPlayerBtn_Click);
            //this.PopupModal.Hide();
            //Show notification message to the user
            String notificationMessage = GetNotificationMessages();
            if (notificationMessage != null) this.ShowMessage(notificationMessage);
        }

        private DateTime EastDateTimeToday
        {
            get
            {
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName);
                return TimeZoneInfo.ConvertTime(DateTime.Today, easternZone);
            }
        }

        private DateTime EastDateTimeNow
        {
            get
            {
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName);
                return TimeZoneInfo.ConvertTime(DateTime.UtcNow, easternZone);
            }
        }

        private String GetNotificationMessages()
        {
            if (Request.Cookies[Constants.PRIMARY_USER] == null) return null;
            String userId = Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID];
            Player user = Manager.FindPlayerById(userId);
            String messages = null;
            foreach (Notification notificaiton in user.Notifications)
            {
                if (EastDateTimeToday <= notificaiton.Date) messages += "* " + notificaiton.Text + "\r\n";
            }
            user.Notifications.Clear();
            DataAccess.Save(Manager);
            return messages;
        }

        private bool SameDate(DateTime today, DateTime gameDate)
        {
            if (today.Year == gameDate.Year && today.Month == gameDate.Month && today.Day == gameDate.Day)
            {
                return true;
            }
            return false;
        }
        private void AutoReserveCoopPlayers()
        {
            foreach (Pool pool in Manager.Pools)
            {
                if (pool.AutoCoopReserve && pool.DayOfWeek == CurrentPool.DayOfWeek && EastDateTimeToday.Date == ComingGameDate.Date && EastDateTimeNow.Hour >= pool.ReservHourForCoop)
                {
                    Game game = pool.FindGameByDate(ComingGameDate);
                    //Check to see if number of reserved coop players already reaches maximum
                    while (Validation.DropinSpotAvailableForCoop(pool, ComingGameDate))
                    {
                        Dropin coopCandidate = null;
                        //Find the best candidate of coop
                        foreach (Dropin dropin in pool.Dropins)
                        {
                            if (dropin.IsCoop && !game.Pickups.Exists(dropin.Id) && PlayerAttendedLastWeekGame(dropin.Id))
                            {
                                //find it if it is member and reserved in another pool on same day
                                foreach (Pool otherPool in Manager.Pools)
                                {
                                    if (otherPool.Name != pool.Name && otherPool.DayOfWeek == pool.DayOfWeek)
                                    {
                                        //If number of attedning players in other pool is not enough, then stop moving coop
                                        if (otherPool.GetNumberOfAttendingMembers(ComingGameDate) + otherPool.GetNumberOfDropins(ComingGameDate) > otherPool.LessThanPayersForCoop)
                                        {
                                            //Is pool member and reserved for game day
                                            if (otherPool.Members.Exists(attendee => attendee.Id == dropin.Id) && !otherPool.FindGameByDate(ComingGameDate).Absences.Exists(dropin.Id))
                                            {
                                                if (coopCandidate == null || coopCandidate.LastCoopDate > dropin.LastCoopDate)
                                                {
                                                    coopCandidate = dropin;
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (coopCandidate == null)
                        {
                            break;
                        }
                        //Send the notification to the coop player
                        Player coopPlayer = Manager.FindPlayerById(coopCandidate.Id);
                        //coopPlayer.Notifications.Add(new Notification(DateTime.Today, "You are selected by system to play at Pool " + pool.Name + " on " + game.Date.ToShortDateString() + ". Your reservation has been moved to Pool " + pool.Name));
                        //Move reservation to current pool for coming game
                        MoveReservatioin(coopCandidate.Id, pool, null);
                    }
                }
            }
        }
        private bool PlayerAttendedLastWeekGame(String playerId)
        {
            foreach (Pool pool in Manager.Pools)
            {
                if (pool.DayOfWeek == CurrentPool.DayOfWeek)
                {
                    Game previousGame = null;
                    List<Game> games = pool.Games;
                    IEnumerable<Game> gameQuery = games.OrderBy(game => game.Date);

                    foreach (Game game in gameQuery)
                    {
                        if (game.Date < ComingGameDate)
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
                    if (pool.Members.Exists(attendee => attendee.Id == playerId) && !previousGame.Absences.Exists(playerId))
                    {
                        return true;
                    }
                    //Return true if the player is dropin and attend previous game
                    if (pool.Dropins.Exists(attendee => attendee.Id == playerId) && previousGame.Pickups.Exists(playerId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AssignDropinSpotToWaiting(Game comingGame)
        {
            String playerId = comingGame.WaitingList[0].PlayerId;// CurrentPool.AssignASpotToWaitingList(ComingGameDate);
            if (CurrentPool.Members.Exists(member => member.Id == playerId))
            {
                comingGame.Reserved.Add(new Identifier(playerId));
                comingGame.Absences.Remove(playerId);
            }
            else
            {
                CostReference reference = CreateDropinFee(playerId);
                Pickup pickup = new Pickup(playerId, reference);
                pickup.OperatorId = comingGame.WaitingList[0].OperatorId;
                comingGame.Pickups.Add(pickup);
            }
            Manager.AddReservationNotifyWechatMessage(playerId, null, Constants.RESERVED, CurrentPool, CurrentPool, ComingGameDate);
            LogHistory log = new LogHistory(DateTime.Now, comingGame.Date, "System", CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Reserve dropin", Manager.FindPlayerById(comingGame.WaitingList[0].OperatorId).Name);
            Manager.Logs.Add(log);
            comingGame.WaitingList.Remove(playerId);
        }

        private void FillNavTable()
        {
            TableRow navRow = new TableRow();
            //Previous
            TableCell prevCell = new TableCell();
            LinkButton lbtn = new LinkButton();
            lbtn.Text = "< Prev";
            lbtn.Font.Bold = true;
            lbtn.Font.Size = new FontUnit(Constants.LINKBUTTON_FONTSIZE);
            lbtn.ID = "PrevGame";
            lbtn.Click += new EventHandler(Navigate_Click);
            prevCell.Controls.Add(lbtn);
            prevCell.HorizontalAlign = HorizontalAlign.Left;
            Game prevGame = GetPreviousGame();
            if (prevGame == null)
            {
                lbtn.Enabled = false;
            }
            navRow.Cells.Add(prevCell);
            //GameInfo text
            TableCell gameinfoCell = new TableCell();
            gameinfoCell.Text = "Game Info";
            gameinfoCell.HorizontalAlign = HorizontalAlign.Center;
            navRow.Cells.Add(gameinfoCell);
            //Next
            TableCell nextCell = new TableCell();
            lbtn = new LinkButton();
            lbtn.Text = "Next >";
            lbtn.Font.Bold = true;
            lbtn.Font.Size = new FontUnit(Constants.LINKBUTTON_FONTSIZE);
            lbtn.ID = "NextGame";
            lbtn.Click += new EventHandler(Navigate_Click);
            Game nextGame = GetNextGame();
            if (nextGame == null)
            {
                lbtn.Enabled = false;
            }
            nextCell.Controls.Add(lbtn);
            nextCell.HorizontalAlign = HorizontalAlign.Right;
            navRow.Cells.Add(nextCell);
            this.NavTable.Rows.Add(navRow);
        }

        private Game GetPreviousGame()
        {
            List<Game> games = CurrentPool.Games;
            IEnumerable<Game> gameQuery = games.OrderByDescending(game => game.Date);
            foreach (Game game in gameQuery)
            {
                if ((Manager.ActionPermitted(Actions.View_Past_Games, CurrentUser.Role) || game.Date >= DateTime.Today) && game.Date < ComingGameDate)
                {
                    return game;
                }
            }
            return null;
        }
        private Game GetNextGame()
        {
            List<Game> games = CurrentPool.Games;
            IEnumerable<Game> gameQuery = games.OrderBy(game => game.Date);
            foreach (Game game in gameQuery)
            {
                if (game.Date > ComingGameDate)
                {
                    return game;
                }
            }
            return null;
        }


        protected void Navigate_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Game game = null;
            if (btn.ID == "PrevGame")
            {
                game = GetPreviousGame();
            }
            else if (btn.ID == "NextGame")
            {
                game = GetNextGame();
            }
            if (game != null)
            {
                Response.Redirect("Default.aspx?Pool=" + CurrentPool.Name + "&gameDate=" + game.Date);
            }
        }


        private void FillGameInfoTable(Pool pool, DateTime date)
        {
            FillNavTable();
            int memberPlayers = pool.GetNumberOfAttendingMembers(date);
            int dropinPlayers = pool.GetNumberOfDropins(date);
            int span = 7;
            FillGameInfoRow("Date", 1, ((DateTime)Session[Constants.GAME_DATE]).ToString("ddd, MMM d, yyyy"), span);
            FillGameInfoRow("Time", 1, pool.GameScheduleTime, span);
            FillGameInfoRow("Members", span, memberPlayers.ToString(), 1);
            TableRow dropinRow = new TableRow();
            TableCell dropinLabel = new TableCell();
            dropinLabel.Text = "Drop-ins";
            dropinLabel.HorizontalAlign = HorizontalAlign.Left;
            dropinRow.Cells.Add(dropinLabel);
            for (int i = 0; i < span - 1; i++)
            {
                TableCell cell = new TableCell();
                cell.Text = "&nbsp;";
                dropinRow.Cells.Add(cell);
            }
            TableCell dropinValue = new TableCell();
            //dropinValue.Text = dropinPlayers.ToString() + " / " + (pool.MaximumPlayerNumber - memberPlayers - dropinPlayers < 0 ? 0 : pool.MaximumPlayerNumber - memberPlayers - dropinPlayers);
            dropinValue.Text = dropinPlayers.ToString();
            dropinValue.HorizontalAlign = HorizontalAlign.Right;
            dropinRow.Cells.Add(dropinValue);
            this.GameInfoTable.Rows.Add(dropinRow);
            //FillGameInfoRow("Drop-ins",span, dropinPlayers.ToString(),1);
            if (pool.HasCap)
            {
                FillGameInfoRow("Total/Max", span, (memberPlayers + dropinPlayers).ToString() + " / " + pool.MaximumPlayerNumber, 1);
            }
            else
            {
                FillGameInfoRow("Total Players", span, (memberPlayers + dropinPlayers).ToString(), 1);
            }
        }



        private void FillGameInfoRow(String label, int labelColSpan, String value, int valueColSpan)
        {
            TableRow row = new TableRow();
            TableCell labelCell = new TableCell();
            labelCell.Text = label;
            labelCell.ColumnSpan = labelColSpan;
            row.Cells.Add(labelCell);
            TableCell valueCell = new TableCell();
            valueCell.Text = value;
            valueCell.ColumnSpan = valueColSpan;
            valueCell.HorizontalAlign = HorizontalAlign.Right;
            row.Cells.Add(valueCell);
            this.GameInfoTable.Rows.Add(row);
        }

        private void FillMemberTable(Pool pool, DateTime date)
        {
            IEnumerable<Player> playerQuery = OrderMembersByStats();
            bool spotsFilledup = !Validation.MemberSpotAvailable(pool, date);
            bool alterbackcolor = false;
            foreach (Player player in playerQuery)
            {
                Member member = pool.Members.Find(attendee => attendee.Id == player.Id);
                TableRow row = new TableRow();
                if (alterbackcolor)
                {
                    row.BackColor = MemberTable.BorderColor;
                }
                alterbackcolor = !alterbackcolor;
                if (player.Id == GetOperatorId())
                {
                    row.BackColor = System.Drawing.Color.CadetBlue;
                    row.BorderStyle = BorderStyle.Outset;
                }
                TableCell nameCell = new TableCell();
                LinkButton lbtn = new LinkButton();
                lbtn.Text = player.Name;
                lbtn.Font.Bold = true;
                lbtn.Font.Size = new FontUnit(Constants.LINKBUTTON_FONTSIZE);
                lbtn.ID = player.Id + ",MEMEBER";
                lbtn.Click += new EventHandler(Username_Click);
                nameCell.Controls.Add(lbtn);
                 Image stats = new Image();
                stats.ImageUrl = "~/Icons/number_" + player.TotalPlayedCount.ToString() + ".png";
                nameCell.Controls.Add(stats);
              if (player.Marked)
                {
                    Image image = new Image();
                    image.ImageUrl = "~/Icons/Colorball.png";
                    nameCell.Controls.Add(image);
                }
                foreach (Fee fee in player.Fees)
                {
                    if (!fee.IsPaid && fee.Amount > 0)
                    {
                        Image image = new Image();
                        image.ImageUrl = "~/Icons/dollar.png";
                        nameCell.Controls.Add(image);
                    }
                }
                row.Cells.Add(nameCell);
                TableCell statusCell = new TableCell();
                statusCell.HorizontalAlign = HorizontalAlign.Right;
                ImageButton imageBtn = new ImageButton();
                //If this player is on the waiting list, disable the action button
                Game comingGame = pool.FindGameByDate(date);
                if (comingGame.WaitingList.Exists(player.Id))
                {
                    imageBtn.ID = player.Id + "-InWaitingList";
                    imageBtn.Enabled = false;
                }
                else
                {
                    imageBtn.ID = player.Id;
                }
                //If current user is not permit to reserve for this player, disable the image btn
                if (!IsPermitted(Actions.Reserve_Pool, player))
                {
                    imageBtn.Enabled = false;
                }
                //If no spot available and current player unreserved, disable image btn
             //   if (!Manager.ActionPermitted(Actions.Power_Reserve, CurrentUser.Role) && spotsFilledup)
           //     {
            //        imageBtn.Enabled = pool.GetMemberAttendance(player.Id, date);
            //    }
                if (member.IsSuspended)
                {
                    lbtn.Enabled = false;
                    imageBtn.Enabled = false;
                }
                imageBtn.ImageUrl = pool.GetMemberAttendance(player.Id, date) ? "~/Icons/In.png" : "~/Icons/Out.png";
                imageBtn.Click += new ImageClickEventHandler(MemberChangeAttendance_Click);
                imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
                imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
                //  imageBtn.CssClass = "imageBtn";
                statusCell.Controls.Add(imageBtn);
                row.Cells.Add(statusCell);
                this.MemberTable.Rows.Add(row);
            }
        }

        private void FillDropinTable(Pool pool, DateTime date)
        {
            //Calcuate stats for dropins
            DoStatsForDropin();
            Game game = pool.FindGameByDate(date);
            bool dropinSpotAvailable = Validation.DropinSpotAvailable(pool, date);
            IEnumerable<Player> query = Manager.FindPlayersByAttendees(pool.Dropins).OrderBy(dropin => dropin.Name);
            foreach (Player player in query)
            {
                if (game.Pickups.Exists(player.Id))
                {
                    TableRow row = CreateDropinTableRow(player, true);
                    this.DropinTable.Rows.Add(row);
                    this.DropinTable.Visible = true;
                }
                else if (!game.WaitingList.Exists(player.Id))// if (dropinSpotAvailable)
                {
                    Dropin dropin = CurrentPool.Dropins.Find(attendee => attendee.Id == player.Id);
                    if (dropin != null && !dropin.IsSuspended)
                    {

                        TableRow row = CreateDropinTableRow(player, false);
                        this.DropinCandidateTable.Rows.Add(row);
                        if (DropinCandidateTable.Rows.Count % 2 == 1)
                        {
                            row.BackColor = DropinCandidateTable.BorderColor;
                        }
                        if (player.Id == GetOperatorId())
                        {
                            row.BorderStyle = BorderStyle.Double;
                            row.BackColor = System.Drawing.Color.DarkOrange;
                        }
                    }
                }
            }

            // List<Player> waitingList = Manager.FindPlayersByIds(game.WaitingList.getPlayerIds());
            foreach (Waiting waiting in game.WaitingList.Items)
            {
                Player player = Manager.FindPlayerById(waiting.PlayerId);
                TableRow row = CreateDropinTableRow(player, true);
                this.DropinWaitingTable.Rows.Add(row);
                this.DropinWaitingTable.Visible = true;
            }

            if (pool.AllowAddNewDropinName)
            {
                TableRow addRow = new TableRow();
                TableCell addNameCell = new TableCell();
                // TextBox nameTb = new TextBox();
                //nameTb.ID = "DropinNameTb";
                //this.DropinNameTb.Visible = true;
                addNameCell.Controls.Add(DropinNameTb);
                // addNameCell.Controls.Add(RequiredFieldValidator);
                addRow.Cells.Add(addNameCell);
                TableCell addCell = new TableCell();
                addCell.HorizontalAlign = HorizontalAlign.Right;
                ImageButton addImageBtn = new ImageButton();
                addImageBtn.ImageUrl = "~/Icons/Add.png";
                addImageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
                addImageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
                addImageBtn.Click += new ImageClickEventHandler(AddNewDropin_Click);
                addCell.Controls.Add(addImageBtn);
                addRow.Cells.Add(addCell);
                this.DropinCandidateTable.Rows.Add(addRow);
            }
            else
            {
                this.DropinNameTb.Visible = false;
            }
            /*else
            {
                this.DropinNameTb.Visible = false;
                TableCell messageCell = new TableCell();
                if (MemberSpotAvailable(date))
                {
                    messageCell.Text = "Sorry, But drop-in spots are filled up. However there are some member spots held until " + Reservations.MemberSpotReleaseHours + ":00 of game day, please check back later.";
                }
                else
                {
                    messageCell.Text = "Sorry, But all spots are filled up. Please check back later.";
                }
                TableRow messageRow = new TableRow();
                messageRow.Cells.Add(messageCell);
             
                this.DropinTable.Rows.Add(messageRow);
             }
          */

        }

        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }
        private Pool CurrentPool
        {
            get
            {
                String poolName = (String)Session[Constants.POOL];
                return Manager.FindPoolByName(poolName);
            }
            set { }
        }

        private Player CurrentUser
        {
            get
            {
                return (Player)Session[Constants.CURRENT_USER];
            }
            set { }
        }

        private DateTime ComingGameDate
        {
            get
            {
                return (DateTime)Session[Constants.GAME_DATE];

            }
            set { }
        }

        private bool IsPermitted(Actions action, Player player)
        {
            if (Manager.ActionPermitted(action, CurrentUser.Role) || CurrentUser.Id == player.Id || player.AuthorizedUsers.Contains(CurrentUser.Id))
            {
                return true;
            }
            return false;
        }

        private bool IsPermittedWithAlert(Actions action, Player player)
        {
            if (Manager.ActionPermitted(action, CurrentUser.Role) || CurrentUser.Id == player.Id || player.AuthorizedUsers.Contains(CurrentUser.Id))
            {
                return true;
            }
            ShowMessage("Sorry, but your device is not linked to user [" + player.Name + "], Please contact admin for advice");
            return false;
        }

        protected void MemberChangeAttendance_Click(object sender, EventArgs e)
        {
            if (lockReservation)
            {
                ShowMessage(appLockedMessage);
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            Session[Constants.CURRENT_PLAYER_ID] = lbtn.ID;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_MEMBER_ATTEND;
            Session[Constants.CONTROL] = sender;
            if (!IsAuthencated(lbtn.ID))
            {
                this.PasscodeAuthPopup.Show();
                return;
            }
            if (!CurrentPool.GetMemberAttendance(lbtn.ID, ComingGameDate) && !Validation.MemberSpotAvailable(CurrentPool, ComingGameDate))
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
            ContinueReserveMemberSpot(lbtn.ID);
        }

        private void ContinueReserveMemberSpot(String playerId)
        {
            //bool attending = Reservations.ReverseMemberAttendance(playerId, NextGameDate);
            Player player = Manager.FindPlayerById(playerId);
            if (!CurrentPool.GetMemberAttendance(playerId, ComingGameDate))
            {
                //Check to see if the player has dropin spot in another pool on same day
                foreach (Pool pool in Manager.Pools)
                {
                    if (pool.Name != CurrentPool.Name && pool.DayOfWeek == CurrentPool.DayOfWeek && pool.Dropins.Exists(dropin => dropin.Id == playerId))
                    {
                        if (pool.FindGameByDate(ComingGameDate).Pickups.Exists(playerId))
                        {
                            Session[Constants.ACTION_TYPE] = Constants.ACTION_MOVE_RESERVATION;
                            ShowPopupModal("You have aleady reserved a spot in pool " + pool.Name + " on the same day. Would you like to cancel that and reserve this one?");
                            return;
                        }
                        else if (pool.FindGameByDate(ComingGameDate).WaitingList.Exists(playerId))
                        {
                            Session[Constants.ACTION_TYPE] = Constants.ACTION_MOVE_RESERVATION;
                            ShowPopupModal("You are aleady on the waiting list of pool " + pool.Name + " on the same day. Would you like to cancel that and reserve this one?");
                            return;
                        }
                    }
                }
                //Remove absence
                Game game = CurrentPool.FindGameByDate(ComingGameDate);
                Absence absence = (Absence)game.Absences.FindByPlayerId(playerId);
                if (absence.TransferId != null)
                {
                    Transfer transfer = player.FindTransferById(absence.TransferId);
                    player.Transfers.Remove(transfer);
                }
                game.Absences.Remove(absence);
                //Add back to reserved list
                Identifier id = new Identifier();
                id.PlayerId = player.Id;
                game.Reserved.Add(id);
                //log and save
                Manager.AddReservationNotifyWechatMessage(playerId, CurrentUser.Id, Constants.RESERVED, CurrentPool, CurrentPool, ComingGameDate);
                LogHistory log = CreateLog(DateTime.Now, game.Date, GetUserIP(), CurrentPool.Name, player.Name, "Reserve member");
                Manager.Logs.Add(log);
                DataAccess.Save(Manager);
                Response.Redirect(Request.RawUrl);
                //ShowMessage("Your spot is reserved.");
            }
            else
            {
                ShowPopupModal("Are you sure to cancel?");
            }
        }

        private LogHistory CreateLog(DateTime date, DateTime gameDate, String userInfo, String poolName, String playerName, String type)
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null && Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID]) != null)
            {
                return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID]).Name);
            }
            return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, "Unknown");
        }

        private LogHistory CreateLog(DateTime date, DateTime gameDate, String userInfo, String poolName, String playerName, String type, String operater)
        {
            return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, operater);
        }

        private void ShowPopupModal(String message)
        {
            this.PopupLabel.Text = message;
            this.ConfirmImageButton.Visible = true;
            this.PopupModal.Show();
        }


        private void ShowMessage(String message)
        {
            this.PopupLabel.Text = message;
            this.ConfirmImageButton.Visible = false;
             this.PopupModal.Show();
        }

        public void MemberCancelConfirm_Click(object sender, ImageClickEventArgs e)
        {
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            Player player = Manager.FindPlayerById(playerId);
            Pool pool = CurrentPool;
            Game game = pool.FindGameByDate(ComingGameDate);
            if (!game.Absences.Exists(playerId))
            {
                Absence absence = new Absence(playerId);
                if (!Manager.ClubMemberMode)
                {
                    Transfer transfer = new Transfer(ComingGameDate);
                    player.Transfers.Add(transfer);
                    absence.TransferId = transfer.TransferId;
                }
                game.Absences.Add(absence);
                //Remove from reserved list
                game.Reserved.Remove(player.Id);
                //Log and save
                Manager.AddReservationNotifyWechatMessage(playerId, CurrentUser.Id, Constants.CANCELLED, CurrentPool, CurrentPool, ComingGameDate);
                LogHistory log = CreateLog(DateTime.Now, game.Date, GetUserIP(), pool.Name, Manager.FindPlayerById(playerId).Name, "Cancel member");
                Manager.Logs.Add(log);
                //Assgin a spot to the first one on waiting list
                if (!this.lockReservation && game.WaitingList.Count > 0 && Validation.DropinSpotAvailable(pool, ComingGameDate))
                {
                    AssignDropinSpotToWaiting(game);
                }
                DataAccess.Save(Manager);
            }
            this.PopupModal.Hide();
            Response.Redirect("Default.aspx");
        }


        protected void AddNewDropin_Click(object sender, EventArgs e)
        {
            this.AddDropinLb.Items.Clear();
            List<Player> players = new List<Player>();
            foreach (Player player in Manager.Players)
            {
                if (player.IsActive && !CurrentPool.Members.Exists(member => member.Id == player.Id) && !CurrentPool.Dropins.Exists(dropin => dropin.Id == player.Id))
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
            /*
            if (String.IsNullOrEmpty(DropinNameTb.Text))
            {
                return;
            }
            Pool pool = CurrentPool;
            ImageButton lbtn = (ImageButton)sender;
            Player player = Manager.FindOrCreateNewPlayer(DropinNameTb.Text);
            //Add to dropin into the pool
            pool.Dropins.Add(player.Id);
            Session[Constants.CURRENT_USER_ID] = player.Id;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_DROPIN_NEW;
            Session[Constants.CONTROL] = sender;
            if (!IsAuthencated(player.Id))
            {
                this.PasscodeAuthPopup.Show();
                return;
            }
            if (pool.IsDropinReserved(ComingGameDate, DropinNameTb.Text))
            {
                ShowMessage("Warning !!! " + DropinNameTb.Text + " has already reserved.");
                return;
            }
            lbtn.ID = player.Id;
            AddBackDropin_Click(sender, e);
            //TableRow row = CreateDropinTableRow(dropin);
            // this.DropinTable.Rows.AddAt(this.DropinTable.Rows.Count -1, row);
      */
        }

        void AddDropinImageBtn_Click(object sender, ImageClickEventArgs e)
        {
            if (this.AddDropinLb.SelectedIndex >= 0 && !CurrentPool.Dropins.Exists(dropin => dropin.Id == this.AddDropinLb.SelectedValue))
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
            String operatorId = Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID];
            Player player = Manager.FindPlayerByName(playerName);
            if (player != null)
            {
                if (!CurrentPool.Dropins.Exists(dropin => dropin.Id == player.Id))
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

        private TableRow CreateDropinTableRow(Player dropin, bool isDropin)
        {
            TableRow row = new TableRow();
            TableCell nameCell = new TableCell();
            LinkButton lbtn = new LinkButton();
            lbtn.Text = dropin.Name;
            lbtn.Font.Bold = true;
            lbtn.Font.Size = new FontUnit(Constants.LINKBUTTON_FONTSIZE);
            lbtn.ID = dropin.Id + ", DROPIN";
            lbtn.Click += new EventHandler(Username_Click);
            nameCell.Controls.Add(lbtn);
            if (dropin.IsRegisterdMember)
            {
                Image stats = new Image();
                stats.ImageUrl = "~/Icons/number_" + dropin.TotalPlayedCount.ToString() + ".png";
                nameCell.Controls.Add(stats);
            }
            foreach (Fee fee in dropin.Fees)
            {
                if (!fee.IsPaid && fee.Amount > 0)
                {
                    Image image = new Image();
                    image.ImageUrl = "~/Icons/dollar.png";
                    nameCell.Controls.Add(image);
                }
            }
            row.Cells.Add(nameCell);
            row.Cells.Add(nameCell);
            TableCell actionCell = new TableCell();
            actionCell.HorizontalAlign = HorizontalAlign.Right;
            ImageButton imageBtn = new ImageButton();
            imageBtn.ID = dropin.Id;
            imageBtn.ImageUrl = isDropin ? "~/Icons/Remove.png" : "~/Icons/Add.png";
            if (isDropin)
            {
                imageBtn.Click += new ImageClickEventHandler(CancelDropin_Click);
            }
            else
            {
                imageBtn.Click += new ImageClickEventHandler(AddBackDropin_Click);
            }
            imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
            actionCell.Controls.Add(imageBtn);
            row.Cells.Add(actionCell);
            return row;
        }

        protected void AddWaitingListConfirm_Click(object sender, ImageClickEventArgs e)
        {
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            Game game = CurrentPool.FindGameByDate(ComingGameDate);
            if (game.Pickups.Exists(playerId) || game.Reserved.Exists(playerId))
            {
                return;
            }
            if (!game.WaitingList.Exists(playerId))
            {
                //Check to see of the player has reserved a spot in another pool on same day
                foreach (Pool pool in Manager.Pools)
                {
                    if (pool.Name != CurrentPool.Name && pool.DayOfWeek == CurrentPool.DayOfWeek)
                    {
                        if (pool.Members.Exists(member => member.Id == playerId) && !pool.FindGameByDate(ComingGameDate).Absences.Exists(playerId))
                        {
                            ShowMessage("Sorry, But you have aleady reserved a spot in pool " + pool.Name + " on the same day. Cancel that spot before adding onto the waiting list");
                            return;
                        }
                        if (pool.Dropins.Exists(dropin => dropin.Id == playerId) && pool.FindGameByDate(ComingGameDate).Pickups.Exists(playerId))
                        {
                            ShowMessage("Sorry, But you have aleady reserved a dropin spot in pool " + pool.Name + " on the same day. Cancel that spot beforeadding onto the waiting list");
                            return;
                        }
                        else if (pool.FindGameByDate(ComingGameDate).WaitingList.Exists(playerId))
                        {
                            ShowMessage("Sorry, But you are on waiting list in pool " + pool.Name + " on the same day. Cancel that before adding onto the waiting list");
                            return;
                        }
                    }
                }
                Waiting waiting = new Waiting(playerId);
                waiting.OperatorId = GetOperatorId();
                game.WaitingList.Add(waiting);
                Manager.AddReservationNotifyWechatMessage(playerId, waiting.OperatorId, Constants.WAITING, CurrentPool, CurrentPool, ComingGameDate);
                LogHistory log = CreateLog(DateTime.Now, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Add to Waiting List");
                Manager.Logs.Add(log);
                DataAccess.Save(Manager);
            }
            this.PopupModal.Hide();
            Response.Redirect(Request.RawUrl);
        }

        protected void AddBackDropin_Click(object sender, EventArgs e)
        {
            if (lockReservation)
            {
                ShowMessage(appLockedMessage);
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            Session[Constants.CURRENT_PLAYER_ID] = lbtn.ID;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_DROPIN_ADD;
            Session[Constants.CONTROL] = sender;
            if (!IsAuthencated(lbtn.ID))
            {
                this.PasscodeAuthPopup.Show();
                return;
            }
            if (!IsPermittedWithAlert(Actions.Reserve_Pool, Manager.FindPlayerById(lbtn.ID)))
            {
                return;
            }
            Player player = Manager.FindPlayerById(lbtn.ID);
            if (Manager.ActionPermitted(Actions.Reserve_Pool, CurrentUser.Role) && !Validation.IsDropinSpotOpening(player.IsRegisterdMember, ComingGameDate, CurrentPool, Manager))
            {
                DateTime reserveDate = ComingGameDate.AddDays(-1 * CurrentPool.DaysToReserve4Member);
                if (!player.IsRegisterdMember)
                {
                    reserveDate = ComingGameDate.AddDays(-1 * CurrentPool.DaysToReserve);
                }
                ShowMessage("Sorry, But drop-in reservations cannot be made until " + Manager.DropinSpotOpeningHour + " on " + reserveDate.ToLongDateString() + ". Please check back later.");
                return;
            }
            ShowPopupModal("Are you sure to reserve?");
        }

        private void ContinueAddDropin_Click(object sender, EventArgs e)
        {
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            Actions action = Actions.Reserve_Pool;
            if (CurrentPool.Dropins.Find(attendee => attendee.Id == playerId).IsCoop)
                {
                    if (!Validation.DropinSpotAvailableForCoop(CurrentPool, ComingGameDate))
                    {
                        if (Manager.ActionPermitted(Actions.Power_Reserve, CurrentUser.Role))
                        {
                            action = Actions.Power_Reserve;
                        }
                        else
                        {
                            ShowMessage("Sorry, but Pool " + CurrentPool.Name + " has got enought players to start games. Please check back later.");
                            return;
                        }
                    }
                    if (EastDateTimeToday.Date < ComingGameDate.Date || EastDateTimeNow.Hour < CurrentPool.ReservHourForCoop)
                    {
                       if (!Manager.ActionPermitted(Actions.Power_Reserve, CurrentUser.Role))
                       {
                            ShowMessage("Sorry, but Pool " + CurrentPool.Name + " reservation starts at  " + CurrentPool.ReservHourForCoop + " O'clock on game day for co-op players. Check back later");
                            return;
                        }
                    }
                }
                else if (!Validation.DropinSpotAvailable(CurrentPool, ComingGameDate))
                {
                       if (Manager.ActionPermitted(Actions.Power_Reserve, CurrentUser.Role))
                        {
                            action = Actions.Power_Reserve;
                        }
                       else
                       {
                            Session[Constants.ACTION_TYPE] = Constants.ACTION_ADD_WAITING_LIST;
                            ShowPopupModal("Sorry, But all spots are already filled up. Would you like to put onto the waiting list?");
                    return;
                      }
                 }

                if (action==Actions.Power_Reserve)
                {
                    Session[Constants.ACTION_TYPE] = Constants.ACTION_POWER_RESERVE;
                    ShowPopupModal("All spots are already filled up. Would you like to reserve an EXTRA spot?");
                    return;
                }
                ContinueReservePickup(playerId);
        }

        private void ContinueReservePickup(String playerId)
        {
            Game game = CurrentPool.FindGameByDate(ComingGameDate);
            if (CurrentPool.Dropins.Exists(dropin => dropin.Id == playerId) && !game.Pickups.Exists(playerId))
            {
                //Check to see of the player has reserved a spot in another pool on same day
                foreach (Pool pool in Manager.Pools)
                {
                    if (pool.Name != CurrentPool.Name && pool.DayOfWeek == CurrentPool.DayOfWeek)
                    {
                        if (pool.Members.Exists(attendee => attendee.Id == playerId))
                        {
                            if (!pool.FindGameByDate(ComingGameDate).Absences.Exists(playerId))
                            {
                                Session[Constants.ACTION_TYPE] = Constants.ACTION_MOVE_RESERVATION;
                                ShowPopupModal("You have aleady reserved a spot in pool " + pool.Name + " on the same day. Would you like to cancel that and reserve this one?");
                                return;
                            }
                            else
                            {
                                break;
                            }

                        }
                        else if (pool.Dropins.Exists(attendee => attendee.Id == playerId))
                        {
                            if (pool.FindGameByDate(ComingGameDate).Pickups.Exists(playerId))
                            {
                                Session[Constants.ACTION_TYPE] = Constants.ACTION_MOVE_RESERVATION;
                                ShowPopupModal("You have aleady reserved a spot in pool " + pool.Name + " on the same day. Would you like to cancel that and reserve this one?");
                                return;
                            }
                            else if (pool.FindGameByDate(ComingGameDate).WaitingList.Exists(playerId))
                            {
                                Session[Constants.ACTION_TYPE] = Constants.ACTION_MOVE_RESERVATION;
                                ShowPopupModal("You are aleady on the waiting list of pool " + pool.Name + " on the same day. Would you like to cancel that and reserve this one?");
                                return;
                            }
                        }
                    }
                }
                //Create fee
                CostReference reference = CreateDropinFee(playerId);
                Pickup pickup = new Pickup(playerId, reference);
                pickup.OperatorId = GetOperatorId();
                game.Pickups.Add(pickup);
                Manager.AddReservationNotifyWechatMessage(playerId, pickup.OperatorId, Constants.RESERVED, CurrentPool, CurrentPool, ComingGameDate);
               LogHistory log = CreateLog(DateTime.Now, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Reserve dropin");
                 Manager.Logs.Add(log);
                //Update last dropin date for coop
                Dropin dropin = CurrentPool.Dropins.Find(attendee => attendee.Id == playerId);
                if (dropin.IsCoop)
                {
                    dropin.LastCoopDate = ComingGameDate;
                }
                DataAccess.Save(Manager);
                // ShowMessage("Congratus! Your spot is reserved!");
                this.PopupModal.Hide();
                Response.Redirect("Default.aspx");
            }
        }

        private String GetOperatorId()
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null && Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID]) != null)
            {
                return Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID]).Id;
            }
            return null;
        }

        private CostReference CreateDropinFee(String playerId)
        {
            Player player = Manager.FindPlayerById(playerId);
            //No cost if clust member mode and the player is the registered member
            if (Manager.ClubMemberMode && player.IsRegisterdMember)
            {
                return new CostReference(CostType.CLUB_MEMBER, null);
            }
            //No fee created and remove one transfer for the dropin player who is the member with cancelled spot in another pool on same day.
            foreach (Pool pool in Manager.Pools)
            {
                if (pool.Name != CurrentPool.Name && pool.DayOfWeek == CurrentPool.DayOfWeek && pool.Members.Exists(attendee => attendee.Id == playerId))
                {
                    player.RemoveTransferByGameDate(ComingGameDate);
                    return new CostReference(CostType.TRANSFER, null);
                }
            }
            //Check to see if the player has free of charge of dropin
            if (player.FreeDropin > 0)
            {
                player.FreeDropin--;
                /*Fee fee = new Fee(0);
                fee.Date = ComingGameDate;
                fee.FeeType = "Free -" + String.Format(Fee.FEETYPE_DROPIN, CurrentPool.Name);
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
                Transfer transfer = player.GetAvailableTransfer(ComingGameDate);
                if (transfer != null)
                {
                    transfer.IsUsed = true;
                    transfer.ApplyGameDate = ComingGameDate;
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
            fee.Date = ComingGameDate;
            fee.FeeType = FeeTypeEnum.Dropin.ToString();
            fee.FeeDesc = String.Format(Fee.FEETYPE_DROPIN, CurrentPool.Name);
            player.Fees.Add(fee);
            return new CostReference(CostType.FEE, fee.FeeId);
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
                if (pool.DayOfWeek == CurrentPool.DayOfWeek)
                {
                    foreach (Game game in pool.Games)
                    {
                        if (game.Pickups.Exists(player.Id))
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
            return amountPaid >= CurrentPool.MembershipFee;
        }

        private void CancelDropinFee(Pool targetPool, Pickup pickup)
        {
            if (pickup.CostReference == null) return;
            Player player = Manager.FindPlayerById(pickup.PlayerId);
            CostType type = pickup.CostReference.CostType;
            if (type == CostType.CLUB_MEMBER || type == CostType.REACH_MAX)
            {
                return;
            }
            if (type == CostType.TRANSFER)
            {
                //Find the original transfer used when reserving, and set it to unused
                Transfer transfer = player.FindTransferById(pickup.CostReference.ReferenceId);
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
                Fee fee = player.FindFeeById(pickup.CostReference.ReferenceId);
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

        private void CancelDropinFee(Pickup pickup)
        {
            CancelDropinFee(CurrentPool, pickup);
        }
        protected void CancelDropin_Click(object sender, EventArgs e)
        {
            if (lockReservation)
            {
                ShowMessage(appLockedMessage);
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            Session[Constants.CURRENT_PLAYER_ID] = lbtn.ID;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_DROPIN_REMOVE;
            Session[Constants.CONTROL] = sender;
            if (!IsAuthencated(lbtn.ID))
            {
                this.PasscodeAuthPopup.Show();
                return;
            }
            Player player = Manager.FindPlayerById(lbtn.ID);
            if (!IsPermittedWithAlert(Actions.Reserve_Pool, player))
            {
                return;
            }
            ShowPopupModal("Are you sure to cancel?");
            /*
            Game game = CurrentPool.FindGameByDate(ComingGameDate);
            if (game.Pickups.Exists(player.Id))
            {
                Pickup pickup = game.Pickups.FindByPlayerId(player.Id);
                if (pickup.OperatorId == operatorId)
                {
                    ShowPopupModal("Are you sure to cancel?");
                    return;
                }
            }
            if (game.WaitingList.Exists(player.Id))
            {
                Waiting waiting = game.WaitingList.FindByPlayerId(player.Id);
                if (waiting.OperatorId == operatorId)
                {
                    ShowPopupModal("Are you sure to cancel?");
                    return;
                }
            }
            ShowMessage("Sorry, but your device is not linked to user [" + Manager.FindPlayerById(player.Id).Name + "], Please contact admin for advice");
            */

        }

        protected void DropinCancelConfirm_Click(object sender, ImageClickEventArgs e)
        {
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            Game game = CurrentPool.FindGameByDate(ComingGameDate);
            if (game.Pickups.Exists(playerId))
            {
                Pickup pickup = (Pickup)game.Pickups.FindByPlayerId(playerId);
                game.Pickups.Remove(pickup);
                //Cancel dropin fee
                CancelDropinFee(pickup);
                Manager.AddReservationNotifyWechatMessage(playerId, pickup.OperatorId, Constants.CANCELLED, CurrentPool, CurrentPool, ComingGameDate);
                LogHistory log = CreateLog(DateTime.Now, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Cancel dropin");
                Manager.Logs.Add(log);
                //reset last dropin time for coop
                Dropin dropin = CurrentPool.Dropins.Find(attendee => attendee.Id == playerId);
                if (dropin.IsCoop) dropin.LastCoopDate = new DateTime();
                //Move first one in waiting list into dropin list
                if (!this.lockReservation && game.WaitingList.Count > 0 && Validation.DropinSpotAvailable(CurrentPool, ComingGameDate))
                {
                    AssignDropinSpotToWaiting(game);
                }
            }
            else
            {
                game.WaitingList.Remove(playerId);
                 LogHistory log = CreateLog(DateTime.Now, game.Date, GetUserIP(), CurrentPool.Name, Manager.FindPlayerById(playerId).Name, "Cancel waitinglist");
                Manager.Logs.Add(log);
            }
            DataAccess.Save(Manager);
            this.PopupModal.Hide();
            Response.Redirect("Default.aspx");
        }

        protected void Username_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            String id = lbtn.ID.Split(',')[0];
            Session[Constants.CURRENT_PLAYER_ID] = id;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_DETAIL;
            Session[Constants.CONTROL] = sender;
            if (!IsAuthencated(id))
            {
                this.PasscodeAuthPopup.Show();
                return;
            }

            Response.Redirect("Detail.aspx?id=" + id);
        }

        protected void MoveReservation_Click(object sender, ImageClickEventArgs e)
        {
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            MoveReservatioin(playerId, CurrentPool, CurrentUser.Id);
        }

        private void MoveReservatioin(String playerId, Pool destPool, String operatorId)
        {
            Player player = Manager.FindPlayerById(playerId);
            //Check to see of the player has reserved a spot in another pool on same day
            foreach (Pool pool in Manager.Pools)
            {
                if (pool.Name != destPool.Name && pool.DayOfWeek == destPool.DayOfWeek)
                {
                    //Is pool member
                    if (pool.Members.Exists(attendee => attendee.Id == playerId))
                    {
                        if (!pool.FindGameByDate(ComingGameDate).Absences.Exists(playerId))
                        {
                            Absence absence = new Absence(playerId);
                            if (!Manager.ClubMemberMode)
                            {
                                Transfer transfer = new Transfer(ComingGameDate);
                                player.Transfers.Add(transfer);
                                absence.TransferId = transfer.TransferId;
                            }
                            pool.FindGameByDate(ComingGameDate).Absences.Add(absence);
                            Manager.AddReservationNotifyWechatMessage(playerId, operatorId, Constants.MOVED, destPool, pool, ComingGameDate);
                            Manager.Logs.Add(CreateLog(DateTime.Now, ComingGameDate, GetUserIP(), pool.Name, Manager.FindPlayerById(playerId).Name, "Cancel member", "System"));
                            break;
                        }
                    }
                    //is pool dropin
                    else if (pool.Dropins.Exists(attendee => attendee.Id == playerId))
                    {
                        if (pool.FindGameByDate(ComingGameDate).Pickups.Exists(playerId))
                        {
                            Pickup pickup = (Pickup)pool.FindGameByDate(ComingGameDate).Pickups.FindByPlayerId(playerId);
                            pool.FindGameByDate(ComingGameDate).Pickups.Remove(playerId);
                            CancelDropinFee(pool, pickup);
                            Manager.AddReservationNotifyWechatMessage(playerId, operatorId, Constants.MOVED, destPool, pool, ComingGameDate);
                            Manager.Logs.Add(CreateLog(DateTime.Now, ComingGameDate, GetUserIP(), destPool.Name, Manager.FindPlayerById(playerId).Name, "Cancel dropin", "System"));
                            break;
                        }
                        else if (pool.FindGameByDate(ComingGameDate).WaitingList.Exists(playerId))
                        {
                            pool.FindGameByDate(ComingGameDate).WaitingList.Remove(playerId);
                            Manager.AddReservationNotifyWechatMessage(playerId, operatorId, Constants.MOVED, destPool, pool, ComingGameDate);
                            Manager.Logs.Add(CreateLog(DateTime.Now, ComingGameDate, GetUserIP(), destPool.Name, Manager.FindPlayerById(playerId).Name, "Canel waitinglist", "System"));
                            break;
                        }
                    }
                }
            }
            Game game = destPool.FindGameByDate(ComingGameDate);
            //Member add back
            if (destPool.Members.Exists(attendee => attendee.Id == playerId))
            {
                Absence absence = (Absence)game.Absences.FindByPlayerId(playerId);
                if (absence.TransferId != null)
                {
                    Transfer transfer = player.FindTransferById(absence.TransferId);
                    if (!transfer.IsUsed) player.Transfers.Remove(transfer);
                }
                game.Absences.Remove(playerId);
                Manager.AddReservationNotifyWechatMessage(playerId, playerId, Constants.RESERVED, destPool, destPool, ComingGameDate);
                Manager.Logs.Add(CreateLog(DateTime.Now, game.Date, GetUserIP(), destPool.Name, Manager.FindPlayerById(playerId).Name, "Reserve member", "System"));
            }
            //Dropin reserve
            else if (destPool.Dropins.Exists(attendee => attendee.Id == playerId))
            {
                CostReference reference = CreateDropinFee(playerId);
                Pickup pickup = new Pickup(playerId, reference);
                game.Pickups.Add(pickup);
                Manager.Logs.Add(CreateLog(DateTime.Now, game.Date, GetUserIP(), destPool.Name, Manager.FindPlayerById(playerId).Name, "Reserve dropin", "System"));
                Dropin dropin = destPool.Dropins.Find(attendee => attendee.Id == playerId);
                if (dropin.IsCoop) dropin.LastCoopDate = ComingGameDate;
            }

            DataAccess.Save(Manager);
            this.PopupModal.Hide();
            Response.Redirect("Default.aspx");
        }

        private bool IsAuthencated(String id)
        {
            Player member = Manager.FindPlayerById(id);
            if (!member.IsActive)
            {
                return false;
            }
            if (!Manager.PasscodeAuthen)
            {
                return true;
            }
            if (Session[Constants.PASSCODE] == null)
            {
                return false;
            }
            String passcode = Session[Constants.PASSCODE].ToString();

            if (passcode == Manager.SuperAdmin || member != null && member.Passcode == passcode)
            {
                return true;
            }
            else
            {
                Player user = Manager.FindPlayerById(id);
                if (user != null && user.Passcode == passcode)
                {
                    return true;
                }
            }
            return false;
        }

        protected void Message_Click(object sender, EventArgs e)
        {
            this.PopupModal.Hide();
            Response.Redirect(Request.RawUrl);

        }

        protected void AuthenLogin_Click(object sender, EventArgs e)
        {
            Session[Constants.PASSCODE] = PasscodeTb.Text;
            if (!IsAuthencated(Session[Constants.CURRENT_PLAYER_ID].ToString()))
            {
                this.PasscodeAuthPopup.Show();
                return;
            }
            Control control = (Control)Session[Constants.CONTROL];
            switch (Session[Constants.ACTION_TYPE].ToString())
            {
                case Constants.ACTION_MEMBER_ATTEND:
                    MemberChangeAttendance_Click(control, null);
                    break;
                case Constants.ACTION_DROPIN_NEW:
                    AddNewDropin_Click(control, null);
                    break;
                case Constants.ACTION_DROPIN_ADD:
                    AddBackDropin_Click(control, null);
                    break;
                case Constants.ACTION_DROPIN_REMOVE:
                    CancelDropin_Click(control, null);
                    break;
                case "Detail":
                    Username_Click(control, null);
                    break;
            }
        }
        protected void SetConfirmButtonHandlder()
        {
            if (Session[Constants.ACTION_TYPE] != null)
            {
                  switch (Session[Constants.ACTION_TYPE].ToString())
                {
                    case Constants.ACTION_MEMBER_ATTEND:
                        this.ConfirmImageButton.Click += MemberCancelConfirm_Click;
                        break;
                    case Constants.ACTION_DROPIN_REMOVE:
                        this.ConfirmImageButton.Click += DropinCancelConfirm_Click;
                        break;
                    case Constants.ACTION_ADD_WAITING_LIST:
                        this.ConfirmImageButton.Click += AddWaitingListConfirm_Click;
                        break;
                    case Constants.ACTION_DROPIN_ADD:
                        this.ConfirmImageButton.Click += ContinueAddDropin_Click;
                        break;
                    case Constants.ACTION_MOVE_RESERVATION:
                        this.ConfirmImageButton.Click += MoveReservation_Click;
                        break;
                    case Constants.ACTION_POWER_RESERVE:
                        this.ConfirmImageButton.Click += PowerReserve_Click;
                        this.CloseImageBtn.Click += this.InquireAddingToWaitingList_Click;
                        break;
                }
            }
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

        protected void ToReadmeBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Readme.aspx");
        }

        protected void PreRegisterBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("PreRegister.aspx?Pool=" + CurrentPool.Name);
        }
        private IEnumerable<Player> OrderMembersByStats()
        {
            foreach (Member member in CurrentPool.Members)
            {
                Player player = Manager.FindPlayerById(member.Id);
                player.TotalPlayedCount = CalculatePlayedStats(player, CurrentPool.StatsType);
            }
            IEnumerable<Player> players = Manager.FindPlayersByAttendees(CurrentPool.Members).OrderBy(member => member.Name);
            return players.OrderByDescending(p => p.TotalPlayedCount);
        }

        private void DoStatsForDropin()
        {
            foreach (Dropin dropin in CurrentPool.Dropins)
            {
                Player player = Manager.FindPlayerById(dropin.Id);
                player.TotalPlayedCount = CalculatePlayedStats(player, CurrentPool.StatsType); 
            }
        }

        private int CalculatePlayedStats(Player player, String statsType)
        {
            int playedCount = 0;
            if (statsType == StatsTypes.None.ToString())
                return playedCount;
            foreach (Pool pool in Manager.Pools)
            {
                if (statsType == StatsTypes.Week.ToString() || pool.DayOfWeek == CurrentPool.DayOfWeek)
                {
                    foreach (Game game in pool.Games)
                    {
                        if (game.Date.Date < DateTime.Today.Date && (game.Reserved.Exists(player.Id) && !game.NoShow.Exists(player.Id) || game.Pickups.Exists(player.Id)))
                        {
                            playedCount++;
                        }
                    }
                }
            }
            return playedCount;
        }

        protected void InquireAddingToWaitingList_Click(object sender, ImageClickEventArgs e)
        {
            if (Session[Constants.ACTION_TYPE].ToString() == Constants.ACTION_POWER_RESERVE)
            {
                Session[Constants.ACTION_TYPE] = Constants.ACTION_ADD_WAITING_LIST;
                ShowPopupModal("Would you like to put onto the waiting list?");
            }
        }
 
        protected void PowerReserve_Click(object sender, ImageClickEventArgs e)
        {
            String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            if (CurrentPool.Members.Exists(member => member.Id == playerId))
            {
                ContinueReserveMemberSpot(playerId);
            }
            else if (CurrentPool.Dropins.Exists(dropin => dropin.Id == playerId))
            {
                ContinueReservePickup(playerId);
            }
        }

    }
}