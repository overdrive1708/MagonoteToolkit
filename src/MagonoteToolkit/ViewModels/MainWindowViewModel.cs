using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Reflection;
using static MagonoteToolkit.Models.Messages;

namespace MagonoteToolkit.ViewModels
{
    internal partial class MainWindowViewModel : ObservableObject, IDisposable
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// タイトル
        /// </summary>
        [ObservableProperty]
        private string _title;

        /// <summary>
        /// プログレスバー最大値
        /// </summary>
        [ObservableProperty]
        private int _progressMaximum = 1;

        /// <summary>
        /// プログレスバー現在値
        /// </summary>
        [ObservableProperty]
        private int _progressValue = 0;

        /// <summary>
        /// プログレスバー進捗不定フラグ
        /// </summary>
        [ObservableProperty]
        private bool _isProgressIndeterminate = false;

        /// <summary>
        /// 進捗メッセージ
        /// </summary>
        [ObservableProperty]
        private string _progressMessage = string.Empty;

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

            // メッセージの登録
            WeakReferenceMessenger.Default.Register<ProgressInfoChangeMessage>(this, ReceiveProgressInfoChangeMessage);
        }

        /// <summary>
        /// Disposeメソッド
        /// </summary>
        public void Dispose()
        {
            // メッセージの登録解除
            WeakReferenceMessenger.Default.Unregister<ProgressInfoChangeMessage>(this);
        }

        /// <summary>
        /// 進捗情報変更メッセージ受信処理
        /// </summary>
        /// <param name="recipient">受信者</param>
        /// <param name="message">メッセージ</param>
        public void ReceiveProgressInfoChangeMessage(object recipient, ProgressInfoChangeMessage message)
        {
            // 進捗情報をUIに反映する
            ProgressMaximum = message.ProgressMaximum;
            ProgressValue = message.ProgressValue;
            IsProgressIndeterminate = message.IsProgressIndeterminate;
            ProgressMessage = message.ProgressMessage;
        }
    }
}
