using ClosedXML.Excel;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
        /// <param name="filename">ファイル名</param>
        /// <param name="examinationTargets">調査対象</param>
        /// <returns>調査結果</returns>
        public static List<ExaminationResult> GetExaminationResult(string filename, List<ExcelFileNumberToNameSettings.ExaminationTarget> examinationTargets)
        {
            List<ExaminationResult> results = [];
            ExaminationResult result;

            // 変換ルール読み込み
            ReadConvertRules();

            try
            {
                // Bookを開く(読み取り専用で開く)
                using (FileStream fs = new(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using XLWorkbook workbook = new(fs);

                    // 調査対象で指定されたセルの調査
                    foreach (ExcelFileNumberToNameSettings.ExaminationTarget examinationTarget in examinationTargets)
                    {
                        // ワークシートを開く
                        if (workbook.TryGetWorksheet(examinationTarget.Sheet, out IXLWorksheet worksheet))
                        {
                            if (!examinationTarget.Cell.Contains(':'))
                            {
                                // 単一セルの場合
                                MatchCollection regexMatchResults = Regex.Matches(worksheet.Cell(examinationTarget.Cell).Value.ToString(), @"[0-9]+");
                                foreach (Match regexMatchResult in regexMatchResults.Cast<Match>())
                                {
                                    // 正規表現で数値を抽出して変換結果を結果とする
                                    result = new()
                                    {
                                        File = filename,
                                        Sheet = examinationTarget.Sheet,
                                        Cell = examinationTarget.Cell,
                                        Memo = examinationTarget.Memo,
                                        Number = regexMatchResult.Value,
                                        Name = GetName(regexMatchResult.Value)
                                    };
                                    results.Add(result);
                                }
                            }
                            else
                            {
                                // 複数セルの場合
                                // 範囲指定でセルの値を取得
                                IXLRange table = worksheet.Range(examinationTarget.Cell).AsTable();
                                foreach (IXLRangeRow rowData in table.Rows())
                                {
                                    foreach (IXLCell cellData in rowData.Cells())
                                    {
                                        MatchCollection regexMatchResults = Regex.Matches(cellData.Value.ToString(), @"[0-9]+");
                                        foreach (Match regexMatchResult in regexMatchResults.Cast<Match>())
                                        {
                                            // 正規表現で数値を抽出して変換結果を結果とする
                                            result = new()
                                            {
                                                File = filename,
                                                Sheet = examinationTarget.Sheet,
                                                Cell = cellData.Address.ToString(),
                                                Memo = examinationTarget.Memo,
                                                Number = regexMatchResult.Value,
                                                Name = GetName(regexMatchResult.Value)
                                            };
                                            results.Add(result);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                result = new()
                {
                    File = filename,
                    Sheet = "―",
                    Cell = "―",
                    Memo = "―",
                    Number = "―",
                    Name = Resources.Strings.MessageResultFileOpenError
                };
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// 名称取得処理
        /// </summary>
        /// <param name="number">数値</param>
        /// <returns></returns>
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
            string ruleFilename = ApplicationSettings.ReadSettingsExcelFileNumberToNameConvertRulesFilePath();
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
