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
        protected void Page_Load(object sender, EventArgs e)
        {
                 if (Request.Params[RESET] != null)
                {
                    ResetCookie();
                    Response.Redirect(Request.AppRelativeCurrentExecutionFilePath);
                    return;
                }
                if (Request.Cookies[Constants.PRIMARY_USER] != null)
                {
                    String userId = Request.Cookies[Constants.PRIMARY_USER][Constants.PLAYER_ID];
                     String passcode = Request.Cookies[Constants.PRIMARY_USER][Constants.PASSCODE];
                      Player player = Manager.FindPlayerById(userId);
                         if (!String.IsNullOrEmpty(player.Passcode) && player.Passcode == passcode)
                         {
                             FillReservationLinkTable(player);
                             return;
                         }
                     ResetCookie();
                     Response.Redirect(Request.RawUrl);
                     return;
               }
                else//Register user or login
                {
                       if (!IsPostBack)
                    {
                       // IEnumerable<Player> playerQuery = Manager.Players.FindAll(player => player.Passcode == null).OrderBy(player => player.Name);
                        IEnumerable<Player> playerQuery = Manager.ActivePlayers.OrderBy(user => user.Name);
                        this.UserList.DataSource = playerQuery;
                        this.UserList.DataTextField = "Name";
                        this.UserList.DataValueField = "Id";
                        this.UserList.DataBind();
                    }
                    else
                    {
                        if (this.UserList.SelectedIndex >= 0)
                        {
                            Player user = Manager.FindPlayerById(this.UserList.SelectedValue);
                             if (user.Passcode == null)
                            {
                                this.LoginBtn.Text = "Register";
                            }
                            else
                            {
                                this.LoginBtn.Text = "Login";
                            }
                        }
                    }

                }
        }

        private void ResetCookie()
        {
             HttpCookie appCookie = new HttpCookie(Constants.PRIMARY_USER);
            appCookie[Constants.PLAYER_ID] = "";
            appCookie[Constants.PASSCODE] = "";
            appCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(appCookie);
        }


        private VolleyballClub Manager
        {
            get
            {
                return (VolleyballClub)Application[Constants.DATA];

            }
            set { }
        }



        private void SetUserCookie(Player user)
        {
            HttpCookie appCookie = new HttpCookie(Constants.PRIMARY_USER);
            //appCookie.Domain = "volleyball.gear.host"; 
            appCookie[Constants.PLAYER_ID] = user.Id;
            appCookie[Constants.PASSCODE] = user.Passcode;
            appCookie.Expires = Manager.CookieExpire;
            Response.Cookies.Add(appCookie);
            user.DeviceLinked = true;
        }

        private void FillReservationLinkTable(Player currentUser)
        {
            //Show reservation links
            this.ReserveLinkTable.Caption = "Open reservation links below";
            this.ReserveLinkTable.Rows.Clear();
            foreach (Pool pool in Manager.Pools)
            {
                if (Manager.ActionPermitted(Actions.View_All_Pools, currentUser.Role) || pool.Members.Exists(attendee => attendee.Id == currentUser.Id) || pool.Dropins.Exists(attendee => attendee.Id == currentUser.Id))
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

        protected void UserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PasswordTb.Text = "";
        }

        protected void LoginBtn_Click(object sender, EventArgs e)
        {
            if (this.PasswordTb.Text.Trim()=="")
            {
                this.LoginLabel.Text = "Please enter password and try again";
                return;
            }
            if (this.UserList.SelectedIndex >= 0)
            {
                Player user = Manager.FindPlayerById(this.UserList.SelectedValue);
                Session[Constants.PLAYER_ID] = user.Id;
                if (String.IsNullOrEmpty(user.Passcode))
                {
                    user.Passcode = this.PasswordTb.Text;
                    SetUserCookie(user);
                    DataAccess.Save(Manager);
                    this.LoginUserPanel.Visible = false;
                    FillReservationLinkTable(user);
                    ViewState["Generated"] = "true";
                }
                else if (user.Passcode == this.PasswordTb.Text)
                {
                    SetUserCookie(user);
                    this.LoginUserPanel.Visible = false;
                    FillReservationLinkTable(user);
                }
                else
                {
                    this.LoginLabel.Text = "Wrong password! try again";
                  }
            }
        }
    }
}