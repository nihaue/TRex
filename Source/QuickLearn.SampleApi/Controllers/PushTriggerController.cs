using QuickLearn.SampleApi.Models;
using System.Web.Http;
using System;
using System.Threading.Tasks;
using TRex.Metadata;
using Swashbuckle.Swagger.Annotations;
using System.Net;

namespace QuickLearn.SampleApi.Controllers
{
    [RoutePrefix("api/marketwatcher")]
    public class PushTriggerController : ApiController
    {
        private static InMemoryCallbackStore<PriceAlertConfig> callbacks = new InMemoryCallbackStore<PriceAlertConfig>();

        [HttpPost, Route("$subscriptions")]
        [Metadata("Target Price Reached", "Fires whenever the target price of a given security is reached", VisibilityType.Important)]
        [Trigger(TriggerType.Subscription, typeof(PriceAlert), "Price Alert")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Subscription created")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Invalid subscription configuration")]
        public async Task<IHttpActionResult> Subscribe([FromBody]PriceAlertConfig config)
        {
            if (config.TargetPrice < 0)
            {
                BadRequest("Cannot subscribe for negative price alerts");
            }

            var triggerId = Guid.NewGuid().ToString("N");

            await callbacks.WriteCallbackAsync(triggerId, new Uri(config.CallbackUrl), config);

            return CreatedAtRoute(nameof(Unsubscribe), new { triggerId = triggerId }, string.Empty);
            
        }

        [HttpDelete, Route("$subscriptions/{triggerId}", Name = nameof(Unsubscribe))]
        [Metadata("Unsubscribe", Visibility = VisibilityType.Internal)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> Unsubscribe(string triggerId)
        {
            await callbacks.DeleteCallbackAsync(triggerId);
            return Ok();
        }

        [HttpPut, Route("{symbol}")]
        [Metadata("PriceUpdate", Visibility = VisibilityType.Internal)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> PriceUpdate(string symbol, decimal newPrice)
        {
            // This operation can be used manually to trigger any matching configured price alerts

            // Search the in-memory store of callbacks for price alert subscriptions matching
            // the current price update
            await Task.Run(async () =>
                    Parallel.ForEach(await callbacks.ReadCallbacksAsync(),
                        async c =>
                        {
                            // Ensure the symbol matches before comparing prices
                            if (c.Configuration.Symbol != symbol) return;

                            // Determine if the current price update is worth firing a trigger for
                            if ((c.Configuration.HigherIsBetter
                                    && newPrice >= c.Configuration.TargetPrice) ||
                               (!c.Configuration.HigherIsBetter
                                    && newPrice <= c.Configuration.TargetPrice))
                            {

                                // Fire the push trigger at the callback URL that we were provided
                                await c.InvokeAsync(new PriceAlert()
                                {
                                    Symbol = symbol,
                                    Price = newPrice
                                });
                            }
                        }));

            return Ok();
        }


    }
}