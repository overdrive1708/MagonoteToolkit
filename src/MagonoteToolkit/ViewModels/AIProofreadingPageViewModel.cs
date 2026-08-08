using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagonoteToolkit.ViewModels
{
    internal partial class AIProofreadingPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// 校正対象テキスト
        /// </summary>
        [ObservableProperty]
        private string _proofreadingTargetText = string.Empty;

        /// <summary>
        /// 校正結果
        /// </summary>
        [ObservableProperty]
        private string _proofreadingResult = string.Empty;

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
        /// 校正実施中フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// 校正実施
        /// </summary>
        [RelayCommand]
        private void Proofreading() => ExecuteCommandProofreading();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AIProofreadingPageViewModel()
        {
            // モデルリストの生成
            LoadModels();
        }

        /// <summary>
        /// 校正実施コマンド実行処理
        /// </summary>
        private async void ExecuteCommandProofreading()
        {
            // 校正前処理
            IsOperationEnable = false;
            ProofreadingResult = Resources.Strings.MessageStatusNowProofreading;

            // 校正処理
            ProofreadingResult = await Task.Run(() => AIProofreader.Proofread(ProofreadingTargetText, SelectedModel));

            // 校正後処理
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
                ModelList = await Task.Run(AIProofreader.GetModels);
            }
            SelectedModel = ModelList.FirstOrDefault();
        }
    }
}
