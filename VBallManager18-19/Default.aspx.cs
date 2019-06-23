using System;
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
                String existingUserId = Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID];
                Player user = Manager.FindPlayerById(existingUserId);
                if (user != null)
                {
                    if (!IsCallback && String.IsNullOrEmpty(user.Birthday) && !String.IsNullOrEmpty(user.WechatName))
                    {
                        Response.Redirect(Constants.PLUGIN_PAGE);
                        return;
                    }
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
                Game game = Manager.FindComingGame(pool);
                if (Manager.ActionPermitted(Actions.View_All_Pools, currentUser.Role) || pool.Members.Exists(currentUser.Id) ||//
                    pool.Dropins.Items.Exists(d => d.PlayerId == currentUser.Id && !d.IsCoop) ||
                    (pool.Dropins.Items.Exists(d => d.PlayerId == currentUser.Id && d.IsCoop) && !pool.AutoCoopReserve) ||//
                    game != null && game.Dropins.Items.Exists(d=>d.PlayerId==currentUser.Id && (!d.IsCoop || d.IsCoop && d.Status== InOutNoshow.In)))
                {
                    TableRow row = new TableRow();
                    TableCell cell = new TableCell();
                    HyperLink link = new HyperLink();
                    link.Text = pool.DayOfWeek.ToString() + " Pool " + pool.Name;
                    //link.NavigateUrl = Constants.RESERVE_PAGE + "?Pool=" + pool.Name;
                    link.NavigateUrl = Constants.PRE_REGISTER_LINK_PAGE + "?Pool=" + pool.Name;
                    cell.Controls.Add(link);
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    row.Cells.Add(cell);
                    this.ReserveLinkTable.Rows.Add(row);
                }
            }
        }
   
    }
}