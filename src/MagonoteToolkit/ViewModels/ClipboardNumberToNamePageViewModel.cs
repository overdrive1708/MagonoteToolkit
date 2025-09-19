using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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
        /// コンストラクタ
        /// </summary>
        public ClipboardNumberToNamePageViewModel()
        {
            ;
        }

        /// <summary>
        /// 調査実施コマンド実行処理
        /// </summary>
        private async void ExecuteCommandExamination()
        {
            List<ClipboardNumberToNameConverter.ExaminationResult> examinationResultList = [];

            // 操作禁止&プログレスバー更新
            IsOperationEnable = false;
            ProgressIsIndeterminate = true;
            ProgressMessage = Resources.Strings.MessageStatusNowProcessing;

            // クリップボード文字列を取得して表示する
            if (Clipboard.ContainsText())
            {
                ClipboardStrings = Clipboard.GetText();
            }
            else
            {
                ClipboardStrings = string.Empty;
            }

            // 調査結果クリア
            ExaminationResultList.Clear();

            // 調査実施
            await Task.Run(() =>
            {
                // クリップボード操作の制約で現在のスレッドをシングル スレッド アパートメント (STA) モードに設定
                if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
                {
                    Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
                    Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
                }

                List<ClipboardNumberToNameConverter.ExaminationResult> examinationResults = ClipboardNumberToNameConverter.GetExaminationResult();
                foreach (ClipboardNumberToNameConverter.ExaminationResult examinationResult in examinationResults)
                {
                    examinationResultList.Add(examinationResult);
                }
            });

            // 調査結果表示
            ExaminationResultList = new(examinationResultList);

            // 操作許可&プログレスバー更新
            IsOperationEnable = true;
            ProgressIsIndeterminate = false;
            ProgressMessage = Resources.Strings.MessageStatusCompleteProcessing;
        }
    }
}
