<%@ Page Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true" CodeBehind="Score.aspx.cs" Inherits="VballManager.Score" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: 3em;
        }
        .label
        {
            font-size: 5em;
        }
       .tdtextBox
        {
            font-size: 3em;
        }
        .style3
        {
            font-size: 5em;
            width: 205px;
        }
        .style4
        {
            font-size: 3em;
            background-color: #CCFF99;
        }
        .style5
        {
            font-size: 5em;
            background-color: #CCFF99;
        }
        .style6
        {
            font-size: 5em;
            width: 205px;
            background-color: #CCFF99;
        }
        .style7
        {
            font-size: 3em;
            background-color: #99FFCC;
        }
        .style8
        {
            font-size: 5em;
            background-color: #99FFCC;
        }
        .style9
        {
            font-size: 5em;
            background-color: #99FFCC;
        }
        .style10
        {
            font-size: 3em;
            background-color: #D6F0BD;
        }
        .style11
        {
            font-size: 5em;
            background-color: #D6F0BD;
        }
        .style12
        {
            font-size: 5em;
            width: 205px;
            background-color: #D6F0BD;
        }
        .style13
        {
            font-size: 3em;
            background-color: #CBC1E6;
        }
        .style14
        {
            font-size: 5em;
            background-color: #CBC1E6;
        }
        .style15
        {
            font-size: 5em;
            width: 205px;
            background-color: #CBC1E6;
        }
        .style16
        {
            font-size: 3em;
            background-color: #E6E2B7;
        }
        .style17
        {
            font-size: 5em;
            background-color: #E6E2B7;
        }
        .style18
        {
            font-size: 5em;
            width: 205px;
            background-color: #E6E2B7;
        }
    </style>
  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <span class="style8">
<br /><br /><br /><br /><br /><br /><br /><br /><br />
    </span>
    <table width="100%">
        <tr class="style8">
            <td class="style6" rowspan="4">
&nbsp;1 (7:30-7:50)</td>
            <td align="center" class="style5">
                B1- SLJ</td>
            <td align="center" class="style5">
                B3 - Hope</td>
        </tr>
        <tr>
            <td align="center" class="style4">
                <asp:TextBox ID="B11" runat="server" AutoPostBack="True" 
                    Width="450px" ontextchanged="SaveB11_Click" CssClass="style1"></asp:TextBox>
            </td>
            <td align="center" class="style4">
                <asp:TextBox ID="B31" runat="server" Width="450px" 
                    ontextchanged="SaveB31_Click" CssClass="style1"></asp:TextBox>
            </td>
        </tr>
        <tr class="style8">
            <td align="center" class="style5">
                A1</td>
            <td align="center" class="style5">
                A2</td>
        </tr>
 
        <tr>
            <td align="center" class="style4">
                <asp:TextBox ID="A11" runat="server" Width="450px" 
                    ontextchanged="SaveA11_Click" CssClass="style1"></asp:TextBox>
            </td>
            <td align="center" class="style4">
                <asp:TextBox ID="A21" runat="server" Width="450px" 
                    ontextchanged="SaveA21_Click" CssClass="style1"></asp:TextBox>
            </td>
        </tr>
 
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td align="center" class="label">
                &nbsp;</td>
            <td align="center" class="label">
                </span></td>
        </tr>
 
        <tr class="style8">
            <td class="style9" rowspan="4">
&nbsp;2 (7:55-8:15)</td>
            <td align="center" class="style8">
                B2- RD</td>
            <td align="center" class="style8">
                B3 - Hope</td>
        </tr>
        <tr>
            <td align="center" class="style7">
                <asp:TextBox ID="B22" runat="server" Width="450px" 
                    ontextchanged="SaveB22_Click" CssClass="style1"></asp:TextBox>
            </td>
            <td align="center" class="style7">
                <asp:TextBox ID="B32" runat="server" Width="450px" 
                    ontextchanged="SaveB32_Click" CssClass="style1"></asp:TextBox>
            </td>
        </tr>
        <tr class="style8">
            <span class="style8">
            <td align="center" class="style8">
                A1</td>
            <td align="center" class="style8">
                A2</td>
            </span>
        </tr>
 
        <tr>
            <td align="center" class="style7">
                <asp:TextBox ID="A12" runat="server" Width="450px" 
                    ontextchanged="SaveA12_Click" CssClass="style1"></asp:TextBox>
            </td>
            <td align="center" class="style7">
                <asp:TextBox ID="A22" runat="server" Width="450px" 
                    ontextchanged="SaveA22_Click" CssClass="style1"></asp:TextBox>
            </td>
        </tr>
 
        <tr class="style8">
            <td class="style3">
                &nbsp;</td>
            <td align="center" class="label">
                &nbsp;</td>
            <td align="center" class="label">
                &nbsp;</td>
        </tr>
 
         <tr class="style8">
            <td class="style12" rowspan="4">
&nbsp;3 (8:20-8:40)</td>
            <td align="center" class="style11">
                B1- SLJ</td>
            <td align="center" class="style11">
                B2 - RD</td>
        </tr>
 
         <tr class="style8">
            <td align="center" class="style10">
                <asp:TextBox ID="B13" runat="server" Width="450px" 
                    ontextchanged="SaveB13_Click" CssClass="style1"></asp:TextBox>
             </td>
            <td align="center" class="style10">
                <asp:TextBox ID="B23" runat="server" Width="450px" 
                    ontextchanged="SaveB23_Click" CssClass="style1"></asp:TextBox>
             </td>
        </tr>
        <tr class="style8">
            <td align="center" class="style11">
                A1</td>
            <td align="center" class="style11">
                A2</td>
        </tr>
        <tr>
            <td align="center" class="style10">
                <asp:TextBox ID="A13" runat="server" Width="450px" 
                    ontextchanged="SaveA13_Click" CssClass="style1"></asp:TextBox>
            </td>
            <td align="center" class="style10">
                <asp:TextBox ID="A23" runat="server" Width="450px" 
                    ontextchanged="SaveA23_Click" CssClass="style1"></asp:TextBox>
            </td>
        </tr>
 
        <tr class="style8">
            <td class="style3">
                &nbsp;</td>
            <td align="center" class="label">
                &nbsp;</td>
            <td align="center" class="label">
                &nbsp;</td>
        </tr>
 
   
        <tr>
            <td class="style9" align="center" colspan="3" bgcolor="#7AC530">
                Final</td>
            <span class="style8"></span>
        </tr>
 
      <tr class="style8">
            <td class="style15" rowspan="4">
&nbsp;4 (8:45-9:05)</td>
            <td align="center" class="style14">
                D2</td>
            <td align="center" class="style14">
                D3</td>
        </tr>
        <tr>
            <td align="center" class="style13">
                <asp:TextBox ID="D24" runat="server" CssClass="style1" Width="450px" 
                    ontextchanged="SaveD24_Click"></asp:TextBox>
            </td>
            <td align="center" class="style13">
                <asp:TextBox ID="D34" runat="server" CssClass="style1" Width="450px" 
                    ontextchanged="SaveD34_Click"></asp:TextBox>
            </td>
        </tr>
        <tr class="style8">
            <span class="style8">
            <td align="center" class="style14">
                D1</td>
            <td align="center" class="style14">
                A1</td>
            </span>
        </tr>
 
        <tr>
            <td align="center" class="style13">
                <asp:TextBox ID="D14" runat="server" CssClass="style1" Width="450px" 
                    ontextchanged="SaveD14_Click"></asp:TextBox>
            </td>
            <td align="center" class="style13">
                <asp:TextBox ID="A14" runat="server" CssClass="style1" Width="450px" 
                    ontextchanged="SaveA14_Click"></asp:TextBox>
            </td>
        </tr>
 
        <tr class="style8">
            <td class="style18" rowspan="4">
&nbsp;5 (9:10-9:30)</span></td>
            <td align="center" class="style17">
                D2</td>
            <td align="center" class="style17">
                D3</td>
        </tr>
        <tr>
            <td align="center" class="style16">
                <asp:TextBox ID="D25" runat="server" CssClass="style1" Width="450px" 
                    ontextchanged="SaveD25_Click"></asp:TextBox>
            </td>
            <td align="center" class="style16">
                <asp:TextBox ID="D35" runat="server" CssClass="style1" Width="450px" 
                    ontextchanged="SaveD35_Click"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" class="style17">
                D1</td>
            <td align="center" class="style17">
                A2</td>
        </tr>
 
        <tr>
            <td align="center" class="style16">
                <asp:TextBox ID="D15" runat="server" CssClass="style1" Width="450px" 
                    ontextchanged="SaveD15_Click"></asp:TextBox>
            </td>
            <td align="center" class="style16">
                <span class="style8"></span><asp:TextBox ID="A25" runat="server" 
                    CssClass="style1" Width="450px" 
                    ontextchanged="SaveA25_Click"></asp:TextBox>
            </td>
        </tr>
 
        <tr>
            <td class="style3">
                &nbsp;</td>
            <span class="style8">
            <td align="center"  colspan="2">
                <asp:Button ID="SaveBtn" runat="server" 
                     Text="Save" Width="627px" 
                    onclick="SaveBtn_Click" Font-Bold="True" 
                    Height="258px" 
                    style="font-weight: 700; font-size: 3em; " />
            </td>
            </span>
        </tr>
 
    </table>

</asp:Content>

