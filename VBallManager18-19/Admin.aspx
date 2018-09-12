<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Admin.aspx.cs" Inherits="VballManager.Admin" %>

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

        .auto-style1 {
            width: 177px;
        }
        .auto-style2 {
            width: 371px;
        }
        .auto-style4 {
            height: 24px;
            width: 186px;
        }
        .auto-style5 {
            width: 186px;
        }
        .auto-style6 {
            width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel1" runat="server" BackColor="#CAD3C5">
        <table id="SystemTable" style="width: 99%;">
            <tr>
                <td align="justify" class="style1">
                    <table style="width: 319px; margin-right: 0px;">
                        <tr>
                            <td class="style4">Admin Passcode
                            </td>
                            <td class="auto-style5">
                                <asp:TextBox ID="AdminPasscodeTb" runat="server" TextMode="Password" Width="107px"></asp:TextBox>
                            </td>
                        </tr>
 <!--                       <tr>
                            <td class="style4">Cookie Authentication
                            </td>
                            <td class="auto-style5">
                                <asp:CheckBox ID="CookieAuthCb" runat="server" />
                            </td>
                        </tr>
 -->
                        <tr>
                            <td class="style4">Auth Expires On
                            </td>
                            <td class="auto-style5">
                                <asp:TextBox ID="AuthCookieExpireTb" runat="server" Width="115px">07/01/2018</asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="AuthCookieExpireTb"
                                    ErrorMessage="MM/dd/YYYY" ForeColor="#FF3300" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">Time Zone
                            </td>
                            <td class="auto-style5">
                                <asp:TextBox ID="TimeZoneTb" runat="server" Width="173px">Eastern Standard Time</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">System Time
                            </td>
                            <td class="auto-style5">
                                <asp:Label ID="SystemTimeLb" runat="server" Width="173px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">Time offset (Mins)
                            </td>
                            <td class="auto-style5">
                                <asp:TextBox ID="TimeOffsetTb" runat="server" Text="0" Width="45px" />
                                <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="TimeOffsetTb"
                                    ErrorMessage="Integers only please" ForeColor="#FF3300" Operator="DataTypeCheck"
                                    Type="Integer"></asp:CompareValidator>
                                <asp:Label ID="ServerTimeLb" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">Club Member Mode
                            </td>
                            <td class="auto-style5">
                                <asp:CheckBox ID="ClubRegisterMemberModeCb" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">Dropin spot opening at hour
                            </td>
                            <td class="auto-style5">
                                <asp:TextBox ID="DropinSpotOpenHourTb" runat="server" Width="47px">0</asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="DropinSpotOpenHourTb"
                                    ErrorMessage="Integers only please" ForeColor="#FF3300" Operator="DataTypeCheck"
                                    Type="Integer"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style6">Dropin Fee Capped
                            </td>
                            <td class="auto-style4">
                                <asp:CheckBox ID="DropinFeeCappedCb" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">Club Membership Fee
                            </td>
                            <td class="auto-style5">
                                <asp:TextBox ID="MembershipFeeTb" runat="server" Width="47px">0</asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="DropinSpotOpenHourTb"
                                    ErrorMessage="Integers only please" ForeColor="#FF3300" Operator="DataTypeCheck"
                                    Type="Integer"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">&nbsp;Lock Waiting list hour&nbsp;
                            </td>
                            <td class="auto-style5">
                                <asp:TextBox ID="LockWaitingListHourTb" runat="server" Width="29px"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="LockWaitingListHourTb"
                                    ErrorMessage="Integers only please" ForeColor="#FF3300" Operator="DataTypeCheck"
                                    Type="Integer"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">&nbsp;Lock Reservation hour&nbsp;
                            </td>
                            <td class="auto-style5">
                                <asp:TextBox ID="LockReservationHourTb" runat="server" Width="29px"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="LockReservationHourTb"
                                    ErrorMessage="Integers only please" ForeColor="#FF3300" Operator="DataTypeCheck"
                                    Type="Integer"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">&nbsp;</td>
                            <td class="auto-style5">&nbsp;</td>
                        </tr>
                    </table>
                </td>
                <td class="auto-style2">
                    <table>
                        <tr>
                            <td>&nbsp;Admin email&nbsp;

                            </td>
                            <td>

                                <asp:TextBox ID="AdminEmailTb" runat="server" Width="167px"></asp:TextBox>

                            </td>
                            <td>Max dropin fee owe</td>
                            <td class="auto-style1">

                                <asp:TextBox ID="MaxDropinfeeOweTb" runat="server" Width="66px">0</asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator9" runat="server" ControlToValidate="MaxDropinfeeOweTb" ErrorMessage="Integers only please" ForeColor="#FF3300" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:TextBox ID="ReadmeTb" runat="server" Height="317px" TextMode="MultiLine" Width="537px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Home Ip</td>
                            <td>
                                <asp:Label ID="HomeIPLb" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td></td>
                            <td align="right" class="auto-style1">
                                <asp:Button ID="SaveSystemBtn" runat="server" OnClick="SaveSystemBtn_Click" Text="Save Changes" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <asp:Panel ID="AuthorizationPanel" runat="server" BackColor="#FFFFCC" BorderColor="#3333CC"
                        BorderStyle="Inset">
                        <asp:Table ID="AuthorizeTable" runat="server" Caption="Authorization/Permissions">
                            <asp:TableHeaderRow ID="TableHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                                <asp:TableHeaderCell ID="TableHeaderCell9" Text="Action Name" HorizontalAlign="Left"
                                    runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell6" Text="Required Role" HorizontalAlign="Left"
                                    runat="server"></asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
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
                            <td align="right">Passcode
                            </td>
                            <td>
                                <asp:TextBox ID="PlayerPasscodeTb" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">Role
                            </td>
                            <td>
                                <asp:DropDownList ID="Role" runat="server" Height="16px" Width="124px">
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
                            <td align="right">Mark
                            </td>
                            <td>
                                <asp:CheckBox ID="PlayerMarkCb" runat="server" />
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
                            <td>&nbsp;
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
                                <asp:Button ID="UpdatePlayerBtn" runat="server" OnClick="UpdatePlayerBtn_Click" Text="Save"
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
                                </asp:DropDownList>
                                &nbsp;<asp:Button ID="SetMembershipBtn0" runat="server" OnClick="SetMembershipBtn_Click" Style="margin-left: 0px" Text="Check Club Memberships" Width="164px" />
                                <asp:Button ID="CreditToMembersBtn" runat="server" OnClick="CreditToMembersBtn_Click" Style="margin-left: 0px" Text="Credit to members" Width="164px" />
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
                                    <asp:CheckBox ID="ClearPoolMemberCb" runat="server" Text="Clear Pool Member/Dropins" />
                                    <asp:CheckBox ID="ClearPoolGamesCb" runat="server" Text="Clear Pool Games" />
                                    <asp:CheckBox ID="ResetPlayerTransferCb" runat="server" Text="Reset Transfer/Free Dropin/Role" />
                                    <asp:CheckBox ID="ResetPlayerMembershipCb" runat="server" Text="Reset Player Membership" />
                                    <asp:CheckBox ID="ResetUserAuthorizationCb" runat="server" Text="Reset User Authorization" />
                                    <asp:CheckBox ID="ClearPlayerFeeCb" runat="server" Text="Clear Player Fees" />
                                    <asp:Button ID="ResetPoolsBtn" runat="server" OnClick="ResetSystemBtn_Click" Text="Reset"
                                        Width="94px" />
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
