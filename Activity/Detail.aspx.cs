using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reservation
{
    public partial class Detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["id"] == null)
            {
                Response.Redirect("Activity.aspx");
                return; ;
            }
            String id = Request.Params["id"].ToString();
            Player player = Reservations.FindPlayerById(id);
            bool mark = false;
            this.DetailTable.Caption = player.Name;
            if (!IsPostBack)
            {
                this.PasscodeTb.Text = player.Passcode;
                this.NameTb.Text = player.Name;
                mark = player.Marked;
            }
            this.DeleteUserLbl.Visible = true;
            this.DeleteUserBtn.Visible = true;
            this.DeleteUserBtn.CommandArgument = player.Id;

            TableRow row = new TableRow();
            TableCell labelCell = new TableCell();
            labelCell.Text = "Change Name";
            row.Cells.Add(labelCell);
            TableCell valueCell = new TableCell();
            valueCell.HorizontalAlign = HorizontalAlign.Right;
            // PasscodeTb.Font.Size = new FontUnit("1.2em");
            valueCell.Controls.Add(NameTb);
            PasscodeTb.Font.Size = FontUnit.XXLarge;
            row.Cells.Add(valueCell);
            TableCell saveCell = new TableCell();
            saveCell.HorizontalAlign = HorizontalAlign.Right;
            ImageButton imageBtn = new ImageButton();
            imageBtn.ID = id + "," + this.DetailTable.Caption + ",saveNameBtn";
            imageBtn.ImageUrl = "~/Icons/Save.png";
            imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
            imageBtn.Click += new ImageClickEventHandler(SaveName_Click);
            saveCell.Controls.Add(imageBtn);
            row.Cells.Add(saveCell);
            this.DetailTable.Rows.Add(row);

        }


         private Reservation Reservations
        {
            get
            {
                return (Reservation)Application[Constants.RESERVATION];

            }
            set { }
        }
        protected void SaveName_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton lbtn = (ImageButton)sender;
            String currentUserId = lbtn.ID.Split(',')[0];
             Player member = Reservations.FindPlayerById(currentUserId);
            if (member != null)
            {
                member.Name = NameTb.Text.Trim();
            }
            DataAccess.Save(Reservations);
            Response.Redirect(Request.RawUrl);
        }

  
        protected void BackBtn_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Activity.aspx");

        }

 
        protected void DeleteUserBtn_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            String id = btn.CommandArgument;
            Player player = Reservations.FindPlayerById(id);
            if (player.FeeIds.Count == 0)
            {
                foreach (Game game in Reservations.Games)
                {
                    game.Players.Remove(id);
                    game.WaitingListIds.Remove(id);
                }
                Reservations.DeletePlayer(id);
                DataAccess.Save(Reservations);
                Response.Redirect("Activity.aspx");
            }
        }
     }
}