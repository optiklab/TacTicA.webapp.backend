using System;

namespace TacTicA.Api.Cities.Models
{
    /*
    Example:
    {
        "statusCode" : "OK",
        "statusMessage" : "",
        "ipAddress" : "176.56.192.1",
        "countryCode" : "GB",
        "countryName" : "United Kingdom of Great Britain and Northern ",
        "regionName" : "England",
        "cityName" : "Lenwade",
        "zipCode" : "NR9",
        "latitude" : "52.7222",
        "longitude" : "1.10601",
        "timeZone" : "+01:00"
    }
    */
    public class IpInfoResponse
    {
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
        public string ipAddress { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string regionName { get; set; }
        public string cityName { get; set; }
        public string zipCode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string timeZone { get; set; }
    }
}