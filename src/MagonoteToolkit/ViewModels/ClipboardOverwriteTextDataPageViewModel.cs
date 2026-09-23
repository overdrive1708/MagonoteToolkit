using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;

namespace MagonoteToolkit.ViewModels
{
    internal partial class ClipboardOverwriteTextDataPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// 書き換え設定：末尾の改行文字を削除するかどうか
        /// </summary>
        [ObservableProperty]
        private bool _isRemoveTrailingNewline = false;

        /// <summary>
        /// 書き換え前のクリップボード文字列
        /// </summary>
        [ObservableProperty]
        private string _beforeModificationClipboardStrings = string.Empty;

        /// <summary>
        /// 書き換え後のクリップボード文字列
        /// </summary>
        [ObservableProperty]
        private string _afterModificationClipboardStrings = string.Empty;

        /// <summary>
        /// 操作可能フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

        /// <summary>
        /// ステータスメッセージ
        /// </summary>
        [ObservableProperty]
        private string _statusMessage = string.Empty;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// 書き換え実施
        /// </summary>
        [RelayCommand]
        private void Overwrite() => ExecuteCommandOverwrite();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 書き換え実施コマンド実行処理
        /// </summary>
        private void ExecuteCommandOverwrite()
        {
            // 書き換え前処理
            IsOperationEnable = false;
            BeforeModificationClipboardStrings = string.Empty;
            AfterModificationClipboardStrings = string.Empty;

            // 書き換え処理
            BeforeModificationClipboardStrings = ClipboardReader.ReadText();

            if(IsRemoveTrailingNewline)
            {
                ClipboardWriter.WriteTextRemoveTrailingNewline(BeforeModificationClipboardStrings);
            }

            AfterModificationClipboardStrings = ClipboardReader.ReadText();

            StatusMessage = Resources.Strings.MessageStatusCompleteOverwriteClipboard;

            // 書き換え後処理
            IsOperationEnable = true;
        }
    }
}
