using Newtonsoft.Json.Linq;
using QuickLearn.SampleApi.Controllers;
using System.Dynamic;
using TRex.Metadata;

namespace QuickLearn.SampleApi.Models
{
    [DynamicSchemaLookup("GetContactInfoSchema",
        valuePath: "Schema", parameters: "type={contactType}")]
    public class ContactInfo { }
}