using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using WebActivatorEx;
using TRex.TestApiApp;
using TRex.Metadata;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace TRex.TestApiApp
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "TRexTestApi");
                        c.ReleaseTheTRex();
                        c.OperationFilter<IncludeParameterNamesInOperationIdFilter>();
                    });

        }
    }

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
                operation.operationId = string.Format(
                    "{0}By{1}",
                    operation.operationId,
                    string.Join("And", parameters));
            }
        }
    }
}