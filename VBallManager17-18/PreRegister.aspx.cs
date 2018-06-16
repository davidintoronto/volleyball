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
            if (CurrentPool == null)
            {
                return;
            }
            //   CreateTableHead();
            List<Member> allMembers = new List<Member>();
            allMembers.AddRange(CurrentPool.Members);
             foreach (Member member in allMembers)
            {
                FillPreRegister(member, true);
            }
            List<Dropin> allDropins = new List<Dropin>();
            allDropins.AddRange(CurrentPool.Dropins);
            foreach (Dropin dropin in allDropins)
            {
                if (!dropin.IsSuspended)
                {
                    FillPreRegister(dropin, false);
                }
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
        private void FillPreRegister(Attendee attendee, bool isMember)
        {
            Player player = Manager.FindPlayerById(attendee.Id);
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = player.Name;
            if (attendee.PreRegistered)
            {
                count++;
            }
            if (isMember)
            {
                cell.ForeColor = System.Drawing.Color.Blue;
            }
            row.Cells.Add(cell);
            row.Cells.Add(createCheckBoxCell(player.Id, attendee.PreRegistered));
             this.SurveyTable.Rows.Add(row);
            this.SurveyTable.Caption = "2017-2018 Member Pre-register(" + count + ")";
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
            Session[Constants.CURRENT_USER_ID] = lbtn.ID;
            Session[Constants.CONTROL] = sender;
            this.ConfirmPopup.Show();
            return;
        }

        protected void Confirm_Click(object sender, EventArgs e){
            String idString = Session[Constants.CURRENT_USER_ID].ToString();
            String id = idString.Split(',')[0];
            Player player = Manager.FindPlayerById(id);
            Attendee attendee = CurrentPool.Members.Find(member => member.Id == id);
            if (attendee == null)
            {
                attendee = CurrentPool.Dropins.Find(dropin => dropin.Id == id);
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