using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Admin : System.Web.UI.Page
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            Application[Constants.DATA] = DataAccess.LoadReservation();
            if (!IsPostBack)
            {
                this.AuthCb.Checked = Manager.PasscodeAuthen;
                this.AdminPasscodeTb.Text = Manager.SuperAdmin;
                this.TimeOffsetTb.Text = Manager.TimezoneOffset.ToString();
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName);
                //this.ServerTimeLb.Text = TimeZoneInfo.ConvertTime(DateTime.Now, easternZone).ToShortTimeString() + "-" + TimeZoneInfo.ConvertTime(DateTime.Today, easternZone).AddHours(Manager.LockReservationHour).ToShortTimeString();//DateTime.UtcNow.ToLocalTime().ToShortTimeString();
                this.DropinSpotOpenHourTb.Text = Manager.DropinSpotOpeningHour.ToString();
               // this.CoopReserveHourTb.Text = Manager.CoopReserveHour.ToString();
                this.LockReservationHourTb.Text = Manager.LockReservationHour.ToString();
                this.ReadmeTb.Text = Manager.Readme;
                this.ClubRegisterMemberModeCb.Checked = Manager.ClubMemberMode;
                this.DropinFeeCappedCb.Checked = Manager.IsDropinFeeWithCap;
                this.MembershipFeeTb.Text = Manager.RegisterMembeshipFee.ToString();
                this.AuthCookieExpireTb.Text = Manager.CookieExpire.ToShortDateString();
                this.CookieAuthCb.Checked = Manager.CookieAuthRequired;
                this.TimeZoneTb.Text = Manager.TimeZoneName;
                DateTime time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName));
                this.SystemTimeLb.Text = time.ToString("MMM dd yyyy HH/mm/ss") + " - H" + time.Hour;
                //
                if (null != Session[Constants.SUPER_ADMIN])
                {
                    ((TextBox)Master.FindControl("PasscodeTb")).Text = Session[Constants.SUPER_ADMIN].ToString();
                }
               //Bind player list
                //int selectPlayerIndex = this.PlayerListbox.SelectedIndex;
                this.PlayerListbox.DataSource = Manager.Players.OrderBy(player => player.Name);
                this.PlayerListbox.DataTextField = "Name";
                this.PlayerListbox.DataValueField = "Id";
                this.PlayerListbox.DataBind();
                //this.PlayerListbox.SelectedIndex = selectPlayerIndex;
                RebindPlayerList();

                int selectPlayerIndex = this.PlayerDDList.SelectedIndex;
                this.PlayerDDList.DataSource = Manager.Players.OrderBy(player => player.Name);
                this.PlayerDDList.DataTextField = "Name";
                this.PlayerDDList.DataValueField = "Id";
                this.PlayerDDList.DataBind();
                this.PlayerDDList.SelectedIndex = selectPlayerIndex;
                //
                this.DeletePlayerBtn.OnClientClick = "if ( !confirm('Are you sure you want to delete this fee?')) return false;";
           }
      }

       private void RebindPlayerList()
       {
           this.PlayerListbox.DataSource = Manager.Players.OrderBy(p => p.Name);
           this.PlayerListbox.DataBind();
           this.PlayerDDList.DataSource = Manager.Players.OrderBy(p => p.Name);
           this.PlayerDDList.DataBind();
           foreach (ListItem playerItem in this.PlayerListbox.Items)
           {
               Player player = Manager.FindPlayerById(playerItem.Value);
               playerItem.Selected = player.IsRegisterdMember;
           }
       }

       protected void AddPlayerBtn_Click(object sender, EventArgs e)
        {
            if(!IsSuperAdminPasscode() || this.PlayerNameTb.Text == "")
            {
                return;
            }
            String[] names = this.PlayerNameTb.Text.Split(',');
            foreach (String name in names)
            {
                if (Manager.PlayerNameExists(name.Trim()))
                {
                    continue;
                }
                Player player = new Player(name.Trim(), PlayerPasscodeTb.Text, PlayerMarkCb.Checked);
                player.Suspend = this.PlayerSuspendCb.Checked;
                Manager.Players.Add(player);
            }
            DataAccess.Save(Manager);
            RebindPlayerList();
            //Response.Redirect(Request.RawUrl);

        }

  
        protected void UpdatePlayerBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdminPasscode() || this.PlayerNameTb.Text == "" || this.PlayerDDList.SelectedItem == null)
            {
                return;
            }
            Player player = Manager.FindPlayerById(this.PlayerDDList.SelectedItem.Value);
            player.Name = PlayerNameTb.Text;
            player.Passcode = PlayerPasscodeTb.Text;
            player.Marked = PlayerMarkCb.Checked;
            player.Suspend = PlayerSuspendCb.Checked;
           //Save
            DataAccess.Save(Manager);
            RebindPlayerList();
            //Response.Redirect(Request.RawUrl);
        }

        private bool IsSuperAdminPasscode()
        {
            TextBox passcodeTb = (TextBox)Master.FindControl("PasscodeTb");
            if (Manager.SuperAdmin != passcodeTb.Text)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong passcode! Re-enter your passcode and try again')", true);
                return false;
            }
            Session[Constants.SUPER_ADMIN] = passcodeTb.Text;
            return true;
        }


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
        protected void DeletePlayerBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdminPasscode())
            {
                return;
            }
            Manager.DeletePlayer(this.PlayerDDList.SelectedValue);
            //Save
            DataAccess.Save(Manager);
            RebindPlayerList();
            //Response.Redirect(Request.RawUrl);
        }



        protected void PlayerDDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsSuperAdminPasscode() || this.PlayerDDList.SelectedIndex < 0)
            {
                return;
            }
            Player player = Manager.FindPlayerById(this.PlayerDDList.SelectedValue);
            PlayerIdTb.Text = player.Id;
            PlayerNameTb.Text = player.Name;
            PlayerPasscodeTb.Text = player.Passcode;
            PlayerMarkCb.Checked = player.Marked;
            PlayerSuspendCb.Checked = player.Suspend;
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




  
        protected void SaveSystemBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdminPasscode())
            {
                return;
            }
            Manager.PasscodeAuthen = AuthCb.Checked;
            if (!String.IsNullOrEmpty(AdminPasscodeTb.Text.Trim()))
            {
                Manager.SuperAdmin = AdminPasscodeTb.Text.Trim();
                Session[Constants.PASSCODE] = Manager.SuperAdmin;
            }
            Manager.TimezoneOffset = int.Parse(TimeOffsetTb.Text);
            Manager.DropinSpotOpeningHour = int.Parse(this.DropinSpotOpenHourTb.Text);
            Manager.LockReservationHour = int.Parse(this.LockReservationHourTb.Text);
            //Manager.CoopReserveHour = int.Parse(this.CoopReserveHourTb.Text);
            Manager.Readme = this.ReadmeTb.Text;
            Manager.ClubMemberMode = this.ClubRegisterMemberModeCb.Checked;
            Manager.IsDropinFeeWithCap = this.DropinFeeCappedCb.Checked;
            Manager.RegisterMembeshipFee = int.Parse(this.MembershipFeeTb.Text);
            Manager.CookieExpire = DateTime.Parse(this.AuthCookieExpireTb.Text);
            Manager.CookieAuthRequired = this.CookieAuthCb.Checked;
            Manager.TimeZoneName = this.TimeZoneTb.Text;
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);

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

        private IEnumerable<Player> GetPlayers(List<Member> members)
        {
            List<Player> players = new List<Player>();
            foreach (Member member in members)
            {
                players.Add(Manager.FindPlayerById(member.Id));
            }
            return players.OrderBy(p => p.Name);
        }
        private IEnumerable<Player> GetPlayers(List<Dropin> dropins)
        {
            List<Player> players = new List<Player>();
            foreach (Dropin dropin in dropins)
            {
                players.Add(Manager.FindPlayerById(dropin.Id));
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

        protected void ClearGamesBtn_Click(object sender, EventArgs e)
        {
            CurrentPool.Games.Clear();
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        protected void ReloadSystemBtn_Click(object sender, EventArgs e)
        {

        }

        protected void SavePlayersBtn_Click(object sender, EventArgs e)
        {
            foreach (ListItem playerItem in this.PlayerListbox.Items)
            {
                Player player = Manager.FindPlayerById(playerItem.Value);
                if (player != null)
                {
                    //Create membership fee entry
                    if (Manager.ClubMemberMode && !player.IsRegisterdMember && playerItem.Selected)
                    {
                        Fee fee = new Fee(Fee.FEETYPE_CLUB_MEMBERSHIP, Manager.RegisterMembeshipFee);
                        fee.Date = DateTime.Today;
                        player.Fees.Add(fee);
                    }
                    player.IsRegisterdMember = playerItem.Selected;
                }
            }
            DataAccess.Save(Manager);
            RebindPlayerList();
        }

        protected void ResetGamesBtn_Click(object sender, EventArgs e)
        {
            //Reset pools
            foreach (Pool pool in Manager.Pools)
            {
                //Delete games
                pool.Games = new List<Game>();
                //Delete Dropins
          //      pool.Dropins = new List<Dropin>();
                //Delete member
         //       pool.Members = new List<Member>();
            }
            foreach (Player player in Manager.Players)
            {
                player.Transfers = new List<Transfer>();
                player.Fees = new List<Fee>();
                player.FreeDropin = 0;
            }
            Manager.Logs = new List<LogHistory>();
            DataAccess.Save(Manager);
        }
        protected void ResetPoolsBtn_Click(object sender, EventArgs e)
        {
            //Reset pools
            foreach (Pool pool in Manager.Pools)
            {
                //Delete games
                pool.Games = new List<Game>();
                //Delete Dropins
                      pool.Dropins = new List<Dropin>();
                //Delete member
                       pool.Members = new List<Member>();
            }
            foreach (Player player in Manager.Players)
            {
                player.Transfers = new List<Transfer>();
                player.Fees = new List<Fee>();
                player.FreeDropin = 0;
                player.IsRegisterdMember = false;
            }
            Manager.Logs = new List<LogHistory>();
            DataAccess.Save(Manager);
        }

        protected void ResetSystemBtn_Click(object sender, EventArgs e)
        {
            if (ClearPoolMemberCb.Checked)
            {
                foreach (Pool pool in Manager.Pools)
                {
                    pool.Members.Clear();
                    pool.Dropins.Clear();
                    foreach (Game game in pool.Games)
                    {
                        game.Absences.Clear();
                        game.Pickups.Clear();
                        game.WaitingList.Clear();
                    }
                }
            }
            if (ClearPoolGamesCb.Checked)
            {
                foreach (Pool pool in Manager.Pools)
                {
                    pool.Games.Clear();
                }
            }
            if (ClearPlayerFeeCb.Checked)
            {
                foreach (Player player in Manager.Players)
                {
                    player.Fees.Clear();
                }
            }
            if (ResetPlayerMembershipCb.Checked)
            {
                foreach (Player player in Manager.Players)
                {
                    player.IsRegisterdMember = false;
                }

            }
            if (ResetUserAuthorizationCb.Checked)
            {
                foreach (Player player in Manager.Players)
                {
                    player.AuthorizedUsers.Clear();
                    player.DeviceLinked = false;
                }
            }
            if (ResetPlayerTransferCb.Checked)
            {
                foreach (Player player in Manager.Players)
                {
                    player.Transfers.Clear();
                    player.FreeDropin = 0;
                }
            }
            DataAccess.Save(Manager);
       }

        protected void updateFeeTypes()
        {
            foreach (Player player in Manager.Players)
            {
                foreach (Fee fee in player.Fees)
                {
                    fee.FeeDesc = fee.FeeType;
                    if (fee.FeeDesc.Contains("Club Membership Fee"))
                    {
                        fee.FeeType = FeeTypeEnum.Membership.ToString();
                    }
                    else if (fee.FeeDesc.Contains("Credit"))
                    {
                        fee.FeeType = FeeTypeEnum.Credit.ToString();
                    }
                    else if (fee.FeeDesc.ToLower().Contains("dropin"))
                    {
                        fee.FeeType = FeeTypeEnum.Dropin.ToString();
                    }
                    else 
                    {
                        fee.FeeType = FeeTypeEnum.Admin.ToString();
                    }
                }
            }
            DataAccess.Save(Manager);
        }

        protected void UpdateFeeTypeBtn_Click(object sender, EventArgs e)
        {
            updateFeeTypes();
        }
    }
}