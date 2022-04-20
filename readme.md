# Intro

關於專案

1. `/Admin`:練習 serilog 紀錄日誌
1. `/Message`: 練習`polly` 熔斷受影響的服務，這個目前沒有特別寫一篇文章出來，主要是還沒整理好，呼叫 `httpClient`，這部分是直接在 `program.cs`就註冊好斷路器，細節等以後有機會再寫

另外本專案最主要的目的是建立一個簡單的環境，支援 vue.js + webpack + .net6 core 的開發環境，也順便做了一個分層架構的示範，避免自己以後忘記也方便自己改來使用

## Blog

[使用 Serilog 和 Seq 紀錄 Log](https://partypeopleland.github.io/artblog/2022/03/23/logging-using-serilog-and-seq/)

## 使用 docker 建立 seq 服務

執行 `docker-compose`，建立 seq 服務

```shell
docker-compose up -d
```

預設 seq 網址為 `http://localhost:8090/`

## 使用 user secret 紀錄連線字串

[link01]: https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows
[link02]: https://plugins.jetbrains.com/plugin/10183--net-core-user-secrets

1. 連線字串請參考 [Safe storage of app secrets in development in ASP.NET Core][link01] 進行設定
2. Rider 使用者可安裝套件[.NET Core User Secrets][link02] 後專案右鍵 -> Tools -> Open Project User Secrets

目前連線到資料庫取資料那邊是直接回假資料，畢竟只是要用來測試看看 Seq 的內容與網頁行為而已
