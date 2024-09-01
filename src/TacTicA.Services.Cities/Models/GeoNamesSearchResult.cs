using System.Collections.Generic;

namespace TacTicA.Api.Cities.Models
{
    public class GeoNamesSearchResult
    {
        public List<GeoName> GeoNames { get; set; }
        public int TotalResultsCount { get; set; }
    }
}