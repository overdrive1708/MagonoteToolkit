using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

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

        //--------------------------------------------------
        // 定数(コンフィギュレーション)
        //--------------------------------------------------
        /// <summary>
        /// デシリアライズ設定(コメント無視)
        /// </summary>
        private static readonly JsonSerializerOptions _deserializeOptions = new() { ReadCommentHandling = JsonCommentHandling.Skip };

        /// <summary>
        /// シリアライズ設定(インデントあり/日本語ありのためエンコーダ設定/高速化のためUTF-8 バイトの配列にシリアル化)
        /// </summary>
        private static readonly JsonSerializerOptions _serializeOptions = new() { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

        //--------------------------------------------------
        // 内部変数
        //--------------------------------------------------
        /// <summary>
        /// 設定
        /// </summary>
        private static List<Setting> _settings = [];

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 設定ファイル読み込み処理
        /// </summary>
        public static void ReadSettings()
        {
            string filename = ApplicationSettings.ReadSettingsExcelFileNumberToNameSettingsFilePath();

            // 設定ファイルがない場合は新規作成する
            if (!File.Exists(filename))
            {
                WriteSettings();
            }

            // 設定ファイルの読み込み
            string jsonString = File.ReadAllText(filename);

            // デシリアライズ
            _settings = JsonSerializer.Deserialize<List<Setting>>(jsonString, _deserializeOptions);
        }

        /// <summary>
        /// 設定ファイル書き込み処理
        /// </summary>
        private static void WriteSettings()
        {
            string filename = ApplicationSettings.ReadSettingsExcelFileNumberToNameSettingsFilePath();

            // シリアライズ
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(_settings, _serializeOptions);

            // ファイル出力
            using FileStream fs = new(filename, FileMode.Create);
            fs.Write(jsonUtf8Bytes);
            fs.Close();
        }

        /// <summary>
        /// プリセットリスト取得処理
        /// </summary>
        /// <returns>プリセットリスト</returns>
        public static List<string> GetPresetList()
        {
            List<string> presetList = [];

            // 読み込み済み設定からプリセット名を取得してプリセットリストを生成する
            foreach (Setting item in _settings)
            {
                presetList.Add(item.PresetName);
            }

            return presetList;
        }

        /// <summary>
        /// プリセット取得処理
        /// </summary>
        /// <param name="presetName">プリセット名</param>
        /// <returns>プリセット</returns>
        public static Setting GetPreset(string presetName)
        {
            Setting setting = new();

            // 読み込み済み設定からプリセット名が一致するものを返す
            foreach (Setting item in _settings)
            {
                if (item.PresetName == presetName)
                {
                    setting = item;
                }
            }

            return setting;
        }
    }
}
