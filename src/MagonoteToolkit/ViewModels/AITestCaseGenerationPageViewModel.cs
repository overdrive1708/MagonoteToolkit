using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagonoteToolkit.ViewModels
{
    internal partial class AITestCaseGenerationPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// テストケース生成対象のソースコード
        /// </summary>
        [ObservableProperty]
        private string _testCaseGenerationTargetSourceCode = string.Empty;

        /// <summary>
        /// テストケース生成結果
        /// </summary>
        [ObservableProperty]
        private string _testCaseGenerationResult = string.Empty;

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
        /// テストケース生成実施中フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// テストケース生成実施
        /// </summary>
        [RelayCommand]
        private void TestCaseGeneration() => ExecuteCommandTestCaseGeneration();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AITestCaseGenerationPageViewModel()
        {
            // モデルリストの生成
            LoadModels();
        }

        private async void ExecuteCommandTestCaseGeneration()
        {
            // テストケース生成前処理
            IsOperationEnable = false;
            TestCaseGenerationResult = Resources.Strings.MessageStatusNowTestCaseGenerating;

            // テストケース生成処理
            TestCaseGenerationResult = await Task.Run(() => AITestCaseGenerator.Generate(TestCaseGenerationTargetSourceCode, SelectedModel));

            // テストケース生成後処理
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
                ModelList = await Task.Run(AITestCaseGenerator.GetModels);
            }
            SelectedModel = ModelList.FirstOrDefault();
        }
    }
}
