using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reservation
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // return;
                Reservations = (Reservation)Application[Constants.RESERVATION];

                //Bind Date list
                this.GameList.DataSource = Reservations.Games;
                this.GameList.DataTextField = "Date";
                this.GameList.DataTextFormatString = "{0:d}";
                this.GameList.DataValueField = "Id";
                this.GameList.DataBind();
                FillUpFields();

                //
                this.DeleteGameBtn.OnClientClick = "if ( !confirm('Are you sure you want to delete?')) return false;";
            }
            this.PasswordLb.Visible = Reservations.OpenAdmin;
            this.PasswordTb.Visible = Reservations.OpenAdmin;
            Page.Title = Reservations.Title;
            ((Label)Master.FindControl("TitleLabel")).Text = Reservations.Title;

        }

        private Reservation Reservations
        {
            get
            {
                return (Reservation)Application[Constants.RESERVATION];

            }
            set { }
        }

        protected void GameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.GameList.SelectedIndex < 0)
            {
                return;
            }
            FillUpFields();
        }

        private void FillUpFields()
        {
            if (this.GameList.SelectedIndex >= 0)
            {
                GameDateTb.Text = this.GameList.SelectedItem.Text;
                Game game = Reservations.FindGameByDate(DateTime.Parse(this.GameList.SelectedItem.Text));
                this.TitleTb.Text = game.Title;
                this.ScheduledTimeTb.Text = game.Time;
                this.LocationTb.Text = game.Location;
                this.MessageTextTb.Text = game.Message;
                this.MaxPlayersTb.Text = game.MaxPlayers.ToString();
                this.PublishCb.Checked = game.Publish;
                this.WechatNameTb.Text = game.WechatName;
                this.PasswordTb.Text = "";
            }
        }

        protected void AddGameBtn_Click(object sender, EventArgs e)
        {
            DateTime date;
            try
            {
                date = DateTime.Parse(GameDateTb.Text);
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong game date format!  Fix it and try again')", true);
                return;
            }

            if (date < DateTime.Today)
            {
                return;
            }
            if (Reservations.OpenAdmin && String.IsNullOrEmpty(this.PasswordTb.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Please enter password and write it down for future use.')", true);
                return;
            }
            Game game = new Game(date);
            game.Title = this.TitleTb.Text;
            game.Time = this.ScheduledTimeTb.Text;
            game.Location = this.LocationTb.Text;
            game.Message = this.MessageTextTb.Text;
            game.MaxPlayers = int.Parse(this.MaxPlayersTb.Text);
            game.WechatName = this.WechatNameTb.Text;
            game.Publish = this.PublishCb.Checked;
            if (!String.IsNullOrEmpty(this.PasswordTb.Text.Trim()))
            {
                game.DeletePassword = this.PasswordTb.Text;
            }
            Reservations.Games.Add(game);

             this.GameList.DataSource = Reservations.Games;
            this.GameList.DataBind();
            this.GameList.SelectedIndex = this.GameList.Items.Count - 1;
           DataAccess.Save(Reservations);
        }


        protected void UpdateGameBtn_Click(object sender, EventArgs e)
        {
            if (this.GameList.SelectedItem == null)
            {
                return;
            }
            try
            {
                DateTime gameDate = DateTime.Parse(GameDateTb.Text);
                Game game = Reservations.FindGameById(GameList.SelectedItem.Value);
                if (Reservations.OpenAdmin && this.PasswordTb.Text != game.DeletePassword)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong password for this game')", true);
                    return;
                }

                if (game != null)
                {
                    game.Date = gameDate;
                    game.Title = this.TitleTb.Text;
                    game.Time = this.ScheduledTimeTb.Text;
                    game.Location = this.LocationTb.Text;
                    game.Message = this.MessageTextTb.Text;
                    game.MaxPlayers = int.Parse(this.MaxPlayersTb.Text);
                    game.Publish = this.PublishCb.Checked;
                    game.WechatName = this.WechatNameTb.Text;
                    this.GameList.DataSource = Reservations.Games;
                    this.GameList.DataBind();
                    this.GameList.SelectedValue = game.Id;
                    DataAccess.Save(Reservations);
                }
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong game date format!  Fix it and try again')", true);
                return;
            }
       }


        protected void DeleteGameBtn_Click(object sender, EventArgs e)
        {
            if (GameList.SelectedIndex < 0)
            {
                return;
            }
            Game game = Reservations.FindGameById(GameList.SelectedItem.Value);
            if (game != null)
            {
                if (Reservations.OpenAdmin && this.PasswordTb.Text != game.DeletePassword)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Wrong password for this game')", true);
                    return;
                }
                Reservations.Games.Remove(game);
                // this.GameDateTb.Text = "";
                //  this.GameList.SelectedIndex = -1;
                // this.GameList.DataBind();
                this.GameList.DataSource = Reservations.Games;
                this.GameList.DataBind();
                this.GameList.SelectedIndex = -1;
                this.GameDateTb.Text = "";
                this.TitleTb.Text = "";
                this.ScheduledTimeTb.Text = "";
                this.LocationTb.Text = "";
                this.MessageTextTb.Text = "";
                this.MaxPlayersTb.Text = "";
                this.PasswordTb.Text = "";
                this.PublishCb.Checked = false;

                DataAccess.Save(Reservations);
                this.GameList.DataSource = Reservations.Games;
                this.GameList.DataBind();
            }


        }

        protected void ResetDropinsBtn_Click(object sender, EventArgs e)
        {
            if (GameList.SelectedIndex < 0)
            {
                return;
            }
            Game game = Reservations.FindGameByDate(DateTime.Parse(GameList.SelectedItem.Text));
            game.Players.Clear();
            DataAccess.Save(Reservations);
        }

        protected void GotoHomePage_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}