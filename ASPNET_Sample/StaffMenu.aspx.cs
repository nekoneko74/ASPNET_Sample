using System;
using System.Web.UI.WebControls;

namespace ASPNET_Sample.Staff
{
    /// <summary>
    /// スタッフメニュー（StaffMenu.aspx）
    /// </summary>
    public partial class StaffMenu : StaffPage
    {
        /// <summary>
        /// Webページが読み込まれた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // スタッフのログイン状況をチェックする
            this.CheckSession();

            // セッション維持OK
            this.LblWelcome.Text = "ようこそ" + this.LoginStaff.DisplayName + "さん";

            // メニューリストを構築する
            this.BltLstMenuList.Items.Add(new ListItem("スタッフ管理機能", "./StaffList.aspx"));
        }
    }
}