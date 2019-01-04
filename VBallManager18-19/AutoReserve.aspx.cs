using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    // Auto reserve for coop
    public partial class AutoReserve : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ReloadManager();
            InitializeActionHandler();
            Handler.AutoMoveCoop();
            Handler.AutoCancelUnconfirmedReservations();
        }

    }
}