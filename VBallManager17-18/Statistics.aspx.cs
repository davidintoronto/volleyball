﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Statistics : System.Web.UI.Page
    {
        private int count = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            IEnumerable<Player> playerQuery = Manager.Players.OrderBy(player => player.Name);
            foreach (Player player in playerQuery)
            {
                if (!player.Suspend)
                {
                    player.MondayPlayedCount = GetPlayedCount(player, DayOfWeek.Monday);
                    player.FridayPlayedCount = GetPlayedCount(player, DayOfWeek.Friday);
                    player.TotalPlayedCount = player.MondayPlayedCount + player.FridayPlayedCount;
                }
            }
            playerQuery = Manager.Players.OrderByDescending(player => player.TotalPlayedCount);
            int order = 1;
           foreach (Player player in playerQuery)
            {
                if (!player.Suspend && player.TotalPlayedCount>5)
                {
                    TableRow row = new TableRow();
                    TableCell orderCell = new TableCell();
                    orderCell.Text = (order++).ToString();
                    row.Cells.Add(orderCell);
                    //
                     TableCell nameCell = new TableCell();
                    nameCell.Text = player.Name;
                    row.Cells.Add(nameCell);
                    //
                    TableCell mondayCell = new TableCell();
                    mondayCell.Text = player.MondayPlayedCount.ToString();
                    row.Cells.Add(mondayCell);
                    //
                    TableCell fridayCell = new TableCell();
                    fridayCell.Text = player.FridayPlayedCount.ToString();
                    row.Cells.Add(fridayCell);
                    //
                    TableCell totalCell = new TableCell();
                    totalCell.Text = player.TotalPlayedCount.ToString();
                    row.Cells.Add(totalCell);
                    this.StatTable.Rows.Add(row);
                }
            }
                int corder = 1;
                int dorder = 1;
            playerQuery = Manager.Players.OrderByDescending(player => player.FridayPlayedCount);
            foreach (Player player in playerQuery)
            {
                if (!player.Suspend && player.TotalPlayedCount > 5)
                {
                    TableRow row = new TableRow();
                    TableCell orderCell = new TableCell();
                    row.Cells.Add(orderCell);
                    //
                    TableCell nameCell = new TableCell();
                    nameCell.Text = player.Name;
                    row.Cells.Add(nameCell);
                     //
                    TableCell fridayCell = new TableCell();
                    fridayCell.Text = player.FridayPlayedCount.ToString();
                    row.Cells.Add(fridayCell);
                    //
                    if (Manager.FindPoolByName("D").Members.Exists(member => member.Id == player.Id) || Manager.FindPoolByName("D").Dropins.Exists(dropin => dropin.Id == player.Id))
                    {
                        orderCell.Text = (dorder++).ToString();
                       this.DPoolTable.Rows.Add(row);
 
                    }
                    else if (Manager.FindPoolByName("C").Members.Exists(member => member.Id == player.Id) || Manager.FindPoolByName("C").Dropins.Exists(dropin => dropin.Id == player.Id))
                    {
                        orderCell.Text = (corder++).ToString();
                        this.CPoolTable.Rows.Add(row);
                    }
                }
            }
            corder = 1;
            dorder = 1;
            playerQuery = Manager.Players.OrderByDescending(player => player.TotalPlayedCount);
            foreach (Player player in playerQuery)
            {
                if (!player.Suspend && player.TotalPlayedCount > 5)
                {
                    TableRow row = new TableRow();
                    TableCell orderCell = new TableCell();
                    row.Cells.Add(orderCell);
                    //
                    TableCell nameCell = new TableCell();
                    nameCell.Text = player.Name;
                    row.Cells.Add(nameCell);
                    //
                    TableCell fridayCell = new TableCell();
                    fridayCell.Text = player.TotalPlayedCount.ToString();
                    row.Cells.Add(fridayCell);
                    //
                    if (Manager.FindPoolByName("D").Members.Exists(member => member.Id == player.Id) || Manager.FindPoolByName("D").Dropins.Exists(dropin => dropin.Id == player.Id))
                    {
                        orderCell.Text = (dorder++).ToString();
                        this.DPoolTotalTable.Rows.Add(row);

                    }
                    else if (Manager.FindPoolByName("C").Members.Exists(member => member.Id == player.Id) || Manager.FindPoolByName("C").Dropins.Exists(dropin => dropin.Id == player.Id))
                    {
                        orderCell.Text = (corder++).ToString();
                        this.CPoolTotalTable.Rows.Add(row);
                    }
                }
            }
        }


        private int GetPlayedCount(Player player, DayOfWeek day)
        {
            int playedCount = 0;
            foreach (Pool pool in Manager.Pools)
            {
                if (pool.DayOfWeek == day)
                {
                    if (pool.Members.Exists(member => member.Id == player.Id))
                    {
                        foreach (Game game in pool.Games)
                        {
                            if (game.Absences.Exists(player.Id))
                            {
                                continue;
                            }
                            playedCount++;
                        }
                    }
                    else
                    {
                        foreach (Game game in pool.Games)
                        {
                            if (game.Pickups.Exists(player.Id))
                            {
                                playedCount++;
                            }
                        }
                    }
                }
            }
            return playedCount;
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