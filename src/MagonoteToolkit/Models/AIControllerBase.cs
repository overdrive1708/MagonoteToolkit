using OpenAI;
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
    }
}
