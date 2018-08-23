using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace VballManager
{
    public partial class AdminBase : System.Web.UI.Page
    {
        protected bool IsSuperAdmin()
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
            TextBox passcodeTb = (TextBox)Master.FindControl("PasscodeTb");
            if (Manager.SuperAdmin != passcodeTb.Text)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong passcode! Re-enter your passcode and try again')", true);
                return false;
            }
            Session[Constants.SUPER_ADMIN] = passcodeTb.Text;
            return true;
        }


        protected VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }

        protected Pool CurrentPool
        {
            get
            {
                String poolName = (String)Session[Constants.POOL];
                return Manager.FindPoolByName(poolName);
            }
            set { }
        }
    }
}