using Microsoft.Azure.AppService.ApiApps.Service;
using QuickLearn.ApiApps.SampleApiApp.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TRex.Extensions;
using TRex.Metadata;

namespace QuickLearn.ApiApps.SampleApiApp.Controllers
{
    public class PushTriggerSampleController : ApiController
    {

        // See here for more info: http://azure.microsoft.com/en-us/documentation/articles/app-service-api-dotnet-triggers/

        public static Dictionary<string, Uri> CallbackStore = new Dictionary<string, Uri>();

        // PUT trigger/push/{triggerId}
        [Trigger(TriggerTypes.Push, typeof(SamplePushEvent))]
        [Metadata("Receive Simulated Push")]
        [HttpPut, Route("trigger/push/{triggerId}")]
        public HttpResponseMessage RegisterCallback(string triggerId /* Required Magic Parameter */,
                                     [FromBody] TriggerInput<SampleInputMessage, SamplePushEvent> parameters)
        {

            // Callback will occur every 30 seconds, and will happen 10 times
            // This is purely for the sake of simulation

            CallbackStore.Add(triggerId, parameters.GetCallback().CallbackUri);
            
            Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Task.Delay(30000).ContinueWith((a) =>
                    {
                        // Callback has been pulled, let's get out of here
                        if (!CallbackStore.ContainsKey(triggerId)) return;

                        // Get the callback from the store, and make things happen, we've been
                        // triggered
                        var callback = 
                            new ClientTriggerCallback<SamplePushEvent>(CallbackStore[triggerId]);

                        callback.InvokeAsyncWithBody(Runtime.FromAppSettings(),
                            new SamplePushEvent() { SampleStringProperty
                                                        = parameters.inputs.StringProperty });

                    }).Wait();
                }
            });
            
            return Request.PushTriggerRegistered(parameters.GetCallback());
        }

        [UnregisterCallback, HttpDelete, Route("trigger/push/{triggerId}")]
        public HttpResponseMessage UnregisterCallback(string triggerId)
        {
            if (CallbackStore.ContainsKey(triggerId))
                CallbackStore.Remove(triggerId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpPost, Route("trigger/fire/all")]
        [Metadata(Visibility = VisibilityTypes.Internal)]
        [SwaggerResponse(HttpStatusCode.OK, "Indicates the operation completed without error", typeof(int))]
        public HttpResponseMessage FireTheTriggers()
        {
            int count = 0;
            foreach (var item in CallbackStore)
            {
                var callback = new ClientTriggerCallback<SamplePushEvent>(item.Value);

                callback.InvokeAsyncWithBody(Runtime.FromAppSettings(),
                    new SamplePushEvent()
                    {
                        SampleStringProperty = "Fired by simulation push event"
                    });

                count++;

            }

            return Request.CreateResponse<int>(HttpStatusCode.OK, count);
        }
    }
}
