using TRex.Metadata;
using QuickLearn.ApiApps.SampleApiApp;
using Swashbuckle.Application;
using System.Web.Http;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace QuickLearn.ApiApps.SampleApiApp
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration.EnableSwagger(c =>
                                                {
                                                    c.SingleApiVersion("v1", "QuickLearn Sample API App");
                                                    c.ReleaseTheTRex();
                                                }).EnableSwaggerUi();
        }
    }
}