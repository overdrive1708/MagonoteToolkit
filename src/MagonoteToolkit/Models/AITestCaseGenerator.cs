using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.ClientModel;
using System.Collections.ObjectModel;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// AIテストケース生成クラス
    /// </summary>
    internal class AITestCaseGenerator
    {
        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// AIテストケース生成処理
        /// </summary>
        /// <param name="inputSourceCode">入力ソースコード</param>
        /// <param name="model">使用するモデル</param>
        /// <returns>生成結果</returns>
        public static string Generate(string inputSourceCode, string model)
        {
            // ｢使用するモデル｣が空の場合は、ダミー値を設定する｡(OpenAI SDKのエラー回避)
            if (model == string.Empty)
            {
                model = "dummy_model";
            }

            // OpenAI APIの設定
            OpenAIClientOptions options = new() { Endpoint = new Uri(ApplicationSettings.ReadSettingsAIOpenAIAPIBaseUrl()) };
            OpenAIClient client = new(new ApiKeyCredential(ApplicationSettings.ReadSettingsAIOpenAIAPIKey()), options);
            ChatClient chatClient = client.GetChatClient(model);

            // プロンプトの作成
            string prompt;
            try
            {
                // ファイルの読み込みに成功した場合は、プロンプトファイルの内容を使用
                string promptTemplate = System.IO.File.ReadAllText(ApplicationSettings.ReadSettingsAITestCaseGenerationPromptFilePath());
                prompt = string.Format(promptTemplate, inputSourceCode);
            }
            catch (Exception)
            {
                // ファイルの読み込みに失敗した場合は、デフォルトのプロンプトを使用
                prompt = string.Format(Resources.Strings.AITestCaseGenerationDefaultPrompt, inputSourceCode);
            }

            // リクエストしてレスポンスをテストケース生成結果として採用
            string testCaseGenerationResult;
            try
            {
                ChatCompletion completion = chatClient.CompleteChat(prompt);
                testCaseGenerationResult = completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                testCaseGenerationResult = $"{Resources.Strings.Error}:{ex.Message}";
            }
            return testCaseGenerationResult;
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
