using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Routing;

namespace TRex.Metadata
{
    public static class PollingTriggerExtensions
    {

        /// <summary>
        /// Generate a response message indicating that the polling trigger has data to return
        /// </summary>
        /// <typeparam name="TConfig">Type of the configuration data to pass to the polling operation (usually an anonymous type)</typeparam>
        /// <typeparam name="TResult">Type of the result that the polling trigger is returning</typeparam>
        /// <param name="request">Request for which to generate a response</param>
        /// <param name="result">Result that the polling trigger is returning</param>
        /// <param name="pollAgain">Timespan that indicates when the next polling operation should occur. This should be adjusted to a very small value in the case there is additional data available.</param>
        /// <param name="triggerRouteName">Name of the route that points to the polling operation that should be invoked at the next poll interval (typically specified in the Route attribute)</param>
        /// <param name="config">Object containing as members the parameters to pass to the polling operation. The names of the properties must exactly match the input parameters.</param>
        /// <returns>Returns an HttpResponseMessage that contains a result that can be used to trigger a Logic App or Flow</returns>
        public static HttpResponseMessage EventTriggered<TConfig, TResult>(this HttpRequestMessage request,
             TResult result, TimeSpan pollAgain, string triggerRouteName, TConfig config)
        {
            var response = request.CreateResponse(HttpStatusCode.OK, result);

            response.Headers.RetryAfter = new RetryConditionHeaderValue(pollAgain);
            response.Headers.Location = new Uri(new UrlHelper(request).Link(triggerRouteName, config));

            return response;
        }

        /// <summary>
        /// Generate a response message indicating that the polling trigger has data to return
        /// </summary>
        /// <typeparam name="TConfig">Type of the configuration data to pass to the polling operation (usually an anonymous type)</typeparam>
        /// <typeparam name="TResult">Type of the result that the polling trigger is returning</typeparam>
        /// <param name="request">Request for which to generate a response</param>
        /// <param name="result">Result that the polling trigger is returning</param>
        /// <param name="triggerRouteName">Name of the route that points to the polling operation that should be invoked at the next poll interval (typically specified in the Route attribute)</param>
        /// <param name="config">Object containing as members the parameters to pass to the polling operation. The names of the properties must exactly match the input parameters.</param>
        /// <returns>Returns an HttpResponseMessage that contains a result that can be used to trigger a Logic App or Flow</returns>
        public static HttpResponseMessage EventTriggered<TConfig, TResult>(this HttpRequestMessage request,
             TResult result, string triggerRouteName, TConfig config)
        {
            var response = request.CreateResponse(HttpStatusCode.OK, result);

            response.Headers.Location = new Uri(new UrlHelper(request).Link(triggerRouteName, config));

            return response;
        }

        /// <summary>
        /// Generate a response message indicating that the polling trigger does not have data to return
        /// </summary>
        /// <typeparam name="T">Type of the configuration data to pass to the polling operation (usually an anonymous type)</typeparam>
        /// <param name="request">Request for which to generate a response</param>
        /// <param name="retryDelay">Timespan that indicates when the next polling operation should occur. Use TimeSpan.Zero to use the pre-configured defaults. This should be adjusted to a very small value in the case data is expected soon.</param>
        /// <param name="triggerRouteName">Name of the route that points to the polling operation that should be invoked at the next poll interval (typically specified in the Route attribute)</param>
        /// <param name="config">Object containing as members the parameters to pass to the polling operation. The names of the properties must exactly match the input parameters.</param>
        /// <returns>Returns an HttpResponseMessage that contains a result that can be used to avoid triggering a new instance of a Logic App or Flow</returns>
        public static HttpResponseMessage EventWaitPoll<T>(this HttpRequestMessage request, TimeSpan retryDelay, string triggerRouteName, T config)
            where T : class
        {
            var response = request.CreateResponse(HttpStatusCode.Accepted);

            response.Headers.RetryAfter = new RetryConditionHeaderValue(retryDelay);
            response.Headers.Location = new Uri(new UrlHelper(request).Link(triggerRouteName, config));

            return response;
        }
    }

}
