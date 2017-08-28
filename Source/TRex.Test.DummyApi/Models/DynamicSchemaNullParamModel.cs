using TRex.Metadata;

namespace TRex.Test.DummyApi.Models
{
    [DynamicSchemaLookup(lookupOperation: "Some_Other_OperationId",
                    parameters: "sampleParam1=",
                    valuePath: "Id")]
    public class DynamicSchemaNullParamModel : DynamicModelBase
    {
        public DynamicSchemaNullParamModel(object source) : base(source) { }

        public DynamicSchemaNullParamModel() { }

    }
}