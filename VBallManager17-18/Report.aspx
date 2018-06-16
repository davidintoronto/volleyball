<%@ Page Title="2016-2017 Financial Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="VballManager.Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .inlineBlock
        {}
        </style>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
         <asp:Panel ID="FeeReportPanel" runat="server">
       <asp:Table Width="100%" ID="FeeReportTable" runat="server" 
                 Caption="2016-2017 Financial Reports" BackColor="#DFE3FD" Font-Bold="True" 
                 Font-Size="Large">
            <asp:TableHeaderRow ID="FeeReportHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell Text="Date" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Fee Desc" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Credit" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Debit" HorizontalAlign="Left"></asp:TableHeaderCell>
             </asp:TableHeaderRow>
           </asp:Table>
         </asp:Panel>
  </asp:Content>