<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pools.aspx.cs" Inherits="VballManager.Pools" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .inlineBlock
        {}
        .style6
        {
            width: 99px;
        }
        .style8
        {
            width: 167px;
        }
        .style9
        {
            width: 85px;
        }
        .style12
        {
            width: 87px;
        }
        .style13
        {
            width: 38px;
        }
        .style14
        {
            width: 193px;
        }
        .style15
        {
            width: 130px;
        }
    </style>
 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
     <cc1:ModalPopupExtender ID="PlayerListPopup" runat="server" CancelControlID=""
        OkControlID="" TargetControlID="SelectPlayerLabel" PopupControlID="PlayerListPanel" PopupDragHandleControlID=""
        Drag="true">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="PlayerListPanel" runat="server"
         BackColor="#CEE8FF">
        <div >
            <table width="100%" >
                <tr>
                    <td>
                        <asp:Label ID="SelectPlayerLabel" Text="Select Players To Add" Font-Bold="True" Font-Size="Large" runat="server"/>
                    </td>
                </tr>
            </table>
            <asp:CheckBoxList ID="PlayerListbox" runat="server" Height="288px" 
                        Width="898px" 
                        RepeatColumns="4" 
                        RepeatDirection="Horizontal" Font-Size="Medium" Font-Bold="True" TextAlign="Right">
            </asp:CheckBoxList>
         </div>
        <br />
        <br />
        <div class="Controls">
            <asp:Button ID="AddBtn" runat="server" Text="  Add  " OnClick="AddPlayers_Click" PostBackUrl="~/Pools.aspx" />
           <asp:Button ID="CancelBtn" runat="server" Text="Cancel" PostBackUrl="~/Pools.aspx" />
        </div>
        <br />
        <br />
   </asp:Panel>
       
        
      <br />
     <asp:Panel ID="PoolPanel" runat="server" BackColor="#FFCC99">
        
        <table style="width:100%;">
        
        <tr>
        <td rowspan="6" class="style12">
        <asp:ListBox ID="PoolListbox" runat="server" AutoPostBack="True" Height="178px" 
                         onselectedindexchanged="PoolList_SelectedIndexChanged" style="margin-left: 0px" 
                         Width="86px"></asp:ListBox>
          </td>
          <td rowspan="6" class="style9">
                    <table class="inlineBlock" style="width: 80%; top: inherit;">
                        <tr>
                            <td align="center" class="style6">
                                Pools</td>
                        </tr>
                        <tr>
                            <td class="style6">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style6">
                                <asp:TextBox ID="PoolNameTb" runat="server" Width="66px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style6">
                                &nbsp;</td>
                        </tr>
                         <tr>
                            <td align="center" class="style6">
                                <asp:Button ID="AddPoolBtn" runat="server" onclick="AddPoolBtn_Click" 
                                    Text="Add" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="style6"  >
                                <asp:Button ID="UpdatePoolBtn" runat="server" onclick="UpdatePoolBtn_Click" 
                                    Text="Update" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="style6"  >
                                <asp:Button ID="DeletePoolBtn" runat="server" 
                                    Text="Delete" Width="80px" />
                            </td>
                        </tr>
                    </table>
         </td>   
        <td colspan="6" align="center"><asp:Label Text="Pool Settings" runat="server" 
                Font-Bold="True"></asp:Label></td></tr>
            <tr>
                <td class="style15"  >
                    Title
                </td>
                <td   colspan="1">
                    <asp:TextBox ID="TitleTb" runat="server" Width="96%" EnableTheming="True"></asp:TextBox>
                </td>
                  <td class="style14" >Day of Week</td>
                 <td  >
                     <asp:DropDownList ID="DayOfWeekDl" runat="server" Height="25px" Width="88px">
                         <asp:ListItem>Monday</asp:ListItem>
                         <asp:ListItem>Friday</asp:ListItem>
                     </asp:DropDownList>
                 </td>
                                 <td>
                                     Co-op auto reserve</td>
                <td>
                    <asp:CheckBox ID="AutoCoopReserveCb" runat="server" />
                </td>

            </tr>
           <tr> 
               <td class="style15">
                   Scheduled Time</td>
                <td>
                    <asp:TextBox ID="ScheduleTimeTb" runat="server" Width="96%" 
                        EnableTheming="True"></asp:TextBox>
                </td>
               <td class="style14"  > Maximum Players </td>
                <td  >
                     <asp:TextBox ID="MaxPlayers" runat="server" Width="32px"></asp:TextBox>
                     <asp:CompareValidator ID="CompareValidator5" runat="server" 
                         ControlToValidate="MaxPlayers" ErrorMessage="Integers only please" 
                         ForeColor="#FF3300" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                </td>
                <td>
                    Co-op reserve hour</td>
                <td>
                    <asp:TextBox ID="CoopReserveHourTb" runat="server" Width="29px"></asp:TextBox>
                </td>
              </tr>
             <tr>
                 <td rowspan="3" class="style15">
                    Message Board</td>
                 <td rowspan="3"  align="center">
                     <asp:TextBox ID="MessageTb" runat="server" 
                         TextMode="MultiLine" Height="72px" Width="255px"></asp:TextBox>
                 </td>
                <td class="style14"  >Allow add new dropin</td>
                <td  >
                    <asp:CheckBox ID="AllowAddingDropinCb" runat="server" />
                 </td>
                                 <td>
                                     Co-op reserve less than players</td>
                <td>
                    <asp:TextBox ID="CoopLessThanPlayersTb" runat="server" Width="29px"></asp:TextBox>
                </td>

           </tr>
           <tr>
              <td class="style14"  >Membership Fee</td>
                <td  >
                    <asp:TextBox ID="MemberShipFeeTb" runat="server" Width="29px"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator6" runat="server" 
                        ControlToValidate="MemberShipFeeTb" ErrorMessage="Integers only please" 
                        ForeColor="#FF3300" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
               </td>
                               <td>
                                   Co-op max players</td>
                <td>
                    <asp:TextBox ID="MaxCoopPlayerTb" runat="server" Width="29px"></asp:TextBox>
                </td>

            </tr>
           <tr>
                           <td class="style14">
                               Days to reserve(Member)</td>
                <td>
                    <asp:TextBox ID="DaysToReserveForMemberTb" runat="server" Width="29px">0</asp:TextBox>
                </td>

              <td  >
                    Stats Type</td>
                 <td>
                     <asp:DropDownList ID="StatsTypeDdl" runat="server" Height="25px" Width="88px">
                         <asp:ListItem>None</asp:ListItem>
                         <asp:ListItem>Day</asp:ListItem>
                         <asp:ListItem>Week</asp:ListItem>
                     </asp:DropDownList>
                           </td>
           </tr>
           <tr>
                           <td class="style12" colspan="2">
                               <asp:Button ID="SendPrimaryMemberNotificationBtn" runat="server" OnClick="SendPrimaryMemberNotificationWechatMessageBtn_Click" Text="Send Primary Member Notificatioin" Width="212px" />
                </td>
               <td class="style15">
                    Wechat Group</td>
               <td>
                    <asp:TextBox ID="WechatGroupName" runat="server" EnableTheming="True" 
                        Width="96%"></asp:TextBox>
                           </td>
               <td class="style14">
                    Days to reserve</td>
                <td>
                    <asp:TextBox ID="DaysToReserveTb" runat="server" Width="29px">0</asp:TextBox>
                           </td>
               <td>
                    &nbsp;</td>
               <td>
                    <asp:Button ID="SavePoolBtn" runat="server" onclick="SavePoolBtn_Click" 
                        Text="Save" Width="67px" />
                           </td>
           </tr>
        </table>
        
        <br />
        
    </asp:Panel>    <br />
    <asp:Panel ID="PoolDetailPanel" runat="server" BackColor="#FFCC99" Visible="false">
        <asp:Panel ID="Panel3" runat="server" CssClass="inlineBlock" Width="189px" 
            Height="304px">
        <table>
        <tr>
        <td>
            <asp:ListBox ID="MemberListbox" runat="server" AutoPostBack="True" 
                Height="295px" Width="190px" 
                onselectedindexchanged="MemberListbox_SelectedIndexChanged"></asp:ListBox>
            </td>
        <td>
            <table class="inlineBlock" style="width: 45%; top: inherit;">
                <tr>
                    <td align="center" class="style8">
                        Members</td>
                </tr>
                <tr>
                    <td class="style8">
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style8">
                        <asp:Button ID="AddMemberBtn" runat="server" onclick="AddMemberBtn_Click" 
                            Text="Add" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style8">
                        <asp:Button ID="RemoveMemberBtn" runat="server" onclick="RemoveMemberBtn_Click" 
                            Text="Remove" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style8">
                         <asp:Button ID="SaveMemberBtn" runat="server" onclick="SaveMemberBtn_Click" 
                            Text="Save" Width="80px" />
                       &nbsp;</td>
                </tr>
               <tr>
                    <td align="center" class="style8">
                    </td>
                </tr>
                 <tr>
                    <td align="center" class="style8">Cancelled
                    <asp:CheckBox ID="MemberCancelledCb" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="style8">Suspended
                    <asp:CheckBox ID="MemberSuspendedCb" runat="server" />
                    </td>
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
            <asp:ListBox ID="DropinListbox" runat="server" AutoPostBack="True" 
                Height="300px" style="margin-left: 0px" Width="211px" 
                onselectedindexchanged="DropinListbox_SelectedIndexChanged"></asp:ListBox>
            </td>
        <td>
            <table class="inlineBlock" style="width: 32%; top: inherit;">
                <tr>
                    <td align="center">
                        Dropins</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="AddDropinBtn" runat="server" onclick="AddDropinBtn_Click" 
                            Text="Add" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="RemoveDropinBtn" runat="server" onclick="RemoveDropinBtn_Click" 
                            Text="Remove" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="SaveDropinBtn" runat="server" onclick="SaveDropinBtn_Click" 
                            Text="Save" Width="80px" />                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">Co-op
                    <asp:CheckBox ID="DropinCoopCb" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">Suspended
                    <asp:CheckBox ID="DropinSuspendedCb" runat="server" />
                    </td>
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
            <asp:ListBox ID="GameListbox" runat="server" AutoPostBack="True" 
                DataTextFormatString="{0:d}" Height="300px" 
                onselectedindexchanged="GameList_SelectedIndexChanged" Width="132px">
            </asp:ListBox>
            </td>
        <td>
            <table class="inlineBlock" style="width: 32%; top: inherit;">
                <tr>
                    <td align="center" colspan="2">
                        Games</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right">
                        Date</td>
                    <td>
                        <asp:TextBox ID="GameDateTb" runat="server" Height="65px" Width="149px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td align="center">
                        <asp:Button ID="AddGameBtn" runat="server" onclick="AddGameBtn_Click" 
                            Text="Add" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td align="center">
                        <asp:Button ID="UpdateGameBtn" runat="server" onclick="UpdateGameBtn_Click" 
                            Text="Update" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td align="center">
                        <asp:Button ID="DeleteGameBtn" runat="server" onclick="DeleteGameBtn_Click" 
                            Text="Delete" Width="80px" />
                    </td>
                </tr>
               <tr>
                    <td>
                        &nbsp;</td>
                    <td align="center">
                        <asp:LinkButton ID="ResevLink" runat="server" onclick="ResevLink_Click">Reserve Page</asp:LinkButton>
                        </td>
                        </tr>
                        <tr>
                      <td>
                        &nbsp;</td>
                        <td align="center">
                        <asp:LinkButton ID="ResLinkId" runat="server" onclick="ResLink_Click">Reserve Page Id</asp:LinkButton>
                    </td>
                </tr>
              <tr>
                    <td>
                        &nbsp;</td>
                    <td align="center">
                        
                        <asp:Button ID="ClearGamesBtn" runat="server" onclick="ClearGamesBtn_Click" OnClientClick="if ( !confirm('Are you sure you want to clear the games?')) return false;"
                            Text="Clear Games" Enabled="False" />
                        
                    </td>
                </tr>
            </table>
            </td>
        </tr></table>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
