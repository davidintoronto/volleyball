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
               this.RegisterLinkPanel.Visible = false;
               if (Request.Params[RESET] == null && Request.Cookies[Constants.PRIMARY_USER] != null)
                {
                     this.RegisterCodePanel.Visible = false;
                    String playerId = Request.Cookies[Constants.PRIMARY_USER].Value;
                    Player player = Manager.FindPlayerById(playerId);
                    FillReservationLinkTable(player);
                }
            /*   else if (Request.UserAgent.ToLower().Contains("micromessenger"))
               {
                   this.RegisterCodeLb.Text = "Please open exteranal web browser by clicking the option (3 dots at top right)";
                   this.RegisterCodeTb.Visible = false;
                   this.RegisterCodeBtn.Visible = false;
               }
*/
            }
            else
            {
                this.RegisterCodePanel.Visible = false;
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
                    this.UsernameLb.Text = "Register your device to user [" + player.Name + "]";

                    Session[Constants.PLAYER_ID] = playerId;
                }
                if (Convert.ToString(ViewState["Generated"]) == "true")
                {
                    FillReservationLinkTable(player);
                    FillUserTable(player);
                }
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
                    this.UsernameLb.Text = "Warning !!! Your device has already registered as [" + Manager.FindPlayerById(Request.Cookies[Constants.PRIMARY_USER].Value).Name + "]. Please contact admin for advice.";
                    return;
                }
            }
            HttpCookie appCookie = new HttpCookie(Constants.PRIMARY_USER);
            //appCookie.Domain = "volleyball.gear.host"; 
            appCookie.Value = playerId;
            appCookie.Expires = Manager.CookieExpire;
            Response.Cookies.Add(appCookie);
            this.UsernameLb.Text = "Your device has successfully registered as [" + Manager.FindPlayerById(playerId).Name + "].";
            user.DeviceLinked = true;
            DataAccess.Save(Manager);
            //if (Convert.ToString(ViewState["Generated"]) != "true")
            {
                FillReservationLinkTable(user);
                FillUserTable(user);
                ViewState["Generated"] = "true";
            }

           
        }

        protected void RegisterCodeBtn_Click(object sender, EventArgs e)
        {
            if (this.RegisterCodeTb.Text == "")
            {
                return;
            }
             String playerId = Manager.ReversedId(this.RegisterCodeTb.Text);
            Player user = Manager.FindPlayerById(playerId);
            if (user == null)
            {
                    this.RegisterCodeLb.Text = "Wrong register code. Re-entry the code and try again.";
                    return;
             }
            this.RegisterCodeBtn.Visible = false;
            this.RegisterCodeTb.Visible = false;
           HttpCookie appCookie = new HttpCookie(Constants.PRIMARY_USER);
           // appCookie.Domain = "volleyball.gear.host";
            appCookie.Value = playerId;
            appCookie.Expires = Manager.CookieExpire;
            Response.Cookies.Add(appCookie);
            this.RegisterCodeLb.Text = "Your device has successfully registered as [" + user.Name + "].";
            user.DeviceLinked = true;
            DataAccess.Save(Manager);
            //if (Convert.ToString(ViewState["Generated"]) != "true")
            {
                FillReservationLinkTable(user);
                //FillUserTable(user);
                ViewState["Generated"] = "true";
            }


        }

        private void FillReservationLinkTable(Player currentUser)
        {
            //Show reservation links
            this.ReserveLinkTable.Caption = "Open reservation links below";
            this.ReserveLinkTable.Rows.Clear();
            foreach (Pool pool in Manager.Pools)
            {
                if (currentUser.Name=="Admin" || pool.Members.Exists(attendee => attendee.Id == currentUser.Id) || pool.Dropins.Exists(attendee => attendee.Id == currentUser.Id))
                {
                    TableRow row = new TableRow();
                    TableCell cell = new TableCell();
                    HyperLink link = new HyperLink();
                    link.Text = pool.DayOfWeek.ToString() + " Pool " + pool.Name;
                    link.NavigateUrl = "Default.aspx?Pool=" + pool.Name;
                    cell.Controls.Add(link);
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    row.Cells.Add(cell);
                    this.ReserveLinkTable.Rows.Add(row);
                }
            }
        }
        private void FillUserTable(Player currentUser)
        {
            this.UserTable.Caption = "You may authorize someone else to help you with reservation if you want.";
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