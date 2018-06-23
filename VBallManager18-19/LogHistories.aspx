<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogHistories.aspx.cs" Inherits="VballManager.LogHistories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 268px;
        }
        .style2
        {
            width: 79px;
        }
        .style3
        {
            width: 216px;
        }
        .style4
        {
            width: 233px;
        }
        .style5
        {
            width: 105px;
        }
        .inlineBlock
        {}
    </style>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <asp:Panel ID="LogPanel" runat="server">
        <asp:Table Width="100%" ID="LogTable" runat="server" BackColor="#C5E1B9"></asp:Table>
        <asp:Button ID="ClearLogBtn" Text="Clear Log History" runat="server" OnClick="ClearLogHistory_Click" />
        </asp:Panel>
    </asp:Panel>
</asp:Content>
