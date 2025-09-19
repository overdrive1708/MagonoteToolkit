using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// Excelファイル検査クラス
    /// </summary>
    public class ExcelFileInspector
    {
        /// <summary>
        /// 検査結果クラス
        /// </summary>
        public class InspectionResult
        {
            /// <summary>
            /// ファイル名
            /// </summary>
            public string FileName { get; set; } = string.Empty;

            /// <summary>
            /// セル
            /// </summary>
            public string Cell { get; set; } = string.Empty;

            /// <summary>
            /// 結果
            /// </summary>
            public string ResultMessage { get; set; } = string.Empty;
        }

        /// <summary>
        /// ファイル検査処理
        /// </summary>
        /// <param name="filename">検査対象ファイル名</param>
        /// <param name="methods">検査方法</param>
        /// <returns></returns>
        public static List<InspectionResult> InspectionFile(string filename, List<ExcelFileInspectionSettings.InspectionMethod> methods)
        {
            List<InspectionResult> results = [];
            InspectionResult result;
            List<IXLAddress> addresses = [];

            try
            {
                // Bookを開く(読み取り専用で開く)
                using (FileStream fs = new(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using XLWorkbook workbook = new(fs);
                    // 全シート調査
                    foreach (IXLWorksheet worksheet in workbook.Worksheets)
                    {
                        // 検査方法で指定された条件を確認
                        foreach (ExcelFileInspectionSettings.InspectionMethod method in methods)
                        {
                            // 検査対象セルリスト作成
                            addresses.Clear();
                            if (!method.Cell.Contains(':'))
                            {
                                // 単一セルの場合
                                addresses.Add(worksheet.Cell(method.Cell).Address);
                            }
                            else
                            {
                                // 複数セルの場合
                                IXLRange table = worksheet.Range(method.Cell).AsTable();
                                foreach (IXLRangeRow rowData in table.Rows())
                                {
                                    foreach (IXLCell cellData in rowData.Cells())
                                    {
                                        addresses.Add(cellData.Address);
                                    }
                                }
                            }

                            // 検査対象セルを検査
                            foreach (IXLAddress address in addresses)
                            {
                                switch (method.Condition)
                                {
                                    case "Equal":
                                        // 指定されたセルが指定された値になっている場合はNG
                                        if (worksheet.Name.Equals(method.Sheet) && (worksheet.Cell(address).Value.ToString() == method.Value))
                                        {
                                            result = new()
                                            {
                                                FileName = filename,
                                                Cell = address.ToString(),
                                                ResultMessage = string.Format(Resources.Strings.MessageResultInspectionNGEqual, method.Value)
                                            };
                                            results.Add(result);
                                        }
                                        break;
                                    case "NotEqual":
                                        // 指定されたセルが指定された値以外になっている場合はNG
                                        if (worksheet.Name.Equals(method.Sheet) && (worksheet.Cell(address).Value.ToString() != method.Value))
                                        {
                                            result = new()
                                            {
                                                FileName = filename,
                                                Cell = address.ToString(),
                                                ResultMessage = string.Format(Resources.Strings.MessageResultInspectionNGNotEqual, method.Value)
                                            };
                                            results.Add(result);
                                        }
                                        break;
                                    case "Empty":
                                        // 指定されたセルが空である場合はNG
                                        if (worksheet.Name.Equals(method.Sheet) && (worksheet.Cell(address).Value.ToString() == string.Empty))
                                        {
                                            result = new()
                                            {
                                                FileName = filename,
                                                Cell = address.ToString(),
                                                ResultMessage = Resources.Strings.MessageResultInspectionNGEmpty
                                            };
                                            results.Add(result);
                                        }
                                        break;
                                    case "NotEmpty":
                                        // 指定されたセルが空ではない場合はNG
                                        if (worksheet.Name.Equals(method.Sheet) && (worksheet.Cell(address).Value.ToString() != string.Empty))
                                        {
                                            result = new()
                                            {
                                                FileName = filename,
                                                Cell = address.ToString(),
                                                ResultMessage = Resources.Strings.MessageResultInspectionNGNotEmpty
                                            };
                                            results.Add(result);
                                        }
                                        break;
                                    default:
                                        break;
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
                    FileName = filename,
                    Cell = "―",
                    ResultMessage = Resources.Strings.MessageResultFileOpenError
                };
                results.Add(result);
            }

            // NGが1つもない場合は問題なしとする
            if (results.Count == 0)
            {
                result = new()
                {
                    FileName = filename,
                    Cell = "―",
                    ResultMessage = Resources.Strings.MessageResultInspectionOK
                };
                results.Add(result);
            }

            return results;
        }
    }
}
