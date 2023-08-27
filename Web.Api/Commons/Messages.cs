namespace WebApi.Commons
{
    public class Messages
    {

        public const string LOGIN_FAILED = "ログインに失敗しました";
        public const string LOGIN_FAIL = "ログインIDまたはパスワードが違います";

        public const string SUBSCRIBE_SUCCESS = "登録が完了しました";
        public const string SUBSCRIBE_FAIL = "登録に失敗しました";

        public const string UPDATE_SUCCESS = "更新が完了しました";
        public const string UPDATE_FAIL = "更新に失敗しました";

        public const string EDIT_SUCCESS = "更新が完了しました";
        public const string EDIT_FAIL = "更新に失敗しました";

        public const string DELETE_SUCCESS = "削除が完了しました";
        public const string DELETE_FAIL = "削除に失敗しました";

        public const string SPACE = "===============================================================================================";
    }

    enum Level
    {
        Low,
        Medium,
        High
    }
}
