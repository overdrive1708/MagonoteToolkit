using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagonoteToolkit.Models;
using System.Collections.Generic;

namespace MagonoteToolkit.ViewModels
{
    internal partial class StringCharcodeConvertPageViewModel : ObservableObject
    {
        //--------------------------------------------------
        // バインディングデータ
        //--------------------------------------------------
        /// <summary>
        /// 文字コードのリスト
        /// </summary>
        [ObservableProperty]
        private Dictionary<StringCharcodeConverter.CharcodeType, string> _charcodeTypeList = new()
        {
            { StringCharcodeConverter.CharcodeType.ASCII, "ASCII" },
            { StringCharcodeConverter.CharcodeType.ShiftJIS, "Shift-JIS" },
            { StringCharcodeConverter.CharcodeType.Utf8, "UTF-8" }
        };

        /// <summary>
        /// 選択中の文字コード
        /// </summary>
        [ObservableProperty]
        private StringCharcodeConverter.CharcodeType _selectedCharcodeType;

        /// <summary>
        /// 区切り文字のリスト
        /// </summary>
        [ObservableProperty]
        private Dictionary<StringCharcodeConverter.SeparatorType, string> _separatorTypeList = new()
        {
            { StringCharcodeConverter.SeparatorType.Space, "Space" },
            { StringCharcodeConverter.SeparatorType.Comma, "Comma" },
            { StringCharcodeConverter.SeparatorType.Hyphen, "Hyphen" }
        };

        /// <summary>
        /// 選択中の区切り文字
        /// </summary>
        [ObservableProperty]
        private StringCharcodeConverter.SeparatorType _selectedSeparatorType;

        /// <summary>
        /// 文字列
        /// </summary>
        [ObservableProperty]
        private string _convertString;

        /// <summary>
        /// 文字コード
        /// </summary>
        [ObservableProperty]
        private string _convertCharCode;

        //--------------------------------------------------
        // バインディングコマンド
        //--------------------------------------------------
        /// <summary>
        /// 文字列->文字コード変換
        /// </summary>
        [RelayCommand]
        private void ConvertStringToCharcode() => ExecuteCommandConvertStringToCharcode();

        /// <summary>
        /// 文字コード->文字列変換
        /// </summary>
        [RelayCommand]
        private void ConvertCharcodeToString() => ExecuteCommandConvertCharcodeToString();

        //--------------------------------------------------
        // メソッド
        //--------------------------------------------------
        /// <summary>
        /// 文字列->文字コード変換コマンド実行処理
        /// </summary>
        private void ExecuteCommandConvertStringToCharcode()
        {
            ConvertCharCode = StringCharcodeConverter.ConvertStringToCharcode(ConvertString, SelectedCharcodeType, SelectedSeparatorType);
        }

        /// <summary>
        /// 文字コード->文字列変換コマンド実行処理
        /// </summary>
        private void ExecuteCommandConvertCharcodeToString()
        {
            ConvertString = StringCharcodeConverter.ConvertCharcodeToString(ConvertCharCode, SelectedCharcodeType, SelectedSeparatorType);
        }

    }
}
