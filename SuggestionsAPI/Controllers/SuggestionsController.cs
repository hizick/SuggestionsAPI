using Microsoft.AspNetCore.Mvc;
using SuggestionsAPI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Collections;

namespace SuggestionsAPI.Controllers
{
    [Produces("application/json")]
    [Route("/api/Suggestions")]
    public class SuggestionsController : Controller
    {
        /// <summary>
        /// Returns all Suggestion that matches the filter supplied
        /// </summary>
        /// <param name="queryObj">q is required</param>
        /// <returns></returns>

        [HttpGet]
        public IActionResult GetSuggestions(Filter queryObj)
        {
            if (string.IsNullOrEmpty(queryObj.q))
                return BadRequest();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DataSource", "GeoName.json");
            string jsonString = System.IO.File.ReadAllText(path);

            var SuggestionList = JsonConvert.DeserializeObject<List<Suggestion>>(jsonString);

            var SuggestList = QuerySuggestionListByFilter(SuggestionList, queryObj);

            var outSuggestionList = new List<Suggestion>();

            foreach (var suggestion in SuggestList)
            {
                var suggestToSearch = new Suggestion()
                {
                    GeoName = suggestion.GeoName, Latitude = suggestion.Latitude, Longitude = suggestion.Longitude
                };
                float score = GetCountOfMatchQueryHasInObject(suggestToSearch, queryObj);
                if (score > 0)
                {
                    suggestion.Score = Math.Round((score / SuggestList.Count), 2);
                    outSuggestionList.Add(suggestion);
                }
            }
            
            string json = JsonConvert.SerializeObject(new
            {
                Suggestions = outSuggestionList.OrderByDescending(s => s.Score)
            }, Formatting.Indented);

            return Ok(JObject.Parse(json));
        }

        protected List<Suggestion> QuerySuggestionListByFilter(List<Suggestion> SuggestionList, Filter queryObj) //should be private
        {
            var suggestionList = new List<Suggestion>();

            if (!string.IsNullOrEmpty(queryObj.Latitude) && string.IsNullOrEmpty(queryObj.Longitude))
                suggestionList = SuggestionList
                  .Where(x => x.GeoName.Contains(queryObj.q) ||
                  (x.Latitude.ToString().Contains(queryObj.Latitude))).ToList();
            if (string.IsNullOrEmpty(queryObj.Latitude) && !string.IsNullOrEmpty(queryObj.Longitude))
                suggestionList = SuggestionList
                  .Where(x => x.GeoName.Contains(queryObj.q) ||
                  (x.Longitude.ToString().Contains(queryObj.Longitude))).ToList();
            if (!string.IsNullOrEmpty(queryObj.Latitude) && !string.IsNullOrEmpty(queryObj.Longitude))
                suggestionList = SuggestionList
                  .Where(x => x.GeoName.Contains(queryObj.q) || (x.Latitude.ToString().Contains(queryObj.Latitude)) ||
                  (x.Longitude.ToString().Contains(queryObj.Longitude))).ToList();
            if (string.IsNullOrEmpty(queryObj.Latitude) && string.IsNullOrEmpty(queryObj.Longitude))
            {
                suggestionList = SuggestionList
                 .Where(x => x.GeoName.Contains(queryObj.q)).ToList();
            }

            return suggestionList;
        }

        protected int GetCountOfMatchQueryHasInObject(object objectToSearch, Filter queryObj) //should be private
        {
            if (objectToSearch is null || queryObj is null)
                return 0;

            int allCount = 0;
            var queryObjArr = new ArrayList { queryObj.q, queryObj.Latitude, queryObj.Longitude }.ToArray();
            var strArray = queryObjArr.Where((s => !string.IsNullOrEmpty(Convert.ToString(s)))).ToList();
            foreach (var item in strArray)
            {
                var count = 0;
                foreach (PropertyInfo property in objectToSearch.GetType().GetProperties())
                {
                    string propertyValueToString = Convert.ToString(property.GetValue(objectToSearch, null));
                    if (!string.IsNullOrEmpty(propertyValueToString))
                    {
                        var isContain = property.GetValue(objectToSearch).ToString().Contains(item.ToString());
                        if (isContain)
                            count = count + 1; //to do: make this a one liner
                    }
                }
                allCount = count + allCount;
            } 
            return allCount;
        }
    }
}
