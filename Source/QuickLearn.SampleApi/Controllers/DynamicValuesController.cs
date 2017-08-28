using System.Globalization;
using System.Web.Http;
using System.Linq;
using System.Collections.Generic;
using QuickLearn.SampleApi.Models;
using TRex.Metadata;
using System.Net;
using Swashbuckle.Swagger.Annotations;

namespace QuickLearn.SampleApi.Controllers
{

    [RoutePrefix("api/food")]
    public class DynamicValuesController : ApiController
    {

        [HttpPost, Route("vote/{country}")]
        [Metadata("Vote for Favorite", "Votes for favorite foods by location", VisibilityType.Important)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult VoteFavoriteFood([FromUri]
            [Metadata("Country")]
            [DynamicValueLookup("GetCountries", valueCollection: "Countries", valuePath: "Id", valueTitle: "Name")]
            string country,
            [FromBody]FavoriteFoodVoteModel model)
        {
            return Ok();
        }

        [HttpGet, Route("favorite/{country}/{stateprovince}")]
        [Metadata("Favorite Food Lookup", "Looks up favorite foods by location", VisibilityType.Important)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(FavoriteFoodModel))]
        public IHttpActionResult GetFavoriteFood(
            [Metadata("Country")]
            [DynamicValueLookup("GetCountries", valueCollection: "Countries", valuePath: "Id", valueTitle: "Name")]
            string country,

            [Metadata("State or Province")]
            [DynamicValueLookup("GetStates", "country={country}", "States", "Id", "Name")]
            string stateprovince)
        {
            FavoriteFoodModel result = new FavoriteFoodModel();

            if (country == "US" && stateprovince == "WA")
            {
                result.FavoriteFood = "Apples";
            }
            else if (country == "AU")
            {
                result.FavoriteFood = "Upsidedown Cake";
            }
            else
            {
                result.FavoriteFood = "Potatoes";
            }

            return Ok(result);
        }


        [HttpGet, Route("countries")]
        [Metadata("GetCountries", Visibility = VisibilityType.Internal)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CountryList))]
        public IHttpActionResult GetListOfCountries()
        {
            return Ok(new CountryList()
            {
                Countries = LocationCodes.Countries
                                .Select(c => new CountryItem()
                                        { Id = c.Value, Name = c.Key })
                                .ToArray()
            });
        }

        [HttpGet, Route("states/{country}")]
        [Metadata("GetStates", Visibility = VisibilityType.Internal)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StateListModel))]
        public IHttpActionResult GetListOfStates(string country)
        {
            var result = new StateListModel();
            
            switch (country)
            {
                case "US":
                    result.States = LocationCodes.States
                                        .Select(s => new StateItem()
                                        { Id = s.Value, Name = s.Key })
                                        .ToArray();
                    break;

                case "CA":
                    result.States = LocationCodes.Provinces
                                        .Select(p => new StateItem()
                                        { Id = p.Value, Name = p.Key })
                                        .ToArray();
                    break;

                default:
                    result.States = new StateItem[0];
                    break;

            }

            return Ok(result);
        }

    }

    public static class LocationCodes
    {

        private static object syncRoot = new object();
        private static void populate()
        {
            lock (syncRoot)
            {
                states = new Dictionary<string, string>();
                provinces = new Dictionary<string, string>();
                countries = new Dictionary<string, string>();

                states.Add("APO/FPO Americas", "AA");
                states.Add("APO/FPO Africa, Canada, Europe, Middle East", "AE");
                states.Add("APO/FPO Pacific", "AP");
                states.Add("Alabama", "AL");
                states.Add("Alaska", "AK");
                states.Add("American Samoa", "AS");
                states.Add("Arizona", "AZ");
                states.Add("Arkansas", "AR");
                states.Add("California", "CA");
                states.Add("Colorado", "CO");
                states.Add("Connecticut", "CT");
                states.Add("Delaware", "DE");
                states.Add("District of Columbia", "DC");
                states.Add("Federated States of Micronesia", "FM");
                states.Add("Florida", "FL");
                states.Add("Georgia", "GA");
                states.Add("Guam", "GU");
                states.Add("Hawaii", "HI");
                states.Add("Idaho", "ID");
                states.Add("Illinois", "IL");
                states.Add("Indiana", "IN");
                states.Add("Iowa", "IA");
                states.Add("Kansas", "KS");
                states.Add("Kentucky", "KY");
                states.Add("Louisiana", "LA");
                states.Add("Maine", "ME");
                states.Add("Marshall Islands", "MH");
                states.Add("Maryland", "MD");
                states.Add("Massachusetts", "MA");
                states.Add("Michigan", "MI");
                states.Add("Minnesota", "MN");
                states.Add("Mississippi", "MS");
                states.Add("Missouri", "MO");
                states.Add("Montana", "MT");
                states.Add("Nebraska", "NE");
                states.Add("Nevada", "NV");
                states.Add("New Hampshire", "NH");
                states.Add("New Jersey", "NJ");
                states.Add("New Mexico", "NM");
                states.Add("New York", "NY");
                states.Add("North Carolina", "NC");
                states.Add("North Dakota", "ND");
                states.Add("Northern Mariana Islands", "MP");
                states.Add("Ohio", "OH");
                states.Add("Oklahoma", "OK");
                states.Add("Oregon", "OR");
                states.Add("Palau", "PW");
                states.Add("Pennsylvania", "PA");
                states.Add("Puerto Rico", "PR");
                states.Add("Rhode Island", "RI");
                states.Add("South Carolina", "SC");
                states.Add("South Dakota", "SD");
                states.Add("Tennessee", "TN");
                states.Add("Texas", "TX");
                states.Add("Utah", "UT");
                states.Add("Vermont", "VT");
                states.Add("Virgin Islands", "VI");
                states.Add("Virginia", "VA");
                states.Add("Washington", "WA");
                states.Add("West Virginia", "WV");
                states.Add("Wisconsin", "WI");
                states.Add("Wyoming", "WY");

                provinces.Add("Alberta", "AB");
                provinces.Add("British Columbia (Colombie-Britannique)", "BC");
                provinces.Add("Manitoba", "MB");
                provinces.Add("New Brunswick (Nouveau-Brunswick)", "NB");
                provinces.Add("Newfoundland and Labrador (Terre-Neuve-et-Labrador)", "NL");
                provinces.Add("Northwest Territories (Territoires du Nord-Ouest)", "NT");
                provinces.Add("Nova Scotia (Nouvelle-Écosse)", "NS");
                provinces.Add("Nunavut", "NU");
                provinces.Add("Ontario", "ON");
                provinces.Add("Prince Edward Island (Île-du-Prince-Édouard)", "PE");
                provinces.Add("Quebec (Québec)", "QC");
                provinces.Add("Saskatchewan", "SK");
                provinces.Add("Yukon", "YT");

                CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

                var distinctCountries = (from r in
                                             from c in cultures
                                             select new RegionInfo(c.Name)
                                                orderby r.EnglishName ascending
                                                select new KeyValuePair<string, string>(string.Format(
                                                        "{1} ({0})",
                                                        r.EnglishName, r.NativeName),
                                                        r.TwoLetterISORegionName.ToUpperInvariant()))
                                 .Distinct();

                foreach (var pair in distinctCountries)
                {
                    countries.Add(pair.Key, pair.Value);
                }
            }
        }

        private static Dictionary<string, string> countries;
        public static Dictionary<string, string> Countries
        {
            get
            {
                if (countries == null) { populate(); return countries; }

                return countries;
            }
        }

        private static Dictionary<string, string> states;
        public static Dictionary<string, string> States
        {
            get
            {
                if (states == null) { populate(); return states; }

                return states;
            }
        }

        private static Dictionary<string, string> provinces;
        public static Dictionary<string, string> Provinces
        {
            get
            {
                if (provinces == null) { populate(); return provinces; }

                return provinces;
            }
        }

    }
}
