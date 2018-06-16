<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Billing.aspx.cs" Inherits="VballManager.Billing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

        <asp:Panel ID="PlayerPanel" runat="server">
        <br />
        <br />
            <asp:Table ID="PlayerTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Fees & Payments"
                Font-Bold="True" BackColor="#CFDFC6" BorderColor="#DAE9D6">
            </asp:Table>
        <br />
        <br />
            <asp:Table ID="PaidPlayerTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Payment History"
                Font-Bold="True" BackColor="#CFDFC6" BorderColor="#DAE9D6">
            </asp:Table>
        </asp:Panel>
 </asp:Content>
