using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Checker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GoBtn_Click(object sender, EventArgs e)
        {
            if (MaindrawTb.Text == "")
            {
                return;
            }
            String[] mainDrow = MaindrawTb.Text.Split(' ');
            String bonus = BounsTb.Text;
            String[] millionsDraws = MDrawsTb.Text.Split(new char[]{'\n','\r'});

            String[] ticketLines = TicketsTb.Text.Split(new char[]{'\n','\r'});
            List<String[]> tickets = new List<string[]>();
            foreach (String line in ticketLines)
            {
                tickets.Add(line.Split(' '));
            }

            //Main Draw check
            foreach (String[] ticket in tickets)
            {
                TableRow row = new TableRow();
                int matches = 0;
                bool matchBonus = false;
                foreach (String ticketNumber in ticket)
                {
                    TableCell cell = new TableCell();
                    cell.Font.Size = 15;
                    cell.Font.Bold = true;
                    cell.Text = ticketNumber;
                    foreach (String winNumber in mainDrow)
                    {
                        if (ticketNumber == winNumber)
                        {
                            cell.BackColor = System.Drawing.Color.Pink;
                            matches++;
                            break;
                        }
                    }
                    if (ticketNumber == bonus)
                    {
                        matchBonus = true;
                        cell.BackColor = System.Drawing.Color.Yellow;
                    }
                    row.Cells.Add(cell);
                }
                TableCell matchCell = new TableCell();
                matchCell.Font.Size = 15;
                matchCell.Font.Bold = true;
                matchCell.ForeColor = System.Drawing.Color.Blue;
                if (matches >= 3)
                {
                    matchCell.Text = matches.ToString() + (matchBonus ? "/Bonus" : "");
                }
                row.Cells.Add(matchCell);
                this.MainDrawMatchTable.Rows.Add(row);
            }
                //Millions draw check
            foreach (String ticketline in ticketLines)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Font.Size = 15;
                cell.Font.Bold = true;
                cell.Text = ticketline;
                foreach (String winNumbers in millionsDraws)
                {
                    if (ticketline == winNumbers)
                    {
                        cell.BackColor = System.Drawing.Color.Pink;
                        break;
                    }
                }
                row.Cells.Add(cell);
                this.MillionsDrawMatchTable.Rows.Add(row);
            }
        }
    }
}