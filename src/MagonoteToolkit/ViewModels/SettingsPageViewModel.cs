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

        /// <summary>
        /// ExcelファイルID->名称変換:設定ファイルパス
        /// </summary>
        [ObservableProperty]
        private string _excelFileNumberToNameSettingsFilePath;

        /// <summary>
        /// ExcelファイルID->名称変換:変換ルールファイルパス
        /// </summary>
        [ObservableProperty]
        private string _excelFileNumberToNameConvertRulesFilePath;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// ファイルパスドラッグ
        /// </summary>
        /// <param name="e">イベントデータ</param>
        [RelayCommand]
        private static void FilePathPreviewDragOver(DragEventArgs e) => ExecuteCommandFilePathPreviewDragOver(e);

        /// <summary>
        /// Excelファイル検査:設定ファイルパスドロップ
        /// </summary>
        /// <param name="e">イベントデータ</param>
        [RelayCommand]
        private void ExcelFileInspectionSettingsFilePathDrop(DragEventArgs e) => ExecuteCommandExcelFileInspectionSettingsFilePathDrop(e);

        /// <summary>
        /// ExcelファイルID->名称変換:設定ファイルパスドロップ
        /// </summary>
        /// <param name="e">イベントデータ</param>
        [RelayCommand]
        private void ExcelFileNumberToNameSettingsFilePathDrop(DragEventArgs e) => ExecuteCommandExcelFileNumberToNameSettingsFilePathDrop(e);

        /// <summary>
        /// ExcelファイルID->名称変換:変換ルールファイルパスドロップ
        /// </summary>
        /// <param name="e">イベントデータ</param>
        [RelayCommand]
        private void ExcelFileNumberToNameConvertRulesFilePathDrop(DragEventArgs e) => ExecuteCommandExcelFileNumberToNameConvertRulesFilePathDrop(e);

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
                ExcelFileNumberToNameSettingsFilePath = readSettings.ExcelFileNumberToNameSettingsFilePath;
                ExcelFileNumberToNameConvertRulesFilePath = readSettings.ExcelFileNumberToNameConvertRulesFilePath;
            }
        }

        /// <summary>
        /// ファイルパスドラッグコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private static void ExecuteCommandFilePathPreviewDragOver(DragEventArgs e)
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
        /// ExcelファイルID->名称変換:設定ファイルパスドロップコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandExcelFileNumberToNameSettingsFilePathDrop(DragEventArgs e)
        {
            // ドロップされたデータの1つ目をファイル名として採用する｡
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropitems)
            {
                ExcelFileNumberToNameSettingsFilePath = dropitems[0];
            }
        }

        /// <summary>
        /// ExcelファイルID->名称変換:変換ルールファイルパスドロップコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandExcelFileNumberToNameConvertRulesFilePathDrop(DragEventArgs e)
        {
            // ドロップされたデータの1つ目をファイル名として採用する｡
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropitems)
            {
                ExcelFileNumberToNameConvertRulesFilePath = dropitems[0];
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
                ExcelFileInspectionSettingsFilePath = ExcelFileInspectionSettingsFilePath,
                ExcelFileNumberToNameSettingsFilePath = ExcelFileNumberToNameSettingsFilePath,
                ExcelFileNumberToNameConvertRulesFilePath = ExcelFileNumberToNameConvertRulesFilePath,
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
