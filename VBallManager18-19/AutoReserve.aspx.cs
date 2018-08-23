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
            String hourString = this.Request.Params[Constants.HOUR];
            int hour = int.Parse(hourString);
            foreach (Pool pool in Manager.Pools)
            {
                if (pool.AutoCoopReserve && hour == pool.ReservHourForCoop)
                {
                    DateTime comingGameDate = FindComingGameDate(pool);
                    if (Manager.EastDateTimeToday.AddDays(pool.DaysToReserve).Date == comingGameDate.Date)
                    {
                        AutoReserveCoopPlayers(pool, comingGameDate);
                    }
                }
            }
        }

        private DateTime FindComingGameDate(Pool pool)
        {
            DateTime gameDate = Manager.EastDateTimeToday;
            Game targetGame = pool.Games.OrderBy(game => game.Date).ToList<Game>().Find(game => game.Date >= gameDate);
            if (targetGame != null)
            {
                return targetGame.Date;
            }
            return DateTime.MaxValue;
        }

    }
}