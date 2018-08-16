using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Fees : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Application[Constants.DATA] = DataAccess.LoadReservation();
            if (!IsPostBack)
            {
                //
                if (null != Session[Constants.SUPER_ADMIN])
                {
                    ((TextBox)Master.FindControl("PasscodeTb")).Text = Session[Constants.SUPER_ADMIN].ToString();
                }
                //Bind player list
                int selectPlayerIndex = this.PlayerListbox.SelectedIndex;
                this.PlayerListbox.DataSource = Manager.Players.OrderBy(player => player.Name);
                this.PlayerListbox.DataTextField = "Name";
                this.PlayerListbox.DataValueField = "Id";
                this.PlayerListbox.DataBind();
                this.PlayerListbox.SelectedIndex = selectPlayerIndex;
                //Add fee type into FeetypeDropinDownList
                this.FeeTypeDDL.Items.Clear();
                foreach (FeeTypeEnum value in Enum.GetValues(typeof(FeeTypeEnum)))
                {
                    this.FeeTypeDDL.Items.Add(value.ToString());
                }
            }
            //
            this.ResetFeeBtn.OnClientClick = "if ( !confirm('Are you sure you want to delete this fee?')) return false;";
            if (Convert.ToString(ViewState["Generated"]) == "true")
            {
                ShowFees();
            }
            GenerationFeeReport();
            List<Fee> allCredits = new List<Fee>();
            decimal totalAmount = 0;
            foreach (Player player in Manager.Players)
            {
                foreach (Fee fee in player.Fees)
                {
                    if (fee.FeeDesc.StartsWith("Credit"))
                    {
                        Fee paidFee = new Fee();
                        paidFee.PayDate = fee.PayDate;
                        paidFee.FeeType = fee.FeeType;
                        paidFee.Amount = fee.Amount;
                        paidFee.FeeDesc = player.Name + "-" + fee.FeeDesc;
                        totalAmount = totalAmount + fee.Amount;
                        allCredits.Add(paidFee);
                    }
                }
            }

            foreach (Fee fee in allCredits)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Text = fee.FeeDesc;
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.Text = fee.Amount.ToString();
                row.Cells.Add(cell);
                this.TransferTable.Rows.Add(row);
            }
            TableRow totalRow = new TableRow();
            TableCell totalcell = new TableCell();
            totalcell.Text = "Total";
            totalRow.Cells.Add(totalcell);
            totalcell = new TableCell();
            totalcell.Text = totalAmount.ToString();
            totalRow.Cells.Add(totalcell);
            this.TransferTable.Rows.Add(totalRow);

        }

        private bool IsSuperAdmin()
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
            TextBox passcodeTb = (TextBox)Master.FindControl("PasscodeTb");
            if (Manager.SuperAdmin != passcodeTb.Text)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong passcode! Re-enter your passcode and try again')", true);
                return false;
            }
            Session[Constants.SUPER_ADMIN] = passcodeTb.Text;
            return true;
        }


        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }



        private void ShowFees()
        {
            Player player = Manager.FindPlayerById(PlayerListbox.SelectedValue);
            if (player == null)
            {
                return;
            }
            this.FeeTable.Rows.Clear();
            this.FeeTable.Rows.Add(this.FeeTableHeaderRow);
            foreach (Fee fee in player.Fees)
            {
                TableRow row = new TableRow();
                row.Cells.Add(CreateTableCell(fee.Date.ToString("MMM. dd yyyy")));
                row.Cells.Add(CreateTableCell(fee.FeeType));
                row.Cells.Add(CreateTableCell(fee.FeeDesc));

                TableCell amountCell = new TableCell();
                TextBox feeTb = new TextBox();
                amountCell.Controls.Add(feeTb);
                feeTb.Text = fee.Amount.ToString();
                feeTb.AutoPostBack = true;
                feeTb.ID = "Fee," + fee.FeeId;
                feeTb.TextChanged += new EventHandler(FeeTb_TextChanged);
                row.Cells.Add(amountCell);
                if (fee.IsPaid)
                {
                    row.Cells.Add(CreateTableCell(fee.PayDate.ToShortDateString()));
                }
                else
                {
                    row.Cells.Add(new TableCell());
                }
                row.Cells.Add(CreateCheckboxTableCell(fee));
                TableCell editCell = new TableCell();
                row.Cells.Add(editCell);
                Button deleteBtn = new Button();
                deleteBtn.Text = "Delete";
                deleteBtn.ID = "Delete," + fee.FeeId;
                deleteBtn.OnClientClick = "if ( !confirm('Are you sure you want to delete this fee?')) return false;" ;
                deleteBtn.Click += new EventHandler(DeleteBtn_Click);
                editCell.Controls.Add(deleteBtn);
                row.Cells.Add(editCell);
                this.FeeTable.Rows.Add(row);
            }
        }

        void FeeTb_TextChanged(object sender, EventArgs e)
        {
            TextBox feeTb = (TextBox)sender;
            String feeId = feeTb.ID.Split(',')[1];
            Player player = Manager.FindPlayerById(this.PlayerListbox.SelectedValue);
            if (player == null)
            {
                return;
            }
            Fee fee = player.FindFeeById(feeId);
            fee.Amount = decimal.Parse(feeTb.Text);
            DataAccess.Save(Manager);
            Session[Constants.FEE_ID] = null;
            if (Convert.ToString(ViewState["Generated"]) != "true")
            {
                ShowFees();
                ViewState["Generated"] = "true";
            }
        }

 
        void DeleteBtn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            String feeId = btn.ID.Split(',')[1];
            Player player = Manager.FindPlayerById(this.PlayerListbox.SelectedValue);
            if (player == null)
            {
                return;
            }
            Fee fee = player.FindFeeById(feeId);
            if (fee.FeeType == FeeTypeEnum.Prepaid.ToString())
            {
                player.PrePaidBalance -= fee.Amount;
            }

            player.Fees.Remove(fee);
            DataAccess.Save(Manager);
            if (Convert.ToString(ViewState["Generated"]) != "true")
            {
                ShowFees();
                ViewState["Generated"] = "true";
            }
            ShowFees();
            GenerationFeeReport();
            //Response.Redirect(Request.RawUrl);
        }

        private TableCell CreateTableCell(String content)
        {
            TableCell cell = new TableCell();
            cell.Text = content;
            return cell;
        }
        private TableCell CreateCheckboxTableCell(Fee fee)
        {
            TableCell cell = new TableCell();
            CheckBox checkbox = new CheckBox();
            checkbox.ID = fee.FeeId;
            checkbox.Checked = fee.IsPaid;
            checkbox.CheckedChanged += new EventHandler(Checkbox_CheckedChanged);
            checkbox.AutoPostBack = true;
            cell.Controls.Add(checkbox);
            return cell;
        }

        void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            String feeId = checkbox.ID;
            Player player = Manager.FindPlayerById(PlayerListbox.SelectedValue);
            Fee fee = player.FindFeeById(feeId);
            fee.IsPaid = checkbox.Checked;
            if (fee.IsPaid)
            {
                fee.PayDate = DateTime.Today;
                if (fee.FeeType == FeeTypeEnum.Prepaid.ToString())
                {
                    player.PrePaidBalance += fee.Amount;
                }
            }
            else
            {
                fee.PayDate = DateTime.MinValue;
                if (fee.FeeType == FeeTypeEnum.Prepaid.ToString())
                {
                    player.PrePaidBalance -= fee.Amount;
                }
            }
            DataAccess.Save(Manager);
            ShowFees();
            GenerationFeeReport();
           //Response.Redirect(Request.RawUrl);
        }

        protected void PlayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.PlayerListbox.SelectedIndex == -1)
            {
                return;
            }
            Player player = Manager.FindPlayerById(this.PlayerListbox.SelectedValue);
            if (player == null)
            {
                return;
            }
            if (Convert.ToString(ViewState["Generated"]) != "true")
            {
                ShowFees();
                ViewState["Generated"] = "true";
            }
            this.FreeDropinTb.Text = player.FreeDropin.ToString();
            this.TransferTb.Text = player.AvailableTransferCount.ToString();
            this.UsedTransferTb.Text = player.TransferUsed.ToString();
        }

        protected void AddFeeBtn_Click(object sender, EventArgs e)
        {
            Player player = Manager.FindPlayerById(this.PlayerListbox.SelectedValue);
            if (player == null)
            {
                return;
            }
            Fee fee = new Fee();
            fee.FeeType = this.FeeTypeDDL.SelectedValue;
            fee.FeeDesc = this.FeeDescTb.Text;
            fee.Amount = decimal.Parse(this.FeeAmountTb.Text);
            fee.Date = DateTime.Today;
            player.Fees.Add(fee);
            DataAccess.Save(Manager);
            ShowFees();
            //Response.Redirect(Request.RawUrl);

        }

        protected void UpdateFreeDropinBtn_Click(object sender, EventArgs e)
        {
            Player player = Manager.FindPlayerById(this.PlayerListbox.SelectedValue);
            if (player == null || this.FreeDropinTb.Text == "")
            {
                return;
            }
            player.FreeDropin = int.Parse(this.FreeDropinTb.Text);
           // player.Transfers = int.Parse(this.TransferTb.Text);
           // player.TransferUsed = int.Parse(this.UsedTransferTb.Text);
            DataAccess.Save(Manager);
            //Response.Redirect(Request.RawUrl);
        }

        protected void ResetAllFeeBtn_Click(object sender, EventArgs e)
        {
            foreach (Player player in Manager.Players)
            {
                player.Fees = new List<Fee>();
            }
            foreach (Pool pool in Manager.Pools)
            {
                foreach (Member member in pool.Members)
                {
                    Player player = Manager.FindPlayerById(member.Id);
                    player.Fees.Add(new Fee(String.Format(Fee.FEETYPE_MEMBERSHIP, pool.Name), pool.MembershipFee));
                }
            }
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        protected void ReverseAllFeeBtn_Click(object sender, EventArgs e)
        {
            foreach (Player player in Manager.Players)
            {
                foreach (Fee fee in player.Fees)
                {
                    fee.Amount = -1 * fee.Amount;
                }
            }
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        private void GenerationFeeReport()
        {
            List<Fee> allPaidFees = new List<Fee>();
            foreach (Player player in Manager.Players)
            {
                foreach (Fee fee in player.Fees)
                {
                    if (fee.IsPaid)
                    {
                        Fee paidFee = new Fee();
                        paidFee.PayDate = fee.PayDate;
                        paidFee.FeeType = fee.FeeType;
                        paidFee.Amount = fee.Amount;
                        paidFee.FeeDesc = player.Name + "-" + fee.FeeDesc;
                        allPaidFees.Add(paidFee);
                    }
                }
            }
            this.FeeReportTable.Rows.Clear();
            this.FeeReportTable.Rows.Add(this.FeeReportHeaderRow);
            decimal balance = 0;
            IEnumerable<Fee> feesQuery = allPaidFees.OrderBy(fee => fee.PayDate);
            foreach (Fee paidFee in feesQuery)
            {
                TableRow row = new TableRow();
                TableCell dateCell = new TableCell();
                dateCell.Text = paidFee.PayDate.ToString("MMM. dd yyyy");
                row.Cells.Add(dateCell);
                TableCell typeCell = new TableCell();
                typeCell.Text = paidFee.FeeType.ToString();
                row.Cells.Add(typeCell);
                TableCell descCell = new TableCell();
                descCell.Text = paidFee.FeeDesc;
                row.Cells.Add(descCell);
                TableCell creditCell = new TableCell();
                if (paidFee.Amount > 0)
                {
                    creditCell.Text = paidFee.Amount.ToString();
                }
                row.Cells.Add(creditCell);
                TableCell debitCell = new TableCell();
                if (paidFee.Amount < 0)
                {
                    debitCell.Text = paidFee.Amount.ToString();
                }
                row.Cells.Add(debitCell);
                TableCell balanceCell = new TableCell();
                balance += paidFee.Amount;
                balanceCell.Text = balance.ToString();
                row.Cells.Add(balanceCell);
                this.FeeReportTable.Rows.AddAt(1, row);
            }
/*            TableRow balanceRow = new TableRow();
            TableCell labelCell = new TableCell();
            labelCell.Text = "Balance";
            balanceRow.Cells.Add(labelCell);
            balanceRow.Cells.Add(new TableCell());
            balanceRow.Cells.Add(new TableCell());
            balanceRow.Cells.Add(new TableCell());
            TableCell balanceCell = new TableCell();
            balanceCell.Text = balance.ToString();
            balanceRow.Cells.Add(balanceCell);
   */         
            //this.FeeReportTable.Rows.AddAt(0, balanceRow);
            this.FeeReportTable.Caption = "Fee & Payment Reports - Balance : $" + balance.ToString();
  
        }

    }
}