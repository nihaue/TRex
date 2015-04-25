using Swashbuckle.Swagger;
using System.Collections.Generic;
using TRex.Metadata;

namespace QuickLearn.ApiApps.Metadata.Extensions
{
    internal static class SchemaExtensions
    {
        public static void EnsureVendorExtensions(this Schema modelDescription)
        {
            if (modelDescription.vendorExtensions == null) modelDescription.vendorExtensions = new Dictionary<string, object>();
        }
        

        public static void SetChildPropertyRequired(this Schema model, string requiredChildPropertyName)
        {
            if (model.required == null) model.required = new List<string>();
            if (!model.required.Contains(requiredChildPropertyName)) model.required.Add(requiredChildPropertyName);
        }

        public static void SetSchedulerRecommendation(this Schema modelDescription, string recommendation)
        {
            if (string.IsNullOrWhiteSpace(recommendation)) return;

            modelDescription.EnsureVendorExtensions();

            if (!modelDescription.vendorExtensions.ContainsKey(Constants.X_MS_SCHEDULER_RECOMMENDATION))
            {
                modelDescription.vendorExtensions.Add(Constants.X_MS_SCHEDULER_RECOMMENDATION, recommendation);
            }
        }

        public static void SetVisibility(this Schema modelDescription, VisibilityTypes visibility)
        {
            if (visibility == VisibilityTypes.Default) return;

            modelDescription.EnsureVendorExtensions();

            if (!modelDescription.vendorExtensions.ContainsKey(Constants.X_MS_VISIBILITY))
            {
                modelDescription.vendorExtensions.Add(Constants.X_MS_VISIBILITY, visibility.ToString().ToLower());
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
