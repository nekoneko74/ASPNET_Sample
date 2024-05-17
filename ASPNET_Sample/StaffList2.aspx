<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffList2.aspx.cs" Inherits="ASPNET_Sample.Staff.StaffList2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="./css/StaffPage.css" />
    <title>スタッフ一覧</title>
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
                        <h3><asp:Label ID="LblTitle" runat="server" Text="スタッフ一覧"></asp:Label></h3>
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
            <div class="body">
                <div style="margin-top: 1em;">
                    検索条件
                    <table>
                        <tr>
                            <td style="text-align: right;">アカウント：</td>
                            <td>
                                <asp:TextBox ID="TxtAccount" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">表示名：</td>
                            <td>
                                <asp:TextBox ID="TxtDisplayName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">スタッフ種別：</td>
                            <td>
                                <asp:DropDownList ID="DrLstStaffType" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="margin-top: 0.8em;">
                            <td style="text-align: center;" colspan="2">
                                <asp:Button ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" Text="検索" />
                                <asp:Button ID="BtnClear" runat="server" OnClick="BtnClear_Click" Text="クリア" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin-top: 1.5em;">
                    <asp:GridView ID="GrdvStaffList" runat="server" EmptyDataText="該当するスタッフ情報は存在しません" ShowHeaderWhenEmpty="True" OnRowEditing="GrdvStaffList_RowEditing" OnRowDeleting="GrdvStaffList_RowDeleting" BorderStyle="Solid" OnRowDataBound="GrdvStaffList_RowDataBound" DataKeyNames="StaffId" DataSourceID="SqlDataSource" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
                        <Columns>
                            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                            <asp:BoundField DataField="Account" HeaderText="アカウント" SortExpression="Account" />
                            <asp:BoundField DataField="Password" HeaderText="パスワード" SortExpression="Password" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="DisplayName" HeaderText="表示名" SortExpression="DisplayName" />
                            <asp:BoundField DataField="StaffType" HeaderText="スタッフ種別" SortExpression="StaffType" >
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UpdateDate" HeaderText="最終更新日時" SortExpression="UpdateDate" DataFormatString="{0:yyyy/MM/dd HH:mm}">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UpdateStaffId" HeaderText="最終更新者" SortExpression="UpdateStaffId" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="DeleteDate" HeaderText="削除日時" SortExpression="DeleteDate" Visible="False" ></asp:BoundField>
                            </Columns>
                    </asp:GridView>
                    <div style="margin-top: 1.5em;">
                        <asp:Button ID="BtnAddNew" runat="server" Text="新規登録" OnClick="BtnAddNew_Click" />
                    </div>
                </div>
            </div>
            <!-- フッター -->
            <!-- #include file="./inc/StaffFooter.inc" -->
        </div>
        <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ASPNETConnectionString %>" SelectCommand="SELECT StaffId, Account, Password, DisplayName, StaffType, UpdateDate, UpdateStaffId, DeleteDate FROM Staff WHERE (Account LIKE '%' + @Account + '%') AND (DisplayName LIKE '%' + @DisplayName + '%') AND (@StaffType = - 1 OR StaffType = @StaffType) AND (DeleteDate IS NULL) ORDER BY Account">
            <SelectParameters>
                <asp:ControlParameter ControlID="TxtAccount" Name="Account" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                <asp:ControlParameter ControlID="TxtDisplayName" Name="DisplayName" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                <asp:ControlParameter ControlID="DrLstStaffType" PropertyName="SelectedValue" Name="StaffType" Type="Int32"></asp:ControlParameter>
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>