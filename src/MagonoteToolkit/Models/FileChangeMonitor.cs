using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// ファイル変更監視クラス
    /// </summary>
    internal class FileChangeMonitor
    {
        /// <summary>
        /// ファイル変更監視結果クラス
        /// </summary>
        public class MonitorResult
        {
            /// <summary>
            /// チェック結果
            /// </summary>
            public string CheckResult { get; set; } = string.Empty;

            /// <summary>
            /// ファイル
            /// </summary>
            public string File { get; set; } = string.Empty;

            /// <summary>
            /// チェック済みのタイムスタンプ
            /// </summary>
            public string CheckedTimestamp { get; set; } = string.Empty;

            /// <summary>
            /// 現在のタイムスタンプ
            /// </summary>
            public string CurrentTimestamp { get; set; } = string.Empty;

            /// <summary>
            /// メモ
            /// </summary>
            public string Memo { get; set; } = string.Empty;
        }

        //--------------------------------------------------
        // 定数(コンフィギュレーション)
        //--------------------------------------------------
        /// <summary>
        /// SQLコマンド(テーブル作成)
        /// </summary>
        private static readonly string _createTableCommand = "CREATE TABLE IF NOT EXISTS FileChangeMonitorInfo("
                                                             + "File TEXT PRIMARY KEY, "
                                                             + "CheckedTimestamp TEXT, "
                                                             + "Memo TEXT)";

        /// <summary>
        /// SQLコマンド(レコード登録)
        /// </summary>
        private static readonly string _insertCommand = "INSERT OR REPLACE INTO FileChangeMonitorInfo"
                                                        + "(File, CheckedTimestamp, Memo) "
                                                        + "VALUES(@p_File, @p_CheckedTimestamp, @p_Memo)";

        /// <summary>
        /// SQLコマンド(レコード取得)
        /// </summary>
        private static readonly string _selectCommand = "SELECT * FROM FileChangeMonitorInfo";

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// ワークスペース追加処理
        /// </summary>
        /// <param name="workspaceName">ワークスペース名(拡張子無し)</param>
        public static void AddWorkspace(string workspaceName)
        {
            // パス作成
            string workspaceDirectory = ApplicationSettings.ReadSettingsFileChangeMonitorWorkspaceDirectory();
            string workspacePath = Path.Combine(workspaceDirectory, workspaceName+".db");

            // ディレクトリがないなら作成
            if (!Directory.Exists(workspaceDirectory))
            {
                Directory.CreateDirectory(workspaceDirectory);
            }

            // DBファイル作成
            SQLiteConnection.CreateFile(workspacePath);
            using SQLiteConnection connection = new($"Data Source = {workspacePath}");
            connection.Open();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = _createTableCommand;
                _ = command.ExecuteNonQuery();
            }
            connection.Close();
        }

        /// <summary>
        /// ワークスペース削除処理
        /// </summary>
        /// <param name="workspaceName">ワークスペース名(拡張子有り)</param>
        public static void DeleteWorkspace(string workspaceName)
        {
            // パス作成
            string workspaceDirectory = ApplicationSettings.ReadSettingsFileChangeMonitorWorkspaceDirectory();
            string workspacePath = Path.Combine(workspaceDirectory, workspaceName);

            // ファイルが有るなら削除
            if (File.Exists(workspacePath))
            {
                File.Delete(workspacePath);
            }
        }

        /// <summary>
        /// ファイル追加処理
        /// </summary>
        /// <param name="workspaceName">ワークスペース名(拡張子有り)</param>
        /// <param name="filePath">追加ファイルパス</param>
        /// <param name="memo">メモ</param>
        public static void AddFile(string workspaceName, string filePath, string memo)
        {
            // パス作成
            if (workspaceName == null)
            {
                return;
            }
            string workspaceDirectory = ApplicationSettings.ReadSettingsFileChangeMonitorWorkspaceDirectory();
            string workspacePath = Path.Combine(workspaceDirectory, workspaceName);

            // DBファイルが有るなら書き込み
            if (File.Exists(workspacePath))
            {
                using SQLiteConnection connection = new($"Data Source = {workspacePath}");
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = _insertCommand;
                    _ = command.Parameters.Add(new SQLiteParameter("@p_File", filePath));
                    _ = command.Parameters.Add(new SQLiteParameter("@p_CheckedTimestamp", File.GetLastWriteTime(filePath).ToString()));
                    _ = command.Parameters.Add(new SQLiteParameter("@p_Memo", memo));
                    command.Prepare();
                    _ = command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        /// <summary>
        /// ファイルチェック処理
        /// </summary>
        /// <param name="workspaceName">ワークスペース名(拡張子有り)</param>
        /// <returns>チェック結果</returns>
        public static List<MonitorResult> CheckFile(string workspaceName)
        {
            List<MonitorResult> results = [];
            MonitorResult result;

            // パス作成
            if (workspaceName == null)
            {
                return results;
            }
            string workspaceDirectory = ApplicationSettings.ReadSettingsFileChangeMonitorWorkspaceDirectory();
            string workspacePath = Path.Combine(workspaceDirectory, workspaceName);

            // DBファイルが有るならチェック
            if (File.Exists(workspacePath))
            {
                using SQLiteConnection connection = new($"Data Source = {workspacePath}");
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = _selectCommand;
                    using var executeReader = command.ExecuteReader();
                    while (executeReader.Read())
                    {
                        result = new()
                        {
                            File = executeReader["File"].ToString(),
                            CheckedTimestamp = executeReader["CheckedTimestamp"].ToString(),
                            Memo = executeReader["Memo"].ToString(),
                        };
                        if (File.Exists(result.File))
                        {
                            result.CurrentTimestamp = File.GetLastWriteTime(result.File).ToString();
                        }
                        else
                        {
                            result.CurrentTimestamp = string.Empty;
                        }
                        result.CheckResult = JudgeTimestamp(result.CheckedTimestamp, result.CurrentTimestamp);
                        results.Add(result);
                    }
                }
                connection.Close();
            }

            return results;
        }

        /// <summary>
        /// タイムスタンプ判定処理
        /// </summary>
        /// <param name="checkedTimestamp">チェック済みのタイムスタンプ</param>
        /// <param name="currentTimestamp">現在のタイムスタンプ</param>
        /// <returns>判定結果</returns>
        private static string JudgeTimestamp(string checkedTimestamp, string currentTimestamp)
        {
            if (currentTimestamp == string.Empty)
            {
                return Resources.Strings.MessageResultFileChangeMonitorNGNotFound;
            }
            else if (checkedTimestamp != currentTimestamp)
            {
                return Resources.Strings.MessageResultFileChangeMonitorNGTimestampUnmatch;
            }
            else
            {
                return Resources.Strings.MessageResultFileChangeMonitorOK;
            }
        }
    }
}
