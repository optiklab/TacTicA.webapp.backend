using Newtonsoft.Json;
using System.Collections.Generic;

namespace TacTicA.Api.Cities.Models
{
    public class CitiesSearchResult
    {
        [JsonProperty("cities")]
        public List<City> Cities { get; set; }
        [JsonProperty("isError")]
        public bool IsError { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}