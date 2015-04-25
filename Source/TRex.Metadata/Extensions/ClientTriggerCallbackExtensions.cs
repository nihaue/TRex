using Microsoft.Azure.AppService.ApiApps.Service;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRex.Extensions
{
    public static class ClientTriggerCallbackExtensions
    {
        /// <summary>
        /// Invokes the callback while wrapping the result in a "body" object 
        /// to go along with the default expression completion in the 
        /// Logic App designer
        /// </summary>
        /// <param name="callback">Callback to invoke</param>
        /// <param name="runtime">Runtime settings</param>
        /// <param name="result">Result to push to the Logic App</param>
        public async static Task InvokeAsyncWithBody<TResult>(
            this ClientTriggerCallback<TResult> callback,
            Runtime runtime,
            TResult result)
        {
            // This is a hack, and it doesn't feel sound, but it works.
            JObject values = new JObject();
            values.Add("body", JToken.FromObject(result));

            await callback.InvokeAsync(runtime, values);
             
        }
    }
}
