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

        public static void SetTrigger(this Operation operation, SchemaRegistry schemaRegistry, TriggerAttribute triggerDescription)
        {
            operation.EnsureVendorExtensions();

            string batchMode = null;

            switch (triggerDescription.Pattern)
            {
                case TriggerType.PollingBatched:
                    batchMode = Constants.BATCH;
                    break;
                case TriggerType.PollingSingle:
                    batchMode = Constants.SINGLE;
                    break;
                case TriggerType.Subscription:
                    operation.SetCallbackType(schemaRegistry, triggerDescription.DataType, triggerDescription.DataFriendlyName);
                    break;
                default:
                    break;
            }

            if (null == batchMode) return;

            if (!operation.vendorExtensions.ContainsKey(Constants.X_MS_TRIGGER))
            {
                operation.vendorExtensions.Add(Constants.X_MS_TRIGGER,
                    batchMode.ToString().ToLowerInvariant());
            }
            
            var dataResponse = new Response()
            {
                description = triggerDescription.DataFriendlyName,
                schema = null != triggerDescription.DataType
                            ? schemaRegistry.GetOrRegister(triggerDescription.DataType)
                            : null
            };

            var acceptedResponse = new Response()
            {
                description = Constants.ACCEPTED
            };

            operation.responses[Constants.HAPPY_POLL_WITH_DATA_RESPONSE_CODE] = dataResponse;
            operation.responses[Constants.HAPPY_POLL_NO_DATA_RESPONSE_CODE] = acceptedResponse;

        }

        public static void SetCallbackType(this Operation operation, SchemaRegistry schemaRegistry, Type callbackType, string description)
        {
            operation.EnsureVendorExtensions();

            if (!operation.vendorExtensions.ContainsKey(Constants.X_MS_NOTIFICATION_CONTENT))
            {
                var schemaInfo = schemaRegistry.GetOrRegister(callbackType);

                var notificationData = new NotificationContentModel()
                {
                    Description = description,
                    Schema = new SchemaModel(schemaInfo)
                };

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
           
           if (!friendlyName.Contains(" ")) return friendlyName;

           return CultureInfo.CurrentCulture.TextInfo
                                            .ToTitleCase(friendlyName)
                                            .Replace(" ", string.Empty);
        }
    }
}
