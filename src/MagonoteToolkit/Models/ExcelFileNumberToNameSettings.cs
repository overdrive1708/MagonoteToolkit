using System.Collections.Generic;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// ExcelファイルID->名称変換設定クラス
    /// </summary>
    public class ExcelFileNumberToNameSettings
    {
        /// <summary>
        /// 設定クラス
        /// </summary>
        public class Setting
        {
            /// <summary>
            /// プリセット名
            /// </summary>
            public string PresetName { get; set; } = string.Empty;

            /// <summary>
            /// 調査ファイルキーワード
            /// </summary>
            public string ExaminationFileKeyword { get; set; } = string.Empty;

            /// <summary>
            /// 調査対象
            /// </summary>
            public List<ExaminationTarget> ExaminationTargets { get; set; } = [];
        }

        /// <summary>
        /// 調査対象クラス
        /// </summary>
        public class ExaminationTarget
        {
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
        }
    }
}
