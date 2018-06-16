using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Menu)Master.FindControl("NavigationMenu")).Visible = false;
            ((TextBox)Master.FindControl("PasscodeTb")).Visible = false;
            ((Label)Master.FindControl("TitleLabel")).Text = "Financial Reports";
            Application[Constants.DATA] = DataAccess.LoadReservation();
            GenerationFeeReport();
        }

        private bool IsSuperAdminPasscode()
        {
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
                        paidFee.Amount = fee.Amount;
                        paidFee.FeeDesc = player.Name + "-" + fee.FeeDesc;
                        allPaidFees.Add(paidFee);
                    }
                }
            }
            this.FeeReportTable.Rows.Clear();
            this.FeeReportTable.Rows.Add(this.FeeReportHeaderRow);
            decimal balance = 0;
            IEnumerable<Fee> feesQuery = allPaidFees.OrderByDescending(fee => fee.PayDate);
            foreach (Fee paidFee in feesQuery)
            {
                TableRow row = new TableRow();
                TableCell dateCell = new TableCell();
                dateCell.Text = paidFee.PayDate.ToString("MMM. dd yyyy");
                row.Cells.Add(dateCell);
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
                this.FeeReportTable.Rows.AddAt(1, row);
                balance += paidFee.Amount;
            }
            TableRow balanceRow = new TableRow();
            TableCell labelCell = new TableCell();
            labelCell.Text = "Balance";
            balanceRow.Cells.Add(labelCell);
            balanceRow.Cells.Add(new TableCell());
            balanceRow.Cells.Add(new TableCell());
//            balanceRow.Cells.Add(new TableCell());
            TableCell balanceCell = new TableCell();
            balanceCell.Text = balance.ToString();
            balanceRow.Cells.Add(balanceCell);
            
            //this.FeeReportTable.Rows.AddAt(0, balanceRow);
            this.FeeReportTable.Caption = "2016-2017 Financial Reports - Balance : $" + balance.ToString();
  
        }

    }
}