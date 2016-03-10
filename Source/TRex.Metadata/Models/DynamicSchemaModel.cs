using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickLearn.ApiApps.Metadata;

namespace TRex.Metadata.Models
{
    internal class DynamicSchemaModel
    {

        [JsonProperty(PropertyName = Constants.OPERATION_ID)]
        public string OperationId { get; set; }

        [JsonProperty(PropertyName = Constants.PARAMETERS)]
        public JObject Parameters { get; set; }
        
        [JsonProperty(PropertyName = Constants.VALUE_PATH)]
        public string ValuePath { get; set; }
        
    }
}
