namespace MagonoteToolkit.Models
{
    /// <summary>
    /// クリップボード書き込みクラス
    /// </summary>
    internal class ClipboardWriter
    {
        /// <summary>
        /// 文字列の末尾の改行コードを削除してクリップボードに書き込む
        /// </summary>
        /// <param name="text">書き込み文字列</param>
        public static void WriteTextRemoveTrailingNewline(string text)
        {
            // 書き込み文字列が空の場合は無処理
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            // 末尾の改行コードを削除
            text = text.TrimEnd('\r', '\n');

            // クリップボードに書き込む
            System.Windows.Clipboard.SetText(text);
        }
    }
}
