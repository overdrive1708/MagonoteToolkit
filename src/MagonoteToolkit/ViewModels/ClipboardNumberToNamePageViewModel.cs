using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MagonoteToolkit.ViewModels
{
    internal partial class ClipboardNumberToNamePageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// 調査結果
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ClipboardNumberToNameConverter.ExaminationResult> _examinationResultList = [];

        /// <summary>
        /// クリップボード文字列
        /// </summary>
        [ObservableProperty]
        private string _clipboardStrings = string.Empty;

        /// <summary>
        /// プログレスバー不確定フラグ
        /// </summary>
        [ObservableProperty]
        private bool _progressIsIndeterminate = false;

        /// <summary>
        /// 進捗メッセージ
        /// </summary>
        [ObservableProperty]
        private string _progressMessage = string.Empty;

        /// <summary>
        /// 操作可能フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// 調査実施
        /// </summary>
        [RelayCommand]
        private void Examination() => ExecuteCommandExamination();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 調査実施コマンド実行処理
        /// </summary>
        private async void ExecuteCommandExamination()
        {
            // 調査開始

            // 調査実施
            await Task.Run(() =>
            {
            });

            // 調査完了
        }
    }
}
