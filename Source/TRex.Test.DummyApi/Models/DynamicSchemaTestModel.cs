using System;
using TRex.Metadata;

namespace TRex.Test.DummyApi.Models
{
    [DynamicSchemaLookup("FriendlyNameForOperation",
                                parameters: "sampleParam1={noAttributeParameter}&sampleParam2=hardcoded-value-here",
                                valuePath: "Id")]
    public class DynamicSchemaTestModel : DynamicModelBase
    {
        public DynamicSchemaTestModel(object source) : base(source) { }

        public DynamicSchemaTestModel() { }

    }
}