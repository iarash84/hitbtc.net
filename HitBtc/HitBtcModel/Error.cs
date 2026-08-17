using Newtonsoft.Json;

#nullable disable
namespace Hitbtc.HitBtcModel
{
    public class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", Code, Message, Description);
        }
    }
}
