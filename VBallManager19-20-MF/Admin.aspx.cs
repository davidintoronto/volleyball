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

        private const String POOL_NAME = "PoolName";
        private const String HOUR = "Hour";
        private const String LOW_POOL_NAME = "LowPoolName";
        private const String HIGH_POOL_NAME = "HighPoolName";
        private const String LOW_POOL_FROM = "LowPoolFrom";
        private const String LOW_POOL_TO = "LowPoolTo";
        private const String LOW_POOL_WAITING = "LowPoolWaiting";
        private const String COOP_FROM = "CoopFrom";
        private const String COOP_TO = "CoopTo";
        private const String HIGH_POOL_FROM = "HighPoolFrom";
        private const String HIGH_POOL_TO = "HighPoolTo";
        private const String HIGH_POOL_WAITING = "HighPoolWaiting";
        private const String FACTOR_VALUE = "FactorValue";
        private const String ADD = "Add";
        private const String DELETE = "Delete";
        private List<String> Numbers = new List<String>() { "24", "20", "19", "18", "17", "16", "15", "14", "13", "12", "11", "10", "9", "8", "7", "6", "5", "4", "3", "2", "1", "0" };
        private List<String> toMoveNumbers = new List<String>() { "2", "1", "0", "-1", "2" };

        protected void Page_Load(object sender, EventArgs e)
        {
            //Application[Constants.DATA] = DataAccess.LoadReservation();
            if (!IsPostBack)
            {
                // this.AuthCb.Checked = Manager.PasscodeAuthen;
                this.AdminPasscodeTb.Text = Manager.SuperAdmin;
                this.TimeOffsetTb.Text = Manager.TimezoneOffset.ToString();
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName);
                //this.ServerTimeLb.Text = TimeZoneInfo.ConvertTime(Manager.EastDateTimeNow, easternZone).ToShortTimeString() + "-" + TimeZoneInfo.ConvertTime(DateTime.Today, easternZone).AddHours(Manager.LockReservationHour).ToShortTimeString();//DateTime.UtcNow.ToLocalTime().ToShortTimeString();
                this.DropinSpotOpenHourTb.Text = Manager.DropinSpotOpeningHour.ToString();
                // this.CoopReserveHourTb.Text = Manager.CoopReserveHour.ToString();
                this.LockReservationHourTb.Text = Manager.LockReservationHour.ToString();
                this.LockWaitingListHourTb.Text = Manager.LockWaitingListHour.ToString();
                this.ReadmeTb.Text = Manager.Readme;
                this.ClubRegisterMemberModeCb.Checked = Manager.ClubMemberMode;
                this.DropinFeeCappedCb.Checked = Manager.IsDropinFeeWithCap;
                this.MembershipFeeTb.Text = Manager.RegisterMembeshipFee.ToString();
                this.AuthCookieExpireTb.Text = Manager.CookieExpire.ToShortDateString();
                this.AutoCancelHourTb.Text = Manager.AutoCancelHour.ToString();
                this.TimeZoneTb.Text = Manager.TimeZoneName;
                this.AdminEmailTb.Text = Manager.AdminEmail;
                this.AttendRateStartDateTb.Text = Manager.AttendRateStartDate.ToShortDateString();
                this.MaxDropinfeeOweTb.Text = Manager.MaxDropinFeeOwe.ToString();
                DateTime time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName));
                this.SystemTimeLb.Text = time.ToString("MMM dd yyyy HH/mm/ss") + " - H" + time.Hour;
                this.SeasonTb.Text = Manager.Season;
                this.MaintenanceCb.Checked = Manager.InMaintenance;
                this.WechatTb.Text = Manager.WechatGroupName;
                //
                if (null != Session[Constants.SUPER_ADMIN])
                {
                    ((TextBox)Master.FindControl("PasscodeTb")).Text = Session[Constants.SUPER_ADMIN].ToString();
                }
                //Update permits
                UpdatePermits();
            }
            // ShowPermits();
            ShowPermits();
            RanderMoveRulePanel();
            //Home IP
            String homeIpFile = System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE;
            if (File.Exists(homeIpFile))
                this.HomeIPLb.Text = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE);

        }

        #region Move Rules
        private void RanderMoveRulePanel()
        {
            IOrderedEnumerable<MoveRule> orderedMoveRules = Manager.MoveRules.OrderBy(moveRule => moveRule.HighPoolNumberFrom).//
                ThenBy(moveRule => moveRule.HighPoolNumberTo).ThenBy(moveRule => moveRule.LowPoolName).//
                ThenBy(moveRule => moveRule.LowPoolNumberFrom);//.ThenBy(moveRule => moveRule.LowPoolNumberTo).ThenBy(moveRule => moveRule.CoopNumberFrom);
            foreach (MoveRule moveRule in orderedMoveRules)
            {
                CreateMoveRuleTableRow(moveRule);
            }
            CreateMoveRuleTableRow(new MoveRule());
        }

        private TableCell CreatePoolNameCell(MoveRule moveRule, String poolNameType, String poolName)
        {
            TableCell poolNameCell = new TableCell();
            DropDownList poolNameDDL = new DropDownList();
            poolNameDDL.ID = moveRule.Id + "," + poolNameType + ",moveRule";
            poolNameDDL.DataSource = Manager.Pools;
            poolNameDDL.DataTextField = "Name";
            poolNameDDL.DataValueField = "Name";
            poolNameDDL.DataBind();
            poolNameDDL.SelectedValue = poolName;
            if (moveRule.Id != null)
            {
                poolNameDDL.AutoPostBack = true;
                poolNameDDL.SelectedIndexChanged += new EventHandler(MoveRulePoolNameDDL_Changed);
            }
            poolNameCell.Controls.Add(poolNameDDL);
            return poolNameCell;
        }

        private TableCell CreateNumberCell(MoveRule moveRule, String numberType, String number, List<String> numbers)
        {
            TableCell numberCell = new TableCell();
            DropDownList numberDDL = new DropDownList();
            numberDDL.ID = moveRule.Id + "," + numberType + ",moveRule";
            numberDDL.DataSource = numbers;
            numberDDL.DataBind();
            numberDDL.SelectedValue = number;
            if (moveRule.Id != null)
            {
                numberDDL.AutoPostBack = true;
                numberDDL.SelectedIndexChanged += new EventHandler(MoveRuleNumber_Changed);
            }
            numberCell.Controls.Add(numberDDL);
            return numberCell;
        }
        private void MoveRulePoolNameDDL_Changed(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            String[] ids = ddl.ID.Split(',');
            MoveRule moveRule = Manager.MoveRules.Find(fa => fa.Id == ids[0]);
            if (ids[1] == LOW_POOL_NAME) moveRule.LowPoolName = ddl.Text;
            else if (ids[1] == HIGH_POOL_NAME) moveRule.HighPoolName = ddl.Text;
            DataAccess.Save(Manager);
        }

        private void MoveRuleNumber_Changed(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            String[] ids = ddl.ID.Split(',');
            MoveRule moveRule = Manager.MoveRules.Find(fa => fa.Id == ids[0]);
            if (ids[1] == HOUR) moveRule.Hour = int.Parse(ddl.Text);
            else if (ids[1] == LOW_POOL_FROM) moveRule.LowPoolNumberFrom = int.Parse(ddl.Text);
            else if (ids[1] == LOW_POOL_TO) moveRule.LowPoolNumberTo = int.Parse(ddl.Text);
            else if (ids[1] == LOW_POOL_WAITING) moveRule.LowPoolWaiting = int.Parse(ddl.Text);
            else if (ids[1] == COOP_FROM) moveRule.CoopNumberFrom = int.Parse(ddl.Text);
            else if (ids[1] == COOP_TO) moveRule.CoopNumberTo = int.Parse(ddl.Text);
            else if (ids[1] == HIGH_POOL_FROM) moveRule.HighPoolNumberFrom = int.Parse(ddl.Text);
            else if (ids[1] == HIGH_POOL_TO) moveRule.HighPoolNumberTo = int.Parse(ddl.Text);
            else if (ids[1] == HIGH_POOL_WAITING) moveRule.HighPoolWaiting = int.Parse(ddl.Text);
            else if (ids[1] == FACTOR_VALUE) moveRule.ToMove = int.Parse(ddl.Text);
            DataAccess.Save(Manager);
        }

        private void CreateMoveRuleTableRow(MoveRule moveRule)
        {
            TableRow tableRow = new TableRow();
            //Hour
            tableRow.Cells.Add(CreateNumberCell(moveRule, HOUR, moveRule.Hour.ToString(), Numbers));
            //Low pool name
            tableRow.Cells.Add(CreatePoolNameCell(moveRule, LOW_POOL_NAME, moveRule.LowPoolName));
            //Low pool number from
            tableRow.Cells.Add(CreateNumberCell(moveRule, LOW_POOL_FROM, moveRule.LowPoolNumberFrom.ToString(), Numbers));
            //Low pool number to
            tableRow.Cells.Add(CreateNumberCell(moveRule, LOW_POOL_TO, moveRule.LowPoolNumberTo.ToString(), Numbers));
            //Low pool waiting
            tableRow.Cells.Add(CreateNumberCell(moveRule, LOW_POOL_WAITING, moveRule.LowPoolWaiting.ToString(), Numbers));
            //Coop from
            tableRow.Cells.Add(CreateNumberCell(moveRule, COOP_FROM, moveRule.CoopNumberFrom.ToString(), Numbers));
            //Coop to
            tableRow.Cells.Add(CreateNumberCell(moveRule, COOP_TO, moveRule.CoopNumberTo.ToString(), Numbers));
            //high pool name
            tableRow.Cells.Add(CreatePoolNameCell(moveRule, HIGH_POOL_NAME, moveRule.HighPoolName));
            //high pool number from
            tableRow.Cells.Add(CreateNumberCell(moveRule, HIGH_POOL_FROM, moveRule.HighPoolNumberFrom.ToString(), Numbers));
            //high pool number to
            tableRow.Cells.Add(CreateNumberCell(moveRule, HIGH_POOL_TO, moveRule.HighPoolNumberTo.ToString(), Numbers));
            //High pool waiting
            tableRow.Cells.Add(CreateNumberCell(moveRule, HIGH_POOL_WAITING, moveRule.HighPoolWaiting.ToString(), Numbers));
            //Value
            tableRow.Cells.Add(CreateNumberCell(moveRule, FACTOR_VALUE, moveRule.ToMove.ToString(), toMoveNumbers));

            TableCell actionCell = new TableCell();
            Button actionBtn = new Button();
            actionBtn.ID = moveRule.Id + "," + (moveRule.Id == null ? ADD : DELETE) + ",moveRule";
            actionBtn.Text = (moveRule.Id == null) ? ADD : DELETE;
            actionBtn.Click += new EventHandler(ActionMoveRule_Click);
            actionBtn.OnClientClick = "if ( !confirm('Are you sure?')) return false;";
            actionCell.Controls.Add(actionBtn);
            tableRow.Cells.Add(actionCell);
            if (moveRule.Id == null)
            {
                tableRow.BackColor = System.Drawing.Color.CadetBlue;
            }
            this.MoveRuleTable.Rows.Add(tableRow);
        }

        private void ActionMoveRule_Click(object sender, EventArgs e)
        {
            Button tb = (Button)sender;
            String[] ids = tb.ID.Split(',');
            if (ids[1] == ADD)
            {
                int hour = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HOUR + ",moveRule")).Text);
                String lowPoolName = ((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_NAME + ",moveRule")).Text;
                int lowPoolFrom = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_FROM + ",moveRule")).Text);
                int lowPoolTo = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_TO + ",moveRule")).Text);
                int lowPoolWaiting = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_WAITING + ",moveRule")).Text);
                int coopFrom = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + COOP_FROM + ",moveRule")).Text);
                int coopTo = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + COOP_TO + ",moveRule")).Text);
                String highPoolName = ((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_NAME + ",moveRule")).Text;
                int highPoolFrom = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_FROM + ",moveRule")).Text);
                int highPoolTo = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_TO + ",moveRule")).Text);
                int highPoolWaiting = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_WAITING + ",moveRule")).Text);
                int value = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + FACTOR_VALUE + ",moveRule")).Text);
                MoveRule moveRule = new MoveRule(hour, lowPoolName, lowPoolFrom, lowPoolTo, lowPoolWaiting, coopFrom, coopTo, highPoolName, highPoolFrom, highPoolTo, highPoolWaiting, value);
                Manager.MoveRules.Add(moveRule);
            }
            else if (ids[1] == DELETE)
            {
                MoveRule moveRule = Manager.MoveRules.Find(fa => fa.Id == ids[0]);
                Manager.MoveRules.Remove(moveRule);
            }
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }
        #endregion       
 
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
            //Manager.PasscodeAuthen = AuthCb.Checked;
            if (!String.IsNullOrEmpty(AdminPasscodeTb.Text.Trim()))
            {
                Manager.SuperAdmin = AdminPasscodeTb.Text.Trim();
                Session[Constants.PASSCODE] = Manager.SuperAdmin;
            }
            Manager.TimezoneOffset = int.Parse(TimeOffsetTb.Text);
            Manager.DropinSpotOpeningHour = int.Parse(this.DropinSpotOpenHourTb.Text);
            Manager.LockReservationHour = int.Parse(this.LockReservationHourTb.Text);
            Manager.LockWaitingListHour = int.Parse(this.LockWaitingListHourTb.Text);
            //Manager.CoopReserveHour = int.Parse(this.CoopReserveHourTb.Text);
            Manager.Readme = this.ReadmeTb.Text;
            Manager.ClubMemberMode = this.ClubRegisterMemberModeCb.Checked;
            Manager.IsDropinFeeWithCap = this.DropinFeeCappedCb.Checked;
            Manager.RegisterMembeshipFee = int.Parse(this.MembershipFeeTb.Text);
            Manager.CookieExpire = DateTime.Parse(this.AuthCookieExpireTb.Text);
            Manager.AttendRateStartDate = DateTime.Parse(this.AttendRateStartDateTb.Text);
            //Manager.CookieAuthRequired = this.CookieAuthCb.Checked;
            Manager.TimeZoneName = this.TimeZoneTb.Text;
            Manager.AdminEmail = this.AdminEmailTb.Text;
            Manager.MaxDropinFeeOwe = int.Parse(this.MaxDropinfeeOweTb.Text);
            Manager.Season = this.SeasonTb.Text;
            Manager.AutoCancelHour = int.Parse(this.AutoCancelHourTb.Text);
            Manager.InMaintenance = this.MaintenanceCb.Checked;
            Manager.WechatGroupName = this.WechatTb.Text;
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

        protected void ClearGamesBtn_Click(object sender, EventArgs e)
        {
            CurrentPool.Games.Clear();
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        protected void ReloadSystemBtn_Click(object sender, EventArgs e)
        {

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


        protected void AllWechatNameBtn_Click(object sender, EventArgs e)
        {
            foreach (Player player in Manager.Players)
            {
                // player.WechatName = "";
            }
            DataAccess.Save(Manager);
        }

        protected void SetMembershipBtn_Click(object sender, EventArgs e)
        {
            foreach (Pool pool in Manager.Pools)
            {
                foreach (Member member in pool.Members.Items)
                {
                    Player player = Manager.FindPlayerById(member.PlayerId);
                    //Create membership fee entry
                    if (Manager.ClubMemberMode && !player.IsRegisterdMember)
                    {
                        Fee fee = new Fee(Fee.FEETYPE_CLUB_MEMBERSHIP, Manager.RegisterMembeshipFee);
                        fee.Date = DateTime.Today;
                        player.Fees.Add(fee);
                    }
                    player.IsRegisterdMember = true;
                }
            }
            DataAccess.Save(Manager);
        }

        protected void RecalculateFactorBtn_Click(object sender, EventArgs e)
        {
            foreach (Pool pool in Manager.Pools.FindAll(p => p.IsLowPool))
            {
                foreach (Game game in pool.Games.FindAll(g => g.Date > Manager.AttendRateStartDate))
                {
                    Manager.ReCalculateFactor(pool, game.Date);
                }
            }
            DataAccess.Save(Manager);
        }
    }
}