<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffLogout.aspx.cs" Inherits="ASPNET_Sample.Staff.StaffLogout" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="./css/StaffPage.css" />
    <title>ログアウト完了</title>
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
                        <h3><asp:Label ID="LblTitle" runat="server" Text="ログアウト完了"></asp:Label></h3>
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
                    <div style="text-align: center;">
                        ログアウトしました。
                    </div>
                    <div style="margin-top: 3em; text-align: center;">
                        <a href="./StaffLogin.aspx">スタッフログイン画面に戻る</a>
                    </div>
                </div>
            </div>
            <!-- コンテンツフッター -->
            <!-- #include file="./inc/StaffFooter.inc" -->
        </div>
    </form>
</body>
</html>