using ASPNET_Sample.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace ASPNET_Sample.Staff
{
    /// <summary>
    /// スタッフ一覧ページ（StaffList.aspx）
    /// </summary>
    public partial class StaffList3 : StaffPage
    {
        /// <summary>
        ///「新規登録」ボタンがクリックされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAddNew_Click(object sender, EventArgs e)
        {
            // スタッフ登録を呼び出す
            List<string> paramValues = new List<string>();
            paramValues.Add(String.Join("=", new string[] { PageMode.PARAM_NAME, PageMode.ADD }));
            string url = "./StaffDetail.aspx?" + String.Join("&", paramValues);
            this.Response.Redirect(url);
        }

        /// <summary>
        ///「クリア」ボタンがクリックされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            // 検索条件をクリアする
            this.TxtAccount.Text = string.Empty;
            this.TxtDisplayName.Text = string.Empty;
            this.DrLstStaffType.SelectedValue = "-1";
        }

        /// <summary>
        ///「検索」ボタンがクリックされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            // 現在入力されている検索条件での検索処理を行う
            this.Search();
        }

        /// <summary>
        /// スタッフ一覧グリッドビューでページング処理が行われた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdvStaffList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 検索処理を行い、新しいページを表示する
            this.Search(e.NewPageIndex);
        }

        /// <summary>
        /// スタッフ一覧グリッドビューの行にデータがバインドされる際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdvStaffList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // 各行の「スタッフ種別」列の表示内容をカスタマイズする
            if (DataControlRowType.DataRow == e.Row.RowType)
            {
                StaffType staffType;
                if (true == Enum.TryParse<StaffType>(((DataRowView)e.Row.DataItem).Row.ItemArray[4].ToString(), out staffType))
                {
                    e.Row.Cells[3].Text = (StaffType.ADMIN == staffType) ? "システム管理者" : "一般スタッフ";
                }
            }
        }

        /// <summary>
        /// スタッフ一覧グリッドビューの「削除」リンクがクリックされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdvStaffList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // 選択された行に該当するレコードの「スタッフID」をパラメータにしてスタッフ削除ページを呼び出す
            string staffId = e.Keys[0].ToString();
            List<string> paramValues = new List<string>();
            paramValues.Add(String.Join("=", new string[] { PageMode.PARAM_NAME, PageMode.DELETE }));
            paramValues.Add(String.Join("=", new string[] { StaffDetail.PARAM_NAME_ID, staffId }));
            string url = "./StaffDetail.aspx?" + String.Join("&", paramValues);
            this.Response.Redirect(url);
        }

        /// <summary>
        /// スタッフ一覧グリッドビューの「編集」リンクがクリックされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdvStaffList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // 選択された行に該当するレコードの「スタッフID」をパラメータにしてスタッフ詳細ページを「編集」モードで呼び出す
            string staffId = GrdvStaffList.DataKeys[e.NewEditIndex].Values[0].ToString();
            List<string> paramValues = new List<string>();
            paramValues.Add(String.Join("=", new string[] { PageMode.PARAM_NAME, PageMode.EDIT }));
            paramValues.Add(String.Join("=", new string[] { StaffDetail.PARAM_NAME_ID, staffId }));
            string url = "./StaffDetail.aspx?" + String.Join("&", paramValues);
            this.Response.Redirect(url);
        }

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
            LblWelcome.Text = "ようこそ" + this.LoginStaff.DisplayName + "さん";

            if (true != IsPostBack)
            {
                //「スタッフ種別」のドロップダウンリストボックスに選択肢を設定する
                this.DrLstStaffType.Items.Add(new ListItem("選択してください", "-1"));
                this.DrLstStaffType.Items.Add(new ListItem("一般スタッフ", StaffType.MEMBER.ToString("d")));
                this.DrLstStaffType.Items.Add(new ListItem("システム管理者", StaffType.ADMIN.ToString("d")));

                //（初期表示）検索処理を行う
                this.Search();
            }
        }

        /// <summary>
        /// スタッフ一覧の検索を行う
        /// </summary>
        /// <param name="pageIndex">スタッフ一覧グリッドビューにバインド（表示）するページの番号</param>
        protected void Search(int pageIndex = 0)
        {
            // 入力されている検索条件を取得する
            string account = (true != String.IsNullOrEmpty(this.TxtAccount.Text)) ? this.TxtAccount.Text : null;
            string displayName = (true != String.IsNullOrEmpty(this.TxtDisplayName.Text)) ? this.TxtDisplayName.Text : null;
            StaffType? staffType = null;
            if (true == Enum.TryParse<StaffType>(this.DrLstStaffType.SelectedValue, out StaffType staffTypeConv))
            {
                if (Enum.IsDefined(typeof(StaffType), staffTypeConv))
                {
                    staffType = staffTypeConv;
                }
            }

            // 検索用のSELECTクエリを生成する
            StringBuilder selectQuery = new StringBuilder("SELECT [StaffId], [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] FROM [Staff]");
            List<string> whereConds = new List<string>();
            if (true != String.IsNullOrEmpty(account))
            {
                whereConds.Add("[Account] LIKE @Account");
            }
            if (null != staffType)
            {
                whereConds.Add("[StaffType] = @StaffType");
            }
            if (true != String.IsNullOrEmpty(displayName))
            {
                whereConds.Add("[DisplayName] LIKE @DisplayName");
            }
            whereConds.Add("[DeleteDate] IS NULL");
            if (0 < whereConds.Count)
            {
                selectQuery.Append(" WHERE " + String.Join(" AND ", whereConds));
            }
            selectQuery.Append(" ORDER BY [Account] ASC");
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery.ToString(), ConnectionManager.GetConnectionString());

            // SQLパラメータオブジェクトを生成してパラメータに値を設定する
            if (true != String.IsNullOrEmpty(account))
            {
                dataAdapter.SelectCommand.Parameters.Add("@Account", SqlDbType.NVarChar).Value = String.Format("%{0}%", account);
            }
            if (null != staffType)
            {
                dataAdapter.SelectCommand.Parameters.Add("@StaffType", SqlDbType.TinyInt).Value = (byte)staffType;
            }
            if (true != String.IsNullOrEmpty(displayName))
            {
                dataAdapter.SelectCommand.Parameters.Add("@DisplayName", SqlDbType.NVarChar).Value = String.Format("%{0}%", displayName);
            }

            // スタッフの一覧を取得してグリッドビューにデータバインドする
            DataTable staffList = new DataTable();
            dataAdapter.Fill(staffList);
            this.GrdvStaffList.DataSource = staffList;
            this.GrdvStaffList.PageIndex = pageIndex;
            this.GrdvStaffList.DataKeyNames = new string[] { "StaffId" };
            this.GrdvStaffList.DataBind();
        }
    }
}