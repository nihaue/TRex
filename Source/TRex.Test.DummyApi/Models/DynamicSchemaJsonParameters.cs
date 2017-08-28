using TRex.Metadata;

namespace TRex.Test.DummyApi.Models
{
    [DynamicSchemaLookup(lookupOperation: "Some_Other_OperationId",
                            parameters: @"{ ""sampleParam1"": ""{noAttributeParameter}"", ""sampleParam2"": ""hardcoded-value"" }",
                            valuePath: "Id")]
    public class DynamicSchemaJsonParameters : DynamicModelBase
    {
        public DynamicSchemaJsonParameters(object source) : base(source) { }

        public DynamicSchemaJsonParameters() { }       

    }
}