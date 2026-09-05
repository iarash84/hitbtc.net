# HitBTC.Net Wiki

[English](#english) · [فارسی](#فارسی)

## English

### Overview

HitBTC.Net 2.1.0 is an unofficial C# client for the HitBTC API v3 REST and
WebSocket interfaces. The library targets both .NET Framework 4.8 and .NET 8.0.

### Build and test

```powershell
nuget restore Hitbtc.sln
dotnet build Hitbtc.sln --configuration Release --no-restore
dotnet test HitBtc.Tests/HitBtc.Tests.csproj --configuration Release --no-build --no-restore
```

Release outputs are generated in `HitBtc/bin/Release/net48/` and
`HitBtc/bin/Release/net8.0/`. Select one complete directory for the target
application; do not mix dependencies from the two targets.

### REST quick start

```csharp
using Hitbtc;

using (var api = new HitBtcRestApi())
{
    var ticker = await api.PublicData.GetTicker("BTCUSDT");
    api.Authorize(apiKey, secretKey);
    var balances = await api.Trading.GetBalance();
}
```

Reuse an API instance for related calls and dispose it when its application
scope ends. Calling `Authorize` with changed credentials resets only the
authenticated HTTP client. REST failures throw `HitBtcApiException`.

### WebSocket notifications

```csharp
using (var socket = new HitBtcSocketApi())
using (var stop = new CancellationTokenSource())
{
    socket.NotificationReceived += (sender, notification) =>
        Console.WriteLine(notification.RawJson);
    await socket.MarketData.SubscribeTicker("BTCUSDT");
    await socket.ListenForNotificationsAsync(false, stop.Token);
}
```

Complete subscriptions before starting the listener because commands and
continuous receives are serialized per connection. On connection failure, the
listener reconnects with capped exponential backoff and replays successful
subscriptions. Configure it with `WebSocketReconnectOptions` and observe
attempts through `Reconnecting`. Trading commands are never replayed.

API v3 uses separate public and authenticated trading WebSocket connections.
Currency, symbol, and historical-trade lookups must use REST. Protocol failures
throw `HitBtcWebSocketException`.

### Demo and release

`Test/Test.csproj` is a .NET Framework 4.8 WinForms, read-only verification
console. Credentials may be loaded from `HITBTC_API_KEY` and
`HITBTC_SECRET_KEY`; it never submits orders, transfers, or withdrawals.

After merging to `master` and passing CI, create the `v2.1.0` tag. The workflow
packages separate `net48` and `net8.0` directories in the release ZIP.

---

<div dir="rtl" align="right">

## فارسی

### معرفی

HitBTC.Net نسخهٔ ۲.۱.۰ یک کلاینت غیررسمی C# برای رابط‌های REST و WebSocket
نسخهٔ ۳ صرافی HitBTC است. کتابخانه هم‌زمان از .NET Framework 4.8 و .NET 8.0
پشتیبانی می‌کند.

### بیلد و تست

</div>

```powershell
nuget restore Hitbtc.sln
dotnet build Hitbtc.sln --configuration Release --no-restore
dotnet test HitBtc.Tests/HitBtc.Tests.csproj --configuration Release --no-build --no-restore
```

<div dir="rtl" align="right">

خروجی‌ها در مسیرهای `HitBtc/bin/Release/net48/` و
`HitBtc/bin/Release/net8.0/` ساخته می‌شوند. پوشهٔ کامل Target موردنظر را توزیع
کنید و وابستگی‌های دو Target را با یکدیگر ترکیب نکنید.

### استفاده از REST

کلاس `HitBtcRestApi` را برای درخواست‌های مرتبط reuse و در پایان scope آن را
Dispose کنید. عملیات خصوصی به فراخوانی `Authorize` نیاز دارند. خطاهای REST با
`HitBtcApiException` گزارش می‌شوند.

### دریافت پیوستهٔ WebSocket

پس از Subscribe، با رویداد `NotificationReceived` و متد
`ListenForNotificationsAsync` می‌توان اعلان‌ها را تا زمان لغو CancellationToken
دریافت کرد. Subscriptionها باید پیش از Listener اجرا شوند. در صورت قطع ارتباط،
اتصال با exponential backoff برقرار و Subscriptionهای موفق بازیابی می‌شوند.
تنظیمات در `WebSocketReconnectOptions` و وضعیت تلاش‌ها در رویداد `Reconnecting`
قرار دارد. فرمان‌های معاملاتی برای جلوگیری از عملیات تکراری بازپخش نمی‌شوند.

### برنامهٔ نمونه و انتشار

پروژهٔ `Test` یک برنامهٔ WinForms فقط‌خواندنی برای بررسی REST و WebSocket است و
هیچ سفارش، انتقال یا برداشتی ثبت نمی‌کند. اعتبارنامه‌ها را می‌توان از متغیرهای
`HITBTC_API_KEY` و `HITBTC_SECRET_KEY` بارگذاری کرد.

پس از ادغام در `master` و موفقیت CI، تگ `v2.1.0` را ایجاد کنید. فایل ZIP انتشار
شامل پوشه‌های جداگانهٔ `net48` و `net8.0` خواهد بود.

### امنیت و مجوز

کلید API یا اطلاعات حساب را Commit نکنید. این پروژه تحت مجوز MIT و بدون ضمانت
ارائه می‌شود.

</div>
