using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace VballManager
{
    public partial class Admin : AdminBase
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
                this.AdminEmailTb.Text = Manager.AdminEmail;
                this.MaxDropinfeeOweTb.Text = Manager.MaxDropinFeeOwe.ToString();
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

                int selectPlayerIndex = this.PlayerLb.SelectedIndex;
                this.PlayerLb.DataSource = Manager.Players.OrderBy(player => player.Name);
                this.PlayerLb.DataTextField = "Name";
                this.PlayerLb.DataValueField = "Id";
                this.PlayerLb.DataBind();
                this.PlayerLb.SelectedIndex = selectPlayerIndex;
                BindRoleDropdownList(this.Role);
                //
                this.DeletePlayerBtn.OnClientClick = "if ( !confirm('Are you sure you want to delete this Player?')) return false;";
                this.SendWelcomeWechatMessageBtn.OnClientClick = "if ( !confirm('Are you sure to send welcome message to all the Players?')) return false;";
                //Update permits
                UpdatePermits();
            }
            // ShowPermits();
            ShowPermits();
            //Home IP
            String homeIpFile = System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE;
            if (File.Exists(homeIpFile))
                this.HomeIPLb.Text = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE);

        }


        private void ShowPermits()
        {
            //Bind permits
            foreach (Permit permit in Manager.Permits)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Text = permit.Action.ToString();
                row.Cells.Add(cell);
                cell = new TableCell();
                DropDownList roleDDL = new DropDownList();
                roleDDL.SelectedIndexChanged += new EventHandler(roleDDL_SelectedIndexChanged);
                roleDDL.AutoPostBack = true;
                BindRoleDropdownList(roleDDL);
                roleDDL.ID = permit.Action.ToString();
                roleDDL.DataBind();
                roleDDL.SelectedValue = permit.Role.ToString();
                cell.Controls.Add(roleDDL);
                row.Cells.Add(cell);
                this.AuthorizeTable.Rows.Add(row);
            }

        }

        void roleDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            String action = ddl.ID;
            Permit permit = Manager.Permits.Find(per => per.Action.ToString() == action);
            if (permit != null) permit.Role = int.Parse(ddl.SelectedValue);
            DataAccess.Save(Manager);
        }

        private void BindRoleDropdownList(DropDownList ddl)
        {
            foreach (int value in Enum.GetValues(typeof(Roles)))
            {
                ddl.Items.Add(new ListItem(Enum.GetName(typeof(Roles), value), value.ToString()));
            }
        }

        private void UpdatePermits()
        {
            foreach (Permit permit in Manager.Permits)
            {
                if (!Enum.IsDefined(typeof(Actions), permit.Action))
                {
                    Manager.Permits.Remove(permit);
                    DataAccess.Save(Manager);
                }
            }
            foreach (Actions newAction in Enum.GetValues(typeof(Actions)))
            {
                if (!Manager.Permits.Exists(permit => permit.Action == newAction))
                {
                    Permit permit = new Permit();
                    permit.Action = newAction;
                    permit.Role = 0;
                    Manager.Permits.Add(permit);
                    DataAccess.Save(Manager);
                }
            }

        }

        private void RebindPlayerList()
        {
            this.PlayerListbox.DataSource = Manager.Players.OrderBy(p => p.Name);
            this.PlayerListbox.DataBind();
            this.PlayerLb.DataSource = Manager.Players.OrderBy(p => p.Name);
            this.PlayerLb.DataBind();
            foreach (ListItem playerItem in this.PlayerListbox.Items)
            {
                Player player = Manager.FindPlayerById(playerItem.Value);
                if (this.PlayerPropertiesList.SelectedValue == PlayerBooleanProperties.IsRegisterMember.ToString())
                {
                    playerItem.Selected = player.IsRegisterdMember;
                }
                else if (this.PlayerPropertiesList.SelectedValue == PlayerBooleanProperties.IsActive.ToString())
                {
                    playerItem.Selected = player.IsActive;
                }
                else if (this.PlayerPropertiesList.SelectedValue == PlayerBooleanProperties.Marked.ToString())
                {
                    playerItem.Selected = player.Marked;
                }
            }
        }

        protected void AddPlayerBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.PlayerNameTb.Text == "")
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
                Player player = new Player(name.Trim(), null, PlayerMarkCb.Checked);
                player.Role = int.Parse(this.Role.SelectedValue);
                player.WechatName = this.WechatNameTb.Text;
                player.IsActive = this.PlayerActiveCb.Checked;
                Manager.Players.Add(player);
            }
            DataAccess.Save(Manager);
            RebindPlayerList();
            //Response.Redirect(Request.RawUrl);

        }


        protected void UpdatePlayerBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.PlayerNameTb.Text == "" || this.PlayerLb.SelectedItem == null)
            {
                return;
            }
            Player player = Manager.FindPlayerById(this.PlayerLb.SelectedItem.Value);
            player.Name = PlayerNameTb.Text;
            player.Passcode = PlayerPasscodeTb.Text;
            player.Role = int.Parse(Role.SelectedValue);
            player.WechatName = WechatNameTb.Text;
            player.Marked = PlayerMarkCb.Checked;
            player.IsActive = PlayerActiveCb.Checked;
            //Save
            DataAccess.Save(Manager);
            RebindPlayerList();
            //Response.Redirect(Request.RawUrl);
        }

        protected void DeletePlayerBtn_Click(object sender, EventArgs e)
        {
            if (!IsSuperAdmin())
            {
                return;
            }
            Manager.DeletePlayer(this.PlayerLb.SelectedValue);
            //Save
            DataAccess.Save(Manager);
            RebindPlayerList();
            //Response.Redirect(Request.RawUrl);
        }



        protected void PlayerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsSuperAdmin() || this.PlayerLb.SelectedIndex < 0)
            {
                return;
            }
            Player player = Manager.FindPlayerById(this.PlayerLb.SelectedValue);
            PlayerIdTb.Text = player.Id;
            PlayerNameTb.Text = player.Name;
            PlayerPasscodeTb.Text = player.Passcode;
            PlayerMarkCb.Checked = player.Marked;
            PlayerActiveCb.Checked = player.IsActive;
            this.Role.SelectedValue = player.Role.ToString();
            this.WechatNameTb.Text = player.WechatName;
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
            if (!IsSuperAdmin())
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
            Manager.AdminEmail = this.AdminEmailTb.Text;
            Manager.MaxDropinFeeOwe = int.Parse(this.MaxDropinfeeOweTb.Text);
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
                players.Add(Manager.FindPlayerById(member.PlayerId));
            }
            return players.OrderBy(p => p.Name);
        }
        private IEnumerable<Player> GetPlayers(List<Dropin> dropins)
        {
            List<Player> players = new List<Player>();
            foreach (Dropin dropin in dropins)
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
                    if (this.PlayerPropertiesList.SelectedValue == PlayerBooleanProperties.IsRegisterMember.ToString())
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
                    else if (this.PlayerPropertiesList.SelectedValue == PlayerBooleanProperties.IsActive.ToString())
                    {
                        player.IsActive = playerItem.Selected;
                    }
                    else if (this.PlayerPropertiesList.SelectedValue == PlayerBooleanProperties.Marked.ToString())
                    {
                        player.Marked = playerItem.Selected;
                    }
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
                pool.Dropins = new VList<Dropin>();
                //Delete member
                pool.Members = new VList<Member>();
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
                        game.Members.Clear();
                        game.Dropins.Clear();
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
                    player.Passcode = null;
                }
            }
            if (ResetPlayerTransferCb.Checked)
            {
                foreach (Player player in Manager.Players)
                {
                    player.Transfers.Clear();
                    player.FreeDropin = 0;
                    player.Role = (int)Roles.Player;
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

        protected void SendWelcomeWechatMessageBtn_Click(object sender, EventArgs e)
        {
            IEnumerable<Player> players = Manager.Players.FindAll(player => player.IsActive && !String.IsNullOrEmpty(player.WechatName));
            foreach (Player player in players)
            {
                if (player.IsRegisterdMember)
                {
                    String message = " Welcome to Hitmen Volleyball Club! We are happy to inform you that your membership has been approved! As a registed member, you could become primary member of the pools you attend if your attendance rate is high. And you are allow to make reservation one day before the game. let's play and have great fun!";
                    Manager.AddNotifyWechatMessage(player, message);
                    message = " Here is your private register link " + Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath) + "/" + Constants.REGISTER_DEVICE_PAGE + "?id=" + Manager.ReversedId(player.Id);
                    Manager.AddNotifyWechatMessage(player, message);
                    decimal total = 0;
                    foreach (Fee fee in player.Fees)
                    {
                        if (!fee.IsPaid) total = total + fee.Amount;
                    }
                    if (total > 0)
                    {
                        message = " You membership fee is " + Manager.RegisterMembeshipFee + ". You unpaid fee is " + total + ". If you would like to pay e-transfer, please send to " + Manager.AdminEmail + ". Thanks";
                        Manager.AddNotifyWechatMessage(player, message);
                    }
                }
                else
                {
                    String message = " Welcome to Hitmen Volleyball Club! Here is your private register link " + Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath) + "/" + Constants.REGISTER_DEVICE_PAGE + "?id=" + Manager.ReversedId(player.Id);
                    Manager.AddNotifyWechatMessage(player, message);
                }
            }
        }

        protected void SendPrimaryMemberNotificationWechatMessageBtn_Click(object sender, EventArgs e)
        {
            foreach (Pool pool in Manager.Pools)
            {
                String message = "Hi, everyone. We will re-assign primary members as scheduled. Following " + pool.Members.Count + " players are highly rated on their attendance, they will be the primary members for next few months";
                Manager.AddNotifyWechatMessage(pool, message);
                foreach(Member member in pool.Members.Items)
                {
                    Player player = Manager.FindPlayerById(member.PlayerId);
                    message = "Congratus!. We are very pleased that you have become 1 of " + pool.Members.Count + " primary members in pool " + pool.Name + ". You deserve this because you attended a lot of games in the post. We will pre-reserve a spot for you for each week in this pool. However, please cancel your reservation if you cannot make it. Thanks";
                    Manager.AddNotifyWechatMessage(player, message);
                    message = "Congratus! You have become the primary member in pool " + pool.Name;
                    Manager.AddNotifyWechatMessage(pool, player, message);
                }
            }
        }

        protected void AllWechatNameBtn_Click(object sender, EventArgs e)
        {
            foreach (Player player in Manager.Players)
            {
               // player.WechatName = "";
            }
            DataAccess.Save(Manager);
        }

        protected void PlayerPropertiesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RebindPlayerList();
        }

        protected void SetMembershipBtn_Click(object sender, EventArgs e)
        {
            foreach (Pool pool in Manager.Pools)
            {
                foreach (Member member in pool.Members.Items)
                {
                    Player player = Manager.FindPlayerById(member.PlayerId);
                    player.IsRegisterdMember = true;
                }
            }
            DataAccess.Save(Manager);
        }

    }
}