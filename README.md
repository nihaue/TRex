
# T-Rex Metadata Library
<img src="https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/PackageIcon.png" align="right" />The T-Rex Metadata Library enables you to quickly write Web API applications that are readily consumable from the Logic App Designer. It is implemented as a set of .NET Attributes that you can use to decorate methods, parameters, and properties, and a set of filters for [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) that use those attributes to override the [swagger](http://swagger.io/) metadata generation.

So let's go ahead and get started!

# Getting Started
To get started, you will need to [install the **TRex** NuGet package](https://www.nuget.org/packages/TRex/). From there, follow the instructions in the **Enabling T-Rex Metadata Generation** section, and then whichever other sections are applicable below. If you want to get straight to some working code, you can also [look through the sample application](https://github.com/nihaue/TRex/tree/master/Source/QuickLearn.ApiApps.SampleApiApp) which implements a set of simple actions, a polling trigger, and a push trigger.

# Enabling T-Rex Metadata Generation
To enable T-Rex Metadata Generation, head over to the **SwaggerConfig.cs** file in the **App_Start** folder. Add the requisite using directive:
```csharp
using TRex.Metadata;
```

Then within the configure action passed to the **EnableSwagger** method, add the following line:
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
        [Metadata("Create Message", "Creates a new message absolutely nowhere")]
        // ^-- Here it is!
        public SampleOutputMessage Post([FromBody,
                      /* and here too --> */ Metadata("Sample Input", "A sample input message")] 
                                             SampleInputMessage sampleInput)
        {
            return new SampleOutputMessage();
        }
```
_**NOTE:** This is an extract from the the companion sample app. You can find this file [here](https://github.com/nihaue/TRex/blob/master/Source/QuickLearn.ApiApps.SampleApiApp/Controllers/ActionSampleController.cs)._

Here's what that would look like in a Logic App (it's the one on the far right):
![Create Message action within a Logic App](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/SampleWithinLogicApp.png "Create Message action within a Logic App")

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
        // ^-- Here it is again!
        public string StringProperty { get; set; }

        [Metadata(Visibility = VisibilityType.Advanced, FriendlyName = "Advanced String Property")]
        public string AdvancedStringProperty { get; set; }
       
    }
```
_**NOTE:** This is an extract from the the companion sample app. You can find this file [here](https://github.com/nihaue/TRex/blob/master/Source/QuickLearn.ApiApps.SampleApiApp/Models/SampleInputMessage.cs)._

It's pretty straight-forward stuff, eh?

# Building a Polling Trigger API App

Now let's say that you want to trigger a Logic App by having it poll one of your actions until it hits a magical value. That's a little bit more complicated, but T-Rex is willing to help with the **Trigger** attribute.

```csharp
using Microsoft.Azure.AppService.ApiApps.Service;
using TRex.Metadata;

/* lots of unrelated code here */

        [Trigger(TriggerType.Poll, typeof(SamplePollingResult))]
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
_**NOTE:** This is an extract from the the companion sample app. You can find this file [here](https://github.com/nihaue/TRex/blob/master/Source/QuickLearn.ApiApps.SampleApiApp/Controllers/PollingTriggerSampleController.cs)._

The **Trigger** attribute accepts two values: **TriggerType**, and **ResultType**.

| Property        | Description   
| ------------- | ------------- | 
| TriggerType | This will either be Push or Poll, Push meaning we're actively calling back to the Logic App, Poll meaning the Logic App keeps bugging us and asking if we have data |
| ResultType | Type of object that we will be returning when we have data, this determines the schema used in the metadata |

### Polling Trigger Checklist
1. Add using directives for the **Microsoft.Azure.AppService.ApiApps.Service**, and **TRex.Metadata** namespaces.
  * Microsoft.Azure.AppService.ApiApps.Service provides the **EventWaitPoll** and **EventTriggered** extension methods
  * TRex.Metadata provides the T-Rex **Metadata** attribute, and the **Trigger** attribute
2. Create an action that returns an **HttpResponseMessage**
3. Decorate the action with the **HttpGet** attribute
4. Decorate the action with the **Metadata** attribute and provide a friendly name, and description, for your polling action 
5. Decorate the action with the **Trigger** attribute passing the argument **TriggerType.Poll** to the constructor, as well as the type of model that will be sent when data is available (e.g., **typeof(MyModelClassHere)**)
6. Make sure the action has a **string** parameter named **triggerState**
  * This is a value that you can populate and pass back whenever polling data is returned to the Logic App, and the Logic App will send it back to you on the next poll (e.g., to let you know that it is finished with the last item sent)
  * You do not need to decorate this parameter with any attributes. T-Rex looks for this property by name and automatically applies the correct metadata (friendly name, description, visibility, and default value)
7. _Optionally_, add any other parameters that controls how it should poll (e.g., file name mask, warning temperature, target heart rate, etc...)
  * Decorate these parameters with the **Metadata** attribute to control their friendly names, descriptions, and visibility settings
8. Make sure that the action returns the value generated by calling **Request.EventWaitPoll** when no data is available
  * You can also provide a hint to the Logic App as to a proper polling interval for the next request (if you anticipate data available at a certain time)
  * You can also provide a triggerState value that you want the Logic App to send to you on the next poll
9. Make sure that the action returns the value generated by calling **Request.EventTriggered** when data is available
  * The first argument should be the data to be returned to the Logic App, followed by the new **triggerState** value that you want to receive on the next poll, and optionally a new polling interval for the next request (if you anticipate data available at a certain time, or more likely that you know more data is immediately available and there isn't a need to wait).

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
        [Trigger(TriggerType.Push, typeof(SamplePushEvent))]
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
_**NOTE:** This is an extract from the the companion sample app. You can find this file [here](https://github.com/nihaue/TRex/blob/master/Source/QuickLearn.ApiApps.SampleApiApp/Controllers/PushTriggerSampleController.cs)._

Wow! That was a lot of code, right? Well let's break it down in checklist form so that we can separate what parts of this magic are actually required, and what is simply there for the purposes of the sample.

### Push Trigger Checklist
1. Add using directives for the **Microsoft.Azure.AppService.ApiApps.Service**, **TRex.Metadata**, **TRex.Extensions** namespaces.
  * Microsoft.Azure.AppService.ApiApps.Service provides the **PushTriggerRegistered** extension method
  * TRex.Metadata provides the T-Rex **Metadata** attribute, the **Trigger** attribute, and the **UnregisterCallback** attribute
  * TRex.Extensions provides the **InvokeAsyncWithBody** extension method for the **ClientTriggerCallback<T>**
2. Create an action that returns an **HttpResponseMessage**
3. Decorate the action with the **HttpPut** attribute
4. Decorate the action with the **Metadata** attribute and provide a friendly name, and description, for your trigger
  * This should not be "Register Callback" even though that's what _this specific_ action represents. It should be a meaningful way to describe _WHAT_ is triggering the Logic App like _Target Temp Reached_, _Files Available_, _Customer Entered Store_, etc...
5. Decorate the action with the **Trigger** attribute passing the argument **TriggerType.Push** to the constructor
  * This first action is going to be the method the Logic App uses to register the fact that it wants to be notified whenever data is available.
6. Make sure the action has a **string** parameter named **triggerId**
  * You do not need to decorate this parameter with any attributes. T-Rex looks for this property by name and automatically applies the correct metadata (friendly name, description, visibility, and default value -- currently the name of the Logic App itself)
7. Make sure the action has a parameter named **parameters** of type **TriggerInput<TInput,TOutput>** or **TriggerInput<TOutput>**
  * The type used for the **TInput** type parameter should be whatever inputs may be required to control exactly what ought to fire the trigger (e.g., file name mask, warning temperature, target heart rate, etc...). If we don
  * The type used for the **TOutput** type parameter should be whatever the trigger sends to the Logic App's callback uri when it has data available. While it may appear at first glance like you are somehow receiving this data from the Logic App, the reality is that you're _not_ going to be getting anything of this type sent _in_ from the Logic App. The Logic App _does_ want to know what the shape of that output is though, so referencing it here makes sure that it gets the appropriate metadata is generated.
8. Decorate the **parameters** parameter with the **FromBody** attribute, since it will be contained in the body of the callback registration message.
9. Make sure that somewhere within the code for the action you _store_ the value generated by calling **parameters.GetCallback().CallbackUri**
  * This value can be stored anywhere that you can reliably retrieve it later
  * This value should be correlated with the **triggerId** when stored (so that the callback can be unregistered if the Logic App no longer wants to hear from your trigger)
  * The input model instance stored in **parameters.inputs** should also be stored or dealt with (to configure what actually ought to trigger that specific Logic App). These are the values that the user will be able to configure for this action in the designer.
10. Make sure that when data is available for any waiting clients, you new-up an instance of **ClientTriggerCallback<TOutput>** by passing it the **CallbackUri** stored earlier, and then invoke the **InvokeAsyncWithBody** extension method on that instance
  * The design isn't that you are invoking this within the callback registration action, but instead that this is invoked from within some other running code that is listening for whatever event ought to cause the trigger to fire
  * The first parameter can be simply **Runtime.FromAppSettings()** if executed from within the code of an API App -- given that the App Settings on the host already contain the otherwise required microserviceId and gatewayKey)
  * The last parameter should be the data that you actually want to send to the Logic App
11. Create another action that returns an **HttpResponseMessage**, and has the same route as the first
12. Decorate the second action with the **HttpDelete** attribute
13. Decorate the second action with the **UnregisterCallback** attribute
14. Make sure the second action has a **string** parameter named **triggerId**
  * You do not need to decorate this parameter with any attributes. T-Rex looks for this property by name and automatically applies the correct metadata (friendly name, description, visibility, and default value -- currently the name of the Logic App itself)
15. Make sure that within the code for the action you _delete_ the callback previously registered for the Logic App with the trigger id of **triggerId** from wherever it was stored.

# Go Build Great Things!
Well, what are you waiting for? Reading documentation never built software. Go make mistakes, let those mistakes lead you into building great things!

# Do I Have To Use This Library?
What if you don't want to use this library, and want to do it by hand instead? Well, you certainly can! There's decent write-ups and examples [here](http://azure.microsoft.com/en-us/documentation/articles/app-service-api-dotnet-triggers/) and [here](https://code.msdn.microsoft.com/vstudio/Connector-API-App-Sample-66013c3b#content). In fact, these write-ups are what informed a lot of my work here. So you do have a choice, and you can do what makes you the most happy :-)
