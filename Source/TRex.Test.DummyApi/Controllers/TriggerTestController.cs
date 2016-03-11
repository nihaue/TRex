using System.Web.Http;
using TRex.Metadata;

namespace TRex.Test.DummyApi.Controllers
{
    // Reference:
    // https://azure.microsoft.com/en-us/documentation/articles/powerapps-develop-api/

    [RoutePrefix("test/x-ms-trigger")]
    public class TriggerTestController : ApiController
    {

        [HttpGet]
        [Route("batched")]
        [Trigger(BatchMode = BatchMode.Batched)]
        [Metadata("Batched Trigger", "This operation triggers a single flow per item in the result array")]
        public IHttpActionResult BatchedTrigger()
        {
            return Ok();
        }

        [HttpGet]
        [Route("single")]
        [Trigger(BatchMode = BatchMode.Single)]
        [Metadata("Single Trigger", "This operation triggers a single flow")]
        public IHttpActionResult SingleTrigger()
        {
            return Ok();
        }

        [Trigger, HttpGet, Route("single-implicit")]
        [Metadata("Single By Default Trigger", "This operation triggers a single flow, because the attribute does not specify a batch mode")]
        public IHttpActionResult SingleByDefaultTrigger()
        {
            return Ok();
        }

        [HttpGet]
        [Route("regular-op")]
        [Metadata("Regular Operation", "This operation does not represent a trigger.")]
        public IHttpActionResult RegularOp()
        {
            return Ok();
        }
    }
}