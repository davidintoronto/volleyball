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
        private const String REGISTER_LINK = "{register.url}";
        private const String POOL_MEMBER_COUNT = "{pool.member.count}";
        private const String POOL_NAME = "{pool.name}";
        private const String PLAYER_NAME = "{player.name}";
        private const String MIN="min";
        private const String MAX = "max";
        private const String ADD = "Add";
        private const String DELETE = "Delete";
        private const String TYPE = "type";
        
        int playerPerCol = 4;
        private List<int> playerNumbers = new List<int>() { 14, 13, 12, 11, 10, 8, 7, 1 };
        private List<String> types = new List<String>() {"Reserve", "Cancel"};
        protected void Page_Load(object sender, EventArgs e)
        {
            Application[Constants.DATA] = DataAccess.LoadReservation();
            if (!IsPostBack)
            {
                this.PrimaryMemberMessgeTb.Text = Manager.WechatNotifier.WechatPrimaryMemberMessage;
                this.WelcomeMemberWechatMessageTb.Text = Manager.WechatNotifier.WechatMemberWelcomeMessage;
                this.WelcomeDropinWechatMessageTb.Text = Manager.WechatNotifier.WechatDropinWelcomeMessage;
                this.PoolWechatMessageTb.Text = Manager.WechatNotifier.WechatPoolMessage;
                this.TestAllTb.Text = Manager.WechatNotifier.WechatToAllTestMessage;
                this.WechatNotifyCb.Checked = Manager.WechatNotifier.Enable;
                this.PoolListBox.DataSource = Manager.Pools;
                this.PoolListBox.DataTextField = "Name";
                this.PoolListBox.DataValueField = "Name";
                this.PoolListBox.DataBind();
                this.PlayerListBox.DataSource = Manager.Players.FindAll(player=>player.IsActive&&!String.IsNullOrEmpty(player.WechatName)).OrderBy(player=>player.Name);
                this.PlayerListBox.DataTextField = "Name";
                this.PlayerListBox.DataValueField = "Id";
                this.PlayerListBox.DataBind();
            }
            this.SendToGroupBtn.OnClientClick = "if ( !confirm('Are you sure to send notification to new primary members?')) return false;";
            this.SendToDropinsBtn.OnClientClick = "if ( !confirm('Are you sure to send welcome message to all the dropins?')) return false;";
            this.SendWelcomeToPlayersBtn.OnClientClick = "if ( !confirm('Are you sure to send welcome message to all the members?')) return false;";
            this.TestToAllBtn.OnClientClick = "if ( !confirm('Are you sure to send test messages to all?')) return false;";
            RanderEmoMessagePanel();
            RanderPlayerWechatPanel();
        }

        private void RanderEmoMessagePanel()
        {
            IOrderedEnumerable<EmoMessage> orderedEmoMessages = WechatNotifier.EmoMessages.OrderBy(emo => emo.Type).ThenBy(emo => emo.Min).ThenBy(emo => emo.Max);
            foreach (EmoMessage emoMessage in orderedEmoMessages)
            {
                CreateEmoTableRow(emoMessage);
            }
            CreateEmoTableRow(new EmoMessage());
        }

        private void BindEmoTypeDropdownList(DropDownList ddl)
        {
            foreach (int value in Enum.GetValues(typeof(EmoTypes)))
            {
                ddl.Items.Add(new ListItem(Enum.GetName(typeof(EmoTypes), value), value.ToString()));
            }
        }


        private void CreateEmoTableRow(EmoMessage emoMessage)
        {
            TableRow tableRow = new TableRow();
            //
            TableCell typeCell = new TableCell();
            DropDownList typeDDL = new DropDownList();
            typeDDL.ID = emoMessage.Id + "," + TYPE;
            BindEmoTypeDropdownList(typeDDL);
            typeDDL.DataBind();
            typeDDL.SelectedValue = emoMessage.Type.ToString();
            typeCell.Controls.Add(typeDDL);
            tableRow.Cells.Add(typeCell);
            //min
            TableCell minCell = new TableCell();
            DropDownList minDDL = new DropDownList();
            minDDL.ID = emoMessage.Id + "," + MIN;
            minDDL.DataSource = playerNumbers;
            minDDL.DataBind();
            minDDL.SelectedValue = emoMessage.Min.ToString();
            minCell.Controls.Add(minDDL);
            tableRow.Cells.Add(minCell);
            //max
            TableCell maxCell = new TableCell();
            DropDownList maxDDL = new DropDownList();
            maxDDL.ID = emoMessage.Id + "," + MAX;
            maxDDL.DataSource = playerNumbers;
            maxDDL.DataBind();
            maxDDL.SelectedValue = emoMessage.Max.ToString();
            maxCell.Controls.Add(maxDDL);
            tableRow.Cells.Add(maxCell);
            //
            TableCell messageCell = new TableCell();
            TextBox messageTb = new TextBox();
            messageTb.ID = emoMessage.Id + ",message";
            messageTb.Width = 800;
            messageTb.Text = emoMessage.Message;
            messageCell.Controls.Add(messageTb);
            tableRow.Cells.Add(messageCell);
            TableCell actionCell = new TableCell();
            Button actionBtn = new Button();
            actionBtn.ID = emoMessage.Id + "," + (String.IsNullOrEmpty(emoMessage.Message) ? ADD : DELETE);
            actionBtn.Text = String.IsNullOrEmpty(emoMessage.Message) ? ADD : DELETE;
            actionBtn.Click += new EventHandler(ActionEmoMessage_Click);
            actionBtn.OnClientClick = "if ( !confirm('Are you sure?')) return false;";
            actionCell.Controls.Add(actionBtn);
            tableRow.Cells.Add(actionCell);
            if (!String.IsNullOrEmpty(emoMessage.Message))
            {
                minDDL.AutoPostBack = true;
                minDDL.SelectedIndexChanged += new EventHandler(EmoMessageDDL_Changed);
                maxDDL.AutoPostBack = true;
                maxDDL.SelectedIndexChanged += new EventHandler(EmoMessageDDL_Changed);
                typeDDL.AutoPostBack = true;
                typeDDL.SelectedIndexChanged += new EventHandler(EmoMessageDDL_Changed);
                messageTb.AutoPostBack = true;
                messageTb.TextChanged += new EventHandler(EmoMessage_Changed);
                tableRow.BackColor = System.Drawing.Color.CadetBlue;
            }
            this.EmoTable.Rows.Add(tableRow);
        }

        private void EmoMessageDDL_Changed(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            String[] ids = ddl.ID.Split(',');
            EmoMessage emoMessage = WechatNotifier.EmoMessages.Find(emo => emo.Id == ids[0]);
            if (ids[1] == MIN) emoMessage.Min = int.Parse(ddl.Text);
            else if (ids[1] == MAX) emoMessage.Max = int.Parse(ddl.Text);
            else if (ids[1] == TYPE) emoMessage.Type = int.Parse(ddl.SelectedValue);
            DataAccess.Save(Manager);
        }
        private void EmoMessage_Changed(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            String[] ids = tb.ID.Split(',');
            EmoMessage emoMessage = WechatNotifier.EmoMessages.Find(emo => emo.Id == ids[0]);
            emoMessage.Message = tb.Text;
            DataAccess.Save(Manager);
        }

        private void ActionEmoMessage_Click(object sender, EventArgs e)
        {
            Button tb = (Button)sender;
            String[] ids = tb.ID.Split(',');
            if (ids[1] == ADD)
            {
                int min = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl( "," + MIN)).Text);
                int max = int.Parse(((DropDownList)this.Master.FindControl("MainContent").FindControl( "," + MAX)).Text);
                String typeString = ((DropDownList)this.Master.FindControl("MainContent").FindControl("," + TYPE)).SelectedValue;
                int type = int.Parse(typeString);
                String message = ((TextBox)this.Master.FindControl("MainContent").FindControl(",message")).Text;
                EmoMessage emoMessage = new EmoMessage(type, min, max, message);
                WechatNotifier.EmoMessages.Add(emoMessage);
            }
            else if (ids[1] == DELETE)
            {
                EmoMessage emoMessage = WechatNotifier.EmoMessages.Find(emo => emo.Id == ids[0]);
                WechatNotifier.EmoMessages.Remove(emoMessage);
            }
            DataAccess.Save(Manager);
            Response.Redirect(Request.RawUrl);
        }

        private void RanderPlayerWechatPanel()
        {
            int rows = (Manager.Players.Count + playerPerCol - 1) / playerPerCol;
            List<Player> players = Manager.Players.OrderBy(player => player.Name).ToList();
            for (int row = 0; row < rows; row++)
            {
                TableRow tableRow = new TableRow();
                for (int col = 0; col < playerPerCol; col++)
                {
                    int index = row * playerPerCol + col;
                    if (index < Manager.Players.Count)
                    {
                        Player player = players[index];
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

        protected void SendWelcomeToMembersBtn_Click(object sender, EventArgs e)
        {
            IEnumerable<Player> players = Manager.Players.FindAll(player => player.IsActive && !String.IsNullOrEmpty(player.WechatName));
            foreach (Player player in players)
            {
                String registerLink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath) + "/" + Constants.REGISTER_DEVICE_PAGE + "?id=" + Manager.ReversedId(player.Id); 
                if (player.IsRegisterdMember)
                {
                    SendWelcomeMessageToPlayer(player);
                }
             }
            DataAccess.Save(Manager);
        }

        private void SendWelcomeMessageToPlayer(Player player)
        {
            String registerLink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath) + "/" + Constants.REGISTER_DEVICE_PAGE + "?id=" + Manager.ReversedId(player.Id);
            if (player.IsRegisterdMember)
            {
                String message = this.WelcomeMemberWechatMessageTb.Text.Replace(REGISTER_LINK, registerLink);
                Manager.WechatNotifier.AddNotifyWechatMessage(player, message);
                decimal total = 0;
                foreach (Fee fee in player.Fees)
                {
                    if (!fee.IsPaid) total = total + fee.Amount;
                }
                if (total > 0)
                {
                    message = "You membership fee is $" + Manager.RegisterMembeshipFee + ", and it is due now. You unpaid fee is $" + total + ". If you would like to pay e-transfer, please send to " + Manager.AdminEmail + " with password HITMEN. Thanks";
                    Manager.WechatNotifier.AddNotifyWechatMessage(player, message);
                }
            }
            else
            {
                String message = this.WelcomeDropinWechatMessageTb.Text.Replace(REGISTER_LINK, registerLink);
                Manager.WechatNotifier.AddNotifyWechatMessage(player, message);
            }
            DataAccess.Save(Manager);
        }

        protected void SendPrimaryMemberNotificationWechatMessageBtn_Click(object sender, EventArgs e)
        {
            if (this.PoolListBox.SelectedIndex >= 0)
            {
                Pool pool = Manager.FindPoolByName(this.PoolListBox.SelectedValue);
                String message = this.PoolWechatMessageTb.Text.Replace(POOL_MEMBER_COUNT, pool.Members.Count.ToString());
                Manager.WechatNotifier.AddNotifyWechatMessage(pool, message);
                foreach (Member member in pool.Members.Items)
                {
                    Player player = Manager.FindPlayerById(member.PlayerId);
                    message = this.PrimaryMemberMessgeTb.Text.Replace(POOL_MEMBER_COUNT, pool.Members.Count.ToString()).Replace(POOL_NAME, pool.Name);
                    Manager.WechatNotifier.AddNotifyWechatMessage(player, message);
                    message = "Congratus! You have become the primary member in pool " + pool.Name;
                    Manager.WechatNotifier.AddNotifyWechatMessage(pool, player, message);
                }
                DataAccess.Save(Manager);
            }
            else 
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Pool is not selected')", true);
                return;
            }
        }

        protected void SendWelcomeToDropinsBtn_Click(object sender, EventArgs e)
        {
            IEnumerable<Player> players = Manager.Players.FindAll(player => player.IsActive && !String.IsNullOrEmpty(player.WechatName));
            foreach (Player player in players)
            {
                SendWelcomeMessageToPlayer(player);
            }
            DataAccess.Save(Manager);
        }

        protected void SendToAllTestBtn_Click(object sender, EventArgs e)
        {
            IEnumerable<Player> players = Manager.Players.FindAll(player => player.IsActive && !String.IsNullOrEmpty(player.WechatName));
            foreach (Player player in players)
            {
                if (player.IsActive && String.IsNullOrEmpty(player.Birthday))
                {
                String message = this.TestAllTb.Text.Replace(PLAYER_NAME, player.Name);
                Manager.WechatNotifier.AddNotifyWechatMessage(player, message);
                }
            }
            DataAccess.Save(Manager);
        }

        protected void WechatNotifyCb_CheckedChanged(object sender, EventArgs e)
        {
            Manager.WechatNotifier.Enable = this.WechatNotifyCb.Checked;
            if (!this.WechatNotifyCb.Checked)
            {
                Manager.WechatNotifier.WechatMessages.Clear();
            }
            DataAccess.Save(Manager);
        }

 
        protected void SendWelcomeToPlayerBtn_Click(object sender, EventArgs e)
        {
            if (this.PlayerListBox.SelectedIndex < 0)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msgid", "alert('Player is not selected')", true);
                return;
            }
            Player player = Manager.FindPlayerById(this.PlayerListBox.SelectedValue);
            SendWelcomeMessageToPlayer(player);
        }

        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            Manager.WechatNotifier.WechatPoolMessage = this.PoolWechatMessageTb.Text;
            Manager.WechatNotifier.WechatPrimaryMemberMessage = this.PrimaryMemberMessgeTb.Text;
            Manager.WechatNotifier.WechatMemberWelcomeMessage = this.WelcomeMemberWechatMessageTb.Text;
            Manager.WechatNotifier.WechatDropinWelcomeMessage = this.WelcomeDropinWechatMessageTb.Text;
            Manager.WechatNotifier.WechatToAllTestMessage = this.TestAllTb.Text;
            DataAccess.Save(Manager);
        }
    }
}