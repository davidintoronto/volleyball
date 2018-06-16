<%@ Page Title="Fees" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Fees.aspx.cs" Inherits="VballManager.Fees" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .inlineBlock
        {}
        .style1
        {
            width: 177px;
        }
        .style3
        {
            width: 166px;
        }
        .style4
        {
            width: 152px;
        }
        .style5
        {
            width: 67px;
        }
        .style6
        {
            width: 63px;
        }
        .style7
        {
            width: 138px;
        }
        .style8
        {
            width: 90px;
        }
    </style>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:Panel ID="PaymentPanel" runat="server">
     <table width="100%"><tr>
     <td class="style1" rowspan="3">
    <asp:ListBox ID="PlayerListbox" runat="server"  AutoPostBack="True" Height="574px" 
             onselectedindexchanged="PlayerList_SelectedIndexChanged" Width="176px" 
             Font-Bold="True" Font-Size="Large">
     </asp:ListBox>
     </td>
     <td>
         <table style="width:100%;" bgcolor="#B3AB4D">
             <tr>
                  <td style="font-weight: 700" class="style8">
                      <asp:Label ID="Label1" runat="server" Text="Transfers" />
                  </td>
                 <td>
                      <asp:TextBox ID="TransferTb" runat="server" Text="" Enabled="False" />
                 </td>
                 <td style="font-weight: 700" class="style8">
                      <asp:Label ID="Label2" runat="server" Text="Used" />
                  </td>
                 <td>
                      <asp:TextBox ID="UsedTransferTb" runat="server" Text="" Enabled="False" />
                 </td>
                 <td style="font-weight: 700" class="style8">
                     <asp:Label runat="server" Text="Free Dropin" />
                 </td>
                 <td class="style3">
                     <asp:TextBox ID="FreeDropinTb" runat="server"></asp:TextBox>
                 </td>
                 <td>
                     <asp:Button ID="UpdateFreeDropinBtn" runat="server" Text="Update" 
                         onclick="UpdateFreeDropinBtn_Click" Width="77px" />
                 </td>
             </tr>
         </table>
     </td>
     </tr>
     <tr>
     <td valign="top">                  
        <asp:Table Width="100%" ID="FeeTable" runat="server" Caption="Fees" 
             BorderColor="#D6CCE1" Font-Bold="True" Font-Size="Large" BackColor="#99CCFF">
            <asp:TableHeaderRow ID="FeeTableHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell Text="Date" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Fee Type" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Fee Desc" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Amount" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Pay Date" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Paid/Used" HorizontalAlign="Left"></asp:TableHeaderCell>
            </asp:TableHeaderRow>
           </asp:Table>
         </td>
     </tr>
     <tr>
     <td>
        
         <table style="width:100%;" bgcolor="#B3AB4D">
             <tr>
                 <td style="font-weight: 700" class="style5">
                     Fee Type</td>
                 <td class="style4">
                    
                     <asp:DropDownList ID="FeeTypeDDL" runat="server" Height="100%" Width="100%">
                     </asp:DropDownList>
                    
                 </td>
                <td style="font-weight: 700" class="style5">
                     Fee Desc</td>
                 <td class="style4">
                     <asp:TextBox ID="FeeDescTb" runat="server"></asp:TextBox>
                 </td>
                 <td style="font-weight: 700" class="style6">
                     Amount</td>
             <td class="style7">
             <asp:TextBox 
             ID="FeeAmountTb" runat="server">-</asp:TextBox>             </td>
             <td>
         <asp:Button ID="AddFeeBtn" runat="server" Text="Add Fee or Credit" 
             onclick="AddFeeBtn_Click" />
             </td>
             </tr>
         </table>
     </td>
     </tr>

     </table>
       </asp:Panel>
         <asp:Panel ID="Panel1" runat="server">
             <asp:Button ID="ResetFeeBtn" runat="server" Text="Reset All" 
             onclick="ReverseAllFeeBtn_Click" Width="127px" Visible="False" />
         </asp:Panel>
         <asp:Panel ID="FeeReportPanel" runat="server">
       <asp:Table Width="100%" ID="FeeReportTable" runat="server" 
                 Caption="Fee &amp; Payment Report" BackColor="#DFE3FD" Font-Bold="True" 
                 Font-Size="Large">
            <asp:TableHeaderRow ID="FeeReportHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell Text="Date" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Fee Type" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Fee Desc" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Credit" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Debit" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Balance" HorizontalAlign="Left"></asp:TableHeaderCell>
             </asp:TableHeaderRow>
           </asp:Table>
         </asp:Panel>
        <asp:Panel ID="TransferPanel" runat="server" BackColor="#999966">
       <asp:Table Width="100%" ID="TransferTable" runat="server" Caption="Credits" BorderColor="#D6CCE1" Font-Bold="True" Font-Size="Large">
            <asp:TableHeaderRow HorizontalAlign="Left">
                <asp:TableHeaderCell Text="Name" HorizontalAlign="Left"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Amount" HorizontalAlign="Left"></asp:TableHeaderCell>
             </asp:TableHeaderRow>
           </asp:Table>
         </asp:Panel>       
</asp:Content>