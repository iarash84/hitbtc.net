# HitBTC.Net Wiki

[English](#english) · [فارسی](#فارسی)

## English

### Overview

HitBTC.Net is an unofficial C# client for HitBTC API v3. It targets .NET Framework 4.8 and supports public market data, spot trading, wallet operations, trading history, and public/authenticated WebSocket commands.

### Project layout

| Project | Description |
| --- | --- |
| `HitBtc` | Main class library and API models |
| `HitBtc.Tests` | Deterministic xUnit test suite |
| `Test` | Interactive WinForms demo |

### Build and test

```powershell
nuget restore Hitbtc.sln
dotnet build Hitbtc.sln --configuration Release --no-restore
dotnet test HitBtc.Tests/HitBtc.Tests.csproj --configuration Release --no-build --no-restore
```

The primary output is `HitBtc/bin/Release/Hitbtc.dll`.

### REST quick start

```csharp
using Hitbtc;

var api = new HitBtcRestApi();
var ticker = await api.PublicData.GetTicker("BTCUSDT");
```

Private endpoints require authorization:

```csharp
api.Authorize(apiKey, secretKey);
var balances = await api.Trading.GetBalance();
```

Never hard-code credentials. Read them from environment variables or a secure secret store.
REST failures throw `HitBtcApiException`; HTTP status and exchange error code are
available when supplied by the server.

### WebSocket quick start

```csharp
using (var socket = new HitBtcSocketApi())
{
    await socket.MarketData.SubscribeTicker("BTCUSDT");
}
```

API v3 uses separate public and trading WebSocket endpoints. Currency, symbol, and historical-trade lookups must use the REST client.
Malformed responses, exchange errors, and mismatched response IDs throw
`HitBtcWebSocketException`. Subscription methods currently return only the
acknowledgement and do not expose a continuous high-level notification stream.

### API v3 migration

- REST resources now use `/api/3/public`, `/api/3/spot`, and `/api/3/wallet`.
- JSON fields and parameters use snake_case.
- Public collections can be dictionaries keyed by symbol or currency.
- Order-book entries are `[price, size]` arrays.
- WebSocket subscriptions use channels such as `ticker/1s`.
- Wallet transfers specify `source` and `destination` accounts.

### Demo credentials

```powershell
$env:HITBTC_API_KEY = "your-api-key"
$env:HITBTC_SECRET_KEY = "your-secret-key"
dotnet run --project Test/Test.csproj
```

The demo is a read-only verification console with grouped REST/WebSocket actions,
masked credentials, a result grid, operation status, and a timestamped activity
log. It never submits orders, transfers, or withdrawals.

### Releases

Merge changes through a pull request to the protected `master` branch. After CI succeeds, create and push a semantic version tag:

```powershell
git tag -a v1.1.0 -m "Release v1.1.0"
git push origin v1.1.0
```

The GitHub Actions workflow builds and tests the tagged commit, creates a ZIP package, and attaches it to a GitHub Release.

### License

HitBTC.Net is distributed under the [MIT License](https://github.com/iarash84/hitbtc.net/blob/master/LICENSE). It can be used,
modified, and redistributed as long as the copyright and permission notices are
retained. The software is provided without warranty.

---

<div dir="rtl" align="right">

## فارسی

### معرفی

HitBTC.Net یک کلاینت غیررسمی C# برای API نسخهٔ ۳ صرافی HitBTC است. پروژه بر پایهٔ .NET Framework 4.8 ساخته شده و داده‌های عمومی بازار، معاملات اسپات، کیف پول، تاریخچهٔ معاملات و WebSocket عمومی و خصوصی را پوشش می‌دهد.

### ساختار پروژه

| پروژه | توضیح |
| --- | --- |
| `HitBtc` | کتابخانهٔ اصلی و مدل‌های API |
| `HitBtc.Tests` | تست‌های قطعی xUnit |
| `Test` | برنامهٔ نمایشی WinForms |

### بیلد و تست

</div>

```powershell
nuget restore Hitbtc.sln
dotnet build Hitbtc.sln --configuration Release --no-restore
dotnet test HitBtc.Tests/HitBtc.Tests.csproj --configuration Release --no-build --no-restore
```

<div dir="rtl" align="right">

خروجی اصلی در مسیر `HitBtc/bin/Release/Hitbtc.dll` ساخته می‌شود.

### شروع سریع REST

</div>

```csharp
using Hitbtc;

var api = new HitBtcRestApi();
var ticker = await api.PublicData.GetTicker("BTCUSDT");

api.Authorize(apiKey, secretKey);
var balances = await api.Trading.GetBalance();
```

<div dir="rtl" align="right">

کلیدهای API را داخل کد قرار ندهید. آن‌ها را از متغیر محیطی یا یک مخزن امن اسرار دریافت کنید.

خطاهای REST با `HitBtcApiException` گزارش می‌شوند و در صورت موجود بودن، وضعیت HTTP و کد خطای صرافی قابل دسترسی است.

### شروع سریع WebSocket

</div>

```csharp
using (var socket = new HitBtcSocketApi())
{
    await socket.MarketData.SubscribeTicker("BTCUSDT");
}
```

<div dir="rtl" align="right">

نسخهٔ ۳ برای داده‌های عمومی و عملیات معاملاتی از endpointهای WebSocket جداگانه استفاده می‌کند. دریافت ارزها، نمادها و تاریخچهٔ معامله باید از طریق REST انجام شود.

پاسخ خراب، خطای صرافی یا شناسهٔ پاسخ نامنطبق با `HitBtcWebSocketException` گزارش می‌شود. متدهای اشتراک فعلی فقط acknowledgement را برمی‌گردانند و جریان سطح‌بالای notification هنوز پیاده‌سازی نشده است.

### مهاجرت به API نسخهٔ ۳

- مسیرها به گروه‌های `/api/3/public`، `/api/3/spot` و `/api/3/wallet` منتقل شده‌اند.
- فیلدها و پارامترهای JSON از الگوی snake_case استفاده می‌کنند.
- مجموعه‌های عمومی ممکن است با نام نماد یا ارز کلیدگذاری شده باشند.
- هر ردیف order book به شکل آرایهٔ `[price, size]` برمی‌گردد.
- اشتراک WebSocket با کانال‌هایی مانند `ticker/1s` انجام می‌شود.
- انتقال کیف پول دارای حساب‌های `source` و `destination` است.

### اعتبارنامه‌های برنامهٔ نمونه

</div>

```powershell
$env:HITBTC_API_KEY = "your-api-key"
$env:HITBTC_SECRET_KEY = "your-secret-key"
dotnet run --project Test/Test.csproj
```

<div dir="rtl" align="right">

برنامهٔ نمونه یک کنسول تست فقط‌خواندنی با عملیات دسته‌بندی‌شدهٔ REST و WebSocket، اعتبارنامهٔ مخفی، جدول نتایج، وضعیت عملیات و لاگ زمان‌دار است. این برنامه هیچ سفارش، انتقال یا برداشتی ثبت نمی‌کند.

</div>

<div dir="rtl" align="right">

### انتشار نسخهٔ جدید

تغییرات را با Pull Request در شاخهٔ محافظت‌شدهٔ `master` ادغام کنید. پس از موفقیت CI یک تگ نسخهٔ معنایی بسازید و push کنید:

</div>

```powershell
git tag -a v1.1.0 -m "Release v1.1.0"
git push origin v1.1.0
```

<div dir="rtl" align="right">

GitHub Actions کد همان تگ را بیلد و تست می‌کند، فایل ZIP می‌سازد و آن را به GitHub Release متصل می‌کند.

### امنیت و مشارکت

- کلید یا رمز API را commit نکنید.
- تست‌های معمول نباید به سرویس زنده وابسته باشند.
- برای رفع هر باگ تست بازگشتی اضافه کنید.
- قبل از ارسال Pull Request بیلد Release و تست‌ها را اجرا کنید.

### مجوز

HitBTC.Net تحت [مجوز MIT](https://github.com/iarash84/hitbtc.net/blob/master/LICENSE) منتشر می‌شود. استفاده، تغییر و بازتوزیع آن با حفظ اعلان حق نشر و متن مجوز مجاز است. نرم‌افزار بدون ضمانت ارائه می‌شود.

</div>
