[日本語](README.md)

<h1 align="center">
    <a href="https://github.com/overdrive1708/MagonoteToolkit">
        <img alt="MagonoteToolkit" src="asetts/ApplicationIcon.png" width="50" height="50">
    </a><br>
    MagonoteToolkit
</h1>

<h2 align="center">
    かゆいところに手が届く!!<br>
    ソフトウェア開発支援ツールキット
</h2>

<div align="center">
    <img alt="csharp" src="https://img.shields.io/badge/csharp-blue.svg?style=plastic&logo=csharp">
    <img alt="dotnet" src="https://img.shields.io/badge/.NET-blue.svg?style=plastic&logo=dotnet">
    <img alt="license" src="https://img.shields.io/github/license/overdrive1708/MagonoteToolkit?style=plastic">
    <br>
    <img alt="repo size" src="https://img.shields.io/github/repo-size/overdrive1708/MagonoteToolkit?style=plastic&logo=github">
    <img alt="release" src="https://img.shields.io/github/release/overdrive1708/MagonoteToolkit?style=plastic&logo=github">
    <img alt="download" src="https://img.shields.io/github/downloads/overdrive1708/MagonoteToolkit/total?style=plastic&logo=github&color=brightgreen">
    <img alt="open issues" src="https://img.shields.io/github/issues-raw/overdrive1708/MagonoteToolkit?style=plastic&logo=github&color=brightgreen">
    <img alt="closed issues" src="https://img.shields.io/github/issues-closed-raw/overdrive1708/MagonoteToolkit?style=plastic&logo=github&color=brightgreen">
</div>

---

## 機能一覧
- Excelファイル検査

## ダウンロード方法
- [GitHubのReleases](https://github.com/overdrive1708/MagonoteToolkit/releases)にあるLatestのAssetsより
MagonoteToolkit_vx.x.x.zipをダウンロードしてください｡

## 初回セットアップ方法

### 全体設定

設定が必要な画面もしくは設定画面を初めて表示すると､｢ApplicationSettings.json｣が生成されます｡  
デフォルト設定でも動作しますが､必要に応じて設定を変更してください｡  

｢ApplicationSettings.json｣で設定する設定項目は以下の通りです｡
| 設定項目 | 設定内容 |
| --- | --- |
| ExcelFileInspectionSettingsFilePath | Excelファイル検査で使用する設定ファイルのパスを設定します｡ |

### Excelファイル検査
[設定サンプル](asetts/SampleSettings/ExcelFileInspectionSettings.json)を｢MagonoteToolkit.exe｣と同じ場所に格納してください｡  
(全体設定を変更することで格納場所を変更することが可能です｡)  
その後､下記を参考に設定してください｡  

｢ExcelFileInspectionSettings.json｣で設定する設定項目は以下のとおりです｡
| 設定項目 | 設定内容 |
| --- | --- |
| PresetName | プリセットを識別するための名前を設定します｡ |
| TargetFileKeyword | 検査ファイルの一覧に登録する際のキーワードを設定します｡<br>検査ファイルに検査対象フォルダもしくは検査対象ファイルをドラッグ&ドロップした際に､設定したキーワードを含むファイルのみを検査対象とします｡ |
| InspectionMethods | Conditionによって設定値が異なるため､後述する説明を参照してください｡ |

｢ExcelFileInspectionSettings.json｣で設定するConditionは以下をサポートしています｡
| Condition | 設定時の挙動 |
|---|---|
| Equal | SheetNameで設定したシート名の､Cellで設定したセルが､Valueで設定した値である場合にNGとします｡ |
| NotEqual | SheetNameで設定したシート名の､Cellで設定したセルが､Valueで設定した値以外である場合にNGとします｡ |
| Empty | SheetNameで設定したシート名の､Cellで設定したセルが､空である場合にNGとします｡ |
| NotEmpty | SheetNameで設定したシート名の､Cellで設定したセルが､空ではない場合にNGとします｡ |

## 開発環境
- Microsoft Visual Studio Community 2022

## 使用しているライブラリ
詳細は[NOTICE.md](NOTICE.md)を参照してください｡

## ライセンス
このプロジェクトはMITライセンスです。  
詳細は [LICENSE](LICENSE) を参照してください。

## 不具合報告と機能要望
[GitHubのIssue](https://github.com/overdrive1708/MagonoteToolkit/issues)より報告してください｡

## 作者
[overdrive1708](https://github.com/overdrive1708)
