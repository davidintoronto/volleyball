using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Authen : System.Web.UI.Page
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


                this.LinkStatusList.DataSource = Manager.Players.OrderBy(player => player.Name);
                this.LinkStatusList.DataTextField = "Name";
                this.LinkStatusList.DataValueField = "Id";
                this.LinkStatusList.DataBind();

                foreach (ListItem playerItem in this.LinkStatusList.Items)
                {
                    Player player = Manager.FindPlayerById(playerItem.Value);
                    playerItem.Selected = player.DeviceLinked;
                }
 
            }
            //
         }

        private bool IsSuperAdmin()
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null)
            {
                String userId = Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID];
                String passcode = Request.Cookies[Constants.PRIMARY_USER][Constants.PASSCODE];
                Player player = Manager.FindPlayerById(userId);
                if (!String.IsNullOrEmpty(player.Passcode) && player.Passcode == passcode && Manager.ActionPermitted(Actions.Admin_Management, player.Role))
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
            this.AuthUusersLb.Items.Clear();
            foreach (Player user in Manager.Players.OrderBy(user => user.Name))
            {
                ListItem item = new ListItem(user.Name, user.Id);
                if (player.AuthorizedUsers.Contains(user.Id))
                {
                    item.Selected = true;
                }
                this.AuthUusersLb.Items.Add(item);
            }
            this.LinkDeviceTb.Text = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath) + "/" + Constants.REGISTER_DEVICE_PAGE + "?id=" + Manager.ReversedId(player.Id);
            //this.LinkDeviceTb.Text = "http://hitmen.000webhostapp.com/register.html?id=" + Manager.ReversedId(player.Id);
            this.ResetLinkDeviceTb.Text = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath) + "/" + Constants.REGISTER_DEVICE_PAGE + "?reset=true";
        }

 
        protected void AuthUusersLb_SelectedIndexChanged(object sender, EventArgs e)
        {
            Player player = Manager.FindPlayerById(this.PlayerListbox.SelectedValue);
            if (player == null)
            {
                return;
            }
            player.AuthorizedUsers.Clear();
            foreach (ListItem item in this.AuthUusersLb.Items)
            {
                if (item.Selected)
                {
                    player.AuthorizedUsers.Add(item.Value);
                }
            }
            DataAccess.Save(Manager);           
        }

        protected void LinkDeviceLb_Click(object sender, EventArgs e)
        {
            if (this.PlayerListbox.SelectedIndex >= 0)
            {
                Response.Redirect(Constants.REGISTER_DEVICE_PAGE + "?id=" + Manager.ReversedId(this.PlayerListbox.SelectedItem.Value));
            }
        }

        protected void ResetLinkDeviceLb_Click(object sender, EventArgs e)
        {
            if (this.PlayerListbox.SelectedIndex >= 0)
            {
                Response.Redirect(Constants.REGISTER_DEVICE_PAGE + "?reset=true");
            }
        }

    }
}