using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SuggestionsAPI.Model
{
    public class Filter
    {
        [Required] public string q { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
