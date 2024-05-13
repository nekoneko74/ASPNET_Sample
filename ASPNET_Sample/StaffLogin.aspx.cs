using ASPNET_Sample.Data;
using System;

namespace ASPNET_Sample.Staff
{
    /// <summary>
    /// スタッフログイン（StaffLogin.aspx）
    /// </summary>
    public partial class StaffLogin : StaffPage
    {
        /// <summary>
        ///「クリア」ボタンがクリックされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            // ログインアカウントとパスワードの入力欄をクリアする
            this.TxtLoginAccount.Text = string.Empty;
            this.TxtLoginPassword.Text = string.Empty;
        }

        /// <summary>
        ///「ログイン」ボタンがクリックされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            // 入力されたパラメータを取得する
            string loginAccount = this.TxtLoginAccount.Text;
            string loginPassword = this.TxtLoginPassword.Text;
            if (true == String.IsNullOrEmpty(loginAccount) || true == String.IsNullOrEmpty(loginPassword))
            {
                // 入力が不足していることをメッセージ表示する
                this.LblMessage.Text = Messages.E9002;
                this.LblMessage.Visible = true;
            }
            else
            {
                // ログイン処理を試みる
                StaffDto staffInfo = StaffDao.SelectByAccount(loginAccount);
                if (staffInfo is StaffDto)
                {
                    // パスワードが一致した ⇒ ログインに成功した
                    if (loginPassword == staffInfo.Password)
                    {
                        // ログインしているスタッフの「スタッフID」をセッション情報に保持する
                        this.CreateSession(staffInfo.StaffId);

                        // スタッフメニュー画面に遷移する
                        this.Response.Redirect("./StaffMenu.aspx");
                    }
                    // パスワードが一致しない
                    else
                    {
                        // ログイン処理に失敗したことをメッセージ表示する
                        this.LblMessage.Text = Messages.E9001;
                        this.LblMessage.Visible = true;
                    }
                }
                // 指定されたログインアカウントを持つスタッフ情報が存在しない
                else
                {
                    // ログイン処理に失敗したことをメッセージ表示する
                    this.LblMessage.Text = Messages.E9001;
                    this.LblMessage.Visible = true;
                }
            }
        }

        /// <summary>
        /// Webページが読み込まれた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // メッセージ表示をクリアする
            this.LblMessage.Text = string.Empty;
            this.LblMessage.Visible = false;
        }
    }
}