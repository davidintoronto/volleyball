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
    public partial class Billing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsSuperAdmin()) return;

            IEnumerable<Player> playerrQuery = Manager.Players.OrderBy(player => player.Name);
            bool alterbackcolor = false;
            foreach (Player player in playerrQuery)
            {
                TableRow row = new TableRow();
                if (alterbackcolor)
                {
                    row.BackColor = this.PlayerTable.BorderColor;
                }
                alterbackcolor = !alterbackcolor;
                TableCell nameCell = new TableCell();
                nameCell.HorizontalAlign = HorizontalAlign.Center;
                LinkButton lbtn = new LinkButton();
                lbtn.Text = player.Name;
                lbtn.Font.Bold = true;
                lbtn.Font.Size = new FontUnit("2em");
                lbtn.ID = player.Id + ",MEMEBER";
                lbtn.Click += new EventHandler(PlayerName_Click);
                nameCell.Controls.Add(lbtn);
                bool hasUnpaid = false;
                foreach (Fee fee in player.Fees)
                {
                    if (!fee.IsPaid && (fee.Amount > 0 || fee.Amount < 0))
                    {
                        Image image = new Image();
                        image.ImageUrl = "~/Icons/dollar.png";
                        nameCell.Controls.Add(image);
                        hasUnpaid = true;
                    }
                }
                row.Cells.Add(nameCell);
                if (hasUnpaid)
                {
                    this.PlayerTable.Rows.Add(row);
                }
                else
                {
                    this.PaidPlayerTable.Rows.Add(row);
                }
            }
        }

        protected void PlayerName_Click(object sender, EventArgs e)
        {
            LinkButton lbtn = (LinkButton)sender;
            String id = lbtn.ID.Split(',')[0];
            Session[Constants.CURRENT_PLAYER_ID] = id;
            Session[Constants.ACTION_TYPE] = Constants.ACTION_DETAIL;
            Session[Constants.CONTROL] = sender;
            Response.Redirect("BillingDetail.aspx?id=" + id);
        }
        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }

        public bool IsSuperAdmin()
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null)
            {
                String userId = Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID];
                 Player player = Manager.FindPlayerById(userId);
                if (Manager.ActionPermitted(Actions.Admin_Management, player.Role))
                {
                    return true;
                }
            }
            return false;
        }

    }
}