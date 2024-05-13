<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffLogin.aspx.cs" Inherits="ASPNET_Sample.Staff.StaffLogin" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="./css/StaffPage.css" />
    <title>スタッフログイン</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="contents">
            <!-- コンテンツヘッダー -->
            <div class="header">
                <!-- バナー -->
                <!-- #include file="./inc/StaffBanner.inc" -->
                <!-- ページタイトルとログインユーザー -->
                <div class="headerArea">
                    <div class="titleArea">
                        <h3><asp:Label ID="LblTitle" runat="server" Text="スタッフログイン"></asp:Label></h3>
                    </div>
                    <div class="logoutArea"></div>
                </div>
                <!-- メッセージ -->
                <div class="messageArea">
                    <asp:Label ID="LblMessage" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <!-- コンテンツボディ -->
            <div class="body alignCenter">
                <div style="margin-top: 3em;">
                    <table>
                        <tr>
                            <td style="text-align: right;">ログインアカウント：</td>
                            <td>
                                <asp:TextBox ID="TxtLoginAccount" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">パスワード：</td>
                            <td>
                                <asp:TextBox ID="TxtLoginPassword" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin-top: 5em; text-align: center;">
                    <asp:Button ID="BtnLogin" runat="server" Text="ログイン" OnClick="BtnLogin_Click" />
                    <asp:Button ID="BtnClear" runat="server" Text="クリア" OnClick="BtnClear_Click" />
                </div>
            </div>
            <!-- コンテンツフッター -->
            <!-- #include file="./inc/StaffFooter.inc" -->
        </div>
    </form>
</body>
</html>