<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Factors.aspx.cs" Inherits="VballManager.Factors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 486px;
        }
        .auto-style2 {
            width: 572px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <asp:Panel ID="RulePanel" runat="server" BackColor="#99FFCC" BorderColor="#3333CC"
        BorderStyle="Inset">
        <table>
            <tr>
                <td class="auto-style2">
                    <table>
                        <tr>
                            <td>
                                <asp:Table ID="FactorSettingTable" runat="server" Caption="Factors" Font-Bold="True">
                                    <asp:TableHeaderRow ID="TableHeaderRow1" HorizontalAlign="Center" BackColor="#B3AB4D">
                                        <asp:TableHeaderCell ID="TableHeaderCell1" Text="Pool" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell ID="TableHeaderCell2" Text="Low Pool" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell ID="TableHeaderCell3" Text="From" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell ID="TableHeaderCell4" Text="To" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell ID="TableHeaderCell5" Text="Coop From" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell ID="TableHeaderCell7" Text="Coop To" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell ID="TableHeaderCell8" Text="High Pool" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell ID="TableHeaderCell10" Text="From" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell ID="TableHeaderCell11" Text="To" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell ID="TableHeaderCell12" Text="Value" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell Text="Action" HorizontalAlign="Center"></asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                </asp:Table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Button ID="RecalculateFactor" runat="server" OnClick="RecalculateFactorBtn_Click" Text="Re-calculate Factors" Width="141px" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                    </table>

                </td>
                <td align="center" class="auto-style1">
                    <asp:Table ID="PoolGameFactorTable" runat="server" Caption="Game Factors" Font-Bold="True" BackColor="#BFC6EE" GridLines="Both">
                        <asp:TableHeaderRow ID="TableHeaderRow" HorizontalAlign="Center" BackColor="#B3AB4D">
                            <asp:TableHeaderCell ID="TableHeaderDateCell" Text="Date" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="TableHeaderLowPoolCell" Text="Pool B" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="TableHeaderlowPoolFactorCell" Text="Factor" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="TableHeaderInternCell" Text="Intern" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="TableHeaderHighPoolCell" Text="Pool A" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                            <asp:TableHeaderCell ID="TableHeaderHighPoolFactorCell" Text="Factor" HorizontalAlign="Center" runat="server"></asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
