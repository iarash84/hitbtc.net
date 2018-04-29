# hitbtc
HitBTC Websocket API 2.0 Client written in C#.\
A full-featured .NET **[Hitbtc API](https://api.hitbtc.com)** designed for ease of use.\
Compatible with **.NET Standard 2.0** and **.NET Framework 4.7.1**


## Getting Started

Go to https://hitbtc.com/settings/api-keys and create your api keys

## Quick Examples

```C#
using Hitbtc;

var hitBtcRestApi = new HitBtcRestApi();
if (!hitBtcRestApi.IsAuthorized)
	hitBtcRestApi.Authorize(ApiKey, SecretKey);

//var response = await hitBtcRestApi.Trading.GetBalance();
//var response = await hitBtcRestApi.Trading.GetFee("BTCUSD");
var response = await hitBtcRestApi.Trading.GetOrders("BTCUSD");
```

#### Web Socket
Get real-time aggregate trades.

```C#
using Hitbtc;

var hitBtcSocketApi = new HitBtcSocketApi();
//var response = await hitBtcSocketApi.MarketData.GetCurrency("ETH");
//var response = await hitBtcSocketApi.MarketData.GetCurrencies();
var response = await hitBtcSocketApi.MarketData.UnsubscribeCandles("BTCUSD");

```

## Donate
If you find this tool useful, you can show you support with a kind donation:

**ETH**: 0x4e135a99630a391b71979a57f34f85ad28a1a1ef\
**LTC**: LLgZmzfLA6uLTVqCfZBHub3C3dhjBbMxk2\
**BTC**: 3QuChD5q4FXU8iW3zCsCV6GJvcJegAvTsi

Thank you.


