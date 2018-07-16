<%@ Page Title="Register Device" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="LinkDevice.aspx.cs" Inherits="VballManager.LinkDevice" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="LoginUserPanel" runat="server">
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="SelectUserLb" runat="server" Text="Select User" Style="width: 100%; font-size: 5em;"
                        ForeColor="#993300"></asp:Label>
                    <asp:DropDownList ID="UserList" runat="server" ForeColor="#39952D" BackColor="#B5E1E3"
                        Font-Bold="True" Style="width: 40%; font-size: 4em;" AutoPostBack="True" 
                        onselectedindexchanged="UserList_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="Label1" runat="server" ForeColor="#993300" 
                        Style="width: 100%; font-size: 5em;" Text="Password"></asp:Label>
                    <asp:TextBox ID="PasswordTb" runat="server" BackColor="#B5E1E3" 
                        Font-Bold="True" ForeColor="#39952D" Style="width: 40%; font-size: 4em;" />
                </td>
            </tr>
              <tr>
                    <td align="center">
                        <asp:Button ID="LoginBtn" runat="server" OnClick="LoginBtn_Click" Style="width: 50%;
                        font-size: 5em;" Text="Login" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="LoginLabel" runat="server" ForeColor="#993300" 
                            Style="width: 100%; font-size: 5em;" Text=""></asp:Label>
                    </td>
                </tr>
        </table>
    </asp:Panel>
  
    <asp:Panel ID="ReservePanel" runat="server">
        <asp:Table ID="ReserveLinkTable" runat="server" Style="width: 100%; font-size: 5em;"
            Font-Bold="True" BackColor="#CFDFC6" BorderColor="#DAE9D6">
        </asp:Table>
    </asp:Panel>
    <br />
    <asp:Panel ID="UserPanel" runat="server">
        <asp:Table ID="UserTable" runat="server" Style="width: 100%; font-size: 5em;" Font-Bold="True"
            BackColor="#CFDFC6" BorderColor="#DAE9D6">
        </asp:Table>
    </asp:Panel>
</asp:Content>
