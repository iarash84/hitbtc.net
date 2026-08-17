using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Hitbtc;
using Hitbtc.HitBtcModel;

namespace Test
{
    /// <summary>Interactive, read-only verification console for HitBTC.Net API v3.</summary>
    public partial class frmTest : Form
    {
        private const string ApiKeyVariable = "HITBTC_API_KEY";
        private const string SecretKeyVariable = "HITBTC_SECRET_KEY";

        public frmTest()
        {
            InitializeComponent();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            LoadCredentialsFromEnvironment(false);
            WriteLog(LogLevel.Info, "Console initialized. Public operations are ready.");
            WriteLog(LogLevel.Info, "All available actions are read-only; no order, transfer, or withdrawal is submitted.");
            if (HasCredentials)
                WriteLog(LogLevel.Success, "Credentials were loaded from environment variables.");
            else
                WriteLog(LogLevel.Info, "Private operations require API key and secret fields.");
        }

        private async void btnTicker_Click(object sender, EventArgs e)
        {
            await RunOperation("Get ticker", (Button)sender,
                async () => await new HitBtcRestApi().PublicData.GetTicker(Symbol));
        }

        private async void btnSymbols_Click(object sender, EventArgs e)
        {
            await RunOperation("Get symbols", (Button)sender,
                async () => await new HitBtcRestApi().PublicData.GetSymbol());
        }

        private async void btnCurrencies_Click(object sender, EventArgs e)
        {
            await RunOperation("Get currencies", (Button)sender,
                async () => await new HitBtcRestApi().PublicData.GetCurrency());
        }

        private async void btnOrderBook_Click(object sender, EventArgs e)
        {
            await RunOperation("Get order book", (Button)sender, async () =>
            {
                var orderbook = await new HitBtcRestApi().PublicData.GetOrderbook(Symbol, 25);
                return ToOrderBookRows(orderbook);
            });
        }

        private async void btnCandles_Click(object sender, EventArgs e)
        {
            await RunOperation("Get M30 candles", (Button)sender,
                async () => await new HitBtcRestApi().PublicData.GetCandles(Symbol, PublicEnum.EnPeriod.M30));
        }

        private async void btnSpotBalance_Click(object sender, EventArgs e)
        {
            await RunOperation("Get spot balance", (Button)sender,
                async () => await CreateAuthorizedRestClient().Trading.GetBalance());
        }

        private async void btnActiveOrders_Click(object sender, EventArgs e)
        {
            await RunOperation("Get active orders", (Button)sender,
                async () => await CreateAuthorizedRestClient().Trading.GetOrders(Symbol));
        }

        private async void btnTradingHistory_Click(object sender, EventArgs e)
        {
            await RunOperation("Get trade history", (Button)sender,
                async () => await CreateAuthorizedRestClient().TradingHistory
                    .GetTraders(Symbol, null, null, 0, 100));
        }

        private async void btnWalletBalance_Click(object sender, EventArgs e)
        {
            await RunOperation("Get wallet balance", (Button)sender,
                async () => await CreateAuthorizedRestClient().Account.GetBalance());
        }

        private async void btnSubscribeTicker_Click(object sender, EventArgs e)
        {
            await RunOperation("Subscribe ticker WebSocket", (Button)sender, async () =>
            {
                using (var api = new HitBtcSocketApi())
                using (var stop = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                {
                    var notifications = new List<NotificationRow>();
                    var notificationLock = new object();
                    api.Reconnecting += (reconnectSender, reconnect) => BeginInvoke(new Action(() =>
                        WriteLog(LogLevel.Info, string.Format(
                            "WebSocket disconnected; reconnect attempt {0} starts in {1:N0} ms.",
                            reconnect.Attempt, reconnect.Delay.TotalMilliseconds))));
                    api.NotificationReceived += (notificationSender, notification) =>
                    {
                        List<NotificationRow> snapshot;
                        lock (notificationLock)
                        {
                            notifications.Add(new NotificationRow(notification.Channel,
                                notification.Method, notification.RawJson));
                            snapshot = notifications.ToList();
                        }
                        BeginInvoke(new Action(() =>
                        {
                            BindResult(snapshot);
                            WriteLog(LogLevel.Success, "WebSocket notification received from " +
                                (notification.Channel ?? notification.Method ?? "unknown channel") + ".");
                        }));
                    };
                    var acknowledgement = await api.MarketData.SubscribeTicker(Symbol);
                    WriteLog(LogLevel.Info, "Subscription acknowledged. Listening for notifications for 10 seconds.");
                    await api.ListenForNotificationsAsync(false, stop.Token);
                    lock (notificationLock)
                    {
                        return notifications.Count == 0
                            ? (object)new[] { acknowledgement }
                            : notifications.ToList();
                    }
                }
            });
        }

        private async void btnSocketBalance_Click(object sender, EventArgs e)
        {
            await RunOperation("Get WebSocket trading balance", (Button)sender, async () =>
            {
                using (var api = CreateAuthorizedSocketClient())
                {
                    var response = await api.Trading.GetTradingBalance();
                    return response.Result;
                }
            });
        }

        private async Task RunOperation(string operationName, Button source,
            Func<Task<object>> operation)
        {
            var timer = Stopwatch.StartNew();
            SetBusy(true, operationName + "...");

            try
            {
                WriteLog(LogLevel.Request, operationName + " started" + OperationContext(operationName) + ".");
                var result = await operation();
                var count = BindResult(result);
                timer.Stop();
                WriteLog(LogLevel.Success, string.Format("{0} completed in {1:N0} ms; {2} row(s) displayed.",
                    operationName, timer.Elapsed.TotalMilliseconds, count));
                lblStatus.Text = operationName + " completed";
            }
            catch (Exception exception)
            {
                timer.Stop();
                gridResults.DataSource = null;
                lblResultCount.Text = "0 rows";
                WriteLog(LogLevel.Error, FormatException(operationName, exception, timer.Elapsed));
                lblStatus.Text = operationName + " failed";
            }
            finally
            {
                SetBusy(false, lblStatus.Text);
                source.Focus();
            }
        }

        private int BindResult(object result)
        {
            gridResults.DataSource = null;
            if (result == null)
            {
                lblResultCount.Text = "0 rows";
                return 0;
            }

            var list = result as IList;
            if (list != null)
            {
                gridResults.DataSource = list;
                lblResultCount.Text = list.Count + " rows";
                return list.Count;
            }

            gridResults.DataSource = new[] { result };
            lblResultCount.Text = "1 row";
            return 1;
        }

        private static List<OrderBookRow> ToOrderBookRows(Orderbook orderbook)
        {
            var rows = new List<OrderBookRow>();
            if (orderbook == null) return rows;
            if (orderbook.Ask != null)
                rows.AddRange(orderbook.Ask.Select((level, index) =>
                    new OrderBookRow(index + 1, "Ask", level.Price, level.Size, orderbook.Timestamp)));
            if (orderbook.Bid != null)
                rows.AddRange(orderbook.Bid.Select((level, index) =>
                    new OrderBookRow(index + 1, "Bid", level.Price, level.Size, orderbook.Timestamp)));
            return rows;
        }

        private HitBtcRestApi CreateAuthorizedRestClient()
        {
            EnsureCredentials();
            var api = new HitBtcRestApi();
            api.Authorize(txtApiKey.Text.Trim(), txtSecret.Text);
            return api;
        }

        private HitBtcSocketApi CreateAuthorizedSocketClient()
        {
            EnsureCredentials();
            var api = new HitBtcSocketApi();
            api.Authorize(txtApiKey.Text.Trim(), txtSecret.Text);
            return api;
        }

        private void EnsureCredentials()
        {
            if (!HasCredentials)
                throw new InvalidOperationException("Enter API key and secret, or load HITBTC_API_KEY and HITBTC_SECRET_KEY from the environment.");
        }

        private bool HasCredentials =>
            !string.IsNullOrWhiteSpace(txtApiKey.Text) && !string.IsNullOrWhiteSpace(txtSecret.Text);

        private string Symbol
        {
            get
            {
                var symbol = txtSymbol.Text.Trim().ToUpperInvariant();
                if (string.IsNullOrWhiteSpace(symbol))
                    throw new InvalidOperationException("Enter a trading symbol such as BTCUSDT.");
                return symbol;
            }
        }

        private string OperationContext(string operationName)
        {
            return operationName.IndexOf("balance", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   operationName.IndexOf("currencies", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   operationName.IndexOf("symbols", StringComparison.OrdinalIgnoreCase) >= 0
                ? string.Empty
                : " for " + Symbol;
        }

        private void btnLoadEnvironment_Click(object sender, EventArgs e)
        {
            LoadCredentialsFromEnvironment(true);
        }

        private void LoadCredentialsFromEnvironment(bool reportResult)
        {
            txtApiKey.Text = Environment.GetEnvironmentVariable(ApiKeyVariable) ?? string.Empty;
            txtSecret.Text = Environment.GetEnvironmentVariable(SecretKeyVariable) ?? string.Empty;
            if (!reportResult) return;

            WriteLog(HasCredentials ? LogLevel.Success : LogLevel.Error,
                HasCredentials
                    ? "Credentials loaded from environment variables."
                    : "Environment credentials were not found or were incomplete.");
        }

        private void chkShowSecret_CheckedChanged(object sender, EventArgs e)
        {
            txtApiKey.UseSystemPasswordChar = !chkShowSecret.Checked;
            txtSecret.UseSystemPasswordChar = !chkShowSecret.Checked;
            WriteLog(LogLevel.Info, chkShowSecret.Checked ? "Credential visibility enabled." : "Credential visibility disabled.");
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            WriteLog(LogLevel.Info, "Log cleared.");
        }

        private void SetBusy(bool busy, string status)
        {
            actionsPanel.Enabled = !busy;
            settingsPanel.Enabled = !busy;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            lblStatus.Text = status;
        }

        private void WriteLog(LogLevel level, string message)
        {
            var color = Color.FromArgb(209, 213, 219);
            var label = "INFO";
            switch (level)
            {
                case LogLevel.Request:
                    color = Color.FromArgb(96, 165, 250);
                    label = "SEND";
                    break;
                case LogLevel.Success:
                    color = Color.FromArgb(74, 222, 128);
                    label = " OK ";
                    break;
                case LogLevel.Error:
                    color = Color.FromArgb(248, 113, 113);
                    label = "ERROR";
                    break;
            }

            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.SelectionColor = Color.FromArgb(156, 163, 175);
            txtLog.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + "  ");
            txtLog.SelectionColor = color;
            txtLog.AppendText("[" + label + "] ");
            txtLog.SelectionColor = Color.FromArgb(229, 231, 235);
            txtLog.AppendText(message + Environment.NewLine);
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.ScrollToCaret();
        }

        private static string FormatException(string operation, Exception exception,
            TimeSpan elapsed)
        {
            var restError = exception as HitBtcApiException;
            if (restError != null)
                return string.Format("{0} failed after {1:N0} ms: {2} (HTTP {3}, API code {4}).",
                    operation, elapsed.TotalMilliseconds, restError.Message,
                    (int)restError.StatusCode, restError.ApiErrorCode ?? "n/a");

            var socketError = exception as HitBtcWebSocketException;
            if (socketError != null)
                return string.Format("{0} failed after {1:N0} ms: {2} (API code {3}).",
                    operation, elapsed.TotalMilliseconds, socketError.Message,
                    socketError.ApiErrorCode ?? "n/a");

            return string.Format("{0} failed after {1:N0} ms: {2}: {3}", operation,
                elapsed.TotalMilliseconds, exception.GetType().Name, exception.Message);
        }

        private enum LogLevel
        {
            Info,
            Request,
            Success,
            Error
        }

        private sealed class OrderBookRow
        {
            public OrderBookRow(int level, string side, string price, string size, string timestamp)
            {
                Level = level;
                Side = side;
                Price = price;
                Size = size;
                Timestamp = timestamp;
            }

            public int Level { get; }
            public string Side { get; }
            public string Price { get; }
            public string Size { get; }
            public string Timestamp { get; }
        }

        private sealed class NotificationRow
        {
            public NotificationRow(string channel, string method, string rawJson)
            {
                ReceivedAt = DateTime.Now;
                Channel = channel;
                Method = method;
                RawJson = rawJson;
            }

            public DateTime ReceivedAt { get; }
            public string Channel { get; }
            public string Method { get; }
            public string RawJson { get; }
        }
    }
}
