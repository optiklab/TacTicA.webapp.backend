using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TacTicA.Api.Cities.Models;
using Newtonsoft.Json;

namespace TacTicA.Api.Cities.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        //private static readonly HttpClient HttpClient = new HttpClient();
        //private IMemoryCache _cache;
        private readonly ILogger<CitiesController> _logger;

        public CitiesController(ILogger<CitiesController> logger, IHttpClientFactory clientFactory) //IMemoryCache memoryCache
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        // Request: https://localhost:5001/Cities/GetCityInfo/Taganrog
        // Response:
        // {"woeid":"484907","country":"RU","cityName":"Taganrog","long":"38.89688","lat":"47.23617","gmtOffset":3}
        [HttpGet("{action}/{cityName}")]
        public async Task<JsonResult> GetCityInfo(string cityName)
        {
            var result = new CityResult();

            var client = _clientFactory.CreateClient();
            using(var response = await client.GetAsync("http://api.geonames.org/searchJSON?username=optiklab&maxRows=1&q=" + cityName))
            {
                if (!response.IsSuccessStatusCode)
                {
                    result.IsError = true;
                    result.Error = $"Status: {(int)response.StatusCode}";
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var deserializedSearchResults = JsonConvert.DeserializeObject<GeoNamesSearchResult>(responseContent);

                // TODO Rework. As simple as possible.
                if (deserializedSearchResults != null && deserializedSearchResults.TotalResultsCount > 0 && deserializedSearchResults.GeoNames != null)
                {
                    var geoName = deserializedSearchResults.GeoNames.FirstOrDefault();

                    if (geoName != null)
                    {
                        result.City = new City
                        {
                            Woeid = geoName.geonameId,
                            Country = geoName.countryCode,
                            CityName = geoName.name,
                            Lat = geoName.lat,
                            Long = geoName.lng
                        };

                        using (var tzResponse = await client.GetAsync("http://api.geonames.org/timezoneJSON?username=optiklab&lng=" + geoName.lng + "&lat=" + geoName.lat))
                        {
                            if (!tzResponse.IsSuccessStatusCode)
                            {
                                result.IsError = true;
                                result.Error = $"Status: {(int)tzResponse.StatusCode}";
                            }
                            else
                            {
                                responseContent = await tzResponse.Content.ReadAsStringAsync();
                                var deserializedTz = JsonConvert.DeserializeObject<Timezone>(responseContent);

                                if (deserializedTz != null)
                                {
                                    result.City.GmtOffset = deserializedTz.gmtOffset;
                                    result.City.RawOffset = deserializedTz.rawOffset;
                                    result.City.DstOffset = deserializedTz.dstOffset;
                                    result.City.Time = deserializedTz.time ?? "";
                                    result.City.TimezoneId = deserializedTz.timezoneId ?? "";
                                }
                            }
                        }
                    }
                }
            }

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        // Request: https://localhost:5001/Cities/GetCityByIp/178_76_221_84
        // Response: {"woeid":null,"country":"RU","cityName":"Taganrog","long":"38.8969","lat":"47.2362","gmtOffset":0}
        [HttpGet("{action}/{ipAddress}")]
        public async Task<JsonResult> GetCityByIp(string ipAddress)
        {
            var result = new CityResult();

            var client = _clientFactory.CreateClient();
            ipAddress = ipAddress.Replace("_", ".");
            using(var ipInfoResponse = await client.GetAsync("https://api.ipinfodb.com/v3/ip-city/?key=debfc7c448e8b9d818084949fa23db2382f2488fbfd52e805a3e059091c65d8b&ip=" + ipAddress + "&format=json"))
            {
                if (!ipInfoResponse.IsSuccessStatusCode)
                {
                    result.IsError = true;
                    result.Error = $"Status: {(int)ipInfoResponse.StatusCode}";
                }

                var responseContent = await ipInfoResponse.Content.ReadAsStringAsync();
                var deserializedIpInfoResponse = JsonConvert.DeserializeObject<IpInfoResponse>(responseContent);

                // TODO Rework. As simple as possible.
                if (deserializedIpInfoResponse != null)
                {
                    result.City = new City
                    {
                        //Woeid = geoName.geonameId,
                        Country = deserializedIpInfoResponse.countryCode,
                        CityName = deserializedIpInfoResponse.cityName,
                        Lat = deserializedIpInfoResponse.latitude,
                        Long = deserializedIpInfoResponse.longitude,
                        //GmtOffset = deserializedTz.gmtOffset
                    };

                    using (var tzResponse = await client.GetAsync("http://api.geonames.org/timezoneJSON?username=optiklab&lng=" + result.City.Long + "&lat=" + result.City.Lat))
                    {
                        if (!tzResponse.IsSuccessStatusCode)
                        {
                            result.IsError = true;
                            result.Error = $"Status: {(int)tzResponse.StatusCode}";
                        }
                        else
                        {
                            responseContent = await tzResponse.Content.ReadAsStringAsync();
                            var deserializedTz = JsonConvert.DeserializeObject<Timezone>(responseContent);

                            if (deserializedTz != null)
                            {
                                result.City.GmtOffset = deserializedTz.gmtOffset;
                                result.City.RawOffset = deserializedTz.rawOffset;
                                result.City.DstOffset = deserializedTz.dstOffset;
                                result.City.Time = deserializedTz.time ?? "";
                                result.City.TimezoneId = deserializedTz.timezoneId ?? "";
                            }
                        }
                    }
                }
            }

            return new JsonResult(JsonConvert.SerializeObject(result));
        }

        [HttpGet("{action}/{letters}")]
        public async Task<JsonResult> GetCitySuggested(string letters)
        {
            var result = new CitiesSearchResult();
            result.Cities = new List<City>();

            var client = _clientFactory.CreateClient();
            using(var response = await client.GetAsync("http://api.geonames.org/searchJSON?username=optiklab&maxRows=10&q=" + letters))
            {
                if (!response.IsSuccessStatusCode)
                {
                    result.IsError = true;
                    result.Error = $"Status: {(int)response.StatusCode}";
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var deserializedSearchResults = JsonConvert.DeserializeObject<GeoNamesSearchResult>(responseContent);

                    // TODO Rework. As simple as possible.
                    if (deserializedSearchResults != null && deserializedSearchResults.TotalResultsCount > 0 && deserializedSearchResults.GeoNames != null)
                    {
                        foreach (var geoName in deserializedSearchResults.GeoNames)
                        {
                            if (geoName != null)
                            {
                                City cityFound = new City
                                {
                                    Woeid = geoName.geonameId,
                                    Country = geoName.countryCode,
                                    CityName = geoName.name,
                                    Lat = geoName.lat,
                                    Long = geoName.lng,
                                };

                                using (var tzResponse = await client.GetAsync("http://api.geonames.org/timezoneJSON?username=optiklab&lng=" + geoName.lng + "&lat=" + geoName.lat))
                                {
                                    if (!tzResponse.IsSuccessStatusCode)
                                    {
                                        result.IsError = true;
                                        result.Error = $"Status: {(int)tzResponse.StatusCode}";

                                        break;
                                    }
                                    else
                                    {
                                        responseContent = await tzResponse.Content.ReadAsStringAsync();
                                        var deserializedTz = JsonConvert.DeserializeObject<Timezone>(responseContent);

                                        cityFound.GmtOffset = deserializedTz.gmtOffset;
                                        cityFound.RawOffset = deserializedTz.rawOffset;
                                        cityFound.DstOffset = deserializedTz.dstOffset;
                                        cityFound.Time = deserializedTz.time ?? "";
                                        cityFound.TimezoneId = deserializedTz.timezoneId ?? "";
                                    }
                                }

                                result.Cities.Add(cityFound);
                            }
                        }
                    }
                }

            }

            return new JsonResult(JsonConvert.SerializeObject(result));
        }
    }
}
