namespace MagonoteToolkit.Models
{
    /// <summary>
    /// クリップボード読み込みクラス
    /// </summary>
    internal class ClipboardReader
    {
        /// <summary>
        /// クリップボードから文字列として読み込む
        /// </summary>
        /// <returns></returns>
        public static string ReadText()
        {
            if (System.Windows.Clipboard.ContainsText())
            {
                return System.Windows.Clipboard.GetText();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
