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
        .auto-style7 {
            width: 10px;
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
                        <tr>
                            <td class="style4">Season
                            </td>
                            <td class="auto-style5">
                                <asp:CheckBox ID="SeasonCb" runat="server" />
                            </td>
                        </tr>

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
                            <td class="style4">Attend Rate Start</td>
                            <td class="auto-style5">
                                <asp:TextBox ID="AttendRateStartDateTb" runat="server" Width="115px">09/01/2018</asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator10" runat="server" ControlToValidate="AttendRateStartDateTb" ErrorMessage="MM/dd/YYYY" ForeColor="#FF3300" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                            </td>
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
    <asp:Panel ID="FactorPanel" runat="server" BackColor="#99FFCC" BorderColor="#3333CC"
                        BorderStyle="Inset">
        <table>
            <tr>
                <td>
                        <asp:Table ID="FactorTable" runat="server" Caption="Factors">
                            <asp:TableHeaderRow ID="TableHeaderRow1" HorizontalAlign="Left" BackColor="#B3AB4D">
                                <asp:TableHeaderCell ID="TableHeaderCell1" Text="Pool Name" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell2" Text="Low Pool Name" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell3" Text="From" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell4" Text="To" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell5" Text="Coop From" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell7" Text="Coop To" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell8" Text="High Pool Name" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell10" Text="From" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell11" Text="To" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="TableHeaderCell12" Text="Value" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                <asp:TableHeaderCell Text="Action" HorizontalAlign="Left"></asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                </td>
                <td align="right">
                        <asp:Button ID="RecalculateFactor" runat="server" OnClick="RecalculateFactorBtn_Click" Text="Re-calculate Factors" Width="141px" />
                </td>
            </tr>
        </table>


    </asp:Panel>
    <asp:Panel ID="PlayerPanel" runat="server" BackColor="#99CCFF">
        <table id="SystemTable0" style="width: 100%;">
            <tr>
                <td class="auto-style6">
                    <asp:CheckBox ID="ClearPoolMemberCb" runat="server" Text="Clear Pool Member/Dropins" />
                </td>
                <td class="auto-style6">
                    <asp:CheckBox ID="ClearPoolGamesCb" runat="server" Text="Clear Pool Games" />
                </td>
                <td class="auto-style6">
                    <asp:CheckBox ID="ResetPlayerTransferCb" runat="server" Text="Reset Transfer/Free Dropin/Role" />
                </td>
                <td class="auto-style6">
                    <asp:CheckBox ID="ResetPlayerMembershipCb" runat="server" Text="Reset Player Membership" />
                </td>
                <td class="auto-style6">
                    <asp:CheckBox ID="ResetUserAuthorizationCb" runat="server" Text="Reset User Authorization" />
                </td>
                <td class="auto-style6">
                    <asp:CheckBox ID="ClearPlayerFeeCb" runat="server" Text="Clear Player Fees" />
                </td>
                <td class="auto-style6">
                    <asp:Button ID="ResetPoolsBtn" runat="server" Enabled="False" OnClick="ResetSystemBtn_Click" Text="Reset" Width="94px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
