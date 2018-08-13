using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class BillingDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsSuperAdmin()) return;
            if (Request.Params["id"] == null)
            {
                Response.Redirect("Billing.aspx");
                return; ;
            }
            String id = Request.Params["id"].ToString();
            Session[Constants.PLAYER_ID] = id;
            Player player = Manager.FindPlayerById(id);
            this.FeeTable.Caption = player.Name;
            FillFeeAndPaymentTable(player);
            FillPrePaymentTable(player);
        }

        public bool IsSuperAdmin()
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null)
            {
                String userId = Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID];
                Player player = Manager.FindPlayerById(userId);
                if (Manager.ActionPermitted(Actions.Admin_Management, player.Role))
                {
                    return true;
                }
            }
            return false;
        }

        private void FillFeeAndPaymentTable(Player player)
        {
            IEnumerable<Fee> query = player.Fees.OrderByDescending(fee => fee.Date);
            decimal sum = 0;

            TableRow sumRow = new TableRow();
            sumRow.BackColor = System.Drawing.Color.YellowGreen;
            TableCell cell = new TableCell();
            cell.Text = "Fee";
            sumRow.Cells.Add(cell);
            cell = new TableCell();
            cell.Text = "Unpaid Total";
            sumRow.Cells.Add(cell);
            TableCell statusCell = new TableCell();
            statusCell.HorizontalAlign = HorizontalAlign.Right;
            Image image = new Image();
            image.ImageUrl = "~/Icons/dollar.png";
            //  imageBtn.CssClass = "imageBtn";
            statusCell.Controls.Add(image);
            sumRow.Cells.Add(statusCell);
            TableCell feeTotalDollarCell = new TableCell();
            feeTotalDollarCell.HorizontalAlign = HorizontalAlign.Right;
            //dollarCell.Text = fee.Amount.ToString();
            sumRow.Cells.Add(feeTotalDollarCell);
            TableCell paidCell = new TableCell();
            ImageButton imageBtn = new ImageButton();
            imageBtn.ID = "feetotal";
            imageBtn.ImageUrl = "~/Icons/Out.png";
            imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Click += new ImageClickEventHandler(PayAll_Click);
            paidCell.Controls.Add(imageBtn);
            sumRow.Cells.Add(paidCell);
            this.FeeTable.Rows.Add(sumRow);

            bool hasUnpaid = false;
            foreach (Fee fee in query)
            {
                if (fee.Amount == 0)
                {
                    continue;
                }
                hasUnpaid = hasUnpaid || !fee.IsPaid;
                TableRow row = new TableRow();
                TableCell dateCell = new TableCell();
                dateCell.Text = fee.Date.ToString("MM/dd/yyyy");
                row.Cells.Add(dateCell);
                TableCell typeCell = new TableCell();
                typeCell.Text = fee.FeeType;
                row.Cells.Add(typeCell);
                TableCell descCell = new TableCell();
                typeCell.Text = fee.FeeDesc;
                row.Cells.Add(descCell);
                statusCell = new TableCell();
                statusCell.HorizontalAlign = HorizontalAlign.Right;
                image = new Image();
                image.ImageUrl = "~/Icons/dollar.png";
                //  imageBtn.CssClass = "imageBtn";
                statusCell.Controls.Add(image);
                row.Cells.Add(statusCell);
                TableCell dollarCell = new TableCell();
                dollarCell.HorizontalAlign = HorizontalAlign.Right;
                dollarCell.Text = fee.Amount.ToString();
                row.Cells.Add(dollarCell);
                paidCell = new TableCell();
                imageBtn = new ImageButton();
                imageBtn.ID = fee.FeeId;
                imageBtn.ImageUrl = fee.IsPaid ? "~/Icons/In.png" : "~/Icons/Out.png";
                imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
                imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
                imageBtn.Click += new ImageClickEventHandler(FeePaid_Click);
                paidCell.Controls.Add(imageBtn);
                row.Cells.Add(paidCell);
                this.FeeTable.Rows.Add(row);
                if (!fee.IsPaid) sum += fee.Amount;
            }
            feeTotalDollarCell.Text = sum.ToString();
            this.PrepayTable.Visible = !hasUnpaid;
        }

         protected void FeePaid_Click(object sender, EventArgs e)
         {
             String feeId = ((ImageButton)sender).ID;
             String playerId = (String)Session[Constants.PLAYER_ID];
             Player player = Manager.FindPlayerById(playerId);
             Fee fee = player.FindFeeById(feeId);
             fee.IsPaid = !fee.IsPaid;
             if (fee.IsPaid)
             {
                 fee.PayDate = DateTime.Today;
             }
             else
             {
                 fee.PayDate = DateTime.MinValue;
             }
             DataAccess.Save(Manager);
             if (fee.IsPaid)
             {
                 Manager.AddNotifyWechatMessage(player, "Hi, " + player.Name + ". We have received your payment $" + fee.Amount + " . Thank you!");
             }
             Response.Redirect("BillingDetail.aspx?id="+playerId);
         }

         protected void PayAll_Click(object sender, EventArgs e)
         {
             
             String playerId = (String)Session[Constants.PLAYER_ID];
             Player player = Manager.FindPlayerById(playerId);
             decimal total = 0;
             foreach (Fee fee in player.Fees)
             {
                 if (!fee.IsPaid && fee.Amount > 0)
                 {
                     fee.IsPaid = true;
                     fee.PayDate = DateTime.Today;
                     total = total + fee.Amount;
                 }
             }
              DataAccess.Save(Manager);
             if (total >0) Manager.AddNotifyWechatMessage(player, "Hi, " + player.Name + ". We have received your payment $" + total + " . Thank you!");
             Response.Redirect("BillingDetail.aspx?id=" + playerId);
         }

         private void FillPrePaymentTable(Player player)
         {
             this.PrepayTable.Caption = "Pre-Payment";
             TableRow row = new TableRow();
             TableCell textCell = new TableCell();
             textCell.Text = "Balance";
             textCell.HorizontalAlign = HorizontalAlign.Left;
             row.Cells.Add(textCell);
             TableCell balanceCell = new TableCell();
             Image image = new Image();
             image.ImageUrl = "~/Icons/dollar.png";
             //  imageBtn.CssClass = "imageBtn";
             balanceCell.Controls.Add(image);
             balanceCell.HorizontalAlign = HorizontalAlign.Right;
             row.Cells.Add(balanceCell);
             balanceCell = new TableCell();
             balanceCell.Text = player.PrePaidBalance.ToString();
             balanceCell.HorizontalAlign = HorizontalAlign.Center;
             row.Cells.Add(balanceCell);
             this.PrepayTable.Rows.Add(row);
             //
             row = new TableRow();
             TableCell amountCell = new TableCell();
             amountCell.Controls.Add(this.PrePayAmountTb);
             amountCell.Controls.Add(this.MoneyVD);
             row.Cells.Add(amountCell);
             TableCell addCell = new TableCell();
             ImageButton imageBtn = new ImageButton();
             imageBtn.ID = player.Id;
             imageBtn.ImageUrl = "~/Icons/Add.png";
             imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
             imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
             imageBtn.Click += new ImageClickEventHandler(AddPrepayBtn_Click);
             addCell.Controls.Add(imageBtn);
             addCell.HorizontalAlign = HorizontalAlign.Right;
             addCell.ColumnSpan = 2;
             row.Cells.Add(addCell);
             this.PrepayTable.Rows.Add(row);
         }
    
        protected void AddPrepayBtn_Click(object sender, ImageClickEventArgs e)
        {
            if (String.IsNullOrEmpty(this.PrePayAmountTb.Text.Trim()))
            {
                return;
            }
            String playerId = ((ImageButton)sender).ID;
            Player player = Manager.FindPlayerById(playerId);
            decimal amount = decimal.Parse(this.PrePayAmountTb.Text);
            player.PrePaidBalance += amount;
            //Create payment fee
            Fee fee = new Fee(Fee.FEETYPE_DROPIN_PRE_PAID, amount);
            fee.FeeType = FeeTypeEnum.Prepaid.ToString();
            fee.IsPaid = true;
            fee.PayDate = DateTime.Today;
            player.Fees.Add(fee);
            this.PrePayAmountTb.Text = "";
            DataAccess.Save(Manager);
            Response.Redirect("BillingDetail.aspx?id=" + playerId);
        }


        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }


        protected void BackBtn_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Billing.aspx");

        }

 
     }
}