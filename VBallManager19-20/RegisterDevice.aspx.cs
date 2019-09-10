using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class RegisterDevice : System.Web.UI.Page
    {
         private const String RESET = "reset";
         private const String PLAYER_ID = "id";
         protected void Page_Load(object sender, EventArgs e)
         {
             if (IsPostBack) return;
             if (Request.Params[RESET] != null)
             {
                 if (Request.Params[PLAYER_ID] == null)
                 {
                     ResetCookie();
                     Response.Redirect(Constants.REQUEST_REGISTER_LINK_PAGE);
                     return;
                 }
                 Player user = Manager.FindPlayerById(Manager.ReversedId(Request.Params["id"]));
                 SetUserCookie(user);
                 this.PromptLb.Text = "Your device is registered as [" + user.Name + "]. Would you like to authorize someone to help you with reservations?";
                 return;
             }
             //
             String userId = Request.Params[PLAYER_ID];
             if (userId == null)
             {
                 if (Request.Cookies[Constants.PRIMARY_USER] == null || Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID] == null)
                 {
                     Response.Redirect(Constants.REQUEST_REGISTER_LINK_PAGE);
                     return;
                 }
                 String existingUserId = Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID];
                 Player existingUser = Manager.FindPlayerById(existingUserId);
                 this.PromptLb.Text = "Your device has registered as [" + existingUser.Name + "]. Would you like to authorize someone else to help you with reservations?";
                 return;
             }
             else
             {
                 userId = Manager.ReversedId(userId);
                 Player user = Manager.FindPlayerById(userId);
                 if (Request.Cookies[Constants.PRIMARY_USER] != null && Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID] != null)
                 {
                     String existingUserId = Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID];
                     Player existingUser = Manager.FindPlayerById(existingUserId);
                     this.PromptLb.Text = "Your device has already registered as [" + existingUser.Name + "].  Would you like to authorize someone to help you with reservations?";
                     return;
                 }
                 else
                 {
                     SetUserCookie(user);
                 }
                 this.PromptLb.Text = "Your device is registered as [" + user.Name + "]. Would you like to authorize someone to help you with reservations?";
                 return;
             }
         }


        private void ResetCookie()
        {
            HttpCookie appCookie = new HttpCookie(Constants.PRIMARY_USER);
            appCookie[Constants.USER_ID] = null;
            appCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(appCookie);
        }


        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }

        private void SetUserCookie(Player user)
        {
              HttpCookie appCookie = new HttpCookie(Constants.PRIMARY_USER);
            //appCookie.Domain = "volleyball.gear.host"; 
            appCookie[Constants.USER_ID] = user.Id;
            //appCookie[Constants.PASSCODE] = user.Passcode;
            appCookie.Expires = Manager.CookieExpire;
            Response.Cookies.Add(appCookie);
            user.DeviceLinked = true;
        }

        protected void ConfirmBtn_Click(object sender, EventArgs e)
        {
           if (Session[Constants.USER_ID] != null)
            {
                ResetCookie();
                String userId = Session[Constants.USER_ID].ToString();
                Player user = Manager.FindPlayerById(userId);
                SetUserCookie(user);
                Session[Constants.USER_ID] = null;
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                Response.Redirect(Constants.AUTHORIZE_USER_PAGE);
            }
        }

        protected void NoBtn_Click(object sender, EventArgs e)
        {
           Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
        }

    }
}