using System;

namespace ASPNET_Sample.Staff
{
    /// <summary>
    /// ログアウト（StaffLogout.aspx）
    /// </summary>
    public partial class StaffLogout : StaffPage
    {
        /// <summary>
        /// Webページが読み込まれた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // セッション情報内のスタッフログイン情報を破棄する
            this.DestroySession();
        }
    }
}