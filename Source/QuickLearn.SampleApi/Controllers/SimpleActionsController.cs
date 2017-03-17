using QuickLearn.SampleApi.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;
using TRex.Metadata;

namespace QuickLearn.SampleApi.Controllers
{

    [RoutePrefix("api/time")]
    public class SimpleActionsController : ApiController
    {
        [HttpGet, Route("until/{month}/{day}")]
        [Metadata("Get Time Remaining", "Gets the amount of time remaining until the target date")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TimeRemaining))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Invalid target date")]
        public IHttpActionResult GetCountdown(
            [Metadata("Target Month", "Month part of the target date", VisibilityType.Important)]
            int month,
            [Metadata("Target Day", "Day part of the target date", VisibilityType.Important)]
            int day,
            [Metadata("Target Year", "Year part of the target date", VisibilityType.Advanced)]
            int year = 2038)
        {

            DateTime targetDate = DateTime.UtcNow;

            try
            {
                targetDate = new DateTime(year, month, day);
            }
            catch (Exception)
            {
                BadRequest("Failed to create target date/time based on inputs provided");
            }

            return Ok(TimeRemaining.FromTimeSpan(targetDate - DateTime.UtcNow));

        }
    }
}
