using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickLearn.ApiApps.Metadata;

namespace TRex.Metadata.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    internal class DynamicValuesModel
    {

        [JsonProperty(PropertyName = Constants.OPERATION_ID)]
        public string OperationId { get; set; }

        [JsonProperty(PropertyName = Constants.PARAMETERS)]
        public JObject Parameters { get; set; }

        [JsonProperty(PropertyName = Constants.VALUE_COLLECTION)]
        public string ValueCollection { get; set; }

        [JsonProperty(PropertyName = Constants.VALUE_PATH)]
        public string ValuePath { get; set; }

        [JsonProperty(PropertyName = Constants.VALUE_TITLE)]
        public string ValueTitle { get; set; }

        [JsonProperty(PropertyName = Constants.CAPABILITY)]
        public string Capability { get; set; }
    }
}
