using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Xunit;

namespace Hitbtc.Tests
{
    public class RestTransportTests
    {
        [Fact]
        public async Task Execute_PublicSuccess_ReturnsContentWithoutAuthenticator()
        {
            var transport = new FakeRestTransport(new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed,
                Content = "{\"last\":\"1\"}"
            });
            var api = new HitBtcRestApi(transport);

            var response = await api.Execute(new RestRequest("/api/3/public/ticker/BTCUSDT"), false);

            Assert.Equal("{\"last\":\"1\"}", response.Content);
            Assert.Null(transport.Options.Authenticator);
        }

        [Fact]
        public async Task Execute_AuthorizedRequest_ConfiguresAuthenticator()
        {
            var transport = SuccessfulTransport();
            var api = new HitBtcRestApi(transport);
            api.Authorize("key", "secret");

            await api.Execute(new RestRequest("/api/3/spot/balance"));

            Assert.NotNull(transport.Options.Authenticator);
        }

        [Fact]
        public async Task Execute_ApiError_ThrowsTypedExceptionWithStatusAndCode()
        {
            var transport = new FakeRestTransport(new RestResponse
            {
                StatusCode = (HttpStatusCode)429,
                ResponseStatus = ResponseStatus.Completed,
                Content = "{\"error\":{\"code\":\"429\",\"message\":\"rate limit\"}}"
            });
            var api = new HitBtcRestApi(transport);

            var error = await Assert.ThrowsAsync<HitBtcApiException>(
                () => api.Execute(new RestRequest("/api/3/public/ticker"), false));

            Assert.Equal((HttpStatusCode)429, error.StatusCode);
            Assert.Equal("429", error.ApiErrorCode);
            Assert.Contains("rate limit", error.Message);
        }

        [Fact]
        public async Task Execute_TransportFailure_PreservesInnerException()
        {
            var cause = new TimeoutException("transport timeout");
            var transport = new FakeRestTransport(new RestResponse
            {
                ResponseStatus = ResponseStatus.TimedOut,
                ErrorException = cause
            });
            var api = new HitBtcRestApi(transport);

            var error = await Assert.ThrowsAsync<HitBtcApiException>(
                () => api.Execute(new RestRequest("/api/3/public/ticker"), false));

            Assert.Same(cause, error.InnerException);
        }

        [Fact]
        public async Task Execute_CancelledToken_DoesNotInvokeTransport()
        {
            var transport = SuccessfulTransport();
            var api = new HitBtcRestApi(transport);
            using (var source = new CancellationTokenSource())
            {
                source.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                    api.Execute(new RestRequest("/api/3/public/ticker"), false, source.Token));
            }
            Assert.Equal(0, transport.CallCount);
        }

        [Fact]
        public void IsAuthorized_PublicSetter_IsNotExposed()
        {
            var restSetter = typeof(HitBtcRestApi).GetProperty("IsAuthorized").SetMethod;
            var socketSetter = typeof(HitBtcSocketApi).GetProperty("IsAuthorized").SetMethod;

            Assert.True(restSetter.IsPrivate);
            Assert.True(socketSetter.IsPrivate);
        }

        private static FakeRestTransport SuccessfulTransport()
        {
            return new FakeRestTransport(new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed,
                Content = "{}"
            });
        }

        private sealed class FakeRestTransport : IRestTransport
        {
            private readonly RestResponse _response;
            public FakeRestTransport(RestResponse response) { _response = response; }
            public RestClientOptions Options { get; private set; }
            public int CallCount { get; private set; }

            public Task<RestResponse> ExecuteAsync(RestRequest request, RestClientOptions options,
                CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                CallCount++;
                Options = options;
                return Task.FromResult(_response);
            }
        }
    }
}
