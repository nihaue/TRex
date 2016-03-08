using Newtonsoft.Json;
using QuickLearn.ApiApps.Metadata;

namespace TRex.Metadata.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    internal class NotificationContentModel
    {

        [JsonProperty(PropertyName = Constants.DESCRIPTION)]
        public string Description { get; set; }

        [JsonProperty(PropertyName = Constants.SCHEMA)]
        public SchemaModel Schema { get; set; }

    }
    
}
