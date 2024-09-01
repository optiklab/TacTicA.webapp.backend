using Newtonsoft.Json;

namespace TacTicA.Api.Cities.Models
{
    public class City
    {
        [JsonProperty("woeid")]
        public string Woeid { get; set; }

        [JsonProperty("city")]
        public string CityName { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("long")]
        public string Long { get; set; }
        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("gmtOffset")]
        public decimal GmtOffset { get; set; }

        [JsonProperty("dstOffset")]
        public decimal DstOffset { get; set; }

        [JsonProperty("rawOffset")]
        public decimal RawOffset { get; set; }

        [JsonProperty("timezoneId")]
        public string TimezoneId { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }
    }
}