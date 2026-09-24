namespace MagonoteToolkit.Models
{
    /// <summary>
    /// メッセージクラス
    /// </summary>
    internal class Messages
    {
        /// <summary>
        /// 進捗情報変更通知メッセージ
        /// </summary>
        public class ProgressInfoChangeMessage
        {
            /// <summary>
            /// 最大値
            /// </summary>
            public int ProgressMaximum { get; set; }

            /// <summary>
            /// 現在値
            /// </summary>
            public int ProgressValue { get; set; }

            /// <summary>
            /// 進捗不定フラグ
            /// </summary>
            public bool IsProgressIndeterminate { get; set; }

            /// <summary>
            /// 進捗メッセージ
            /// </summary>
            public string ProgressMessage { get; set; }
        }
    }
}
