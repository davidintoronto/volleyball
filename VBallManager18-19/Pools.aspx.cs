using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Pools : AdminBase
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            Application[Constants.DATA] = DataAccess.LoadReservation();
            if (!IsPostBack)
            {
                //
                if (null != Session[Constants.SUPER_ADMIN])
                {
                    ((TextBox)Master.FindControl("PasscodeTb")).Text = Session[Constants.SUPER_ADMIN].ToString();
                }
                this.DeletePoolBtn.OnClientClick = "if ( !confirm('Are you sure to delete this pool?')) return false;";
                this.DeleteGameBtn.OnClientClick = "if ( !confirm('Are you sure to delete this game?')) return false;";
                //Bind pool list
                int selectPoolIndex = this.PoolListbox.SelectedIndex;
                this.PoolListbox.DataSource = Manager.Pools;
                this.PoolListbox.DataTextField = "Name";
                this.PoolListbox.DataValueField = "Id";
                this.PoolListbox.DataBind();
                this.PoolListbox.SelectedIndex = selectPoolIndex;
                //Bind player list
                int selectPlayerIndex = this.PlayerListbox.SelectedIndex;
                this.PlayerListbox.DataSource = Manager.Players.OrderBy(player => player.Name);
                this.PlayerListbox.DataTextField = "Name";
                this.PlayerListbox.DataValueField = "Id";
                this.PlayerListbox.DataBind();
                this.PlayerListbox.SelectedIndex = selectPlayerIndex;
                //
           }
      }

       protected void AddPoolBtn_Click(object sender, EventArgs e)
       {
           if (!IsSuperAdmin() || this.PoolNameTb.Text == "")
           {
               return;
           }
           if (Manager.FindPoolByName(this.PoolNameTb.Text) != null)
           {
               ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Pool name already exists')", true);
               return;
           }
           Pool pool = new Pool();
           pool.Name = this.PoolNameTb.Text;
           Manager.Pools.Add(pool);
           this.PoolListbox.DataSource = Manager.Pools;
           this.PoolListbox.DataBind();
           DataAccess.Save(Manager);
           //Response.Redirect(Request.RawUrl);
       }

       protected void UpdatePoolBtn_Click(object sender, EventArgs e)
       {
           if (!IsSuperAdmin() || this.PoolNameTb.Text == "")
           {
               return;
           }
           Pool pool = Manager.FindPoolById(this.PoolListbox.SelectedValue);
           pool.Name = this.PoolNameTb.Text;
           this.PoolListbox.DataSource = Manager.Pools;
           this.PoolListbox.DataBind();
           DataAccess.Save(Manager);

       }

       protected void AddMemberBtn_Click(object sender, EventArgs e)
       {
           if (!IsSuperAdmin())
           {
               return;
           }
           Session[Constants.ADD_PLAYER_POOL] = Constants.ACTION_MEMEBER_ADD;
           this.PlayerListbox.ClearSelection();
           this.PlayerListPopup.Show();
       }
       protected void AddDropinBtn_Click(object sender, EventArgs e)
       {
           if (!IsSuperAdmin())
           {
               return;
           }
           Session[Constants.ADD_PLAYER_POOL] = Constants.ACTION_DROPIN_ADD;
           this.PlayerListbox.ClearSelection();
           this.PlayerListPopup.Show();
       }

       protected void AddPlayers_Click(object sender, EventArgs e)
       {
           if (Session[Constants.ADD_PLAYER_POOL].ToString() == Constants.ACTION_MEMEBER_ADD)
           {
               foreach (ListItem item in this.PlayerListbox.Items)
               {
                   if (item.Selected)
                   {
                       if (CurrentPool.Members.Exists(item.Value) || CurrentPool.Dropins.Exists(item.Value))
                       {
                           ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Error! " + item.Text + " has already added as primary member or dropin')", true);
                           return;
                       }
                       else
                       {
                           CurrentPool.Members.Add(new Member(item.Value));
                           //Add to reserved list for the future games
                           foreach (Game game in CurrentPool.Games)
                           {
                               if (game.Date >= DateTime.Today)
                               {
                                   game.Members.Add(new Attendee(item.Value, InOutNoshow.In));
                               }
                           }
                           //Change the player to club registered member and create membership fee it is  club member mode
                           Player player = Manager.FindPlayerById(item.Value);
                           if (Manager.ClubMemberMode)
                           {
                               if (!player.IsRegisterdMember)
                               {
                                   player.IsRegisterdMember = true;
                                   Fee fee = new Fee(Fee.FEETYPE_CLUB_MEMBERSHIP, Manager.RegisterMembeshipFee);
                                   fee.FeeType = FeeTypeEnum.Membership.ToString();
                                   fee.Date = DateTime.Today;
                                   player.Fees.Add(fee);
                               }
                           }
                           else
                           {
                               Fee fee = new Fee(CurrentPool.MembershipFee);
                               fee.FeeType = FeeTypeEnum.Membership.ToString();
                               fee.FeeDesc = String.Format(Fee.FEETYPE_MEMBERSHIP, CurrentPool.Name);
                               fee.Date = DateTime.Today;
                               player.Fees.Add(fee);
                           }
                           this.MemberListbox.DataSource = GetPlayers(CurrentPool.Members);
                           this.MemberListbox.DataBind();
                           DataAccess.Save(Manager);
                           // Response.Redirect(Request.RawUrl);
                       }
                   }
               }
           }
           else if (Session[Constants.ADD_PLAYER_POOL].ToString() == Constants.ACTION_DROPIN_ADD)
           {
               foreach (ListItem item in this.PlayerListbox.Items)
               {
                   if (item.Selected)
                   {
                       if (CurrentPool.Members.Exists(item.Value) || CurrentPool.Dropins.Exists(item.Value))
                       {
                           ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Error! " + item.Text + " has already added as primary member or dropin')", true);
                           return;
                       }
                       CurrentPool.Dropins.Add(new Dropin(item.Value));
                       //Add into game dropin list for the future games
                       foreach (Game game in CurrentPool.Games)
                       {
                           if (game.Date >= DateTime.Today)
                           {
                               game.Dropins.Add(new Attendee(item.Value));
                           }
                       }
                       this.DropinListbox.DataSource = GetPlayers(CurrentPool.Dropins);
                       this.DropinListbox.DataBind();
                       DataAccess.Save(Manager);
                       // Response.Redirect(Request.RawUrl);
                   }
               }
           }
       }
 
        protected void RemoveMemberBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin())
            {
                return;
            }
            String playerId = this.MemberListbox.SelectedValue;
            CurrentPool.RemoveMember(playerId);
            //Remove from reserved and absence list for the future games
            foreach (Game game in CurrentPool.Games)
            {
                if (game.Date >= DateTime.Today)
                {
                    if (game.Members.Exists(playerId))
                    {
                        game.Members.Remove(playerId);
                    }
                    if (game.WaitingList.Exists(playerId))
                    {
                        game.WaitingList.Remove(playerId);
                    }
                }
            }

            //Save
            DataAccess.Save(Manager);
            this.MemberListbox.DataSource = GetPlayers(CurrentPool.Members);
            this.MemberListbox.DataBind();
            // Response.Redirect(Request.RawUrl);
        }
        protected void RemoveDropinBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin())
            {
                return;
            }
            String playerId = this.DropinListbox.SelectedValue;
            CurrentPool.RemoveDropin(playerId);
            foreach (Game game in CurrentPool.Games)
            {
                if (game.Date >= DateTime.Today)
                {
                    if (game.Dropins.Exists(playerId))
                    {
                        game.Dropins.Remove(playerId);
                    }
                    if (game.WaitingList.Exists(playerId))
                    {
                        game.WaitingList.Remove(playerId);
                    }
                }
            }
            //Save
            DataAccess.Save(Manager);
            this.DropinListbox.DataSource = GetPlayers(CurrentPool.Dropins);
            this.DropinListbox.DataBind();
            //Response.Redirect(Request.RawUrl);
        }

        protected void PoolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.PoolListbox.SelectedIndex ==-1)
            {
                return;
            }
            this.PoolDetailPanel.Visible = true;
            Pool pool = Manager.FindPoolById(this.PoolListbox.SelectedValue);
            Session[Constants.POOL] = pool.Name;
            PoolNameTb.Text = pool.Name;
            this.TitleTb.Text = pool.Title;
            this.ScheduleTimeTb.Text = pool.GameScheduleTime;
            this.DayOfWeekDl.SelectedValue = pool.DayOfWeek.ToString();
            this.MessageTb.Text = pool.MessageBoard;
            this.MaxPlayers.Text = pool.MaximumPlayerNumber.ToString();
            this.MemberShipFeeTb.Text = pool.MembershipFee.ToString();
            this.AllowAddingDropinCb.Checked = pool.AllowAddNewDropinName;
            this.CoopReserveHourTb.Text = pool.ReservHourForCoop.ToString();
            this.CoopLessThanPlayersTb.Text = pool.LessThanPayersForCoop.ToString();
            this.DaysToReserveForMemberTb.Text = pool.DaysToReserve4Member.ToString();
            this.DaysToReserveTb.Text = pool.DaysToReserve.ToString();
            this.AutoCoopReserveCb.Checked = pool.AutoCoopReserve;
            this.MaxCoopPlayerTb.Text = pool.MaxCoopPlayers.ToString();
            this.StatsTypeDdl.SelectedValue = pool.StatsType;
            this.WechatGroupName.Text = pool.WechatGroupName;
            //Bind memeber list
            int selectMemberIndex = this.MemberListbox.SelectedIndex;
            this.MemberListbox.DataSource = GetPlayers(pool.Members);
            this.MemberListbox.DataTextField = "Name";
            this.MemberListbox.DataValueField = "Id";
            this.MemberListbox.DataBind();
            //this.MemberListbox.SelectedIndex = selectMemberIndex;
            //Bind dropin list
            int selectDropinIndex = this.DropinListbox.SelectedIndex;
            this.DropinListbox.DataSource = GetPlayers(pool.Dropins);
            this.DropinListbox.DataTextField = "Name";
            this.DropinListbox.DataValueField = "Id";
            this.DropinListbox.DataBind();
            //this.DropinListbox.SelectedIndex = selectDropinIndex;
             //Bind Date list
            int selectGameIndex = this.GameListbox.SelectedIndex;
            this.GameListbox.DataSource = Manager.FindPoolByName(this.PoolListbox.SelectedItem.Text).Games;
            this.GameListbox.DataTextField = "Date";
            this.GameListbox.DataTextFormatString = "{0:d}";
            this.GameListbox.DataBind();
            //this.GameListbox.SelectedIndex = selectGameIndex;
            if (this.PoolListbox.SelectedIndex >= 0)
            {
                this.PoolPanel.Enabled = true;
                this.PoolDetailPanel.Enabled = true;
            }

       }
          protected void GameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameDateTb.Text = this.GameListbox.SelectedItem.Text;
        }

        protected void AddGameBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.GameDateTb.Text == "")
            {
                return;
            }
            String[] gameDates = GameDateTb.Text.Split(',');
            //Parse Date string
            foreach (String gameDate in gameDates)
            {
                try
                {
                    DateTime date = DateTime.Parse(gameDate);
                    if (CurrentPool.GameExists(date))
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Game on " + date.ToShortDateString() + " is already added!')", true);
                        return;
                    }
                }
                catch (Exception)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong game date format!  Fix it and try again')", true);
                    return;
                }
            }
            foreach (String gameDate in gameDates)
            {
                DateTime date = DateTime.Parse(gameDate);
                if (date.Date < Manager.EastDateTimeToday.Date)
                {
                    continue;
                }
                Game game = new Game(date);
                foreach (Member member in CurrentPool.Members.Items)
                {
                    game.Members.Add(new Attendee(member.PlayerId, InOutNoshow.In));
                }
                foreach (Dropin dropin in CurrentPool.Dropins.Items)
                {
                    game.Dropins.Add(new Attendee(dropin.PlayerId));
                }
                CurrentPool.Games.Add(game);
            }
            GameListbox.DataSource = CurrentPool.Games;
            GameListbox.DataBind();
            //GameList.SelectedIndex = -1;
            //GameDateTb.Text = "";
            DataAccess.Save(Manager);
            SetNextGameDate();
            //Response.Redirect(Request.RawUrl);
        }

        private void SetNextGameDate()
        {
            Session[Constants.GAME_DATE] = null;
            List<Game> games = CurrentPool.Games;
            IEnumerable<Game> gameQuery = games.OrderBy(game => game.Date);

            foreach (Game game in gameQuery)
            {
                if (game.Date >= DateTime.Today)
                {
                    Session[Constants.GAME_DATE] = game.Date;
                    break;
                }
            }
        }


        protected void UpdateGameBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.GameDateTb.Text == "" || this.GameListbox.SelectedItem == null || this.GameListbox.SelectedItem.Text == this.GameDateTb.Text)
            {
                return;
            }
                try
                {
                    DateTime gameDate = DateTime.Parse(GameDateTb.Text);
                    if (CurrentPool.GameExists(gameDate))
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Game on " + gameDate.ToShortDateString() + " is already in system!')", true);
                        return;
                    }
                    Game game = CurrentPool.FindGameByDate(DateTime.Parse(GameListbox.SelectedItem.Text));
                    if (game != null)
                    {
                        game.Date = gameDate;
                    }
                }
                catch (Exception)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong game date format!  Fix it and try again')", true);
                    return;
                }
                this.GameListbox.DataSource = CurrentPool.Games;
                this.GameListbox.DataBind();
                DataAccess.Save(Manager);
                SetNextGameDate();
               // Response.Redirect(Request.RawUrl);

        }

        protected void DeleteGameBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.GameListbox.SelectedIndex == -1)
            {
                return;
            }
            Game game = CurrentPool.FindGameByDate(DateTime.Parse(GameListbox.SelectedItem.Text));
            if (game != null)
            {
                CurrentPool.Games.Remove(game);
               // this.GameDateTb.Text = "";
               //  this.GameList.SelectedIndex = -1;
                this.GameListbox.DataSource = CurrentPool.Games;
                this.GameListbox.DataBind();
                DataAccess.Save(Manager);
                SetNextGameDate();
                //Response.Redirect(Request.RawUrl);
            }


        }

 
        protected void SavePoolBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.PoolListbox.SelectedIndex <0)
            {
                return;
            }
            CurrentPool.Title = TitleTb.Text;
            CurrentPool.MaximumPlayerNumber = int.Parse(this.MaxPlayers.Text);
            CurrentPool.GameScheduleTime = this.ScheduleTimeTb.Text;
            CurrentPool.MessageBoard = this.MessageTb.Text;
            CurrentPool.DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek),this.DayOfWeekDl.SelectedValue);
            CurrentPool.AllowAddNewDropinName = this.AllowAddingDropinCb.Checked;
            CurrentPool.MembershipFee = int.Parse(this.MemberShipFeeTb.Text);
            CurrentPool.ReservHourForCoop = int.Parse(this.CoopReserveHourTb.Text);
            CurrentPool.LessThanPayersForCoop = int.Parse(this.CoopLessThanPlayersTb.Text);
            CurrentPool.DaysToReserve4Member = int.Parse(this.DaysToReserveForMemberTb.Text);
            CurrentPool.DaysToReserve = int.Parse(this.DaysToReserveTb.Text);
            CurrentPool.AutoCoopReserve = AutoCoopReserveCb.Checked;
            CurrentPool.MaxCoopPlayers = int.Parse(this.MaxCoopPlayerTb.Text);
            CurrentPool.StatsType = this.StatsTypeDdl.SelectedValue;
            CurrentPool.WechatGroupName = this.WechatGroupName.Text;
            DataAccess.Save(Manager);
            //Response.Redirect(Request.RawUrl);

        }

        protected void ResetDropinsBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.PoolListbox.SelectedIndex < 0)
            {
                return;
            }
            List<Game> games = CurrentPool.Games;
            IEnumerable<Game> gameQuery = games.OrderBy(game => game.Date);
            foreach (Game game in gameQuery)
            {
                if (game.Date >= DateTime.Today)
                {
                    foreach (Attendee attendee in game.Dropins.Items)
                    {
                        attendee.Status = InOutNoshow.Out;
                    }
                    game.WaitingList.Clear();
                    DataAccess.Save(Manager);
                    break;
                }
            }

        }
        private IEnumerable<Player> GetPlayers(List<String> ids)
        {
            List<Player> players = new List<Player>();
            foreach (String id in ids)
            {
                players.Add(Manager.FindPlayerById(id));
            }
            return players.OrderBy(p => p.Name);
        }

        private IEnumerable<Player> GetPlayers(VList<Member> members)
        {
            List<Player> players = new List<Player>();
            foreach (Member member in members.Items)
            {
                players.Add(Manager.FindPlayerById(member.PlayerId));
            }
            return players.OrderBy(p => p.Name);
        }
        private IEnumerable<Player> GetPlayers(VList<Dropin> dropins)
        {
            List<Player> players = new List<Player>();
            foreach (Dropin dropin in dropins.Items)
            {
                players.Add(Manager.FindPlayerById(dropin.PlayerId));
            }
            return players.OrderBy(p => p.Name);
        }

        protected void ClearPlayerSelectionBtn_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in this.PlayerListbox.Items)
            {
                item.Selected = false;
            }
        }

        protected void ResevLink_Click(object sender, EventArgs e)
        {
            if (this.PoolListbox.SelectedIndex >= 0 && this.GameListbox.SelectedIndex >= 0)
            {
                Response.Redirect("Default.aspx?Pool=" + this.PoolListbox.SelectedItem.Text + "&GameDate=" + this.GameListbox.SelectedItem.Text + "&Admin=1");
            }
        }

        protected void ClearGamesBtn_Click(object sender, EventArgs e)
        {
            CurrentPool.Games.Clear();
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        protected void ReloadSystemBtn_Click(object sender, EventArgs e)
        {

        }

        protected void MemberListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Member member = CurrentPool.Members.FindByPlayerId(MemberListbox.SelectedValue);
            //this.MemberCancelledCb.Checked = member.IsCancelled;
            //Response.Redirect(Request.RawUrl);
        }

        protected void DropinListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dropin dropin = CurrentPool.Dropins.FindByPlayerId(DropinListbox.SelectedValue);
            this.DropinCoopCb.Checked = dropin.IsCoop;
            //Response.Redirect(Request.RawUrl);
        }

        protected void SaveMemberBtn_Click(object sender, EventArgs e)
        {
            Member member = CurrentPool.Members.FindByPlayerId(MemberListbox.SelectedValue);
            Player player = Manager.FindPlayerById(member.PlayerId);
            //member.IsCancelled = this.MemberCancelledCb.Checked;
             DataAccess.Save(Manager);
           // Response.Redirect(Request.RawUrl);
        }

        protected void SaveDropinBtn_Click(object sender, EventArgs e)
        {
            Dropin dropin = CurrentPool.Dropins.FindByPlayerId(DropinListbox.SelectedValue);
            dropin.IsCoop = this.DropinCoopCb.Checked;
            //dropin.IsSuspended = this.DropinSuspendedCb.Checked;
            //Update future game 
            foreach (Game game in CurrentPool.Games)
            {
                if (game.Date >= DateTime.Today)
                {
                    Attendee attendee = game.Dropins.FindByPlayerId(dropin.PlayerId);
                    if (attendee != null) attendee.IsCoop = dropin.IsCoop;
                }
            }

            DataAccess.Save(Manager);
           // Response.Redirect(Request.RawUrl);
        }

        protected void ResLink_Click(object sender, EventArgs e)
        {
            if (this.PoolListbox.SelectedIndex >= 0 && this.GameListbox.SelectedIndex >= 0)
            {
                Response.Redirect("Default.aspx?abcd=" + this.PoolListbox.SelectedValue + "&GameDate=" + this.GameListbox.SelectedItem.Text + "&Admin=1");
            }
        }
     }
}