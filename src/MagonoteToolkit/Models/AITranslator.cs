using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.ClientModel;
using System.Collections.ObjectModel;

namespace MagonoteToolkit.Models
{
    internal class AITranslator
    {
        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 翻訳処理
        /// </summary>
        /// <param name="inputLanguage">入力言語</param>
        /// <param name="outputLanguage">出力言語</param>
        /// <param name="inputText">入力テキスト</param>
        /// <param name="model">使用するモデル</param>
        /// <returns>翻訳結果</returns>
        public static string Translate(string inputLanguage, string outputLanguage, string inputText, string model)
        {
            // OpenAI APIの設定
            OpenAIClientOptions options = new() { Endpoint = new Uri(ApplicationSettings.ReadSettingsAIOpenAIAPIBaseUrl()) };
            OpenAIClient client = new(new ApiKeyCredential(ApplicationSettings.ReadSettingsAIOpenAIAPIKey()), options);
            ChatClient chatClient = client.GetChatClient(model);

            // プロンプトの作成
            string prompt = string.Format(Resources.Strings.AITranslationPrompt, inputLanguage, outputLanguage, inputText);

            // リクエストしてレスポンスを翻訳結果として採用
            string translationResult;
            try
            {
                ChatCompletion completion = chatClient.CompleteChat(prompt);
                translationResult = completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                translationResult = $"{Resources.Strings.Error}:{ex.Message}";
            }

            return translationResult;
        }

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
