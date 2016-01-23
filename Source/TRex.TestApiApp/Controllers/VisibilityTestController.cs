using System.Web.Http;
using TRex.Metadata;
using TRex.TestApiApp.Models;

namespace TRex.TestApiApp.Controllers
{
    [RoutePrefix("test/x-ms-visibility")]
    public class VisibilityTestController : ApiController
    {

        [Route("properties")]
        [HttpPost]
        public IHttpActionResult PropertiesVisibility(
            [FromBody]VisibilityTestModel testModel)
        {
            return Ok();
        }

        [Route("parameters")]
        [HttpGet]
        public IHttpActionResult ParametersVisibility(
                [Metadata(Visibility = VisibilityType.Advanced)]
                string advanced,
                [Metadata(Visibility = VisibilityType.Default)]
                string @default,
                [Metadata(Visibility = VisibilityType.Important)]
                string important,
                [Metadata(Visibility = VisibilityType.Internal)]
                string @internal,
                string noattribute
            )
        {
            return Ok();
        }

        [Metadata(visibility: VisibilityType.Advanced)]
        [Route("advanced")]
        [HttpGet()]
        public IHttpActionResult Advanced()
        {
            return Ok();
        }

        [Metadata(visibility: VisibilityType.Default)]
        [Route("default")]
        [HttpGet()]
        public IHttpActionResult Default()
        {
            return Ok();
        }
        
        [Metadata(visibility: VisibilityType.Important)]
        [Route("important")]
        [HttpGet()]
        public IHttpActionResult Important()
        {
            return Ok();
        }

        [Metadata(visibility: VisibilityType.Internal)]
        [Route("internal")]
        [HttpGet()]
        public IHttpActionResult Internal()
        {
            return Ok();
        }

        [Route("no-attribute")]
        [HttpGet]
        public IHttpActionResult NoAttribute()
        {
            return Ok();
        }

    }
}
