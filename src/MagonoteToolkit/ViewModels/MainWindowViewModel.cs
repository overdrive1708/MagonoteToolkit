using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

namespace MagonoteToolkit.ViewModels
{
    internal partial class MainWindowViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// タイトル
        /// </summary>
        [ObservableProperty]
        private string _title;

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            Assembly assm = Assembly.GetExecutingAssembly();

            // バージョン情報を取得してタイトルに反映する
            string version = assm.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            Title = $"MagonoteToolkit Ver.{version}";
        }
    }
}
