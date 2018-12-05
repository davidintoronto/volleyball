using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace VballManager
{
    public partial class Factors : AdminBase
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
        private List<String> factorNumbers = new List<String>() { "3", "2.75", "2.5", "2.25", "2", "1.75", "1.5", "1.25", "1", "0.75", "0.5", "0.25", "0.1", "0.01", "0" };

        protected void Page_Load(object sender, EventArgs e)
        {
            //Application[Constants.DATA] = DataAccess.LoadReservation();
            if (!IsPostBack)
            {
                // this.AuthCb.Checked = Manager.PasscodeAuthen;
                //
                if (null != Session[Constants.SUPER_ADMIN])
                {
                    ((TextBox)Master.FindControl("PasscodeTb")).Text = Session[Constants.SUPER_ADMIN].ToString();
                }
            }
            RanderFactorPanel();
            RanderPoolFactorList();
        }

        #region Factor Setting
        private void RanderFactorPanel()
        {
            IOrderedEnumerable<Factor> orderedFactors = Manager.Factors.OrderBy(factor => factor.PoolName).ThenBy(factor => factor.LowPoolName).//
                ThenBy(factor => factor.LowPoolNumberFrom).ThenBy(factor => factor.LowPoolNumberTo).ThenBy(factor => factor.HighPoolNumberFrom).//
                ThenBy(factor => factor.HighPoolNumberTo).ThenBy(factor => factor.CoopNumberFrom);
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
            poolNameDDL.ID = factor.Id + "," + poolNameType + ",factor";
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
            numberDDL.ID = factor.Id + "," + numberType + ",factor";
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

        private void CreateFactorTableRow(Factor factor)
        {
            TableRow tableRow = new TableRow();
            //Pool Name
            tableRow.Cells.Add(CreatePoolNameCell(factor, POOL_NAME, factor.PoolName));
            //Low pool name
            tableRow.Cells.Add(CreatePoolNameCell(factor, LOW_POOL_NAME, factor.LowPoolName));
            //Low pool number from
            tableRow.Cells.Add(CreateNumberCell(factor, LOW_POOL_FROM, factor.LowPoolNumberFrom.ToString(), Numbers));
            //Low pool number to
            tableRow.Cells.Add(CreateNumberCell(factor, LOW_POOL_TO, factor.LowPoolNumberTo.ToString(), Numbers));
            //Coop from
            tableRow.Cells.Add(CreateNumberCell(factor, COOP_FROM, factor.CoopNumberFrom.ToString(), Numbers));
            //Coop to
            tableRow.Cells.Add(CreateNumberCell(factor, COOP_TO, factor.CoopNumberTo.ToString(), Numbers));
            //high pool name
            tableRow.Cells.Add(CreatePoolNameCell(factor, HIGH_POOL_NAME, factor.HighPoolName));
            //high pool number from
            tableRow.Cells.Add(CreateNumberCell(factor, HIGH_POOL_FROM, factor.HighPoolNumberFrom.ToString(), Numbers));
            //high pool number to
            tableRow.Cells.Add(CreateNumberCell(factor, HIGH_POOL_TO, factor.HighPoolNumberTo.ToString(), Numbers));
            //Value
            tableRow.Cells.Add(CreateNumberCell(factor, FACTOR_VALUE, factor.Value.ToString(), factorNumbers));

            TableCell actionCell = new TableCell();
            Button actionBtn = new Button();
            actionBtn.ID = factor.Id + "," + (factor.Id == null ? ADD : DELETE) + ",factor";
            actionBtn.Text = (factor.Id == null) ? ADD : DELETE;
            actionBtn.Click += new EventHandler(ActionFactor_Click);
            actionBtn.OnClientClick = "if ( !confirm('Are you sure?')) return false;";
            actionCell.Controls.Add(actionBtn);
            tableRow.Cells.Add(actionCell);
            if (factor.Id == null)
            {
                tableRow.BackColor = System.Drawing.Color.CadetBlue;
            }
            this.FactorSettingTable.Rows.Add(tableRow);
        }


        private void ActionFactor_Click(object sender, EventArgs e)
        {
            Button tb = (Button)sender;
            String[] ids = tb.ID.Split(',');
            if (ids[1] == ADD)
            {
                String poolName = ((DropDownList)this.Master.FindControl("MainContent").FindControl("," + POOL_NAME + ",factor")).Text;
                String lowPoolName = ((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_NAME + ",factor")).Text;
                int lowPoolFrom = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_FROM + ",factor")).Text);
                int lowPoolTo = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + LOW_POOL_TO + ",factor")).Text);
                int coopFrom = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + COOP_FROM + ",factor")).Text);
                int coopTo = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + COOP_TO + ",factor")).Text);
                String highPoolName = ((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_NAME + ",factor")).Text;
                int highPoolFrom = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_FROM + ",factor")).Text);
                int highPoolTo = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + HIGH_POOL_TO + ",factor")).Text);
                decimal value = decimal.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl("," + FACTOR_VALUE + ",factor")).Text);
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
            Response.Redirect(Request.RawUrl);
        }
        #endregion

        private void RanderPoolFactorList()
        {
            Pool poolA = Manager.Pools.Find(p => p.DayOfWeek == DayOfWeek.Monday && !p.IsLowPool);
            Pool poolB = Manager.Pools.Find(p => p.DayOfWeek == DayOfWeek.Monday && p.IsLowPool);
            if (poolA == null || poolB == null) return;
            foreach (Game gameA in poolA.Games.FindAll(g=>g.Date <= Manager.EastDateTimeToday))
            {
                Game gameB = poolB.FindGameByDate(gameA.Date);
                TableRow row = new TableRow();
                AddTableCell(row, gameA.Date.ToShortDateString());
                AddTableCell(row, gameB.NumberOfReservedPlayers.ToString());
                AddTableCell(row, gameB.Factor);
                AddTableCell(row, gameA.Dropins.Items.FindAll(d=>d.IsCoop && d.Status == InOutNoshow.In).Count.ToString());
                AddTableCell(row, gameA.NumberOfReservedPlayers.ToString());
                AddTableCell(row, gameA.Factor);
                this.PoolGameFactorTable.Rows.Add(row);
            }
        }

        private void AddTableCell(TableRow row, String text)
        {
            TableCell cell = new TableCell();
            cell.Text = text;
            row.Cells.Add(cell);
        }
        private void AddTableCell(TableRow row, decimal factor)
        {
            TableCell cell = new TableCell();
            cell.Text = factor.ToString();
            cell.ForeColor = System.Drawing.Color.DarkRed;
            row.Cells.Add(cell);
        }
    }
}