<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="v2pc.aspx.cs" Inherits="VballManager.v2pc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="PrefixTb" runat="server">TVE00000</asp:TextBox>
        <asp:TextBox ID="StartNumTb" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
    
    </div>
    <p>
        <asp:TextBox ID="ResultXsltTb" runat="server" Height="383px" 
            TextMode="MultiLine" Width="675px"></asp:TextBox>
    </p>
    <p>
        <asp:TextBox ID="ResultCpCmdTb" runat="server" Height="205px" 
            TextMode="MultiLine" Width="675px"></asp:TextBox>
    </p>
    </form>
</body>
</html>
