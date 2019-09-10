<%@ Page Title="Register Device" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="LinkDevice.aspx.cs" Inherits="VballManager.LinkDevice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="RegisterLinkPanel" runat="server">
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="UsernameLb" runat="server" Text="Username"  
                        Style="width: 100%; font-size: 5em;" ForeColor="#993300" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="RegisterBtn" runat="server" Text="Click To Register" 
                        Style="width: 50%; font-size: 5em;" onclick="RegisterBtn_Click" />
                </td>
            </tr>
        </table>
        </asp:Panel>
   <asp:Panel ID="RegisterCodePanel" runat="server">
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="RegisterCodeLb" runat="server" Text="Enter your register code"  
                        Style="width: 100%; font-size: 5em;" ForeColor="#993300" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    
                    &nbsp;&nbsp;&nbsp; &nbsp;</td>
            </tr>
             <tr>
             <td align="center">
                    <asp:TextBox ID="RegisterCodeTb" runat="server" 
                        Style="width: 80%; font-size: 4em;"  
                        BorderStyle="Solid" BackColor="#CCFFFF" />
                </td>
            </tr>
           <tr>
                <td align="center">
                    
                    &nbsp;&nbsp;&nbsp; &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="RegisterCodeBtn" runat="server" Text="Click To Register" 
                        Style="width: 50%; font-size: 5em;" onclick="RegisterCodeBtn_Click" />
                </td>
            </tr>
        </table>
        </asp:Panel>
       <asp:Panel ID="ReservePanel" runat="server">
            <asp:Table ID="ReserveLinkTable" runat="server" Style="width: 100%; font-size: 5em;" 
                Font-Bold="True" BackColor="#CFDFC6" BorderColor="#DAE9D6">
            </asp:Table>
        </asp:Panel>
        <br />
       <asp:Panel ID="UserPanel" runat="server">
            <asp:Table ID="UserTable" runat="server" Style="width: 100%; font-size: 5em;" 
                Font-Bold="True" BackColor="#CFDFC6" BorderColor="#DAE9D6">
            </asp:Table>
        </asp:Panel>
</asp:Content>
