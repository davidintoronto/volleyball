using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

namespace VballManager
{
    public partial class Default : System.Web.UI.Page
    {

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

        private Player CurrentUser
        {
            get
            {
                return (Player)Session[Constants.CURRENT_USER];
            }
            set { }
        }

        private DateTime ComingGameDate
        {
            get
            {
                return (DateTime)Session[Constants.GAME_DATE];

            }
            set { }
        }

        private bool IsPermitted(Actions action, Player player)
        {
            if (Manager.ActionPermitted(action, CurrentUser.Role) || CurrentUser.Id == player.Id || player.AuthorizedUsers.Contains(CurrentUser.Id))
            {
                return true;
            }
            return false;
        }

        private String GetOperatorId()
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null && Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]) != null)
            {
                return Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]).Id;
            }
            return null;
        }
        private LogHistory CreateLog(DateTime date, DateTime gameDate, String userInfo, String poolName, String playerName, String type)
        {
            if (Request.Cookies[Constants.PRIMARY_USER] != null && Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]) != null)
            {
                return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.USER_ID]).Name);
            }
            return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, "Unknown");
        }

        private LogHistory CreateLog(DateTime date, DateTime gameDate, String userInfo, String poolName, String playerName, String type, String operater)
        {
            return new LogHistory(date, gameDate, userInfo, poolName, playerName, type, operater);
        }
    }
}