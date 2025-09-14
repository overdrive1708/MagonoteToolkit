using System.Windows;

namespace MagonoteToolkit
{
    /// <summary>
    /// ナビゲーション項目
    /// </summary>
    public enum NavigationItem
    {
        Home,                   // ホーム
        ExcelFileInspection,    // Excelファイル検査
        ExcelFileNumberToName,  // ExcelファイルID->名称変換
        Help                    // ヘルプ
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// NavigationView選択項目変更時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavigationView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            object header;
            System.Type sourcePageType;

            // 選択項目取得
            ModernWpf.Controls.NavigationViewItem selectedItem = (ModernWpf.Controls.NavigationViewItem)args.SelectedItem;

            // 切り替え先決定
            if (args.IsSettingsSelected)
            {
                header = MagonoteToolkit.Resources.Strings.Settings;
                sourcePageType = typeof(Views.SettingsPage);
            }
            else
            {
                switch (selectedItem.Tag)
                {
                    case NavigationItem.Home:
                        header = selectedItem.Content;
                        sourcePageType = typeof(Views.HomePage);
                        break;
                    case NavigationItem.ExcelFileInspection:
                        header = selectedItem.Content;
                        sourcePageType = typeof(Views.ExcelFileInspectionPage);
                        break;
                    case NavigationItem.ExcelFileNumberToName:
                        sourcePageType = typeof(Views.ExcelFileNumberToNamePage);
                        header = selectedItem.Content;
                        break;
                    case NavigationItem.Help:
                        header = selectedItem.Content;
                        sourcePageType = typeof(Views.HelpPage);
                        break;
                    default:
                        header = string.Empty;
                        sourcePageType = typeof(Views.HomePage);
                        break;
                }
            }

            // ヘッダ切り替え
            sender.Header = header;

            // コンテンツ切り替え
            ContentFrame.Navigate(sourcePageType);
        }
    }
}