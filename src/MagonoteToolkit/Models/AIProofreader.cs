using System;

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
            return Chat(prompt, model);
        }   
    }
}
