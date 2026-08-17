# HitBTC.Net

A C# client library for the HitBTC REST and WebSocket API. The library targets
.NET Framework 4.8 and provides typed clients for public market data, account
operations, trading, trading history, and JSON-RPC WebSocket operations.

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
var ticker = await api.PublicData.GetTicker("BTCUSD");
var candles = await api.PublicData.GetCandles(
    "BTCUSD",
    PublicEnum.EnPeriod.H4);
```

Account and trading endpoints require authorization:

```csharp
using Hitbtc;

var api = new HitBtcRestApi();
api.Authorize(apiKey, secretKey);

var balances = await api.Trading.GetBalance();
var orders = await api.Trading.GetOrders("BTCUSD");
```

Do not hard-code or commit API credentials. Load them from a secure secret store
or environment variables. Only grant the API key permissions required by your
application.

REST requests fail with an exception when authorization is missing, the server
returns an unsuccessful HTTP status, or a response contains malformed JSON.
These errors are not converted into empty response models.

## Using the WebSocket API

`HitBtcSocketApi` owns a WebSocket connection and implements `IDisposable`.
Reuse one instance for related operations and dispose it when finished:

```csharp
using Hitbtc;

using (var api = new HitBtcSocketApi())
{
    var currencies = await api.MarketData.GetCurrencies();
    var symbol = await api.MarketData.GetSymbol("BTCUSD");
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
serializes operations on the shared socket, and exposes cancellation through the
`Execute` overload accepting a `CancellationToken`.

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

No license file is currently included. Add an explicit license before distributing
the project as an open-source package.
