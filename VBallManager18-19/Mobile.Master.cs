using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class MobileMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string title = GetTitleOfCurrentPool();
            if (String.IsNullOrEmpty(Page.Title))
            {
            Page.Title = title;
            }
            this.TitleLabel.Text = title;
            //this.ClosePanel.Visible = false;
            if (Request.UserAgent.Contains("MicroMessenger"))
            {
                this.ClosePanel.Visible = true;
            }
        }

        private VolleyballClub Reservations
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }
        private String GetTitleOfCurrentPool()
        {
           /* String poolName = this.Request.Params[Constants.POOL];
            if (poolName != null)
            {
                Pool currentPool = Reservations.FindPoolByName(poolName);
                if (currentPool != null)
                {
                    return currentPool.Title;
                }
            }*/
            return "Hitmen Volleyball Club";

        }
    }
}
