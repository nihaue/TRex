using Swashbuckle.Swagger;
using System.Collections.Generic;
using TRex.Metadata;

namespace QuickLearn.ApiApps.Metadata.Extensions
{
    internal static class OperationExtensions
    {
        public static void EnsureVendorExtensions(this Operation operation)
        {
            if (operation.vendorExtensions == null) operation.vendorExtensions = new Dictionary<string, object>();
        }

        public static void SetVisibility(this Operation operation, VisibilityTypes visibility)
        {
            if (visibility == VisibilityTypes.Default) return;

            operation.EnsureVendorExtensions();
            
            if (!operation.vendorExtensions.ContainsKey(Constants.X_MS_VISIBILITY))
                operation.vendorExtensions.Add(Constants.X_MS_VISIBILITY, visibility.ToString().ToLower());
        }

        public static void SetFriendlyNameAndDescription(this Operation operation, string friendlyName, string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
                operation.description = description;

            if (!string.IsNullOrWhiteSpace(friendlyName))
                operation.summary = friendlyName;
        }

    }
}
