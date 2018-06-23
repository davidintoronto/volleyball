<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Detail.aspx.cs" Inherits="VballManager.Detail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 741px;
        }
    </style>
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
  
   <asp:Panel ID="ProfilePanel" runat="server">
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:ImageButton ID="BackBtn" runat="server" ImageUrl="~/Icons/Back.png" OnClick="BackBtn_Click" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="DetailPanel" runat="server">
            <asp:Table ID="DetailTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Name"
                Font-Bold="True" BackColor="#FAE0DC">
            </asp:Table>
            <asp:TextBox ID="PasscodeTb" runat="server" Style="width: 250px; font-size: 1.2em;"
                Visible="False"></asp:TextBox>
            <asp:TextBox ID="NameTb" runat="server" Style="width: 400px; font-size: 1.2em;" Visible="False"></asp:TextBox>
        </asp:Panel>
        <br />
        <asp:Panel ID="GameFeePaymentPanel" runat="server">
            <asp:Table ID="FeeTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Fees"
                Font-Bold="True" BackColor="#FAE0DC">
            </asp:Table>
            <br />
            <br />
            <asp:Table ID="PaymentTable" runat="server" Style="width: 100%; font-size: 5em;"
                Caption="Payment History" Font-Bold="True" BackColor="#FAE0DC">
            </asp:Table>
            <br />
            <br />
            <asp:Table ID="GameTable" runat="server" Style="width: 100%; font-size: 5em;" Caption="Games"
                Font-Bold="True" BackColor="#FAE0DC">
            </asp:Table>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
