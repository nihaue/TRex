// See here for more info: http://azure.microsoft.com/en-us/documentation/articles/app-service-api-dotnet-triggers/

using Microsoft.Azure.AppService.ApiApps.Service;
using QuickLearn.ApiApps.SampleApiApp.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TRex.Extensions;
using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Controllers
{
    [RoutePrefix("trigger/push")]
    public class PushTriggerSampleController : ApiController
    {

        // Simulated storage for callback data
        public static Dictionary<string, SampleStoredCallback> CallbackStore
            = new Dictionary<string, SampleStoredCallback>();
        
        // PUT trigger/push/{triggerId}

        [Trigger(TriggerType.Push, typeof(SamplePushEvent))]
        [Metadata("Receive Simulated Push")]
        [HttpPut, Route("{triggerId}")]
        public HttpResponseMessage RegisterCallback(
                                        string triggerId,
                                        [FromBody]TriggerInput<SamplePushConfig, SamplePushEvent> parameters)
        {

            // Store the callback for later use
            CallbackStore[triggerId] = new SampleStoredCallback()
            {
                 SampleConfigFromLogicApp = parameters.inputs, 
                 CallbackUri = parameters.GetCallback().CallbackUri
            };
                        
            // Notify the Logic App that the callback was registered
            return Request.PushTriggerRegistered(parameters.GetCallback());

        }

        // DELETE trigger/push/{triggerId}

        [UnregisterCallback]
        [SwaggerResponse(HttpStatusCode.NotFound, "The trigger id had no callback registered")]
        [HttpDelete, Route("{triggerId}")]
        public HttpResponseMessage UnregisterCallback(string triggerId)
        {

            if (!CallbackStore.ContainsKey(triggerId))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The trigger had no callback registered");
            
            // Remove the stored callback by trigger id
            CallbackStore.Remove(triggerId);
            return Request.CreateResponse(HttpStatusCode.OK);

        }

        // POST trigger/push/all

        [Metadata("Fire Push Triggers", "Fires all Logic Apps awaiting callback", VisibilityType.Internal)]
        [SwaggerResponse(HttpStatusCode.OK, "Indicates the operation completed without error", typeof(string))]
        [HttpPost, Route("all", Order = 0)]
        public async Task<HttpResponseMessage> FireTheTriggers()
        {

            // This action is the simulation of some external force causing
            // the trigger to fire for all awaiting Logic Apps where
            // our custom configuration value has been satisfied

            var readyCallbacks = from callback in CallbackStore.Values
                                    where callback.SampleConfigFromLogicApp.QuietHour != DateTime.UtcNow.Hour
                                    select callback;

            foreach (var storedCallback in readyCallbacks)
            {
                var callback = new ClientTriggerCallback<SamplePushEvent>(storedCallback.CallbackUri);

                await callback.InvokeAsyncWithBody(
                                    Runtime.FromAppSettings(),
                                    new SamplePushEvent()
                                    {
                                        SampleStringProperty = 
                                            string.Format("Fired with configuration data: {0}", 
                                                storedCallback.SampleConfigFromLogicApp.QuietHour)
                                    });
            }

            return Request.CreateResponse<string>(HttpStatusCode.OK,
                    string.Format("{0} triggers were fired.", readyCallbacks.Count()));

        }

    }
}
