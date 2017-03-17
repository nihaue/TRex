using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRex.Metadata;

namespace TRex.Test.DummyApi.Models
{
    [DynamicSchemaLookup("FriendlyNameForOperation",
                                parameters: "sampleParam1={noAttributeParameter}&sampleParam2=hardcoded-value-here",
                                valuePath: "Id")]
    public class DynamicSchemaTestModel
    {
    }
}