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
        public async Task<Address> GetAddress(string currency)
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
        public async Task<Address> PostAddress(string currency)
        {
            var request = new RestRequest("/api/2/account/crypto/address/{currency}", Method.POST);
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
        public async Task<Id> PostWithraw(string currency, int amount, string address, string paymentId = null,
            string networkFee = null, bool includeFee = false, bool autoCommit = true)
        {
            var request = new RestRequest("/api/2/account/crypto/withdraw");
            if (!string.IsNullOrEmpty(currency))
                request.AddParameter("currency", currency, ParameterType.UrlSegment);
            if (amount > 0)
                request.AddParameter("amount", amount, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(address))
                request.AddParameter("address", address, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(paymentId))
                request.AddParameter("paymentId", paymentId, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(networkFee))
                request.AddParameter("networkFee", networkFee, ParameterType.UrlSegment);
            request.AddParameter("includeFee", includeFee, ParameterType.UrlSegment);
            request.AddParameter("autoCommit", autoCommit, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }

        /// <summary>
        /// Commit withdraw crypro
        /// </summary>
        /// <param name="withrawId">Unique identifier for Transaction as assigned by exchange</param>
        /// <returns></returns>
        public async Task<WithdrawConfirm> PutWithraw(string withrawId)
        {
            var request = new RestRequest("/api/2/account/crypto/withdraw/{id}");
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
            var request = new RestRequest("/api/2/account/crypto/withdraw/{id}", Method.DELETE);
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
        public async Task<Id> PostTransfer(string currency, int amount,
            PublicEnum.EnTransferType type = PublicEnum.EnTransferType.bankToExchange)
        {
            var request = new RestRequest("/api/2/account/transfer", Method.POST);
            if (!string.IsNullOrEmpty(currency))
                request.AddParameter("currency", currency, ParameterType.UrlSegment);
            if (amount > 0)
                request.AddParameter("amount", amount, ParameterType.UrlSegment);
            request.AddParameter("type", type, ParameterType.UrlSegment);
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
            var request = new RestRequest("/api/2/account/transactions/{id}", Method.GET);
            request.AddParameter("id", transactionId, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(currency))
                request.AddParameter("currency", currency, ParameterType.UrlSegment);
            request.AddParameter("sort", sort.ToString(), ParameterType.UrlSegment);
            request.AddParameter("by", by.ToString(), ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(from))
                request.AddParameter("from", from, ParameterType.UrlSegment);
            if (!string.IsNullOrEmpty(till))
                request.AddParameter("till", till, ParameterType.UrlSegment);
            if (limit > 0)
                request.AddParameter("limit", limit, ParameterType.UrlSegment);
            if (offset > 0)
                request.AddParameter("offset", offset, ParameterType.UrlSegment);
            return await _hitBtcRestApi.Execute(request);
        }
    }
}
