using Newtonsoft.Json.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using TRex.Metadata;
using TRex.Test.DummyApi.Models;

namespace TRex.Test.DummyApi.Controllers
{
    [RoutePrefix("test/x-ms-dynamic-schema")]
    public class DynamicSchemaLookupTestController : ApiController
    {
        [HttpGet]
        [Route("friendly-operation-as-source")]
        
        public IHttpActionResult FriendlyAsSource(
                DynamicSchemaTestModel lookupParameter,
                string noAttributeParameter)
        {
            return Ok();
        }
        
        [HttpGet]
        [Route("literal-operationid-null-parameter")]
        public IHttpActionResult LiteralOperationIdNullParameter(
                        DynamicSchemaNullParamModel lookupParameter,
                        string noAttributeParameter)
        {
            return Ok();
        }

        [HttpGet]
        [Route("literal-operationid-json-parameters")]
        public IHttpActionResult LiteralOperationIdJsonParameters(
                                        DynamicSchemaJsonParameters lookupParameter,
                                        string noAttributeParameter)
        {
            return Ok();
        }

        [HttpGet]
        [Route("literal-operationid-no-parameters")]
        public IHttpActionResult LiteralOperationIdNoParameters(
                                DynamicSchemaNoParamsModel lookupParameter,
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
