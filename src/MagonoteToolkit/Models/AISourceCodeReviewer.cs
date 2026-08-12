using System;

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
            return Chat(prompt, model);
        }
    }
}
