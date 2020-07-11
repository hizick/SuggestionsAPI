using Microsoft.AspNetCore.Mvc;
using SuggestionsAPI.Controllers;
using SuggestionsAPI.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using Xunit;

namespace SuggestionsAPI.Test
{
    public class SuggestionsControllerTest
    {
        private readonly SuggestionsController _controller;
        public SuggestionsControllerTest()
        {
            _controller = new SuggestionsController();
        }

        /*[Fact]
        public void GetCountOfMatchQueryHasInObject_ObjectAndSearchQueryPassed_ReturnsCountOfTimesSearchMatchesObjectPropertiesValue()
        {
            var filterObj = new Filter() { q = "Manchester", Latitude = null };
            var suggestion = new Suggestion() { GeoName = "Manchester", Latitude = 0, AsciiName = "Manchester" };

            var countOfMatches = _controller.GetCountOfMatchQueryHasInObject(suggestion, filterObj);

            Assert.Equal(2, countOfMatches);
        }*/
        /*
        [Fact]
        public void QuerySuggestionListByFilter_FilterObjectPassed_ReturnAListOfSuggestion()
        {
            var filterObj = new Filter() { q = "Manchester", Latitude = null };
            var suggestion = new Suggestion() { GeoName = "Manchester", Latitude = 0 };
            var suggestionList = new List<Suggestion>();
            suggestionList.Add(suggestion);

            var result = _controller.QuerySuggestionListByFilter(suggestionList, filterObj);

            var listResult = Assert.IsType<List<Suggestion>>(result);
            Assert.Single(listResult);
        }*/

        [Fact]
        public void GetSuggestions_MandatoryFilterPropertyNotPassed_ReturnBadRequest()
        {
            var filterObj = new Filter() { q = null};

            var badRequestResult = _controller.GetSuggestions(filterObj);

            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public void GetSuggestions_SearchStringPassed_ReturnAtLeastOneJsonObject()
        {
            var filterObj = new Filter() { q = "Manchester" };

            var result = _controller.GetSuggestions(filterObj);

            var objResult = Assert.IsType<OkObjectResult>(result);
            var jsonData = objResult.Value;
            Assert.NotNull(jsonData);
        }


    }
}

/*
 * Type type = typeof(SuggestionsController);
            var instanceOfClass = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic)
            .Where(x => x.Name == "QuerySuggestionListByFilter" && x.IsPrivate).First();

            var result = (List<Suggestion>)method.Invoke(instanceOfClass, new object[] { suggestionList, filterObj });

 not testing private methods
     */
       