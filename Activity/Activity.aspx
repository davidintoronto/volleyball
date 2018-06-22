<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Activity.aspx.cs" Inherits="Reservation.Activity" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <asp:Label ID="L1" runat="server" Text="" />
    <cc1:ModalPopupExtender ID="PopupModal" runat="server" CancelControlID=""
        OkControlID="" TargetControlID="L1" PopupControlID="ConfirmPanel" PopupDragHandleControlID="PopupHeader"
        Drag="true" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="ConfirmPanel" runat="server" CssClass="modalPopup" align="center" Style="display: none">
        <div class="popup_Body">
            <table width="100%" bgcolor="#666633">
                <tr>
                    <td align="left" class="popupHeader">
                        Confirmation
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="CloseImageBtn" runat="server" ImageUrl="~/Icons/Close.png"/>
                    </td>
                </tr>
            </table>
            <p class="popuplabel">
                <asp:Label ID="PopupLabel" Text="Please confirm your cancellation:" runat="server"></asp:Label>
            </p>
        </div>
        <br />
        <br />
        <div class="Controls">
            <asp:ImageButton ID="ConfirmImageButton" runat="server"  ImageUrl="~/Icons/yes.png" />
        </div>
    </asp:Panel>
  
    <asp:Panel ID="ReservationPanel" runat="server">
        <asp:Panel ID="MessageBoardPanel" runat="server" CssClass="xxx-size">
            <asp:Table ID="MessageTextTable" runat="server" Style="width: 100%; font-size: 5em;"
                Caption="Message Board" Font-Bold="True" BackColor="#FFFF99" ForeColor="#009933"
                >
            </asp:Table>
        </asp:Panel>
        <asp:Panel ID="GameInfoPanel" runat="server" CssClass="xxx-size">
            <asp:Table ID="GameInfoTable" runat="server" Style="width: 100%; font-size: 3em;"
                Caption="Game Info" Font-Bold="True" BorderColor="#C5E2F3" ForeColor="#663300"
                CssClass="xxx-size" BackColor="#F3F5B4">
            </asp:Table>
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="DropinPanel" runat="server">
            <asp:Table ID="DropinTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Reserved"
                Font-Bold="True" BackColor="#CFDFC6" BorderColor="#DAE9D6" Visible="True">
            </asp:Table>
            <br />
             <asp:Table ID="DropinWaitingTable" runat="server" Style="width: 100%; font-size: 5em;"
                Caption="Waiting List" Font-Bold="True" BackColor="#D2DB95" BorderColor="#FFECFF" Visible="False">
            </asp:Table>
            <br />
           <asp:Table ID="DropinCandidateTable" runat="server" Style="width: 100%; font-size: 5em;"
                Caption="Enter or find your name to reserve" Font-Bold="True" BackColor="#FAE0DC" BorderColor="#FFECFF" >
            </asp:Table>
            <br />
            <asp:TextBox ID="DropinNameTb" runat="server" Font-Bold="True" Width="600px" Font-Size="1.2em"></asp:TextBox>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
