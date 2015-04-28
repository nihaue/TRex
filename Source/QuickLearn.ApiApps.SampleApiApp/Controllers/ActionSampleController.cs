using QuickLearn.ApiApps.SampleApiApp.Models;
using System.Web.Http;
using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Controllers
{
    [RoutePrefix("action")]
    public class ActionSampleController : ApiController
    {

        [Metadata("Get message", "Gets a message without requiring parameters")]
        [HttpGet, Route]
        public SampleOutputMessage Get()
        {
            return new SampleOutputMessage();
        }

        [Metadata("Create Message", "Creates a new message absolutely nowhere")]
        [HttpPost, Route]
        public SampleOutputMessage Post([FromBody]
                                        [Metadata("Sample Input", "A sample input message")]
                                        SampleInputMessage sampleInput)
        {
            return new SampleOutputMessage();
        }

        [Metadata("Replace Message", Visibility = VisibilityType.Advanced)]
        [HttpPut, Route]
        public SampleOutputMessage Put([FromBody, Metadata("Sample Input")] SampleInputMessage sampleInput)
        {
            return new SampleOutputMessage();
        }

    }
}
