using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace MagonoteToolkit.ViewModels
{
    internal partial class ExcelFileInspectionPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// 検査設定プリセット(リスト)
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _inspectionSettingPresetList = [];

        /// <summary>
        /// 検査設定プリセット(選択状態)
        /// </summary>
        [ObservableProperty]
        private string _inspectionSettingPreset = string.Empty;

        /// <summary>
        /// 対象ファイルキーワード
        /// </summary>
        [ObservableProperty]
        private string _targetFileKeyword = string.Empty;

        /// <summary>
        /// 検査方法(リスト)
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ExcelFileInspectionSettings.InspectionMethod> _inspectionMethods = [];

        /// <summary>
        /// 検査ファイル
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _inspectionFiles = [];

        /// <summary>
        /// 検査ファイルガイド表示可否
        /// </summary>
        [ObservableProperty]
        private bool _inspectionFileGuideVisibility = true;

        /// <summary>
        /// 操作可能フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

        /// <summary>
        /// 検査結果
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ExcelFileInspector.InspectionResult> _inspectionResultList = [];

        /// <summary>
        /// プログレスバー最大値
        /// </summary>
        [ObservableProperty]
        private int _progressMaximum = 1;

        /// <summary>
        /// プログレスバー現在値
        /// </summary>
        [ObservableProperty]
        private int _progressValue = 0;

        /// <summary>
        /// 進捗メッセージ
        /// </summary>
        [ObservableProperty]
        private string _progressMessage = string.Empty;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// プリセット選択変更
        /// </summary>
        [RelayCommand]
        private void PresetChange() => ExecuteCommandPresetChange();

        /// <summary>
        /// 検査ファイルドラッグ
        /// </summary>
        [RelayCommand]
        private void InspectionPreviewDragOver(DragEventArgs e) => ExecuteCommandInspectionPreviewDragOver(e);

        /// <summary>
        /// 検査ファイルドロップ
        /// </summary>
        [RelayCommand]
        private void InspectionFileDrop(DragEventArgs e) => ExecuteCommandInspectionFileDrop(e);

        /// <summary>
        /// 検査ファイルクリア
        /// </summary>
        [RelayCommand]
        private void ClearInspectionFile() => ExecuteCommandClearInspectionFile();

        /// <summary>
        /// 検査実施
        /// </summary>
        [RelayCommand]
        private void Inspection() => ExecuteCommandInspection();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExcelFileInspectionPageViewModel()
        {
            // 設定ファイルの読み込み
            if (!(bool)System.ComponentModel.DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
                // (XAMLデザイナーのエラー対策でデザインモードではない場合のみ)
                ExcelFileInspectionSettings.ReadSettings();
            }

            // 検査設定プリセット(リスト)の作成
            InspectionSettingPresetList = new(ExcelFileInspectionSettings.GetPresetList());

            // 検査設定の反映(プリセットリスト先頭)
            if (InspectionSettingPresetList.Count != 0)
            {
                LoadInspectionSettings(InspectionSettingPresetList[0]);
            }
        }

        /// <summary>
        /// プリセット選択変更コマンド実行処理
        /// </summary>
        private void ExecuteCommandPresetChange()
        {
            // 検査設定の反映
            LoadInspectionSettings(InspectionSettingPreset);
        }

        /// <summary>
        /// 検査ファイルドラッグコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandInspectionPreviewDragOver(DragEventArgs e)
        {
            // ドラッグしてきたデータがファイルの場合､ドロップを許可する｡
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        /// <summary>
        /// 検査ファイルドロップコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandInspectionFileDrop(DragEventArgs e)
        {
            // ドロップしてきたデータを解析する
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropitems)
            {
                foreach (string dropitem in dropitems)
                {
                    if (System.IO.Directory.Exists(dropitem) == true)
                    {
                        // フォルダの場合は配下の対象ファイルキーワードを含むサポートExcelファイルを検査ファイルのリストに追加
                        if (System.IO.Directory.GetFiles(@dropitem, "*", System.IO.SearchOption.AllDirectories) is string[] files)
                        {
                            foreach (string file in files)
                            {
                                if (System.IO.Path.GetFileName(file).StartsWith("~$"))
                                {
                                    // 一時ファイルは追加しない
                                    continue;
                                }
                                if (TargetFileKeyword == string.Empty)
                                {
                                    // 対象ファイルキーワードが空の場合は拡張子のみ判定
                                    if (System.IO.Path.GetExtension(file) == ".xlsx" || System.IO.Path.GetExtension(file) == ".xlsm")
                                    {
                                        InspectionFiles.Add(file);
                                    }
                                }
                                else
                                {
                                    // 対象ファイルキーワードが空ではない場合はキーワードと拡張子を判定
                                    if (System.IO.Path.GetFileName(file).Contains(TargetFileKeyword) && (System.IO.Path.GetExtension(file) == ".xlsx" || System.IO.Path.GetExtension(file) == ".xlsm"))
                                    {
                                        InspectionFiles.Add(file);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (System.IO.Path.GetFileName(dropitem).StartsWith("~$"))
                        {
                            // 一時ファイルは追加しない
                            continue;
                        }
                        if (TargetFileKeyword == string.Empty)
                        {
                            // 対象ファイルキーワードが空の場合は拡張子のみ判定
                            if (System.IO.Path.GetExtension(dropitem) == ".xlsx" || System.IO.Path.GetExtension(dropitem) == ".xlsm")
                            {
                                InspectionFiles.Add(dropitem);
                            }
                        }
                        else
                        {
                            // 対象ファイルキーワードを含むサポートExcelファイルの場合は検査ファイルのリストに追加
                            if (System.IO.Path.GetFileName(dropitem).Contains(TargetFileKeyword) && (System.IO.Path.GetExtension(dropitem) == ".xlsx" || System.IO.Path.GetExtension(dropitem) == ".xlsm"))
                            {
                                InspectionFiles.Add(dropitem);
                            }
                        }
                    }
                }
            }

            // 検査ファイルガイド表示可否判定
            if (InspectionFiles.Count != 0)
            {
                InspectionFileGuideVisibility = false;
            }

            // 検査実施できるか確認
            CheckExecuteInspection();
        }

        /// <summary>
        /// 検査ファイルクリアコマンド実行処理
        /// </summary>
        private void ExecuteCommandClearInspectionFile()
        {
            // 検査ファイルリストをクリア
            InspectionFiles.Clear();
            InspectionFileGuideVisibility = true;

            // 検査実施できるか確認
            CheckExecuteInspection();
        }

        /// <summary>
        /// 検査実施コマンド実行処理
        /// </summary>
        private async void ExecuteCommandInspection()
        {
            List<ExcelFileInspector.InspectionResult> inspectionResult = [];
            List<ExcelFileInspectionSettings.InspectionMethod> inspectionMethods = new(InspectionMethods);

            // 検査開始
            InspectionResultList.Clear();
            ProgressMaximum = InspectionFiles.Count;
            ProgressValue = 0;
            IsOperationEnable = false;
            ProgressMessage = string.Format(Resources.Strings.MessageStatusNowInspection, ProgressValue, ProgressMaximum);

            // 検査実施
            await Task.Run(() =>
            {
                foreach (string file in InspectionFiles)
                {
                    List<ExcelFileInspector.InspectionResult> inspectionResults = ExcelFileInspector.InspectionFile(file, inspectionMethods);
                    foreach (ExcelFileInspector.InspectionResult result in inspectionResults)
                    {
                        inspectionResult.Add(result);
                    }
                    ProgressValue++;
                    ProgressMessage = string.Format(Resources.Strings.MessageStatusNowInspection, ProgressValue, ProgressMaximum);
                }
            });

            // 検査完了
            InspectionResultList = new(inspectionResult);
            IsOperationEnable = true;
            ProgressMessage = Resources.Strings.MessageStatusCompleteInspection;
        }

        /// <summary>
        /// 検査設定反映処理
        /// </summary>
        /// <param name="presetName">プリセット名</param>
        private void LoadInspectionSettings(string presetName)
        {
            // プリセット取得
            ExcelFileInspectionSettings.Setting setting = ExcelFileInspectionSettings.GetPreset(presetName);

            // 検査設定プリセット(選択状態)に反映
            InspectionSettingPreset = presetName;

            // 対象ファイルキーワードに反映
            TargetFileKeyword = setting.TargetFileKeyword;

            // 検査方法(リスト)に反映
            InspectionMethods = new(setting.InspectionMethods);

            // 検査実施できるか確認
            CheckExecuteInspection();
        }

        /// <summary>
        /// 検査実施可否確認処理
        /// </summary>
        private void CheckExecuteInspection()
        {
            if (InspectionMethods.Count == 0)
            {
                // 検査設定の検査方法がない場合は検査実施不可
                IsOperationEnable = false;
                ProgressMessage = Resources.Strings.MessageStatusInspectionMethodEmpty;
            }
            else if (InspectionFiles.Count == 0)
            {
                // 検査ファイルがない場合は検査実施不可
                IsOperationEnable = false;
                ProgressMessage = Resources.Strings.MessageStatusInspectionFileEmpty;
            }
            else
            {
                // チェック通過時検査実施可能
                IsOperationEnable = true;
                ProgressMessage = Resources.Strings.MessageStatusAlreadyInspection;
            }
        }
    }
}
