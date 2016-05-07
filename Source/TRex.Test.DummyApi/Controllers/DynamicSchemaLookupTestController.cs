using Newtonsoft.Json.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using TRex.Metadata;

namespace TRex.Test.DummyApi.Controllers
{
    [RoutePrefix("test/x-ms-dynamic-schema")]
    public class DynamicSchemaLookupTestController : ApiController
    {
        [HttpGet]
        [Route("friendly-operation-as-source")]
        public IHttpActionResult FriendlyAsSource(
                // T-Rex will reflect this method to lookup operation id
                [DynamicSchemaLookup(nameof(FriendlySource),
                                // This will be parsed and re-created as JSON
                                parameters: "sampleParam1={noAttributeParameter}&sampleParam2=hardcoded-value-here",
                                valuePath: "Id")]
                JObject lookupParameter,
                string noAttributeParameter)
        {
            return Ok();
        }

        [HttpGet]
        [Route("no-attribute-operation-as-source")]
        public IHttpActionResult NoAttributeAsSource(
                // T-Rex will reflect this method to lookup operation id
                [DynamicSchemaLookup(nameof(NoAttributeSource),
                                        // This will be parsed and re-created as JSON
                                        parameters: "sampleParam1=hardcoded",
                                        valuePath: "Id")]
                JObject lookupParameter,
                string noAttributeParameter)
        {
            return Ok();
        }

        [HttpGet]
        [Route("literal-operationid-json-parameters")]
        public IHttpActionResult LiteralOperationIdJsonParameters(
                        [DynamicSchemaLookup(lookupOperation: "Some_Other_OperationId",
                                                parameters: @"{ ""sampleParam1"": ""{noAttributeParameter}"", ""sampleParam2"": ""hardcoded-value"" }",
                                                valuePath: "Id")]
                                        JObject lookupParameter,
                                        string noAttributeParameter)
        {
            return Ok();
        }

        [HttpGet]
        [Route("literal-operationid-null-parameter")]
        public IHttpActionResult LiteralOperationIdNullParameter(
                [DynamicSchemaLookup(lookupOperation: "Some_Other_OperationId",
                                parameters: "sampleParam1=",
                                valuePath: "Id")]
                        JObject lookupParameter,
                        string noAttributeParameter)
        {
            return Ok();
        }

        [HttpGet]
        [Route("literal-operationid-no-parameters")]
        public IHttpActionResult LiteralOperationIdNoParameters(
                                [DynamicSchemaLookup(lookupOperation: "Some_Other_OperationId",
                                                        valuePath: "Id")]
                                JObject lookupParameter,
                                string noAttributeParameter)
        {
            return Ok();
        }

        //[Metadata("Dynamic schema return result")]
        //[HttpGet]
        //[Route("dynamic-schema-return")]
        //[ResponseTypeLookup(statusCode: HttpStatusCode.OK,
        //                        lookupOperation: "FriendlySource",
        //                        parameters: "sampleParam1={sampleParam1}",
        //                        valuePath: "schema")]
        //public IHttpActionResult DynamicReturn(string sampleParam1)
        //{
        //    return Ok();
        //}


        [Metadata("Friendly Name For Operation")]
        [HttpGet]
        [Route("friendly-source")]
        public IHttpActionResult FriendlySource(string sampleParam1, string sampleParam2)
        {
            return Ok();
        }

        [HttpGet]
        [Route("no-attribute-source")]
        public IHttpActionResult NoAttributeSource(string sampleParam1)
        {
            return Ok();
        }

    }
}
