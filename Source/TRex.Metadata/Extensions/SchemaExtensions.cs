using Swashbuckle.Swagger;
using System.Collections.Generic;
using TRex.Metadata;
using TRex.Metadata.Models;

namespace QuickLearn.ApiApps.Metadata.Extensions
{
    internal static class SchemaExtensions
    {
        public static void EnsureVendorExtensions(this Schema modelDescription)
        {
            if (modelDescription.vendorExtensions == null) modelDescription.vendorExtensions = new Dictionary<string, object>();
        }

        public static void SetSchemaLookup(this Schema modelDescription, DynamicSchemaModel dynamicSchemaSettings)
        {
            modelDescription.EnsureVendorExtensions();

            if (!modelDescription.vendorExtensions.ContainsKey(Constants.X_MS_DYNAMIC_SCHEMA))
            {
                modelDescription.vendorExtensions.Add(Constants.X_MS_DYNAMIC_SCHEMA,
                    dynamicSchemaSettings);
            }
        }

        public static void SetCallbackUrl(this Schema modelDescription)
        {
            modelDescription.EnsureVendorExtensions();

            if (!modelDescription.vendorExtensions.ContainsKey(Constants.X_MS_NOTIFICATION_URL))
            {
                modelDescription.vendorExtensions.Add(Constants.X_MS_NOTIFICATION_URL,
                    true.ToString().ToLowerInvariant());
            }

            modelDescription.SetVisibility(VisibilityType.Internal);
        }

        public static void SetVisibility(this Schema modelDescription, VisibilityType visibility)
        {
            if (visibility == VisibilityType.Default) return;

            modelDescription.EnsureVendorExtensions();

            if (!modelDescription.vendorExtensions.ContainsKey(Constants.X_MS_VISIBILITY))
            {
                modelDescription.vendorExtensions
                    .Add(Constants.X_MS_VISIBILITY,
                            visibility.ToString().ToLowerInvariant());
            }
        }

        public static void SetFriendlyNameAndDescription(this Schema modelDescription, string friendlyName, string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
                modelDescription.description = description;

            if (string.IsNullOrWhiteSpace(friendlyName)) return;

            modelDescription.EnsureVendorExtensions();

            if (!modelDescription.vendorExtensions.ContainsKey(Constants.X_MS_SUMMARY))
            {
                modelDescription.vendorExtensions.Add(Constants.X_MS_SUMMARY, friendlyName);
            }
        }

    }
}
