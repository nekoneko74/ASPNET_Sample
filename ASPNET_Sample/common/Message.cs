namespace ASPNET_Sample
{
    /// <summary>
    /// メッセージリスト 
    /// </summary>
    public class Messages
    {
        // 成功メッセージ
        public static string M0001 = "{0}の新規登録が完了しました";
        public static string M0002 = "{0}の更新が完了しました";
        public static string M0003 = "{0}の削除が完了しました";

        // エラーメッセージ
        public static string E0001 = "{0}の新規登録に失敗しました";
        public static string E0002 = "{0}の更新に失敗しました";
        public static string E0003 = "{0}の削除に失敗しました";

        public static string E9001 = "ログイン処理に失敗しました";
        public static string E9002 = "ログインアカウントとパスワードを入力してください";
    }
}