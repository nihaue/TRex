using System.Collections.Generic;
using System.Web.Http;
using Swashbuckle.Application;
using WebActivatorEx;
using QuickLearn.SampleApi;
using TRex.Metadata;
using TRex.Metadata.Models;
using Newtonsoft.Json.Linq;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace QuickLearn.SampleApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        var capability = new FilePickerCapabilityModel 
                            (
                            new FilePickerOperationModel ("GetRootFolders", null),
                            new FilePickerOperationModel ("GetChildFolders", new Dictionary<string, string> { { "folderName", "Name" } }),
                            "Name",
                            "Name",
                            null);

                        c.SingleApiVersion("v1", "QuickLearn.SampleApi");
                        c.ReleaseTheTRex(capability);
                     }
                )
                .EnableSwaggerUi(c => { });
        }
    }

    /// <summary>
    /// If you would prefer to control the Swagger Operation ID
    /// values globally, uncomment this class, as well as the 
    /// call above that wires this Operation Filter into 
    /// the pipeline.
    /// </summary>
    /*
    internal class IncludeParameterNamesInOperationIdFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters != null)
            {
                // Select the capitalized parameter names
                var parameters = operation.parameters.Select(
                    p => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(p.name));

                // Set the operation id to match the format "OperationByParam1AndParam2"
                operation.operationId = $"{operation.operationId}By{string.Join("And", parameters)}";
            }
        }
    }
    */
}