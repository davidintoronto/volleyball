<%@ Page Title="2017-2018 Pool Statistics" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true" CodeBehind="PoolStatistics.aspx.cs" Inherits="VballManager.PoolStatistics" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
         <asp:Panel ID="PoolStatPanel" runat="server">
            <asp:Table ID="PoolStatTable" runat="server"  Style="width: 100%;  font-size: 5em;" Caption=" Pool Statistics"
                Font-Bold="True" BackColor="#FAE0DC"> 
             <asp:TableHeaderRow ID="TableHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell ID="TotalCell" Text="Total" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell9" Text="Less 12" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell4" Text="12-13" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell6" Text="14 & More" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="FullWithWaitingCell" Text="14 & More w/ Waiting" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
             </asp:TableHeaderRow>
            </asp:Table>
        </asp:Panel>
           <asp:Table ID="FullTable" runat="server"  Style="width: 100%;  font-size: 5em;" Caption="Full Games"
                Font-Bold="True" BackColor="#FAE0DC"> 
             <asp:TableHeaderRow ID="TableHeaderRow1" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell ID="TableHeaderCell1" Text="" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell2" Text="Date" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell3" Text="Reserved" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell5" Text="Intern" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell7" Text="Waiting" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell8" Text="Factor" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
             </asp:TableHeaderRow>
            </asp:Table>
 </asp:Content>

