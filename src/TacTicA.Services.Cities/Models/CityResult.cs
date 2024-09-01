using Newtonsoft.Json;

namespace TacTicA.Api.Cities.Models
{
    public class CityResult
    {
        [JsonProperty("city")]
        public City City { get; set; }
        [JsonProperty("isError")]
        public bool IsError { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}