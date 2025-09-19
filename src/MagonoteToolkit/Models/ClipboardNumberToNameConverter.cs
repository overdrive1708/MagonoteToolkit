namespace MagonoteToolkit.Models
{
    /// <summary>
    /// クリップボードID->名称変換クラス
    /// </summary>
    public class ClipboardNumberToNameConverter
    {
        /// <summary>
        /// 調査結果クラス
        /// </summary>
        public class ExaminationResult
        {
            /// <summary>
            /// 数値
            /// </summary>
            public string Number { get; set; } = string.Empty;

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; } = string.Empty;
        }
    }
}
