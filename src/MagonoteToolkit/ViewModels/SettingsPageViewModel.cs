using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Windows;

namespace MagonoteToolkit.ViewModels
{
    internal partial class SettingsPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// Excelファイル検査:設定ファイルパス
        /// </summary>
        [ObservableProperty]
        private string _excelFileInspectionSettingsFilePath;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// ファイルパスドラッグ
        /// </summary>
        [RelayCommand]
        private void FilePathPreviewDragOver(DragEventArgs e) => ExecuteCommandFilePathPreviewDragOver(e);

        /// <summary>
        /// Excelファイル検査:設定ファイルパスドロップ
        /// </summary>
        [RelayCommand]
        private void ExcelFileInspectionSettingsFilePathDrop(DragEventArgs e) => ExecuteCommandExcelFileInspectionSettingsFilePathDrop(e);

        /// <summary>
        /// 設定ファイル保存
        /// </summary>
        [RelayCommand]
        private void SaveSettings() => ExecuteCommandSaveSettings();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingsPageViewModel()
        {
            // 設定読み込みとバインディングデータの設定
            if (!(bool)System.ComponentModel.DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
                // (XAMLデザイナーのエラー対策でデザインモードではない場合のみ)
                ApplicationSettings readSettings = ApplicationSettings.ReadSettings();
                ExcelFileInspectionSettingsFilePath = readSettings.ExcelFileInspectionSettingsFilePath;
            }
        }

        /// <summary>
        /// ファイルパスドラッグコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandFilePathPreviewDragOver(DragEventArgs e)
        {
            // ドラッグしてきたデータがファイルの場合､ドロップを許可する｡
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        /// <summary>
        /// Excelファイル検査:設定ファイルパスドロップコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandExcelFileInspectionSettingsFilePathDrop(DragEventArgs e)
        {
            // ドロップされたデータの1つ目をファイル名として採用する｡
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropitems)
            {
                ExcelFileInspectionSettingsFilePath = dropitems[0];
            }
        }

        /// <summary>
        /// 設定ファイル保存コマンド実行処理
        /// </summary>
        private void ExecuteCommandSaveSettings()
        {
            // 書き込み値作成
            ApplicationSettings writeSettings = new()
            {
                ExcelFileInspectionSettingsFilePath = ExcelFileInspectionSettingsFilePath
            };

            // 設定書き込み
            ApplicationSettings.WriteSettings(writeSettings);

            // 完了メッセージの表示
            _ = MessageBox.Show(Resources.Strings.MessageStatusCompleteSaveSettings,
                                Resources.Strings.Notice,
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
        }
    }
}
