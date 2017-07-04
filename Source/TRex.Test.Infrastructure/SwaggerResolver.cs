using System.Collections.Generic;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using TRex.Metadata;
using TRex.Metadata.Models;

namespace TRex.Test.Infrastructure
{
    public static class SwaggerResolver
    {
        internal static HttpConfiguration config = new HttpConfiguration();

        internal static SwaggerDocsConfig GetDefaultConfigWithTRex()
        {
            SwaggerDocsConfig config = new SwaggerDocsConfig();

            config.SingleApiVersion("v1", "TRexTestApi");
            config.ReleaseTheTRex();
            config.OperationFilter<IncludeParameterNamesInOperationIdFilter>();

            return config;
        }

        internal static SwaggerDocsHandler GetDefaultHandler()
        {
            return new SwaggerDocsHandler(GetDefaultConfigWithTRex());
        }

        internal static void SetupRoutesFor(Assembly assembly)
        {
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();
        }
        
        internal static string GetSwagger()
        {

            SetupRoutesFor(typeof(TRex.Test.DummyApi.WebApiApplication).Assembly);

            var request = new HttpRequestMessage(HttpMethod.Get, "http://tempuri.org/swagger/docs/v1");

            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            var route = new HttpRoute("swagger/docs/{apiVersion}");

            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = route.GetRouteData("/", request);

            var messageInvoker = new HttpMessageInvoker(GetDefaultHandler());

            var result = messageInvoker.SendAsync(request, new CancellationToken(false)).Result;

            var responseContent = result.Content.ReadAsStringAsync().Result;

            return responseContent;
        }

        private static string swagger = null;
        public static string Swagger
        {
            get { return swagger ?? (swagger = GetSwagger()); }
        }
    }

    public static class SwaggerResolverCapability
        {
        internal static HttpConfiguration config = new HttpConfiguration();

        internal static SwaggerDocsConfig GetDefaultConfigWithTRex (FilePickerCapabilityModel capability)
            {
            SwaggerDocsConfig config = new SwaggerDocsConfig();

            config.SingleApiVersion("v1", "TRexTestApi");
            if ( capability != null )
                config.ReleaseTheTRex(capability);
            else
                config.ReleaseTheTRex();
            config.OperationFilter<IncludeParameterNamesInOperationIdFilter>();

            return config;
            }

        internal static SwaggerDocsHandler GetDefaultHandler (FilePickerCapabilityModel capability)
            {
            return new SwaggerDocsHandler(GetDefaultConfigWithTRex(capability));
            }

        internal static void SetupRoutesFor (Assembly assembly)
            {
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();
            }

        internal static string GetSwagger (FilePickerCapabilityModel capability = null)
            {

            SetupRoutesFor(typeof(TRex.Test.DummyApi.WebApiApplication).Assembly);

            var request = new HttpRequestMessage(HttpMethod.Get, "http://tempuri.org/swagger/docs/v1");

            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            var route = new HttpRoute("swagger/docs/{apiVersion}");

            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = route.GetRouteData("/", request);

            var messageInvoker = new HttpMessageInvoker(GetDefaultHandler(capability));

            var result = messageInvoker.SendAsync(request, new CancellationToken(false)).Result;

            var responseContent = result.Content.ReadAsStringAsync().Result;

            return responseContent;
            }

        public static string GetSwaggerWithCapability (FilePickerCapabilityModel capability)
            {
            SwaggerResolverCapability.config = new HttpConfiguration();
            return GetSwagger (capability);
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
