using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace MagonoteToolkit.ViewModels
{
    internal partial class FileCopyPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// コピー元パス
        /// </summary>
        [ObservableProperty]
        private string _copySourcePath = string.Empty;

        /// <summary>
        /// コピー先パス
        /// </summary>
        [ObservableProperty]
        private string _copyDestinationPath = string.Empty;

        /// <summary>
        /// コピー条件：パスに含まれる文字列：有効無効
        /// </summary>
        [ObservableProperty]
        private bool _isEnableCopyConditionContainsText = false;

        /// <summary>
        /// コピー条件：パスに含まれる文字列
        /// </summary>
        [ObservableProperty]
        private string _copyConditionContainsText = string.Empty;

        /// <summary>
        /// コピー条件：期間指定：有効無効
        /// </summary>
        [ObservableProperty]
        private bool _isEnableCopyConditionWithinPeriod = false;

        /// <summary>
        /// コピー条件：期間指定：開始日
        /// </summary>
        [ObservableProperty]
        private DateTime _copyConditionStartDate = DateTime.Now;

        /// <summary>
        /// コピー条件：期間指定：終了日
        /// </summary>
        [ObservableProperty]
        private DateTime _copyConditionEndDate = DateTime.Now;

        /// <summary>
        /// コピーオプション：ディレクトリ構造をコピーするかどうか
        /// </summary>
        [ObservableProperty]
        private bool _isCopyTheDirectoryStructure = true;

        /// <summary>
        /// 操作可能フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

        /// <summary>
        /// プログレスバー現在値
        /// </summary>
        [ObservableProperty]
        private int _progressValue = 0;

        /// <summary>
        /// プログレスバー最大値
        /// </summary>
        [ObservableProperty]
        private int _progressMaximum = 1;

        /// <summary>
        /// 進捗メッセージ
        /// </summary>
        [ObservableProperty]
        private string _progressMessage = string.Empty;

        /// <summary>
        /// プログレスバー不確定フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isProgressIndeterminate = false;

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
        /// コピー元パスドロップ
        /// </summary>
        /// <param name="e">イベントデータ</param>
        [RelayCommand]
        private void CopySourcePathDrop(DragEventArgs e) => ExecuteCommandCopySourcePathDrop(e);

        /// <summary>
        /// コピー先パスドロップ
        /// </summary>
        /// <param name="e">イベントデータ</param>
        [RelayCommand]
        private void CopyDestinationPathDrop(DragEventArgs e) => ExecuteCommandCopyDestinationPathDrop(e);

        /// <summary>
        /// コピー
        /// </summary>
        [RelayCommand]
        private void Copy() => ExecuteCommandCopy();

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
        /// コピー元パスドロップコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandCopySourcePathDrop(DragEventArgs e)
        {
            // ドロップされたデータの1つ目がディレクトリならコピー元パスとして採用する｡
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropitems)
            {
                if (System.IO.Directory.Exists(dropitems[0]) == true)
                {
                    CopySourcePath = dropitems[0];
                }
            }
        }

        /// <summary>
        /// コピー先パスドロップコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandCopyDestinationPathDrop(DragEventArgs e)
        {
            // ドロップされたデータの1つ目がディレクトリならコピー先パスとして採用する｡
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropitems)
            {
                if (System.IO.Directory.Exists(dropitems[0]) == true)
                {
                    CopyDestinationPath = dropitems[0];
                }
            }
        }

        /// <summary>
        /// コピーコマンド実行処理
        /// </summary>
        private async void ExecuteCommandCopy()
        {
            // コピー前処理
            if (!System.IO.Directory.Exists(CopySourcePath))
            {
                ProgressMessage = Resources.Strings.MessageStatusCopySourceNotExist;
                return;
            }

            if (!System.IO.Directory.Exists(CopyDestinationPath))
            {
                ProgressMessage = Resources.Strings.MessageStatusCopyDestinationNotExist;
                return;
            }

            IsOperationEnable = false;
            IsProgressIndeterminate = true;
            ProgressMessage = Resources.Strings.MessageStatusNowProcessing;
            
            // コピー処理
            await Task.Run(() =>
            {
                // コピー先のファイルリスト作成
                if (System.IO.Directory.Exists(CopySourcePath))
                {
                    if (System.IO.Directory.GetFiles(@CopySourcePath, "*", System.IO.SearchOption.AllDirectories) is string[] files)
                    {
                        IsProgressIndeterminate = false;
                        ProgressMaximum = files.Length;
                        ProgressValue = 0;
                        ProgressMessage = string.Format(Resources.Strings.MessageStatusNowCopying, ProgressValue, ProgressMaximum);

                        foreach (string file in files)
                        {
                            bool isCopy = true;

                            // コピー条件を満たさない場合はコピーしない
                            if (IsEnableCopyConditionContainsText)
                            {
                                if (!file.Contains(CopyConditionContainsText))
                                {
                                    isCopy = false;
                                }
                            }
                            if (IsEnableCopyConditionWithinPeriod)
                            {
                                if ((System.IO.File.GetLastWriteTime(file).Date < CopyConditionStartDate.Date) || (CopyConditionEndDate.Date < System.IO.File.GetLastWriteTime(file).Date))
                                {
                                    isCopy = false;
                                }
                            }

                            // コピー条件を満たす場合はコピーする
                            if (isCopy)
                            {
                                // コピー先のパスを作成
                                string destinationFilePath = string.Empty;
                                if (IsCopyTheDirectoryStructure)
                                {
                                    // コピー元のディレクトリ構造をコピーする場合は、コピー元のパスをコピー先のパスに置換する
                                    destinationFilePath = file.Replace(CopySourcePath, CopyDestinationPath);
                                }
                                else
                                {
                                    // コピー元のディレクトリ構造をコピーしない場合は、コピー先のパスにファイル名を付加する
                                    destinationFilePath = System.IO.Path.Combine(CopyDestinationPath, System.IO.Path.GetFileName(file));
                                }
                                
                                // コピー先のディレクトリが存在しない場合は作成する
                                string destinationDirectoryPath = System.IO.Path.GetDirectoryName(destinationFilePath);
                                if (System.IO.Directory.Exists(destinationDirectoryPath) == false)
                                {
                                    System.IO.Directory.CreateDirectory(destinationDirectoryPath);
                                }

                                // ファイルコピー
                                System.IO.File.Copy(file, destinationFilePath, true);
                            }

                            ProgressValue++;
                            ProgressMessage = string.Format(Resources.Strings.MessageStatusNowCopying, ProgressValue, ProgressMaximum);
                        }
                    }
                }

                ProgressMaximum = 1;
                ProgressValue = 0;
            });

            // コピー後処理
            IsOperationEnable = true;
            IsProgressIndeterminate = false;
            ProgressMessage = Resources.Strings.MessageStatusCompleteProcessing;
        }
    }
}
