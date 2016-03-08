using Newtonsoft.Json;
using QuickLearn.ApiApps.Metadata;
using Swashbuckle.Swagger;

namespace TRex.Metadata.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    internal class SchemaModel
    {
        public SchemaModel(Schema schema)
        {
            if (schema.@ref != null)
                Ref = schema.@ref;

            if (schema.type != null)
                Type = schema.type;
        }

        [JsonProperty(PropertyName = Constants.TYPE, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = Constants.REF, NullValueHandling = NullValueHandling.Ignore)]
        public string Ref { get; set; }

    }
}
