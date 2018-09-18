<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="SuperAdmin.aspx.cs" Inherits="Reservation.SuperAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .inlineBlock
        {
            display: inline-block;
            vertical-align: top;
        }
        .style22
        {
            width: 100%;
        }
        .style27
        {
            width: 109px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:Panel ID="SystemSettingPanel" runat="server" BackColor="#C5DEF3" >
    <table class="style22">
        <tr>
            <td class="style27">
                <strong>System Settings</strong></td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style27">
                Title</td>
            <td>
                <asp:TextBox ID="SystemTitleTb" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style27">
                Share Players</td>
            <td>
                <asp:CheckBox ID="SharePlayerstCb" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="style27">
                Open Admin</td>
            <td>
                <asp:CheckBox ID="OpenAdminCb" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="style27">
                &nbsp;</td>
            <td align="right">
                <asp:Button ID="SavButton" runat="server" OnClick="SaveSystemBtn_Click" 
                    Text="Save" Width="80px" />
            </td>
        </tr>
        <tr>
            <td class="style27">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

</asp:Panel>
<asp:Panel ID="Panel3" runat="server" CssClass="inlineBlock" Width="189px" 
            Height="304px" BackColor="#FFCC66" Visible="False">
        <table bgcolor="#FFFFCC">
        <tr>
        <td>
            <asp:ListBox ID="ActivityTypeListbox" runat="server" AutoPostBack="True" 
                Height="295px" Width="190px" 
                onselectedindexchanged="ActivityTypeListbox_SelectedIndexChanged"></asp:ListBox>
            </td>
        <td>
            <table class="inlineBlock" style="width: 45%; top: inherit;">
                <tr>
                    <td align="center" class="style8">
                        Activity Type</td>
                </tr>
                <tr>
                    <td class="style8">
                        <asp:TextBox ID="ActivityNameTb" runat="server" Height="20px" Width="149px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style8">
                        <asp:Button ID="AddActivityTypeBtn" runat="server" onclick="AddActivityTypeBtn_Click" 
                            Text="Add" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style8">
                        <asp:Button ID="DeleteActivityTypeBtn" runat="server" onclick="DeleteActivityTypeBtn_Click" 
                            Text="Delete" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style8">
                         <asp:Button ID="SaveActivityTypeBtn" runat="server" onclick="UpdateActivityTypeBtn_Click" 
                            Text="Update" Width="80px" />
                       &nbsp;</td>
                </tr>
               <tr>
                    <td align="center" class="style8">
                    </td>
                </tr>
                 <tr>
                    <td align="center" class="style8">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" class="style8">&nbsp;</td>
                 </tr>
                <tr>
                    <td align="center" class="style8">
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style8">
                        &nbsp;</td>
                </tr>
            </table>
            </td>
        <td>
            <asp:ListBox ID="FeeTypeListbox" runat="server" AutoPostBack="True" 
                Height="300px" style="margin-left: 0px" Width="211px" 
                onselectedindexchanged="FeeTypeListbox_SelectedIndexChanged"></asp:ListBox>
            </td>
        <td>
            <table class="inlineBlock" style="width: 32%; top: inherit;">
                <tr>
                    <td align="center">
                        Fee Type</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="FeeTypeNameTb" runat="server" Height="20px" Width="149px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="AddFeeTypeBtn" runat="server" onclick="AddFeeTypeBtn_Click" 
                            Text="Add" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="DeleteFeeTypeBtn" runat="server" onclick="DeleteFeeTypeBtn_Click" 
                            Text="Delete" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="SaveFeeTypeBtn" runat="server" onclick="UpdateFeeTypeBtn_Click" 
                            Text="Update" Width="80px" />                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                 </tr>
                 <tr>
                    <td align="center">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;</td>
                </tr>
            </table>
            </td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        </tr></table>
        </asp:Panel>
</asp:Content>
