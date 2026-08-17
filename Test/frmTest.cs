using System;
using System.Collections;
using System.Windows.Forms;
using Hitbtc;

namespace Test
{
    /// <summary>Small interactive client for exercising the HitBTC API v3 wrapper.</summary>
    public partial class frmTest : Form
    {
        private const string DefaultSymbol = "BTCUSDT";

        public frmTest()
        {
            InitializeComponent();
        }

        private async void btnPublicTest_Click(object sender, EventArgs e)
        {
            await Run(async () => Bind(await new HitBtcRestApi().PublicData.GetTicker(DefaultSymbol)));
        }

        private async void btnTradingTest_Click(object sender, EventArgs e)
        {
            await Run(async () =>
            {
                var api = AuthorizedRestClient();
                Bind(await api.Trading.GetOrders(DefaultSymbol));
            });
        }

        private async void btnTradingHistory_Click(object sender, EventArgs e)
        {
            await Run(async () =>
            {
                var api = AuthorizedRestClient();
                Bind(await api.TradingHistory.GetTraders(DefaultSymbol, null, null, 0));
            });
        }

        private async void btnAccount_Click(object sender, EventArgs e)
        {
            await Run(async () => Bind(await AuthorizedRestClient().Account.GetBalance()));
        }

        private async void btnMarketData_Click(object sender, EventArgs e)
        {
            await Run(async () =>
            {
                using (var api = new HitBtcSocketApi())
                    Bind(await api.MarketData.SubscribeTicker(DefaultSymbol));
            });
        }

        private async void btnSocketTrading_Click(object sender, EventArgs e)
        {
            await Run(async () =>
            {
                using (var api = AuthorizedSocketClient())
                    Bind(await api.Trading.GetTradingBalance());
            });
        }

        private static HitBtcRestApi AuthorizedRestClient()
        {
            var api = new HitBtcRestApi();
            api.Authorize(ApiKey(), SecretKey());
            return api;
        }

        private static HitBtcSocketApi AuthorizedSocketClient()
        {
            var api = new HitBtcSocketApi();
            api.Authorize(ApiKey(), SecretKey());
            return api;
        }

        private static string ApiKey()
        {
            return RequiredEnvironmentVariable("HITBTC_API_KEY");
        }

        private static string SecretKey()
        {
            return RequiredEnvironmentVariable("HITBTC_SECRET_KEY");
        }

        private static string RequiredEnvironmentVariable(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException("Set the " + name + " environment variable first.");
            return value;
        }

        private void Bind(object response)
        {
            gridviewReponse.DataSource = response is IList ? response : new[] { response };
        }

        private static async System.Threading.Tasks.Task Run(Func<System.Threading.Tasks.Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "HitBTC API v3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
