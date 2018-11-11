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
            this.PoolStatTable.Caption = Manager.Season + this.PoolStatTable.Caption;
            //  Calculate attendence statistics for games;
            int less12 = 0;
            int less14 = 0;
            int full = 0;
            int fullAndWaiting = 0;
            List<Game> fullGames = new List<Game>();
            foreach (Game game in CurrentPool.Games.FindAll(g=>g.Date < Manager.EastDateTimeToday))
            {
                int attended = game.AllPlayers.Items.FindAll(p=>p.Status== InOutNoshow.In).Count;
                if (attended < 12)
                {
                    less12++;
                }
                else if (attended < 14)
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
            TableRow row = new TableRow();
            //Total
            TableCell cell = new TableCell();
            cell.Text = CurrentPool.Games.FindAll(g=>g.Date < Manager.EastDateTimeToday).Count.ToString();
            row.Cells.Add(cell);
            //Less 12
            cell = new TableCell();
            cell.Text = less12.ToString();// + " / "+ less14.ToString();
            row.Cells.Add(cell);
            //Less 14
            cell = new TableCell();
            cell.Text = (less14).ToString();// + " / "+ less14.ToString();
            row.Cells.Add(cell);
            //Full no waiting
            cell = new TableCell();
            cell.Text = full.ToString();// +"/ " + full.ToString();
            row.Cells.Add(cell);
            //Full and waiting
            cell = new TableCell();
            cell.Text = fullAndWaiting.ToString();// +" / " + fullAndWaiting.ToString();
            row.Cells.Add(cell);
            this.PoolStatTable.Rows.Add(row);
              //Fill ful game table
              int index =1;
              foreach (Game game in CurrentPool.Games.FindAll(g=>g.Date.Date < Manager.EastDateTimeToday))
              {
                  row = new TableRow();
                  //Order
                  cell = new TableCell();
                  cell.Text = (index++).ToString();
                  row.Cells.Add(cell);
                  //Date
                  cell = new TableCell();
                  cell.Text = game.Date.ToShortDateString();
                  row.Cells.Add(cell);
                  //reserved
                  cell = new TableCell();
                  cell.Text = game.AllPlayers.Items.FindAll(p => p.Status == InOutNoshow.In).Count.ToString() ;
                  row.Cells.Add(cell);
                  //Intern
                  cell = new TableCell();
                  int intern = 0;
                  if (CurrentPool.IsLowPool)
                  {
                      Pool highPool = Manager.FindSameDayPool(CurrentPool);
                      intern = highPool.FindGameByDate(game.Date).Dropins.Items.FindAll(d => d.Status == InOutNoshow.In && d.IsCoop).Count;
                  }
                  cell.Text = intern.ToString();
                  row.Cells.Add(cell);
                  //Waiting list
                  cell = new TableCell();
                  cell.Text = game.WaitingList.Count.ToString(); ;
                  row.Cells.Add(cell);
                  //Factor
                  cell = new TableCell();
                  cell.Text = game.Factor.ToString(); ;
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