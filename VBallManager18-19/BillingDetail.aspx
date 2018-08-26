<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="BillingDetail.aspx.cs" Inherits="VballManager.BillingDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="ProfilePanel" runat="server">
        <table width="100%">
            <tr>
                <td align="left">
                    <asp:ImageButton ID="BackBtn" runat="server" ImageUrl="~/Icons/Back.png" OnClick="BackBtn_Click" />
                </td>
            </tr>
        </table>
      <br />
        <asp:Panel ID="GameFeePaymentPanel" runat="server">
           <asp:Table ID="FeeTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Fees & Payments"
                Font-Bold="True" BackColor="#FAE0DC">
            </asp:Table>
         </asp:Panel>
      <br />
        <asp:Panel ID="PrepayPanel" runat="server">
           <asp:Table ID="PrepayTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Pre-Payments"
                Font-Bold="True" BackColor="#FAE0DC">
            </asp:Table>
         </asp:Panel>
    </asp:Panel>
    <asp:TextBox ID="PrePayAmountTb" runat="server" Font-Bold="True" Width="200px" Font-Size="1.2em"></asp:TextBox>
                                  <asp:CompareValidator ID="MoneyVD" runat="server" 
                                      ControlToValidate="PrePayAmountTb" ErrorMessage="* Digital Only" 
                                      ForeColor="#FF3300" Operator="DataTypeCheck" 
        Type="Integer"></asp:CompareValidator>
                                  </asp:Content>
