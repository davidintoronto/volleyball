<%@ Page Title="Fees" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Authen.aspx.cs" Inherits="VballManager.Authen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .inlineBlock
        {
        }
        .style1
        {
            width: 177px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="PaymentPanel" runat="server" BackColor="#C7C7F8">
        <table width="100%">
            <tr><td colspan="2"><asp:Label runat="server" Font-Bold="True" Font-Size="X-Large">Agent Authorization</asp:Label></td></tr>
            <tr>
                <td class="style1" rowspan="3">
                    <asp:ListBox ID="PlayerListbox" runat="server" AutoPostBack="True" Height="574px"
                        OnSelectedIndexChanged="PlayerList_SelectedIndexChanged" Width="176px" Font-Bold="True"
                        Font-Size="Large" BackColor="#EEF2BB"></asp:ListBox>
                </td>
                <td align="center">
                    &nbsp;
                    <asp:CheckBoxList ID="AuthUusersLb" runat="server" Height="523px" Width="100%" 
                        AutoPostBack="True" onselectedindexchanged="AuthUusersLb_SelectedIndexChanged" 
                        RepeatColumns="6" BackColor="#C5DAFA" Font-Bold="True" 
                        RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td >
                    <asp:TextBox ID="LinkDeviceTb" runat="server" Width="751px" 
                       ></asp:TextBox>
                    <asp:LinkButton ID="LinkDeviceLb" runat="server" onclick="LinkDeviceLb_Click">Link Device To User</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td >
                    <asp:TextBox ID="ResetLinkDeviceTb" runat="server" Width="754px"></asp:TextBox>
                    <asp:LinkButton ID="ResetLinkDeviceLb" runat="server" 
                        onclick="ResetLinkDeviceLb_Click">Reset Link Device To User</asp:LinkButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="DeviceLinkStatus" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large">Link Device Status</asp:Label>
                    <asp:CheckBoxList ID="LinkStatusList" runat="server" Height="523px" Width="100%" 
                        RepeatColumns="6" BackColor="#ACC8C8" Font-Bold="True">
                    </asp:CheckBoxList>
    </asp:Panel>
</asp:Content>
