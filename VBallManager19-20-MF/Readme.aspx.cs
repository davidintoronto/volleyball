using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Readme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ReadmeLb.Text = Manager.Readme.Replace(Environment.NewLine, "<br/>");
        }
        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }
        private Pool CurrentPool
        {
            get
            {
                String poolName = (String)Session[Constants.POOL];
                return Manager.FindPoolByName(poolName);
            }
            set { }
        }
 
        protected void BackBtn_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(Constants.RESERVE_PAGE + "?Pool=" + CurrentPool.Name);
        }


    }
}