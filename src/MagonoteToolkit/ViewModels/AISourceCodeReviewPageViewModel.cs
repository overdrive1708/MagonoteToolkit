using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagonoteToolkit.ViewModels
{
    internal partial class AISourceCodeReviewPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// ソースコードレビュー対象
        /// </summary>
        [ObservableProperty]
        private string _sourceCodeReviewTarget = string.Empty;

        /// <summary>
        /// ソースコードレビュー結果
        /// </summary>
        [ObservableProperty]
        private string _sourceCodeReviewResult = string.Empty;

        /// <summary>
        /// モデルリスト
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _modelList = [];

        /// <summary>
        /// 選択中のモデル
        /// </summary>
        [ObservableProperty]
        private string _selectedModel = string.Empty;

        /// <summary>
        /// ソースコードレビュー実施中フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// ソースコードレビュー実施
        /// </summary>
        [RelayCommand]
        private void SourceCodeReview() => ExecuteCommandSourceCodeReview();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AISourceCodeReviewPageViewModel()
        {
            // モデルリストの生成
            LoadModels();
        }

        /// <summary>
        /// ソースコードレビュー実施コマンド実行処理
        /// </summary>
        private async void ExecuteCommandSourceCodeReview()
        {
            // ソースコードレビュー前処理
            IsOperationEnable = false;
            SourceCodeReviewResult = Resources.Strings.MessageStatusNowSourceCodeReviewing;

            // ソースコードレビュー処理
            SourceCodeReviewResult = await Task.Run(() => AISourceCodeReviewer.SourceCodeReview(SourceCodeReviewTarget, SelectedModel));

            // ソースコードレビュー後処理
            IsOperationEnable = true;
        }

        /// <summary>
        /// モデルリストの生成
        /// </summary>
        private async void LoadModels()
        {
            // モデルリストの初期化
            if (!(bool)System.ComponentModel.DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
                // (XAMLデザイナーのエラー対策でデザインモードではない場合のみ)
                ModelList = await Task.Run(AISourceCodeReviewer.GetModels);
            }
            SelectedModel = ModelList.FirstOrDefault();
        }
    }
}
