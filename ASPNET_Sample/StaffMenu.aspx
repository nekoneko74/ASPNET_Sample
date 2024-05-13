<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffMenu.aspx.cs" Inherits="ASPNET_Sample.Staff.StaffMenu" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="./css/StaffPage.css" />
    <title>スタッフメニュー</title>
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
                        <h3><asp:Label ID="LblTitle" runat="server" Text="スタッフメニュー"></asp:Label></h3>
                        <asp:Label ID="LblWelcome" runat="server" Text="ようこそ"></asp:Label>
                    </div>
                    <div class="logoutArea">
                        <asp:HyperLink ID="LnkLogout" runat="server" NavigateUrl="./StaffLogout.aspx" Text="ログアウト"></asp:HyperLink>
                    </div>
                </div>
                <!-- メッセージ -->
                <div class="messageArea">
                    <asp:Label ID="LblMessage" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <!-- コンテンツボディ -->
            <div class="body alignCenter">
                <div style="margin-top: 3em;">
                    <asp:BulletedList ID="BltLstMenuList" runat="server" BulletStyle="Square" DisplayMode="HyperLink"></asp:BulletedList>
                </div>
            </div>
            <!-- コンテンツフッター -->
            <!-- #include file="./inc/StaffFooter.inc" -->
        </div>
    </form>
</body>
</html>