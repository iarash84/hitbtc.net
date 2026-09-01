using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace Hitbtc
{
    internal interface IRestTransport : IDisposable
    {
        Task<RestResponse> ExecuteAsync(RestRequest request, RestClientOptions options,
            CancellationToken cancellationToken);
        void ResetAuthenticatedClient();
    }

    internal sealed class RestSharpTransport : IRestTransport
    {
        private readonly object _sync = new object();
        private RestClient? _publicClient;
        private RestClient? _authenticatedClient;
        private bool _disposed;

        public async Task<RestResponse> ExecuteAsync(RestRequest request, RestClientOptions options,
            CancellationToken cancellationToken)
        {
            RestClient client;
            lock (_sync)
            {
                ThrowIfDisposed();
                if (options.Authenticator == null)
                    client = _publicClient ?? (_publicClient = new RestClient(options));
                else
                    client = _authenticatedClient ?? (_authenticatedClient = new RestClient(options));
            }
            return await client.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
        }

        public void ResetAuthenticatedClient()
        {
            lock (_sync)
            {
                ThrowIfDisposed();
                _authenticatedClient?.Dispose();
                _authenticatedClient = null;
            }
        }

        public void Dispose()
        {
            lock (_sync)
            {
                if (_disposed) return;
                _publicClient?.Dispose();
                _authenticatedClient?.Dispose();
                _publicClient = null;
                _authenticatedClient = null;
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
#if NET8_0_OR_GREATER
            ObjectDisposedException.ThrowIf(_disposed, this);
#else
            if (_disposed) throw new ObjectDisposedException(nameof(RestSharpTransport));
#endif
        }
    }
}
