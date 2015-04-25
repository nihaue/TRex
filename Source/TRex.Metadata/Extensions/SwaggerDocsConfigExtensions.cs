using QuickLearn.ApiApps.Metadata;
using Swashbuckle.Application;

namespace TRex.Metadata
{
    public static class SwaggerDocsConfigExtensions
    {
        public static void ReleaseTheTRex(this SwaggerDocsConfig config)
        {
            if (config == null) return;

            config.SchemaFilter<TRexSchemaFilter>();
            config.OperationFilter<TRexOperationFilter>();
        }
    }
}
