using System.Web.Http;
using TRex.Test.DummyApi.Models;

namespace TRex.Test.DummyApi.Controllers
{
    [RoutePrefix("test/jsonproperty")]
    public class JsonPropertyController : ApiController
    {
        [HttpGet]
        [Route("result-includes-jsonproperty")]
        public JsonPropertyModel GetJsonPropertyInResultModel()
        {
            return new JsonPropertyModel() { SampleProperty = "SampleValue" };
        }
    }
}