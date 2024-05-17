using ASPNET_Sample.Data;
using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace ASPNET_Sample.Staff
{
    /// <summary>
    /// スタッフ一覧ページ（StaffList2.aspx）
    /// </summary>
    public partial class StaffList2 : StaffPage
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
            // NOP：No Operation
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
                    e.Row.Cells[4].Text = (StaffType.ADMIN == staffType) ? "システム管理者" : "一般スタッフ";
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
            string staffId = this.GrdvStaffList.DataKeys[e.NewEditIndex].Values[0].ToString();
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
                List<KeyValuePair<int, string>> StaffTypes = new List<KeyValuePair<int, string>>();
                StaffTypes.Add(new KeyValuePair<int, string>(-1, "選択してください" ));
                StaffTypes.Add(new KeyValuePair<int, string>((int)StaffType.MEMBER, "スタッフ"));
                StaffTypes.Add(new KeyValuePair<int, string>((int)StaffType.ADMIN, "システム管理者"));
                this.DrLstStaffType.DataSource = StaffTypes;
                this.DrLstStaffType.DataTextField = "Value";
                this.DrLstStaffType.DataValueField = "Key";
                this.DrLstStaffType.DataBind();
            }
        }
    }
}