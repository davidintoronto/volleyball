using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class LogHistories : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            this.LogTable.Rows.Add(createLogTableRow("Date", "IP", "Pool", "Player", "Type", "Operator"));
            foreach (LogHistory log in Manager.Logs)
            {
                this.LogTable.Rows.Add(createLogTableRow(TimeZoneInfo.ConvertTime(log.Date, easternZone).ToString("yyyy-MM-dd hh:mm:ss"), log.UserInfo, log.PoolName, log.PlayerName, log.Type, log.OperatorName));
            }
        }
        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }
        private TableRow createLogTableRow(String date, String userInfo, String poolName, String playerName, String type, String operatorName)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = date;
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Text = userInfo;
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Text = poolName;
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Text = playerName;
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Text = type;
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Text = operatorName;
            row.Cells.Add(cell);
            return row;
        }

        protected void ClearLogHistory_Click(object sender, EventArgs e)
        {
            Manager.Logs.Clear();
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

    }
}