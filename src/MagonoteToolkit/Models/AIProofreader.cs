using OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// AI校正クラス
    /// </summary>
    internal class AIProofreader : AIControllerBase
    {
        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 校正処理
        /// </summary>
        /// <param name="inputText">入力テキスト</param>
        /// <param name="model">使用するモデル</param>
        /// <returns>校正結果</returns>
        public static string Proofread(string inputText, string model)
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
                string promptTemplate = System.IO.File.ReadAllText(ApplicationSettings.ReadSettingsAIProofreadingPromptFilePath());
                prompt = string.Format(promptTemplate, inputText);
            }
            catch (Exception)
            {
                // ファイルの読み込みに失敗した場合は、デフォルトのプロンプトを使用
                prompt = string.Format(Resources.Strings.AIProofreadingDefaultPrompt, inputText);
            }

            // リクエストしてレスポンスを校正結果として採用
            string proofreadingResult;
            try
            {
                ChatCompletion completion = chatClient.CompleteChat(prompt);
                proofreadingResult = completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                proofreadingResult = $"{Resources.Strings.Error}:{ex.Message}";
            }

            return proofreadingResult;
        }   
    }
}
