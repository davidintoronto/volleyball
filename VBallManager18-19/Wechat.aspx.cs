using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace VballManager
{
    public partial class Wechat : AdminBase
    {
        private const String REGISTER_LINK = "{REGISTER_LINK}";
        private const String POOL_MEMBER_COUNT = "{POOL.MEMBER.COUNT}";
        private const String POOL_NAME = "{POOL_NAME}";
        int playerPerCol = 4;
        protected void Page_Load(object sender, EventArgs e)
        {
            Application[Constants.DATA] = DataAccess.LoadReservation();
            if (!IsPostBack)
            {
                this.PrimaryMemberMessgeTb.Text = Manager.WechatPrimaryMemberMessage;
                this.MemberWechatMessageTb.Text = Manager.WechatMemberWelcomeMessage;
                this.DropinWechatMessageTb.Text = Manager.WechatDropinWelcomeMessage;
                this.PoolWechatMessageTb.Text = Manager.WechatPoolMessage;
                this.PoolListBox.DataSource = Manager.Pools;
                this.PoolListBox.DataTextField = "Name";
                this.PoolListBox.DataValueField = "Name";
                this.PoolListBox.DataBind();
            }
            this.SendToGroupBtn.OnClientClick = "if ( !confirm('Are you sure to send notification to new primary members?')) return false;";
            this.SendToDropinsBtn.OnClientClick = "if ( !confirm('Are you sure to send welcome message to all the dropins?')) return false;";
            this.SendToMembersBtn.OnClientClick = "if ( !confirm('Are you sure to send welcome message to all the members?')) return false;";
            RanderPlayerWechatPanel();
        }

        private void RanderPlayerWechatPanel()
        {
            int rows = (Manager.Players.Count + playerPerCol - 1) / playerPerCol;
            for (int row = 0; row < rows; row++)
            {
                TableRow tableRow = new TableRow();
                for (int col = 0; col < playerPerCol; col++)
                {
                    int index = row * playerPerCol + col;
                    if (index < Manager.Players.Count)
                    {
                        Player player = Manager.Players[index];
                        TableCell playerNameCell = new TableCell();
                        playerNameCell.Text = player.Name;
                        TableCell wechatNameCell = new TableCell();
                        TextBox wechatNameTb = new TextBox();
                        wechatNameTb.ID = player.Id;
                        wechatNameTb.Text = player.WechatName;
                        wechatNameTb.AutoPostBack = true;
                        wechatNameTb.TextChanged += new EventHandler(WechatNameTb_TextChanged);
                        wechatNameCell.Controls.Add(wechatNameTb);
                        tableRow.Cells.Add(playerNameCell);
                        tableRow.Cells.Add(wechatNameCell);
                    }
                }
                this.WechatNameTable.Rows.Add(tableRow);
            }
        }

        private void WechatNameTb_TextChanged(object sender, EventArgs e)
        {
            TextBox wechatNameTb = (TextBox)sender;
            Player player = Manager.FindPlayerById(wechatNameTb.ID);
            player.WechatName = wechatNameTb.Text;
            DataAccess.Save(Manager);
        }

        protected void SetWechatNameBtn_Click(object sender, EventArgs e)
        {
            foreach (Player player in Manager.Players)
            {
                player.WechatName = player.Name;
            }
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        protected void SendToMembersBtn_Click(object sender, EventArgs e)
        {
            IEnumerable<Player> players = Manager.Players.FindAll(player => player.IsActive && !String.IsNullOrEmpty(player.WechatName));
            foreach (Player player in players)
            {
                String registerLink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath) + "/" + Constants.REGISTER_DEVICE_PAGE + "?id=" + Manager.ReversedId(player.Id); 
                if (player.IsRegisterdMember)
                {
                    String message = this.MemberWechatMessageTb.Text.Replace(REGISTER_LINK, registerLink);
                    Manager.AddNotifyWechatMessage(player, message);
                    decimal total = 0;
                    foreach (Fee fee in player.Fees)
                    {
                        if (!fee.IsPaid) total = total + fee.Amount;
                    }
                    if (total > 0)
                    {
                        message = " You membership fee is " + Manager.RegisterMembeshipFee + ". You unpaid fee is " + total + ". If you would like to pay e-transfer, please send to " + Manager.AdminEmail + ". Thanks";
                        Manager.AddNotifyWechatMessage(player, message);
                    }
                }
             }
            Manager.WechatPrimaryMemberMessage = this.MemberWechatMessageTb.Text;
            DataAccess.Save(Manager);
        }

        protected void SendPrimaryMemberNotificationWechatMessageBtn_Click(object sender, EventArgs e)
        {
            if (this.PoolListBox.SelectedIndex >= 0)
            {
                Pool pool = Manager.FindPoolByName(this.PoolListBox.SelectedValue);
                String message = this.PoolWechatMessageTb.Text.Replace(POOL_MEMBER_COUNT, pool.Members.Count.ToString());
                Manager.AddNotifyWechatMessage(pool, message);
                foreach (Member member in pool.Members.Items)
                {
                    Player player = Manager.FindPlayerById(member.PlayerId);
                    message = this.PrimaryMemberMessgeTb.Text.Replace(POOL_MEMBER_COUNT, pool.Members.Count.ToString()).Replace(POOL_NAME, pool.Name);
                    Manager.AddNotifyWechatMessage(player, message);
                    message = "Congratus! You have become the primary member in pool " + pool.Name;
                    Manager.AddNotifyWechatMessage(pool, player, message);
                }
                Manager.WechatPoolMessage = this.PoolListBox.Text;
                Manager.WechatPrimaryMemberMessage = this.PrimaryMemberMessgeTb.Text;
                DataAccess.Save(Manager);
            }
            else 
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Pool is not selected')", true);
                return;
            }
        }

        protected void SendToDropinsBtn_Click(object sender, EventArgs e)
        {
            IEnumerable<Player> players = Manager.Players.FindAll(player => player.IsActive && !String.IsNullOrEmpty(player.WechatName));
            foreach (Player player in players)
            {
                String registerLink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath) + "/" + Constants.REGISTER_DEVICE_PAGE + "?id=" + Manager.ReversedId(player.Id);
                if (!player.IsRegisterdMember)
                {
                    String message = this.DropinWechatMessageTb.Text.Replace(REGISTER_LINK, registerLink);
                    Manager.AddNotifyWechatMessage(player, message);
                }
            }
            DataAccess.Save(Manager);
        }
 
    }
}