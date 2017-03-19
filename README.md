
# T-Rex Metadata Library
<img src="https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/PackageIcon.png" align="right" />
QuickLearn's T-Rex Metadata Library enables you to quickly write Web API applications that are
readily consumable from the Logic App Designer. It is implemented as a set of .NET Attributes
that you can use to decorate methods, parameters, models and properties, and a set of filters for
[Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) that use those attributes to
override the [swagger](http://swagger.io/) metadata generation.

So let's go ahead and get started!

# Getting Started
To get started, you will need to [install the **TRex** NuGet package](https://www.nuget.org/packages/TRex/).
From there, follow the instructions in the **Enabling T-Rex Metadata Generation** section, and
then whichever other sections are applicable below. If you want to get straight to some working
code, you can also [look through the sample application](https://github.com/nihaue/TRex/tree/powerapps/Source/QuickLearn.SampleApi)
which implements an API that demonstrates each of the core features of the T-Rex library.

# Enabling T-Rex Metadata Generation
To enable T-Rex Metadata Generation, head over to the **SwaggerConfig.cs** file in the
**App_Start** folder. Add the requisite using directive:
```csharp
using TRex.Metadata;
```

Then within the configure action passed to the **EnableSwagger** method, add the following line:
```csharp
GlobalConfiguration.Configuration.EnableSwagger(c =>
	{
	    c.SingleApiVersion("v1", "QuickLearn Sample API App");
	    c.ReleaseTheTRex(); // <-- This line does all of the magic
	}).EnableSwaggerUi();
```

# Building an Action or Connector API App
If you are building an Action/Connector API App, T-Rex helps you make sure your actions
look pretty within the Logic Apps designer, and within the generated swagger metadata. It does
this through a set of custom .NET attributes.

The attribute that you will use most often is the **Metadata** attribute, which can be used to
provide custom friendly names, descriptions, and visibility settings for each of your API App
methods, parameters, or properties of the models used by your parameters.

To use this beast, you'll need to add a pesky **using** directive (CTRL+. is your friend if
you just start typing the attribute without thinking about it):
```csharp
using TRex.Metadata;
```

Now that we have that out of the way, let's start with a typical Web API action method:

```csharp
        [HttpPost, Route]
		[SwaggerOperation("PostInput")]
		[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SampleOutputMessage))]
        public async Task<IHttpActionResult> Post([FromBody]SampleInputMessage sampleInput)
        {
            return await SampleOutputMessage.FromInputAsync(sampleInput);
        }
```

How do we make it look pretty in the Logic App designer? We simply replace the **SwaggerOperation** attribute
with the T-Rex **Metadata** attribute.

Here it is in action, providing a friendly name and description for the action itself, and its parameter:

```csharp
        [HttpPost, Route]
		[Metadata("Create Message", "Creates a new message absolutely nowhere")]
		[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SampleOutputMessage))]
        public async Task<IHttpActionResult> Post([FromBody]
                                        [Metadata("Sample Input", "A sample input message")]
                                            SampleInputMessage sampleInput)
        {
            return await SampleOutputMessage.FromInputAsync(sampleInput);
        }
```

Where did the **SwaggerOperation** attribute go? Well, the **Metadata** attribute goes a step further
than the **SwaggerOperation** attriubte. The **SwaggerOperation** attribute indicates what the id of
the operation should be in the swagger metadata. The **Metadata** attribute indicates a friendly name
from which an operation id is generated. If you type a friendly name that does not include spaces, it will
be used directly as the operation id in the swagger metadata.

You're not limited to using the **Metadata** attribute on just *actions* or *parameters* though,
you can bring it to your *models* as well:

```csharp
    public class SampleInputMessage
    {

        [Metadata("String Property", "A happy string input value")]
        public string StringProperty { get; set; }

        [Metadata(Visibility = VisibilityType.Advanced, FriendlyName = "Advanced String Property")]
        public string AdvancedStringProperty { get; set; }
       
    }
```

The **Metadata** attribute accepts three values: **FriendlyName**, **Description**, and **Visibility**.

| Property        | Description   
| ------------- | ------------- | 
| FriendlyName | This is the name that will be used for the item in the Logic App designer. In some cases this will be adding an x-ms-summary object in the generated swagger metadata |
| Description | This text describes the item within the generated swagger metadata |
| Visibility | Default - The item shows by default in the Logic App designer, Advanced - The item shows whenever the user clicks the ellipses (...) button to see more, Internal - The item only appears in code view, Important - The item is especially highlighted in the designer |

It's pretty straight-forward stuff, eh? So how does the Logic App know what to show? It's ultimately reading the swagger metadata for the API. 

If you [fire up a new API App](https://azure.microsoft.com/en-us/documentation/articles/app-service-logic-custom-hosted-api/), you can see some of the information contained in the metadata yourself in a nice visual form by going to **/swagger/ui/index**.

In the example shown in the figure below, we find the **Create Message** action with its lovely friendly name being displayed.

![Create Message action shown in Swagger UI](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/CreateMessageSwagger1.png "Create Message action shown in Swagger UI")

Clicking on the action reveals not only the description that was contained in the metadata, but also a form that you can use to test the action.

![Create Message action shown in Swagger UI](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/CreateMessageSwagger2.png "Create Message action shown in Swagger UI")

So where is all of the metadata that we clearly added to the _model_ for this action? It showed up in the Logic App designer, so surely it must live somewhere! Well, it's mostly stored in the form of _vendor extensions_ to the swagger metadata. If you were to look at the raw metadata at **/swagger/docs/v1** in this case, it would look something like this:

![Metadata Generated by T-Rex](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/GeneratedByTRex.png "Metadata Generated by T-Rex")

Well that's pretty cool, but what else can T-Rex do for me?

# New Capabilities for Power Apps / Microsoft Flow 

T-Rex is currently being updated to support [new functionality](https://powerapps.microsoft.com/en-us/tutorials/customapi-how-to-swagger/) within Power Apps and Microsoft Flow. At the moment, these features are **only** functioning in Microsoft Flow (since your custom API is treated as a Managed API and has access to the same dynamic schema/values functionality).

You can find this functionality in version **[2.0.2-alpha](http://www.nuget.org/packages/TRex/2.0.2-alpha)** of the NuGet package.

What will I have to change in my code?
- Triggers as a concept have evolved into a completely different form in the new runtime/designer. Triggers can actually be used at any point of a Logic App -- they're not necessarily only triggering the flow -- the flow can pause and wait for an event and/or poll for data. As a result the **Trigger** attribute no longer serves the same purpose as it once did. You can currently build three types of triggers with T-Rex (1) [Polling with a single returned result](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Controllers/PollingTriggerController.cs), (2) Polling with a returned batch of results that will each trigger their own flow, (3) [Subscription based trigger](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Controllers/PushTriggerController.cs) (requires use of the [CallbackUrl attribute somewhere in the response model](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Models/PushTrigger/PriceAlertConfig.cs)).
- The **UnregisterCallback** attribute has been removed. The runtime simply performs a DELETE on the resource specified in the Location header when the callback is created. [See sample here](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Controllers/PushTriggerController.cs).
- The **ResponseTypeLookup** attribute has been removed. This has been replaced by the **DynamicSchemaLookup** attribute to be specified on a class that dervices from **DynamicModelBase**. [See sample here](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Models/DynamicSchemas/ContactInfo.cs).

What are the new features?
- [**Trigger** attribute](https://github.com/nihaue/TRex/blob/powerapps/Source/TRex.Metadata.Attributes/Attributes/TriggerAttribute.cs) to be used with [polling triggers](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Controllers/PollingTriggerController.cs)/[subscription triggers](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Controllers/PushTriggerController.cs) to identify the batch mode (treat trigger result as a sinlge item vs. treat trigger result as a batch) and pattern
- [**EventTriggered** and **EventWaitPoll** extension methods](https://github.com/nihaue/TRex/blob/powerapps/Source/TRex.Metadata/Extensions/PollingTriggerExtensions.cs) that are compatible with new Logic Apps polling style. [See sample here](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Controllers/PollingTriggerController.cs))
- [**DynamicValueLookup** attribute](https://github.com/nihaue/TRex/blob/powerapps/Source/TRex.Metadata.Attributes/Attributes/DynamicValueLookupAttribute.cs) to be used with a parameter to define an operation to call to lookup possible values for a given parameter (emits x-ms-dynamic-values vendor extension). [See sample here](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Controllers/DynamicValuesController.cs). 
- [**DynamicSchemaLookup** attribute](https://github.com/nihaue/TRex/blob/powerapps/Source/TRex.Metadata.Attributes/Attributes/DynamicSchemaLookupAttribute.cs) to define an operation to call to lookup a JSON schema for a given model (emits x-ms-dynamic-schmea swagger vendor extension). See [sample controller here](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Controllers/DynamicSchemasController.cs), and [dynamic model here](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Models/DynamicSchemas/ContactInfo.cs).
- [**DynamicModelBase** class](https://github.com/nihaue/TRex/blob/powerapps/Source/TRex.Metadata/Models/DynamicModelBase.cs) to use as a base of your dynamic models (models for which you are using the **DynamicSchemaLookup** attribute). It has been designed to be backed by a JToken, and implicitly convertible to/from JToken. It can also be used as a dynamic. Properties added by classes deriving from this class will also be backed by the JToken during serialization/deserialization. [See sample here](https://github.com/nihaue/TRex/blob/powerapps/Source/QuickLearn.SampleApi/Models/DynamicSchemas/ContactInfo.cs).
    
# Go Build Great Things!
Well, what are you waiting for? Reading documentation never built software. Go make mistakes, let those mistakes lead you into building great things!

# Do I Have To Use This Library?
What if you don't want to use this library, and want to do it by hand instead? Well, you certainly can! There's decent write-ups and examples [here](http://azure.microsoft.com/en-us/documentation/articles/app-service-api-dotnet-triggers/) and [here](https://code.msdn.microsoft.com/vstudio/Connector-API-App-Sample-66013c3b#content). In fact, these write-ups are what informed a lot of my work here. So you do have a choice, and you can do what makes you the most happy :-)