using QuickLearn.ApiApps.SampleApiApp.Models;
using System.Web.Http;
using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Controllers
{
    public class ActionSampleController : ApiController
    {

        [Metadata("Get message", "Gets a message without requiring parameters")]
        public SampleOutputMessage Get()
        {
            return new SampleOutputMessage();
        }

        [Metadata("Create Message", "Creates a new message absolutely nowhere")]
        public SampleOutputMessage Post([FromBody,
                                            Metadata("Sample Input", "A sample input message")]
                                            SampleInputMessage sampleInput)
        {
            return new SampleOutputMessage();
        }

        [Metadata("Replace Message", Visibility = VisibilityType.Advanced)]
        public SampleOutputMessage Put([FromBody, Metadata("Sample Input")] SampleInputMessage sampleInput)
        {
            return new SampleOutputMessage();
        }

    }
}
