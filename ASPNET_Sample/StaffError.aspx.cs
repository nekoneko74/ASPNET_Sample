using System;
using System.Text;

namespace ASPNET_Sample.Staff
{
    /// <summary>
    /// スタッフ用エラーページ（StaffError.aspx）の表示内容
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// エラーページのタイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 遷移先WebページのURL
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 遷移先Webページの名称
        /// </summary>
        public string LinkDisplayName { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErrorInfo()
        {
            this.Message = string.Empty;
            this.Title = string.Empty;
            this.LinkUrl = string.Empty;
            this.LinkDisplayName = string.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public ErrorInfo(string message) : this()
        {
            this.Message = message;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="title">エラーページのタイトル</param>
        public ErrorInfo(string message, string title) : this(message)
        {
            this.Title = title;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="title">エラーページのタイトル</param>
        /// <param name="linkUrl">エラーページからさらに遷移する先のWebページのURL</param>
        public ErrorInfo(string message, string title, string linkUrl) : this(message, title)
        {
            this.LinkUrl = linkUrl;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="title">エラーページのタイトル</param>
        /// <param name="linkUrl">エラーページからさらに遷移する先のWebページのURL</param>
        /// <param name="linkDisplayName">エラーページからさらに遷移する先のWebページの名称</param>
        public ErrorInfo(string message, string title, string linkUrl, string linkDisplayName) : this(message, title, linkUrl)
        {
            this.LinkDisplayName = linkDisplayName;
        }

        /// <summary>
        /// オブジェクトを表す文字列を取得する
        /// </summary>
        /// <returns>オブジェクトを表す文字列</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<<<");
            stringBuilder.AppendLine($" Message:{this.Message}");
            stringBuilder.AppendLine($" Title:{this.Title}");
            stringBuilder.AppendLine($" LinkUrl:{this.LinkUrl}");
            stringBuilder.AppendLine($" LinkDisplayName:{this.LinkDisplayName}");
            stringBuilder.Append(">>>");
            return stringBuilder.ToString();
        }
    }

    /// <summary>
    /// スタッフ用エラーページ（StaffError.aspx）
    /// </summary>
    public partial class StaffError : StaffPage
    {
        /// <summary>
        /// セッション情報内でのエラー情報の名称
        /// </summary>
        public const string ERROR_INFO_NAME = "StaffErrorInfo";

        /// <summary>
        /// Webページが読み込まれた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // セッション情報からエラー情報を取得する
            ErrorInfo errInfo = Session[StaffError.ERROR_INFO_NAME] as ErrorInfo;
            if (errInfo is ErrorInfo)
            {
                // 設定されたエラー情報から画面を構成する
                if (true != String.IsNullOrEmpty(errInfo.Title))
                {
                    this.Title = errInfo.Title;
                }
                if (true != String.IsNullOrEmpty(errInfo.Message))
                {
                    this.LblErrorMessage.Text = errInfo.Message;
                }
                if (true != String.IsNullOrEmpty(errInfo.LinkUrl))
                {
                    this.LnkToNextPage.NavigateUrl = errInfo.LinkUrl;
                }
                if (true != String.IsNullOrEmpty(errInfo.LinkDisplayName))
                {
                    this.LnkToNextPage.Text = errInfo.LinkDisplayName;
                }
            }

            // エラー情報をセッション情報から削除する
            this.Session.Remove(StaffError.ERROR_INFO_NAME);

        }
    }
}