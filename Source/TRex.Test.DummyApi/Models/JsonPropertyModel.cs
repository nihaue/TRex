using Newtonsoft.Json;
using TRex.Metadata;

namespace TRex.Test.DummyApi.Models
{
    //https://github.com/nihaue/TRex/issues/11
    public class JsonPropertyModel
    {
        [JsonProperty("sampleProperty")]
        [Metadata("Sample Property", "This is the description", VisibilityType.Advanced)]
        public string SampleProperty { get; set; }


    }
}