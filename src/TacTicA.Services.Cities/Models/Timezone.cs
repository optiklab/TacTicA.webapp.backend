using System;

namespace TacTicA.Api.Cities.Models
{
    public class Timezone
    {
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string time { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string timezoneId { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public decimal gmtOffset { get; set; }
        public decimal rawOffset { get; set; }
        public decimal dstOffset { get; set; }
    }
}