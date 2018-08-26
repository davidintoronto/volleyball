using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Detail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Manager.CookieAuthRequired && Request.Cookies[Constants.PRIMARY_USER] == null)
            {
                Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
                return;
            }
            if (Request.Params["id"] == null)
            {
                Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
                return;
            }
            String id = Request.Params["id"].ToString();
            Player player = Manager.FindPlayerById(id);
            if (!IsPermitted(Actions.View_Player_Details, player))
            {
                Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
                return;
            }
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
            //imageBtn.Click += new ImageClickEventHandler(SaveName_Click);
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
            //imageBtn.Click += new ImageClickEventHandler(SavePasscode_Click);
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
            //imageBtn.Click += new ImageClickEventHandler(Mark_Click);
            saveCell.Controls.Add(imageBtn);
            row.Cells.Add(saveCell);
            //this.DetailTable.Rows.Add(row);
            FillFeeAndPaymentTable(player);
            FillGameTable(player);
        }

        protected void Message_Click(object sender, EventArgs e)
        {
            this.PopupModal.Hide();
            Response.Redirect(Request.RawUrl);

        }

        private void FillGameTable(Player player)
        {
            IEnumerable<Game> query = CurrentPool.Games.OrderBy(game => game.Date);
            DateTime comingGameDate = DateTime.Today.AddYears(1);
            if (Session[Constants.GAME_DATE] != null)
            {
                comingGameDate = (DateTime)Session[Constants.GAME_DATE];
            }
             foreach (Game game in query)
            {
                 if (game.Members.Items.Exists(member => member.PlayerId == player.Id))
                {
                    Attendee attendee = game.Members.FindByPlayerId(player.Id);
                    TableRow row = new TableRow();
                    TableCell dateCell = new TableCell();
                    dateCell.Text = game.Date.ToString("ddd, MMM d, yyyy");
                    row.Cells.Add(dateCell);
                    TableCell statusCell = new TableCell();
                    statusCell.HorizontalAlign = HorizontalAlign.Right;
                    ImageButton imageBtn = new ImageButton();
                    imageBtn.ID = player.Id + "," + game.Date.ToShortDateString();
                    imageBtn.ImageUrl = GetStatusImageUrl(attendee.Status);
                    imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
                    imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
                    if (IsReservationLocked(game.Date))
                    {
                        imageBtn.Enabled = false;
                    }
                    else
                    {
                        imageBtn.Click += new ImageClickEventHandler(MemberChangeAttendance_Click);
                        row.BackColor = System.Drawing.Color.Aqua;
                    }
                    //  imageBtn.CssClass = "imageBtn";
                    statusCell.Controls.Add(imageBtn);
                    row.Cells.Add(statusCell);
                    this.GameTable.Rows.Add(row);
                }
                else if (game.Dropins.Items.Exists(dropin => dropin.PlayerId == player.Id && dropin.Status != InOutNoshow.Out))
                {
                    Attendee pickup = game.Dropins.FindByPlayerId(player.Id);
                    TableRow row = new TableRow();
                    TableCell dateCell = new TableCell();
                    dateCell.Text = game.Date.ToString("ddd, MMM d, yyyy");
                    row.Cells.Add(dateCell);
                    TableCell statusCell = new TableCell();
                    statusCell.HorizontalAlign = HorizontalAlign.Right;
                    Image imageBtn = new Image();
                    imageBtn.ID = player.Id + "," + game.Date.ToShortDateString();
                    imageBtn.ImageUrl = GetStatusImageUrl(pickup.Status);
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
 

 
        protected void BackBtn_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(Constants.DEFAULT_PAGE + "?Pool=" + CurrentPool.Name);

        }

        protected void MemberChangeAttendance_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            String playerId = btn.ID.Split(',')[0];
 
            DateTime date = DateTime.Parse(btn.ID.Split(',')[1]);
            Player player = Manager.FindPlayerById(playerId);
            Game game = CurrentPool.FindGameByDate(date);
            Attendee attendee = game.Members.FindByPlayerId(playerId);
            if (attendee.Status == InOutNoshow.Out)
            {
                if (IsSpotAvailable(CurrentPool, game.Date))
                {
                    ReservePromarySpot(CurrentPool, game, player);
                    Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.RESERVED, CurrentPool, CurrentPool, ComingGameDate);
                }
                else
                {
                    ShowMessage("Pool " + CurrentPool.Name + " is full. If you would like to add to waiting list, use the pool reservation page");
                    return;
                }
                return;
            }
            else if (attendee.Status == InOutNoshow.In)
            {
                CancelPromarySpot(CurrentPool, game, player);
                Manager.AddReservationNotifyWechatMessage(player.Id, CurrentUser.Id, Constants.CANCELLED, CurrentPool, CurrentPool, ComingGameDate);
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
     }
}