using ASPNET_Sample.Data;
using System;

namespace ASPNET_Sample.Staff
{
    /// <summary>
    /// スタッフ用Webページの基底クラス
    /// </summary>
    /// <remarks>
    /// スタッフのログイン／ログアウト機能やエラーページへの遷移機能など、スタッフ用ページの共通機能を実装します。
    /// System.Web.UI.Pageの拡張版として使用します。
    /// </remarks>
    public class StaffPage : PageWithDebug
    {
        /// <summary>
        /// ログインしているスタッフのIDを格納しているセッション情報の名称
        /// </summary>
        public const string LOGIN_STAFF_ID = "loginStaffId";

        /// <summary>
        /// 現在ログインしているスタッフの情報
        /// </summary>
        protected StaffDto LoginStaff = null;

        /// <summary>
        /// スタッフのログイン状況をチェックする
        /// </summary>
        /// <returns>ログインしているスタッフの情報</returns>
        public StaffDto CheckSession()
        {
            // 現在ログインしているスタッフの情報をクリアしておく
            this.LoginStaff = null;

            try
            {
                // スタッフのログイン状況をチェックする
                string loginStaffIdStr = (string)this.Session[StaffPage.LOGIN_STAFF_ID];
                if (true == String.IsNullOrEmpty(loginStaffIdStr))
                {
                    throw new Exception("ログインしているスタッフの情報が存在していません。");
                }

                try
                {
                    // ログインしているスタッフの情報をデータベースから取得する
                    int loginStaffId = Convert.ToInt32(loginStaffIdStr);
                    this.LoginStaff = StaffDao.Select(loginStaffId);
                    if (null == this.LoginStaff)
                    {
                        // ログインしているスタッフの情報がデータベースから取得できなかった
                        //  ⇒ ログインしているはずのスタッフ情報が削除されている？
                        throw new Exception("ログインスタッフの情報を取得できません。");
                    }
                    else if (true == this.LoginStaff.IsDeleted)
                    {
                        // ログインしているはずのスタッフが「削除」状態になっている？
                        throw new Exception("ログインスタッフが無効化されています。");
                    }
                }
                catch (Exception ex) when (ex is FormatException || ex is OverflowException)
                {
                    throw new Exception(String.Format("無効なログインスタッフID（{0}）が設定されています。", loginStaffIdStr), ex);
                }
            }
            catch (Exception ex)
            {
                // ログインしていない ⇒ セッションエラーである
                // スタッフログイン画面に遷移する
                string errMsg = String.Format("セッションエラーです。<br>再度ログインしてください。<br><br>{0}", ex.Message);
                this.TransferSessionErrorPage(errMsg);
            }

            // ログインしているスタッフの情報を返す
            return this.LoginStaff;
        }

        /// <summary>
        /// 新たなスタッフログイン情報をセッション情報に保存する
        /// </summary>
        /// <param name="loginStaffId">ログインしているスタッフのスタッフID</param>
        public void CreateSession(int loginStaffId)
        {
            // スタッフログイン情報を破棄する
            this.DestroySession();

            // セッション情報に「ログインスタッフID」を保持する
            this.Session[StaffPage.LOGIN_STAFF_ID] = loginStaffId.ToString();
        }

        /// <summary>
        /// スタッフログイン情報を破棄する
        /// </summary>
        public void DestroySession()
        {
            // セッション情報内の「ログインスタッフID」をクリアする
            this.Session.Remove(StaffPage.LOGIN_STAFF_ID);
        }

        /// <summary>
        /// エラーページに遷移（フォワード）する
        /// </summary>
        /// <param name="errInfo">エラー情報</param>
        /// <remarks>
        /// このメソッドをtryブロック内で使用しないようにしてください。
        /// 転送元スレッドの実行が中止された際にスローされるSystem.Threading.ThreadAbortException例外がcatchされることで思わぬエラー処理が実行される場合があります。
        /// </remarks>
        public void TransferErrorPage(ErrorInfo errInfo)
        {
            this.Session[StaffError.ERROR_INFO_NAME] = errInfo;
            this.Server.Transfer("./StaffError.aspx");
        }

        /// <summary>
        /// セッションエラー：エラーページに遷移（フォワード）する
        /// </summary>
        /// <param name="message">エラーメッセージ文字列</param>
        public void TransferSessionErrorPage(string message)
        {
            // エラーメッセージをセッション情報に格納してエラーページに遷移する
            ErrorInfo errInfo = new ErrorInfo();
            errInfo.Title = "セッションエラー";
            errInfo.Message = message;
            errInfo.LinkUrl = "./StaffLogin.aspx";
            errInfo.LinkDisplayName = "ログイン画面に戻る";
            this.TransferErrorPage(errInfo);
        }

        /// <summary>
        /// システムエラー：例外発生時にエラーページに遷移（フォワード）する
        /// </summary>
        /// <param name="exception">発生している例外オブジェクト</param>
        public void TransferSystemErrorPage(Exception exception)
        {
            // エラーメッセージをセッション情報に格納してエラーページに遷移する
            string errMessage = exception.Message;
            if (null != exception.InnerException)
            {
                errMessage += "<br>" + exception.InnerException.Message;
            }
            this.TransferErrorPage(new ErrorInfo(errMessage, "システムエラー"));
        }
    }
}