<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="VballManager.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Label ID="L1" runat="server" Text="" />


    <cc1:ModalPopupExtender ID="PopupModal" runat="server" CancelControlID="" Y="300"
        OkControlID="" TargetControlID="L1" PopupControlID="ConfirmPanel" PopupDragHandleControlID="PopupHeader"
        Drag="true" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="ConfirmPanel" runat="server" CssClass="modalPopup" align="center" Style="display: none">
        <div class="popup_Body">
            <table width="100%" bgcolor="#666633">
                <tr>
                    <td align="left" class="popupHeader">Confirmation
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="CloseImageBtn" runat="server" ImageUrl="~/Icons/Close.png" />
                    </td>
                </tr>
            </table>
            <p class="popuplabel">
                <asp:Label ID="PopupLabel" Text="Please confirm your cancellation:" runat="server" Font-Bold="True"></asp:Label>
            </p>
        </div>
        <br />
        <br />
        <div class="Controls">
            <asp:ImageButton ID="ConfirmImageButton" runat="server" ImageUrl="~/Icons/yes.png" />
            <asp:ImageButton ID="NoImageButton" runat="server" ImageUrl="~/Icons/no.png" Visible="False" />
        </div>
    </asp:Panel>

    <cc1:ModalPopupExtender ID="AddDropinPopup" runat="server" CancelControlID="" Y="300"
        OkControlID="" TargetControlID="L1" PopupControlID="AddDropinPanel" PopupDragHandleControlID="PopupHeader"
        Drag="true" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="AddDropinPanel" runat="server" CssClass="modalPopup" align="center" Style="display: none">
        <div class="popup_Body">
            <table width="100%" bgcolor="#666633">
                <tr>
                    <td align="left" class="popupHeader">Add player into dropin candidate list
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Icons/Close.png" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="Controls">
            <p class="popuplabel">
                <asp:Label ID="Label2" Text="Select an existing player" runat="server"></asp:Label>
            </p>
            <asp:DropDownList runat="server" ID="AddDropinLb" ForeColor="#39952D" BackColor="#B5E1E3" Font-Bold="True" Style="width: 70%; font-size: 6em;" />
            <asp:ImageButton ID="AddDropinImageBtn" runat="server" ImageUrl="~/Icons/add.png" />
        </div>
        <div class="Controls">
            <p class="popuplabel">
                <asp:Label ID="Label1" Text="Or create a new player" runat="server"></asp:Label>
            </p>
            <asp:TextBox ID="NewPlayerTb" runat="server" Font-Bold="True" Style="width: 70%; font-size: 6em;" />
            <asp:ImageButton ID="CreateNewPlayerBtn" runat="server" ImageUrl="~/Icons/add.png" />
        </div>
    </asp:Panel>

    <asp:Panel ID="ReservationPanel" runat="server">
        <div id="wrapper" style="width: 100%; height: 100%; overflow: auto; position: relative; -webkit-overflow-scrolling: touch;">
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="ToReadmeBtn" Text="About Us" runat="server"
                            Style="font-size: 5em;" OnClick="ToReadmeBtn_Click"
                            BackColor="#F2A8AA" ForeColor="#006600" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Button ID="PreRegisterBtn" Text="2017-2018 Pre-Register" runat="server"
                            Style="font-size: 5em;" OnClick="PreRegisterBtn_Click"
                            BackColor="#52A8AA" ForeColor="Red" Font-Bold="True"
                            BorderStyle="Groove" Visible="False" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="MessageBoardPanel" runat="server">
                <asp:Table ID="MessageTextTable" runat="server" Style="width: 100%; font-size: 5em;"
                    Caption="Message Board" Font-Bold="True" BackColor="#FFFF99" ForeColor="Red">
                </asp:Table>
            </asp:Panel>
            <asp:Panel ID="GameInfoPanel" runat="server" CssClass="xxx-size">
                <asp:Table ID="NavTable" runat="server" Style="width: 100%; font-size: 5em;"
                    Font-Bold="True" ForeColor="#663300"
                    CssClass="xxx-size">
                </asp:Table>
                <asp:Table ID="GameInfoTable" runat="server" Style="width: 100%; font-size: 5em;"
                    Font-Bold="True" BackColor="#C5E2F3" ForeColor="#663300"
                    CssClass="xxx-size">
                </asp:Table>
            </asp:Panel>
            =
        <br />
            <asp:Panel ID="MemberPanel" runat="server">
                <asp:Table ID="MemberTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Members"
                    Font-Bold="True" BackColor="#CFDFC6" BorderColor="#DAE9D6">
                </asp:Table>
            </asp:Panel>
            <br />
            <asp:Panel ID="DropinPanel" runat="server">
                <asp:Table ID="DropinTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Drop-ins" Visible="false"
                    Font-Bold="True" BackColor="#DDDDF9" BorderColor="#DDDDF9">
                </asp:Table>
                <br />
                <asp:Table ID="DropinWaitingTable" runat="server" Style="width: 100%; font-size: 5em;" Visible="false"
                    Caption="Waiting List" Font-Bold="True" BackColor="#D2DB95" BorderColor="#FFECFF">
                </asp:Table>
                <br />
                <asp:Table ID="DropinCandidateTable" runat="server" Style="width: 100%; font-size: 5em;"
                    Caption="Find your name to reserve" Font-Bold="True" BackColor="#FAE0DC" BorderColor="#FFECFF">
                </asp:Table>
                <br />
                <asp:TextBox ID="DropinNameTb" runat="server" Font-Bold="True" Width="400px" Font-Size="1.2em" Visible="false"></asp:TextBox>
            </asp:Panel>
        </div>
    </asp:Panel>

</asp:Content>
