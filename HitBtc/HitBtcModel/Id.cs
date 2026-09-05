using Newtonsoft.Json;

#nullable disable
namespace Hitbtc.HitBtcModel
{
    public class IdObject
    {
        [JsonProperty("id")]
        public string Id { set; get; }
    }
}
