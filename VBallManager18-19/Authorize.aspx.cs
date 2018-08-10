using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class Authorize : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if (Request.Cookies[Constants.PRIMARY_USER] != null)
            {
                String existingUserId = Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID];
                Player user = Manager.FindPlayerById(existingUserId);
                if (user != null)
                {
                    FillUserTable(user);
                    return;
                }
            }
            Response.Redirect(Constants.REQUEST_REGISTER_LINK_PAGE);
            return;
         }



        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }

        private void FillUserTable(Player currentUser)
        {
            this.UserTable.Caption = "Select your agents.";
            this.UserTable.Visible = true;
            this.UserTable.Rows.Clear();
            IEnumerable<Player> playerQuery = Manager.ActivePlayers.OrderBy(member => member.Name);
            bool alterbackcolor = false;
            foreach (Player user in playerQuery)
            {
                if (user.Id == currentUser.Id)
                {
                    continue;
                }
                TableRow row = new TableRow();
                if (alterbackcolor)
                {
                    row.BackColor = UserTable.BorderColor;
                }
                alterbackcolor = !alterbackcolor;
                TableCell nameCell = new TableCell();
                LinkButton lbtn = new LinkButton();
                lbtn.Text = user.Name;
                lbtn.Font.Bold = true;
                lbtn.Font.Size = new FontUnit(Constants.LINKBUTTON_FONTSIZE);
                lbtn.ID = user.Id + ",USER";
                nameCell.Controls.Add(lbtn);

                row.Cells.Add(nameCell);
                TableCell statusCell = new TableCell();
                statusCell.HorizontalAlign = HorizontalAlign.Right;
                ImageButton imageBtn = new ImageButton();
                imageBtn.ID = user.Id;

                imageBtn.ImageUrl = currentUser.AuthorizedUsers.Contains(user.Id) ? "~/Icons/In.png" : "~/Icons/Out.png";
                imageBtn.Click += new ImageClickEventHandler(Authorize_Click);
                imageBtn.Width = new Unit(Constants.IMAGE_BUTTON_SIZE);
                imageBtn.Height = new Unit(Constants.IMAGE_BUTTON_SIZE);
                //  imageBtn.CssClass = "imageBtn";
                statusCell.Controls.Add(imageBtn);
                row.Cells.Add(statusCell);
                this.UserTable.Rows.Add(row);
            }
        }

        protected void Authorize_Click(object sender, EventArgs e)
        {
            ImageButton lbtn = (ImageButton)sender;
            String userid = lbtn.ID;
            Player currentUser = Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID]);
            if (currentUser.AuthorizedUsers.Contains(userid))
            {
                currentUser.AuthorizedUsers.Remove(userid);
            }
            else
            {
                currentUser.AuthorizedUsers.Add(userid);
            }
            DataAccess.Save(Manager);
            FillUserTable(currentUser);

        }

    
        protected void GotoNextBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.POOL_LINK_LIST_PAGE);
        }
    }
}