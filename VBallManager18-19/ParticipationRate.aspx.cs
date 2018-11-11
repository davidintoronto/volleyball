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
            int order = 1;
            foreach (Stats stats in CalculateStats(pool))
            {
                FillList(order++, stats);
            }
            this.StatsTable.Caption = "Pool " + pool.Name + " Attendance Rate";
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
        private void FillList(int order, Stats stats)
        {
            TableRow row = new TableRow();
            //Order
            TableCell cell = new TableCell();
            cell.Text = order.ToString();
            row.Cells.Add(cell);
            //Name
            cell = new TableCell();
            cell.Text = stats.Player.Name;

            row.Cells.Add(cell);
            //PlayerCount
            cell = new TableCell();
            cell.Text = stats.PlayedCount.ToString(); ;
            row.Cells.Add(cell);
            //Bonus
            cell = new TableCell();
            cell.Text = stats.FactorBonus.ToString(); ;
            row.Cells.Add(cell);
            //total
            cell = new TableCell();
            cell.Text = stats.Total.ToString(); ;
            row.Cells.Add(cell);
            this.StatsTable.Rows.Add(row);
        }




        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }

        private IEnumerable<Stats> CalculateStats(Pool pool)
        {
            List<Stats> statsList = new List<Stats>();
            List<Player> players = new List<Player>();
            foreach (Person person in pool.AllPlayers.Items)
            {
                Player player = Manager.FindPlayerById(person.PlayerId);
                if (player.IsActive && player.IsRegisterdMember && !pool.Dropins.Items.Exists(dropin => dropin.PlayerId == person.PlayerId && dropin.IsCoop))
                {
                    Stats stats = new Stats();
                    stats.Player = player;
                    stats.PlayedCount = pool.Games.FindAll(g => g.Date >= Manager.AttendRateStartDate && g.Date <Manager.EastDateTimeToday.Date && g.AllPlayers.Items.Find(p => p.PlayerId == player.Id && p.Status == InOutNoshow.In) != null).Count;
                    stats.FactorBonus = CalculateFactorBonus(pool, player);
                    stats.Total = stats.PlayedCount + stats.FactorBonus;
                    statsList.Add(stats);
                }
            }
            return statsList.OrderByDescending(stats => stats.Total);
        }

        private decimal CalculateFactorBonus(Pool thePool, Player player)
        {
            decimal sum = 0;
            if (thePool.StatsType == StatsTypes.None.ToString())
                return sum;
            if (thePool.StatsType == StatsTypes.Week.ToString())
            {
                Pool sameLevelPool = Manager.Pools.Find(p => p.DayOfWeek != thePool.DayOfWeek && p.IsLowPool == thePool.IsLowPool);
                foreach (Game game in sameLevelPool.Games)
                {
                    //Skip the games that are earlier than specified date or later than today
                    if (game.Date < Manager.AttendRateStartDate.Date || game.Date.Date >= Manager.EastDateTimeToday.Date) continue;
                    if (game.AllPlayers.Items.Exists(member => member.PlayerId == player.Id && member.Status == InOutNoshow.In))
                    {
                        sum = sum + game.Factor;
                    }
                    else
                    {
                        Pool otherPoolOnSameDay = Manager.Pools.Find(p => p.IsLowPool != sameLevelPool.IsLowPool && p.DayOfWeek == sameLevelPool.DayOfWeek);
                        Game theGame = otherPoolOnSameDay.FindGameByDate(game.Date);
                        if (theGame.AllPlayers.Items.Exists(member => member.PlayerId == player.Id && member.Status == InOutNoshow.In))
                        {
                            sum = sum + game.Factor;
                        }
                    }
                }
            }
            return sum;
        }

        public class Stats
        {
            private Player player;
            private int playedCount;
            private decimal factorBonus;
            private decimal total;

            public decimal Total
            {
                get { return total; }
                set { total = value; }
            }

            public Player Player
            {
                get { return player; }
                set { player = value; }
            }

            public int PlayedCount
            {
                get { return playedCount; }
                set { playedCount = value; }
            }

            public decimal FactorBonus
            {
                get { return factorBonus; }
                set { factorBonus = value; }
            }
        }
    }
}