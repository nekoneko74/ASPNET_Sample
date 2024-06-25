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
                                                        "HTTP_ACCEPT_ENCODING", "HTTP_ACCEPT_LANGUAGE", "HTTP_COOKIE", "HTTP_CONTENT_LENGTH", "HTTP_CONTENT_TYPE", "HTTP_ORIGIN", "HTTP_HOST", "HTTP_PRIORITY", "HTTP_REFERER", "HTTP_TE",
                                                        "HTTP_USER_AGENT", "HTTP_UPGRADE_INSECURE_REQUESTS", "HTTP_SEC_FETCH_DEST", "HTTP_SEC_FETCH_MODE", "HTTP_SEC_FETCH_SITE", "HTTP_SEC_FETCH_USER",
                                                        "__VIEWSTATE", "__VIEWSTATEENCRYPTED", "__VIEWSTATEGENERATOR", "__EVENTVALIDATION", "__EVENTTARGET", "__EVENTARGUMENT" };

        /// <summary>
        /// ユーティリティ：列挙型の元になっている値を文字列化して取得する
        /// </summary>
        /// <param name="instance">処理対象の列挙型</param>
        /// <returns>列挙型の元になっている値を文字列化したもの</returns>
        /// <exception cref="Exception">列挙型の値を文字列化することができなかった</exception>
        public static string FormatEnumValue(object enumValue)
        {
            try
            {
                // 渡された値の型が列挙型であることを確認する
                Type type = enumValue.GetType();
                if (true != type.IsEnum)
                {
                    throw new Exception("値が列挙型ではありません。");
                }

                string retStr = null;

                // 列挙型の元になっている整数型の値を文字列に変換する
                Type enumType = Enum.GetUnderlyingType(type);
                switch (enumType.Name)
                {
                    case nameof(UInt64):
                        retStr = ((ulong)enumValue).ToString();
                        break;
                    case nameof(Int64):
                        retStr = ((long)enumValue).ToString();
                        break;
                    case nameof(UInt32):
                        retStr = ((uint)enumValue).ToString();
                        break;
                    case nameof(Int32):
                        retStr = ((int)enumValue).ToString();
                        break;
                    case nameof(UInt16):
                        retStr = ((ushort)enumValue).ToString();
                        break;
                    case nameof(Int16):
                        retStr = ((short)enumValue).ToString();
                        break;
                    case nameof(Byte):
                        retStr = ((byte)enumValue).ToString();
                        break;
                    case nameof(SByte):
                        retStr = ((sbyte)enumValue).ToString();
                        break;
                    default:
                        break;
                }

                if (true == String.IsNullOrEmpty(retStr))
                {
                    throw new Exception("列挙型の元になっている整数型が不明です。");
                }

                return retStr;
            }
            catch (Exception ex)
            {
                throw new Exception("列挙型の値を文字列化することができませんでした", ex);
            }
        }

        /// <summary>
        /// ユーティリティ：オブジェクトの保持している値を文字列化して取得する
        /// </summary>
        /// <param name="instance">処理対象のオブジェクト</param>
        /// <param name="depth">再帰処理の深さ</param>
        /// <returns>オブジェクトの保持している値を文字列化したもの</returns>
        public static string FormatValue(object instance, int depth = 0)
        {
            string retStr = "null";

            // 処理対象のオブジェクトが何らかの形で存在している（nullではない）
            if (null != instance)
            {
                // 処理対象のオブジェクトの型情報を取得する
                Type type = instance.GetType();

                // 基本型／文字列型である場合
                if (true == type.IsPrimitive || instance is string)
                {
                    retStr = String.Format("({0}) {1}", type.Name, instance);
                }
                // 10進浮動小数点数型である場合
                else if (instance is Decimal)
                {
                    retStr = String.Format("({0}) {1}", type.Name, instance.ToString());
                }
                // 日付時刻型である場合
                else if (instance is DateTime)
                {
                    retStr = String.Format("({0}) {1}", type.Name, instance.ToString());
                }
                // 列挙型である場合
                else if (true == type.IsEnum)
                {
                    string enumValStr = PageWithDebug.FormatEnumValue(instance);
                    retStr = String.Format("(enum {0}) {1} => {2}", type.Name, enumValStr, instance);
                }
                // 配列／コレクションである場合
                else if (true == type.IsArray || instance is IEnumerable)
                {
                    // 配列／コレクションの各要素を文字列化する
                    List<string> arrayMembers = new List<string>();
                    int index = 0;
                    foreach (object inner in (IEnumerable)instance)
                    {
                        arrayMembers.Add(String.Format("{0} : {1}", index, PageWithDebug.FormatValue(inner)));
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
                            if (value is null)
                            {
                                objectMembers.Add(String.Format("{0} : {1}", fieldInfo.Name, PageWithDebug.FormatValue(value, depth + 1)));
                            }
                            else
                            {
                                objectMembers.Add(String.Format("{0} : {1}", fieldInfo.Name, (depth < 3) ? PageWithDebug.FormatValue(value, depth + 1) : value.ToString()));
                            }
                        }
                    }
                    foreach (PropertyInfo propInfo in type.GetProperties())
                    {
                        object value = propInfo.GetValue(instance);
                        if (value is null)
                        {
                            objectMembers.Add(String.Format("{0} : {1}", propInfo.Name, PageWithDebug.FormatValue(value, depth + 1)));
                        }
                        else
                        {
                            objectMembers.Add(String.Format("{0} : {1}", propInfo.Name, (depth < 3) ? PageWithDebug.FormatValue(value, depth + 1) : value.ToString()));
                        }
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
            Debug.WriteLine(">>> {0} {1} ********************************************", methodName, "START");

            // デバッグ出力にプログラムの呼び出し情報を出力する
            Debug.WriteLine(String.Format("\tProgram:{0}", this.AppRelativeVirtualPath));
            Debug.WriteLine(String.Format("\tRemote Host:{0}", Request.Params["REMOTE_HOST"]));
            Debug.WriteLine(String.Format("\tRemote Address:{0}", Request.Params["REMOTE_ADDR"]));
            if (this.PreviousPage is System.Web.UI.Page)
            {
                // フォワード元
                Debug.WriteLine(String.Format("\tTransfer by {0}", this.PreviousPage.AppRelativeVirtualPath));
            }
            else
            {
                // リクエストメソッド／リファラ
                Debug.WriteLine(String.Format("\tMethod:{0}", Request.Params["REQUEST_METHOD"]));
                Debug.WriteLine(String.Format("\tReferer:{0}", Request.Params["HTTP_REFERER"]));
            }

            // クッキーの内容
            Debug.WriteLine(String.Format("\tCookie:{0}", Request.Params["HTTP_COOKIE"]));

            // セッション情報
            if (0 < Session.Keys.Count)
            {
                Debug.WriteLine("\t*** Session ***");
                foreach (string key in Session.Keys)
                {
                    Debug.WriteLine(String.Format("\t{0}:{1}", key, PageWithDebug.FormatValue(Session[key])));
                }
            }

            // リクエストパラメータ
            List<string> requestParams = new List<string>();
            foreach (string key in Request.Params.AllKeys)
            {
                if (-1 == Array.IndexOf(PageWithDebug.excludeParamNames, key))
                {
                    requestParams.Add(String.Format("\t{0}:{1}", key, PageWithDebug.FormatValue(Request.Params[key])));
                }
            }
            if (0 < requestParams.Count)
            {
                Debug.WriteLine("\t*** Parameter ***");
                foreach (string requestParam in requestParams)
                {
                    Debug.WriteLine(requestParam);
                }
            }

            Debug.WriteLine("<<< {0} {1} **********************************************", methodName, "END");
        }

        /// <summary>
        /// ページの処理が完了してメモリ上からアンロードされる際に呼び出される
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            string methodName = String.Format("{0}::{1}", this.GetType().FullName, MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine(">>> {0} {1} ********************************************", methodName, "START");

            // セッション情報
            if (0 < Session.Keys.Count)
            {
                Debug.WriteLine("\t*** Session ***");
                foreach (string key in Session.Keys)
                {
                    Debug.WriteLine(String.Format("\t{0}:{1}", key, PageWithDebug.FormatValue(Session[key])));
                }
            }

            Debug.WriteLine("<<< {0} {1} **********************************************", methodName, "END");
        }
    }
}