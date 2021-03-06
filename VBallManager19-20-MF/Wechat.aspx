﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Wechat.aspx.cs" Inherits="VballManager.Wechat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style3 {
            width: 126px;
        }

        .auto-style5 {
            width: 837px;
        }

        .auto-style6 {
            width: 837px;
            height: 55px;
        }

        .auto-style7 {
            height: 55px;
        }

        .auto-style8 {
            width: 972px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="SendWechatPanel" runat="server" BackColor="#6AD3C5">
        <table id="Table1" style="width: 100%;">
            <tr>
                <td class="auto-style8">
                    <asp:CheckBox ID="WechatNotifyCb" runat="server" AutoPostBack="True" OnCheckedChanged="WechatNotifyCb_CheckedChanged" Text="Enbale Wechat Notification" />
                </td>
                <td class="auto-style8">
                    <asp:Label ID="messageNumber" runat="server" Text=""></asp:Label>
                </td>
                <td align="center">
                    <asp:Button ID="SaveBtn" runat="server" OnClick="SaveBtn_Click" Text="Save" Width="89px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" BackColor="#CAD3C5">
        <table id="WechatSenderTable" style="width: 100%;">
            <tr>
                <td class="auto-style3" rowspan="2" align="center">Wecome
                    <asp:ListBox ID="PlayerListBox" runat="server" Height="92px" Width="114px"></asp:ListBox>
                    <asp:Button ID="SendWelcomeToPlayerBtn" runat="server" OnClick="SendWelcomeToPlayerBtn_Click" Text="Send To Player" Width="100px" />
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="WelcomeMemberWechatMessageTb" runat="server" Height="72px" Width="836px" TextMode="MultiLine">Welcome to Hitmen Volleyball Club! We are happy to inform you that your membership has been approved! As a registed member, you could become primary member of the pools you attend if your attendance rate is high. And you are allow to make reservation one day before the game. let&#39;s play and have great fun! 
 Here is your private register link {REGISTER_LINK}</asp:TextBox>
                </td>
                <td align="center">Welcome Members<asp:Button ID="SendWelcomeToPlayersBtn" runat="server" Text="Send All Members" Width="113px" OnClick="SendWelcomeToMembersBtn_Click" />
                </td>
            </tr>
            <tr>
                <td class="auto-style6">
                    <asp:TextBox ID="WelcomeDropinWechatMessageTb" runat="server" Height="39px" Width="839px" TextMode="MultiLine">Welcome to Hitmen Volleyball Club! Here is your private register link {REGISTER_LINK}</asp:TextBox>
                </td>
                <td align="center" class="auto-style7">Welcome Dropins<asp:Button ID="SendToDropinsBtn" runat="server" Text="Send All Dropins" Width="114px" OnClick="SendWelcomeToDropinsBtn_Click" />
                </td>
            </tr>
            <tr>
                <td rowspan="2" class="auto-style3">To Group Primary Members<asp:ListBox ID="PoolListBox" runat="server" Width="114px" Height="132px"></asp:ListBox>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="PoolWechatMessageTb" runat="server" Height="61px" Width="836px" TextMode="MultiLine">&quot;Hi, everyone. We will re-assign primary members as scheduled. Following {POOL.MEMBER.COUNT} players are highly rated on their attendance, they will be the primary members for next few months</asp:TextBox>
                </td>
                <td rowspan="2" align="center">
                    <asp:Button ID="SendToGroupBtn" runat="server" Text="Send" Width="89px" OnClick="SendPrimaryMemberNotificationWechatMessageBtn_Click" />
                </td>
            </tr>
            <tr>
                <td class="auto-style5">
                    <asp:TextBox ID="PrimaryMemberMessgeTb" runat="server" Height="87px" Width="838px" TextMode="MultiLine">Congratus!. We are very pleased that you have become 1 of {POOL.MEMBER.COUNT} primary members in pool {POOL_NAME}. You deserve this because you attended a lot of games in the post. We will pre-reserve a spot for you for each week in this pool. However, please cancel your reservation if you cannot make it. Thanks!</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">Send Test To All</td>
                <td class="auto-style5">
                    <asp:TextBox ID="TestAllTb" runat="server" Height="39px" Width="839px" TextMode="MultiLine">Welcome to Hitmen Volleyball Club! This is a test. Thanks</asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="TestToAllBtn" runat="server" Text="Send" Width="89px" OnClick="SendToAllTestBtn_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="EmoPanel" runat="server" BackColor="#99CCFF">
        <table id="Table2" style="width: 100%;">
            <tr>
                <td class="auto-style8">
                    <asp:CheckBox ID="EnableReserveEmoCb" runat="server" AutoPostBack="True" OnCheckedChanged="EnableReserveEmoCb_CheckedChanged" Text="Enbale Sending Emo Messages for Reserve" />
                    <asp:CheckBox ID="EnableCancelEmoCb" runat="server" AutoPostBack="True" OnCheckedChanged="EnableCancelEmoCb_CheckedChanged" Text="Enbale Sending Emo Messages for cancel" />
                </td>
             </tr>
    </asp:Panel>
       <asp:Table ID="EmoTable" runat="server" Caption="Emo Messages" Width="100%"
            BorderColor="#D6CCE1" Font-Bold="True" Font-Size="Medium" BackColor="#99CCFF">
            <asp:TableHeaderRow ID="EmoTableHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell Text="Type" HorizontalAlign="center"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Min" HorizontalAlign="center"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Max" HorizontalAlign="center"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Message" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Action" HorizontalAlign="Left"></asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </asp:Panel>
    <asp:Panel ID="PlayerPanel" runat="server" BackColor="#99CCFF">
        <table id="SystemTable0" style="width: 100%;">
            <tr>
                <td>
                    <asp:Table ID="WechatNameTable" runat="server" Caption="User Wechat Name" Width="100%"
                        BorderColor="#D6CCE1" Font-Bold="True" Font-Size="Medium" BackColor="#99CCFF">

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
                    <asp:Button ID="SetWechatNameBtn" runat="server" OnClick="SetWechatNameBtn_Click" Text="Set Username as Wechat" Width="181px" Visible="False" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
