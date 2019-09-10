using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Register : System.Web.UI.Page
    {
        private static String USER_ID = "id";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params[USER_ID] != null)
            {

                String playerId = Manager.ReversedId(Request.Params[USER_ID]);
                Player user = Manager.FindPlayerById(playerId);
                if (user != null)
                {
                    HttpCookie appCookie = new HttpCookie(Constants.PRIMARY_USER);
                    //appCookie.Domain = "volleyball.gear.host"; 
                    appCookie.Value = playerId;
                    appCookie.Expires = Manager.CookieExpire;
                    Response.Cookies.Add(appCookie);
                    user.DeviceLinked = true;
                    DataAccess.Save(Manager);
                    this.ResultLb.Text = "Registered!";
                   
                }
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