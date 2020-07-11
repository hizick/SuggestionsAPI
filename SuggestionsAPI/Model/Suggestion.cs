using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuggestionsAPI.Model
{
    public class Suggestion
    {
        public int GeoNameId { get; set; }
        public string GeoName { get; set; }
        public string AsciiName { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string CountryCode { get; set; }
        public DateTime ModificationDate { get; set; }
        public string Timezone { get; set; }
        public double Score { get; set; }
    }
}
