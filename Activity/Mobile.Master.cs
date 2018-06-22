using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reservation
{
    public partial class MobileMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Page.Title = Reservations.Title;
            //this.TitleLabel.Text = Reservations.Title;
            if (!Request.UserAgent.Contains("MicroMessenger"))
            {
                this.ClosePanel.Visible = false;
            }
        }
        private Reservation Reservations
        {
            get
            {
                return (Reservation)Application[Constants.RESERVATION];

            }
            set { }
        }
    }
}
