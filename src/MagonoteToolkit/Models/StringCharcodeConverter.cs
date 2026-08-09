using System;
using System.Text;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// 文字列<-->文字コード変換クラス
    /// </summary>
    internal class StringCharcodeConverter
    {
        /// <summary>
        /// 文字コードの種類
        /// </summary>
        public enum CharcodeType
        {
            ASCII,      // ASCII
            ShiftJIS,   // Shift-JIS
            Utf8        // UTF-8
        }

        /// <summary>
        /// 区切り文字の種類
        /// </summary>
        public enum SeparatorType
        {
            Space,      // スペース( )
            Comma,      // カンマ(,)
            Hyphen      // ハイフン(-)
        }

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 文字列->文字コード変換
        /// </summary>
        /// <param name="inputString">入力文字列</param>
        /// <param name="charcodeType">文字コードの種類</param>
        /// <param name="separatorType">区切り文字の種類</param>
        /// <returns>変換結果</returns>
        public static string ConvertStringToCharcode(string inputString, CharcodeType charcodeType, SeparatorType separatorType)
        {
            try
            {
                // 文字列を文字コードのバイト配列に変換
                byte[] bytes = charcodeType switch
                {
                    CharcodeType.ASCII => Encoding.ASCII.GetBytes(inputString),
                    CharcodeType.ShiftJIS => Encoding.GetEncoding("Shift-JIS").GetBytes(inputString),
                    CharcodeType.Utf8 => Encoding.UTF8.GetBytes(inputString),
                    _ => throw new ArgumentException("Invalid charcode type"),
                };

                // バイト配列をハイフン区切りの16進数の文字列に変換
                string hexString = BitConverter.ToString(bytes);

                // ハイフンを指定された区切り文字に置換して返す
                return hexString.Replace("-", separatorType switch
                {
                    SeparatorType.Space => " ",
                    SeparatorType.Comma => ",",
                    SeparatorType.Hyphen => "-",
                    _ => throw new ArgumentException("Invalid separator type"),
                });
            }
            catch (Exception ex)
            {
                return $"{Resources.Strings.Error}:{ex.Message}";
            }
        }

        /// <summary>
        /// 文字コード->文字列変換
        /// </summary>
        /// <param name="inputString">入力文字列</param>
        /// <param name="charcodeType">文字コードの種類</param>
        /// <param name="separatorType">区切り文字の種類</param>
        /// <returns>変換結果</returns>
        public static string ConvertCharcodeToString(string inputString, CharcodeType charcodeType, SeparatorType separatorType)
        {
            try
            {
                // 指定された区切り文字で区切られた16進数の文字列をバイト配列に変換
                string[] hexValues = inputString.Split(separatorType switch
                {
                    SeparatorType.Space => " ",
                    SeparatorType.Comma => ",",
                    SeparatorType.Hyphen => "-",
                    _ => throw new ArgumentException("Invalid separator type"),
                });
                byte[] bytes = new byte[hexValues.Length];

                // 16進数の文字列を1つずつバイト配列に格納
                for (int charCnt = 0; charCnt < hexValues.Length; charCnt++)
                {
                    bytes[charCnt] = Convert.ToByte(hexValues[charCnt], 16);
                }

                // バイト配列を文字列に変換して返す
                return charcodeType switch
                {
                    CharcodeType.ASCII => Encoding.ASCII.GetString(bytes),
                    CharcodeType.ShiftJIS => Encoding.GetEncoding("Shift-JIS").GetString(bytes),
                    CharcodeType.Utf8 => Encoding.UTF8.GetString(bytes),
                    _ => throw new ArgumentException("Invalid charcode type"),
                };
            }
            catch (Exception ex)
            {
                return $"{Resources.Strings.Error}:{ex.Message}";
            }
        }
    }
}
