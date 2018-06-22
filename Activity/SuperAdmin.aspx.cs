using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reservation
{
    public partial class SuperAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // return;
                Reservations = (Reservation)Application[Constants.RESERVATION];
                this.SystemTitleTb.Text = Reservations.Title;
                this.SharePlayerstCb.Checked = Reservations.SharePlayers;
                this.OpenAdminCb.Checked = Reservations.OpenAdmin;
                //Bind activity type
                int selectPoolIndex = this.ActivityTypeListbox.SelectedIndex;
                this.ActivityTypeListbox.DataSource = Reservations.ActivityTypes;
                this.ActivityTypeListbox.DataTextField = "Name";
                this.ActivityTypeListbox.DataValueField = "Id";
                this.ActivityTypeListbox.DataBind();
                this.ActivityTypeListbox.SelectedIndex = selectPoolIndex;
            }
         }

        private Reservation Reservations
        {
            get
            {
                return (Reservation)Application[Constants.RESERVATION];

            }
            set { }
        }


        protected void SaveSystemBtn_Click(object sender, EventArgs e)
        {
            Reservations.Title = this.SystemTitleTb.Text;
            Reservations.SharePlayers = this.SharePlayerstCb.Checked;
            Reservations.OpenAdmin = this.OpenAdminCb.Checked;
            DataAccess.Save(Reservations);
            Response.Redirect(Request.RawUrl);
        }

        protected void AddActivityTypeBtn_Click(object sender, EventArgs e)
        {
            if (ActivityNameTb.Text != null && ActivityNameTb.Text != "")
            {
                ActivityType type = new ActivityType(ActivityNameTb.Text);
                Reservations.ActivityTypes.Add(type);
                DataAccess.Save(Reservations);
                this.ActivityTypeListbox.DataSource = Reservations.ActivityTypes;
                this.ActivityTypeListbox.DataBind();
            }
        }

        protected void ActivityTypeListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ActivityTypeListbox.SelectedIndex >= 0)
            {
                ActivityType aType = Reservations.FindActivityTypeById(this.ActivityTypeListbox.SelectedValue);
                this.ActivityNameTb.Text = aType.Name;
                this.FeeTypeListbox.DataSource = aType.FeeTypes;
                this.FeeTypeListbox.DataTextField = "Name";
                this.FeeTypeListbox.DataValueField = "Id";
                this.FeeTypeListbox.DataBind();
            }

        }

        protected void DeleteActivityTypeBtn_Click(object sender, EventArgs e)
        {
            if (this.ActivityTypeListbox.SelectedIndex >= 0)
            {
                ActivityType aType = Reservations.FindActivityTypeById(this.ActivityTypeListbox.SelectedValue);
                Reservations.ActivityTypes.Remove(aType);
                DataAccess.Save(Reservations);
                this.ActivityTypeListbox.DataSource = Reservations.ActivityTypes;
                this.ActivityTypeListbox.DataBind();
            }
        }

        protected void UpdateActivityTypeBtn_Click(object sender, EventArgs e)
        {
            if (this.ActivityTypeListbox.SelectedIndex >= 0 && ActivityNameTb.Text != null && ActivityNameTb.Text != "")
            {
                ActivityType aType = Reservations.FindActivityTypeById(this.ActivityTypeListbox.SelectedValue);
                aType.Name = ActivityNameTb.Text;
                DataAccess.Save(Reservations);
                this.ActivityTypeListbox.DataSource = Reservations.ActivityTypes;
                this.ActivityTypeListbox.DataBind();
            }
        }

        protected void AddFeeTypeBtn_Click(object sender, EventArgs e)
        {
            if (this.ActivityTypeListbox.SelectedIndex >= 0 && this.FeeTypeNameTb.Text != null && this.FeeTypeNameTb.Text != "")
            {
                ActivityType aType = Reservations.FindActivityTypeById(this.ActivityTypeListbox.SelectedValue);
                FeeType fType = new FeeType(this.FeeTypeNameTb.Text);
                aType.FeeTypes.Add(fType);
                DataAccess.Save(Reservations);
                this.FeeTypeListbox.DataSource = aType.FeeTypes;
                this.FeeTypeListbox.DataBind();
            }

        }
        protected void FeeTypeListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ActivityTypeListbox.SelectedIndex >= 0 && this.FeeTypeListbox.SelectedIndex >= 0)
            {
                FeeType fType = Reservations.FindFeeTypeById(this.ActivityTypeListbox.SelectedValue, this.FeeTypeListbox.SelectedValue);
                this.FeeTypeNameTb.Text = fType.Name;
            }

        }

        protected void DeleteFeeTypeBtn_Click(object sender, EventArgs e)
        {
            if (this.ActivityTypeListbox.SelectedIndex >= 0 && this.FeeTypeListbox.SelectedIndex >= 0)
            {
                ActivityType aType = Reservations.FindActivityTypeById(this.ActivityTypeListbox.SelectedValue);
                FeeType fType = Reservations.FindFeeTypeById(this.ActivityTypeListbox.SelectedValue, this.FeeTypeListbox.SelectedValue);
                aType.FeeTypes.Remove(fType);
                DataAccess.Save(Reservations);
                this.FeeTypeListbox.DataSource = aType.FeeTypes;
                this.FeeTypeListbox.DataBind();
            }
        }

        protected void UpdateFeeTypeBtn_Click(object sender, EventArgs e)
        {
            if (this.ActivityTypeListbox.SelectedIndex >= 0 && this.FeeTypeListbox.SelectedIndex >= 0 && FeeTypeNameTb.Text != null && FeeTypeNameTb.Text != "")
            {
                ActivityType aType = Reservations.FindActivityTypeById(this.ActivityTypeListbox.SelectedValue);
                FeeType fType = Reservations.FindFeeTypeById(this.ActivityTypeListbox.SelectedValue, this.FeeTypeListbox.SelectedValue);
                fType.Name = FeeTypeNameTb.Text;
                 DataAccess.Save(Reservations);
                 this.FeeTypeListbox.DataSource = aType.FeeTypes;
                this.FeeTypeListbox.DataBind();
            }
        }
    }
}