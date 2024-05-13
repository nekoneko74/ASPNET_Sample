using System;

namespace ASPNET_Sample
{
    /// <summary>
    /// Webページの処理モード
    /// </summary>
    internal class PageMode
    {
        /// <summary>
        /// 処理モードを指定するhttpパラメータ名
        /// </summary>
        public const string PARAM_NAME = "mode";

        /// <summary>
        /// 処理モード：新規登録
        /// </summary>
        public const string ADD = "ADD";

        /// <summary>
        /// 処理モード：削除／再登録
        /// </summary>
        public const string DELETE = "DELETE";

        /// <summary>
        /// 処理モード：編集
        /// </summary>
        public const string EDIT = "EDIT";

        /// <summary>
        /// 処理モード：閲覧（デフォルト）
        /// </summary>
        public const string VIEW = "VIEW";

        /// <summary>
        /// 処理モード（ADD／EDIT／DELETE／VIEW）
        /// </summary>
        protected string modeValue;

        /// <summary>
        /// プロパティ：処理モード
        /// </summary>
        public string Value
        {
            get
            {
                return this.modeValue;
            }
            set
            {
                // フィールド「modeValue」に格納される文字列を正規化する
                if (true != String.IsNullOrEmpty(value))
                {
                    switch (value.ToUpper())
                    {
                        case DELETE:
                        case EDIT:
                        case ADD:
                            this.modeValue = value.ToString();
                            break;
                        case VIEW:
                        default:
                            this.modeValue = PageMode.VIEW;
                            break;
                    }
                }
                else
                {
                    this.modeValue = PageMode.VIEW;
                }
            }
        }

        /// <summary>
        /// Webページの処理モードを表すHTTPパラメータを生成する
        /// </summary>
        /// <returns>生成されたHTTPパラメータ（mode=ADDなどの形式）</returns>
        public string CreateHttpParameter()
        {
            return String.Format("{0}={1}", PageMode.PARAM_NAME, this.Value);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PageMode()
        {
            this.modeValue = PageMode.VIEW;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pageMode">Webページの処理モードを表す文字列</param>
        public PageMode(string pageMode) : this()
        {
            this.Value = pageMode;
        }
    }
}