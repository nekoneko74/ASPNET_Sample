<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffError.aspx.cs" Inherits="ASPNET_Sample.Staff.StaffError" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="./css/StaffPage.css" />
    <title>エラーが発生しました</title>
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
                        <h3><asp:Label ID="LblTitle" runat="server" Text="エラーが発生しました"></asp:Label></h3>
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
                <p>
                    <asp:Label ID="LblErrorMessage" runat="server" Text="エラーが発生しました<br>（エラー情報が設定されていません）"></asp:Label>
                </p>
                <p>
                    <asp:HyperLink ID="LnkToNextPage" runat="server" NavigateUrl="./StaffLogin.aspx" Text="スタッフログインへ"></asp:HyperLink>
                </p>
            </div>
            <!-- コンテンツフッター -->
            <!-- #include file="./inc/StaffFooter.inc" -->
        </div>
    </form>
</body>
</html>