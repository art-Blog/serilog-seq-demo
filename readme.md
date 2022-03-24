## 使用 docker 建立 seq 服務

執行 `docker-compose`，建立 seq 服務

```shell
docker-compose up -d
```

預設 seq 網址為 `http://localhost:8090/`

## 使用 user secret 紀錄連線字串

[link01]:https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows

[link02]:https://plugins.jetbrains.com/plugin/10183--net-core-user-secrets

1. 連線字串請參考 [Safe storage of app secrets in development in ASP.NET Core][link01] 進行設定
2. Rider 使用者可安裝套件[.NET Core User Secrets][link02] 後專案右鍵 -> Tools -> Open Project User Secrets

測試資料庫需要自行建立，可滿足下列 SQL 語法即可

```sql
select a.AdminID as 'Id', a.Account, a.XingMing as 'Name', a.Authority
from admins as a
```