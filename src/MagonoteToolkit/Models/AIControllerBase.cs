using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.ClientModel;
using System.Collections.ObjectModel;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// AIコントローラー基底クラス
    /// </summary>
    internal class AIControllerBase
    {
        /// <summary>
        /// モデルリスト取得処理
        /// </summary>
        /// <returns>モデルリスト</returns>
        public static ObservableCollection<string> GetModels()
        {
            // OpenAI APIの設定
            OpenAIClientOptions options = new() { Endpoint = new Uri(ApplicationSettings.ReadSettingsAIOpenAIAPIBaseUrl()) };
            OpenAIClient client = new(new ApiKeyCredential(ApplicationSettings.ReadSettingsAIOpenAIAPIKey()), options);
            OpenAIModelClient modelClient = client.GetOpenAIModelClient();

            // モデルリストを取得
            ObservableCollection<string> modelList = [];
            try
            {
                OpenAIModelCollection models = modelClient.GetModels();
                foreach (var model in models)
                {
                    modelList.Add(model.Id);
                }
            }
            catch (Exception)
            {
                // エラーが発生した場合は空のリストを返す
            }

            return modelList;
        }

        /// <summary>
        /// チャット処理
        /// </summary>
        /// <param name="prompt">プロンプト</param>
        /// <param name="model">使用するモデル</param>
        /// <returns>チャット結果</returns>
        public static string Chat(string prompt, string model)
        {
            // ｢使用するモデル｣が空の場合は、ダミー値を設定する｡(OpenAI SDKのエラー回避)
            if (model == string.Empty)
            {
                model = "dummy_model";
            }

            // ｢タイムアウト時間｣の設定読み込み(不正な場合は、デフォルト値を設定する｡)
            if (!TimeSpan.TryParse(ApplicationSettings.ReadSettingsAIOpenAIAPITimeoutTime(), out TimeSpan timeout))
            {
                timeout = TimeSpan.FromSeconds(100);
            }

            // OpenAI APIの設定
            OpenAIClientOptions options = new() { Endpoint = new Uri(ApplicationSettings.ReadSettingsAIOpenAIAPIBaseUrl()), NetworkTimeout = timeout };
            OpenAIClient client = new(new ApiKeyCredential(ApplicationSettings.ReadSettingsAIOpenAIAPIKey()), options);
            ChatClient chatClient = client.GetChatClient(model);

            // リクエストしてレスポンスを校正結果として採用
            string result;
            try
            {
                ChatCompletion completion = chatClient.CompleteChat(prompt);
                result = completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                result = $"{Resources.Strings.Error}:{ex.Message}";
            }

            return result;
        }
    }
}
