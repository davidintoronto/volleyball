<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Wechat.aspx.cs" Inherits="VballManager.Wechat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style3 {
            width: 126px;
        }
        .auto-style4 {
            width: 861px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="SendWechatPanel" runat="server" BackColor="#CAD3C5">
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" BackColor="#CAD3C5">
        <table id="WechatSenderTable" style="width: 100%;">
            <tr>
                <td class="auto-style3">
                    Welcome Members</td>
                <td class="auto-style4" >
                   <asp:TextBox ID="MemberWechatMessageTb" runat="server" Height="39px" Width="839px" TextMode="MultiLine">Welcome to Hitmen Volleyball Club! We are happy to inform you that your membership has been approved! As a registed member, you could become primary member of the pools you attend if your attendance rate is high. And you are allow to make reservation one day before the game. let&#39;s play and have great fun! 
 Here is your private register link {REGISTER_LINK}</asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="SendToMembersBtn" runat="server" Text="Send" Width="89px" OnClick="SendToMembersBtn_Click" />
                </td>
            </tr>
           <tr>
                <td class="auto-style3" >
                    Welcome Dropins</td>
                <td class="auto-style4" >
                   <asp:TextBox ID="DropinWechatMessageTb" runat="server" Height="39px" Width="839px" TextMode="MultiLine">Welcome to Hitmen Volleyball Club! Here is your private register link {REGISTER_LINK}</asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="SendToDropinsBtn" runat="server" Text="Send" Width="89px" OnClick="SendToDropinsBtn_Click" />
                </td>
            </tr>
            <tr>
                <td rowspan="2" class="auto-style3">
                    To Group Members<asp:ListBox ID="PoolListBox" runat="server" Width="114px"></asp:ListBox>
                </td>
                <td class="auto-style4" >
                   <asp:TextBox ID="PoolWechatMessageTb" runat="server" Height="39px" Width="839px" TextMode="MultiLine">&quot;Hi, everyone. We will re-assign primary members as scheduled. Following {POOL.MEMBER.COUNT} players are highly rated on their attendance, they will be the primary members for next few months</asp:TextBox>
                </td>
                  <td rowspan="2" align="center">
                   <asp:Button ID="SendToGroupBtn" runat="server" Text="Send" Width="89px" OnClick="SendPrimaryMemberNotificationWechatMessageBtn_Click" />
                </td>
            </tr>
          <tr>
               
                <td class="auto-style4" >
                   <asp:TextBox ID="PrimaryMemberMessgeTb" runat="server" Height="39px" Width="839px" TextMode="MultiLine">Congratus!. We are very pleased that you have become 1 of {POOL.MEMBER.COUNT} primary members in pool {POOL_NAME}. You deserve this because you attended a lot of games in the post. We will pre-reserve a spot for you for each week in this pool. However, please cancel your reservation if you cannot make it. Thanks!</asp:TextBox>
                </td>
            </tr>
          </table>
  </asp:Panel>
    <br />
    <asp:Panel ID="PlayerPanel" runat="server" BackColor="#99CCFF">
        <table id="SystemTable0" style="width: 100%;">
            <tr>
                <td>
                    <asp:Table ID="WechatNameTable" runat="server" Caption="User Wechat Name" Width="100%"
                        BorderColor="#D6CCE1" Font-Bold="True" Font-Size="Large" BackColor="#99CCFF">

                        <asp:TableHeaderRow ID="FeeTableHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                            <asp:TableHeaderCell Text="Username" HorizontalAlign="Left"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Text="Wechat Name" HorizontalAlign="Left"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Text="Username" HorizontalAlign="Left"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Text="Wechat Name" HorizontalAlign="Left"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Text="Username" HorizontalAlign="Left"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Text="Wechat Name" HorizontalAlign="Left"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Text="Username" HorizontalAlign="Left"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Text="Wechat Name" HorizontalAlign="Left"></asp:TableHeaderCell>
                       </asp:TableHeaderRow>
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="SetWechatNameBtn" runat="server" OnClick="SetWechatNameBtn_Click" Text="Set Username as Wechat" Width="181px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
