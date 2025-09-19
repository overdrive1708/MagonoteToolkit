using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// アプリケーション設定クラス
    /// </summary>
    internal class ApplicationSettings
    {
        //--------------------------------------------------
        // 公開変数
        //--------------------------------------------------
        /// <summary>
        /// Excelファイル検査:設定ファイルパス
        /// </summary>
        public string ExcelFileInspectionSettingsFilePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExcelFileInspectionSettings.json");

        /// <summary>
        /// ExcelファイルID->名称変換:設定ファイルパス
        /// </summary>
        public string ExcelFileNumberToNameSettingsFilePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExcelFileNumberToNameSettings.json");

        /// <summary>
        /// ExcelファイルID->名称変換:変換ルールファイルパス
        /// </summary>
        public string ExcelFileNumberToNameConvertRulesFilePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExcelFileNumberToNameConvertRules.csv");

        /// <summary>
        /// ファイル変更監視:ワークスペースディレクトリ
        /// </summary>
        public string FileChangeMonitorWorkspaceDirectory { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileChangeMonitorWorkspace");

        /// <summary>
        /// クリップボードID->名称変換:変換ルールファイルパス
        /// </summary>
        public string ClipboardNumberToNameConvertRulesFilePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ClipboardNumberToNameConvertRules.csv");

        //--------------------------------------------------
        // 定数(コンフィギュレーション)
        //--------------------------------------------------
        /// <summary>
        /// 設定ファイル名
        /// </summary>
        private static readonly string _fileName = "ApplicationSettings.json";

        /// <summary>
        /// デシリアライズ設定(コメント無視)
        /// </summary>
        private static readonly JsonSerializerOptions _deserializeOptions = new() { ReadCommentHandling = JsonCommentHandling.Skip };

        /// <summary>
        /// シリアライズ設定(インデントあり/日本語ありのためエンコーダ設定/高速化のためUTF-8 バイトの配列にシリアル化)
        /// </summary>
        private static readonly JsonSerializerOptions _serializeOptions = new() { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 設定読み込み処理
        /// </summary>
        /// <returns>設定読み込み結果</returns>
        public static ApplicationSettings ReadSettings()
        {
            string settingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);

            // 設定ファイルがない場合は新規作成する
            if (!File.Exists(settingFilePath))
            {
                ApplicationSettings writeSettings = new();
                WriteSettings(writeSettings);
            }

            // 設定ファイルの読み込み
            string jsonString = File.ReadAllText(_fileName);

            // デシリアライズ
            ApplicationSettings readSettings = JsonSerializer.Deserialize<ApplicationSettings>(jsonString, _deserializeOptions);

            return readSettings;
        }

        /// <summary>
        /// 設定読み込み処理:Excelファイル検査:設定ファイルパス
        /// </summary>
        /// <returns>Excelファイル検査:設定ファイルパス</returns>
        public static string ReadSettingsExcelFileInspectionSettingsFilePath()
        {
            return ReadSettings().ExcelFileInspectionSettingsFilePath;
        }

        /// <summary>
        /// 設定読み込み処理:ExcelファイルID->名称変換:設定ファイルパス
        /// </summary>
        /// <returns>ExcelファイルID->名称変換:設定ファイルパス</returns>
        public static string ReadSettingsExcelFileNumberToNameSettingsFilePath()
        {
            return ReadSettings().ExcelFileNumberToNameSettingsFilePath;
        }

        /// <summary>
        /// 設定読み込み処理:ExcelファイルID->名称変換:変換ルールファイルパス
        /// </summary>
        /// <returns>ExcelファイルID->名称変換:変換ルールファイルパス</returns>
        public static string ReadSettingsExcelFileNumberToNameConvertRulesFilePath()
        {
            return ReadSettings().ExcelFileNumberToNameConvertRulesFilePath;
        }

        /// <summary>
        /// 設定読み込み処理:ファイル変更監視:ワークスペースディレクトリ
        /// </summary>
        /// <returns></returns>
        public static string ReadSettingsFileChangeMonitorWorkspaceDirectory()
        {
            return ReadSettings().FileChangeMonitorWorkspaceDirectory;
        }

        /// <summary>
        /// 設定読み込み処理:クリップボードID->名称変換:変換ルールファイルパス
        /// </summary>
        /// <returns>クリップボードID->名称変換:変換ルールファイルパス</returns>
        public static string ReadSettingsClipboardNumberToNameConvertRulesFilePath()
        {
            return ReadSettings().ClipboardNumberToNameConvertRulesFilePath;
        }

        /// <summary>
        /// 設定書き込み処理
        /// </summary>
        /// <param name="settings">書き込みする設定</param>
        public static void WriteSettings(ApplicationSettings settings)
        {
            string settingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName);

            // シリアライズ
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(settings, _serializeOptions);

            // ファイル出力
            using FileStream fs = new(settingFilePath, FileMode.Create);
            fs.Write(jsonUtf8Bytes);
            fs.Close();
        }
    }
}
