using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Reflection;

namespace MagonoteToolkit.ViewModels
{
    internal partial class HelpPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// 製品名
        /// </summary>
        [ObservableProperty]
        private string _productBody;

        /// <summary>
        /// ライセンス
        /// </summary>
        [ObservableProperty]
        private string _licenseBody;

        /// <summary>
        /// バージョン情報
        /// </summary>
        [ObservableProperty]
        private string _versionBody;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// URLを開く
        /// </summary>
        [RelayCommand]
        private void OpenUrl(string url) => ExecuteCommandOpenUrl(url);

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HelpPageViewModel()
        {
            Assembly assm = Assembly.GetExecutingAssembly();

            ProductBody = assm.GetCustomAttribute<AssemblyProductAttribute>().Product;
            LicenseBody = assm.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            VersionBody = assm.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }

        /// <summary>
        /// URLを開くコマンド実行処理
        /// </summary>
        /// <param name="url">URL</param>
        private void ExecuteCommandOpenUrl(string url)
        {
            ProcessStartInfo psi = new()
            {
                FileName = url,
                UseShellExecute = true,
            };
            Process.Start(psi);
        }
    }
}
