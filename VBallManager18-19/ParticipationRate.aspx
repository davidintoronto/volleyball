<%@ Page Title="2017-2018 Pre-Register" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true" CodeBehind="ParticipationRate.aspx.cs" Inherits="VballManager.ParticipationRate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
         <asp:Panel ID="SurveyPanel" runat="server">
            <asp:Table ID="StatsTable" runat="server"  Style="width: 100%;  font-size: 5em;" Caption=""
                Font-Bold="True" BackColor="#FAE0DC"> 
             <asp:TableHeaderRow ID="TableHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell ID="TableHeaderCell9" Text="" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell6" Text="Player" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="PlayedCountCell" Text="Rate" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="AdditionalFactorCell" Text="Bonus" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell1" Text="Total" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
             </asp:TableHeaderRow>
            </asp:Table>
        </asp:Panel>
        <asp:TextBox ID="DropinNameTb" runat="server" Font-Bold="True" Width="400px" Font-Size="1.2em" Visible="False"></asp:TextBox>
</asp:Content>

