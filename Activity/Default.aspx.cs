using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reservation
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Application[Constants.RESERVATION] = DataAccess.LoadReservation();
            Page.Title = Reservations.Title;
            ((Label)Master.FindControl("TitleLabel")).Text = Reservations.Title;

            foreach (Game game in Reservations.Games.OrderBy(game =>game.Date))
            {
                if (game.Id == null)
                {
                    game.Id = Guid.NewGuid().ToString();
                    DataAccess.Save(Reservations);
                }
                if (game.Date < DateTime.Today || !game.Publish)
                {
                    continue;
                }
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                LinkButton lbtn = new LinkButton();
                if (!String.IsNullOrEmpty(game.Title))
                {
                    lbtn.Text = game.Title + "(" + game.Date.ToShortDateString() +")";
                }
                else
                {
                    lbtn.Text = game.Date.ToString("ddd, MMM d, yyyy");
                }
                lbtn.Font.Bold = true;
                lbtn.Font.Size = new FontUnit(Constants.LINKBUTTON_FONTSIZE);
                lbtn.ID = game.Id + "," + game.Title;
                lbtn.Click += new EventHandler(Activity_Click);
                cell.Controls.Add(lbtn);
                cell.HorizontalAlign = HorizontalAlign.Center;
                row.Cells.Add(cell);
                this.ActivityTable.Rows.Add(row);
                this.AdminLink.Visible = Reservations.OpenAdmin;
            }
        }

        protected void Activity_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            String gameId = lbtn.ID.Split(',')[0];

            Response.Redirect("Activity.aspx?gameId=" + gameId);
        }

        private Reservation Reservations
        {
            get
            {
                return (Reservation)Application[Constants.RESERVATION];

            }
            set { }
        }

        protected void AdminLink_Clcik(object sender, EventArgs e)
        {
            Response.Redirect("Admin.aspx");

        }

    }
}