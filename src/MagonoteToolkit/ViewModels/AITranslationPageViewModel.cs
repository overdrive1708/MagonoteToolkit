using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagonoteToolkit.ViewModels
{
    internal partial class AITranslationPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// 翻訳入力言語リスト
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _translationInputLanguageList = [];

        /// <summary>
        /// 翻訳入力言語
        /// </summary>
        [ObservableProperty]
        private string _translationInputLanguage = string.Empty;

        /// <summary>
        /// 翻訳出力言語リスト
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _translationOutputLanguageList = [];

        /// <summary>
        /// 翻訳出力言語
        /// </summary>
        [ObservableProperty]
        private string _translationOutputLanguage = string.Empty;

        /// <summary>
        /// 翻訳入力テキスト
        /// </summary>
        [ObservableProperty]
        private string _translationInputText = string.Empty;

        /// <summary>
        /// 翻訳出力テキスト
        /// </summary>
        [ObservableProperty]
        private string _translationOutputText = string.Empty;

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
        /// 翻訳実施中フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isOperationEnable = true;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// 翻訳実施
        /// </summary>
        [RelayCommand]
        private void Translation() => ExecuteCommandTranslation();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AITranslationPageViewModel()
        {
            // 翻訳言語リストの初期化
            TranslationInputLanguageList = [Resources.Strings.English, Resources.Strings.Japanese];
            TranslationOutputLanguageList = [Resources.Strings.English, Resources.Strings.Japanese];

            // デフォルトの翻訳言語を設定
            TranslationInputLanguage = Resources.Strings.English;
            TranslationOutputLanguage = Resources.Strings.Japanese;

            // モデルリストの生成
            LoadModels();
        }

        /// <summary>
        /// 翻訳実施コマンド実行処理
        /// </summary>
        private async void ExecuteCommandTranslation()
        {
            // 翻訳前処理
            IsOperationEnable = false;
            TranslationOutputText = Resources.Strings.MessageStatusNowTranslation;

            // 翻訳処理
            await Task.Run(() =>
            {
                TranslationOutputText = AITranslator.Translate(TranslationInputLanguage, TranslationOutputLanguage, TranslationInputText, SelectedModel);
            });

            // 翻訳後処理
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
                ModelList = await Task.Run(AITranslator.GetModels);
            }
            SelectedModel = ModelList.FirstOrDefault();
        }
    }
}
