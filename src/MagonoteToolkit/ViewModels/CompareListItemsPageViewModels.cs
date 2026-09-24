using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MagonoteToolkit.Models;
using System.Threading.Tasks;
using static MagonoteToolkit.Models.Messages;

namespace MagonoteToolkit.ViewModels
{
    internal partial class CompareListItemsPageViewModels : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// 変更前リストアイテム
        /// </summary>
        [ObservableProperty]
        private string _beforeListItems = string.Empty;

        /// <summary>
        /// 変更後リストアイテム
        /// </summary>
        [ObservableProperty]
        private string _afterListItems = string.Empty;

        /// <summary>
        /// 追加リストアイテム
        /// </summary>
        [ObservableProperty]
        private string _addListItems = string.Empty;
        
        /// <summary>
        /// 削除リストアイテム
        /// </summary>
        [ObservableProperty]
        private string _deleteListItems = string.Empty;

        /// <summary>
        /// 操作可能フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

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
        /// プログレスバー進捗不定フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isProgressIndeterminate = false;

        /// <summary>
        /// 進捗メッセージ
        /// </summary>
        [ObservableProperty]
        private string _progressMessage = string.Empty;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// 比較実施
        /// </summary>
        [RelayCommand]
        private void Comparison() => ExecuteCommandComparison();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 比較実施コマンド実行処理
        /// </summary>
        private async void ExecuteCommandComparison()
        {
            // 比較前処理
            IsOperationEnable = false;
            AddListItems = string.Empty;
            DeleteListItems = string.Empty;

            // 比較処理
            await Task.Run(() =>
            {
                WeakReferenceMessenger.Default.Register<ProgressInfoChangeMessage>(this, ReceiveProgressInfoChangeMessage);

                Comparator.CompareListItemsResult result = Comparator.CompareListItems(BeforeListItems, AfterListItems);
                AddListItems = result.AddListItems;
                DeleteListItems = result.DeleteListItems;

                WeakReferenceMessenger.Default.Unregister<ProgressInfoChangeMessage>(this);
            });

            // 比較後処理
            IsOperationEnable = true;
        }

        /// <summary>
        /// 進捗情報変更メッセージ受信処理
        /// </summary>
        /// <param name="recipient">受信者</param>
        /// <param name="message">メッセージ</param>
        public void ReceiveProgressInfoChangeMessage(object recipient, ProgressInfoChangeMessage message)
        {
            ProgressMaximum = message.ProgressMaximum;
            ProgressValue = message.ProgressValue;
            IsProgressIndeterminate = message.IsProgressIndeterminate;
            ProgressMessage = message.ProgressMessage;
        }
    }
}
