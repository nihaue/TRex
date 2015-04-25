using Microsoft.Azure.AppService.ApiApps.Service;
using QuickLearn.ApiApps.SampleApiApp.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRex.Metadata;


namespace QuickLearn.ApiApps.SampleApiApp.Controllers
{
    public class PollingTriggerSampleController : ApiController
    {

        // GET trigger/poll/{divisor}
        [Trigger(TriggerType.Poll, typeof(SamplePollingResult))]
        [Metadata("Check Minute", "Poll to see if minute is evenly divisible by the specified divisor")]
        [HttpGet, Route("trigger/poll/{divisor}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Divide by zero fail")]
        public HttpResponseMessage Poll([Metadata("Minute Divisible By",
                                                        "Trigger when minute is evenly divisible by this")]
                                                        int divisor,
                                                        string triggerState /* Required Magic Parameter */)
        {
            if (divisor == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad divisor");
            }

            // It's not the trigger minute, or we've already fired for this minute 
            if (DateTime.UtcNow.Minute % divisor != 0 ||
                (!string.IsNullOrWhiteSpace(triggerState) && Convert.ToInt32(triggerState) == DateTime.UtcNow.Minute))
            {
                return Request.EventWaitPoll(TimeSpan.FromMinutes(1), triggerState);
            }
            else
            {
                return Request.EventTriggered(new SamplePollingResult(), DateTime.UtcNow.Minute.ToString(), TimeSpan.FromMinutes(1));
            }

        }
        
    }
}
