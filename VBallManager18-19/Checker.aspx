<%@ Page Title="Lottery Checker" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Checker.aspx.cs" Inherits="VballManager.Checker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%;">
        <tr>
            <td width="49%">
                Main Draw
            </td>
            <td width="49%">
                Tickets
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="MaindrawTb" runat="server" Width="80%"></asp:TextBox>
                <asp:TextBox ID="BounsTb" runat="server" Width="8%"></asp:TextBox>
            </td>
            <td rowspan="3" valign="top">
                <asp:TextBox ID="TicketsTb" runat="server" TextMode="MultiLine" Height="330" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Millions Draws
            </td>
        </tr>
        <tr>
            <td colspan="1" rowspan="1">
                <asp:TextBox ID="MDrawsTb" runat="server" TextMode="MultiLine" Height="300px" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="center">
                <asp:Button ID="GoBtn" runat="server" Text="Go to Win" OnClick="GoBtn_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Table ID="MainDrawMatchTable" runat="server" Caption="Main Match Result" 
                    Font-Bold="True" Font-Size="Larger" Width="100%">
                </asp:Table>
            </td>
            <td align="center">
                <asp:Table ID="MillionsDrawMatchTable" runat="server" 
                    Caption="Millions Match Result" Font-Bold="True" Font-Size="Larger" 
                    Width="100%">
                </asp:Table>
            </td>
        </tr>
    </table>
</asp:Content>
