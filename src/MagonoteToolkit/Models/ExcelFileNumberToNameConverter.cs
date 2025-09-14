namespace MagonoteToolkit.Models
{
    /// <summary>
    /// ExcelファイルID->名称変換クラス
    /// </summary>
    public class ExcelFileNumberToNameConverter
    {
        /// <summary>
        /// 調査結果クラス
        /// </summary>
        public class ExaminationResult
        {
            /// <summary>
            /// ファイル
            /// </summary>
            public string File { get; set; } = string.Empty;

            /// <summary>
            /// シート
            /// </summary>
            public string Sheet { get; set; } = string.Empty;

            /// <summary>
            /// セル
            /// </summary>
            public string Cell { get; set; } = string.Empty;

            /// <summary>
            /// メモ
            /// </summary>
            public string Memo { get; set; } = string.Empty;

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
