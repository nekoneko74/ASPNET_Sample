using ASPNET_Sample.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET_Sample.Staff
{
    /// <summary>
    /// スタッフ詳細表示（StaffDetail.aspx）
    /// </summary>
    public partial class StaffDetail : StaffPage
    {
        /// <summary>
        /// 処理対象のスタッフを指定するhttpパラメータ名
        /// </summary>
        public const string PARAM_NAME_ID = "id";

        /// <summary>
        ///「登録／更新／削除」ボタンがクリックされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAction_Click(object sender, EventArgs e)
        {
            try
            {
                // 処理モードが「削除」モードである場合
                if (PageMode.DELETE == this.HidPageMode.Value)
                {
                    this.DeleteStaff();
                }
                // 処理モードが「編集」モードである場合
                else if (PageMode.EDIT == this.HidPageMode.Value)
                {
                    this.UpdateStaff();
                }
                // 処理モードが「新規登録」モードである
                else if (PageMode.ADD == this.HidPageMode.Value)
                {
                    this.InsertStaff();
                }
            }
            catch (Exception ex)
            {
                // 処理が継続できないのでエラー表示後に一覧画面に戻る
                ErrorInfo errInfo = new ErrorInfo();
                errInfo.Message = ex.Message;
                if (ex.InnerException is Exception)
                {
                    errInfo.Message += "<br>" + ex.InnerException.Message;
                }
                errInfo.LinkUrl = "./StaffList.aspx";
                errInfo.LinkDisplayName = "スタッフ一覧に戻る";
                this.TransferErrorPage(errInfo);
            }
        }

        /// <summary>
        ///「キャンセル」ボタンがクリックされた際に呼び出されるイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            // スタッフ一覧ページに戻る
            this.Response.Redirect("./StaffList.aspx");
        }

        /// <summary>
        /// スタッフ情報を削除する（削除日時を設定して論理削除を行う）
        /// </summary>
        /// <exception cref="Exception">スタッフ情報の削除を行うことができなかった</exception>
        protected void DeleteStaff()
        {
            try
            {
                try
                {
                    // ログインスタッフが「システム管理者」でない場合には削除を行わせない
                    if (StaffType.ADMIN != this.LoginStaff.StaffType)
                    {
                        throw new Exception("スタッフ情報の削除には管理者権限が必要です。");
                    }

                    // 処理対象のスタッフ情報に削除日時を設定する
                    int staffId = Convert.ToInt32(this.HidStaffId.Value);
                    if (1 == StaffDao.Delete(staffId, this.LoginStaff.StaffId))
                    {
                        // 更新されたデータを現在の処理モード（＝削除モード）で開き直す
                        List<string> paramValues = new List<string>();
                        paramValues.Add(String.Join("=", new string[] { PageMode.PARAM_NAME, this.HidPageMode.Value }));
                        paramValues.Add(String.Join("=", new string[] { StaffDetail.PARAM_NAME_ID, staffId.ToString() }));
                        string url = "./StaffDetail.aspx?" + String.Join("&", paramValues);
                        this.Response.Redirect(url);
                    }
                    // 処理対象のスタッフ情報が存在していない？
                    else
                    {
                        throw new Exception("処理対象のスタッフ情報が存在していません。");
                    }
                }
                catch (Exception ex) when (ex is FormatException || ex is OverflowException)
                {
                    throw new Exception(String.Format("無効なスタッフID（{0}）が指定されました。", this.HidStaffId.Value), ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("スタッフ情報を更新することができません。", ex);
            }
        }

        /// <summary>
        /// スタッフ情報を挿入する
        /// </summary>
        /// <exception cref="Exception">スタッフ情報の挿入を行うことができなかった</exception>
        protected void InsertStaff()
        {
            try
            {
                // 新しいスタッフ情報を生成して挿入処理を行う
                StaffDto staff = new StaffDto();
                staff.Account = this.TxtAccount.Text;
                staff.Password = this.TxtPassword.Text;
                staff.DisplayName = this.TxtDisplayName.Text;
                staff.StaffType = (StaffType)Enum.Parse(typeof(StaffType), this.RdlStaffType.SelectedValue);
                if (1 == StaffDao.Insert(staff, this.LoginStaff.StaffId))
                {
                    // 挿入されたデータを「編集モード」で開き直す
                    List<string> paramValues = new List<string>();
                    paramValues.Add(String.Join("=", new string[] { PageMode.PARAM_NAME, PageMode.EDIT }));
                    paramValues.Add(String.Join("=", new string[] { StaffDetail.PARAM_NAME_ID, staff.StaffId.ToString() }));
                    string url = "./StaffDetail.aspx?" + String.Join("&", paramValues);
                    this.Response.Redirect(url);
                }
                else
                {
                    throw new Exception("スタッフ情報を挿入することができません。");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("スタッフ情報を登録することができません。", ex);
            }
        }

        /// <summary>
        /// 処理対象のスタッフ情報を読み込んでWebページ上に表示する
        /// </summary>
        /// <param name="staffIdStr">処理対象のスタッフID（文字列）</param>
        /// <exception cref="Exception">処理対象のスタッフ情報を読み込むことができなかった</exception>
        protected void LoadStaff(string staffIdStr)
        {
            try
            {
                try
                {
                    // 処理対象のスタッフ情報を取得する
                    int staffId = Convert.ToInt32(staffIdStr);
                    StaffDto staff = StaffDao.Select(staffId);
                    if (null == staff)
                    {
                        // 指定されたスタッフIDを持つレコードを読み込むことが出来なかった
                        throw new Exception(String.Format("スタッフ情報を読み込むことができませんでした（{0}は無効なスタッフIDです）。", staffId));
                    }

                    // 最終更新者のレコードを取得する
                    int lastUpdStaffId = (int)staff.UpdateStaffId;
                    StaffDto lastUpdStaff = StaffDao.Select(lastUpdStaffId);
                    if (null == lastUpdStaff)
                    {
                        // 最終更新者のレコードが取得できない！
                        throw new Exception(String.Format("最終更新者（id={0}）の情報を読み込むことが出来ませんでした。", lastUpdStaffId));
                    }

                    // 取得できたスタッフのデータをWebフォーム上に設定する
                    this.LblUserId.Text = staff.StaffId.ToString();
                    this.LblDelFlg.Visible = (staff.DeleteDate is null) ? false : true;
                    this.TxtAccount.Text = staff.Account;
                    this.TxtPassword.Text = string.Empty;
                    this.TxtPassConfirm.Text = string.Empty;
                    this.TxtDisplayName.Text = staff.DisplayName;
                    this.RdlStaffType.SelectedValue = staff.StaffType.ToString("d");
                    this.LblLastUpdDate.Text = String.Format("{0:yyyy/MM/dd HH:mm:ss}", staff.UpdateDate);
                    this.LblLastUpdUser.Text = lastUpdStaff.DisplayName;
                    this.HidStaffId.Value = staff.StaffId.ToString();
                }
                catch (Exception ex) when (ex is FormatException || ex is OverflowException)
                {
                    throw new Exception(String.Format("無効なスタッフID（{0}）が指定されました。", staffIdStr), ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("スタッフ情報を読み込むことが出来ませんでした", ex);
            }
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

            if (true != this.IsPostBack)
            {
                try
                {
                    //「スタッフ種別」の選択用ラジオボタンを用意する
                    this.RdlStaffType.Items.Add(new ListItem("一般スタッフ", StaffType.MEMBER.ToString("d")));
                    this.RdlStaffType.Items.Add(new ListItem("システム管理者", StaffType.ADMIN.ToString("d")));

                    // 外部からの呼び出しパラメータ（処理モード、処理対象のスタッフID）を取得する
                    PageMode pageMode = new PageMode(this.Request.Params[PageMode.PARAM_NAME]);
                    this.HidPageMode.Value = pageMode.Value;
                    string staffIdStr = this.Request.Params[StaffDetail.PARAM_NAME_ID];

                    // 削除モードの場合の初期化処理を行う
                    if (PageMode.DELETE == this.HidPageMode.Value)
                    {
                        // 処理対象のスタッフ情報を読み込む
                        this.LoadStaff(staffIdStr);

                        // タイトルと処理ボタンの名前を設定する
                        this.Title = "スタッフ詳細：削除";
                        this.LblTitle.Text = "スタッフ詳細：削除";
                        this.BtnAction.Text = "削除";

                        // 各入力欄を操作不能にする
                        this.TxtPassword.ReadOnly = true;
                        this.TxtPassConfirm.ReadOnly = true;
                        this.TxtDisplayName.ReadOnly = true;
                        this.RdlStaffType.Enabled = false;

                        //「削除」ボタンはログインスタッフが「システム管理者」である場合にのみ有効にする
                        this.BtnAction.Enabled = (StaffType.ADMIN == this.LoginStaff.StaffType);
                    }
                    // 編集モードの場合の初期化処理を行う
                    else if (PageMode.EDIT == this.HidPageMode.Value)
                    {
                        // 処理対象のスタッフ情報を読み込む
                        this.LoadStaff(staffIdStr);

                        // タイトルと処理ボタンの名前を設定する
                        this.Title = "スタッフ詳細：編集";
                        this.LblTitle.Text = "スタッフ詳細：編集";
                        this.BtnAction.Text = "更新";
                    }
                    // 新規登録モードの場合の初期化処理を行う
                    else if (PageMode.ADD == this.HidPageMode.Value)
                    {
                        // タイトルと処理ボタンの名前を設定する
                        this.Title = "スタッフ詳細：新規登録";
                        this.LblTitle.Text = "スタッフ詳細：新規登録";
                        this.BtnAction.Text = "登録";

                        // 新規登録時にのみ行うこと
                        // ・アカウントを入力可能にする
                        // ・パスワードの必須チェックを有効にする
                        // ・スタッフ種別の初期選択状態を「一般スタッフ」にする
                        // ・最終更新日時／最終更新者の表示行を非表示にする
                        this.TxtAccount.ReadOnly = false;
                        this.Validator_TxtPassword_Required.Enabled = true;
                        this.RdlStaffType.SelectedValue = StaffType.MEMBER.ToString("d");
                        this.TblStaffInfo.Rows[6].Visible = false;
                        this.TblStaffInfo.Rows[7].Visible = false;
                    }
                    // 閲覧モードの場合の初期化処理を行う
                    else
                    {
                        // 処理対象のスタッフ情報を読み込む
                        this.LoadStaff(staffIdStr);

                        // タイトルを設定する
                        this.Title = "スタッフ詳細：閲覧";
                        this.LblTitle.Text = "スタッフ詳細：閲覧";

                        // データは操作させない
                        this.TxtPassword.ReadOnly = true;
                        this.TxtPassConfirm.ReadOnly = true;
                        this.TxtDisplayName.ReadOnly = true;
                        this.RdlStaffType.Enabled = false;
                        this.BtnAction.Enabled = false;
                        this.BtnAction.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    // 処理が継続できないのでエラー表示後に一覧画面に戻る
                    ErrorInfo errInfo = new ErrorInfo();
                    errInfo.Message = ex.Message;
                    if (null != ex.InnerException)
                    {
                        errInfo.Message += "<br>" + ex.InnerException.Message;
                    }
                    errInfo.LinkUrl = "./StaffList.aspx";
                    errInfo.LinkDisplayName = "スタッフ一覧に戻る";
                    this.TransferErrorPage(errInfo);
                }
            }
        }

        /// <summary>
        /// スタッフ情報を更新する
        /// </summary>
        /// <exception cref="Exception">スタッフ情報の更新を行うことができなかった</exception>
        protected void UpdateStaff()
        {
            try
            {
                try
                {
                    // 処理対象のスタッフ情報を読み込む
                    int staffId = Convert.ToInt32(this.HidStaffId.Value);
                    StaffDto staff = StaffDao.Select(staffId);
                    if (null == staff)
                    {
                        // 処理対象のスタッフ情報を読み込むことが出来なかった
                        throw new Exception(String.Format("スタッフ情報を読み込むことができませんでした（{0}は無効なスタッフIDです）。", staffId));
                    }

                    // 削除済みのデータを更新しようとした
                    if (null != staff.DeleteDate)
                    {
                        throw new Exception("削除済みのスタッフ情報を更新することはできません。");
                    }

                    // フォーム上で入力された情報を取得してスタッフ情報を更新する
                    if (true != String.IsNullOrEmpty(this.TxtPassword.Text))
                    {
                        // パスワードは入力されたときにのみ更新を行う
                        staff.Password = this.TxtPassword.Text;
                    }
                    staff.DisplayName = this.TxtDisplayName.Text;
                    staff.StaffType = (StaffType)Enum.Parse(typeof(StaffType), this.RdlStaffType.SelectedValue);
                    if (1 == StaffDao.Update(staff, staffId, this.LoginStaff.StaffId))
                    {
                        // 更新されたデータを現在の処理モード（＝編集モード）で開き直す
                        List<string> paramValues = new List<string>();
                        paramValues.Add(String.Join("=", new string[] { PageMode.PARAM_NAME, this.HidPageMode.Value }));
                        paramValues.Add(String.Join("=", new string[] { StaffDetail.PARAM_NAME_ID, staff.StaffId.ToString() }));
                        string url = "./StaffDetail.aspx?" + String.Join("&", paramValues);
                        this.Response.Redirect(url);
                    }
                    // 処理対象のスタッフ情報が存在していない？
                    else
                    {
                        throw new Exception("処理対象のスタッフ情報が存在していません。");
                    }
                }
                catch (Exception ex) when (ex is FormatException || ex is OverflowException)
                {
                    throw new Exception(String.Format("無効なスタッフID（{0}）が指定されました。", this.HidStaffId.Value), ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("スタッフ情報を更新することができません。", ex);
            }
        }
    }
}