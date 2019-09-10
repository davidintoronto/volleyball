<%@ Page Title="Register Device" Language="C#" MasterPageFile="~/Mobile.Master" AutoEventWireup="true"
    CodeBehind="Plugin.aspx.cs" Inherits="VballManager.Plugin" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="LoginUserPanel" runat="server">
        <table width="100%">
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="PromptLb" runat="server" Text="Pick up your birth month and day if you wish to receive birthday greetings. Otherwise pick up 0 / 0" Style="width: 100%; font-size: 4em;"
                        ForeColor="#993300"></asp:Label>
                  </td>
            </tr>
            <tr>
                     <td align="center" colspan="2">
                    <asp:Label  runat="server" Text=". " Style="width: 100%; font-size: 3em;" ></asp:Label>
                     </td>
                </tr>
            <tr>
                     <td align="center" colspan="2">
                    <asp:Label ID="Label1"  runat="server" Text=". " Style="width: 100%; font-size: 3em;" ></asp:Label>
                     </td>
                </tr>
            <tr>
                     <td align="center">
                         <asp:DropDownList ID="MonthDDL" runat="server" Font-Bold="True" Style="width: 40%; font-size: 4em;" >
                             <asp:ListItem></asp:ListItem>
                             <asp:ListItem>0</asp:ListItem>
                             <asp:ListItem>1</asp:ListItem>
                             <asp:ListItem>2</asp:ListItem>
                             <asp:ListItem>3</asp:ListItem>
                             <asp:ListItem>4</asp:ListItem>
                             <asp:ListItem>5</asp:ListItem>
                             <asp:ListItem>6</asp:ListItem>
                             <asp:ListItem>7</asp:ListItem>
                             <asp:ListItem>8</asp:ListItem>
                             <asp:ListItem>9</asp:ListItem>
                             <asp:ListItem>10</asp:ListItem>
                             <asp:ListItem>11</asp:ListItem>
                             <asp:ListItem>12</asp:ListItem>
                         </asp:DropDownList>
                     </td>
                      <td align="center">
                          <asp:DropDownList ID="DayDDL" runat="server" Font-Bold="True" Style="width: 40%; font-size: 4em;">
                              <asp:ListItem></asp:ListItem>
                              <asp:ListItem>0</asp:ListItem>
                              <asp:ListItem>1</asp:ListItem>
                              <asp:ListItem>2</asp:ListItem>
                              <asp:ListItem>3</asp:ListItem>
                              <asp:ListItem>4</asp:ListItem>
                              <asp:ListItem>5</asp:ListItem>
                              <asp:ListItem>6</asp:ListItem>
                              <asp:ListItem>7</asp:ListItem>
                              <asp:ListItem>8</asp:ListItem>
                              <asp:ListItem>9</asp:ListItem>
                              <asp:ListItem>10</asp:ListItem>
                              <asp:ListItem>11</asp:ListItem>
                              <asp:ListItem>12</asp:ListItem>
                              <asp:ListItem>13</asp:ListItem>
                              <asp:ListItem>14</asp:ListItem>
                              <asp:ListItem>15</asp:ListItem>
                              <asp:ListItem>16</asp:ListItem>
                              <asp:ListItem>17</asp:ListItem>
                              <asp:ListItem>18</asp:ListItem>
                              <asp:ListItem>19</asp:ListItem>
                              <asp:ListItem>20</asp:ListItem>
                              <asp:ListItem>21</asp:ListItem>
                              <asp:ListItem>22</asp:ListItem>
                              <asp:ListItem>23</asp:ListItem>
                              <asp:ListItem>24</asp:ListItem>
                              <asp:ListItem>25</asp:ListItem>
                              <asp:ListItem>26</asp:ListItem>
                              <asp:ListItem>27</asp:ListItem>
                              <asp:ListItem>28</asp:ListItem>
                              <asp:ListItem>29</asp:ListItem>
                              <asp:ListItem>30</asp:ListItem>
                              <asp:ListItem>31</asp:ListItem>
                          </asp:DropDownList>
                    </td>
                </tr>
             <tr>
                     <td align="center" colspan="2">
                    <asp:Label ID="Label2"  runat="server" Text=". " Style="width: 100%; font-size: 3em;" ></asp:Label>
                     </td>
                </tr>
            <tr>
                     <td align="center" colspan="2">
                    <asp:Label ID="Label3"  runat="server" Text=". " Style="width: 100%; font-size: 3em;" ></asp:Label>
                     </td>
                </tr>
           <tr>
                     <td align="center" colspan="2">
                        <asp:Button ID="SaveBtn" runat="server" OnClick="SaveBtn_Click" Style="width: 50%;  font-size: 4em;" Text="Save" />
                     </td>
                </tr>
          </table>
    </asp:Panel>

    <br />
</asp:Content>
