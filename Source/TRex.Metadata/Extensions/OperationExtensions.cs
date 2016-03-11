using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using TRex.Metadata;
using TRex.Metadata.Models;

namespace QuickLearn.ApiApps.Metadata.Extensions
{
    internal static class OperationExtensions
    {

        public static void SetResponseTypeLookup(this Operation operation, HttpStatusCode statusCode, DynamicSchemaModel dynamicSchemaSettings)
        {

            // Currently this adds a extension property marker on the operation that
            // is copied down to the response by a document filter. If issue #678 is
            // resolved in the Swashbuckle library, this can be re-written to simply
            // apply the extension directly on the response instead

            if (dynamicSchemaSettings == null) throw new ArgumentNullException(nameof(dynamicSchemaSettings));

            var statusCodeString = ((int)statusCode).ToString(CultureInfo.InvariantCulture);

            var vendorExtensionKey = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", Constants.X_MS_DYNAMIC_SCHEMA, statusCodeString);

            operation.EnsureVendorExtensions();

            if (!operation.vendorExtensions.ContainsKey(vendorExtensionKey))
            {
                operation.vendorExtensions.Add(vendorExtensionKey, dynamicSchemaSettings);
            }
        }

        public static void SetTrigger(this Operation operation, BatchMode batchMode)
        {
            operation.EnsureVendorExtensions();

            if (!operation.vendorExtensions.ContainsKey(Constants.X_MS_TRIGGER))
            {
                operation.vendorExtensions.Add(Constants.X_MS_TRIGGER,
                    batchMode.ToString().ToLowerInvariant());
            }
        }

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
