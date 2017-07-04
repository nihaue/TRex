using QuickLearn.ApiApps.Metadata;
using Swashbuckle.Application;
using System;
using TRex.Metadata.Models;

namespace TRex.Metadata
{
    public static class SwaggerDocsConfigExtensions
    {
        /// <summary>
        /// Makes your API App more easily consumable in the Logic App designer.
        /// Adds a schema filter and operation filter to apply changes declared
        /// using the T-Rex MetadataAttribute, TriggerAttribute, and
        /// the UnregisterCallbackAttribute.
        /// </summary>
        /// <param name="config">SwaggerDocsConfig instance that will be used
        /// to configure Swashbuckle</param>
        [CLSCompliant(false)]
        public static void ReleaseTheTRex(this SwaggerDocsConfig config)
            {
            if (config == null) return;

            config.SchemaFilter<TRexSchemaFilter>();
            config.OperationFilter<TRexOperationFilter>();
            config.DocumentFilter<TRexDocumentFilter>();
            }

        /// <summary>
        /// Makes your API App more easily consumable in the Logic App designer.
        /// Adds a schema filter and operation filter to apply changes declared
        /// using the T-Rex MetadataAttribute, TriggerAttribute, and
        /// the UnregisterCallbackAttribute.
        /// </summary>
        /// <param name="config">SwaggerDocsConfig instance that will be used
        /// to configure Swashbuckle</param>
        /// <param name="capability">Capability which will be serialized in SwaggerDocs</param>
        [CLSCompliant (false)]
        public static void ReleaseTheTRexCapabilities (this SwaggerDocsConfig config, FilePickerCapabilityModel capability)
            {
            if (config == null) return;

            config.DocumentFilter (() => new TRexDocumentFilter (capability));
            }
    }
}
