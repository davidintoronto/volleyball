using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Confirm : BasePage
    {
         private const String POOL = "p";
         private const String GAME_DATE = "date";
         private const String PLAYER_ID = "id";
         protected void Page_Load(object sender, EventArgs e)
         {
             if (IsPostBack) return;
             if (Request.Params[GAME_DATE] != null && Request.Params[POOL] != null && Request.Params[PLAYER_ID] != null)
             {
                 Pool pool = Manager.FindPoolByName(Request.Params[POOL]);
                 Session[Constants.POOL] = pool;
                 DateTime gameDate = DateTime.Parse(Request.Params[GAME_DATE]);
                 Session[Constants.GAME_DATE] = gameDate;
                 Session[Constants.CURRENT_PLAYER_ID] = Request.Params[PLAYER_ID];
                 if (!IsReservationLocked(gameDate))
                 {
                     this.PromptLb.Text = "One dropin spot is available in pool " + pool.Name + ". It is kind of late now, would you like to take it?";
                     return;
                 }
             }
             this.ConfirmBtn.Visible = false;
             this.NoBtn.Visible = false;
             this.PromptLb.Text = "Too late!";
         }

        protected void ConfirmBtn_Click(object sender, EventArgs e)
        {/*
            Game game = CurrentPool.FindGameByDate(TargetGameDate);
             String playerId = Session[Constants.CURRENT_PLAYER_ID].ToString();
            Waiting waiting = game.WaitingList.FindByPlayerId(playerId);
            Player player = Manager.FindPlayerById(playerId);
            ReserveSpot(CurrentPool, game, player);
            theGame.WaitingList.Remove(playerId);
            Manager.AddReservationNotifyWechatMessage(playerId, null, Constants.WAITING_TO_RESERVED, thePool, thePool, theGame.Date);
            LogHistory log = CreateLog(Manager.EastDateTimeNow, theGame.Date, GetUserIP(), thePool.Name, Manager.FindPlayerById(playerId).Name, "Reserved", "Admin");
            Manager.Logs.Add(log);
            theGame.WaitingList.Remove(playerId);
            //Cancel the member spot in another pool on same day
            Pool sameDayPool = Manager.Pools.Find(pool => pool.Name != thePool.Name && pool.DayOfWeek == thePool.DayOfWeek);
            */
        }

        protected void NoBtn_Click(object sender, EventArgs e)
        {
           Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
        }

    }
}