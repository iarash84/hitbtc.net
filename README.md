# HitBTC.Net

A C# client library for the HitBTC API v3 REST and WebSocket interfaces. The library targets
.NET Framework 4.8 and provides typed clients for public market data, account
operations, trading, trading history, and JSON-RPC WebSocket operations.

> The client has migrated from the deprecated API v2 contract to API v3. Existing
> applications should review the migration notes below before upgrading.

> This is an unofficial client library. Test trading and withdrawal operations
> carefully before using them with a funded account.

## Repository structure

| Path | Purpose |
| --- | --- |
| `HitBtc/` | The .NET Framework 4.8 class library that produces `Hitbtc.dll` |
| `HitBtc/HitBtcCategories/` | REST and WebSocket API operation groups |
| `HitBtc/HitBtcModel/` | Request and response models |
| `HitBtc.Tests/` | Deterministic xUnit tests that do not use the live HitBTC service |
| `Test/` | A WinForms demo application for optional manual testing |
| `.github/workflows/` | Automated build, test, artifact, and release workflow |

The `Test` project is a demo application, not the automated test suite. CI uses
`HitBtc.Tests` and never requires real API credentials.

## Requirements

- Windows with the .NET Framework 4.8 Developer Pack
- Visual Studio 2022, or a compatible .NET SDK and NuGet CLI
- Git, when contributing or creating a release

## Restore, build, and test

Clone the repository and restore its dependencies:

```powershell
git clone https://github.com/iarash84/hitbtc.net.git
cd hitbtc.net
nuget restore Hitbtc.sln
```

Build the complete solution:

```powershell
dotnet build Hitbtc.sln --configuration Release --no-restore
```

Run the automated tests:

```powershell
dotnet test HitBtc.Tests/HitBtc.Tests.csproj `
  --configuration Release `
  --no-build `
  --no-restore
```

The Release library is generated at:

```text
HitBtc/bin/Release/Hitbtc.dll
```

The DLL depends on the other assemblies copied into the same output directory.
When distributing the library manually, ship all required DLL files from that
directory rather than `Hitbtc.dll` alone.

## Using the REST API

Public endpoints do not require credentials:

```csharp
using Hitbtc;

var api = new HitBtcRestApi();
var symbols = await api.PublicData.GetSymbol();
var ticker = await api.PublicData.GetTicker("BTCUSDT");
var candles = await api.PublicData.GetCandles(
    "BTCUSDT",
    PublicEnum.EnPeriod.H4);
```

Account and trading endpoints require authorization:

```csharp
using Hitbtc;

var api = new HitBtcRestApi();
api.Authorize(apiKey, secretKey);

var balances = await api.Trading.GetBalance();
var orders = await api.Trading.GetOrders("BTCUSDT");
```

Do not hard-code or commit API credentials. Load them from a secure secret store
or environment variables. Only grant the API key permissions required by your
application.

REST requests fail with an exception when authorization is missing, the server
returns an unsuccessful HTTP status, or a response contains malformed JSON.
These errors are not converted into empty response models.
HTTP/API failures throw `HitBtcApiException`, which exposes `StatusCode` and the
exchange `ApiErrorCode` when available.

## Using the WebSocket API

`HitBtcSocketApi` owns a WebSocket connection and implements `IDisposable`.
Reuse one instance for related operations and dispose it when finished:

```csharp
using Hitbtc;

using (var api = new HitBtcSocketApi())
{
    var subscription = await api.MarketData.SubscribeTicker("BTCUSDT");
}
```

Authenticated WebSocket operations require credentials:

```csharp
using (var api = new HitBtcSocketApi())
{
    api.Authorize(apiKey, secretKey);
    var balances = await api.Trading.GetTradingBalance();
}
```

The client reuses its open connection, assembles fragmented UTF-8 messages,
uses separate public and trading v3 connections, serializes operations on each socket, and exposes cancellation through the
`Execute` overload accepting a `CancellationToken`.

API v3 no longer provides one-shot currency, symbol, or historical-trade queries
over WebSocket. Use `HitBtcRestApi.PublicData` and `HitBtcRestApi.TradingHistory` for those
queries. The corresponding legacy methods are retained as obsolete members and
throw `NotSupportedException` so that migration failures are explicit.
Malformed messages, exchange errors, and mismatched response IDs throw
`HitBtcWebSocketException`. The current high-level subscription methods return
the subscription acknowledgement. After subscribing, attach a
`NotificationReceived` handler and run `ListenForNotificationsAsync` with a
cancellation token to consume the continuous stream:

```csharp
using (var api = new HitBtcSocketApi())
using (var stop = new CancellationTokenSource())
{
    api.NotificationReceived += (sender, notification) =>
        Console.WriteLine(notification.RawJson);

    await api.MarketData.SubscribeTicker("BTCUSDT");
    await api.ListenForNotificationsAsync(false, stop.Token);
}
```

Commands and the listener are serialized on each connection, so complete all
subscriptions before starting the long-running listener.

## Migrating from API v2 to API v3

This version talks exclusively to REST paths under `/api/3`. Public streaming
uses `wss://api.hitbtc.com/api/3/ws/public`, while authenticated spot commands
use `wss://api.hitbtc.com/api/3/ws/trading`.

The important contract differences are:

| API v2 | API v3 |
| --- | --- |
| REST resources grouped under `/api/2` | Resources grouped by `public`, `spot`, and `wallet` under `/api/3` |
| Many JSON fields and parameters used camelCase | Fields and parameters use snake_case, such as `client_order_id` and `time_in_force` |
| Public symbol and ticker collections were arrays | Collections are objects keyed by symbol/currency; the client converts them to the existing typed lists |
| Order-book levels were `{ price, size }` objects | Levels are compact `[price, size]` arrays; conversion is handled by the model converter |
| One WebSocket endpoint and methods such as `subscribeTicker` | Separate public/trading endpoints and channel subscriptions such as `subscribe` with `ticker/1s` |
| Account and trading balances used `/account` and `/trading` | Wallet operations use `/wallet`; spot balances and orders use `/spot` |
| Transfer direction used `bankToExchange`/`exchangeToBank` | Transfers specify `source` and `destination` (`wallet` and `spot`) |

The public C# method names have largely been preserved to reduce application
changes, including the historical misspelling `PostWithraw`. New code should use
`PostWithdraw`, which accepts decimal amounts as strings and supports v3
`network_code`. Do not send real orders or withdrawals until the upgraded client
has been verified with the permissions and symbols used by your application.

## Running the demo application

`Test/Test.csproj` is an interactive WinForms API v3 demo. Before using buttons
that access private data, set credentials in the process environment:

```powershell
$env:HITBTC_API_KEY = "your-api-key"
$env:HITBTC_SECRET_KEY = "your-secret-key"
dotnet run --project Test/Test.csproj
```

The Public Test and socket MarketData buttons do not need credentials. The
Trading, Trading History, Account, and socket Trading buttons do. The demo only
reads public/private data or creates a subscription; it does not submit orders,
transfers, or withdrawals.

The redesigned console groups public REST, private REST, and WebSocket actions.
It provides an editable symbol, masked credential fields, tabular responses, a
status indicator, and a timestamped activity log containing request duration and
typed API errors. Credentials can be loaded from the environment without being
written to the log.

## Development workflow

The `master` branch is protected. Make each logical change on a feature branch
and merge it through a pull request:

```powershell
git switch master
git pull origin master
git switch -c feature/short-description

# Edit files, then commit the related changes.
git add .
git commit -m "feat: describe the change"
git push --set-upstream origin feature/short-description
```

Open a pull request targeting `master`. The automated workflow restores
dependencies, builds the Release configuration, and runs the test suite. Merge
only after the required checks pass.

## Continuous integration artifacts

Every push runs `.github/workflows/build-test-release.yml`. A successful run:

1. restores NuGet dependencies;
2. builds the solution in Release mode;
3. runs the automated tests;
4. uploads `Hitbtc.dll` and its runtime dependencies as a GitHub Actions artifact.

The artifact name contains the commit SHA and is retained for 30 days. Artifacts
from ordinary pushes are build outputs; they are not permanent GitHub Releases.

## Creating a new release

Releases use semantic version tags such as `v1.1.0`:

- increment the major version for breaking public API changes;
- increment the minor version for backward-compatible features;
- increment the patch version for backward-compatible fixes.

Before releasing:

1. ensure the intended changes have been merged into `master`;
2. confirm the GitHub Actions checks are successful;
3. update assembly/package version metadata when the version changes;
4. build and test the exact commit locally.

Then synchronize `master`, create an annotated tag, and push the tag:

```powershell
git switch master
git pull --ff-only origin master

git tag -a v1.1.0 -m "Release v1.1.0"
git push origin v1.1.0
```

Pushing the tag triggers the workflow. After the build and tests pass, it:

1. collects the Release DLL files;
2. creates `Hitbtc-v1.1.0.zip`;
3. creates the GitHub Release from the tag;
4. attaches the ZIP file and generated release notes.

If the workflow fails, fix the problem through another pull request. Do not move
or overwrite a published version tag. Create a new patch version instead.

## Contributing

- Keep normal tests deterministic and independent of live exchange connectivity.
- Never commit API keys, secret keys, account identifiers, or funded-account data.
- Add regression tests for bug fixes.
- Preserve the public API unless a breaking change is planned for a major release.
- Keep pull requests focused on one logical change.

## License

This project is licensed under the [MIT License](LICENSE). You may use, copy,
modify, merge, publish, distribute, sublicense, and sell copies of the software,
provided that the copyright and permission notices are retained. The software is
provided without warranty.

---

<div dir="rtl" align="right">

# HitBTC.Net — راهنمای فارسی

کتابخانه‌ای برای استفاده از رابط‌های REST و WebSocket نسخه ۳ صرافی HitBTC در زبان C# است. این پروژه بر پایهٔ .NET Framework 4.8 ساخته شده و برای دریافت اطلاعات عمومی بازار، مدیریت کیف پول، معاملات اسپات و تاریخچهٔ معاملات، مدل‌ها و کلاینت‌های نوع‌دار ارائه می‌دهد.

> این کتابخانه رسمی HitBTC نیست. پیش از استفاده از قابلیت‌های معامله یا برداشت روی حساب دارای موجودی، آن‌ها را با دقت بررسی کنید.

## ساختار مخزن

| مسیر | کاربرد |
| --- | --- |
| `HitBtc/` | کتابخانهٔ اصلی که فایل `Hitbtc.dll` را تولید می‌کند |
| `HitBtc/HitBtcCategories/` | گروه‌های عملیاتی REST و WebSocket |
| `HitBtc/HitBtcModel/` | مدل‌های درخواست و پاسخ |
| `HitBtc.Tests/` | تست‌های خودکار و مستقل از سرویس زنده |
| `Test/` | برنامهٔ نمایشی WinForms برای آزمایش دستی |
| `.github/workflows/` | فرایند خودکار بیلد، تست و انتشار |

پروژهٔ `Test` برنامهٔ نمونه است و جایگزین مجموعه‌تست خودکار نیست. فرایند CI فقط تست‌های `HitBtc.Tests` را اجرا می‌کند و به کلید واقعی API نیاز ندارد.

## پیش‌نیازها

- ویندوز و بستهٔ توسعهٔ .NET Framework 4.8
- Visual Studio 2022 یا یک نسخهٔ سازگار از .NET SDK و NuGet CLI
- Git برای مشارکت و ساخت نسخهٔ انتشار

## بازیابی وابستگی‌ها، بیلد و تست

</div>

```powershell
git clone https://github.com/iarash84/hitbtc.net.git
cd hitbtc.net
nuget restore Hitbtc.sln
dotnet build Hitbtc.sln --configuration Release --no-restore
dotnet test HitBtc.Tests/HitBtc.Tests.csproj --configuration Release --no-build --no-restore
```

<div dir="rtl" align="right">

فایل DLL نسخهٔ Release در مسیر `HitBtc/bin/Release/Hitbtc.dll` ساخته می‌شود. هنگام توزیع دستی، وابستگی‌های موجود در پوشهٔ خروجی را نیز همراه آن منتشر کنید.

## استفاده از REST API

endpointهای عمومی به کلید API نیاز ندارند:

</div>

```csharp
using Hitbtc;

var api = new HitBtcRestApi();
var symbols = await api.PublicData.GetSymbol();
var ticker = await api.PublicData.GetTicker("BTCUSDT");
```

<div dir="rtl" align="right">

برای عملیات خصوصی ابتدا کلاینت را احراز هویت کنید:

</div>

```csharp
var api = new HitBtcRestApi();
api.Authorize(apiKey, secretKey);

var balances = await api.Trading.GetBalance();
var orders = await api.Trading.GetOrders("BTCUSDT");
```

<div dir="rtl" align="right">

کلیدها را داخل کد یا مخزن قرار ندهید و فقط دسترسی‌های موردنیاز برنامه را برای آن‌ها فعال کنید.

خطاهای HTTP و خطاهای API با `HitBtcApiException` گزارش می‌شوند. این exception در صورت موجود بودن، وضعیت HTTP و کد خطای صرافی را نیز ارائه می‌دهد و پاسخ نامعتبر را به مدل خالی تبدیل نمی‌کند.

## استفاده از WebSocket

</div>

```csharp
using (var api = new HitBtcSocketApi())
{
    var subscription = await api.MarketData.SubscribeTicker("BTCUSDT");
}
```

<div dir="rtl" align="right">

نسخهٔ ۳ از اتصال‌های جداگانه برای داده‌های عمومی و عملیات معاملاتی استفاده می‌کند. درخواست‌های یک‌بارهٔ ارزها، نمادها و تاریخچهٔ معاملات دیگر از طریق WebSocket ارائه نمی‌شوند و باید از `HitBtcRestApi.PublicData` یا `HitBtcRestApi.TradingHistory` استفاده شود.

پاسخ JSON نامعتبر، خطای سرور یا شناسهٔ نامنطبق با `HitBtcWebSocketException` گزارش می‌شود. پس از دریافت تأیید اشتراک، با event به نام `NotificationReceived` و متد `ListenForNotificationsAsync` می‌توان notificationها را تا زمان لغو شدن `CancellationToken` به‌صورت پیوسته دریافت کرد. تمام subscriptionها را پیش از شروع listener اجرا کنید، زیرا commandها و دریافت پیوسته روی هر اتصال به‌صورت سریال اجرا می‌شوند.

## تفاوت نسخهٔ ۲ و ۳

| نسخهٔ ۲ | نسخهٔ ۳ |
| --- | --- |
| مسیرهای REST زیر `/api/2` | مسیرهای گروه‌بندی‌شده زیر `/api/3` |
| نام‌های camelCase | نام‌های snake_case مانند `client_order_id` |
| لیست آرایه‌ای symbol و ticker | آبجکت‌های کلیدگذاری‌شده با نام نماد یا ارز |
| یک endpoint برای WebSocket | endpointهای جداگانهٔ Public و Trading |
| متدهایی مانند `subscribeTicker` | اشتراک کانال با `subscribe` و `ticker/1s` |
| حساب‌های `/account` و `/trading` | گروه‌های `/wallet` و `/spot` |

نام بیشتر متدهای عمومی C# برای کاهش تغییرات مصرف‌کنندگان حفظ شده است. برای برداشت در کد جدید از `PostWithdraw` استفاده کنید؛ متد قدیمی `PostWithraw` فقط برای سازگاری باقی مانده است.

## اجرای برنامهٔ نمونه

پیش از استفاده از دکمه‌های خصوصی برنامهٔ WinForms، متغیرهای محیطی زیر را تنظیم کنید:

</div>

```powershell
$env:HITBTC_API_KEY = "your-api-key"
$env:HITBTC_SECRET_KEY = "your-secret-key"
dotnet run --project Test/Test.csproj
```

<div dir="rtl" align="right">

برنامهٔ نمونه سفارش، انتقال وجه یا برداشت ایجاد نمی‌کند و فقط داده‌ها و اشتراک‌ها را بررسی می‌کند.

کنسول بازطراحی‌شده عملیات REST عمومی، REST خصوصی و WebSocket را جدا می‌کند. نماد قابل‌ویرایش، فیلدهای مخفی اعتبارنامه، نمایش جدولی پاسخ، وضعیت عملیات و لاگ زمان‌دار برای مدت درخواست و خطاهای API در دسترس است. مقدار کلیدها هیچ‌گاه داخل لاگ نوشته نمی‌شود.

## روند توسعه و انتشار

شاخهٔ `master` محافظت‌شده است. تغییرات را در یک شاخهٔ feature انجام دهید، تست‌ها را اجرا کنید و سپس Pull Request بسازید. برای انتشار نسخهٔ جدید، پس از ادغام تغییرات و موفقیت CI یک تگ معنایی مانند `v1.1.0` ایجاد و push کنید. workflow فایل Release را می‌سازد و ZIP خروجی را به GitHub Release متصل می‌کند.

## مشارکت و امنیت

- تست‌ها باید قطعی و مستقل از سرویس زنده باشند.
- هیچ کلید API، رمز، شناسهٔ حساب یا اطلاعات مالی را commit نکنید.
- برای رفع باگ، تست بازگشتی اضافه کنید.
- تغییرات هر Pull Request را متمرکز و محدود نگه دارید.

## مجوز

این پروژه تحت [مجوز MIT](LICENSE) منتشر شده است. استفاده، کپی، تغییر، ادغام، انتشار، توزیع و فروش نرم‌افزار با حفظ اعلان حق نشر و متن مجوز مجاز است. نرم‌افزار بدون هیچ‌گونه ضمانت ارائه می‌شود.

</div>
