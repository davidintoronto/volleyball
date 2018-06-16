<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="VballManager.Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 268px;
        }
        .style4
        {
            width: 233px;
        }
        .style5
        {
            width: 105px;
        }
        .inlineBlock
        {}
        .style6
        {
            width: 233px;
            height: 24px;
        }
        .style7
        {
            height: 24px;
        }
        </style>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Panel1" runat="server" BackColor="#CAD3C5">
        <table id="SystemTable" style="width:100%;">
            <tr>
                  <td align="justify" class="style1">
                      <table style="width: 319px; margin-right: 0px;">
                          <tr>
                              <td class="style4">
                                  Admin Passcode
                              </td>
                              <td>
                                  <asp:TextBox ID="AdminPasscodeTb" runat="server" TextMode="Password" 
                                      Width="107px"></asp:TextBox>
                              </td>
                          </tr>
                          <tr>
                              <td class="style4">
                                  Passcode Authentication</td>
                              <td>
                                  <asp:CheckBox ID="AuthCb" runat="server" />
                              </td>
                          </tr>
                          <tr>
                              <td class="style4">
                                  Cookie Authentication</td>
                              <td>
                                  <asp:CheckBox ID="CookieAuthCb" runat="server" />
                              </td>
                          </tr>
                        <tr>
                              <td class="style4">
                                  Auth Expires On</td>
                              <td>
                                  <asp:TextBox ID="AuthCookieExpireTb" runat="server" Width="115px">07/01/2018</asp:TextBox>
                                  <asp:CompareValidator ID="CompareValidator8" runat="server" 
                                      ControlToValidate="AuthCookieExpireTb" ErrorMessage="MM/dd/YYYY" 
                                      ForeColor="#FF3300" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                              </td>
                          </tr>
                         <tr>
                            <td class="style4"  >
                                Time Zone</td>
                            <td>
                                <asp:TextBox ID="TimeZoneTb" runat="server" Width="173px">Eastern Standard Time</asp:TextBox>
                             </td>
                        </tr>
                        <tr>
                            <td class="style4"  >
                                System Time</td>
                            <td>
                                <asp:Label ID="SystemTimeLb" runat="server" Width="173px"/>
                             </td>
                        </tr>
                          <tr>
                              <td class="style4">
                                  Time offset (Mins)
                              </td>
                              <td>
                                  <asp:TextBox ID="TimeOffsetTb" runat="server" Text="0" Width="45px" />
                                  <asp:CompareValidator ID="CompareValidator4" runat="server" 
                                      ControlToValidate="TimeOffsetTb" ErrorMessage="Integers only please" 
                                      ForeColor="#FF3300" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                                  <asp:Label ID="ServerTimeLb" runat="server"></asp:Label>
                              </td>
                          </tr>
                        <tr>
                              <td class="style4">
                                  Club Member Mode</td>
                              <td>
                                   <asp:CheckBox ID="ClubRegisterMemberModeCb" runat="server" />
                              </td>
                          </tr>
                          <tr>
                              <td class="style4">
                                  Dropin spot opening at hour</td>
                              <td>
                                  <asp:TextBox ID="DropinSpotOpenHourTb" runat="server" Width="47px">0</asp:TextBox>
                                  <asp:CompareValidator ID="CompareValidator2" runat="server" 
                                      ControlToValidate="DropinSpotOpenHourTb" ErrorMessage="Integers only please" 
                                      ForeColor="#FF3300" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                              </td>
                          </tr>
                          <tr>
                              <td class="style6">
                                 Dropin Fee Capped</td>
                              <td class="style7">
                                   <asp:CheckBox ID="DropinFeeCappedCb" runat="server" />
                              </td>
                          </tr>
                            <tr>
                              <td class="style4">
                                  Club Membership Fee</td>
                              <td>
                                  <asp:TextBox ID="MembershipFeeTb" runat="server" Width="47px">0</asp:TextBox>
                                  <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                      ControlToValidate="DropinSpotOpenHourTb" ErrorMessage="Integers only please" 
                                      ForeColor="#FF3300" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                              </td>
                          </tr>
                        <tr>
                          <td class="style4">&nbsp;Lock Reservation hour&nbsp;</td>
                          <td>
                              <asp:TextBox ID="LockReservationHourTb" runat="server" Width="29px"></asp:TextBox>
                              <asp:CompareValidator ID="CompareValidator7" runat="server" 
                                  ControlToValidate="LockReservationHourTb" ErrorMessage="Integers only please" 
                                  ForeColor="#FF3300" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                              </td></tr>
                         <tr>
                              <td class="style4">
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                          </tr>
 
                   </table>
                  </td>
                 
                 <td>
                 <table>
                 <tr><td colspan="4">
                 <asp:TextBox ID="ReadmeTb" runat="server" Height="317px" TextMode="MultiLine"  
                         Width="760px" ></asp:TextBox></td></tr>
                 <tr>
                 <td></td>
                 <td></td>
                 <td></td>
                 <td align="right">
                     <asp:Button ID="SaveSystemBtn" runat="server" onclick="SaveSystemBtn_Click" 
                         Text="Save Changes" />
                     </td>
                 </tr>
                 </table>
                 
                        
                 </td>
              </tr>
        </table>
        
        
    </asp:Panel>
         <br />
        <table id="SystemTable0" style="width:100%;" __designer:mapid="3b" 
        bgcolor="#BBCCEC">
            <tr __designer:mapid="3c">
               <td class="style5">
               <table><tr><td>Players and Registered Members</td></tr><tr><td>
                    <asp:CheckBoxList ID="PlayerListbox" runat="server" 
                        Height="288px" Width="898px" 
                        SelectionMode="Multiple" 
                        BorderColor="#0000CC" BorderStyle="Double" RepeatColumns="4" 
                        RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                    </td></tr><tr><td align="right">
                    <asp:Button ID="SavePlayersBtn" runat="server" Text="Save" Width="89px" 
                           onclick="SavePlayersBtn_Click" />
                                </td></tr></table>
                 </td>
                 <td>
                     <table class="inlineBlock" style="width: 32%; top: inherit;">
                         <tr>
                             <td align="center" colspan="2">
                                 &nbsp;</td>
                         </tr>
                         <tr>
                             <td align="center" colspan="2">
                                </td>
                         </tr>
                         <tr>
                             <td align="center" colspan="2">
                                 Players</td>
                         </tr>
                         <tr>
                             <td>
                                 &nbsp;</td>
                             <td>
                                 <asp:DropDownList ID="PlayerDDList" runat="server" Height="20px" 
                                     onselectedindexchanged="PlayerDDList_SelectedIndexChanged" Width="122px" 
                                     AutoPostBack="True">
                                 </asp:DropDownList>
                             </td>
                         </tr>
                          <tr>
                             <td>
                                 &nbsp;</td>
                             <td>
                                 &nbsp;</td>
                         </tr>
                         <tr>
                             <td align="right">
                                 Name</td>
                             <td>
                                 <asp:TextBox ID="PlayerNameTb" runat="server"></asp:TextBox>
                                 <asp:TextBox ID="PlayerIdTb" runat="server" Visible="False" Width="16px"></asp:TextBox>
                             </td>
                         </tr>
                         <tr>
                             <td align="right">
                                 Passcode</td>
                             <td>
                                 <asp:TextBox ID="PlayerPasscodeTb" runat="server"></asp:TextBox>
                             </td>
                         </tr>
                         <tr>
                             <td align="right">
                                 Mark</td>
                             <td>
                                 <asp:CheckBox ID="PlayerMarkCb" runat="server" />
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Suspend</td>
                             <td>
                                 <asp:CheckBox ID="PlayerSuspendCb" runat="server" />
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 &nbsp;</td>
                             <td align="center">
                                 <asp:Button ID="AddPlayerBtn" runat="server" onclick="AddPlayerBtn_Click" 
                                     Text="Add" Width="80px" />
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 &nbsp;</td>
                             <td align="center">
                                 <asp:Button ID="UpdatePlayerBtn" runat="server" onclick="UpdatePlayerBtn_Click" 
                                     Text="Update" Width="80px" />
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 &nbsp;</td>
                             <td align="center">
                                 <asp:Button ID="DeletePlayerBtn" runat="server" onclick="DeletePlayerBtn_Click" 
                                     Text="Delete" Width="80px" />
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 &nbsp;</td>
                             <td align="center">
                                 &nbsp;</td>
                         </tr>
                     </table>
                  </td>
             </tr>
        </table>
        
        
      <asp:Panel ID="Panel2" runat="server" style="margin-bottom: 0px">
          <asp:CheckBox ID="ClearPoolMemberCb" runat="server" 
              Text="Clear Pool Member/Dropins" />
          <asp:CheckBox ID="ClearPoolGamesCb" runat="server" Text="Clear Pool Games" />
          <asp:CheckBox ID="ResetPlayerTransferCb" runat="server" 
              Text="Reset Transfer/Free Dropin" />
          <asp:CheckBox ID="ResetPlayerMembershipCb" runat="server" 
              Text="Reset Player Membership" />
          <asp:CheckBox ID="ResetUserAuthorizationCb" runat="server" 
              Text="Reset User Authorization" />
          <asp:CheckBox ID="ClearPlayerFeeCb" runat="server" Text="Clear Player Fees" />
          <asp:Button ID="ResetPoolsBtn" runat="server" onclick="ResetSystemBtn_Click" 
              Text="Reset" Width="94px" />
    </asp:Panel>
        
        
      <br />
     </asp:Content>
