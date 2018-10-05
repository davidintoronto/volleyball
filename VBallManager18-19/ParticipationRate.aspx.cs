using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class ParticipationRate : System.Web.UI.Page
    {
        private int count = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            String poolId = this.Request.Params[Constants.POOL_ID];
            String poolName = this.Request.Params[Constants.POOL];

            if (poolId != null)
            {
                poolName = Manager.FindPoolById(poolId).Name;
                Session[Constants.POOL] = poolName;
            }
            else if (poolName != null)
            {
                Session[Constants.POOL] = poolName;
            }
            if (Session[Constants.POOL] == null)
            {
                return;
            }
            //   CreateTableHead();
            Pool pool = Manager.FindPoolByName(poolName);
            List<Player> players = new List<Player>();
            foreach (Person person in pool.AllPlayers.Items)
            {
                Player player = Manager.FindPlayerById(person.PlayerId);
                if (player.IsActive && player.IsRegisterdMember && !pool.Dropins.Items.Exists(dropin=>dropin.PlayerId==person.PlayerId && dropin.IsCoop))
                {
                    player.TotalPlayedCount = CalculatePlayedStats(pool, player);
                    players.Add(player);
                }
            }
            //Statistic played count
            IEnumerable<Player> sortedPlayers = players.OrderByDescending(p => p.TotalPlayedCount);
            int order = 1;
            foreach (Player player in sortedPlayers)
            {
                FillPreRegister(order++, player);
            }
             this.SurveyTable.Caption = "Pool " + pool.Name + " Attendance Rate";
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
        private void FillPreRegister(int order, Player player)
        {
             TableRow row = new TableRow();
            //Order
            TableCell cell = new TableCell();
            cell.Text = order.ToString();
            row.Cells.Add(cell);
            //Name
            cell = new TableCell();
            cell.Text = player.Name;
 
            row.Cells.Add(cell);
             //PlayerCount
            cell = new TableCell();
            cell.Text = player.TotalPlayedCount.ToString(); ;
            row.Cells.Add(cell);
            this.SurveyTable.Rows.Add(row);
        }
 
  

  
        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }

        private int CalculatePlayedStats(Pool thePool, Player player)
        {
            int playedCount = 0;
            if (thePool.StatsType == StatsTypes.None.ToString())
                return playedCount;
            foreach (Pool pool in Manager.Pools)
            {
                if (thePool.StatsType == StatsTypes.Week.ToString() || pool.DayOfWeek == thePool.DayOfWeek)
                {
                    foreach (Game game in pool.Games)
                    {
                        if (game.Date.Date < Manager.EastDateTimeToday.Date && (game.Members.Items.Exists(member=> member.PlayerId == player.Id && member.Status == InOutNoshow.In) || game.Dropins.Items.Exists(pickup=>pickup.PlayerId == player.Id && pickup.Status == InOutNoshow.In)))
                        {
                            playedCount++;
                        }
                    }
                }
            }
            return playedCount;
        } 
    }
}