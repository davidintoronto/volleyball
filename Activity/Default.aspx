<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Reservation.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
        <asp:Panel ID="ActivityPanel" runat="server">
            <br />
            <br />
            <asp:Table ID="ActivityTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Games"
                Font-Bold="True" BackColor="#DDDDF9" BorderColor="#DDDDF9" Visible="True">
            </asp:Table>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:LinkButton ID="AdminLink" Text="Admin" runat="server" 
                OnClick="AdminLink_Clcik" Font-Size="XX-Large"></asp:LinkButton>
         </asp:Panel>
</asp:Content>
