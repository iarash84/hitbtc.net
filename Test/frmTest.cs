using System;
using System.Windows.Forms;
using Hitbtc;

namespace Test
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private const string ApiKey = "<APIKEY>";
        private const string SecretKey = "<SecretKey>";

        private async void  btnTest_Click(object sender, EventArgs e)
        {
            var hitBtcRestApi = new HitBtcRestApi();
            //var response = await hitBtcRestApi.PublicData.GetSymbol();
            //var response = await hitBtcRestApi.PublicData.GetSymbol("BTCUSD");
            //var response = await hitBtcRestApi.PublicData.GetCurrency();
            //var response = await hitBtcRestApi.PublicData.GetCurrency("btc");
            //var response = await hitBtcRestApi.PublicData.GetTicker();
            //var response = await hitBtcRestApi.PublicData.GetTicker("BTCUSD");
            //var response = await hitBtcRestApi.PublicData.GetOrderbook("BTCUSD", 10);
            //gridviewReponse.DataSource = response.ask;
            var response = await hitBtcRestApi.PublicData.GetCandles("BTCUSD");

            gridviewReponse.DataSource = response;
        }

        private async void btnTradingTest_Click(object sender, EventArgs e)
        {


            var hitBtcRestApi = new HitBtcRestApi();
            if (!hitBtcRestApi.IsAuthorized)
                hitBtcRestApi.Authorize(ApiKey, SecretKey);

            //var response = await hitBtcRestApi.Trading.GetBalance();
            //var response = await hitBtcRestApi.Trading.GetFee("BTCUSD");
            var response = await hitBtcRestApi.Trading.GetOrders("BTCUSD");
            gridviewReponse.DataSource = response;
        }

        private async void btnAccount_Click(object sender, EventArgs e)
        {
            var hitBtcRestApi = new HitBtcRestApi();
            if (!hitBtcRestApi.IsAuthorized)
                hitBtcRestApi.Authorize(ApiKey, SecretKey);

            //var response = await hitBtcRestApi.Account.GetBalance();
            var response = await hitBtcRestApi.Account.GetAddress("btc");
            gridviewReponse.DataSource = response;
        }

        private async void btnMarketData_Click(object sender, EventArgs e)
        {
            var hitBtcSocketApi = new HitBtcSocketApi();
            //var response = await hitBtcSocketApi.MarketData.GetCurrency("ETH");
            //var response = await hitBtcSocketApi.MarketData.GetCurrencies();
            var response = await hitBtcSocketApi.MarketData.GetTrades("BTCUSD","From","till",1);
            //rtbResponse.Text = response.Content;
            //var response = await hitBtcSocketApi.MarketData.UnsubscribeCandles("BTCUSD");
            gridviewReponse.DataSource = response;
        }

        private async void btnSocketTrading_Click(object sender, EventArgs e)
        {
            var hitBtcSocketApi = new HitBtcSocketApi();

            if (!hitBtcSocketApi.IsAuthorized)
                hitBtcSocketApi.Authorize(ApiKey, SecretKey);

            //var response = await hitBtcSocketApi.Trading.SubscribeReports();
            var response = await hitBtcSocketApi.Trading.GetTradingBalance();

            gridviewReponse.DataSource = response;
        }
    
    }
}
