using System;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// AI翻訳クラス
    /// </summary>
    internal class AITranslator : AIControllerBase
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
            // プロンプトの作成
            string prompt;
            try
            {
                // ファイルの読み込みに成功した場合は、プロンプトファイルの内容を使用
                string promptTemplate = System.IO.File.ReadAllText(ApplicationSettings.ReadSettingsAITranslationPromptFilePath());
                prompt = string.Format(promptTemplate, inputLanguage, outputLanguage, inputText);
            }
            catch (Exception)
            {
                // ファイルの読み込みに失敗した場合は、デフォルトのプロンプトを使用
                prompt = string.Format(Resources.Strings.AITranslationDefaultPrompt, inputLanguage, outputLanguage, inputText);
            }

            // リクエストしてレスポンスを翻訳結果として採用
            return Chat(prompt, model);
        }
    }
}
