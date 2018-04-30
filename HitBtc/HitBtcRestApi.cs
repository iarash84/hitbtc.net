using System;
using System.Threading.Tasks;
using Hitbtc.HitBtcCategories;
using RestSharp;
using RestSharp.Authenticators;

namespace Hitbtc
{
    
//    200 OK Successful request
//    400 Bad Request. Returns JSON with the error message
//    401 Unauthorized. Authorisation required or failed
//    403 Forbidden. Action is forbidden for API key
//    429 Too Many Requests. Your connection is being rate limited
//    500 Internal Server. Internal Server Error
//    503 Service Unavailable. Service is down for maintenance
//    504 Gateway Timeout. Request timeout expired

    /// <summary>
    /// https://api.hitbtc.com/api/2/explore/
    /// https://api.hitbtc.com/
    /// </summary>
    public class HitBtcRestApi
    {
        private const string Url = "https://api.hitbtc.com";
        private  string _apiKey;
        private  string _secretKey;

        public RestTrading Trading { get; set; }
        public RestAccount Account { get; set; }
        public RestPublicData PublicData { get; set; }
        public RestTradingHistory TradingHistory { get; set; }

        public HitBtcRestApi()
        {
            PublicData = new RestPublicData(this);
            TradingHistory = new RestTradingHistory(this);
            Trading = new RestTrading(this);
            Account = new RestAccount(this);
        }

        public async Task<ApiResponse> Execute(RestRequest request, bool requireAuthentication = true)
        {
            if (requireAuthentication && !IsAuthorized)
                throw new Exception("AccessTokenInvalid");

            var client = new RestClient(Url);

            if (requireAuthentication)
                client.Authenticator = new HttpBasicAuthenticator(_apiKey, _secretKey);

            var response = await client.GetResponseAsync(request).ConfigureAwait(false);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }

            return new ApiResponse { Content = response.Content };
        }

        /// <summary>
        /// Flag shows that user is authorized
        /// </summary>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// Method for authorization 
        /// </summary>
        /// <param name="apiKey">API key from the Settings page.</param>
        /// <param name="secretKey">Secret key from the Settings page.</param>
        public void Authorize(string apiKey, string secretKey)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
            IsAuthorized = true;
        }

    }
    public static class RestClientExtensions
    {
        private static Task<T> SelectAsync<T>(this RestClient client, IRestRequest request,
            Func<IRestResponse, T> selector)
        {
            var tcs = new TaskCompletionSource<T>();
            var loginResponse = client.ExecuteAsync(request, r =>
            {
                if (r.ErrorException == null)
                {
                    tcs.SetResult(selector(r));
                }
                else
                {
                    tcs.SetException(r.ErrorException);
                }
            });
            return tcs.Task;
        }

        public static Task<IRestResponse> GetResponseAsync(this RestClient client, IRestRequest request)
        {
            return client.SelectAsync(request, r => r);
        }
    }
}
