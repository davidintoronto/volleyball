using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class PreRegister : System.Web.UI.Page
    {
        private int count = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            int year = DateTime.Today.Year;
            string newSeason = string.Format("{0}-{1}", year, year + 1);
            this.Page.Title = string.Format("{0}{1}", newSeason, this.Page.Title);
            String poolId = this.Request.Params[Constants.POOL_ID];
            String poolName = this.Request.Params[Constants.POOL];

            if (poolId != null)
            {
                poolName = Manager.FindPoolById(poolId).Name;
                Session[Constants.POOL] = poolName;
            }
            else if (poolName != null)
            {
                Session[Constants.POOL] = poolName;
            }
            if (Session[Constants.POOL] == null)
            {
                return;
            }
            //   CreateTableHead();
            List<Person> allAttendees = new List<Person>();
            char[] poolNames = Session[Constants.POOL].ToString().ToCharArray();
            DayOfWeek day = DayOfWeek.Monday;
            foreach (char name in poolNames)
            {
                Pool pool = Manager.FindPoolByName(name.ToString());
                if (pool != null)
                {
                    day = pool.DayOfWeek;
                    foreach(Person member in pool.Members.Items)
                    {
                        bool included = false;
                        foreach(Person attendee in allAttendees)
                        {
                            if (member.PlayerId == attendee.PlayerId)
                            {
                                included = true;
                                break;
                            }
                        }
                        if (!included)
                        {
                            SetPlayedCount(member, day);
                            allAttendees.Add(member);
                        }
                    }
                    foreach(Person dropin in pool.Dropins.Items)
                    {
                        bool included = false;
                        foreach (Person attendee in allAttendees)
                        {
                            if (dropin.PlayerId == attendee.PlayerId)
                            {
                                included = true;
                                break;
                            }
                        }
                        if (!included)
                        {
                            SetPlayedCount(dropin, day);
                            allAttendees.Add(dropin);
                        }
                    }
                  }
            }
            //Statistic played count
            IEnumerable<Person> sortedAttendees = allAttendees.OrderByDescending(attendee => attendee.PlayedCount);
            int order = 1;
            foreach (Person attendee in sortedAttendees)
            {
                Player player = Manager.FindPlayerById(attendee.PlayerId);
                if (!player.IsActive || (typeof(Dropin).IsInstanceOfType(attendee) && ((Dropin)attendee).IsCoop) || player.Role == (int)Roles.Guest) continue;
                FillPreRegister(order++, attendee);
            }
            this.SurveyTable.Caption = newSeason + " Pre-register Membership (" + count + ")";
        }

        private void SetPlayedCount(Person attendee, DayOfWeek day)
        {
            int playedCount = 0;
            Player player = Manager.FindPlayerById(attendee.PlayerId);
            foreach (Pool pool in Manager.Pools)
            {
                if (pool.DayOfWeek == day)
                {
                    foreach (Game game in pool.Games)
                    {
                        if (game.AllPlayers.Items.Exists(p => p.PlayerId == player.Id && p.Status == InOutNoshow.In))
                             playedCount++;
                    }
                }
            }
            attendee.PlayedCount = playedCount;
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
        private void FillPreRegister(int order, Person attendee)
        {
            Player player = Manager.FindPlayerById(attendee.PlayerId);
             TableRow row = new TableRow();
            //Order
            TableCell cell = new TableCell();
            cell.Text = order.ToString();
            row.Cells.Add(cell);
            //Name
            cell = new TableCell();
            cell.Text = player.Name;
            if (attendee.PreRegistered)
            {
                count++;
            }
 
            row.Cells.Add(cell);
             //PlayerCount
            //cell = new TableCell();
            //cell.Text = attendee.PlayedCount.ToString(); ;
            //row.Cells.Add(cell);
             //Membership
            row.Cells.Add(createCheckBoxCell(player.Id, attendee.PreRegistered));
           this.SurveyTable.Rows.Add(row);
        }
        private TableCell createCheckBoxCell(String id, bool status)
        {
            TableCell cell = new TableCell();
            ImageButton imageBtn = new ImageButton();
            imageBtn.ID = id + ",preregister" ;
            imageBtn.ImageUrl = status ? "~/Icons/In.png" : "~/Icons/Out.png";
            imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Click += new ImageClickEventHandler(checkBox_CheckedChanged);
            cell.Controls.Add(imageBtn);
            return cell;
        }

        void checkBox_CheckedChanged(object sender, ImageClickEventArgs e)
        {
            ImageButton lbtn = (ImageButton)sender;
            Session[Constants.CURRENT_PLAYER_ID] = lbtn.ID;
            Session[Constants.CONTROL] = sender;
            this.ConfirmPopup.Show();
            return;
        }

        protected void Confirm_Click(object sender, EventArgs e){
            String idString = Session[Constants.CURRENT_PLAYER_ID].ToString();
            String id = idString.Split(',')[0];
            Player player = Manager.FindPlayerById(id);
            Person attendee = CurrentPool.Members.FindByPlayerId(id);
            if (attendee == null)
            {
                attendee = CurrentPool.Dropins.FindByPlayerId(id);
            }
            attendee.PreRegistered = !attendee.PreRegistered;
             DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

  
         protected void AddNewDropin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(DropinNameTb.Text))
            {
                return;
            }
            ImageButton lbtn = (ImageButton)sender;
            TextBox nameTb = DropinNameTb;//(TextBox)FindControl("DropinNameTb");
            Player nmb = Manager.FindPlayerByName(nameTb.Text);
            if (nmb == null)
            {
                nmb = new Player(nameTb.Text.Trim(), null, false);
                Dropin dropin = new Dropin(nmb.Id);
                CurrentPool.Dropins.Add(dropin);
            }
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
            //TableRow row = CreateDropinTableRow(dropin);
            // this.DropinTable.Rows.AddAt(this.DropinTable.Rows.Count -1, row);
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