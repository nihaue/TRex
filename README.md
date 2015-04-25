# T-Rex Metadata Library
The T-Rex Metadata Library enables you to quickly write Web API applications that are readily consumable from the Logic App Designer. It is implemented as a set of .NET Attributes that you can use to decorate methods, parameters, and properties, and a set of filters for [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) that use those attributes to override the [swagger](http://swagger.io/) metadata generation.

So why does this exist when you could do it by hand? Because it's quite involved to do it by hand, and I don't like repeating myself. If you want to do it by hand, you certainly can. There's decent write-ups and examples [here](http://azure.microsoft.com/en-us/documentation/articles/app-service-api-dotnet-triggers/) and [here](https://code.msdn.microsoft.com/vstudio/Connector-API-App-Sample-66013c3b#content). However, let's not do that, and say that we did.

# Getting Started
To get started, you will need to install the NuGet package. From there, follow the instructions in the **Enabling T-Rex Metadata Generation** section, and then whichever other sections are applicable below.

# Enabling T-Rex Metadata Generation
To enable T-Rex Metadata Generation, head over to the **SwaggerConfig.cs** file in the **App_Start** folder, and then within the configure action passed to the **EnableSwagger** method, add the following line:

```csharp
GlobalConfiguration.Configuration.EnableSwagger(c =>
  {
    c.SingleApiVersion("v1", "QuickLearn Sample API App");
    c.ReleaseTheTRex(); // <-- This line right here makes the magic happen
  }).EnableSwaggerUi();

```

# Building an Action or Connector API App
If you are building an Action or Connector API App, T-Rex helps you make sure your actions look pretty within the Logic Apps designer, and the generated swagger metadata. It does this through the **Metadata** attribute, which can be used to provide custom friendly names, descriptions, and visibility settings for each of your API App methods, parameters, or properties of the models used by your parameters.

To use this beast, you'll need to add a pesky **using** directive (CTRL+. is your friend if you just start typing the attribute without thinking about it):
```csharp
using TRex.Metadata;
```

Now that we have that out of the way, let's see the **Metadata** attribute in action:

```csharp
        [Metadata("Create Message", "Creates a new message absolutely nowhere")] // <-- Here is is!
        public SampleOutputMessage Post([FromBody,
                                            Metadata("Sample Input", "A sample input message")] // <-- And here too!
                                            SampleInputMessage sampleInput)
        {
            return new SampleOutputMessage();
        }

        [Metadata("Replace Message", Visibility = VisibilityTypes.Advanced)] // <-- Advanced means we're making the user click to see this action
        public SampleOutputMessage Put([FromBody, Metadata("Sample Input")] SampleInputMessage sampleInput)
        {
            return new SampleOutputMessage();
        }
```

The **Metadata** attribute accepts three values: **FriendlyName**, **Description**, and **Visibility**.

| Property        | Description   
| ------------- | ------------- | 
| FriendlyName | This is the name that will be used for the item in the Logic App designer. In some cases this will be adding an x-ms-summary object in the generated swagger metadata |
| Description | This text describes the item within the generated swagger metadata |
| Visibility | Default - The item shows by default in the Logic App designer, Advanced - The item shows whenever the user clicks the ellipses (...) button to see more, Internal - The item only appears in code view |

You're not limited to using the **Metadata** attribute on actions or parameters though, you can bring it to your models as well:

```csharp
    public class SampleInputMessage
    {

        [Metadata("String Property", "A happy string input value")]
        public string StringProperty { get; set; }

        [Metadata(Visibility = VisibilityTypes.Advanced, FriendlyName = "Advanced String Property")]
        public string AdvancedStringProperty { get; set; }
       
    }
```
It's pretty straight-forward stuff, eh?

# Building a Polling Trigger API App

Now let's say that you want to trigger a Logic App by having it poll one of your actions until it hits a magical value. That's a little bit more complicated, but T-Rex is willing to help with the **Trigger** attribute.

```csharp
using Microsoft.Azure.AppService.ApiApps.Service;
using TRex.Metadata;

/* lots of unrelated code here */

        [Trigger(TriggerTypes.Poll, typeof(SamplePollingResult))]
        // ^-- Trigger attribute, that's nifty! It makes sure the Logic App knows what type we will be returning too.
        [Metadata("Check Minute", "Poll to see if minute is evenly divisible by the specified divisor")]
        // ^-- Our old friend Metadata
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
```

The **Trigger** attribute accepts two values: **TriggerType**, and **ResultType**.

| Property        | Description   
| ------------- | ------------- | 
| TriggerType | This will either be Push or Poll, Push meaning we're actively calling back to the Logic App, Poll meaning the Logic App keeps bugging us and asking if we have data |
| ResultType | Type of object that we will be returning when we have data, this determines the schema used in the metadata |

# Building a Push Trigger API App

So you want to call the Logic App on your own terms, eh? Well here's the deal, it will get in touch with you first. It will give you its name, and number (callback url in reality), and you can call it anytime you have information for it. How? Well T-Rex already gave you a **Trigger** attribute, and you already know it accepts a **Push** trigger type. But that's not all the fun, because sometimes the Logic App doesn't want to be bothered by your trigger anymore. It wants to unregister for callbacks. Well in that case, you get an **UnregisterCallbackAttribute**.

Let's see this jazz in action (also note: we're sending the callbacks with an extension method that T-Rex provides to make sure that the Logic App can successfully evaluate expressions against your result):

```csharp
using Microsoft.Azure.AppService.ApiApps.Service;
using TRex.Metadata;
using TRex.Extensions; // <-- For our custom callback logic

/* lots of unrelated code here */

        // Isolated storage would be better, but for simulation:
        public static Dictionary<string, Uri> CallbackStore = new Dictionary<string, Uri>();

        // PUT trigger/push/{triggerId}
        [Trigger(TriggerTypes.Push, typeof(SamplePushEvent))]
        [Metadata("Receive Simulated Push")]
        [HttpPut, Route("trigger/push/{triggerId}")]
        public HttpResponseMessage RegisterCallback(string triggerId /* Required magic parameter */,
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

        // Notice the UnregisterCallback attribute to ensure that we're generating the correct metadata around the
        // parameters and visibility of this method
        [UnregisterCallback, HttpDelete, Route("trigger/push/{triggerId}")]
        public HttpResponseMessage UnregisterCallback(string triggerId) // triggerId is a magic name, leave it alone
        {
            if (CallbackStore.ContainsKey(triggerId))
                CallbackStore.Remove(triggerId);

            return Request.CreateResponse(HttpStatusCode.OK);
        }


```

# Go Build Great Things!
Well, what are you waiting for? Reading documentation never built software. Go make mistakes, let those mistakes lead you into building great things!
