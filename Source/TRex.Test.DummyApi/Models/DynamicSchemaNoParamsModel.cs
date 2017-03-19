using TRex.Metadata;

namespace TRex.Test.DummyApi.Models
{
    [DynamicSchemaLookup(lookupOperation: "Some_Other_OperationId",
                            valuePath: "Id")]
    public class DynamicSchemaNoParamsModel : DynamicModelBase
    {
        public DynamicSchemaNoParamsModel(object source) : base(source) { }

        public DynamicSchemaNoParamsModel() { }

    }
}