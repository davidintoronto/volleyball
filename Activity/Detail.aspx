<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="Reservation.Detail" %>
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
   <td align="left">  <asp:ImageButton ID="BackBtn" runat="server" ImageUrl="~/Icons/Back.png" onclick="BackBtn_Click" /> 
</td>
       
   <td align="right" valign="middle" class="style1">
       <asp:Label ID="DeleteUserLbl" runat="server" 
           Text="DELETE USER" Font-Bold="True" Font-Size="5em" ForeColor="Red" 
           Visible="False"></asp:Label>
           </td><td align="right">
           <asp:ImageButton ID="DeleteUserBtn" runat="server" ImageUrl="~/Icons/Delete.png" 
           onclick="DeleteUserBtn_Click" Visible="False" /> 
 </td>
   </tr>
   </table>
      <asp:Panel ID="DetailPanel" runat="server">
            
            <asp:Table ID="DetailTable" runat="server"  Style="width: 100%;  font-size: 5em;" Caption="Name"
                Font-Bold="True" BackColor="#FAE0DC">
            </asp:Table>
            <asp:TextBox ID="PasscodeTb" runat="server" Style="width: 250px;  font-size: 1.2em;" Visible="False"></asp:TextBox>
            <asp:TextBox ID="NameTb" runat="server" Style="width: 400px;  font-size: 1.2em;"></asp:TextBox>
        </asp:Panel>
        <br />
    </asp:Panel>
</asp:Content>

