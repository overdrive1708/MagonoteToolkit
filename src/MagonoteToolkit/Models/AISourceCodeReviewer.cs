using OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// AIソースコードレビュークラス
    /// </summary>
    internal class AISourceCodeReviewer : AIControllerBase
    {
        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// ソースコードレビュー処理
        /// </summary>
        /// <param name="inputSourceCode">入力ソースコード</param>
        /// <param name="model">使用するモデル</param>
        /// <returns>レビュー結果</returns>
        public static string SourceCodeReview(string inputSourceCode, string model)
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
                string promptTemplate = System.IO.File.ReadAllText(ApplicationSettings.ReadSettingsAISourceCodeReviewPromptFilePath());
                prompt = string.Format(promptTemplate, inputSourceCode);
            }
            catch (Exception)
            {
                // ファイルの読み込みに失敗した場合は、デフォルトのプロンプトを使用
                prompt = string.Format(Resources.Strings.AISourceCodeReviewDefaultPrompt, inputSourceCode);
            }

            // リクエストしてレスポンスをソースコードレビュー結果として採用
            string reviewResult;
            try
            {
                ChatCompletion completion = chatClient.CompleteChat(prompt);
                reviewResult = completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                reviewResult = $"{Resources.Strings.Error}:{ex.Message}";
            }

            return reviewResult;
        }
    }
}
