using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ASPNET_Sample
{
    /// <summary>
    /// Webページの基底クラス（デバッグサポート）
    /// System.Web.UI.Pageクラスにデバッグ用機能のサポートを追加します。
    /// </summary>
    /// <remarks>
    /// Visual Studioのデバッグウィンドウに以下の情報を表示します。
    /// 【処理開始前】
    ///  ・呼び出し元ホスト名とIPアドレス
    ///  ・呼び出し元 ⇒ リクエストメソッドとリファラ（またはフォワード元のURL）
    ///  ・クッキーに保持されているデータ
    ///  ・PostBack以外にGET／POSTされたパラメータ
    ///  ・セッション情報データ
    /// 【処理完了後】
    ///  ・セッション情報データ
    /// </remarks>
    public class PageWithDebug : System.Web.UI.Page
    {
        /// <summary>
        /// デバッグ出力への出力を行わないリクエストパラメータ名を列挙する
        /// </summary>
        protected static string[] excludeParamNames = { "ASP.NET_SessionId", "ALL_HTTP", "HTTP_ACCEPT",
                                                        "HTTP_UPGRADE_INSECURE_REQUESTS", "HTTP_SEC_FETCH_DEST", "HTTP_SEC_FETCH_MODE", "HTTP_SEC_FETCH_SITE", "HTTP_SEC_FETCH_USER", "ALL_RAW", "APPL_MD_PATH", "APPL_PHYSICAL_PATH",
                                                        "AUTH_TYPE", "AUTH_USER", "AUTH_PASSWORD", "LOGON_USER", "REMOTE_USER", "CERT_COOKIE", "CERT_FLAGS", "CERT_ISSUER", "CERT_KEYSIZE", "CERT_SECRETKEYSIZE", "CERT_SERIALNUMBER",
                                                        "CERT_SERVER_ISSUER", "CERT_SERVER_SUBJECT", "CERT_SUBJECT", "CONTENT_LENGTH", "CONTENT_TYPE", "GATEWAY_INTERFACE", "HTTPS", "HTTPS_KEYSIZE", "HTTPS_SECRETKEYSIZE",
                                                        "HTTPS_SERVER_ISSUER", "HTTPS_SERVER_SUBJECT", "INSTANCE_ID", "INSTANCE_META_PATH", "LOCAL_ADDR", "PATH_INFO", "PATH_TRANSLATED", "QUERY_STRING", "REMOTE_ADDR", "REMOTE_HOST",
                                                        "REMOTE_PORT", "REQUEST_METHOD", "SCRIPT_NAME", "SERVER_NAME", "SERVER_PORT", "SERVER_PORT_SECURE", "SERVER_PROTOCOL", "SERVER_SOFTWARE", "URL", "HTTP_CONNECTION", "HTTP_ACCEPT",
                                                        "HTTP_ACCEPT_ENCODING", "HTTP_ACCEPT_LANGUAGE", "HTTP_COOKIE", "HTTP_CONTENT_LENGTH", "HTTP_CONTENT_TYPE", "HTTP_ORIGIN", "HTTP_HOST", "HTTP_REFERER", "HTTP_TE", "HTTP_USER_AGENT",
                                                        "HTTP_UPGRADE_INSECURE_REQUESTS", "HTTP_SEC_FETCH_DEST", "HTTP_SEC_FETCH_MODE", "HTTP_SEC_FETCH_SITE", "HTTP_SEC_FETCH_USER",
                                                        "__VIEWSTATE", "__VIEWSTATEGENERATOR", "__EVENTVALIDATION", "__EVENTTARGET", "__EVENTARGUMENT" };

        /// <summary>
        /// ユーティリティ：オブジェクトの保持している値を文字列化して取得する
        /// </summary>
        /// <param name="instance">処理対象のオブジェクト</param>
        /// <returns>オブジェクトの保持している値を文字列化したもの</returns>
        public static string GetValueString(object instance)
        {
            string retStr = "null";

            // 処理対象のオブジェクトが何らかの形で存在している（nullではない）
            if (null != instance)
            {
                // 処理対象のオブジェクトの型情報を取得する
                Type type = instance.GetType();

                // 基本型／列挙型／文字列型である場合
                if (true == type.IsPrimitive || true == type.IsEnum || instance is string)
                {
                    retStr = String.Format("({0}) {1}", type.Name, instance);
                }
                // 配列／コレクションである場合
                else if (true == type.IsArray || instance is IEnumerable)
                {
                    // 配列／コレクションの各要素を文字列化する
                    List<string> arrayMembers = new List<string>();
                    int index = 0;
                    foreach (object inner in (IEnumerable)instance)
                    {
                        arrayMembers.Add(String.Format("{0} : {1}", index, PageWithDebug.GetValueString(inner)));
                        index++;
                    }
                    retStr = String.Format("({0}) [ {1} ]", type.Name, String.Join(", ", arrayMembers));
                }
                // オブジェクトである場合
                else if (instance is object)
                {
                    // オブジェクトのメンバを文字列化する
                    List<string> objectMembers = new List<string>();
                    foreach (FieldInfo fieldInfo in type.GetRuntimeFields())
                    {
                        // 自動実装プロパティ用の隠しメンバは除外する
                        Regex regExp = new Regex("\\A<.+>k__BackingField\\z");
                        if (true == regExp.IsMatch(fieldInfo.Name))
                        {
                            continue;
                        }
                        else
                        {
                            object value = fieldInfo.GetValue(instance);
                            objectMembers.Add(String.Format("{0} : {1}", fieldInfo.Name, PageWithDebug.GetValueString(value)));
                        }
                    }
                    foreach (PropertyInfo propInfo in type.GetProperties())
                    {
                        object value = propInfo.GetValue(instance);
                        objectMembers.Add(String.Format("{0} : {1}", propInfo.Name, PageWithDebug.GetValueString(value)));
                    }
                    retStr = String.Format("({0}) {{ {1} }}", type.Name, String.Join(", ", objectMembers));
                }
                else
                {
                    retStr = String.Format("({0}) {1}", type.Name, instance);
                }
            }

            // オブジェクトの保持している値を文字列化したものを返す
            return retStr;
        }

        /// <summary>
        /// ロードイベント前のポストバックデータが全て読み込まれた段階で呼び出される
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);

            string methodName = String.Format("{0}::{1}", this.GetType().FullName, MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine(">>> {0} {1} ********************************************", "START", methodName);

            // デバッグ出力にプログラムの呼び出し情報を出力する
            Debug.WriteLine(String.Format("Program:{0}", this.AppRelativeVirtualPath));
            Debug.WriteLine(String.Format("Remote Host:{0}", Request.Params["REMOTE_HOST"]));
            Debug.WriteLine(String.Format("Remote Address:{0}", Request.Params["REMOTE_ADDR"]));
            if (this.PreviousPage is System.Web.UI.Page)
            {
                // フォワード元
                Debug.WriteLine(String.Format("Transfer by {0}", this.PreviousPage.AppRelativeVirtualPath));
            }
            else
            {
                // リクエストメソッド／リファラ
                Debug.WriteLine(String.Format("Method:{0}", Request.Params["REQUEST_METHOD"]));
                Debug.WriteLine(String.Format("Referer:{0}", Request.Params["HTTP_REFERER"]));
            }

            // クッキーの内容
            Debug.WriteLine(String.Format("Cookie:{0}", Request.Params["HTTP_COOKIE"]));

            // セッション情報
            Debug.WriteLine("*** Session ***");
            foreach (string key in Session.Keys)
            {
                Debug.WriteLine(String.Format("{0}:{1}", key, PageWithDebug.GetValueString(Session[key])));
            }

            // リクエストパラメータ
            Debug.WriteLine("*** Parameter ***");
            foreach (string key in Request.Params.AllKeys)
            {
                if (-1 == Array.IndexOf(PageWithDebug.excludeParamNames, key))
                {
                    Debug.WriteLine(String.Format("{0}:{1}", key, PageWithDebug.GetValueString(Request.Params[key])));
                }
            }

            Debug.WriteLine("<<< {0} {1} **********************************************", "END", methodName);
        }

        /// <summary>
        /// ページの処理が完了してメモリ上からアンロードされる際に呼び出される
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            string methodName = String.Format("{0}::{1}", this.GetType().FullName, MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine(">>> {0} {1} ********************************************", "START", methodName);

            // セッション情報
            Debug.WriteLine("*** Session ***");
            foreach (string key in Session.Keys)
            {
                Debug.WriteLine(String.Format("{0}:{1}", key, PageWithDebug.GetValueString(Session[key])));
            }

            Debug.WriteLine("<<< {0} {1} **********************************************", "END", methodName);
        }
    }
}