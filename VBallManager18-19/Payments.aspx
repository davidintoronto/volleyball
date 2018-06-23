<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payments.aspx.cs" Inherits="VballManager.Payments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .inlineBlock
        {}
    </style>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:Panel ID="PaymentPanel" runat="server">
        <asp:Table Width="100%" ID="PaymentTable" runat="server">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Text="Date"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Player"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Day Of Week"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Amount"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Note"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Action"></asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow ID="paymentEditRow" runat="server">
                <asp:TableCell>
                    <asp:TextBox ID="PayDateTb" runat="server" Width="105px"></asp:TextBox>                
                </asp:TableCell>
                <asp:TableCell>
             <asp:DropDownList ID="PayPlayerDl" runat="server"             
                Width="126px">
            </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell>
            <asp:DropDownList ID="PayDayOfWeekDl" runat="server" Height="25px" Width="88px">
                <asp:ListItem>Monday</asp:ListItem>
                <asp:ListItem>Friday</asp:ListItem>
            </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell>
            <asp:TextBox ID="PayAmountTb" runat="server" Width="119px"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell>
           <asp:TextBox ID="PayNoteTb" runat="server" Width="407px"></asp:TextBox>
                 </asp:TableCell>
                <asp:TableCell>
           <asp:Button ID="PaymentSaveBtn" runat="server" onclick="SavePayment_Click" PostBackUrl="~/Payments.aspx?Command=cancel&PaymentId="
                Text="Save"/>
            <asp:Button ID="PaymentCancelBtn" runat="server" OnClick="CancelPayment_Click" PostBackUrl="~/Payments.aspx?Command=cancel&PaymentId="
                Text="Cancel"/>
                </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
       </asp:Panel>
 </asp:Content>
