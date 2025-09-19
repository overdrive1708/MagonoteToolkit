using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

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

        /// <summary>
        /// ルールクラス
        /// </summary>
        public class Rule
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

        //--------------------------------------------------
        // 内部変数
        //--------------------------------------------------
        /// <summary>
        /// 変換ルール
        /// </summary>
        private static List<Rule> _rules = [];

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 調査結果取得処理
        /// </summary>
        /// <returns>調査結果</returns>
        public static List<ExaminationResult> GetExaminationResult()
        {
            List<ExaminationResult> results = [];
            ExaminationResult result;
            string clipboardStrings;

            // 変換ルール読み込み
            ReadConvertRules();

            // クリップボード文字列を取得する
            if (Clipboard.ContainsText())
            {
                clipboardStrings = Clipboard.GetText();
            }
            else
            {
                clipboardStrings = string.Empty;
            }

            // クリップボード文字列から数値を抽出する
            MatchCollection regexMatchResults = Regex.Matches(clipboardStrings, @"[0-9]+");

            // 数値を名称に変換する
            foreach (Match regexMatchResult in regexMatchResults.Cast<Match>())
            {
                result = new()
                {
                    Number = regexMatchResult.Value,
                    Name = GetName(regexMatchResult.Value)
                };
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// 名称取得処理
        /// </summary>
        /// <param name="number">数値</param>
        /// <returns>名称</returns>
        private static string GetName(string number)
        {
            string name = string.Empty;     // 見つからない場合は空

            // 変換ルールを検索して数値を名称に変換する
            foreach (var rule in from Rule rule in _rules
                                 where rule.Number == number
                                 select rule)
            {
                name = rule.Name;
            }

            return name;
        }

        /// <summary>
        /// 変換ルール読み込み処理
        /// </summary>
        public static void ReadConvertRules()
        {
            string ruleFilename = ApplicationSettings.ReadSettingsClipboardNumberToNameConvertRulesFilePath();
            // 変換ルールファイルがない場合は処理を抜ける
            if (!File.Exists(ruleFilename))
            {
                return;
            }

            // 変換ルールファイルの読み込み
            CsvConfiguration options = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            using StreamReader srCsv = new(ruleFilename, System.Text.Encoding.UTF8);
            using CsvHelper.CsvReader csv = new(srCsv, options);
            _rules = csv.GetRecords<Rule>().ToList();

            srCsv.Close();
        }
    }
}
