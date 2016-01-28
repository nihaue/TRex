using System.Web.Http;
using TRex.TestApiApp.Models;

namespace TRex.TestApiApp.Controllers
{
    [RoutePrefix("test/x-ms-notification-content")]
    public class SubscriptionTestController : ApiController
    {

        [HttpPost]
        [Route("$subscriptions")]
        public IHttpActionResult CreateSubscription([FromBody]SubscriptionTestModel subscription)
        {
            return Ok();
        }

    }
}
