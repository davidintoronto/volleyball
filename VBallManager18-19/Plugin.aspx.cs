using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Plugin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if (Request.Cookies[Constants.PRIMARY_USER] != null)
            {
                String existingUserId = Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID];
                Player user = Manager.FindPlayerById(existingUserId);
                if (user != null)
                {                    
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
        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
        }

        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            Player currentUser = Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]);
            if (this.MonthDDL.SelectedValue.Length > 0 && this.DayDDL.SelectedValue.Length > 0)
            {
                currentUser.Birthday = MonthDDL.SelectedValue + "/" + this.DayDDL.SelectedValue;
                LogHistory log = new LogHistory(Manager.EastDateTimeNow, Manager.EastDateTimeToday, currentUser.Name, "", currentUser.Name, "Birthday: " + currentUser.Birthday, currentUser.Name);
                Manager.Logs.Add(log);
                DataAccess.Save(Manager);
                Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
            }
        }

    
        protected void GotoNextBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
        }
    }
}