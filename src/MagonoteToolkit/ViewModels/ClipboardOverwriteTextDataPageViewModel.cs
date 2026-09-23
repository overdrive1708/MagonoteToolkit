using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.ObjectModel;

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
        /// 書き換え設定：改行文字を置換するかどうか
        /// </summary>
        [ObservableProperty]
        private bool _isReplaceNewline = false;

        /// <summary>
        /// 書き換え設定：置換する改行文字のリスト
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _replaceNewlineCharacterList = [Resources.Strings.ReplaceLF, Resources.Strings.ReplaceCR, Resources.Strings.ReplaceCRLF];

        /// <summary>
        /// 書き換え設定：置換する改行文字
        /// </summary>
        [ObservableProperty]
        private string _replaceNewlineCharacter = Resources.Strings.ReplaceCRLF;

        /// <summary>
        /// 書き換え設定：先頭と末尾のダブルクォーテーションを削除するかどうか
        /// </summary>
        [ObservableProperty]
        private bool _isRemoveLeadingTrailingDoubleQuotation = false;

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

            // 末尾の改行文字を削除する
            if (IsRemoveTrailingNewline)
            {
                ClipboardWriter.WriteTextRemoveTrailingNewline(ClipboardReader.ReadText());
            }

            // 改行文字を置換する
            if (IsReplaceNewline)
            {
                if (ReplaceNewlineCharacter == Resources.Strings.ReplaceLF)
                {
                    ClipboardWriter.WriteTextReplaceNewline(ClipboardReader.ReadText(), "\n");
                }
                else if (ReplaceNewlineCharacter == Resources.Strings.ReplaceCR)
                {
                    ClipboardWriter.WriteTextReplaceNewline(ClipboardReader.ReadText(), "\r");
                }
                else if (ReplaceNewlineCharacter == Resources.Strings.ReplaceCRLF)
                {
                    ClipboardWriter.WriteTextReplaceNewline(ClipboardReader.ReadText(), "\r\n");
                }
                else
                {
                    // 置換する改行文字が不正な場合は何もしない
                }
            }

            // 先頭と末尾のダブルクォーテーションを削除する
            if (IsRemoveLeadingTrailingDoubleQuotation)
            {
                ClipboardWriter.WriteTextRemoveLeadingTrailingDoubleQuotation(ClipboardReader.ReadText());
            }

            AfterModificationClipboardStrings = ClipboardReader.ReadText();

            StatusMessage = Resources.Strings.MessageStatusCompleteOverwriteClipboard;

            // 書き換え後処理
            IsOperationEnable = true;
        }
    }
}
