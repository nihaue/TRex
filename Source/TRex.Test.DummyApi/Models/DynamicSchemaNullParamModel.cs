using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRex.Metadata;

namespace TRex.Test.DummyApi.Models
{
    [DynamicSchemaLookup(lookupOperation: "Some_Other_OperationId",
                    parameters: "sampleParam1=",
                    valuePath: "Id")]
    public class DynamicSchemaNullParamModel
    {
    }
}