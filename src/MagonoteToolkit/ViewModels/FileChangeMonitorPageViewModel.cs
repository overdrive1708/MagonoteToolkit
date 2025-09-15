using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            // TODO:ワークスペースディレクトリを確認してデータベースファイルをリスト化
            // TODO:リストはファイル名(非フルパス)
        }

        /// <summary>
        /// ワークスペース選択変更コマンド実行処理
        /// </summary>
        private void ExecuteCommandWorkspaceChange()
        {
            // TODO:選択したワークスペースでチェック実施
        }

        /// <summary>
        /// ワークスペース追加コマンド実行処理
        /// </summary>
        private void ExecuteCommandAddWorkspace()
        {
            // TODO:InputBoxで名前を取得してファイルにできる名前に置き換えてDB作成
        }

        /// <summary>
        /// ワークスペース削除コマンド実行処理
        /// </summary>
        private void ExecuteCommandDelWorkspace()
        {
            // TODO:確認画面を出してDB削除
        }

        /// <summary>
        /// ファイルドラッグコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandDrop(DragEventArgs e)
        {
            // TODO:ドロップ許可
        }

        /// <summary>
        /// ファイルドロップコマンド実行処理
        /// </summary>
        /// <param name="e">イベントデータ</param>
        private void ExecuteCommandPreviewDragOver(DragEventArgs e)
        {
            // TODO:ドロップしたファイルの情報をDBに書き込み
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
            // TODO:再チェック実施
        }
    }
}
