
# T-Rex Metadata Library
<img src="https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/PackageIcon.png" align="right" />QuickLearn's T-Rex Metadata Library enables you to quickly write Web API applications that are readily consumable from the Logic App Designer. It is implemented as a set of .NET Attributes that you can use to decorate methods, parameters, and properties, and a set of filters for [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) that use those attributes to override the [swagger](http://swagger.io/) metadata generation.

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
	    c.ReleaseTheTRex(); /* <-- This line does all of the magic */
	}).EnableSwaggerUi();
```

# Building an Action or Connector API App
If you are building an Action or Connector API App, T-Rex helps you make sure your actions look pretty within the Logic Apps designer, and the generated swagger metadata. It does this through the **Metadata** attribute, which can be used to provide custom friendly names, descriptions, and visibility settings for each of your API App methods, parameters, or properties of the models used by your parameters.

To use this beast, you'll need to add a pesky **using** directive (CTRL+. is your friend if you just start typing the attribute without thinking about it):
```csharp
using TRex.Metadata;
```

Now that we have that out of the way, let's start with a typical Web API action method:

```csharp
        [HttpPost, Route]
        public SampleOutputMessage Post([FromBody]SampleInputMessage sampleInput)
        {
            return new SampleOutputMessage();
        }
```

How do we make it look pretty in the Logic App designer? We simply add the T-Rex **Metadata** attribute. Here it is in action, providing a friendly name and description for the action itself, and its parameter:

```csharp
        [Metadata("Create Message", "Creates a new message absolutely nowhere")]
        [HttpPost, Route]
        public SampleOutputMessage Post([FromBody]
                                        [Metadata("Sample Input", "A sample input message")]
                                            SampleInputMessage sampleInput)
        {
            return new SampleOutputMessage();
        }
```
_**NOTE:** This is an extract from the the companion sample app. You can find this file [here](https://github.com/nihaue/TRex/blob/master/Source/QuickLearn.ApiApps.SampleApiApp/Controllers/ActionSampleController.cs)._

You're not limited to using the **Metadata** attribute on actions or parameters though, you can bring it to your models as well:

```csharp
    public class SampleInputMessage
    {

        [Metadata("String Property", "A happy string input value")]
        public string StringProperty { get; set; }

        [Metadata(Visibility = VisibilityType.Advanced, FriendlyName = "Advanced String Property")]
        public string AdvancedStringProperty { get; set; }
       
    }
```
_**NOTE:** This is an extract from the the companion sample app. You can find this file [here](https://github.com/nihaue/TRex/blob/master/Source/QuickLearn.ApiApps.SampleApiApp/Models/ActionSample/SampleInputMessage.cs)._

Here's what that would look like in a Logic App (it's the one on the far right):
![Create Message action within a Logic App](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/CreateMessageAction.png "Create Message action within a Logic App")

The **Metadata** attribute accepts three values: **FriendlyName**, **Description**, and **Visibility**.

| Property        | Description   
| ------------- | ------------- | 
| FriendlyName | This is the name that will be used for the item in the Logic App designer. In some cases this will be adding an x-ms-summary object in the generated swagger metadata |
| Description | This text describes the item within the generated swagger metadata |
| Visibility | Default - The item shows by default in the Logic App designer, Advanced - The item shows whenever the user clicks the ellipses (...) button to see more, Internal - The item only appears in code view |

It's pretty straight-forward stuff, eh? So how does the Logic App know what to show? It's ultimately reading the swagger metadata for the API. You can see it yourself in a nice visual form by going to **/swagger/ui/index** on the sample application:

![Create Message action shown in Swagger UI](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/CreateMessageSwagger1.png "Create Message action shown in Swagger UI")

Clicking on the action reveals a form that you can use to test the action.

![Create Message action shown in Swagger UI](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/CreateMessageSwagger2.png "Create Message action shown in Swagger UI")

So where is all of the metadata that we clearly added to the model, and that showed up in the Logic App designer? Well, it's mostly stored in the form of _vendor extensions_ to the swagger metadata. If you were to look at the raw metadata at **/swagger/docs/v1** in this case, it would look something like this:

![Metadata Generated by T-Rex](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/GeneratedByTRex.png "Metadata Generated by T-Rex")

Before going through the rest of this, I recommend that you spend some time playing with the sample by deploying it to Azure. You can do that using the button below, but do note that it will deploy as a Web App for more easy experimentation and not an API App (locked away behind the gateway by default):
[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

Well that's pretty cool, but what else can T-Rex do for me?

# Building a Polling Trigger API App

Let's say that you want to trigger a Logic App by having it poll one of your actions until it hits a magical value. That's a little bit more complicated, but T-Rex is willing to help with the **Trigger** attribute.

```csharp
using Microsoft.Azure.AppService.ApiApps.Service;
using TRex.Metadata;

/* lots of unrelated code here */

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
```
_**NOTE:** This is an extract from the the companion sample app. You can find this file [here](https://github.com/nihaue/TRex/blob/master/Source/QuickLearn.ApiApps.SampleApiApp/Controllers/PollingTriggerSampleController.cs)._

The **Trigger** attribute can be used to describe the type of Trigger and the type of Result that will be sent to the Logic App when it fires. It can also be in combination with the **Metadata** attribute to fully describe what is triggering the Logic App. If there are any other responses that could be generated (e.g., in the case of error), you can use the **SwaggerResponse** attribute that Swashbuckle provides.

The **Trigger** attribute accepts two values: **TriggerType**, and **ResultType**.

| Property        | Description   
| ------------- | ------------- | 
| TriggerType | This will either be Push or Poll, Push meaning we're actively calling back to the Logic App, Poll meaning the Logic App keeps bugging us and asking if we have data |
| ResultType | Type of object that we will be returning when we have data, this determines the schema used in the metadata |

So what does that look like in the Logic App designer? Well, hopefully by this point, what you would expect:

![Roll the Dice Trigger in Logic App](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/RollTheDiceTrigger.png "Roll the Dice Trigger in Logic App")

What is T-Rex bringing to the table in terms of the raw Metadata here? Well, you may or may not have noticed a random **triggerState** property being thrown around. That is a special property for polling triggers, and T-Rex knows exactly what to do with it. Additionally the operation itself is special - inasmuch as it represents a method that should be polled. T-Rex knows what to do with that too:

![Polling Trigger Metadata](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/PollingTriggerMetadata.png "Polling Trigger Metadata")

### Polling Trigger Checklist
1. Make sure you have using directives for the **Microsoft.Azure.AppService.ApiApps.Service**, and **TRex.Metadata** namespaces.
  * Microsoft.Azure.AppService.ApiApps.Service provides the **EventWaitPoll** and **EventTriggered** extension methods
  * TRex.Metadata provides the T-Rex **Metadata** attribute, and the **Trigger** attribute
2. Make sure your action returns an **HttpResponseMessage**
3. Make sure your polling action is decorated with the **HttpGet** attribute
4. Make sure your polling action is decorated with the **Metadata** attribute and provide a friendly name, and description, for your polling action 
5. Make sure your polling action is decorated with the **Trigger** attribute passing the argument **TriggerType.Poll** to the constructor, as well as the type of model that will be sent when data is available (e.g., **typeof(MyModelClassHere)**)
6. Make sure the action has a **string** parameter named **triggerState**
  * This is a value that you can populate and pass back whenever polling data is returned to the Logic App, and the Logic App will send it back to you on the next poll (e.g., to let you know that it is finished with the last item sent)
  * You do not need to decorate this parameter with any attributes. T-Rex looks for this property by name and automatically applies the correct metadata (friendly name, description, visibility, and default value)
7. _You can optionally include_, any other parameters that control how it should poll (e.g., file name mask, warning temperature, target heart rate, etc...)
  * Decorate these parameters with the **Metadata** attribute to control their friendly names, descriptions, and visibility settings
8. Make sure that the action returns the value generated by calling **Request.EventWaitPoll** when no data is available
  * You can also provide a hint to the Logic App as to a proper polling interval for the next request (if you anticipate data available at a certain time)
  * You can also provide a triggerState value that you want the Logic App to send to you on the next poll
9. Make sure that the action returns the value generated by calling **Request.EventTriggered** when data is available
  * The first argument should be the data to be returned to the Logic App, followed by the new **triggerState** value that you want to receive on the next poll, and optionally a new polling interval for the next request (if you anticipate data available at a certain time, or more likely that you know more data is immediately available and there isn't a need to wait).

# Building a Push Trigger API App

So you want to call the Logic App on your own terms, eh? Well here's the deal, it will get in touch with you first. It will give you its name, and number (callback url in reality), and you can call it anytime you have information for it. How? Well T-Rex already gave you a **Trigger** attribute, and you already know it accepts a **Push** trigger type. Unfortunately, we're going to need a few more tools in our toolbox to make it all happen. Why? Because sometimes the Logic App doesn't want to be bothered by your trigger anymore. It wants to unregister for callbacks. It is for that purpose that T-Rex gives you an **UnregisterCallbackAttribute**.

Let's see this all in action (also note: we're sending the callbacks with an extension method that T-Rex provides to make sure that the Logic App can successfully evaluate expressions against your result):

```csharp
using Microsoft.Azure.AppService.ApiApps.Service;
using TRex.Metadata;
using TRex.Extensions; // <-- For our custom callback logic

/* lots of unrelated code here */

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

```
_**NOTE:** This is an extract from the the companion sample app. You can find this file [here](https://github.com/nihaue/TRex/blob/master/Source/QuickLearn.ApiApps.SampleApiApp/Controllers/PushTriggerSampleController.cs)._

What does that end up looking like in the Logic App UI?

![Receive Simulated Push in use within Logic App](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/ReceiveSimulatedPush.png "Receive Simulated Push in use within Logic App")

But that was a lot of code, right? Well let's break it down in checklist form so that we can separate what parts of this magic are actually required, and what is simply there for the purposes of the sample.

### Push Trigger Checklist
1. Make sure you have using directives for the **Microsoft.Azure.AppService.ApiApps.Service**, **TRex.Metadata**, **TRex.Extensions** namespaces.
  * Microsoft.Azure.AppService.ApiApps.Service provides the **PushTriggerRegistered** extension method
  * TRex.Metadata provides the T-Rex **Metadata** attribute, the **Trigger** attribute, and the **UnregisterCallback** attribute
  * TRex.Extensions provides the **InvokeAsyncWithBody** extension method for the **ClientTriggerCallback<T>**
2. Make sure your callback registration action returns an **HttpResponseMessage**
3. Make sure that action is decorated with the **HttpPut** attribute
4. Make sure that action is decorated with the **Metadata** attribute and provide a friendly name, and description, for your trigger
  * This should not be "Register Callback" even though that's what _this specific_ action represents. It should be a meaningful way to describe _WHAT_ is triggering the Logic App like _Target Temp Reached_, _Files Available_, _Customer Entered Store_, etc...
5. Make sure that action is decorated with the **Trigger** attribute passing the argument **TriggerType.Push** to the constructor
  * This first action is going to be the method the Logic App uses to register the fact that it wants to be notified whenever data is available.
6. Make sure the action has a **string** parameter named **triggerId**
  * You do not need to decorate this parameter with any attributes. T-Rex looks for this property by name and automatically applies the correct metadata (friendly name, description, visibility, and default value -- currently the name of the Logic App itself)
7. Make sure the action has a parameter named **parameters** of type **TriggerInput<TInput,TOutput>** or **TriggerInput<TOutput>**
  * The type used for the **TInput** type parameter should be whatever inputs may be required to control exactly what ought to fire the trigger (e.g., file name mask, warning temperature, target heart rate, etc...). If we don
  * The type used for the **TOutput** type parameter should be whatever the trigger sends to the Logic App's callback uri when it has data available. While it may appear at first glance like you are somehow receiving this data from the Logic App, the reality is that you're _not_ going to be getting anything of this type sent _in_ from the Logic App. The Logic App _does_ want to know what the shape of that output is though, so referencing it here makes sure that it gets the appropriate metadata is generated.
8. Make sure that the **parameters** parameter is decorated with the **FromBody** attribute, since it will be contained in the body of the callback registration message.
9. Make sure that somewhere within the code for the action you _store_ the value generated by calling **parameters.GetCallback().CallbackUri**
  * This value can be stored anywhere that you can reliably retrieve it later
  * This value should be correlated with the **triggerId** when stored (so that the callback can be unregistered if the Logic App no longer wants to hear from your trigger)
  * The input model instance stored in **parameters.inputs** should also be stored or dealt with (to configure what actually ought to trigger that specific Logic App). These are the values that the user will be able to configure for this action in the designer.
10. Make sure that when data is available for any waiting clients, you new-up an instance of **ClientTriggerCallback<TOutput>** by passing it the **CallbackUri** stored earlier, and then invoke the **InvokeAsyncWithBody** extension method on that instance
  * The design isn't that you are invoking this within the callback registration action, but instead that this is invoked from within some other running code that is listening for whatever event ought to cause the trigger to fire
  * The first parameter can be simply **Runtime.FromAppSettings()** if executed from within the code of an API App -- given that the App Settings on the host already contain the otherwise required microserviceId and gatewayKey)
  * The last parameter should be the data that you actually want to send to the Logic App
11. Make sure you have an unregister callback action as well that returns an **HttpResponseMessage**, and has the same route as the first
12. Make sure that action is decorated with the **HttpDelete** attribute
13. Make sure that action is decorated with the **UnregisterCallback** attribute
14. Make sure that action has a **string** parameter named **triggerId**
  * You do not need to decorate this parameter with any attributes. T-Rex looks for this property by name and automatically applies the correct metadata (friendly name, description, visibility, and default value -- currently the name of the Logic App itself)
15. Make sure that within the code for the action you _delete_ the callback previously registered for the Logic App with the trigger id of **triggerId** from wherever it was stored.
  
# Go Build Great Things!
Well, what are you waiting for? Reading documentation never built software. Go make mistakes, let those mistakes lead you into building great things!

# Do I Have To Use This Library?
What if you don't want to use this library, and want to do it by hand instead? Well, you certainly can! There's decent write-ups and examples [here](http://azure.microsoft.com/en-us/documentation/articles/app-service-api-dotnet-triggers/) and [here](https://code.msdn.microsoft.com/vstudio/Connector-API-App-Sample-66013c3b#content). In fact, these write-ups are what informed a lot of my work here. So you do have a choice, and you can do what makes you the most happy :-)