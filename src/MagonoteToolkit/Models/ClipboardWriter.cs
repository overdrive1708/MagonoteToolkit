using System.Text.RegularExpressions;

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

        /// <summary>
        /// 文字列の改行コードを指定の改行コードに置換してクリップボードに書き込む
        /// </summary>
        /// <param name="text">書き込み文字列</param>
        /// <param name="replaceNewlineCharacter">置換する改行文字</param>
        public static void WriteTextReplaceNewline(string text, string replaceNewlineCharacter)
        {
            // 書き込み文字列が空の場合は無処理
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            // 改行コードを置換
            string pattern = @"\r\n|\r|\n";
            string replacedText = Regex.Replace(text, pattern, replaceNewlineCharacter);

            // クリップボードに書き込む
            System.Windows.Clipboard.SetText(replacedText);
        }

        public static void WriteTextRemoveLeadingTrailingDoubleQuotation(string text)
        {
            // 書き込み文字列が空の場合は無処理
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            // 先頭と末尾のダブルクォーテーションを削除
            text = text.Trim('"');

            // クリップボードに書き込む
            System.Windows.Clipboard.SetText(text);
        }
    }
}
