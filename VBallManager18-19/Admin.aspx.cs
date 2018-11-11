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
                //this.CookieAuthCb.Checked = Manager.CookieAuthRequired;
                this.TimeZoneTb.Text = Manager.TimeZoneName;
                this.AdminEmailTb.Text = Manager.AdminEmail;
                this.AttendRateStartDateTb.Text = Manager.AttendRateStartDate.ToShortDateString();
                this.MaxDropinfeeOweTb.Text = Manager.MaxDropinFeeOwe.ToString();
                DateTime time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(Manager.TimeZoneName));
                this.SystemTimeLb.Text = time.ToString("MMM dd yyyy HH/mm/ss") + " - H" + time.Hour;
                this.SeasonCb.Text = Manager.Season;
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
            RanderFactorPanel();
            //Home IP
            String homeIpFile = System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE;
            if (File.Exists(homeIpFile))
                this.HomeIPLb.Text = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + Constants.IP_FILE);

        }
        private void RanderFactorPanel()
        {
            IOrderedEnumerable<Factor> orderedFactors = Manager.Factors.OrderBy(factor => factor.PoolName).ThenBy(factor => factor.LowPoolName).//
                ThenBy(factor => factor.LowPoolNumberFrom).ThenBy(factor => factor.CoopNumberFrom).ThenBy(factor => factor.HighPoolNumberFrom);
            foreach (Factor factor in orderedFactors)
            {
                CreateFactorTableRow(factor);
            }
            CreateFactorTableRow(new Factor());
        }

        private TableCell CreatePoolNameCell(Factor factor, String poolNameType, String poolName)
        {
            TableCell poolNameCell = new TableCell();
            DropDownList poolNameDDL = new DropDownList();
            poolNameDDL.ID = factor.Id + "," + poolNameType;
            poolNameDDL.DataSource = Manager.Pools;
            poolNameDDL.DataTextField = "Name";
            poolNameDDL.DataValueField = "Name";
            poolNameDDL.DataBind();
            poolNameDDL.SelectedValue = poolName;
            if (factor.Id != null)
            {
                poolNameDDL.AutoPostBack = true;
                poolNameDDL.SelectedIndexChanged += new EventHandler(FactorPoolNameDDL_Changed);
            }
            poolNameCell.Controls.Add(poolNameDDL);
            return poolNameCell;
        }

        private void FactorPoolNameDDL_Changed(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            String[] ids = ddl.ID.Split(',');
            Factor factor = Manager.Factors.Find(fa => fa.Id == ids[0]);
            if (ids[1] == POOL_NAME) factor.PoolName = ddl.Text;
            else if (ids[1] == LOW_POOL_NAME) factor.LowPoolName = ddl.Text;
            else if (ids[1] == HIGH_POOL_NAME) factor.HighPoolName = ddl.Text;
            DataAccess.Save(Manager);
        }

        private void FactorNumber_Changed(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            String[] ids = ddl.ID.Split(',');
            Factor factor = Manager.Factors.Find(fa => fa.Id == ids[0]);
            if (ids[1] == LOW_POOL_FROM) factor.LowPoolNumberFrom = int.Parse(ddl.Text);
            else if (ids[1] == LOW_POOL_TO) factor.LowPoolNumberTo = int.Parse(ddl.Text);
            else if (ids[1] == COOP_FROM) factor.CoopNumberFrom = int.Parse(ddl.Text);
            else if (ids[1] == COOP_TO) factor.CoopNumberTo = int.Parse(ddl.Text);
            else if (ids[1] == HIGH_POOL_FROM) factor.HighPoolNumberFrom = int.Parse(ddl.Text);
            else if (ids[1] == HIGH_POOL_TO) factor.HighPoolNumberTo = int.Parse(ddl.Text);
            else if (ids[1] == FACTOR_VALUE) factor.Value = decimal.Parse(ddl.Text);
            DataAccess.Save(Manager);
        }
        
        private TableCell CreateNumberCell(Factor factor, String numberType, String number, List<String> numbers)
        {
            TableCell numberCell = new TableCell();
            DropDownList numberDDL = new DropDownList();
            numberDDL.ID = factor.Id + "," + numberType;
            numberDDL.DataSource = numbers;
            numberDDL.DataBind();
            numberDDL.SelectedValue = number;
            if (factor.Id != null)
            {
                numberDDL.AutoPostBack = true;
                numberDDL.SelectedIndexChanged += new EventHandler(FactorNumber_Changed);
            }
            numberCell.Controls.Add(numberDDL);
            return numberCell;
        }

        private const String POOL_NAME = "PoolName";
        private const String LOW_POOL_NAME = "LowPoolName";
        private const String HIGH_POOL_NAME = "HighPoolName";
        private const String LOW_POOL_FROM = "LowPoolFrom";
        private const String LOW_POOL_TO = "LowPoolTo";
        private const String COOP_FROM = "CoopFrom";
        private const String COOP_TO = "CoopTo";
        private const String HIGH_POOL_FROM = "HighPoolFrom";
        private const String HIGH_POOL_TO = "HighPoolTo";
        private const String FACTOR_VALUE = "FactorValue";
        private const String ADD = "Add";
        private const String DELETE = "Delete";
        private List<String> playerNumbers = new List<String>() { "24", "18", "16", "15", "14", "13", "12", "11", "10", "8", "7", "5", "4", "3", "2", "1", "0" };
        private List<String> factorNumbers = new List<String>() { "3", "2.5", "2", "1.5", "1", "0.5", "0.1", "0.01", "0"};

        private void CreateFactorTableRow(Factor factor)
        {
            TableRow tableRow = new TableRow();
            //Pool Name
            tableRow.Cells.Add(CreatePoolNameCell(factor, POOL_NAME, factor.PoolName));
            //Low pool name
            tableRow.Cells.Add(CreatePoolNameCell(factor, LOW_POOL_NAME, factor.LowPoolName));
            //Low pool number from
            tableRow.Cells.Add(CreateNumberCell(factor, LOW_POOL_FROM, factor.LowPoolNumberFrom.ToString(), playerNumbers));
            //Low pool number to
            tableRow.Cells.Add(CreateNumberCell(factor, LOW_POOL_TO, factor.LowPoolNumberTo.ToString(), playerNumbers));
            //Coop from
            tableRow.Cells.Add(CreateNumberCell(factor, COOP_FROM, factor.CoopNumberFrom.ToString(), playerNumbers));
            //Coop to
            tableRow.Cells.Add(CreateNumberCell(factor, COOP_TO, factor.CoopNumberTo.ToString(), playerNumbers));
            //high pool name
            tableRow.Cells.Add(CreatePoolNameCell(factor, HIGH_POOL_NAME, factor.HighPoolName));
            //high pool number from
            tableRow.Cells.Add(CreateNumberCell(factor, HIGH_POOL_FROM, factor.HighPoolNumberFrom.ToString(), playerNumbers));
            //high pool number to
            tableRow.Cells.Add(CreateNumberCell(factor, HIGH_POOL_TO, factor.HighPoolNumberTo.ToString(), playerNumbers));
            //Value
            tableRow.Cells.Add(CreateNumberCell(factor, FACTOR_VALUE, factor.Value.ToString(), factorNumbers));

            TableCell actionCell = new TableCell();
            Button actionBtn = new Button();
            actionBtn.ID = factor.Id + "," + (factor.Id == null ? ADD : DELETE);
            actionBtn.Text = (factor.Id == null) ? ADD : DELETE;
            actionBtn.Click += new EventHandler(ActionFactor_Click);
            actionBtn.OnClientClick = "if ( !confirm('Are you sure?')) return false;";
            actionCell.Controls.Add(actionBtn);
            tableRow.Cells.Add(actionCell);
            if (factor.Id == null)
            {
                tableRow.BackColor = System.Drawing.Color.CadetBlue;
            }
            this.FactorTable.Rows.Add(tableRow);
        }

        private void ActionFactor_Click(object sender, EventArgs e)
        {
            Button tb = (Button)sender;
            String[] ids = tb.ID.Split(',');
            if (ids[1] == ADD)
            {
                String poolName = ((DropDownList)this.Master.FindControl("MainContent").FindControl("," + POOL_NAME)).Text;
                String lowPoolName = ((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_NAME)).Text;
                int lowPoolFrom = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_FROM)).Text);
                int lowPoolTo = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_TO)).Text);
                int coopFrom = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + COOP_FROM)).Text);
                int coopTo = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + COOP_TO)).Text);
                String highPoolName = ((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_NAME)).Text;
                int highPoolFrom = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_FROM)).Text);
                int highPoolTo = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_TO)).Text);
                decimal value = decimal.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + FACTOR_VALUE)).Text);
                Factor factor = new Factor(poolName, lowPoolName, lowPoolFrom, lowPoolTo, coopFrom, coopTo, highPoolName, highPoolFrom, highPoolTo, value);
                Manager.Factors.Add(factor);
            }
            else if (ids[1] == DELETE)
            {
                Factor factor = Manager.Factors.Find(fa => fa.Id == ids[0]);
                Manager.Factors.Remove(factor);
            }
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
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
            Manager.Season = this.SeasonCb.Text;
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