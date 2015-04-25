using QuickLearn.ApiApps.Metadata;
using Swashbuckle.Application;

namespace TRex.Metadata
{
    public static class SwaggerDocsConfigExtensions
    {
        public static void ReleaseTheTRex(this SwaggerDocsConfig c)
        {
            c.SchemaFilter<QuickLearnLogicAppMetadataFilter>();
            c.OperationFilter<QuickLearnLogicAppMetadataFilter>();
        }
    }
}
