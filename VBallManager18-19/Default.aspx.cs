﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

namespace VballManager
{
    public partial class Default : BasePage
    {
        private bool lockReservation = false;
        private String appLockedMessage = "Reservation system is locked at this moment, please contact admin";
 
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PopupModal.Hide();
            if (Manager.CookieAuthRequired && Request.Cookies[Constants.PRIMARY_USER] == null)
            {
                Response.Redirect(Constants.REQUEST_REGISTER_LINK_PAGE);
                return;
            }
            Player currentUser = Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]);
            if (currentUser == null || !currentUser.IsActive)
            {
                ShowMessage("Sorry, but your device is no longer linked to any account, Please contact admin for advice");
                return;
            }
            Session[Constants.CURRENT_USER] = currentUser;
             String operatorId = Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID];

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
                Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
                return;
            }
            //Check to see if user is quilified to view current pool
            if (!Manager.ActionPermitted(Actions.View_All_Pools, currentUser.Role) && !CurrentPool.Members.Exists(currentUser.Id) && !CurrentPool.Dropins.Exists(currentUser.Id))
            {
                Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
                return;
            }
            // 
            DateTime gameDate = Manager.EastDateTimeToday;
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
            this.lockReservation = Validation.IsReservationLocked(ComingGameDate, Manager);
            //Check if there is dropin spots available for the players on waiting list
            Game comingGame = CurrentPool.FindGameByDate(ComingGameDate);
            while (!this.lockReservation && Validation.IsSpotAvailable(CurrentPool, ComingGameDate) && comingGame.WaitingList.Count > 0)
            {
                AssignDropinSpotToWaiting(CurrentPool, comingGame);
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
        }

        #region Navigation
        private void FillNavTable()
        {
            if (!Manager.ActionPermitted(Actions.Admin_Management, CurrentUser.Role))
            {
                return;
            }
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
                if ((Manager.ActionPermitted(Actions.View_Past_Games, CurrentUser.Role) || game.Date >= Manager.EastDateTimeToday) && game.Date < ComingGameDate)
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

        #endregion

        #region Fill 
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
            IEnumerable<Player> playerQuery = OrderMembersByStats(pool, date);
            bool spotsFilledup = !Validation.IsSpotAvailable(pool, date);
            bool alterbackcolor = false;
            foreach (Player player in playerQuery)
            {
                //If this player is on the waiting list, don't list it in member section
                Game comingGame = pool.FindGameByDate(date);
                if (comingGame.WaitingList.Exists(player.Id)) continue;
                //
                Member member = pool.Members.FindByPlayerId(player.Id);
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
                LinkButton nameLink = new LinkButton();
                nameLink.Text = player.Name;
                nameLink.Font.Bold = true;
                nameLink.Font.Size = new FontUnit(Constants.LINKBUTTON_FONTSIZE);
                nameLink.ID = player.Id + ",MEMEBER";
                this.MemberTable.Rows.Add(row);
                nameCell.Controls.Add(nameLink);
                //Statistics 
                Label stats = new Label();
                stats.Font.Size = new FontUnit(Constants.STATS_FONTSIZE);
                stats.ForeColor = System.Drawing.Color.OrangeRed;
                stats.Text = "   " + player.TotalPlayedCount.ToString();               
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
                imageBtn.ID = player.Id;
                //If current user is not permit to reserve for this player, disable the image btn
                if (IsPermitted(Actions.Reserve_Pool, player))
                {
                    nameLink.Click += new EventHandler(Username_Click);
                }
                else
                {
                    imageBtn.Enabled = false;
                }
                Game game = pool.FindGameByDate(date);
                Attendee attdenee = game.Members.FindByPlayerId(player.Id);
                if (attdenee.Status == InOutNoshow.Out)
                {
                    imageBtn.ImageUrl = "~/Icons/Out.png";
                    imageBtn.Click += new ImageClickEventHandler(Reserve_Click);
                }
                else if (attdenee.Status == InOutNoshow.In)
                {
                   imageBtn.ImageUrl = "~/Icons/In.png";
                    imageBtn.Click += new ImageClickEventHandler(Cancel_Click);
                }
                else//No show
                    {
                        imageBtn.ImageUrl = "~/Icons/noShow.png";
                    imageBtn.Click += new ImageClickEventHandler(Cancel_Click);
                }
                //imageBtn.Click += new ImageClickEventHandler(MemberChangeAttendance_Click);
                imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
                imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
                //  imageBtn.CssClass = "imageBtn";
                statusCell.Controls.Add(imageBtn);
                row.Cells.Add(statusCell);
            }
        }

        private void FillDropinTable(Pool pool, DateTime date)
        {
            //Calcuate stats for dropins
            List<Player> players = CalculateDropinStats();
            Game game = pool.FindGameByDate(date);
            bool dropinSpotAvailable = Validation.IsSpotAvailable(pool, date);
            foreach (Player player in players)
            {
                Attendee attdenee = game.Dropins.FindByPlayerId(player.Id);
                if (attdenee.Status != InOutNoshow.Out)
                {
                    TableRow row = CreateDropinTableRow(player, attdenee, game.WaitingList.Exists(player.Id));
                    this.DropinTable.Rows.Add(row);
                    this.DropinTable.Visible = true;
                }
                else if (!game.WaitingList.Exists(player.Id))// if (dropinSpotAvailable)
                {
                    Dropin dropin = CurrentPool.Dropins.FindByPlayerId(player.Id);
                    if (dropin != null)
                    {

                        TableRow row = CreateDropinTableRow(player, attdenee, game.WaitingList.Exists(player.Id));
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
                TableRow row = CreateDropinTableRow(player, game.Dropins.FindByPlayerId(player.Id), game.WaitingList.Exists(player.Id));
                this.DropinWaitingTable.Rows.Add(row);
                this.DropinWaitingTable.Visible = true;
            }

            if (Manager.ActionPermitted(Actions.Add_New_Player, CurrentUser.Role))
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

        }

        private TableRow CreateDropinTableRow(Player player, Attendee attendee, bool isWaiting)
        {
            TableRow row = new TableRow();
            TableCell nameCell = new TableCell();
            LinkButton nameLink = new LinkButton();
            nameLink.Text = player.Name;
            nameLink.Font.Bold = true;
            nameLink.Font.Size = new FontUnit(Constants.LINKBUTTON_FONTSIZE);
            nameLink.ID = player.Id + ", DROPIN";
            nameCell.Controls.Add(nameLink);
            if (player.IsRegisterdMember)
            {
                Label stats = new Label();
                stats.Font.Size = new FontUnit(Constants.STATS_FONTSIZE);
                stats.ForeColor = System.Drawing.Color.OrangeRed;
                stats.Text = "   " + player.TotalPlayedCount.ToString();
                nameCell.Controls.Add(stats);
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
            row.Cells.Add(nameCell);
            TableCell actionCell = new TableCell();
            actionCell.HorizontalAlign = HorizontalAlign.Right;
            ImageButton imageBtn = new ImageButton();
            imageBtn.ID = player.Id;
            if (isWaiting)
            {
                imageBtn.ImageUrl = "~/Icons/Remove.png";
                imageBtn.Click += new ImageClickEventHandler(Cancel_Waiting_Click);
            }
            else if (attendee.Status==InOutNoshow.In)
            {
                imageBtn.ImageUrl = "~/Icons/Remove.png";
                 imageBtn.Click += new ImageClickEventHandler(Cancel_Click);
            }else if (attendee.Status == InOutNoshow.NoShow)
            {
                imageBtn.ImageUrl = "~/Icons/noShow.png";
                 imageBtn.Click += new ImageClickEventHandler(Cancel_Click);
            }else{
                imageBtn.ImageUrl = "~/Icons/Add.png";
                if (attendee.IsCoop) imageBtn.Click += new ImageClickEventHandler(Coop_Reserve_Click);
                else imageBtn.Click += new ImageClickEventHandler(Reserve_Click);          
            }
            
            imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
            actionCell.Controls.Add(imageBtn);
            row.Cells.Add(actionCell);
            if (IsPermitted(Actions.Reserve_Pool, player))
            {
                nameLink.Click += new EventHandler(Username_Click);

            }
            else
            {
                imageBtn.Enabled = false;
            }
            return row;
        }

        #endregion


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

 


 



        protected void Username_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            String id = lbtn.ID.Split(',')[0];
            Session[Constants.CURRENT_PLAYER_ID] = id;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_DETAIL;
            Session[Constants.CONTROL] = sender;
 
            Response.Redirect("Detail.aspx?id=" + id);
        }

 
        protected void SetConfirmButtonHandlder()
        {
            if (Session[Constants.ACTION_TYPE] != null)
            {
                  switch (Session[Constants.ACTION_TYPE].ToString())
                {
                    case Constants.ACTION_CANCEL:
                        this.ConfirmImageButton.Click += Cancel_Confirm_Click;
                        break;
                    case Constants.ACTION_ADD_WAITING_LIST:
                        this.ConfirmImageButton.Click += AddWaitingList_Confirm_Click;
                        break;
                    case Constants.ACTION_MOVE_RESERVATION:
                        this.ConfirmImageButton.Click += Move_Confirm_Click;
                        break;
                    case Constants.ACTION_NO_SHOW:
                        this.ConfirmImageButton.Click += No_Show_Confirm_Click;
                        break;
                    case Constants.ACTION_POWER_RESERVE:
                        this.ConfirmImageButton.Click += Reserve_Confirm_Click;
                        this.CloseImageBtn.Click += this.InquireAddingToWaitingList_Click;
                        break;
                }
            }
        }

 
         private IEnumerable<Player> OrderMembersByStats(Pool pool, DateTime gameDate)
        {
            Game game = pool.FindGameByDate(gameDate);
            List<Player> players = new List<Player>();
            foreach (Attendee attendee in game.Members.Items)
            {
                Player player = Manager.FindPlayerById(attendee.PlayerId);
                player.TotalPlayedCount = CalculatePlayedStats(player, CurrentPool.StatsType);
                players.Add(player);
            }
            return players.OrderByDescending(p => p.TotalPlayedCount);
        }

        private List<Player> CalculateDropinStats()
        {
            List<Player> players = new List<Player>();
            Game game = CurrentPool.FindGameByDate(ComingGameDate);
            foreach (Attendee attendee in game.Dropins.Items)
            {
                Player player = Manager.FindPlayerById(attendee.PlayerId);
                player.TotalPlayedCount = CalculatePlayedStats(player, CurrentPool.StatsType);
                players.Add(player);
            }
            return players.OrderBy(dropin => dropin.Name).ToList();

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
                        if (game.Date.Date < Manager.EastDateTimeToday.Date && (game.Members.Items.Exists(member=> member.PlayerId == player.Id && member.Status == InOutNoshow.In) || game.Dropins.Items.Exists(pickup=>pickup.PlayerId == player.Id && pickup.Status == InOutNoshow.In)))
                        {
                            playedCount++;
                        }
                    }
                }
            }
            return playedCount;
        } 

    }
}