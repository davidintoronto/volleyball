using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class RequestRegisterLink : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IEnumerable<Player> playerQuery = Manager.Players.FindAll(player => player.IsActive).OrderBy(player => player.Name);
                 this.UserList.DataSource = playerQuery;
                this.UserList.DataTextField = "Name";
                this.UserList.DataValueField = "Id";
                this.UserList.DataBind();
            }

        }

        protected void RequestBtn_Click(object sender, EventArgs e)
        {
            if (UserList.SelectedIndex < 0) return;
            Player user = Manager.FindPlayerById(UserList.SelectedValue);
            if (String.IsNullOrEmpty(user.WechatName))
            {
                this.ResultLabel.Text = "Your account is not bound to Wechat, please contact admin for advice.";
                this.RequestBtn.Visible = false;
            }
            else
            {
                this.RequestBtn.Visible = true;
                String notification = "Hi, " + user.Name + ". Here is your private register link " + Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath) + "/" + Constants.REGISTER_DEVICE_PAGE + "?id=" + Manager.ReversedId(user.Id);
                notification = notification.Replace("//" + Constants.REGISTER_DEVICE_PAGE, "/" + Constants.REGISTER_DEVICE_PAGE);
                Manager.AddNotifyWechatMessage(user, notification);
                this.ResultLabel.Text = "Your private register link has sent, you will receive it in your Wechat in minute";
            }
        }

        protected void UserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UserList.SelectedIndex < 0) return;
            Player user = Manager.FindPlayerById(UserList.SelectedValue);
            if (String.IsNullOrEmpty(user.WechatName))
            {
                this.RequestBtn.Visible = false;
                this.ResultLabel.Text = "Your account is not bound to Wechat, please contact admin for advice.";
            }
            else
            {
                this.RequestBtn.Visible = true;
                this.ResultLabel.Text = "Click the button above, you will receive your private register link in your Wechat";
            }
        }

        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }
    }
}