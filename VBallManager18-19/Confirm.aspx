<%@ Page Title="Register Device" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Confirm.aspx.cs" Inherits="VballManager.Confirm" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="LoginUserPanel" runat="server">
        <table width="100%">
             <tr>
                <td align="center" colspan="2" >
                    <asp:Label ID="PromptLb" runat="server" ForeColor="#993300" 
                        Style="width: 100%; font-size: 5em;" Text=""></asp:Label>
               </td>
            </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="ConfirmBtn" runat="server" OnClick="ConfirmBtn_Click" Style="width: 50%;
                        font-size: 5em;" Text="Yes" />
                    </td>
                     <td align="center">
                       <asp:Button ID="NoBtn" runat="server" OnClick="NoBtn_Click" Style="width: 50%;
                        font-size: 5em;" Text="No" />
                    </td>
                </tr>
        </table>
    </asp:Panel>
</asp:Content>
