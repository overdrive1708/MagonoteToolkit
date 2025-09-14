using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace MagonoteToolkit.ViewModels
{
    internal partial class ExcelFileNumberToNamePageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// 調査設定プリセット(リスト)
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _examinationSettingPresetList = [];

        /// <summary>
        /// 調査設定プリセット(選択状態)
        /// </summary>
        [ObservableProperty]
        private string _examinationSettingPreset = string.Empty;

        /// <summary>
        /// 調査ファイルキーワード
        /// </summary>
        [ObservableProperty]
        private string _examinationFileKeyword = string.Empty;

        /// <summary>
        /// 調査対象(リスト)
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ExcelFileNumberToNameSettings.ExaminationTarget> _examinationTargets = [];

        /// <summary>
        /// 調査ファイル
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _examinationFiles = [];

        /// <summary>
        /// 調査ファイルガイド表示可否
        /// </summary>
        [ObservableProperty]
        private bool _examinationFileGuideVisibility = true;

        /// <summary>
        /// 操作可能フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

        /// <summary>
        /// 調査結果
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ExcelFileNumberToNameConverter.ExaminationResult> _examinationResultList = [];

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
        /// 調査ファイルドラッグ
        /// </summary>
        [RelayCommand]
        private void ExaminationFilePreviewDragOver(DragEventArgs e) => ExecuteCommandExaminationFilePreviewDragOver(e);

        /// <summary>
        /// 調査ファイルドロップ
        /// </summary>
        [RelayCommand]
        private void ExaminationFileDrop(DragEventArgs e) => ExecuteCommandExaminationFileDrop(e);

        /// <summary>
        /// 調査ファイルクリア
        /// </summary>
        [RelayCommand]
        private void ClearExaminationFile() => ExecuteCommandClearExaminationFile();

        /// <summary>
        /// 調査実施
        /// </summary>
        [RelayCommand]
        private void Examination() => ExecuteCommandExamination();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExcelFileNumberToNamePageViewModel()
        {
            // 設定ファイルの読み込み
            if (!(bool)System.ComponentModel.DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
                // (XAMLデザイナーのエラー対策でデザインモードではない場合のみ)
                ExcelFileNumberToNameSettings.ReadSettings();
            }

            // 調査設定プリセット(リスト)の作成
            ExaminationSettingPresetList = new(ExcelFileNumberToNameSettings.GetPresetList());

            // 調査設定の反映(プリセットリスト先頭)
            if (ExaminationSettingPresetList.Count != 0)
            {
                LoadExaminationSettings(ExaminationSettingPresetList[0]);
            }
        }

        /// <summary>
        /// プリセット選択変更コマンド実行処理
        /// </summary>
        private void ExecuteCommandPresetChange()
        {
            // 検査設定の反映
            LoadExaminationSettings(ExaminationSettingPreset);
        }

        /// <summary>
        /// 調査ファイルドラッグコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandExaminationFilePreviewDragOver(DragEventArgs e)
        {
            // ドラッグしてきたデータがファイルの場合､ドロップを許可する｡
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        /// <summary>
        /// 調査ファイルドロップコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandExaminationFileDrop(DragEventArgs e)
        {
            // ドロップしてきたデータを解析する
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropitems)
            {
                foreach (string dropitem in dropitems)
                {
                    if (System.IO.Directory.Exists(dropitem) == true)
                    {
                        // フォルダの場合は配下の調査ファイルキーワードを含むサポートExcelファイルを調査ファイルのリストに追加
                        if (System.IO.Directory.GetFiles(@dropitem, "*", System.IO.SearchOption.AllDirectories) is string[] files)
                        {
                            foreach (string file in files)
                            {
                                if (System.IO.Path.GetFileName(file).StartsWith("~$"))
                                {
                                    // 一時ファイルは追加しない
                                    continue;
                                }
                                if (ExaminationFileKeyword == string.Empty)
                                {
                                    // 調査ファイルキーワードが空の場合は拡張子のみ判定
                                    if (System.IO.Path.GetExtension(file) == ".xlsx" || System.IO.Path.GetExtension(file) == ".xlsm")
                                    {
                                        ExaminationFiles.Add(file);
                                    }
                                }
                                else
                                {
                                    // 調査ファイルキーワードが空ではない場合はキーワードと拡張子を判定
                                    if (System.IO.Path.GetFileName(file).Contains(ExaminationFileKeyword) && (System.IO.Path.GetExtension(file) == ".xlsx" || System.IO.Path.GetExtension(file) == ".xlsm"))
                                    {
                                        ExaminationFiles.Add(file);
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
                        if (ExaminationFileKeyword == string.Empty)
                        {
                            // 調査ファイルキーワードが空の場合は拡張子のみ判定
                            if (System.IO.Path.GetExtension(dropitem) == ".xlsx" || System.IO.Path.GetExtension(dropitem) == ".xlsm")
                            {
                                ExaminationFiles.Add(dropitem);
                            }
                        }
                        else
                        {
                            // 調査ファイルキーワードを含むサポートExcelファイルの場合は調査ファイルのリストに追加
                            if (System.IO.Path.GetFileName(dropitem).Contains(ExaminationFileKeyword) && (System.IO.Path.GetExtension(dropitem) == ".xlsx" || System.IO.Path.GetExtension(dropitem) == ".xlsm"))
                            {
                                ExaminationFiles.Add(dropitem);
                            }
                        }
                    }
                }
            }

            // 調査ファイルガイド表示可否判定
            if (ExaminationFiles.Count != 0)
            {
                ExaminationFileGuideVisibility = false;
            }

            // 調査実施できるか確認
            CheckExecuteExamination();
        }

        /// <summary>
        /// 調査ファイルクリアコマンド実行処理
        /// </summary>
        private void ExecuteCommandClearExaminationFile()
        {
            // 調査ファイルリストをクリア
            ExaminationFiles.Clear();
            ExaminationFileGuideVisibility = true;

            // 調査実施できるか確認
            CheckExecuteExamination();
        }

        /// <summary>
        /// 調査実施コマンド実行処理
        /// </summary>
        private async void ExecuteCommandExamination()
        {
            List<ExcelFileNumberToNameConverter.ExaminationResult> examinationResultList = [];
            List<ExcelFileNumberToNameSettings.ExaminationTarget> examinationTargetList = new(ExaminationTargets);

            // 調査開始
            ExaminationResultList.Clear();
            ProgressMaximum = ExaminationFiles.Count;
            ProgressValue = 0;
            IsOperationEnable = false;
            ProgressMessage = string.Format(Resources.Strings.MessageStatusNowExamination, ProgressValue, ProgressMaximum);

            // 調査実施
            await Task.Run(() =>
            {
                foreach (string file in ExaminationFiles)
                {
                    List<ExcelFileNumberToNameConverter.ExaminationResult> examinationResults = ExcelFileNumberToNameConverter.GetExaminationResult(file, examinationTargetList);
                    foreach (ExcelFileNumberToNameConverter.ExaminationResult examinationResult in examinationResults)
                    {
                        examinationResultList.Add(examinationResult);
                    }
                    ProgressValue++;
                    ProgressMessage = string.Format(Resources.Strings.MessageStatusNowExamination, ProgressValue, ProgressMaximum);
                }
            });

            // 調査完了
            ExaminationResultList = new(examinationResultList);
            IsOperationEnable = true;
            ProgressMessage = Resources.Strings.MessageStatusCompleteExamination;
        }

        /// <summary>
        /// 調査設定反映処理
        /// </summary>
        /// <param name="presetName">プリセット名</param>
        private void LoadExaminationSettings(string presetName)
        {
            // プリセット取得
            ExcelFileNumberToNameSettings.Setting setting = ExcelFileNumberToNameSettings.GetPreset(presetName);

            // 調査設定プリセット(選択状態)に反映
            ExaminationSettingPreset = presetName;

            // 調査ファイルキーワードに反映
            ExaminationFileKeyword = setting.ExaminationFileKeyword;

            // 調査対象(リスト)に反映
            ExaminationTargets = new(setting.ExaminationTargets);

            // 調査実施できるか確認
            CheckExecuteExamination();
        }

        /// <summary>
        /// 調査実施可否確認処理
        /// </summary>
        private void CheckExecuteExamination()
        {
            if (ExaminationTargets.Count == 0)
            {
                // 調査設定の調査対象がない場合は調査実施不可
                IsOperationEnable = false;
                ProgressMessage = Resources.Strings.MessageStatusExaminationTargetEmpty;
            }
            else if (ExaminationFiles.Count == 0)
            {
                // 調査ファイルがない場合は調査実施不可
                IsOperationEnable = false;
                ProgressMessage = Resources.Strings.MessageStatusExaminationFileEmpty;
            }
            else
            {
                // チェック通過時検査実施可能
                IsOperationEnable = true;
                ProgressMessage = Resources.Strings.MessageStatusAlreadyExamination;
            }
        }
    }
}
