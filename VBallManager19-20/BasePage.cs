using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace VballManager
{
    public class BasePage : System.Web.UI.Page
    {
        private ActionHandler handler;

        public ActionHandler Handler
        {
            get { return handler; }
            set { handler = value; }
        }

        protected void ReloadManager()
        {
             //Application[Constants.DATA] = DataAccess.LoadReservation();
       }
        protected void InitializeActionHandler()
        {
            handler = new ActionHandler();
            handler.Manager = Manager;
            handler.CurrentPool = CurrentPool;
            handler.CurrentUser = CurrentUser;
            handler.TargetGameDate = TargetGameDate;
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

        protected Player CurrentUser
        {
            get
            {
                return (Player)Session[Constants.CURRENT_USER];
            }
            set { }
        }

        protected DateTime TargetGameDate
        {
            get
            {
                if (Session[Constants.GAME_DATE] == null) return DateTime.MinValue;
                return (DateTime)Session[Constants.GAME_DATE];

            }
            set { }
        }

        protected String GetOperatorId()
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null && Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]) != null)
            {
                return Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]).Id;
            }
            return null;
        }
    }
}