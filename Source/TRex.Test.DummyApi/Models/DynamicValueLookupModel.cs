using TRex.Metadata;

// https://github.com/nihaue/TRex/issues/10
namespace TRex.Test.DummyApi.Models
{
    public class DynamicValueLookupModel
    {
        public string Name { get; }

        [DynamicValueLookup(lookupOperationId: "GetCountries", valueCollection: "Countries", valuePath: "Id", valueTitle: "Name")]
        public string CountryOfOrigin { get; }

        public DynamicValueLookupModel(string name, string countryOfOrigin)
        {
            Name = name;
            CountryOfOrigin = countryOfOrigin;
        }
    }
}