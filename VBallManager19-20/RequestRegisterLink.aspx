<%@ Page Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true" CodeBehind="RequestRegisterLink.aspx.cs"
    Inherits="VballManager.RequestRegisterLink" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="LoginUserPanel" runat="server">
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="SelectUserLb" runat="server" Text="Select Your Name" Style="width: 100%;
                        font-size: 5em;" ForeColor="#993300"></asp:Label>
                </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ListBox ID="UserList" runat="server" ForeColor="#39952D" BackColor="#B5E1E3"
                            Font-Bold="True" Style="width: 40%; font-size: 4em;" AutoPostBack="True" OnSelectedIndexChanged="UserList_SelectedIndexChanged" />
                    </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="RequestBtn" runat="server" OnClick="RequestBtn_Click" Style="width: 50%;
                        font-size: 5em;" Text="Request Link" Visible="False" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="ResultLabel" runat="server" ForeColor="#993300" Style="width: 100%;
                        font-size: 5em;" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
