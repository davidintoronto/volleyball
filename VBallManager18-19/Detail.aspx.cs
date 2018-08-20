using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Manager.CookieAuthRequired && Request.Cookies[Constants.PRIMARY_USER] == null)
            {
                Response.Redirect(Constants.REQUEST_REGISTER_LINK_PAGE);
                return;
            }
            if (Request.Params["id"] == null)
            {
                Response.Redirect("Default.aspx");
                return; ;
            }
            String id = Request.Params["id"].ToString();
            Player player = Manager.FindPlayerById(id);
            bool mark = false;
            this.DetailTable.Caption = player.Name;
            if (!IsPostBack)
            {
                this.PasscodeTb.Text = player.Passcode;
                this.NameTb.Text = player.Name;
                mark = player.Marked;
            }
            TableRow row = new TableRow();
            TableCell labelCell = new TableCell();
            labelCell.Text = "Change Name";
            row.Cells.Add(labelCell);
            TableCell valueCell = new TableCell();
            valueCell.HorizontalAlign = HorizontalAlign.Right;
            // PasscodeTb.Font.Size = new FontUnit("1.2em");
            valueCell.Controls.Add(NameTb);
            PasscodeTb.Font.Size = FontUnit.XXLarge;
            row.Cells.Add(valueCell);
            TableCell saveCell = new TableCell();
            saveCell.HorizontalAlign = HorizontalAlign.Right;
            ImageButton imageBtn = new ImageButton();
            imageBtn.ID = id + "," + this.DetailTable.Caption + ",saveNameBtn";
            imageBtn.ImageUrl = "~/Icons/Save.png";
            imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Click += new ImageClickEventHandler(SaveName_Click);
            saveCell.Controls.Add(imageBtn);
            row.Cells.Add(saveCell);
            //this.DetailTable.Rows.Add(row);
            //
            row = new TableRow();
            labelCell = new TableCell();
            labelCell.Text = "Change Passcode";
            row.Cells.Add(labelCell);
            valueCell = new TableCell();
            valueCell.HorizontalAlign = HorizontalAlign.Right;
            // PasscodeTb.Font.Size = new FontUnit("1.2em");
            valueCell.Controls.Add(PasscodeTb);
            PasscodeTb.Font.Size = FontUnit.XXLarge;
            row.Cells.Add(valueCell);
            saveCell = new TableCell();
            saveCell.HorizontalAlign = HorizontalAlign.Right;
            imageBtn = new ImageButton();
            imageBtn.ID = id + "," + this.DetailTable.Caption + ",saveBtn";
            imageBtn.ImageUrl = "~/Icons/Save.png";
            imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Click += new ImageClickEventHandler(SavePasscode_Click);
            saveCell.Controls.Add(imageBtn);
            row.Cells.Add(saveCell);
            //this.DetailTable.Rows.Add(row);
            //Free dropin. Do not show it if it is club member mode
            if (!Manager.ClubMemberMode || !player.IsRegisterdMember)
            {
                row = new TableRow();
                labelCell = new TableCell();
                labelCell.Text = "Free Dropin";
                row.Cells.Add(labelCell);
                TableCell freeDropinCell = new TableCell();
                Image image = new Image();
                image.ImageUrl = "~/Icons/transfer.png";
                //  imageBtn.CssClass = "imageBtn";
                freeDropinCell.Controls.Add(image);
                row.Cells.Add(freeDropinCell);
                this.DetailTable.Rows.Add(row);
                valueCell = new TableCell();
                valueCell.HorizontalAlign = HorizontalAlign.Right;
                // PasscodeTb.Font.Size = new FontUnit("1.2em");
                valueCell.Text = player.FreeDropin.ToString();
                row.Cells.Add(valueCell);
            }
            //Show Transfer if club member mode is off
            if (!Manager.ClubMemberMode)
            {
                row = new TableRow();
                labelCell = new TableCell();
                labelCell.Text = "Availale Transfers";
                row.Cells.Add(labelCell);
                TableCell transferCell = new TableCell();
                Image image = new Image();
                image.ImageUrl = "~/Icons/transfer.png";
                //  imageBtn.CssClass = "imageBtn";
                transferCell.Controls.Add(image);
                row.Cells.Add(transferCell);
                valueCell = new TableCell();
                valueCell.HorizontalAlign = HorizontalAlign.Right;
                // PasscodeTb.Font.Size = new FontUnit("1.2em");
                valueCell.Text = player.AvailableTransferCount.ToString();
                row.Cells.Add(valueCell);
                this.DetailTable.Rows.Add(row);
                //Transfer used
                row = new TableRow();
                labelCell = new TableCell();
                labelCell.Text = "Transfers Used";
                row.Cells.Add(labelCell);
                transferCell = new TableCell();
                image = new Image();
                image.ImageUrl = "~/Icons/transfer.png";
                //  imageBtn.CssClass = "imageBtn";
                transferCell.Controls.Add(image);
                row.Cells.Add(transferCell);

                valueCell = new TableCell();
                valueCell.HorizontalAlign = HorizontalAlign.Right;
                // PasscodeTb.Font.Size = new FontUnit("1.2em");
                valueCell.Text = player.TransferUsed.ToString() + " / " + Manager.MaxTransfers;
                row.Cells.Add(valueCell);
                this.DetailTable.Rows.Add(row);
            }
            row = new TableRow();
            labelCell = new TableCell();
            labelCell.Text = "Mark";
            row.Cells.Add(labelCell);
            valueCell = new TableCell();
            valueCell.HorizontalAlign = HorizontalAlign.Right;
            // PasscodeTb.Font.Size = new FontUnit("1.2em");
            //valueCell.Controls.Add(PasscodeTb);
            //PasscodeTb.Font.Size = FontUnit.XXLarge;
            row.Cells.Add(valueCell);
            saveCell = new TableCell();
            saveCell.HorizontalAlign = HorizontalAlign.Right;
            imageBtn = new ImageButton();
            imageBtn.ID = id + "," + this.DetailTable.Caption + ",markBtn";
            if (mark)
            {
                imageBtn.ImageUrl = "~/Icons/ColorBall.png";
            }
            else
            {
                imageBtn.ImageUrl = "~/Icons/BlackBall.png";
            }
            imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Click += new ImageClickEventHandler(Mark_Click);
            saveCell.Controls.Add(imageBtn);
            row.Cells.Add(saveCell);
            //this.DetailTable.Rows.Add(row);
            FillFeeAndPaymentTable(player);
            if (CurrentPool.Members.Exists(attendee => attendee.PlayerId == player.Id))
            {
                FillGameTable(player);
            }
            else
            {
                FillDropinAttendedGameTable(player);
            }
        }

        protected void Message_Click(object sender, EventArgs e)
        {
            this.PopupModal.Hide();
            Response.Redirect(Request.RawUrl);

        }

        private void FillGameTable(Player member)
        {
            IEnumerable<Game> query = CurrentPool.Games.OrderBy(game => game.Date);
            DateTime comingGameDate = (DateTime)Session[Constants.GAME_DATE];
            if (comingGameDate == null)
            {
                return;
            }
             foreach (Game game in query)
            {
                if (!IsAdmin && game.Date <= comingGameDate)
                {
                    continue;
                }
                TableRow row = new TableRow();
                TableCell dateCell = new TableCell();
                dateCell.Text = game.Date.ToString("ddd, MMM d, yyyy");
                row.Cells.Add(dateCell);
                TableCell statusCell = new TableCell();
                statusCell.HorizontalAlign = HorizontalAlign.Right;
                ImageButton imageBtn = new ImageButton();
                imageBtn.ID = member.Id + "," + game.Date.ToShortDateString();
                imageBtn.ImageUrl = CurrentPool.GetMemberAttendance(member.Id, game.Date) ? "~/Icons/In.png" : "~/Icons/Out.png";
                imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
                imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
                imageBtn.Click += new ImageClickEventHandler(MemberChangeAttendance_Click);
                //  imageBtn.CssClass = "imageBtn";
                statusCell.Controls.Add(imageBtn);
                row.Cells.Add(statusCell);
                this.GameTable.Rows.Add(row);
            }
        }
        private bool IsAdmin
        {
            get
            {
                String operatorId = Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID];
                if (Manager.FindPlayerById(operatorId).Role >= (int)Roles.Admin)
                {
                    return true;
                }
                return false;
            }
        }

        private void FillDropinAttendedGameTable(Player dropin)
        {
            IEnumerable<Game> query = CurrentPool.Games.OrderBy(game => game.Date);
            DateTime comingGameDate = (DateTime)Session[Constants.GAME_DATE];
              if (!IsAdmin)
            {
                this.GameTable.Visible = false;
                return;
            }
            this.GameTable.Caption = "Games Played";
            foreach (Game game in query)
            {
                if (game.Date > DateTime.Today)
                {
                    break;
                }
                if (game.Pickups.Exists(dropin.Id))
                {
                    TableRow row = new TableRow();
                    TableCell dateCell = new TableCell();
                    dateCell.Text = game.Date.ToString("ddd, MMM d, yyyy");
                    row.Cells.Add(dateCell);
                    TableCell statusCell = new TableCell();
                    statusCell.HorizontalAlign = HorizontalAlign.Right;
                    Image imageBtn = new Image();
                    imageBtn.ID = dropin.Id + "," + game.Date.ToShortDateString();
                    imageBtn.ImageUrl = "~/Icons/In.png";
                    imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
                    imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
                    //  imageBtn.CssClass = "imageBtn";
                    statusCell.Controls.Add(imageBtn);
                    row.Cells.Add(statusCell);
                    this.GameTable.Rows.Add(row);
                }
            }
        }
        
        private void FillFeeAndPaymentTable(Player player)
        {
            this.FeeTable.Visible = false;
            IEnumerable<Fee> query = player.Fees.OrderBy(fee => fee.Date);
            decimal feeTotal = 0;
            foreach (Fee fee in query)
            {
                if (fee.Amount == 0)
                {
                    continue;
                }
                this.FeeTable.Visible = true;
                TableRow row = new TableRow();
                TableCell dateCell = new TableCell();
                dateCell.Text = fee.Date.ToString("MM/dd/yyyy");
                row.Cells.Add(dateCell); 
                TableCell typeCell = new TableCell();
                typeCell.Text = fee.FeeDesc;
                row.Cells.Add(typeCell);
                TableCell statusCell = new TableCell();
                statusCell.HorizontalAlign = HorizontalAlign.Right;
                Image image = new Image();
                image.ImageUrl =  "~/Icons/dollar.png";
                //  imageBtn.CssClass = "imageBtn";
                statusCell.Controls.Add(image);
                 row.Cells.Add(statusCell);
                TableCell dollarCell = new TableCell();
                dollarCell.HorizontalAlign = HorizontalAlign.Right;
                dollarCell.Text = fee.Amount.ToString();
                row.Cells.Add(dollarCell);
                if (fee.IsPaid)
                {
                    dollarCell.Text = fee.Amount.ToString();
                    this.PaymentTable.Rows.Add(row);
                }
                else
                {
                    this.FeeTable.Rows.Add(row);
                    feeTotal += fee.Amount;
                }
          }
            if (this.FeeTable.Visible && feeTotal > 0)
            {
                TableRow totalRow = new TableRow();
                TableCell textCell = new TableCell();
                textCell.Text = "Total";
                totalRow.Cells.Add(textCell);
                TableCell cell = new TableCell();
                cell.Text = "  ";
                totalRow.Cells.Add(cell);
                TableCell markCell = new TableCell();
                markCell.HorizontalAlign = HorizontalAlign.Right;
                Image dollarImage = new Image();
                dollarImage.ImageUrl = "~/Icons/dollar.png";
                //  imageBtn.CssClass = "imageBtn";
                markCell.Controls.Add(dollarImage);
                totalRow.Cells.Add(markCell);
                TableCell balanceCell = new TableCell();
                balanceCell.HorizontalAlign = HorizontalAlign.Right;
                balanceCell.Text = feeTotal.ToString();
                totalRow.Cells.Add(balanceCell);
                this.FeeTable.Rows.Add(totalRow);
                totalRow.BackColor = System.Drawing.Color.Cyan;
            }
            //Prepaid
            if (player.PrePaidBalance >= Manager.DropinFee)
            {
                this.FeeTable.Visible = true;
                this.FeeTable.Caption = "Pre-paid";
                TableRow totalRow = new TableRow();
                TableCell textCell = new TableCell();
                textCell.Text = "Balance";
                totalRow.Cells.Add(textCell);
                TableCell cell = new TableCell();
                cell.Text = "  ";
                totalRow.Cells.Add(cell);
                TableCell markCell = new TableCell();
                Image dollarImage = new Image();
                dollarImage.ImageUrl = "~/Icons/dollar.png";
                //  imageBtn.CssClass = "imageBtn";
                markCell.Controls.Add(dollarImage);
                totalRow.Cells.Add(markCell);
                TableCell balanceCell = new TableCell();
                balanceCell.HorizontalAlign = HorizontalAlign.Right;
                balanceCell.Text = player.PrePaidBalance.ToString();
                totalRow.Cells.Add(balanceCell);
                this.FeeTable.Rows.Add(totalRow);
                totalRow.BackColor = System.Drawing.Color.Cyan;

            }
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
        protected void SaveName_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton lbtn = (ImageButton)sender;
            String currentUserId = lbtn.ID.Split(',')[0];
            if (!IsAuthencated(currentUserId))
            {
                return;
            }
            Player member = Manager.FindPlayerById(currentUserId);
            if (member != null)
            {
                member.Name = NameTb.Text.Trim();
            }
            else
            {
                Player user = Manager.FindPlayerById(currentUserId);
                if (user != null)
                {
                    user.Name = NameTb.Text.Trim();
                }
            }
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        protected void Mark_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton lbtn = (ImageButton)sender;
            String currentUserId = lbtn.ID.Split(',')[0];
            if (!IsAuthencated(currentUserId))
            {
                return;
            }
            Player member = Manager.FindPlayerById(currentUserId);
            if (member != null)
            {
                member.Marked = !member.Marked;
                DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
           }
         }
        protected void SavePasscode_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton lbtn = (ImageButton)sender;
            String currentUserId = lbtn.ID.Split(',')[0];
            if (!IsAuthencated(currentUserId))
            {
                return;
            }
            Player player = Manager.FindPlayerById(currentUserId);
               player.Passcode = PasscodeTb.Text.Trim();
            DataAccess.Save(Manager);
            if (PasscodeTb.Text.Trim() != Manager.SuperAdmin)
            {
                Session[Constants.PASSCODE] = PasscodeTb.Text.Trim();
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void BackBtn_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Default.aspx?Pool=" + CurrentPool.Name);

        }

        protected void MemberChangeAttendance_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            String playerId = btn.ID.Split(',')[0];
            if (!IsAuthencated(playerId))
            {
                return;
            }
            DateTime date = DateTime.Parse(btn.ID.Split(',')[1]);
            Player player = Manager.FindPlayerById(playerId);
            Game game = CurrentPool.FindGameByDate(date);
            if (game.Absences.Exists(playerId))
            {
                Absence absence = (Absence)game.Absences.FindByPlayerId(playerId);
                if (absence.TransferId != null)
                {
                    Transfer transfer = player.FindTransferById(absence.TransferId);
                    player.Transfers.Remove(transfer);
                }
                //Add to reserved list
                game.Presences.Add(new Presence(playerId));
                game.Absences.Remove(absence);
            }
            else
            {
                //Check authorization
                if (!this.IsAdmin)
                {
                    if (Request.Cookies[Constants.PRIMARY_USER] == null)
                    {
                        ShowMessage("Sorry, but your device is not linked to the user " + player.Name + ", Please contact admin for advice");
                        return;
                    }

                    String operatorId = Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID];
                    if (operatorId != player.Id && !player.AuthorizedUsers.Contains(operatorId))
                    {
                        ShowMessage("Sorry, but you are not authorized to make the cancellation for " + player.Name + ", Please contact admin for advice");
                        return;
                    }
                }
                Absence absence = new Absence(playerId);
                if (!Manager.ClubMemberMode)
                {
                    Transfer transfer = new Transfer(game.Date);
                    player.Transfers.Add(transfer);
                    absence.TransferId = transfer.TransferId;
                }
                game.Absences.Add(absence);
                //Remove from reserved list
                game.Presences.Remove(playerId);
            }
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        private void ShowMessage(String message)
        {
            this.PopupLabel.Text = message;
            this.ConfirmImageButton.Visible = false;
            this.PopupModal.Show();
        }
        
        private bool IsAuthencated(String id)
        {
            Player player = Manager.FindPlayerById(id);
            if (!Manager.PasscodeAuthen)
            {
                return true;
            }
            if (Session[Constants.PASSCODE] == null)
            {
                return false;
            }
            String passcode = Session[Constants.PASSCODE].ToString();

            if (passcode == Manager.SuperAdmin || player != null && player.Passcode == passcode)
            {
                return true;
            }
             return false;
        }
     }
}