# T-Rex Metadata Library
The T-Rex Metadata Library enables you to quickly write Web API applications that are readily consumable from the Logic App Designer. It is implemented as a set of .NET Attributes that you can use to decorate methods, parameters, and properties, and a set of filters for [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) that use those attributes to override the [swagger](http://swagger.io/) metadata generation.

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

Let's see the **Metadata** attribute in action:

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

# Building a Polling Trigger API App

# Building a Push Trigger API App

