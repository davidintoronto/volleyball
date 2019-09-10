using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VballManager
{
    public partial class LinkDevice : System.Web.UI.Page
    {
        private static String RESET = "reset";
        private static String USER_ID = "id";
        protected void Page_Load(object sender, EventArgs e)
        {
                if (Request.Params[USER_ID] == null)
                {
                    this.UsernameLb.Text = "Invalid Request !!!";
                    this.RegisterBtn.Visible = false;
                    return;
                }
                String playerId = Manager.ReversedId(Request.Params[USER_ID]);
                Player player = Manager.FindPlayerById(playerId);
                if (!this.IsPostBack)
                {
                    if (player == null)
                {
                    this.UsernameLb.Text = "Invalid Request !!!";
                    this.RegisterBtn.Visible = false;
                    return;
                }
                if (Request.Params[RESET] != null && Request.Cookies[Constants.PRIMARY_USER] != null)
                {
                    HttpCookie appCookie = new HttpCookie(Constants.PRIMARY_USER);
                    appCookie.Value = playerId;
                    appCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(appCookie);
                }
                this.UsernameLb.Text = "Link your device to user [" + player.Name + "]";

                Session[Constants.PLAYER_ID] = playerId;
            }
            if (Convert.ToString(ViewState["Generated"]) == "true")
            {
                FillUserTable(player);
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

        protected void RegisterBtn_Click(object sender, EventArgs e)
        {
            this.RegisterBtn.Visible = false;
            String playerId = (String)Session[Constants.PLAYER_ID];
            Player user = Manager.FindPlayerById(playerId);
            if (Request.Cookies[Constants.PRIMARY_USER] != null)
            {

                if (Request.Cookies[Constants.PRIMARY_USER].Value != playerId)
                {
                    this.UsernameLb.Text = "Warning !!! Your device has already linked to [" + Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER].Value).Name + "]. Please contact admin for advice.";
                    return;
                }
            }
            HttpCookie appCookie = new HttpCookie(Constants.PRIMARY_USER);
            appCookie.Value = playerId;
            appCookie.Expires = Manager.CookieExpire;
            Response.Cookies.Add(appCookie);
            this.UsernameLb.Text = "Your device has successfully linked to [" + Manager.FindPlayerById(playerId).Name + "].";
            user.DeviceLinked = true;
            DataAccess.Save(Manager);
            //if (Convert.ToString(ViewState["Generated"]) != "true")
            {
                FillUserTable(user);
                ViewState["Generated"] = "true";
            }

           
        }

        private void FillUserTable(Player currentUser)
        {
            this.UserTable.Caption = "You may authorize someone to help you with reservation if you wish.";
            this.UserTable.Visible = true;
            this.UserTable.Rows.Clear();
            IEnumerable<Player> playerQuery = Manager.Players.OrderBy(member => member.Name);
            bool alterbackcolor = false;
            foreach (Player user in playerQuery)
            {          
                if (user.Id == currentUser.Id || user.Name =="Admin")
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
            Player currentUser = Manager.FindPlayerById((String)Session[Constants.PLAYER_ID]);
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
     }
}