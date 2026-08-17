using System.Collections.Generic;
using System.Threading.Tasks;
using Hitbtc.HitBtcModel;
using RestSharp;

namespace Hitbtc.HitBtcCategories
{
    public class RestAccount
    {

        private readonly HitBtcRestApi _hitBtcRestApi;

        public RestAccount(HitBtcRestApi hitBtcRestApi)
        {
            _hitBtcRestApi = hitBtcRestApi;
        }


        public async Task<List<Balance>> GetBalance()
        {
            var request = new RestRequest("/api/2/account/balance");
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Get current address
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<AddressModel> GetAddress(string currency)
        {
            var request = new RestRequest("/api/2/account/crypto/address/{currency}");
            request.AddParameter("currency", currency, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Create new address
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<AddressModel> PostAddress(string currency)
        {
            var request = new RestRequest("/api/2/account/crypto/address/{currency}", Method.Post);
            request.AddParameter("currency", currency, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Withdraw crypro
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="amount">The amount that will be sent to the specified address</param>
        /// <param name="address"></param>
        /// <param name="paymentId">Optional parameter</param>
        /// <param name="networkFee">Optional parameter. Too low and too high commission value will be rounded to valid values.</param>
        /// <param name="includeFee">Default false. If set true then total will be spent the specified amount, fee and networkFee will be deducted from the amount</param>
        /// <param name="autoCommit">Default true. If set false then you should commit or rollback transaction in an hour. Used in two phase commit schema.</param>
        /// <returns>Unique identifier for Transaction as assigned by exchange</returns>
        public async Task<IdObject> PostWithraw(string currency, int amount, string address, string paymentId = null,
            string networkFee = null, bool includeFee = false, bool autoCommit = true)
        {
            var request = new RestRequest("/api/2/account/crypto/withdraw", Method.Post);
            if (!string.IsNullOrEmpty(currency))
                request.AddParameter("currency", currency);
            if (amount > 0)
                request.AddParameter("amount", amount);
            if (!string.IsNullOrEmpty(address))
                request.AddParameter("address", address);
            if (!string.IsNullOrEmpty(paymentId))
                request.AddParameter("paymentId", paymentId);
            if (!string.IsNullOrEmpty(networkFee))
                request.AddParameter("networkFee", networkFee);
            request.AddParameter("includeFee", includeFee);
            request.AddParameter("autoCommit", autoCommit);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Commit withdraw crypro
        /// </summary>
        /// <param name="withrawId">Unique identifier for Transaction as assigned by exchange</param>
        /// <returns></returns>
        public async Task<WithdrawConfirm> PutWithraw(string withrawId)
        {
            var request = new RestRequest("/api/2/account/crypto/withdraw/{id}", Method.Put);
            request.AddParameter("id", withrawId, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Rollback withdraw crypro
        /// </summary>
        /// <param name="withrawId">Unique identifier for Transaction as assigned by exchange</param>
        /// <returns></returns>
        public async Task<WithdrawConfirm> DeleteWithraw(string withrawId)
        {
            var request = new RestRequest("/api/2/account/crypto/withdraw/{id}", Method.Delete);
            request.AddParameter("id", withrawId, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Transfer money between trading and account
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<IdObject> PostTransfer(string currency, int amount,
            PublicEnum.EnTransferType type = PublicEnum.EnTransferType.bankToExchange)
        {
            var request = new RestRequest("/api/2/account/transfer", Method.Post);
            if (!string.IsNullOrEmpty(currency))
                request.AddParameter("currency", currency);
            if (amount > 0)
                request.AddParameter("amount", amount);
            request.AddParameter("type", type.ToString());
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Get account transactions
        /// </summary>
        /// <returns></returns>
        public async Task<List<Transaction>> GetTransaction()
        {
            return await _hitBtcRestApi.Execute(new RestRequest("/api/2/account/transactions"));
        }

        /// <summary>
        ///  get transaction by transaction id
        ///  Requires the "Payment information" API key permission.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="currency"></param>
        /// <param name="from"></param>
        /// <param name="till"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public async Task<List<Transaction>> GetTransaction(string transactionId, string currency, string from, string till, int offset, int limit = 100,
            PublicEnum.EnSort sort = PublicEnum.EnSort.Desc, PublicEnum.EnBy by = PublicEnum.EnBy.timestamp)
        {
            var request = new RestRequest("/api/2/account/transactions/{id}", Method.Get);
            request.AddParameter("id", transactionId, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(currency))
                request.AddQueryParameter("currency", currency);
            request.AddQueryParameter("sort", sort.ToString());
            request.AddQueryParameter("by", by.ToString());
            if (!string.IsNullOrEmpty(from))
                request.AddQueryParameter("from", from);
            if (!string.IsNullOrEmpty(till))
                request.AddQueryParameter("till", till);
            if (limit > 0)
                request.AddQueryParameter("limit", limit.ToString());
            if (offset > 0)
                request.AddQueryParameter("offset", offset.ToString());
            return await _hitBtcRestApi.Execute(request);
        }
    }
}
