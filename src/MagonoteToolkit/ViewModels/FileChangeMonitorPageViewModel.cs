using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace MagonoteToolkit.ViewModels
{
    internal partial class FileChangeMonitorPageViewModel : ObservableObject
    {
        /// <summary>
        /// 表示用クラス
        /// </summary>
        public class MonitorResultViewInfo
        {
            /// <summary>
            /// 選択状態
            /// </summary>
            public bool IsSelected { get; set; } = false;

            /// <summary>
            /// チェック結果
            /// </summary>
            public string CheckResult { get; set; } = string.Empty;

            /// <summary>
            /// ファイル
            /// </summary>
            public string File { get; set; } = string.Empty;

            /// <summary>
            /// チェック済みのタイムスタンプ
            /// </summary>
            public string CheckedTimestamp { get; set; } = string.Empty;

            /// <summary>
            /// 現在のタイムスタンプ
            /// </summary>
            public string CurrentTimestamp { get; set; } = string.Empty;

            /// <summary>
            /// メモ
            /// </summary>
            public string Memo { get; set; } = string.Empty;
        }

        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// ワークスペース(リスト)
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _workspaceList = [];

        /// <summary>
        /// ワークスペース(選択状態)
        /// </summary>
        [ObservableProperty]
        private string _workspace = string.Empty;

        /// <summary>
        /// ファイル追加時メモ
        /// </summary>
        [ObservableProperty]
        private string _addMemo = string.Empty;

        /// <summary>
        /// ファイル変更監視結果
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<MonitorResultViewInfo> _monitorResult = [];

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
        /// ワークスペース選択変更
        /// </summary>
        [RelayCommand]
        private void WorkspaceChange() => ExecuteCommandWorkspaceChange();

        /// <summary>
        /// ワークスペース追加
        /// </summary>
        [RelayCommand]
        private void AddWorkspace() => ExecuteCommandAddWorkspace();

        /// <summary>
        /// ワークスペース削除
        /// </summary>
        [RelayCommand]
        private void DelWorkspace() => ExecuteCommandDelWorkspace();

        /// <summary>
        /// ファイルドラッグ
        /// </summary>
        [RelayCommand]
        private void PreviewDragOver(DragEventArgs e) => ExecuteCommandPreviewDragOver(e);

        /// <summary>
        /// ファイルドロップ
        /// </summary>
        [RelayCommand]
        private void Drop(DragEventArgs e) => ExecuteCommandDrop(e);

        /// <summary>
        /// 削除(すべて)
        /// </summary>
        [RelayCommand]
        private void DeleteAll() => ExecuteCommandDeleteAll();

        /// <summary>
        /// 削除(選択済み)
        /// </summary>
        [RelayCommand]
        private void DeleteSelectedItem() => ExecuteCommandDeleteSelectedItem();

        /// <summary>
        /// タイムスタンプ更新(すべて)
        /// </summary>
        [RelayCommand]
        private void UpdateTimestampAll() => ExecuteCommandUpdateTimestampAll();

        /// <summary>
        /// タイムスタンプ更新(選択済み)
        /// </summary>
        [RelayCommand]
        private void UpdateTimestampSelectedItem() => ExecuteCommandUpdateTimestampSelectedItem();

        /// <summary>
        /// 再チェック
        /// </summary>
        [RelayCommand]
        private void Recheck() => ExecuteCommandRecheck();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileChangeMonitorPageViewModel()
        {
            // ワークスペースディレクトリ内を確認してデータベースファイルをリスト化
            if (!(bool)System.ComponentModel.DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
                // (XAMLデザイナーのエラー対策でデザインモードではない場合のみ)
                CreateWorkspaceList();
            }
        }

        /// <summary>
        /// ワークスペース選択変更コマンド実行処理
        /// </summary>
        private void ExecuteCommandWorkspaceChange()
        {
            // ファイル変更監視結果生成
            CreateMonitorResult();
        }

        /// <summary>
        /// ワークスペース追加コマンド実行処理
        /// </summary>
        private void ExecuteCommandAddWorkspace()
        {
            // ワークスペース名を入力させる
            string workspaceName = Microsoft.VisualBasic.Interaction.InputBox(Resources.Strings.MessageInputWorkspaceName, Resources.Strings.InputWorkspaceName, "", -1, -1);

            // ワークスペース追加
            if (workspaceName != string.Empty)
            {
                FileChangeMonitor.AddWorkspace(workspaceName);
            }

            // ワークスペース(リスト)再生成
            CreateWorkspaceList();

            // 完了メッセージの表示
            _ = MessageBox.Show(Resources.Strings.MessageStatusCompleteAdd,
                                Resources.Strings.Notice,
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
        }

        /// <summary>
        /// ワークスペース削除コマンド実行処理
        /// </summary>
        private void ExecuteCommandDelWorkspace()
        {
            // 確認画面を出してYESならワークスペース削除
            MessageBoxResult result = MessageBox.Show(Resources.Strings.MessageConfirmDelete, Resources.Strings.Notice, MessageBoxButton.YesNo);

            // ワークスペース削除
            if (result == MessageBoxResult.Yes)
            {
                FileChangeMonitor.DeleteWorkspace(Workspace);
            }

            // ワークスペース(リスト)再生成
            CreateWorkspaceList();

            // 完了メッセージの表示
            _ = MessageBox.Show(Resources.Strings.MessageStatusCompleteDelete,
                                Resources.Strings.Notice,
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
        }

        /// <summary>
        /// ファイルドラッグコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandPreviewDragOver(DragEventArgs e)
        {
            // ドラッグしてきたデータがファイルの場合､ドロップを許可する｡
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        /// <summary>
        /// ファイルドロップコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandDrop(DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropitems)
            {
                foreach (string dropitem in dropitems)
                {
                    if (System.IO.Directory.Exists(dropitem) == true)
                    {
                        if (System.IO.Directory.GetFiles(@dropitem, "*", System.IO.SearchOption.AllDirectories) is string[] files)
                        {
                            foreach (string file in files)
                            {
                                if (System.IO.Path.GetFileName(file).StartsWith("~$"))
                                {
                                    // 一時ファイルは追加しない
                                    continue;
                                }
                                FileChangeMonitor.AddFile(Workspace, file, AddMemo);
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
                        FileChangeMonitor.AddFile(Workspace, dropitem, AddMemo);
                    }
                }
            }

            // ファイル変更監視結果生成
            CreateMonitorResult();
        }

        /// <summary>
        /// 削除(すべて)コマンド実行処理
        /// </summary>
        private void ExecuteCommandDeleteAll()
        {
            // TODO:DBから全て削除
        }

        /// <summary>
        /// 削除(選択済み)コマンド実行処理
        /// </summary>
        private void ExecuteCommandDeleteSelectedItem()
        {
            // TODO:DBから選択済みのものを削除
        }

        /// <summary>
        /// タイムスタンプ更新(すべて)コマンド実行処理
        /// </summary>
        private void ExecuteCommandUpdateTimestampAll()
        {
            // TODO:すべてタイムスタンプ更新
        }

        /// <summary>
        /// タイムスタンプ更新(選択済み)コマンド実行処理
        /// </summary>
        private void ExecuteCommandUpdateTimestampSelectedItem()
        {
            // TODO:選択済みのタイムスタンプ更新
        }

        /// <summary>
        /// 再チェックコマンド実行処理
        /// </summary>
        private void ExecuteCommandRecheck()
        {
            // ファイル変更監視結果生成
            CreateMonitorResult();
        }

        /// <summary>
        /// ワークスペース(リスト)作成処理
        /// </summary>
        private void CreateWorkspaceList()
        {
            // ワークスペースディレクトリ内を確認してデータベースファイルをリスト化
            WorkspaceList.Clear();

            string workspaceDirectory = ApplicationSettings.ReadSettingsFileChangeMonitorWorkspaceDirectory();

            if (System.IO.Directory.Exists(workspaceDirectory))
            {
                if (System.IO.Directory.GetFiles(workspaceDirectory, "*.db", System.IO.SearchOption.AllDirectories) is string[] files)
                {
                    foreach (string file in files)
                    {
                        WorkspaceList.Add(System.IO.Path.GetFileName(file));
                    }
                }
            }
        }

        /// <summary>
        /// ファイル変更監視結果生成処理
        /// </summary>
        private void CreateMonitorResult()
        {
            MonitorResultViewInfo view;
            MonitorResult.Clear();

            List<FileChangeMonitor.MonitorResult> results = FileChangeMonitor.CheckFile(Workspace);
            foreach(FileChangeMonitor.MonitorResult result in results)
            {
                view = new()
                {
                    IsSelected = false,
                    File = result.File,
                    CheckResult = result.CheckResult,
                    CheckedTimestamp = result.CheckedTimestamp,
                    CurrentTimestamp = result.CurrentTimestamp,
                    Memo = result.Memo
                };
                MonitorResult.Add(view);
            }
        }
    }
}
