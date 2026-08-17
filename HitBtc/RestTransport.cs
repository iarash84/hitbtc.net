using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace Hitbtc
{
    internal interface IRestTransport
    {
        Task<RestResponse> ExecuteAsync(RestRequest request, RestClientOptions options,
            CancellationToken cancellationToken);
    }

    internal sealed class RestSharpTransport : IRestTransport
    {
        public async Task<RestResponse> ExecuteAsync(RestRequest request, RestClientOptions options,
            CancellationToken cancellationToken)
        {
            using (var client = new RestClient(options))
                return await client.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
