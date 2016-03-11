using System.Web.Http;
using TRex.Metadata;
using TRex.Test.DummyApi.Models;

namespace TRex.Test.DummyApi.Controllers
{
    [RoutePrefix("test/x-ms-summary")]
    public class SummaryTestController : ApiController
    {

        [HttpPost]
        [Route("operation")]
        [Metadata("Operation friendly name", "Operation description")]
        public IHttpActionResult Operation(
            [Metadata("Parameter friendly name", "Parameter description")]
            string parameter,
            [FromBody]SummaryTestModel testModel)
        {
            return Ok();
        }

        [HttpPost]
        [Route("no-attribute")]
        public IHttpActionResult NoAttribute(
            string parameter,
            [FromBody]NoAttributeTestModel testModel)
        {
            return Ok();
        }
            

    }
}
