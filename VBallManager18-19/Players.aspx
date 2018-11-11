<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Players.aspx.cs" Inherits="VballManager.Players" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1 {
            width: 268px;
        }

        .style4 {
            width: 233px;
        }

        .style5 {
            width: 105px;
        }

        .inlineBlock {
        }

        .style6 {
            width: 233px;
            height: 24px;
        }

        .style7 {
            height: 24px;
        }

        .auto-style6 {
            width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="PlayerPanel" runat="server" BackColor="#99CCFF">
        <table id="SystemTable0" style="width: 100%;">
            <tr>
                <td class="auto-style6">
                    <table class="inlineBlock" style="width: 32%; top: inherit;">
                        <tr>
                            <td>&nbsp;
                            Players</td>
                            <td valign="top">
                                <asp:ListBox ID="PlayerLb" runat="server" Width="124px" Height="332px"
                                    OnSelectedIndexChanged="PlayerListBox_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Name
                            </td>
                            <td>
                                <asp:TextBox ID="PlayerNameTb" runat="server"></asp:TextBox>
                                <asp:TextBox ID="PlayerIdTb" runat="server" Visible="False" Width="16px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Birthday</td>
                            <td>
                                <asp:TextBox ID="PlayerBirthdayTb" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Role
                            </td>
                            <td>
                                <asp:DropDownList ID="Role" runat="server" Width="124px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            Wechat</td>
                            <td>
                                <asp:TextBox ID="WechatNameTb" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td>Active
                            </td>
                            <td>
                                <asp:CheckBox ID="PlayerActiveCb" runat="server" />
                            </td>
                        </tr>
                      <tr>
                            <td align="right">Mark
                            </td>
                            <td>
                                <asp:CheckBox ID="PlayerMarkCb" runat="server" />
                            </td>
                        </tr>
                      <tr>
                            <td align="right">Waiver Signed
                            </td>
                            <td>
                                <asp:CheckBox ID="PlayerWaiverSigned" runat="server" />
                            </td>
                        </tr>
                         <tr>
                            <td>&nbsp;
                                <asp:TextBox ID="PlayerPasscodeTb" runat="server" Visible="False" Width="16px"></asp:TextBox>
                            </td>
                            <td align="center">
                                <asp:Button ID="AddPlayerBtn" runat="server" OnClick="AddPlayerBtn_Click" Text="Add"
                                    Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td align="center">
                                <asp:Button ID="UpdatePlayerBtn" runat="server" OnClick="SavePlayerBtn_Click" Text="Save"
                                    Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td align="center">&nbsp;
                            <asp:Button ID="DeletePlayerBtn" runat="server" OnClick="DeletePlayerBtn_Click" Text="Delete"
                                Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td align="center">
                                <asp:Button ID="AllWechatNameBtn" runat="server" OnClick="AllWechatNameBtn_Click" Text="Reset Wechat"
                                    Width="108px" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="style5">
                    <table>
                        <tr>
                            <td>
                                <asp:DropDownList ID="PlayerPropertiesList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="PlayerPropertiesList_SelectedIndexChanged" Width="194px">
                                    <asp:ListItem Value="IsRegisterMember">Membership</asp:ListItem>
                                    <asp:ListItem Value="IsActive">Active</asp:ListItem>
                                    <asp:ListItem Value="Marked">Mark</asp:ListItem>
                                    <asp:ListItem Value="Waiver">Waiver Signed</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;<asp:Button ID="SetMembershipBtn0" runat="server" OnClick="SetMembershipBtn_Click" Style="margin-left: 0px" Text="Check Club Memberships" Width="164px" Enabled="False" />
                                <asp:Button ID="CreditToMembersBtn" runat="server" OnClick="CreditToMembersBtn_Click" Style="margin-left: 0px" Text="Credit to members" Width="164px" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="PlayerListbox" runat="server" Height="288px" Width="863px"
                                    BorderColor="#0000CC" BorderStyle="Double" RepeatColumns="4"
                                    RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="SavePlayersBtn" runat="server" Text="Save" Width="89px" OnClick="SavePlayersBtn_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right"></td>
                        </tr>
                        <tr>
                            <td align="right">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel2" runat="server" Style="margin-bottom: 0px">
                                </asp:Panel>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>
        </table>
    </asp:Panel>
</asp:Content>
