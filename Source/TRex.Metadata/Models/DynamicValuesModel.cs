using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TRex.Metadata.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    internal class DynamicValuesModel
    {

        [JsonProperty(PropertyName = "operationId")]
        public string OperationId { get; set; }

        [JsonProperty(PropertyName = "parameters")]
        public JObject Parameters { get; set; }

        [JsonProperty(PropertyName = "value-collection")]
        public string ValueCollection { get; set; }

        [JsonProperty(PropertyName = "value-path")]
        public string ValuePath { get; set; }

        [JsonProperty(PropertyName = "value-title")]
        public string ValueTitle { get; set; }

    }
}
