<%@ Page Title="2017-2018 Pre-Register" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true" CodeBehind="PreRegister.aspx.cs" Inherits="VballManager.PreRegister" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Label ID="L1" runat="server" Text="" />
     <cc1:ModalPopupExtender ID="ConfirmPopup" runat="server" CancelControlID=""
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
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Icons/Close.png" />
                    </td>
                </tr>
            </table>
            <p class="popuplabel">
                Please comfirm:
            </p>
        </div>
        <br />
        <br />
        <div class="Controls">
            <asp:ImageButton ID="ConfirmImageButton" runat="server" 
                OnClick="Confirm_Click" ImageUrl="~/Icons/yes.png" 
                PostBackUrl="~/PreRegister.aspx" />
        </div>
    </asp:Panel>
        <asp:Panel ID="SurveyPanel" runat="server">
            <asp:Table ID="SurveyTable" runat="server"  Style="width: 100%;  font-size: 5em;" Caption="2016-2017 Member Pre-registration (*)"
                Font-Bold="True" BackColor="#FAE0DC"> 
             <asp:TableHeaderRow ID="TableHeaderRow" HorizontalAlign="Left" BackColor="#B3AB4D">
                <asp:TableHeaderCell ID="TableHeaderCell9" Text="" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="TableHeaderCell6" Text="Player Name" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="PlayedCountCell" Text="Played" HorizontalAlign="Left" runat="server"></asp:TableHeaderCell>
                <asp:TableHeaderCell ID="DayHeaderCell" Text="Member" HorizontalAlign="Left"></asp:TableHeaderCell>
             </asp:TableHeaderRow>
            </asp:Table>
        </asp:Panel>
        <asp:TextBox ID="DropinNameTb" runat="server" Font-Bold="True" Width="400px" Font-Size="1.2em" Visible="False"></asp:TextBox>
</asp:Content>

