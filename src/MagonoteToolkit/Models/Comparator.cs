using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using static MagonoteToolkit.Models.Messages;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// 比較クラス
    /// </summary>
    internal class Comparator
    {
        /// <summary>
        /// リスト項目比較結果
        /// </summary>
        public class CompareListItemsResult
        {
            /// <summary>
            /// 追加リストアイテム
            /// </summary>
            public string AddListItems { get; set; } = string.Empty;

            /// <summary>
            /// 削除リストアイテム
            /// </summary>
            public string DeleteListItems { get; set; } = string.Empty;
        }

        /// <summary>
        /// リスト項目の比較
        /// </summary>
        /// <param name="beforeListItems">変更前リストアイテム</param>
        /// <param name="afterListItems">変更後リストアイテム</param>
        /// <returns>比較結果</returns>
        public static CompareListItemsResult CompareListItems(string beforeListItems, string afterListItems)
        {
            int progressMaximum;
            int progressValue;
            CompareListItemsResult result = new();

            SendProgressInfoChangeMessageNowComparisonStart();

            // 改行コードで分割してリスト化
            HashSet<string> beforeList = [.. beforeListItems.Split(["\r\n", "\r", "\n"], StringSplitOptions.None)];
            HashSet<string> afterList = [.. afterListItems.Split(["\r\n", "\r", "\n"], StringSplitOptions.None)];

            // 追加リストアイテムの取得
            progressMaximum = afterList.Count;
            progressValue = 0;
            List<string> addList = [];
            foreach (string item in afterList)
            {
                if (!beforeList.Contains(item))
                {
                    addList.Add(item);
                }
                progressValue++;
                SendProgressInfoChangeMessageNowComparisonAdd(progressMaximum, progressValue);
            }
            result.AddListItems = string.Join(Environment.NewLine, addList);

            // 削除リストアイテムの取得
            progressMaximum = beforeList.Count;
            progressValue = 0;
            List<string> deleteList = [];
            foreach (string item in beforeList)
            {
                if (!afterList.Contains(item))
                {
                    deleteList.Add(item);
                }
                progressValue++;
                SendProgressInfoChangeMessageNowComparisonDelete(progressMaximum, progressValue);
            }
            result.DeleteListItems = string.Join(Environment.NewLine, deleteList);

            SendProgressInfoChangeMessageNowComparisonEnd();

            return result;
        }

        private static void SendProgressInfoChangeMessageNowComparisonStart()
        {
            ProgressInfoChangeMessage message = new()
            {
                ProgressMaximum = 1,
                ProgressValue = 0,
                IsProgressIndeterminate = true,
                ProgressMessage = Resources.Strings.MessageStatusNowProcessing
            };
            _ = WeakReferenceMessenger.Default.Send(message);
        }

        private static void SendProgressInfoChangeMessageNowComparisonAdd(int progressMaximum, int progressValue)
        {
            ProgressInfoChangeMessage message = new()
            {
                ProgressMaximum = progressMaximum,
                ProgressValue = progressValue,
                IsProgressIndeterminate = false,
                ProgressMessage = string.Format(Resources.Strings.MessageStatusNowComparisonAdd, progressValue, progressMaximum)
            };
            _ = WeakReferenceMessenger.Default.Send(message);
        }

        private static void SendProgressInfoChangeMessageNowComparisonDelete(int progressMaximum, int progressValue)
        {
            ProgressInfoChangeMessage message = new()
            {
                ProgressMaximum = progressMaximum,
                ProgressValue = progressValue,
                IsProgressIndeterminate = false,
                ProgressMessage = string.Format(Resources.Strings.MessageStatusNowComparisonDelete, progressValue, progressMaximum)
            };
            _ = WeakReferenceMessenger.Default.Send(message);
        }

        private static void SendProgressInfoChangeMessageNowComparisonEnd()
        {
            ProgressInfoChangeMessage message = new()
            {
                ProgressMaximum = 1,
                ProgressValue = 0,
                IsProgressIndeterminate = false,
                ProgressMessage = Resources.Strings.MessageStatusCompleteComparison
            };
            _ = WeakReferenceMessenger.Default.Send(message);
        }
    }
}
