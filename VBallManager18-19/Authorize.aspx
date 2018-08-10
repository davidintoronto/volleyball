<%@ Page Title="Register Device" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Authorize.aspx.cs" Inherits="VballManager.Authorize" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="LoginUserPanel" runat="server">
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="PromptLb" runat="server" Text="You may authorize someone else to help you with reservation if you want. Or" Style="width: 100%; font-size: 5em;"
                        ForeColor="#993300"></asp:Label>
                  </td>
            </tr>
            <tr>
                     <td align="center">
                        <asp:Button ID="GotoNextBtn" runat="server" OnClick="GotoNextBtn_Click" Style="width: 50%;
                        font-size: 5em;" Text="Goto Reservation" />
                    </td>
                </tr>
          </table>
    </asp:Panel>

    <br />
    <asp:Panel ID="UserPanel" runat="server">
        <asp:Table ID="UserTable" runat="server" Style="width: 100%; font-size: 5em;" Font-Bold="True"
            BackColor="#CFDFC6" BorderColor="#DAE9D6">
        </asp:Table>
    </asp:Panel>
</asp:Content>
