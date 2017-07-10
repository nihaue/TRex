using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using QuickLearn.ApiApps.Metadata;
using Swashbuckle.Swagger;
using TRex.Metadata.Models;

namespace TRex.Metadata.Filters
{
    public class TRexCapabilityFilter : IDocumentFilter
    {
        private FilePickerCapabilityModel capability;
        public TRexCapabilityFilter() { }

        public TRexCapabilityFilter(FilePickerCapabilityModel capability)
        {
            this.capability = capability;
        }

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            if (capability == null)
                return;

            //handles file-picker capability (only 1 till microsoft gives more info about capabilities)
            HandleCapabilities(swaggerDoc, apiExplorer, capability);
        }

        private static void HandleCapabilities(SwaggerDocument swaggerDoc, IApiExplorer apiExplorer, FilePickerCapabilityModel capability)
        {
            if (swaggerDoc.vendorExtensions.TryGetValue(Constants.X_MS_CAPABILITIES, out object value))
            {
                if (value is Dictionary<string, object> dictionary)
                    dictionary.Add(Constants.FILE_PICKER, capability);
            }

            swaggerDoc.vendorExtensions
                .Add
                (
                    Constants.X_MS_CAPABILITIES,
                    new Dictionary<string, object> {{Constants.FILE_PICKER, capability}}
                );
        }
    }
}
