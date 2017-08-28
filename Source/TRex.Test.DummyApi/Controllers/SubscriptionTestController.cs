using System.Web.Http;
using TRex.Metadata;
using TRex.Test.DummyApi.Models;

namespace TRex.Test.DummyApi.Controllers
{
    [RoutePrefix("test/x-ms-notification-content")]
    public class SubscriptionTestController : ApiController
    {

        [HttpPost]
        [Route("$subscriptions")]
        [Trigger(TriggerType.Subscription, typeof(string), "String callback value")]
        public IHttpActionResult CreateSubscription([FromBody]SubscriptionTestModel subscription)
        {
            return Ok();
        }

        [HttpPost]
        [Route("complex/$subscriptions")]
        [Trigger(TriggerType.Subscription, typeof(NotificationContentTestModel), "Complex callback value")]
        public IHttpActionResult CreateComplexSubscription([FromBody]SubscriptionTestModel subscription)
        {
            return Ok();
        }

    }
}
