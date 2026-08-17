using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using RestSharp;

namespace Hitbtc.HitBtcCategories
{
    /// <summary>Wallet operations for HitBTC API v3.</summary>
    public class RestAccount
    {
        private readonly HitBtcRestApi _api;
        public RestAccount(HitBtcRestApi api) { _api = api; }

        public async Task<List<Balance>> GetBalance()
        {
            return await _api.Execute(new RestRequest("/api/3/wallet/balance"));
        }

        public async Task<AddressModel> GetAddress(string currency)
        {
            var request = new RestRequest("/api/3/wallet/crypto/address");
            request.AddQueryParameter("currency", currency);
            var response = await _api.Execute(request);
            var addresses = Utilities.ConvertListFromJson<AddressModel>(response);
            return addresses.FirstOrDefault();
        }

        public async Task<AddressModel> PostAddress(string currency)
        {
            var request = new RestRequest("/api/3/wallet/crypto/address", Method.Post);
            request.AddParameter("currency", currency);
            return await _api.Execute(request);
        }

        public Task<IdObject> PostWithraw(string currency, int amount, string address,
            string paymentId = null, string networkFee = null, bool includeFee = false,
            bool autoCommit = true)
        {
            return PostWithdraw(currency, amount.ToString(CultureInfo.InvariantCulture), address,
                paymentId, null, includeFee, autoCommit);
        }

        /// <summary>Creates a v3 crypto withdrawal. The amount is a decimal string.</summary>
        public async Task<IdObject> PostWithdraw(string currency, string amount, string address,
            string paymentId = null, string networkCode = null, bool includeFee = false,
            bool autoCommit = true)
        {
            var request = new RestRequest("/api/3/wallet/crypto/withdraw", Method.Post);
            request.AddParameter("currency", currency);
            request.AddParameter("amount", amount);
            request.AddParameter("address", address);
            AddOptional(request, "payment_id", paymentId);
            AddOptional(request, "network_code", networkCode);
            request.AddParameter("include_fee", includeFee.ToString().ToLowerInvariant());
            request.AddParameter("auto_commit", autoCommit.ToString().ToLowerInvariant());
            return await _api.Execute(request);
        }

        public async Task<WithdrawConfirm> PutWithraw(string withrawId)
        {
            return await _api.Execute(WithdrawalRequest(withrawId, Method.Put));
        }

        public async Task<WithdrawConfirm> DeleteWithraw(string withrawId)
        {
            return await _api.Execute(WithdrawalRequest(withrawId, Method.Delete));
        }

        public async Task<IdObject> PostTransfer(string currency, int amount,
            PublicEnum.EnTransferType type = PublicEnum.EnTransferType.bankToExchange)
        {
            var request = new RestRequest("/api/3/wallet/transfer", Method.Post);
            request.AddParameter("currency", currency);
            request.AddParameter("amount", amount.ToString(CultureInfo.InvariantCulture));
            request.AddParameter("source", type == PublicEnum.EnTransferType.bankToExchange ? "wallet" : "spot");
            request.AddParameter("destination", type == PublicEnum.EnTransferType.bankToExchange ? "spot" : "wallet");
            var response = await _api.Execute(request);
            var ids = Utilities.ConvertListFromJson<string>(response);
            return new IdObject { Id = ids.FirstOrDefault() };
        }

        public async Task<List<Transaction>> GetTransaction()
        {
            return await _api.Execute(new RestRequest("/api/3/wallet/transactions"));
        }

        public async Task<List<Transaction>> GetTransaction(string transactionId, string currency,
            string from, string till, int offset, int limit = 100,
            PublicEnum.EnSort sort = PublicEnum.EnSort.Desc,
            PublicEnum.EnBy by = PublicEnum.EnBy.timestamp)
        {
            var request = new RestRequest("/api/3/wallet/transactions/{id}");
            request.AddParameter("id", transactionId, ParameterType.UrlSegment);
            Transaction transaction = await _api.Execute(request);
            return new List<Transaction> { transaction };
        }

        private static RestRequest WithdrawalRequest(string id, Method method)
        {
            var request = new RestRequest("/api/3/wallet/crypto/withdraw/{id}", method);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            return request;
        }

        private static void AddOptional(RestRequest request, string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) request.AddParameter(name, value);
        }
    }
}
