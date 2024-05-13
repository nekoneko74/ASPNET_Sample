using ASPNET_Sample.Properties;
using System.Data.SqlClient;

namespace ASPNET_Sample
{
    /// <summary>
    /// データベース接続を管理するクラス
    /// </summary>
    internal class ConnectionManager
    {
        /// <summary>
        /// データベース接続オブジェクトを取得する
        /// </summary>
        /// <returns>データベース接続オブジェクト</returns>
        public static SqlConnection GetConnection()
        {
            return ConnectionManager.GetConnection(ConnectionManager.GetConnectionString());
        }

        /// <summary>
        /// データベース接続オブジェクトを取得する
        /// </summary>
        /// <param name="connStr">データベース接続文字列</param>
        /// <returns>データベース接続オブジェクト</returns>
        public static SqlConnection GetConnection(string connStr)
        {
            return new SqlConnection(connStr);
        }

        /// <summary>
        /// データベース接続文字列を取得する
        /// </summary>
        /// <returns>データベース接続文字列</returns>
        public static string GetConnectionString()
        {
            SqlConnectionStringBuilder connStrBuilder = new SqlConnectionStringBuilder();
            connStrBuilder.DataSource = Settings.Default.DbHost;
            connStrBuilder.InitialCatalog = Settings.Default.DbInitDatabase;
            connStrBuilder.UserID = Settings.Default.DbLoginUser;
            connStrBuilder.Password = Settings.Default.DbPassword;
            return connStrBuilder.ConnectionString;
        }
    }
}