using TRex.Metadata;

namespace TRex.Test.DummyApi.Models
{
    [DynamicSchemaLookup(lookupOperation: "Some_Other_OperationId",
                            valuePath: "Id")]
    public class DynamicSchemaNoParamsModel
    {
    }
}