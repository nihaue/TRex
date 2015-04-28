// See here for more info: http://azure.microsoft.com/en-us/documentation/articles/app-service-api-dotnet-triggers/

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

    [RoutePrefix("trigger/poll")]
    public class PollingTriggerSampleController : ApiController
    {

        // GET trigger/poll/diceRoll?triggerState={triggerState}&numberOfSides={numberOfSides}&targetNumber={targetNumber}

        [Trigger(TriggerType.Poll, typeof(SamplePollingResult))]
        [Metadata("Roll the Dice", "Roll the dice to see if we should trigger this time")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Bad configuration. Dice require 1 or more sides")]
        [HttpGet, Route("diceRoll")]
        public HttpResponseMessage DiceRoll(string triggerState,
                                        [Metadata("Number of Sides", "Number of sides that should be on the die that is rolled")]
                                        int numberOfSides,
                                        [Metadata("Target Number", "Trigger will fire if dice roll is above this number")]
                                        int targetNumber)
        {
            // Validate configuration
            if (numberOfSides <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                                                        "Bad configuration. Dice require 1 or more sides");
            }

            int lastRoll = 0;
            int.TryParse(triggerState, out lastRoll);
            int thisRoll = new Random().Next(numberOfSides);

            // Roll the dice
            if (thisRoll >= targetNumber /* We've hit or exceeded the target */
                    && thisRoll != lastRoll /* And this dice roll isn't the same as the last */)
            {
                // Let the Logic App know the dice roll matched
                return Request.EventTriggered(new SamplePollingResult(thisRoll),
                                                triggerState: thisRoll.ToString(),
                                                pollAgain: TimeSpan.FromSeconds(30)); 
            }
            else
            {
                // Let the Logic App know we don't have any data for it
                return Request.EventWaitPoll(retryDelay: null, triggerState: triggerState);

            }

        }
        
    }
}
