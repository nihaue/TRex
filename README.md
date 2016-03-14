
# T-Rex Metadata Library
<img src="https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/PackageIcon.png" align="right" />
QuickLearn's T-Rex Metadata Library enables you to quickly write Web API applications that are
readily consumable from the Logic App Designer. It is implemented as a set of .NET Attributes
that you can use to decorate methods, parameters, and properties, and a set of filters for
[Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) that use those attributes to
override the [swagger](http://swagger.io/) metadata generation.

So let's go ahead and get started!

# Getting Started
To get started, you will need to [install the **TRex** NuGet package](https://www.nuget.org/packages/TRex/).
From there, follow the instructions in the **Enabling T-Rex Metadata Generation** section, and
then whichever other sections are applicable below. If you want to get straight to some working
code, you can also [look through the sample application](https://github.com/nihaue/TRex/tree/master/Source/QuickLearn.Api.Sample) (COMING SOON)
which implements a simple API that demonstrates how one might use the T-Rex attributes with
a custom API.

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
If you are building an Action or Connector API App, T-Rex helps you make sure your actions
look pretty within the Logic Apps designer, and the generated swagger metadata. It does
this through a set of custom attributes. The first attribute that you might want to know
about is the **Metadata** attribute, which can be used to provide custom friendly names,
descriptions, and visibility settings for each of your API App methods, parameters, or
properties of the models used by your parameters.

To use this beast, you'll need to add a pesky **using** directive (CTRL+. is your friend if
you just start typing the attribute without thinking about it):
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


The **Metadata** attribute accepts three values: **FriendlyName**, **Description**, and **Visibility**.

| Property        | Description   
| ------------- | ------------- | 
| FriendlyName | This is the name that will be used for the item in the Logic App designer. In some cases this will be adding an x-ms-summary object in the generated swagger metadata |
| Description | This text describes the item within the generated swagger metadata |
| Visibility | Default - The item shows by default in the Logic App designer, Advanced - The item shows whenever the user clicks the ellipses (...) button to see more, Internal - The item only appears in code view, Important - The item is especially highlighted in the designer |

It's pretty straight-forward stuff, eh? So how does the Logic App know what to show? It's ultimately reading the swagger metadata for the API. 

If you [fire up a new API App](https://azure.microsoft.com/en-us/documentation/articles/app-service-logic-custom-hosted-api/), you can see some of the information contained in the metadata yourself in a nice visual form by going to **/swagger/ui/index**.

In the example shown in the figure belwo, we find the **Create Message** action with its lovely friendly name being displayed.

![Create Message action shown in Swagger UI](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/CreateMessageSwagger1.png "Create Message action shown in Swagger UI")

Clicking on the action reveals not only the description that was contained in the metadata, but also a form that you can use to test the action.

![Create Message action shown in Swagger UI](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/CreateMessageSwagger2.png "Create Message action shown in Swagger UI")

So where is all of the metadata that we clearly added to the _model_ for this action? It showed up in the Logic App designer, so surely it must live somewhere! Well, it's mostly stored in the form of _vendor extensions_ to the swagger metadata. If you were to look at the raw metadata at **/swagger/docs/v1** in this case, it would look something like this:

![Metadata Generated by T-Rex](https://raw.githubusercontent.com/nihaue/TRex/master/Docs/Images/GeneratedByTRex.png "Metadata Generated by T-Rex")

Well that's pretty cool, but what else can T-Rex do for me?

# New Capabilities for Power Apps / Logic Apps Preview Refresh  

T-Rex is currently being updated to support [new functionality](http://www.quicklearn.com/blog/2016/03/09/azure-app-service-logic-apps-refresh/) within the Logic Apps designer and runtime. The current release provides the set of attributes that will be available, but does not necessarily represent the final implementation of those attributes. Meaning, if you write code today and use the attributes, you should not have to modify your code in order to enable the functionality as it becomes available (simply update the NuGet package reference).

You can find this functionality in version **0.3.0-alpha** of the NuGet package. A sample API will be released to further demonstrate usage of these attributes.

What will I have to change in my code?
- Triggers as a concept have evolved into a completely different form in the new runtime/designer. Triggers can actually be used at any point of a Logic App -- they're not necessarily only triggering the flow -- the flow can pause and wait for an event and/or poll for data. As a result the **Trigger** attribute no longer serves the same purpose. A sample and guidance for using each of the triggering models will be made available as it is ready. Currenly only polling triggers can be built with this release of T-Rex.
- The **UnregisterCallback** attribute has been removed. It may return in a later release.

What are the new features?
- **Trigger** attribute to be used with polling triggers/actions to identify the batch mode (treat trigger result as a sinlge item vs. treat trigger result as a batch)
- **DynamicValueLookup** attribute to be used with a parameter to define an operation to call to lookup possible values for a given parameter (emits x-ms-dynamic-values vendor extension)
- **DynamicSchemaLookup** attribute to define an operation to call to lookup a JSON schema for a given parameter (emits x-ms-dynamic-schmea swagger vendor extension)

You can [learn about these new features here](https://azure.microsoft.com/en-us/documentation/articles/powerapps-develop-api/).
    
The following features are not quite ready for use:
- **CallbackType** attribute to be used on callback subscription registration methods to specify the shape of notification that will be sent as a result when the callback url is invoked. The emits the *x-ms-notification-content* swagger vendor extension, however it is currently not emitted in the correct location within the swagger. Instead it is targeting a location within the metadata that the designer **will** be looking in a future release.
- **CallbackUrl** attribute to be used on a member of an input for the callback subscription registration method to specify the member that contains the url at which the Logic App can be notified of new contnet.
- **ResponseTypeLookup** attribute. This has the same functionality as the **DynamicSchemaLookup** attribute, but only applies to return values. Implementation of this functionality is blocked by **Swashbuckle** which does not currently support emitting vendor extensions in the swagger for individual responses. [Pull request #679](https://github.com/domaindrivendev/Swashbuckle/pull/679) should unblock implementation if/when it is merged.


# Go Build Great Things!
Well, what are you waiting for? Reading documentation never built software. Go make mistakes, let those mistakes lead you into building great things!

# Do I Have To Use This Library?
What if you don't want to use this library, and want to do it by hand instead? Well, you certainly can! There's decent write-ups and examples [here](http://azure.microsoft.com/en-us/documentation/articles/app-service-api-dotnet-triggers/) and [here](https://code.msdn.microsoft.com/vstudio/Connector-API-App-Sample-66013c3b#content). In fact, these write-ups are what informed a lot of my work here. So you do have a choice, and you can do what makes you the most happy :-)
