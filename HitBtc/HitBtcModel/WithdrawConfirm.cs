using Newtonsoft.Json;

#nullable disable
namespace Hitbtc.HitBtcModel
{
    public class WithdrawConfirm
    {
        [JsonProperty("result")]
        public bool Result { set; get; }
    }
}
