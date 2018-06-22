<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Admin.aspx.cs" Inherits="Reservation.Admin" %>

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
        .style26
        {
            height: 191px;
        }
        .style27
        {
            height: 26px;
        }
        .style-textbox
        {
            font-size: 7em;
            width: 100%;
            background-color: Silver
        }
        .style-label
        {
            color: Orange;
            font-size: 7em;
            width: 100%;
        }
        .button
        {
            font-size: 7em;
            width: 300px;
        }
        .style30
        {
            font-size: 9em;
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="style22">
        <tr>
            <td align="center" class="style-label">
                <strong>Activity Settings</strong>
            </td>
        </tr>
        <tr>
            <td class="style-label" align="center">
            </td>
        </tr>
        <tr>
            <td class="style-label" align="center">
                Activities
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DropDownList ID="GameList" runat="server" class="style30" AutoPostBack="True"
                    OnSelectedIndexChanged="GameList_SelectedIndexChanged" BackColor="#99CCFF">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style-label" align="center">
                Date
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:TextBox ID="GameDateTb" runat="server" class="style-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style-label" align="center">
                Activity Name
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:TextBox ID="TitleTb" runat="server" class="style-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style-label" align="center">
                Time
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:TextBox ID="ScheduledTimeTb" runat="server" class="style-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style-label" align="center">
                Location
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:TextBox ID="LocationTb" runat="server" class="style-textbox" Height="350px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style-label" align="center">
                Capacity
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:TextBox ID="MaxPlayersTb" runat="server" class="style-textbox"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="MaxPlayersTb"
                    ErrorMessage="Integers only please" ForeColor="#FF3300" Operator="DataTypeCheck"
                    Type="Integer"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="style-label" align="center">
                Message Board
            </td>
        </tr>
        <tr>
            <td align="center" class="style26">
                <asp:TextBox ID="MessageTextTb" runat="server" Height="350px" TextMode="MultiLine"
                    class="style-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" class="style26">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="publishlb" runat="server" Text="Publish"  Font-Size="6.5em" ForeColor="Orange"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:CheckBox ID="PublishCb" runat="server" CssClass="ChkBoxClass"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" class="style26">
                <asp:Label ID="PasswordLb" runat="server" Text="Admin Password" 
                    Font-Size="6.5em" ForeColor="Orange"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:TextBox ID="PasswordTb" runat="server" class="style-textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="AddGameBtn" runat="server" OnClick="AddGameBtn_Click" class="button"
                    Text="Add" />
                <asp:Button ID="UpdateGameBtn" runat="server" OnClick="UpdateGameBtn_Click" class="button"
                    Text="Update" ForeColor="#336600" />
                <asp:Button ID="DeleteGameBtn" runat="server" OnClick="DeleteGameBtn_Click" class="button"
                    Text="Delete" ForeColor="#990000" />
            </td>
        </tr>

     </table>
     <br />
     <br />
     <br />
     <br />
     <br />
     <asp:LinkButton ID="GotoHomePage" runat="server" Font-Size="5em" 
        onclick="GotoHomePage_Click" >Home Page</asp:LinkButton>
</asp:Content>
