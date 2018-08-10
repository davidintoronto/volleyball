﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class PoolLinkList : System.Web.UI.Page
    {
         protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null)
            {
                String existingUserId = Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID];
                Player user = Manager.FindPlayerById(existingUserId);
                if (user != null)
                {
                    FillReservationLinkTable(user);
                    return;
                }
            }
            Response.Redirect(Constants.REQUEST_REGISTER_LINK_PAGE);
            return;
        }

        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }



        private void FillReservationLinkTable(Player currentUser)
        {
            //Show reservation links
            this.ReserveLinkTable.Caption = "Open reservation links below";
            this.ReserveLinkTable.Rows.Clear();
            foreach (Pool pool in Manager.Pools)
            {
                if (Manager.ActionPermitted(Actions.View_All_Pools, currentUser.Role) || pool.Members.Exists(attendee => attendee.Id == currentUser.Id) || pool.Dropins.Exists(attendee => attendee.Id == currentUser.Id))
                {
                    TableRow row = new TableRow();
                    TableCell cell = new TableCell();
                    HyperLink link = new HyperLink();
                    link.Text = pool.DayOfWeek.ToString() + " Pool " + pool.Name;
                    link.NavigateUrl = "Default.aspx?Pool=" + pool.Name;
                    cell.Controls.Add(link);
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    row.Cells.Add(cell);
                    this.ReserveLinkTable.Rows.Add(row);
                }
            }
        }
   
    }
}