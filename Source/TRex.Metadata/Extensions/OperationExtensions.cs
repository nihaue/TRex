using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;
using TRex.Metadata;
using TRex.Metadata.Models;

namespace QuickLearn.ApiApps.Metadata.Extensions
{
    internal static class OperationExtensions
    {

        public static void SetCallbackType(this Operation operation, SchemaRegistry schemaRegistry, Type callbackType, string description)
        {
            operation.EnsureVendorExtensions();

            if (!operation.vendorExtensions.ContainsKey(Constants.X_MS_NOTIFICATION_CONTENT))
            {
                var schemaInfo = schemaRegistry.GetOrRegister(callbackType);
                
                var notificationData = new NotificationContentModel();
                notificationData.Description = description;
                notificationData.Schema = new SchemaModel(schemaInfo);

                operation.vendorExtensions.Add(Constants.X_MS_NOTIFICATION_CONTENT,
                    notificationData);
            }

        }

        public static void EnsureVendorExtensions(this Operation operation)
        {
            if (operation.vendorExtensions == null) operation.vendorExtensions = new Dictionary<string, object>();
        }

        public static void SetVisibility(this Operation operation, VisibilityType visibility)
        {
            if (visibility == VisibilityType.Default) return;

            operation.EnsureVendorExtensions();
            
            if (!operation.vendorExtensions.ContainsKey(Constants.X_MS_VISIBILITY))
                operation.vendorExtensions.Add(Constants.X_MS_VISIBILITY,
                    visibility.ToString().ToLowerInvariant());
        }

        public static void SetFriendlyNameAndDescription(this Operation operation, string friendlyName, string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
                operation.description = description;

            if (!string.IsNullOrWhiteSpace(friendlyName))
            {
                operation.summary = friendlyName;
                operation.operationId = GetOperationId(friendlyName);
            }
        }

        public static string GetOperationId(string friendlyName)
        {
           return CultureInfo.CurrentCulture.TextInfo
                                            .ToTitleCase(friendlyName)
                                            .Replace(" ", string.Empty);
        }
    }
}
