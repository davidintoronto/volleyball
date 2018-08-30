using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reservation
{
    public partial class Activity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Application[Constants.RESERVATION] = DataAccess.LoadReservation();
            String gameId = this.Request.Params[Constants.GAME_ID];

            if (gameId == null && Session[Constants.GAME_ID] != null)
            {
                gameId = Session[Constants.GAME_ID].ToString();
            }
            Game selectedGame = Reservations.FindGameById(gameId);
            if (selectedGame == null)
            {
                GameInfoTable.Caption = "The Game/Activity No Longer Exists!!!";
               // this.GameInfoPanel.Visible = false;
                this.DropinPanel.Visible = false;
                this.MessageTextTable.Visible = false;
                return;
            }
            Session[Constants.GAME_ID] = selectedGame.Id;
            //Fill message board
            if (!String.IsNullOrEmpty(selectedGame.Message))
            {
                this.MessageTextTable.Visible = true;
                TableCell messageCell = new TableCell();
                messageCell.Text = selectedGame.Message;
                TableRow messageRow = new TableRow();
                messageRow.Cells.Add(messageCell);
                this.MessageTextTable.Rows.Add(messageRow);
            }
            else
            {
                this.MessageTextTable.Visible = false;
            }
            //Check if there is dropin spots available for the players in waiting list
            while (selectedGame.MaxPlayers > selectedGame.Players.Count && selectedGame.WaitingListIds.Count > 0)
            {
                Reservations.AssignASpotToWaitingList(selectedGame);
            }
            //Fill game information
            FillGameInfoTable(selectedGame);

            //Fill dropin table
            FillDropinTable(selectedGame);
            SetConfirmButtonHandlder();
            //           
            if (String.IsNullOrEmpty(selectedGame.Title.Trim()))
            {
                Page.Title = Reservations.Title;
                ((Label)Master.FindControl("TitleLabel")).Text = Reservations.Title;
            }
            else
            {
                Page.Title = selectedGame.Title;
                ((Label)Master.FindControl("TitleLabel")).Text = selectedGame.Title;
            }

        }

        private void FillGameInfoTable(Game game)
        {
            //Date time
            this.GameInfoTable.Caption = "";
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.HorizontalAlign = HorizontalAlign.Left;
            cell.Text = "Date/Time";
            row.Cells.Add(cell);
            //this.GameInfoTable.Rows.Add(row);
           // row = new TableRow();
            cell = new TableCell();
            //cell.BackColor = this.GameInfoTable.BorderColor;
            cell.ForeColor = this.MessageTextTable.ForeColor;
            cell.HorizontalAlign = HorizontalAlign.Right;
            cell.Text = game.Time + " " + game.Date.ToString("ddd, MMM d, yyyy");
            row.Cells.Add(cell);
            this.GameInfoTable.Rows.Add(row);
             //Location
            row = new TableRow();
            cell = new TableCell();
            cell.HorizontalAlign = HorizontalAlign.Left;
            cell.Text = "Location";
            cell.ColumnSpan = 2;
            row.Cells.Add(cell);
            this.GameInfoTable.Rows.Add(row);
            row = new TableRow();
            cell = new TableCell();
            cell.ColumnSpan = 2;
            // cell.BackColor = this.GameInfoTable.BorderColor;
            cell.ForeColor = this.MessageTextTable.ForeColor;
            cell.HorizontalAlign = HorizontalAlign.Right;
             Label location = new Label();
             location.Text = game.Location;// +" - Map ";
            cell.Controls.Add(location);
             ImageButton mapBtn = new ImageButton();
            mapBtn.ImageUrl = "~/Icons/Map.png";
            mapBtn.Click += new ImageClickEventHandler(Map_Click);
             cell.Controls.Add(mapBtn);
            row.Cells.Add(cell);
           this.GameInfoTable.Rows.Add(row);
            //Total/Max players
            row = new TableRow();
            cell = new TableCell();
            cell.HorizontalAlign = HorizontalAlign.Left;
            cell.Text = (game.MaxPlayers > 0) ? "Total/Max" : "Total";
            row.Cells.Add(cell);
            //row = new TableRow();
            cell = new TableCell();
            //cell.BackColor = this.GameInfoTable.BorderColor;
            cell.ForeColor = this.MessageTextTable.ForeColor;
            cell.HorizontalAlign = HorizontalAlign.Right;
            cell.Text = (game.MaxPlayers > 0) ? game.Players.Count.ToString() + " / " + game.MaxPlayers : game.Players.Count.ToString();
            row.Cells.Add(cell);
            this.GameInfoTable.Rows.Add(row);
        }

        protected void Map_Click(object sender, EventArgs e)
        {
            Game selectedGame = Reservations.FindGameById(GameId);
            Response.Redirect("https://www.google.com/maps/place/" + selectedGame.Location);
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


        private void FillDropinTable(Game game)
        {
            IEnumerable<Player> query = Reservations.Players;//.OrderBy(dropin => dropin.Name);
            foreach (String dropin in game.Players)
            {
                TableRow row = CreateDropinTableRow(Reservations.FindPlayerById(dropin), true);
                this.DropinTable.Rows.Add(row);
                this.DropinTable.Visible = true;
                if (DropinTable.Rows.Count % 2 == 1)
                {
                    row.BackColor = DropinTable.BorderColor;
                }
            }
            foreach (Player dropin in query)
            {
                if (!game.WaitingListIds.Contains(dropin.Id) && Reservations.SharePlayers)// if (dropinSpotAvailable)
                {
                    TableRow row = CreateDropinTableRow(dropin, false);
                    this.DropinCandidateTable.Rows.Add(row);
                    if (DropinCandidateTable.Rows.Count % 2 == 1)
                    {
                        row.BackColor = DropinCandidateTable.BorderColor;
                    }
                }
            }

            foreach (String id in game.WaitingListIds)
            {
                TableRow row = CreateDropinTableRow(Reservations.FindPlayerById(id), true);
                this.DropinWaitingTable.Rows.Add(row);
                this.DropinWaitingTable.Visible = true;
            }

            TableRow addRow = new TableRow();
            TableCell addNameCell = new TableCell();
            // TextBox nameTb = new TextBox();
            //nameTb.ID = "DropinNameTb";
            this.DropinNameTb.Visible = true;
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
            if (!Reservations.SharePlayers)
            {
                this.DropinCandidateTable.Caption = "Enter your name";
            }
        }

        private Reservation Reservations
        {
            get
            {
                return (Reservation)Application[Constants.RESERVATION];

            }
            set { }
        }
        private String GameId
        {
            get
            {
                return Session[Constants.GAME_ID].ToString();

            }
            set { }
        }


        protected void CancelConfirm_Click(object sender, EventArgs e)
        {
            String userId = Session[Constants.CURRENT_USER_ID].ToString();
            Game game = Reservations.FindGameById(GameId);
            if (game.Players.Contains(userId))
            {
                game.Players.Remove(userId);
                if (!String.IsNullOrEmpty(game.WechatName))
                {
                    String message = "你偷偷的取消了报名参加(" + game.Title + ")。目前的报名总人数: " + game.Players.Count;
                    if (game.MaxPlayers > 0) message = message + "，还有" + (game.MaxPlayers - game.Players.Count) + "个空位";
                    WechatMessage wechatMessage = new WechatMessage(game.WechatName, Reservations.FindPlayerById(userId).Name, message);
                    Reservations.WechatMessages.Add(wechatMessage);
                }
                Reservations.AssignASpotToWaitingList(game);
            }
            if (!Reservations.SharePlayers)
            {
                Reservations.DeletePlayer(userId);
            }
            DataAccess.Save(Reservations);
            Response.Redirect(Request.RawUrl);
            //this.ConfirmPopup.Hide();
        }


        protected void AddNewDropin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(DropinNameTb.Text))
            {
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            TextBox nameTb = DropinNameTb;//(TextBox)FindControl("DropinNameTb");
            if (Reservations.IsPlayerReserved(Reservations.FindGameById(GameId), nameTb.Text))
            {
                ShowMessage("Warning !!! " + nameTb.Text + " has already reserved.");
                return;
            }
            Player nmb = Reservations.FindPlayerByName(nameTb.Text);
            if (nmb != null)
            {
                Session[Constants.CURRENT_USER_ID] = nmb.Id;
                Session[Constants.ACTION_TYPE] = Constants.ACTION_DROPIN_NEW;
                Session[Constants.CONTROL] = sender;
            }
            else
            {
                nmb = new Player(nameTb.Text.Trim(), null, false);
                Reservations.Players.Add(nmb);
                DataAccess.Save(Reservations);
                //ShowMessage("New drop-in palyer is added. Your temporary passcode is \"" + nmb.Passcode + "\".");
            }
            lbtn.ID = nmb.Id;
            AddBackDropin_Click(sender, e);
            //TableRow row = CreateDropinTableRow(dropin);
            // this.DropinTable.Rows.AddAt(this.DropinTable.Rows.Count -1, row);
        }

        private bool checkDropinSpotAvailable(Game game)
        {
            if (game.MaxPlayers > 0 && game.Players.Count >= game.MaxPlayers)
            {
                return false;
            }
            return true;
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
            if (dropin.Marked)
            {
                // Image image = new Image();
                // image.ImageUrl = "~/Icons/Colorball.png";
                // nameCell.Controls.Add(image);
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
                imageBtn.Click += new ImageClickEventHandler(RemoveDropin_Click);
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


        protected void RemoveDropin_Click(object sender, EventArgs e)
        {
             ImageButton lbtn = (ImageButton)sender;
            Session[Constants.CURRENT_USER_ID] = lbtn.ID;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_DROPIN_REMOVE;
            Session[Constants.CONTROL] = sender;
             Game game = Reservations.FindGameById(GameId);
            if (game.Players.Contains(lbtn.ID) || game.WaitingListIds.Contains(lbtn.ID))
            {
                ShowPopupModal("Are you sure to cancel?");
            }

        }
        protected void Username_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            String id = lbtn.ID.Split(',')[0];
            Session[Constants.CURRENT_USER_ID] = id;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_DETAIL;
            Session[Constants.CONTROL] = sender;
            Response.Redirect("Detail.aspx?id=" + id);
        }

        protected void Message_Click(object sender, EventArgs e)
        {
            this.PopupModal.Hide();
            Response.Redirect(Request.RawUrl);

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


        protected void SetConfirmButtonHandlder()
        {
            if (Session[Constants.ACTION_TYPE] != null)
            {
                switch (Session[Constants.ACTION_TYPE].ToString())
                {
                    case "Drop-inRemove":
                        this.ConfirmImageButton.Click += DropinCancelConfirm_Click;
                        break;
                    case "AddWaitingList":
                        this.ConfirmImageButton.Click += AddWaitingListConfirm_Click;
                        break;
                    case "Drop-inAdd":
                        this.ConfirmImageButton.Click += ContinueAddDropin_Click;
                        break;
                }
            }
        }
        protected void DropinCancelConfirm_Click(object sender, ImageClickEventArgs e)
        {
            String userId = Session[Constants.CURRENT_USER_ID].ToString();
            Game game = Reservations.FindGameById(GameId);
            Player player = Reservations.FindPlayerById(userId);
            if (player.FeeIds.Count > 0)
            {
                ShowMessage("This person paid for this activtiy, so cannot be cancelled");
                return;
            }
            if (game.Players.Contains(userId))
            {
                game.Players.Remove(userId);
                String message = "你偷偷的取消了报名参加(" + game.Title + ")。目前的报名总人数: " + game.Players.Count;
                if (game.MaxPlayers > 0) message = message + "，还有" + (game.MaxPlayers - game.Players.Count) + "个空位";
                WechatMessage wechatMessage = new WechatMessage(game.WechatName, Reservations.FindPlayerById(userId).Name, message);
                Reservations.WechatMessages.Add(wechatMessage);
                Reservations.AssignASpotToWaitingList(game);
            }
            else
            {
                game.WaitingListIds.Remove(userId);
            }
            if (!Reservations.SharePlayers)
            {
                //Reservations.DeletePlayer(userId);
            }
           DataAccess.Save(Reservations);
            this.PopupModal.Hide();
            Response.Redirect("Activity.aspx");
        }

        protected void AddWaitingListConfirm_Click(object sender, ImageClickEventArgs e)
        {
            String id = Session[Constants.CURRENT_USER_ID].ToString();
            Game game = Reservations.FindGameById(GameId);
            if (!game.WaitingListIds.Contains(id))
            {

                game.WaitingListIds.Add(id);
                 DataAccess.Save(Reservations);
            }
            this.PopupModal.Hide();
            Response.Redirect(Request.RawUrl);
        }

        protected void AddBackDropin_Click(object sender, EventArgs e)
        {
            ImageButton lbtn = (ImageButton)sender;
            Session[Constants.CURRENT_USER_ID] = lbtn.ID;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_DROPIN_ADD;
            Session[Constants.CONTROL] = sender;

            ShowPopupModal("Are you sure to reserve?");
        }

        private void ContinueAddDropin_Click(object sender, EventArgs e)
        {
            String playerId = Session[Constants.CURRENT_USER_ID].ToString();
            Game game = Reservations.FindGameById(GameId);
            if (!this.checkDropinSpotAvailable(game))
            {
                Session[Constants.ACTION_TYPE] = "AddWaitingList";
                ShowPopupModal("Sorry, But all spots are already filled up. Would you like to put on waiting list?");
                return;
            }
            if (!game.Players.Contains(playerId))
            {
                game.Players.Add(playerId);
                if (!String.IsNullOrEmpty(game.WechatName))
                {
                    String message = "你悄悄地给自己报了名参加" + game.Title + "。目前的报名总人数: " + game.Players.Count;
                    if (game.MaxPlayers > 0) message = message + "，还有" + (game.MaxPlayers - game.Players.Count) + "个空位";
                    WechatMessage wechatMessage = new WechatMessage(game.WechatName, Reservations.FindPlayerById(playerId).Name, message);
                    Reservations.WechatMessages.Add(wechatMessage);
                }
            }
            DataAccess.Save(Reservations);
            this.PopupModal.Hide();
            Response.Redirect("Activity.aspx");
        }
    }
}