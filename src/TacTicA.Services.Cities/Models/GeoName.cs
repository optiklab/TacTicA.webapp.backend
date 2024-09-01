using System;

namespace TacTicA.Api.Cities.Models
{
    public class GeoName
    {
        public string name { get; set; }
        public string countryId { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
        public string adminCode1 { get; set; }
        public string adminName1 { get; set; }
        public string geonameId { get; set; }
        public string toponymName { get; set; }
        public string fcl { get; set; }
        public string fclName { get; set; }
        public string fcode { get; set; }
        public long population { get; set; }
    }
}