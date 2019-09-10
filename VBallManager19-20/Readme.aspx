<%@ Page Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true" CodeBehind="Readme.aspx.cs"
    Inherits="VballManager.Readme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 741px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="ProfilePanel" runat="server">
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:ImageButton ID="BackBtn" runat="server" ImageUrl="~/Icons/Back.png" OnClick="BackBtn_Click" />
                </td>
           </tr>
        </table>
    </asp:Panel>
       <asp:Panel ID="ReadmePanel" runat="server"  BackColor="#BCE0D9">
             <asp:Label ID="ReadmeLb" runat="server" Style="width: 100%; font-size: 5em;" BorderColor="#003366" ForeColor="Maroon">
            </asp:Label>
        </asp:Panel>
</asp:Content>
