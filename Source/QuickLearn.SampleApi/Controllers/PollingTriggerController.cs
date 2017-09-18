using AngleSharp;
using QuickLearn.SampleApi.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using TRex.Metadata;
using System.Linq;

namespace QuickLearn.SampleApi.Controllers
{

    [RoutePrefix("api/webpage")]
    public class PollingTriggerController : ApiController
    {

        
        [HttpGet, Route("element/text", Name = nameof(PollWebPageContent))]
        [Metadata("Poll For Updated Web Page Content")]
        [Trigger(TriggerType.PollingSingle, typeof(PageQueryResult), "Page Element Results")]
        public async Task<IHttpActionResult> PollWebPageContent(
                            [Metadata("Web Address", @"Site to poll for changes (e.g., ""https://msn.com/weather/news"")")]
                            string address,

                            [Metadata("Element Selector", @"Selector for desired content (e.g., ""li.headlines>a>h3"")")]
                            string selector,

                            [Metadata("Last result", "State data stored by the Logic App Runtime", Visibility = VisibilityType.Internal)]
                            string lastResult = null
        )
        {

            PageQueryResult result = await getMatchingElements(address, selector);

            var paramsForNextPoll = new
            {
                address = address,
                selector = selector,
                lastResult = result.GetHash()
            };
            
            if (null != lastResult && result.GetHash() != lastResult)
            {
                // 200 OK Response with result content
                // Location header points to the next address to poll

                // In this case, since we are triggering the event:
                //  TimeSpan.Zero = Poll immediately (not at pre-configured intereval).
                return ResponseMessage(Request.EventTriggered(result, TimeSpan.Zero, nameof(PollWebPageContent), paramsForNextPoll));
            }
            else
            {
                // This code will execute under the following conditions:
                // If this has never been triggered before, pass a hash of the result back to the runtime as a baseline
                // If this has been run before, but the result is the same as it was the last time around

                // 202 Accepted Response with no content
                // Location header points to the next address to poll

                // In this case, since we are not triggering the event:
                //  TimeSpan.Zero = Poll at pre-configured interval in Logic App
                return ResponseMessage(Request.EventWaitPoll(TimeSpan.Zero, nameof(PollWebPageContent), paramsForNextPoll));
            }
            
        }



        #region Helper code that provides implementation of the page content lookup

        /// <summary>
        /// Looks up data on a given web page by searching the DOM for elements matching the configured selector
        /// </summary>
        /// <param name="address">Address of the web page to retrieve</param>
        /// <param name="selector">Element selector to server as the basis for the query</param>
        /// <returns>Returns a PageQueryResult object that contains an array of the text content for matching elements</returns>
        private static async Task<PageQueryResult> getMatchingElements(string address, string selector)
        {

            // Setup AngleSharp to be able to load external pages
            var config = AngleSharp.Configuration.Default.WithDefaultLoader();

            // Load the DOM for the page at the requested address
            var document = await BrowsingContext.New(config).OpenAsync(address);

            // Perform the query to get all matching fragments on the site
            var matchingFragments = document.QuerySelectorAll(selector);

            PageQueryResult result = new PageQueryResult()
            {
                Results = matchingFragments.Select(m => m.TextContent).ToArray()
            };

            return result;
        }

        #endregion

    }
}
