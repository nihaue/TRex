using Microsoft.Azure.AppService.ApiApps.Service;
using QuickLearn.ApiApps.Metadata.Extensions;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Description;
using TRex.Metadata;

namespace QuickLearn.ApiApps.Metadata
{
    public class QuickLearnLogicAppMetadataFilter : IOperationFilter, ISchemaFilter
    {

        public QuickLearnLogicAppMetadataFilter()
        {

        }

        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (schema.properties == null) return;

            bool isPushTrigger = type.AssemblyQualifiedNameNoTypeParams() == typeof(TriggerInput<string, string>).AssemblyQualifiedNameNoTypeParams();

            foreach (var propertyName in schema.properties.Keys)
            {
                var property = schema.properties[propertyName];

                if (isPushTrigger && propertyName == Constants.CALLBACK_URL_PROPERTY_NAME)
                {
                    #region Apply callback magic defaults

                    // Apply trigger magic defaults:
                    // "x-ms-scheduler-recommendation": "@accessKeys('default').primary.secretRunUri
                    schema.SetChildPropertyRequired(Constants.CALLBACK_URL_PROPERTY_NAME);

                    property.SetVisibility(VisibilityTypes.Internal);
                    property.SetSchedulerRecommendation(Constants.CALLBACK_URL_MAGIC_DEFAULT);

                    // This is what this will look like (pulled from HTTP Listener API Definition)
                    //
                    // "TriggerInput[TriggerPushParameters,TriggerOutputParameters]": {
                    //     "required": [            <-- SetChildPropertyRequired (on the parent model containing the callbackUrl property)
                    //       "callbackUrl"
                    // ],
                    // "type": "object",
                    // "properties": {
                    //   "callbackUrl": {            <-- SetSchedulerRecommendation (on the actual property)
                    //     "type": "string",
                    //     "x-ms-visibility": "internal",
                    //     "x-ms-scheduler-recommendation": "@accessKeys('default').primary.secretRunUri"
                    //   },

                    #endregion
                }

                // Apply friendly names and descriptions wherever possible
                // "x-ms-summary" - friendly name (applies to properties)
                // schema.properties["prop"].description - description (applies to parameters)

                var propertyInfo = type.GetRuntimeProperties().Where(p => p.Name == propertyName).FirstOrDefault();

                if (propertyInfo == null) return;

                var propertyMetadata = propertyInfo.GetCustomAttribute<MetadataAttribute>();

                if (propertyMetadata != null)
                {
                    property.SetVisibility(propertyMetadata.Visibility);
                    property.SetFriendlyNameAndDescription(propertyMetadata.FriendlyName, propertyMetadata.Description);
                }

            }
        }

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {

            if (operation == null) return;

            applyTriggerInfo(operation, apiDescription, schemaRegistry);

            applyUnregisterCallbackInfo(operation, apiDescription);

            applyDefaultResponse(operation);

            applyOperationMetadataAndVisibility(operation, apiDescription);

        }

        /// <summary>
        /// Applies appropriate metadata for operation responsible for unregistering callbacks
        /// </summary>
        /// <param name="operation">Callback unregistration operation</param>
        /// <param name="apiDescription">Implementation metadata</param>
        private static void applyUnregisterCallbackInfo(Operation operation, ApiDescription apiDescription)
        {
            var operationUnregisterCallbackInfoResult = apiDescription.ActionDescriptor.GetCustomAttributes<UnregisterCallbackAttribute>();
            var operationUnregisterCallbackInfo = operationUnregisterCallbackInfoResult == null ? null : operationUnregisterCallbackInfoResult.FirstOrDefault();

            if (operationUnregisterCallbackInfo != null)
            {
                operation.SetFriendlyNameAndDescription("Unregister Callback", "Unregisters the callback from being invoked when the event is triggered");
                applyTriggerParameterMetadata(operation, Constants.TRIGGER_ID_PARAM_NAME, Constants.TRIGGER_ID_PARAM_FRIENDLY_NAME, Constants.TRIGGER_ID_MAGIC_DEFAULT);
                operation.SetVisibility(VisibilityTypes.Internal);
            }
        }

        /// <summary>
        /// Applies appropriate metadata for trigger related operations
        /// </summary>
        /// <param name="operation">Polling or callback registration operation</param>
        /// <param name="apiDescription">Implementation metadata</param>
        /// <param name="schemaRegistry">Current registry of schemas used in the metadata</param>
        private static void applyTriggerInfo(Operation operation, ApiDescription apiDescription, SchemaRegistry schemaRegistry)
        {



            var operationTriggerInfoResult = apiDescription.ActionDescriptor.GetCustomAttributes<TriggerAttribute>();
            var operationTriggerInfo = operationTriggerInfoResult == null ? null : operationTriggerInfoResult.FirstOrDefault();

            if (operationTriggerInfo == null) return;

            if (operation.vendorExtensions == null) operation.vendorExtensions = new Dictionary<string, object>();

            // Apply trigger type
            //      "x-ms-scheduler-trigger": "push"
            //      "x-ms-scheduler-trigger": "poll"
            if (!operation.vendorExtensions.ContainsKey(Constants.X_MS_SCHEDULER_TRIGGER))
                operation.vendorExtensions.Add(Constants.X_MS_SCHEDULER_TRIGGER, operationTriggerInfo.TriggerType.ToString().ToLower());

            if (operationTriggerInfo.TriggerType == TriggerTypes.Poll)
            {
                if (operationTriggerInfo.ResponseType != null)
                    applyResponseSchemas(operation, operationTriggerInfo.ResponseType, schemaRegistry);

                // "x-ms-summary": "Trigger State"
                // "x-ms-visibility": "internal"
                // "x-ms-scheduler-recommendation": "@coalesce(triggers()?.outputs?.body?['triggerState'], '')"
                applyTriggerParameterMetadata(operation, Constants.TRIGGER_STATE_PARAM_NAME, Constants.TRIGGER_STATE_PARAM_FRIENDLY_NAME, Constants.TRIGGER_STATE_MAGIC_DEFAULT);
            }
            else
            {
                // "x-ms-summary": "Trigger ID"
                // "x-ms-visibility": "internal"
                // "x-ms-scheduler-recommendation": "@workflow().name"
                applyTriggerParameterMetadata(operation, Constants.TRIGGER_ID_PARAM_NAME, Constants.TRIGGER_ID_PARAM_FRIENDLY_NAME, Constants.TRIGGER_ID_MAGIC_DEFAULT);
            }
        }

        /// <summary>
        /// Applies an appropriate friendly name, description, visibility setting, and default for the parameters of the callback registration operation or polling operation of a push or poll trigger respectively
        /// </summary>
        /// <param name="operation">The callback registration operation of a push trigger or the polling operation of a polling trigger</param>
        /// <param name="paramName">The name of the special parameter for that operation type</param>
        /// <param name="friendlyName">The friendly name and description to apply for the parameter</param>
        /// <param name="magicDefault">The magic default for the parameter</param>
        private static void applyTriggerParameterMetadata(Operation operation, string paramName, string friendlyName, string magicDefault)
        {

            if (operation.parameters == null) return;

            var triggerParam = operation.parameters.Where(p => p.name == paramName).FirstOrDefault();

            if (triggerParam == null) return;

            triggerParam.SetFriendlyNameAndDescription(friendlyName, friendlyName);
            triggerParam.SetVisibility(VisibilityTypes.Internal);
            triggerParam.SetSchedulerRecommendation(magicDefault);

        }

        /// <summary>
        /// Applies the friendly names, descriptions, and visibility settings to the operation
        /// </summary>
        /// <param name="operation">Exposed operation metadata</param>
        /// <param name="apiDescription">Implementation metadata</param>
        private static void applyOperationMetadataAndVisibility(Operation operation, ApiDescription apiDescription)
        {

            // Apply friendly names and descriptions where possible
            //      operation.summary - friendly name (applies to methods)
            //      operation.description - description (applies to methods)
            //      "x-ms-summary" - friendly name (applies to parameters and their properties)
            //      operation.parameters[x].description - description (applies to parameters)

            var operationMetadataResults = apiDescription.ActionDescriptor.GetCustomAttributes<MetadataAttribute>();
            var operationMetadata = operationMetadataResults == null ? null : operationMetadataResults.FirstOrDefault();

            if (operationMetadata != null)
            {
                operation.SetFriendlyNameAndDescription(operationMetadata.FriendlyName, operationMetadata.Description);
                operation.SetVisibility(operationMetadata.Visibility);
            }

            if (operation.parameters == null) return;

            // Ensure that we get the parameters of the operation all annotated appropriately as well
            applyOperationParameterMetadataAndVisibility(operation, apiDescription);

        }

        /// <summary>
        /// Applies the friendly names, descriptions, and visibility settings to all operation parameters
        /// </summary>
        /// <param name="operation">Exposed operation metadata</param>
        /// <param name="apiDescription">Implementation metadata</param>
        private static void applyOperationParameterMetadataAndVisibility(Operation operation, ApiDescription apiDescription)
        {
            var operationParameters = apiDescription.ActionDescriptor.GetParameters();

            if (operationParameters != null)
            {
                foreach (var parameter in apiDescription.ActionDescriptor.GetParameters())
                {
                    var parameterMetadataResults = parameter.GetCustomAttributes<MetadataAttribute>();
                    var parameterMetadata = parameterMetadataResults == null ? null : parameterMetadataResults.FirstOrDefault();

                    var operationParam = operation.parameters.FirstOrDefault(p => p.name == parameter.ParameterName);

                    if (operationParam != null)
                    {
                        if (parameterMetadata != null)
                        {
                            operationParam.SetFriendlyNameAndDescription(parameterMetadata.FriendlyName, parameterMetadata.Description);
                            operationParam.SetVisibility(parameterMetadata.Visibility);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ensures that the 202 response message schema is cleared out
        /// </summary>
        /// <param name="operation">Metadata for the polling operation of a polling trigger</param>
        /// <param name="pollingResponseType">Type of the polling response</param>
        /// <param name="schemaRegistry">Current registry of schemas used in the metadata</param>
        private static void applyResponseSchemas(Operation operation, Type pollingResponseType, SchemaRegistry schemaRegistry)
        {

            if (operation.responses == null) operation.responses = new Dictionary<string, Response>();

            if (!operation.responses.ContainsKey(Constants.HAPPY_POLL_NO_DATA_RESPONSE_CODE))
            {
                operation.responses.Add(Constants.HAPPY_POLL_NO_DATA_RESPONSE_CODE, new Response()
                {
                    description = "Successful poll, but no data available"
                });
            }

            operation.responses[Constants.HAPPY_POLL_NO_DATA_RESPONSE_CODE].schema = null;
            
            if (!operation.responses.ContainsKey(Constants.HAPPY_POLL_WITH_DATA_RESPONSE_CODE))
            {
                operation.responses.Add(Constants.HAPPY_POLL_WITH_DATA_RESPONSE_CODE, new Response()
                {
                    description = "Successful poll with data available"
                });
            }

            operation.responses[Constants.HAPPY_POLL_WITH_DATA_RESPONSE_CODE].schema
                = pollingResponseType == null ? null : schemaRegistry.GetOrRegister(pollingResponseType);

        }

        /// <summary>
        /// Ensures that each operation has a "default" response with a 200-level response code
        /// </summary>
        /// <param name="operation">Metadata for the operation</param>
        private static void applyDefaultResponse(Operation operation)
        {
            if (!operation.responses.ContainsKey(Constants.DEFAULT_RESPONSE_KEY))
            {
                var successCode = (from statusCode in operation.responses.Keys
                                   where statusCode.StartsWith("2")
                                   select statusCode).FirstOrDefault();

                if (successCode != null)
                {
                    operation.responses.Add(Constants.DEFAULT_RESPONSE_KEY, operation.responses[successCode]);
                }
            }
        }

    }


}
