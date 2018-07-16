<%@ Page Title="2017-2018 Statistics" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="VballManager.Statistics" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Label ID="L1" runat="server" Text="" />
         <asp:Panel ID="StatPanel" runat="server">
            <asp:Table ID="StatTable" runat="server"  Style="width: 100%;  font-size: 2em;" Caption="2017-2018 Statistics"
                Font-Bold="True" BackColor="#FAE0DC"> 
             <asp:TableHeaderRow ID="StatTableHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell ID="TableHeaderCell9" Text="" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell6" Text="Player Name" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Monday" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Friday" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Total" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
             </asp:TableHeaderRow>
           </asp:Table>
           <asp:Table ID="DPoolTable" runat="server"  Style="width: 100%;  font-size: 2em;" Caption="2017-2018 Statistics"
                Font-Bold="True" BackColor="#FAE0DC" Visible="False"> 
             <asp:TableHeaderRow ID="TableHeaderRow1" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell ID="TableHeaderCell8" Text="" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
               <asp:TableHeaderCell ID="TableHeaderCell1" Text="Player Name" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell2" Text="Friday" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
              </asp:TableHeaderRow>
           </asp:Table>
           <asp:Table ID="DPoolTotalTable" runat="server"  Style="width: 100%;  font-size: 2em;" Caption="2017-2018 Statistics"
                Font-Bold="True" BackColor="#FAE0DC" Visible="False"> 
            <asp:TableHeaderRow ID="TableHeaderRow2" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell ID="TableHeaderCell3" Text="" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell4" Text="Player Name" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell5" Text="Friday" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
              </asp:TableHeaderRow>
           </asp:Table>
           <asp:Table ID="CPoolTable" runat="server"  Style="width: 100%;  font-size: 2em;" Caption="2017-2018 Statistics"
                Font-Bold="True" BackColor="#FAE0DC" Visible="False"> 
             <asp:TableHeaderRow ID="TableHeaderRow3" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell ID="TableHeaderCell7" Text="" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
               <asp:TableHeaderCell ID="TableHeaderCell10" Text="Player Name" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell11" Text="Friday" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
              </asp:TableHeaderRow>
           </asp:Table>
           <asp:Table ID="CPoolTotalTable" runat="server"  Style="width: 100%;  font-size: 2em;" Caption="2017-2018 Statistics"
                Font-Bold="True" BackColor="#FAE0DC" Visible="False"> 
            <asp:TableHeaderRow ID="TableHeaderRow4" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell ID="TableHeaderCell12" Text="" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell13" Text="Player Name" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell14" Text="Friday" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
              </asp:TableHeaderRow>
           </asp:Table>
        </asp:Panel>
</asp:Content>

