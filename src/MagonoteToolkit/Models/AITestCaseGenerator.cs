using System;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// AIテストケース生成クラス
    /// </summary>
    internal class AITestCaseGenerator : AIControllerBase
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
            return Chat(prompt, model);
        }
    }
}
