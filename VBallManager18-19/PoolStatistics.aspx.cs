using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class PoolStatistics : System.Web.UI.Page
    {
          protected void Page_Load(object sender, EventArgs e)
        {
            String poolName = this.Request.Params[Constants.POOL];

            if (poolName != null)
            {
                Session[Constants.POOL] = poolName;
                if (CurrentPool == null) return;
            }
            else
            {
                return;
            }
            //  Calculate attendence statistics for games;
            int less12 = 0;
            int less12WithoutCoop = 0;
            int less14 = 0;
            int less14WithoutCoop=0;
            int full = 0;
            int fullWithoutCoop = 0;
            int fullAndWaiting = 0;
            int fullAndWaitingWithoutCoop = 0;
            List<Game> fullGames = new List<Game>();
            foreach (Game game in CurrentPool.Games)
            {
                int attended = game.Members.Items.FindAll(member => member.Status != InOutNoshow.Out).Count + game.Dropins.Items.FindAll(dropin => dropin.Status != InOutNoshow.Out).Count;
                if (attended < 12)
                {
                    less12++;
                }
                if (attended < 14)
                {
                    less14++;
                }
                else
                {
                    full++;
                    if (game.WaitingList.Count > 0)
                    {
                        fullAndWaiting++;
                       // this.PoolStatTable.Caption = this.PoolStatTable.Caption + "|" + game.Date.ToShortDateString();
                    }
                }
            }
            foreach (Game game in CurrentPool.Games)
            {
                int attendedWithoutCoop = game.Members.Items.FindAll(member => member.Status != InOutNoshow.Out).Count + game.Dropins.Items.FindAll(dropin => !dropin.IsCoop && dropin.Status != InOutNoshow.Out).Count;
                if (attendedWithoutCoop < 12)
                {
                    less12WithoutCoop++;
                }

                if (attendedWithoutCoop < 14)
                {
                    less14WithoutCoop++;
                }
                else
                {
                    fullWithoutCoop++;
                    fullGames.Add(game);
                    bool coopWaiting = false;
                     foreach (Waiting waiting in game.WaitingList.Items)
                    {
                        if (game.Dropins.Items.Find(dropin => dropin.PlayerId == waiting.PlayerId).IsCoop)
                        {
                            coopWaiting = true;
                            break;
                        }
                    }
                     if (!coopWaiting) fullAndWaitingWithoutCoop++;
                }
            }
            TableRow row = new TableRow();
            //Total
            TableCell cell = new TableCell();
            cell.Text = CurrentPool.Games.Count.ToString();
            row.Cells.Add(cell);
            //Less 12
            cell = new TableCell();
            cell.Text = less12WithoutCoop.ToString();// + " / "+ less14.ToString();
            row.Cells.Add(cell);
            //Less 14
            cell = new TableCell();
            cell.Text = (less14WithoutCoop-less12WithoutCoop).ToString();// + " / "+ less14.ToString();
            row.Cells.Add(cell);
            //Full no waiting
            cell = new TableCell();
            cell.Text = fullWithoutCoop.ToString();// +"/ " + full.ToString();
            row.Cells.Add(cell);
            //Full and waiting
            cell = new TableCell();
            cell.Text = fullAndWaitingWithoutCoop.ToString();// +" / " + fullAndWaiting.ToString();
            row.Cells.Add(cell);
            this.PoolStatTable.Rows.Add(row);
              //Fill ful game table
              int index =1;
              foreach (Game fullGame in fullGames)
              {
                  row = new TableRow();
                  //Order
                  cell = new TableCell();
                  cell.Text = (index++).ToString();
                  row.Cells.Add(cell);
                  //Date
                  cell = new TableCell();
                  cell.Text = fullGame.Date.ToShortDateString();
                  row.Cells.Add(cell);
                  //Waiting list
                  cell = new TableCell();
                  String waitingListNames = null;
                  foreach (Waiting waiting in fullGame.WaitingList.Items)
                  {
                      Player player = Manager.FindPlayerById(waiting.PlayerId);
                      waitingListNames = waitingListNames == null ? player.Name : waitingListNames + "," + player.Name;
                  }
                  cell.Text = waitingListNames;
                  row.Cells.Add(cell);
                  this.FullTable.Rows.Add(row);
              }

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
   
        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }

    }
}