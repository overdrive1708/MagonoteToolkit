using CommunityToolkit.Mvvm.Messaging;

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

        /// <summary>
        /// 進捗情報変更メッセージ送信処理(クリア)
        /// </summary>
        public static void SendProgressInfoChangeMessageClear()
        {
            ProgressInfoChangeMessage message = new()
            {
                ProgressMaximum = 1,
                ProgressValue = 0,
                IsProgressIndeterminate = false,
                ProgressMessage = string.Empty
            };
            _ = WeakReferenceMessenger.Default.Send(message);
        }
    }
}
