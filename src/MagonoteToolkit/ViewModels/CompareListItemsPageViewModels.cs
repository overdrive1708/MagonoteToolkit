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
                Comparator.CompareListItemsResult result = Comparator.CompareListItems(BeforeListItems, AfterListItems);
                AddListItems = result.AddListItems;
                DeleteListItems = result.DeleteListItems;
            });

            // 比較後処理
            IsOperationEnable = true;
        }
    }
}
